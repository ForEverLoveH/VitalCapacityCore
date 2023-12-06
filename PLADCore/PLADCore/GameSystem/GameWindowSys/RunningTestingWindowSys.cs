using PLADCore.GameSystem.GameModel;
using PLADCore.GameSystem.GameWindow;
using PLADCoreDataModel.GameModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniExcelLibs;
using Newtonsoft.Json;
using PLADCore.GameSystem.MyControl;
using Sunny.UI;
using Sunny.UI.Win32;

namespace PLADCore.GameSystem.GameWindowSys
{
    public class RunningTestingWindowSys
    {
        public static RunningTestingWindowSys Instance;
        private IFreeSql freeSql = DB.Sqlite;
        private RunningTestingWindow RunningTestingWindow = null;

        public void Awake()
        {
            Instance = this;
        }

        public void ShowRunningWindow(string createTime, string school)
        {
            RunningTestingWindow = new RunningTestingWindow();
            RunningTestingWindow.CreateTime = createTime;
            RunningTestingWindow.School = school;
            RunningTestingWindow.Show();
        }

        public SportProjectInfos LoadingSportData()
        {
            return freeSql.Select<SportProjectInfos>().ToOne();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userControl1"></param>
        /// <param name="_serialReaders"></param>
        /// <param name="connprot"></param>
        public void CloseAllSerial(List<UserControl1> userControl1, List<SerialReader> _serialReaders, List<string> connprot)
        {
            foreach (var items in _serialReaders)
            {
                if (items != null)
                {
                    items.CloseCom();
                }
            }
            _serialReaders.Clear();
            connprot.Clear();
            foreach (var item in userControl1)
            {
                item.p_toolState = "设备未连接   ";
                item.p_toolState_color = Color.Red;
                item.p_title_Color = System.Drawing.SystemColors.ControlLight;
            }
            GC.Collect();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool ShowRunMachineWindow()
        {
            return RunningMachineSettingWindowSys.Instance.ShowRunningMachineSettingWindow();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public int GetMachineCount()
        {
            return RunningMachineSettingWindowSys.Instance.GetMachineCount();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string GetPortName()
        {
            return RunningMachineSettingWindowSys.Instance.GetPortName();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<LocalInfos> GetLocalInfo()
        {
            return freeSql.Select<LocalInfos>().ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listView1"></param>
        /// <param name="roundCount"></param>
        public void InitListViewHeader(ListView listView1, int roundCount)
        {
            listView1.View = View.Details;
            ColumnHeader[] Header = new ColumnHeader[100];
            int sp = 0;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "序号";
            Header[sp].Width = 50;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "学校";
            Header[sp].Width = 200;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "组号";
            Header[sp].Width = 100;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "准考证号";
            Header[sp].Width = 160;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "姓名";
            Header[sp].Width = 130;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "最好成绩";
            Header[sp].Width = 120;
            sp++;
            for (int i = 1; i <= roundCount; i++)
            {
                Header[sp] = new ColumnHeader();
                Header[sp].Text = $"第{i}轮";
                Header[sp].Width = 120;
                sp++;

                Header[sp] = new ColumnHeader();
                Header[sp].Text = "上传状态";
                Header[sp].Width = 120;
                sp++;
            }

            ColumnHeader[] Header1 = new ColumnHeader[sp];
            listView1.Columns.Clear();
            for (int i = 0; i < Header1.Length; i++)
            {
                Header1[i] = Header[i];
            }
            listView1.Columns.AddRange(Header1);
        }

        public void UpDataGroup(string creatime, string school, ComboBox uiComboBox)
        {
            try
            {
                var ls = freeSql.Select<DbPersonInfos>().Where(a => a.CreateTime == school && a.SchoolName == creatime)
                    .ToList();
                if (ls.Count > 0)
                {
                    uiComboBox.Items.Clear();
                    foreach (var po in ls)
                    {
                        if (uiComboBox.Items.Contains(po.GroupName)) continue;
                        else
                        {
                            uiComboBox.Items.Add(po.GroupName);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LoggerHelper.Debug(e);
                uiComboBox.Items.Clear();
                uiComboBox.AutoCompleteCustomSource = null;
            }
        }

        private string AutoMatchLog = Application.StartupPath + "\\Data\\AutoMatchLog.log";
        private string AutoUploadLog = Application.StartupPath + "\\Data\\AutoUploadLog.log";
        private string AutoPrintLog = Application.StartupPath + "\\Data\\AutoPrintLog.log";

        public void SetRunningWindowCheck(CheckBox checkBox1, CheckBox checkBox2, CheckBox checkBox3)
        {
            try
            {
                string[] strg = File.ReadAllLines(AutoMatchLog);
                if (strg.Length > 0)
                {
                    if (strg[0] == "1")
                    {
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                    }
                }
                strg = File.ReadAllLines(AutoUploadLog);
                if (strg.Length > 0)
                {
                    if (strg[0] == "1")
                    {
                        checkBox2.Checked = true;
                    }
                    else
                    {
                        checkBox2.Checked = false;
                    }
                }

                strg = File.ReadAllLines(AutoPrintLog);
                if (strg.Length > 0)
                {
                    if (strg[0] == "1")
                    {
                        checkBox3.Checked = true;
                    }
                    else
                    {
                        checkBox3.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listView1"></param>
        /// <param name="groupsCbx"></param>
        /// <param name="sportProjectInfos"></param>
        /// <returns></returns>
        public List<DbPersonInfos> UpDataListView(ListView listView1, ComboBox groupsCbx, SportProjectInfos sportProjectInfos)
        {
            List<DbPersonInfos> dbPersonInfos = new List<DbPersonInfos>();
            try
            {
                listView1.Items.Clear();
                int index = groupsCbx.SelectedIndex;
                string groupName = groupsCbx.Text;
                dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == groupName).ToList();
                if (dbPersonInfos.Count == 0) return dbPersonInfos;
                int step = 1;
                listView1.BeginUpdate();
                Font f = new Font(Control.DefaultFont, FontStyle.Bold);
                bool isBestScore = sportProjectInfos.BestScoreMode == 0 ? true : false;
                foreach (var dbPersonInfo in dbPersonInfos)
                {
                    ListViewItem li = new ListViewItem();
                    li.UseItemStyleForSubItems = false;
                    li.Text = step.ToString();
                    li.SubItems.Add(dbPersonInfo.SchoolName);
                    li.SubItems.Add(dbPersonInfo.GroupName);
                    li.SubItems.Add(dbPersonInfo.IdNumber);
                    li.SubItems.Add(dbPersonInfo.Name);
                    li.SubItems.Add("未测试");
                    List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>()
                        .Where(a => a.PersonId == dbPersonInfo.Id.ToString() && a.IsRemoved == 0)
                        .OrderBy(a => a.Id)
                        .ToList();
                    int resultRound = 0;
                    double MaxScore = 99999;
                    if (isBestScore) MaxScore = 0;
                    bool getScore = false;
                    foreach (var resultInfo in resultInfos)
                    {
                        if (resultInfo.State != 1)
                        {
                            string s_rstate = ResultStateType.Match(resultInfo.State);
                            li.SubItems.Add(s_rstate);
                            li.SubItems[li.SubItems.Count - 1].ForeColor = Color.Red;
                        }
                        else
                        {
                            getScore = true;
                            li.SubItems.Add(resultInfo.Result.ToString());
                            li.SubItems[li.SubItems.Count - 1].BackColor = Color.MediumSpringGreen;
                            if (isBestScore)
                            {
                                //取最大值
                                if (MaxScore < resultInfo.Result) MaxScore = resultInfo.Result;
                            }
                            else
                            {
                                //取最小值
                                if (MaxScore > resultInfo.Result) MaxScore = resultInfo.Result;
                            }
                        }
                        li.SubItems[li.SubItems.Count - 1].Font = f;
                        if (resultInfo.uploadState == 0)
                        {
                            li.SubItems.Add("未上传");
                            li.SubItems[li.SubItems.Count - 1].ForeColor = Color.Red;
                        }
                        else if (resultInfo.uploadState == 1)
                        {
                            li.SubItems.Add("已上传");
                            li.SubItems[li.SubItems.Count - 1].ForeColor = Color.MediumSpringGreen;
                            li.SubItems[li.SubItems.Count - 1].Font = f;
                        }
                        resultRound++;
                    }
                    for (int i = resultRound; i < sportProjectInfos.RoundCount; i++)
                    {
                        li.SubItems.Add("未测试");
                        li.SubItems.Add("未上传");
                    }
                    if (getScore)
                    { li.SubItems[5].Text = MaxScore.ToString(); }
                    step++;
                    listView1.Items.Insert(listView1.Items.Count, li);
                }
                listView1.EndUpdate();
            }
            catch (Exception ex)
            {
                listView1.Items.Clear();
                dbPersonInfos.Clear();
                LoggerHelper.Debug(ex);
            }
            return dbPersonInfos;
        }

        public void RefreshGrroupData(ComboBox groupsCbx, string selectGroupName = " ")
        {
            try
            {
                List<string> list = freeSql.Select<DbGroupInfos>().Distinct().ToList(a => a.Name);
                groupsCbx.Items.Clear();
                AutoCompleteStringCollection lstsourece = new AutoCompleteStringCollection();
                foreach (var item in list)
                {
                    groupsCbx.Items.Add(item);
                    lstsourece.Add(item);
                }
                groupsCbx.AutoCompleteCustomSource = lstsourece;
                if (!string.IsNullOrEmpty(selectGroupName))
                {
                    int index = groupsCbx.Items.IndexOf(selectGroupName);
                    if (index >= 0)
                    {
                        groupsCbx.SelectedIndex = index;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                groupsCbx.Items.Clear();
                groupsCbx.AutoCompleteCustomSource = null;
            }
        }

        /// <summary>
        /// 清除已配对
        /// </summary>
        /// <param name="userControl1S"></param>
        public void ClearMatchStudent(List<UserControl1> userControl1S)
        {
            int len = userControl1S.Count;
            for (int i = 0; i < len; i++)
            {
                userControl1S[i].p_IdNumber = "未分配";
                userControl1S[i].p_Name = "未分配";
                userControl1S[i].p_Score = "0";
                userControl1S[i].p_roundCbx_selectIndex = 0;
                userControl1S[i].p_stateCbx_selectIndex = 0;
            }
        }

        public List<ResultInfos> GetResultInfos(DbPersonInfos dpi, int nowRound)
        {
            return freeSql.Select<ResultInfos>().Where(a => a.PersonId == dpi.Id.ToString() && a.IsRemoved == 0 && a.RoundId == nowRound + 1).OrderBy(a => a.Id).ToList();
        }

        public List<ResultInfos> GetResultInfos(string id)
        {
            return freeSql.Select<ResultInfos>()
                .Where(a => a.PersonIdNumber == id && a.IsRemoved == 0).OrderBy(a => a.Id).ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<DbPersonInfos> GetPersonInfo(string text)
        {
            return freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == text).ToList();
        }

        /// <summary>
        /// 保存成绩
        /// </summary>
        /// <param name="userControl1s"></param>
        /// <param name="groupsCbx"></param>
        /// <param name="sportProjectInfos"></param>
        public void WriteScoreIntoDb(List<UserControl1> userControl1s, ComboBox groupsCbx,
            SportProjectInfos sportProjectInfos, ListView listView1)
        {
            try
            {
                freeSql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
                List<ResultInfos> insertResults = new List<ResultInfos>();
                List<DbPersonInfos> dbPersonInfos =
                    freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == groupsCbx.Text).ToList();
                StringBuilder errorsb = new StringBuilder();
                foreach (var user in userControl1s)
                {
                    if (string.IsNullOrEmpty(user.p_IdNumber) || user.p_IdNumber == "未分配") continue;
                    string idNumber = user.p_IdNumber;
                    int state = user.p_stateCbx_selectIndex;
                    int roundId = user.p_roundCbx_selectIndex;
                    double.TryParse(user.p_Score, out double score);
                    DbPersonInfos df = dbPersonInfos.Find(a => a.IdNumber == idNumber);
                    if (state == 0)
                    {
                        errorsb.AppendLine($"{df.IdNumber},{df.Name}未完成测试!!!");
                        continue;
                    }
                    bool t_flag = false;
                    //检查轮次
                    for (int i = roundId; i < sportProjectInfos.RoundCount; i++)
                    {
                        List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>()
                            .Where(a => a.PersonIdNumber == idNumber
                                        && a.IsRemoved == 0
                                        && a.RoundId == i + 1)
                            .OrderBy(a => a.Id)
                            .ToList();
                        if (resultInfos.Count == 0)
                        {
                            t_flag = true;
                            maxSortId++;
                            ResultInfos rinfo = new ResultInfos();
                            rinfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            rinfo.SortId = maxSortId;
                            rinfo.IsRemoved = 0;
                            rinfo.PersonId = df.Id.ToString();
                            rinfo.SportItemType = 0;
                            rinfo.PersonName = df.Name;
                            rinfo.PersonIdNumber = df.IdNumber;
                            rinfo.RoundId = i + 1;
                            rinfo.Result = score;
                            rinfo.State = state;
                            insertResults.Add(rinfo);
                            break;
                        }
                    }

                    if (!t_flag)
                    {
                        errorsb.AppendLine($"{df.IdNumber},{df.Name}轮次已满");
                    }
                }

                int result = freeSql.InsertOrUpdate<ResultInfos>()
                    .SetSource(insertResults)
                    .IfExistsDoNothing()
                    .ExecuteAffrows();
                if (errorsb.Length != 0) MessageBox.Show(errorsb.ToString());
                if (result > 0) UpDataListView(listView1, groupsCbx, sportProjectInfos);
            }
            catch (Exception exception)
            {
                LoggerHelper.Debug(exception);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        public bool UpLoadStuGroupScore(object obj)
        {
            try
            {
                List<Dictionary<string, string>> successList = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> errorList = new List<Dictionary<string, string>>();
                Dictionary<string, string> localInfos = new Dictionary<string, string>();
                List<LocalInfos> list0 = freeSql.Select<LocalInfos>().ToList();
                string cpuid = CPUHelper.GetCpuID();
                foreach (var item in list0)
                {
                    localInfos.Add(item.key, item.value);
                } //组
                string groupName = obj as string;
                SportProjectInfos sportProjectInfos = freeSql.Select<SportProjectInfos>().ToOne();
                List<DbGroupInfos> dbGroupInfos = new List<DbGroupInfos>();
                ///查询本项目已考组
                if (!string.IsNullOrEmpty(groupName))
                {
                    //sql0 += $" AND Name = '{groupName}'";
                    dbGroupInfos = freeSql.Select<DbGroupInfos>().Where(a => a.Name == groupName).ToList();
                }

                UploadResultsRequestParameter urrp = new UploadResultsRequestParameter();
                urrp.AdminUserName = localInfos["AdminUserName"];
                urrp.TestManUserName = localInfos["TestManUserName"];
                urrp.TestManPassword = localInfos["TestManPassword"];
                string MachineCode = localInfos["MachineCode"];
                string ExamId = localInfos["ExamId"];
                if (MachineCode.IndexOf('_') != -1)
                {
                    MachineCode = MachineCode.Substring(MachineCode.IndexOf('_') + 1);
                }

                if (ExamId.IndexOf('_') != -1)
                {
                    ExamId = ExamId.Substring(ExamId.IndexOf('_') + 1);
                }

                urrp.MachineCode = MachineCode;
                urrp.ExamId = ExamId;
                StringBuilder messageSb = new StringBuilder();
                StringBuilder logWirte = new StringBuilder();

                ///按组上传
                foreach (var gInfo in dbGroupInfos)
                {
                    List<DbPersonInfos> dbPersonInfos =
                        freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == gInfo.Name).ToList();
                    StringBuilder resultSb = new StringBuilder();
                    List<SudentsItem> sudentsItems = new List<SudentsItem>();
                    //IdNumber 对应Id
                    Dictionary<string, string> map = new Dictionary<string, string>();
                    //取值模式
                    bool isBestScore = sportProjectInfos.BestScoreMode == 0;
                    foreach (var stu in dbPersonInfos)
                    {
                        List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>()
                            .Where(a => a.PersonIdNumber == stu.IdNumber).ToList();
                        //无成绩的跳过
                        if (resultInfos.Count == 0) continue;
                        int state = -1;
                        string examTime = string.Empty;
                        double MaxScore = 99999;
                        if (isBestScore) MaxScore = 0;
                        foreach (var ri in resultInfos)
                        {
                            if (ri.State <= 0) continue;
                            ///异常状态
                            if (ri.State != 1)
                            {
                                if (isBestScore && MaxScore < 0)
                                {
                                    //取最大值
                                    MaxScore = 0;
                                    state = ri.State;
                                }
                                else if (!isBestScore && MaxScore > 99999)
                                {
                                    //取最小值
                                    MaxScore = 99999;
                                    state = ri.State;
                                }
                            }
                            else
                            {
                                if (isBestScore && MaxScore < ri.Result)
                                {
                                    //取最大值
                                    MaxScore = ri.Result;
                                    state = ri.State;
                                }
                                else if (!isBestScore && MaxScore > ri.Result)
                                {
                                    //取最小值
                                    MaxScore = ri.Result;
                                    state = ri.State;
                                }
                            }

                            examTime = ri.CreateTime;
                        }

                        if (state < 0) continue;
                        if (state != 1)
                        {
                            MaxScore = 0;
                        }
                        List<RoundsItem> roundsItems = new List<RoundsItem>();
                        RoundsItem rdi = new RoundsItem();
                        rdi.RoundId = 1;
                        rdi.State = ResultStateType.Match(state);
                        rdi.Time = examTime;
                        StringBuilder logSb = new StringBuilder();
                        List<LogInfos> logInfos = freeSql.Select<LogInfos>()
                                .Where(a => a.IdNumber == stu.IdNumber && a.State != -404)
                                .ToList();
                        logInfos.ForEach(item =>
                        {
                            string sbtxt = $"时间：{item.CreateTime},考号:{item.IdNumber},{item.Remark};";
                            logSb.Append(sbtxt);
                        });
                        rdi.Memo = logSb.ToString();
                        rdi.Ip = cpuid;
                        ///可以处理成绩
                        rdi.Result = MaxScore;

                        #region 查询文件

                        //成绩根目录
                        Dictionary<string, string> dic_images = new Dictionary<string, string>();
                        Dictionary<string, string> dic_viedos = new Dictionary<string, string>();
                        Dictionary<string, string> dic_texts = new Dictionary<string, string>();
                        string scoreRoot = Application.StartupPath +
                                           $"\\Scores\\{sportProjectInfos.Name}\\{stu.GroupName}\\";
                        DateTime.TryParse(examTime, out DateTime examTime_dt);
                        string dateStr = examTime_dt.ToString("yyyyMMdd");
                        string GroupNo = $"{dateStr}_{stu.GroupName}_{stu.IdNumber}_1";
                        if (Directory.Exists(scoreRoot))
                        {
                            List<DirectoryInfo> rootDirs = new DirectoryInfo(scoreRoot).GetDirectories().ToList();
                            string dirEndWith = $"_{stu.IdNumber}_{stu.Name}";
                            DirectoryInfo directoryInfo = rootDirs.Find(a => a.Name.EndsWith(dirEndWith));
                            if (directoryInfo != null)
                            {
                                string stuDir = Path.Combine(scoreRoot, directoryInfo.Name);
                                GroupNo = $"{dateStr}_{stu.GroupName}_{directoryInfo.Name}_1";
                                if (Directory.Exists(stuDir))
                                {
                                    int step = 1;
                                    FileInfo[] files = new DirectoryInfo(stuDir).GetFiles("*.jpg");
                                    if (files.Length > 0)
                                    {
                                        foreach (var item in files)
                                        {
                                            dic_images.Add(step + "", item.Name);
                                            step++;
                                        }
                                    }
                                    step = 1;
                                    files = new DirectoryInfo(stuDir).GetFiles("*.txt");
                                    if (files.Length > 0)
                                    {
                                        foreach (var item in files)
                                        {
                                            dic_texts.Add(step + "", item.Name);
                                            step++;
                                        }
                                    }
                                    step = 1;
                                    files = new DirectoryInfo(stuDir).GetFiles("*.mp4");
                                    if (files.Length > 0)
                                    {
                                        foreach (var item in files)
                                        {
                                            dic_viedos.Add(step + "", item.Name);
                                            step++;
                                        }
                                    }
                                }
                            }
                        }

                        #endregion 查询文件

                        rdi.GroupNo = GroupNo;
                        rdi.Text = dic_texts;
                        rdi.Images = dic_images;
                        rdi.Videos = dic_viedos;
                        roundsItems.Add(rdi);
                        SudentsItem ssi = new SudentsItem();
                        ssi.SchoolName = stu.SchoolName;
                        ssi.GradeName = stu.GradeName;
                        ssi.ClassNumber = stu.ClassNumber;
                        ssi.Name = stu.Name;
                        ssi.IdNumber = stu.IdNumber;
                        ssi.Rounds = roundsItems;
                        sudentsItems.Add(ssi);
                        map.Add(stu.IdNumber, stu.Id.ToString());
                    }
                    if (sudentsItems.Count == 0) continue;
                    urrp.Sudents = sudentsItems;
                    //序列化json
                    string JsonStr = JsonConvert.SerializeObject(urrp);
                    string url = localInfos["Platform"] + RequestUrl.UploadResults;
                    var httpUpload = new HttpUpload();
                    var formDatas = new List<FormItemModel>();
                    //添加其他字段
                    formDatas.Add(new FormItemModel()
                    {
                        Key = "data",
                        Value = JsonStr
                    });
                    logWirte.AppendLine();
                    logWirte.AppendLine();
                    logWirte.AppendLine(JsonStr);
                    //上传学生成绩
                    string result = HttpUpload.PostForm(url, formDatas);
                    upload_Result upload_Result = JsonConvert.DeserializeObject<upload_Result>(result);
                    string errorStr = "null";
                    List<Dictionary<string, int>> result1 = upload_Result.Result;
                    foreach (var item in sudentsItems)
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        //map
                        dic.Add("Id", map[item.IdNumber]);
                        dic.Add("IdNumber", item.IdNumber);
                        dic.Add("Name", item.Name);
                        dic.Add("uploadGroup", item.Rounds[0].GroupNo);
                        var value = 0;
                        result1.Find(a => a.TryGetValue(item.IdNumber, out value));
                        if (value == 1 || value == -4)
                        {
                            successList.Add(dic);
                        }
                        else if (value != 0)
                        {
                            errorStr = uploadResult.Match(value);
                            dic.Add("error", errorStr);
                            errorList.Add(dic);
                            messageSb.AppendLine(
                                $"{gInfo.Name}组 考号:{item.IdNumber} 姓名:{item.Name}上传失败,错误内容:{errorStr}");
                        }
                    }
                    //写入失败log
                    WriteLogWithFauilt(errorList, gInfo);
                }
                //写入成功写入日志
                WriteLogWriteSucess(successList);
                LoggerHelper.Monitor(logWirte.ToString());
                string outpitMessage = messageSb.ToString();
                return true;
            }
            catch (Exception exception)
            {
                LoggerHelper.Debug(exception);
                return
                false;
            }
        }

        private void WriteLogWriteSucess(List<Dictionary<string, string>> successList)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"成功:{successList.Count}");
            sb.AppendLine("******************success*********************");
            foreach (var item in successList)
            {
                freeSql.Update<DbPersonInfos>()
                    .Set(a => a.uploadGroup == "1")
                    .Where(a => a.Id == Convert.ToInt32(item["Id"]))
                    .ExecuteAffrows();
                freeSql.Update<ResultInfos>()
                    .Set(a => a.uploadState == 1)
                    .Where(a => a.PersonId == item["Id"])
                    .ExecuteAffrows();
                ;
                sb.AppendLine($"考号:{item["IdNumber"]} 姓名:{item["Name"]}");
            }
            sb.AppendLine("*******************success********************");
            if (successList.Count != 0)
            {
                string txtpath = Application.StartupPath + $"\\Log\\upload\\";
                txtpath = Path.Combine(txtpath, $"upload_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
                File.WriteAllText(txtpath, sb.ToString());
                successList.Clear();
            }
        }

        /// <summary>
        /// 写入失败log
        ///
        ///
        /// </summary>
        /// <param name="errorList"></param>
        /// <param name="gInfo"></param>
        private void WriteLogWithFauilt(List<Dictionary<string, string>> errorList, DbGroupInfos gInfo)
        {
            string txtpath = Application.StartupPath + $"\\Log\\upload\\";
            if (!Directory.Exists(txtpath))
            {
                Directory.CreateDirectory(txtpath);
            }
            StringBuilder errorsb = new StringBuilder();
            errorsb.AppendLine($"失败:{errorList.Count}");
            errorsb.AppendLine("****************error***********************");
            foreach (var item in errorList)
            {
                errorsb.AppendLine($"考号:{item["IdNumber"]} 姓名:{item["Name"]} 错误:{item["error"]}");
            }
            errorsb.AppendLine("*****************error**********************");
            if (errorList.Count != 0)
            {
                string txtpath1 = Path.Combine(txtpath,
                    $"error_{gInfo.Name}_upload_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
                File.WriteAllText(txtpath1, errorsb.ToString());
                errorList.Clear();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="sportProjectInfos"></param>
        /// <exception cref="Exception"></exception>
        public void PrintScore(string groupName, SportProjectInfos sportProjectInfos)
        {
            try
            {
                if (string.IsNullOrEmpty(groupName)) throw new Exception("未选择组");
                string path = Application.StartupPath + "\\Data\\PrintExcel\\";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path = Path.Combine(path, $"PrintExcel_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
                List<Dictionary<string, string>> ldic = new List<Dictionary<string, string>>();
                //序号 项目名称    组别名称 姓名  准考证号 考试状态    第1轮 第2轮 最好成绩
                List<DbPersonInfos> dbPersonInfos = new List<DbPersonInfos>();
                dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == groupName).ToList();
                List<OutPutPrintExcelData> outPutExcelDataList = new List<OutPutPrintExcelData>();
                int step = 1;
                bool isBestScore = sportProjectInfos.BestScoreMode == 0;
                foreach (var dpInfo in dbPersonInfos)
                {
                    List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>().Where(a => a.PersonId == dpInfo.Id.ToString() && a.IsRemoved == 0).ToList();
                    OutPutPrintExcelData opd = new OutPutPrintExcelData();
                    opd.Id = step;
                    opd.examTime = dpInfo.CreateTime;
                    opd.School = dpInfo.SchoolName;
                    opd.Name = dpInfo.Name;
                    opd.Sex = dpInfo.Sex == 0 ? "男" : "女";
                    opd.IdNumber = dpInfo.IdNumber;
                    opd.GroupName = dpInfo.GroupName;
                    int state = 0;
                    double MaxScore = 99999;
                    if (isBestScore) MaxScore = 0;
                    foreach (var ri in resultInfos)
                    {
                        ///异常状态
                        if (ri.State != 1)
                        {
                            if (isBestScore && MaxScore < 0)
                            {
                                //取最大值
                                MaxScore = 0;
                                state = ri.State;
                            }
                            else if (!isBestScore && MaxScore > 99999)
                            {
                                //取最小值
                                MaxScore = 99999;
                                state = ri.State;
                            }
                        }
                        else if (ri.State > 0)
                        {
                            if (isBestScore && MaxScore < ri.Result)
                            {
                                //取最大值
                                MaxScore = ri.Result;
                                state = ri.State;
                            }
                            else if (!isBestScore && MaxScore > ri.Result)
                            {
                                //取最小值
                                MaxScore = ri.Result;
                                state = ri.State;
                            }
                        }
                    }
                    if (state < 0) continue;
                    if (state != 1)
                    {
                        MaxScore = 0;
                        opd.Result = ResultStateType.Match(state);
                    }
                    else
                    {
                        opd.Result = MaxScore.ToString();
                    }
                    outPutExcelDataList.Add(opd);
                    step++;
                }
                //result = ExcelUtils.OutPutExcel(ldic, path);
                MiniExcel.SaveAs(path, outPutExcelDataList);
                if (File.Exists(path))
                {
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.FileName = path;
                    p.StartInfo.Verb = "print";
                    p.Start();
                }
                else
                {
                    throw new Exception("导出失败");
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Debug(exception);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listView1"></param>
        /// <param name="strState"></param>

        public void SetErrorState(ListView listView1, int _nowRound, string stateStr, ComboBox comboBox, SportProjectInfos sportProjectInfos)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0) return;
                string idnumber = listView1.SelectedItems[0].SubItems[3].Text;
                int state = ResultStateType.ResultState2Int(stateStr);
                int result = freeSql.Update<ResultInfos>().Set(a => a.State == state)
                    .Where(a => a.PersonIdNumber == idnumber && a.RoundId == _nowRound + 1).ExecuteAffrows();
                if (result == 0)
                {
                    freeSql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
                    List<ResultInfos> insertResults = new List<ResultInfos>();
                    DbPersonInfos dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.IdNumber == idnumber).ToOne();
                    maxSortId++;
                    ResultInfos rinfo = new ResultInfos();
                    rinfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    rinfo.SortId = maxSortId;
                    rinfo.IsRemoved = 0;
                    rinfo.PersonId = dbPersonInfos.Id.ToString();
                    rinfo.SportItemType = 0;
                    rinfo.PersonName = dbPersonInfos.Name;
                    rinfo.PersonIdNumber = dbPersonInfos.IdNumber;
                    rinfo.RoundId = _nowRound + 1;
                    rinfo.Result = 0;
                    rinfo.State = state;
                    insertResults.Add(rinfo);
                    result = freeSql.InsertOrUpdate<ResultInfos>()
                        .SetSource(insertResults)
                        .IfExistsDoNothing().ExecuteAffrows();
                }

                if (result > 0)
                {
                    UpDataListView(listView1, comboBox, sportProjectInfos);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }

        public void CheckChange1(CheckBox checkBox)
        {
            int value = checkBox.Checked ? 1 : 0;
            File.WriteAllText(AutoMatchLog, value.ToString());
        }

        public void CheckChange2(CheckBox checkBox)
        {
            //自动上传
            int value = checkBox.Checked ? 1 : 0;
            File.WriteAllText(AutoUploadLog, value.ToString());
        }

        public void CheckChange3(CheckBox checkBox)
        {
            //自动打印
            int value = checkBox.Checked ? 1 : 0;
            File.WriteAllText(AutoPrintLog, value.ToString());
        }
    }
}