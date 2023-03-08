using PLADCore.GameSystem.GameWindow;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLADCore.GameSystem.GameWindowSys
{
    public class RunningMachineSettingWindowSys
    {
        public static     RunningMachineSettingWindowSys Instance;
        private static int machine = 0;
        private static string protNames = string.Empty;
        public void Awake()
        {
            Instance = this;
        }
        RunningMachineSettingWindow RunningMachineSettingWindow = null;
        public bool ShowRunningMachineSettingWindow()
        {
            RunningMachineSettingWindow = new RunningMachineSettingWindow();
           var dis = RunningMachineSettingWindow.ShowDialog();
           if (dis == DialogResult.OK)
           {
               return true;
           }
           else
           {
               return false;
           }
        }

        public void SaveData(UIComboBox uiComboBox2, UIComboBox uiComboBox1, ref int machineCount, ref string portName)
        {
            int.TryParse(uiComboBox1.Text, out machineCount);
            if(machineCount == 0) { machineCount = 5; }
            
             portName= uiComboBox2.Text;
             machine = machineCount;
             protNames = portName;


        }

        public int GetMachineCount()
        {
            return machine;
        }

        public string GetPortName()
        {
            return protNames;
        }
    }
}
