using HZH_Controls.Forms;
using MiniExcelLibs;
using PLADCoreDataModel.GameModel;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLADCore.GameSystem.GameWindow
{
    public partial class PersonWindow : Form
    {
        public PersonWindow()
        {
            InitializeComponent();
        }
        Dictionary<String, String> localValue = null;
        static  bool isImport = false;
        private void PersonWindow_Load(object sender, EventArgs e)
        {
            PersonWindowSys.Instance.InitListViewHeader(listView1);
        }
        string paths= string.Empty;
        /// <summary>
        /// 平台设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            PersonWindowSys.Instance.ShowPlatFormWindow();
        }
        /// <summary>
        /// 拉取名单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton2_Click(object sender, EventArgs e)
        {
            string name = uiTextBox1.Text.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                HZH_Controls.ControlHelper.ThreadInvokerControl(this, () =>
                {
                    
                     PersonWindowSys.Instance.LoadingServerData(name, ref localValue, listView1,uiLabel5) ;
                  
                });
                
                
            }
            else
            {
                UIMessageBox.ShowWarning("请先输入你需要拉取的数目");
                return;
            }
        }
        /// <summary>
        /// 浏览本地
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton3_Click(object sender, EventArgs e)
        {
            PersonWindowSys.Instance.OpenLocalExcelFile(uiTextBox2);
            if (!string.IsNullOrEmpty(uiTextBox2.Text))
            {
                string path = uiTextBox2.Text;
                bool t_flag = true;
                string errorMsg = string.Empty;
                try
                {
                    var rows = MiniExcel.Query<InputData>(path).ToList();
                    listView1.BeginUpdate();
                    listView1.Items.Clear();
                    int step = 1;
                    foreach (var row in rows)
                    {
                        ListViewItem li = new ListViewItem();
                        li.Text = step.ToString();
                        li.SubItems.Add(row.examTime);
                        li.SubItems.Add(row.School);
                        li.SubItems.Add(row.GradeName);
                        li.SubItems.Add(row.ClassName);
                        li.SubItems.Add(row.IdNumber);
                        li.SubItems.Add(row.Name);
                        li.SubItems.Add(row.Sex);
                        li.SubItems.Add(row.GroupName);
                        listView1.Items.Insert(listView1.Items.Count, li);
                        step++;
                    }
                    listView1.EndUpdate();
                    if (t_flag)
                    {
                        paths = path;
                        MessageBox.Show("读取成功");
                    }
                    else
                    {
                        listView1.Items.Clear();
                        paths = string.Empty;
                        UIMessageBox.ShowError($"读取失败,错误({errorMsg})");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    t_flag = false;
                    LoggerHelper.Debug(ex);
                    return;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton4_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog  = new SaveFileDialog ();
            saveFileDialog.Filter= "xls files(*.xls)|*.xls|xlsx file(*.xlsx)|*.xlsx|All files(*.*)|*.*";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = $"导入模板{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            string path = Application.StartupPath + "\\excel\\output.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = saveFileDialog.FileName;
                File.Copy(@"./模板/导入名单模板.xlsx", path);
                FrmTips.ShowTipsSuccess(this, "导出成功");
            }
            else
            {
                UIMessageBox.ShowError("导出失败！！");
                return;
            }
        }
        /// <summary>
        /// 全新导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton7_Click(object sender, EventArgs e)
        {
            isDeleteBeforeImport = true;
            if (string.IsNullOrEmpty(paths))
            {
                UIMessageBox.ShowError("路径错误！！");
                return;

            }
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(ExcelInputDataBase);
            Thread thread = new Thread(parameterizedThreadStart);
            thread.IsBackground = true;
            thread.Start(paths);


        }
        bool isDeleteBeforeImport = false;
        /// <summary>
        /// 更新导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton6_Click(object sender, EventArgs e)
        {
            isDeleteBeforeImport = false;
            if (string.IsNullOrEmpty(paths))
            {
                UIMessageBox.ShowError("路径错误！！");
                return;

            }
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(ExcelInputDataBase);
            Thread  thread= new Thread(parameterizedThreadStart);
            thread.IsBackground= true;
            thread.Start(paths);
        }

        private void ExcelInputDataBase(object obj)
        {
            HZH_Controls.ControlHelper.ThreadInvokerControl(this, () =>
            {
                PersonWindowSys.Instance.ShowLoadingWindow();
                if(PersonWindowSys.Instance.ExcelInputDataBase(obj, isDeleteBeforeImport,uiLabel5))
                {
                    UIMessageBox.ShowSuccess("导入成功");
                    isImport= true;
                    PersonWindowSys.Instance.SetLoadingWindowClose();
                }
                else
                {
                    UIMessageBox.ShowError("导入失败");
                    PersonWindowSys.Instance.SetLoadingWindowClose();
                }
            });
            
        }
        

        private void PersonWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isImport)
            {
                PersonWindowSys.Instance.SetIsImport(true);
                DialogResult = DialogResult.OK;
            }
            else
            {
                PersonWindowSys.Instance.SetIsImport(false);
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}
