using PLADCore.GameSystem.GameWindowSys;
using PLADCoreDataModel.GameModel;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLADCore.GameSystem.GameWindow
{
    public partial class SystemSettingWindow : Form
    {
        public SystemSettingWindow()
        {
            InitializeComponent();
        }
        SportProjectInfos sportProjectInfos = null;
        private void SystemSettingWindow_Load(object sender, EventArgs e)
        {
            SystemSettingWindowSys.Instance.LoadingInitData(uiTextBox1, uiComboBox1, uiComboBox2, uiComboBox3, uiComboBox4, ref sportProjectInfos);
        }

        

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = uiComboBox1.SelectedIndex;
            if (index < 0) return;
            else
                SystemSettingWindowSys.Instance.UpdataRoundCount(index);
        }
        private void uiComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = uiComboBox2.SelectedIndex;
            if (index < 0) return;
            else
                SystemSettingWindowSys.Instance.UpdataBestScoreMode(index);

        }

        private void uiComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index =uiComboBox3.SelectedIndex;
            if (index < 0) return;
            else
                SystemSettingWindowSys.Instance.UpDataTestMethod(index);
        }

        private void uiComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = uiComboBox4.SelectedIndex;
            if (index < 0) return;
            else
                SystemSettingWindowSys.Instance.UpdataFloatType(index);
        }

        

        private void uiButton1_Click(object sender, EventArgs e)
        {
            string name = uiTextBox1.Text;
            if (!string.IsNullOrEmpty(name))
            {
                SystemSettingWindowSys.Instance.SetFrmClosedData(name);
                DialogResult= DialogResult.OK;
                this.Close();
            }
            else
            {

                UIMessageBox.ShowInfo("数据异常！！");
                return;
            }
        }
    }
}
