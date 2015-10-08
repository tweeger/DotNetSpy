using System;
using System.Management;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace DotNetSpy
{
    public interface IHookInstall
    {
        void OnInstallHook(byte[] data);
    }

    internal class SypHookHelper
    {
        public static bool Hook(IntPtr hWndTarget, IntPtr hWndSource)
        {
            try
            {
                if (hWndTarget != IntPtr.Zero && NativeMethods.IsDotNetWindow(hWndTarget))
                {
                    uint processId;
                    int threadId = (int)NativeMethods.GetWindowThreadProcessId(hWndTarget, out processId);
                    if (IntPtr.Size == 8 && NativeMethods.GetProcessBitWidth((int)processId) == NativeMethods.BitWidth.BIT32)
                    {
                        NativeMethods.InstallHookWithFileMap(hWndTarget, hWndSource);
                    }
                    else
                    {
                        InstallHook(hWndTarget, hWndSource);
                    }
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

        private static void InstallHook(IntPtr hWndTarget, IntPtr hWndSource)
        {
            try
            {
                string name = IntPtr.Size == 4 ? Program.TmpFiles[0] : Program.TmpFiles[1];
                Assembly assembly = LoadAssembly(name);
                if (assembly != null)
                {
                    MethodInfo method = assembly.GetType(@"DotNetSpyLib.HookHelper").GetMethod(@"InstallIdleHandler", BindingFlags.Public | BindingFlags.Static);
                    
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
                    method.Invoke(null, new object[] { (int)processId, threadId, type.Assembly.Location, type.FullName, data });
                }

                // send an idle ;;)
                NativeMethods.SendMessage(hWndTarget, 0, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception exp) { MessageBox.Show(exp.Message); }
        }

        private static bool ReleaseAssembly(string name)
        {
            string path = string.Format(@"{0}{1}", Program.TmpDirName, name);
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

            return File.Exists(path);
        }
        private static Assembly LoadAssembly(string name)
        {
            Assembly assLoaded = null;
            string path = string.Format(@"{0}{1}", Program.TmpDirName, name);
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
    }
}
