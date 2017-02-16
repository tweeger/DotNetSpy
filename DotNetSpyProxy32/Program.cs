using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using DotNetSpyProxy32.IO.FileMap;

namespace DotNetSpyProxy32
{
    static class Program
    {
        private enum ComAction
        {
            None = 0,
            Check,
            Hook,
            Close
        }
        private const long FileMapMaxSize = Int32.MaxValue;
        private static EventWaitHandle DotNetSpyEventWaitHandle = null;
        private static MemoryMappedFile DotNetProcess32FileMap = null;

        public static readonly string TmpDirName = Path.GetTempPath() + @"DotNetSpyTemp\";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                DotNetSpyEventWaitHandle = EventWaitHandle.OpenExisting(@"DotNetSpyEventWaitHandle");
                DotNetProcess32FileMap = MemoryMappedFile.Open(MapAccess.FileMapAllAccess, @"DotNetProcess32FileMap");
            }
            catch { return; }

            ThreadPool.QueueUserWorkItem(ActionThreadProc);
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SpyProxy32());
        }

        #region MemFileMapping

        private static void ActionThreadProc(object state)
        {
            string strMapData = string.Empty;
            while (DotNetSpyEventWaitHandle.WaitOne())
            {
                try
                {
                    string tmp = GetDataFromFileMap();
                    if (!string.IsNullOrEmpty(tmp) && !string.Equals(tmp, strMapData))
                    {
                        strMapData = tmp;
                        string[] strData = tmp.Split('-');
                        ComAction action = (ComAction)Enum.Parse(typeof(ComAction), strData[0]);
                        switch (action)
                        {
                            case ComAction.Check:
                                ProcessCheckAction(int.Parse(strData[1]));
                                break;
                            case ComAction.Hook:
                                ProcessHookAction(new IntPtr(int.Parse(strData[1])), new IntPtr(int.Parse(strData[2])));
                                break;
                            case ComAction.Close:
                                ProcessCloseAction();
                                return;
                        }
                    }
                }
                catch { }

                DotNetSpyEventWaitHandle.Set();
                Thread.Sleep(10);
            }
        }

        #endregion

        #region Help Methods
        private static string GetDataFromFileMap()
        {
            string strData = string.Empty;

            try
            {
                Int32 length = 0;
                int offset = Marshal.SizeOf(length);
                IntPtr hData = DotNetProcess32FileMap.MapView(MapAccess.FileMapAllAccess, 0, offset);
                length = Marshal.ReadInt32(hData);
                DotNetProcess32FileMap.UnMapView(hData);

                hData = DotNetProcess32FileMap.MapView(MapAccess.FileMapAllAccess, 0, length + offset);
                byte[] data = new byte[length + offset];
                Marshal.Copy(hData, data, 0, data.Length);
                strData = Encoding.ASCII.GetString(data, offset, length);
                DotNetProcess32FileMap.UnMapView(hData);
            }
            catch { }

            return strData;
        }
        private static void ProcessCheckAction(int targetProcessId)
        {
            byte val = Convert.ToByte(NativeMethods.IsDotNetProcess(targetProcessId));
            IntPtr hData = DotNetProcess32FileMap.MapView(MapAccess.FileMapWrite, 0, 1);
            Marshal.WriteByte(hData, val);
            DotNetProcess32FileMap.UnMapView(hData);
        }
        private static void ProcessHookAction(IntPtr hWndTarget, IntPtr hWndSource)
        {
            Hook(hWndTarget, hWndSource);
        }
        private static void ProcessCloseAction()
        {
            try
            {
                DotNetProcess32FileMap.Close();
                DotNetProcess32FileMap = null;
                DotNetSpyEventWaitHandle.Close();
                DotNetSpyEventWaitHandle = null;
                File.Delete(string.Format(@"{0}{1}", TmpDirName, @"DotNetSpyProxy32.RefDlls.DotNetSpyLib.dll"));
            }
            catch { }
            finally { Application.Exit(); }
        }
        #endregion

        #region Hook
        public static bool Hook(IntPtr hWndTarget, IntPtr hWndSource)
        {
            try
            {
                if (hWndTarget != IntPtr.Zero)
                {
                    uint processId;
                    int threadId = (int)NativeMethods.GetWindowThreadProcessId(hWndTarget, out processId);

                    int nhWndPanel = hWndSource.ToInt32();
                    int nhWndTarget = hWndTarget.ToInt32();

                    byte[] aryPanelData = BitConverter.GetBytes(nhWndPanel);
                    byte[] aryTargetData = BitConverter.GetBytes(nhWndTarget);

                    byte[] data = new byte[aryPanelData.Length + aryTargetData.Length];
                    Array.Copy(aryPanelData, data, aryPanelData.Length);
                    Array.Copy(aryTargetData, 0, data, aryPanelData.Length, aryTargetData.Length);

                    // Pickup an idle message from the queue
                    Type type = typeof(WindowPropertiesView);
                    InstallHook((int)processId, threadId, type.Assembly.Location, type.FullName, data);

                    // send an idle ;;)
                    NativeMethods.SendMessage(hWndTarget, 0, IntPtr.Zero, IntPtr.Zero);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        private static void InstallHook(int processID, int threadID, string assemblyLocation, string typeName, byte[] data)
        {
            try
            {
                string dllName = @"DotNetSpyProxy32.RefDlls.DotNetSpyLib.dll";
                Assembly assembly = LoadAssembly(dllName);
                if (assembly != null)
                {
                    MethodInfo method = assembly.GetType(@"DotNetSpyLib.HookHelper").GetMethod(@"InstallIdleHandler", BindingFlags.Public | BindingFlags.Static);
                    method.Invoke(null, new object[] { processID, threadID, assemblyLocation, typeName, data });
                }
            }
            catch { }
        }
        private static Assembly LoadAssembly(string name)
        {
            Assembly assLoaded = null;
            string path = string.Format(@"{0}{1}", TmpDirName, name);
            if (!File.Exists(path))
            {
                using (FileStream fWriter = File.Create(path))
                {
                    Assembly assembly = Assembly.GetEntryAssembly();
                    using (Stream fs = assembly.GetManifestResourceStream(name))
                    {
                        byte[] buffer = new byte[Convert.ToInt32(fs.Length)];
                        fs.Read(buffer, 0, buffer.Length);
                        fWriter.Write(buffer, 0, buffer.Length);
                        fWriter.Flush();
                    }
                }
            }

            if (File.Exists(path))
            {
                assLoaded = Assembly.LoadFrom(path);
            }

            return assLoaded;
        }
        #endregion
    }
}
