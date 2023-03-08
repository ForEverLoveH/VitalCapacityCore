using HZH_Controls.Controls;
using PLADCore.GameSystem.GameModel;
using PLADCore.GameSystem.GameWindowSys;
using PLADCoreDataModel.GameModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Sunny.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PLADCore.GameSystem.GameWindow
{
    public partial class ChooseRoundWindow : HZH_Controls.Forms.FrmWithOKCancel2
    {
        public ChooseRoundWindow()
        {
            InitializeComponent();
        }
        //mode 0 �ִ���� 
        //mode 1 �ִ��޸ĳɼ�
        public int mode = -1;
        public IFreeSql fsql = DB.Sqlite;
        public SportProjectInfos sportProjectInfos = null;
        public DbPersonInfos dbPersonInfos = null;
        public string _idnumber = string.Empty;
        private void frmChooseRound_Load(object sender, EventArgs e)
        {
            if (mode == 0) this.Title = "�ִ����";
            else if (mode == 1) this.Title = "�����ɼ�";
            if (!string.IsNullOrEmpty(_idnumber))
            {
                dbPersonInfos = fsql.Select<DbPersonInfos>().Where(a => a.IdNumber == _idnumber).ToOne();
            }
            sportProjectInfos = fsql.Select<SportProjectInfos>().ToOne();
            if (sportProjectInfos != null)
            {
                int roundTotal = sportProjectInfos.RoundCount;
                for (int i = 0; i < roundTotal; i++)
                {
                    comboBox1.Items.Add($"��{i + 1}��");
                }
                if (roundTotal > 0)
                    comboBox1.SelectedIndex = 0;
            }
            if (dbPersonInfos != null)
            {
                textBox1.Text = dbPersonInfos.IdNumber;
                textBox2.Text = dbPersonInfos.Name;
            }
        }
        bool isNoExam = false;
        private void frmChooseRound_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (base.DialogResult == DialogResult.OK)
            {
                int roundid = comboBox1.SelectedIndex + 1;
                if (mode == 0)
                {
                    int result = fsql.Delete<ResultInfos>()
                       .Where(a => a.PersonIdNumber == _idnumber)
                       .Where(a => a.RoundId == roundid)
                       .ExecuteAffrows();
                    if (result == 1) UIMessageBox.ShowSuccess("ɾ���ɹ�");
                }
                else if (mode == 1)
                {
                    double.TryParse(textBox3.Text, out double fhl);

                    if (isNoExam)
                    {
                        List<ResultInfos> insertResults = new List<ResultInfos>();
                        fsql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
                        maxSortId++;
                        ResultInfos rinfo = new ResultInfos();
                        rinfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        rinfo.SortId = maxSortId;
                        rinfo.PersonId = dbPersonInfos.Id.ToString();
                        rinfo.SportItemType = 0;
                        rinfo.PersonName = dbPersonInfos.Name;
                        rinfo.PersonIdNumber = dbPersonInfos.IdNumber;
                        rinfo.RoundId = roundid;
                        rinfo.State = 1;
                        rinfo.IsRemoved = 0;
                        rinfo.Result = fhl;
                        insertResults.Add(rinfo);

                        int result = fsql.InsertOrUpdate<ResultInfos>()
                                           .SetSource(insertResults)
                                           .IfExistsDoNothing()
                                           .ExecuteAffrows();
                        if (result == 1) UIMessageBox.ShowSuccess("�޸ĳɹ�");
                    }
                    else
                    {

                        int result = fsql.Update<ResultInfos>()
                           .Set(a => a.Result == fhl)
                           .Where(a => a.PersonIdNumber == _idnumber)
                           .Where(a => a.RoundId == roundid)
                           .Where(a => a.IsRemoved == 0)
                           .ExecuteAffrows();

                        if (result == 1) UIMessageBox.ShowSuccess("�޸ĳɹ�");
                    }

                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int roundid = comboBox1.SelectedIndex + 1;
            if (roundid <= 0) return;
            List<ResultInfos> resultInfos = fsql.Select<ResultInfos>()
               .Where(a => a.PersonIdNumber == _idnumber)
               .Where(a => a.RoundId == roundid)
               .Where(a => a.IsRemoved == 0)
               .ToList();
            isNoExam = false;
            textBox3.Text = "";
            if (resultInfos.Count == 0)
            {
                isNoExam = true;
                MessageBox.Show("��ѧ������δ�μӿ���");
                return;
            }
            foreach (var ri in resultInfos)
            {
                if (ri.IsRemoved == 0)
                {
                    textBox3.Text = ri.Result.ToString();
                }
            }
        }
    }

}
         