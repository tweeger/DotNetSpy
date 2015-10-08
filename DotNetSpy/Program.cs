using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace DotNetSpy
{
    static class Program
    {
        public static readonly string TmpDirName = Path.GetTempPath() + @"DotNetSpyTemp\";
        public static readonly string[] TmpFiles = new string[] 
        { 
            @"DotNetSpy.RefDlls.DotNetSpyLib.dll",
            @"DotNetSpy.RefDlls.DotNetSpyLib64.dll",
            @"DotNetSpy.RefDlls.DotNetSpyProxy32.exe",
            @"DotNetSpyProxy32.RefDlls.DotNetSpyLib.dll"
        };

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                if (Directory.Exists(TmpDirName))
                { DeleteTmpFiles(); }
                else
                { Directory.CreateDirectory(TmpDirName); }

                NativeMethods.CreateDotNetProcess32FileMap();
                NativeMethods.CreateDotNetSpyEventWaitHandle();
            }
            catch { }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            Application.Run(new DotNetSpyForm());
        }

        static void OnApplicationExit(object sender, EventArgs e)
        {
            try
            {
                NativeMethods.CloseDotNetSpyProxy32Process();
                DeleteTmpFiles();

                NativeMethods.CloseDotNetProcess32FileMap();
                NativeMethods.CloseDotNetSpyEventWaitHandle();
            }
            catch { }
        }
        static void DeleteTmpFiles()
        {
            try
            {
                foreach (string file in TmpFiles)
                {
                    File.Delete(string.Format(@"{0}{1}", TmpDirName, file));
                }
            }
            catch { }
        }
    }
}
