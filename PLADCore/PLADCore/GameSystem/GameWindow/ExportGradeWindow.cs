using HZH_Controls;
using HZH_Controls.Forms;
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
    public partial class ExportGradeWindow : Form
    {

        public ExportGradeWindow()
        {
            InitializeComponent();
        }
        private bool isAllTest = false;
        private bool isOnlyGroup = false;
        public string groupName = string.Empty;
        private  SportProjectInfos SportProjectInfos = null;


        private void ExportGradeWindow_Load(object sender, EventArgs e)
        {
            uiCheckBox1.Checked= true;
            if(string.IsNullOrEmpty(groupName))
            {
                uiCheckBox1.Checked = false;
                uiCheckBox2 .Visible= false;
                uiTextBox1.Text = "未选择组别";

            }
            else
            {
                uiCheckBox2.Checked = true;
                uiCheckBox2.Visible = true;
                uiTextBox1.Text = groupName;
            }
            SportProjectInfos = ExportGradeWindowSys.Instance.LoadingInitData();
        }

        private void uiCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(uiCheckBox2.Checked)
            {
                uiButton1.Text = "导出当前组";

            }
            else
            {
                uiButton1.Text = "导出全部成绩";

            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            isAllTest = uiCheckBox1 .Checked;
            isOnlyGroup = uiCheckBox2.Checked;
            uiButton1.Enabled = false;
            bool result = ExportGradeWindowSys.Instance.OutPutScore(SportProjectInfos,isOnlyGroup,groupName,isAllTest);
            if(result)
            {
                UIMessageBox.Show("导出成功");
                uiButton1.Enabled = true;
            }
        }

        private void uiCheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
