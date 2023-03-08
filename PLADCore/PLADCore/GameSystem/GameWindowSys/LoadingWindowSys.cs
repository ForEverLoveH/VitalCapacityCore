using PLADCore.GameSystem.GameWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLADCore.GameSystem.GameWindowSys
{
    internal class LoadingWindowSys
    {
        public static LoadingWindowSys Instance;
        LoadingWindow loadingWindow = null;
        public  void Awake()
        {
            Instance= this;
        }

        public  void ShowLoadingWindow()
        {
            loadingWindow = new LoadingWindow();
            loadingWindow.Show();
                
        }

        public  void SetLoadingWindowClose()
        {
            loadingWindow.Invoke((EventHandler)delegate { loadingWindow.Close(); });
            loadingWindow.Dispose();
        }
    }
}
