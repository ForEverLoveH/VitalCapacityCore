using PLADCore.GameSystem.GameWindow;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLADCore
{
    internal static class Program
    {

        public const int WM_SYSCOMMAND = 0x112;
        public const int SC_RESTORE = 0xF120;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GameRoot GameRoot = new GameRoot();
            GameRoot.StartGame();
            #region 判断进程只能启动一个实例
            Process cur = Process.GetCurrentProcess();
            foreach (Process p in Process.GetProcesses())
            {
                if (p.Id == cur.Id) continue;
                if (p.ProcessName == cur.ProcessName)
                {
                    GameRoot.SetForegroundWindow(p.MainWindowHandle);
                    GameRoot.SendMessage(p.MainWindowHandle, WM_SYSCOMMAND, SC_RESTORE, 0);
                    //p.Close();
                    return;
                }

                //Application.Run(new MainWindow());
            }
            #endregion
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GameRoot.WriteLog();
            Application.Run(new MainWindow());
        }
    }
}

         
