using HZH_Controls.Forms;
using PLADCore.GameSystem.GameWindowSys;
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
    public partial class PlatFormSettingWindow : Form
    {
        public PlatFormSettingWindow()
        {
            InitializeComponent();
        }
        string MachineCode = String.Empty;
        string ExamId = String.Empty;
        string Platform = String.Empty;
        string Platforms = String.Empty;
        public Dictionary<string, string> localValues = null ;
        private void PlatFormSettingWindow_Load(object sender, EventArgs e)
        {
            PlatFormSettingWindowSys.Instance.LoadingInitData(ref MachineCode, ref ExamId, ref Platforms, ref Platform, uiComboBox1, uiComboBox2, uiComboBox3, ref localValues) ;
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            uiComboBox3.Items.Clear();
            string url = uiComboBox2.Text;
            if(url == String.Empty)
            {
                UIMessageBox.ShowError("网址为空！！");
                return;
            }
            PlatFormSettingWindowSys.Instance.GetExamNum(uiComboBox3,url,localValues) ;
        }
        /// <summary>
        /// 获取机器码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton2_Click(object sender, EventArgs e)
        {
            uiComboBox1.Items.Clear();
            string examID = uiComboBox3.Text;
            if(string.IsNullOrEmpty(examID) )
            {
                FrmTips.ShowTipsError(this, "考试id为空!");
                return;
            }
            PlatFormSettingWindowSys.Instance.GetCode(examID, uiComboBox2, uiComboBox1, localValues);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton3_Click(object sender, EventArgs e)
        {
            if  (PlatFormSettingWindowSys.Instance.SaveData(uiComboBox1, uiComboBox2, uiComboBox3))
            {
                DialogResult dialogResult= DialogResult.OK;
                this.Close();
            }
            else
            {
                DialogResult dialog= DialogResult.Cancel;
                this.Close();
                 
            }
        }
    }
}
