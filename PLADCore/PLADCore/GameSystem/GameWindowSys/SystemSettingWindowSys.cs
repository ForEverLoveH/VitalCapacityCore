using PLADCore.GameSystem.GameModel;
using PLADCore.GameSystem.GameWindow;
using PLADCoreDataModel.GameModel;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PLADCore.GameSystem.GameWindowSys
{
    internal class SystemSettingWindowSys
    {
        public static SystemSettingWindowSys Instance;
        SystemSettingWindow SystemSettingWindow = null;
        IFreeSql freeSql = DB.Sqlite;
        public void Awake()
        {
            Instance = this; 
        }
        public  bool  ShowSystemSettingWindow()
        {
            SystemSettingWindow = new SystemSettingWindow();
            var s= SystemSettingWindow.ShowDialog();
            if(s==System.Windows.Forms.DialogResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public  void LoadingInitData(UITextBox uiTextBox1, UIComboBox uiComboBox1, UIComboBox uiComboBox2, UIComboBox uiComboBox3, UIComboBox uiComboBox4, ref PLADCoreDataModel.GameModel.SportProjectInfos sportProjectInfos)
        {
            sportProjectInfos = freeSql.Select<SportProjectInfos>().ToOne();
            if(sportProjectInfos != null)
            {
                uiTextBox1.Text = sportProjectInfos.Name;
                uiComboBox1.SelectedIndex = sportProjectInfos.RoundCount;
                uiComboBox2.SelectedIndex = sportProjectInfos.BestScoreMode;
                uiComboBox3.SelectedIndex = sportProjectInfos.TestMethod;
                uiComboBox4.SelectedIndex = sportProjectInfos.FloatType;
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void SetFrmClosedData(string name)
        {
            freeSql.Update<SportProjectInfos>().Set(a => a.Name == name).Where("1=1").ExecuteAffrows();
        }

        public void UpdataRoundCount(int index)
        {
            freeSql.Update<SportProjectInfos>().Set(a => a.RoundCount == index).Where("1=1").ExecuteAffrows();
        }

        public void UpdataBestScoreMode(int index)
        {
            freeSql.Update<SportProjectInfos>().Set(a => a.BestScoreMode == index).Where("1=1").ExecuteAffrows();
        }

        public void UpDataTestMethod(int index)
        {
            freeSql.Update<SportProjectInfos>().Set(a => a.TestMethod == index).Where("1=1").ExecuteAffrows();
        }

        public  void UpdataFloatType(int index)
        {
            freeSql.Update<SportProjectInfos>().Set(a => a.FloatType == index).Where("1=1").ExecuteAffrows();
        }
    }
}
