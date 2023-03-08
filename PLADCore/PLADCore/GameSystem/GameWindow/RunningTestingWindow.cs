using PLADCore.GameSystem.GameWindowSys;
using PLADCoreDataModel.GameModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PLADCore.GameSystem.MyControl;
using Sunny.UI;

namespace PLADCore.GameSystem.GameWindow
{
    public partial class RunningTestingWindow : Form
    {
        public RunningTestingWindow()
        {
            InitializeComponent();
        }
           
        private NFC_Help hfNfcHelp = new NFC_Help();
        private SportProjectInfos SportProjectInfos { get; set; }
        private List<UserControl1> _userControl1s = new List<UserControl1>();
        private List<SerialReader> _serialReaders = new List<SerialReader>();
        private List<string> connectPort = new List<string>();

        private Dictionary<string, string> localInfo = new Dictionary<string, string>();
        private string portName = "CH340";
        private string groupName = string.Empty;
        private int _nowRound = 0;//点前轮次
        private bool IsReset = false;// 是否存在重置的学生
        private List<string> ResetStudentExamIDData = new List<string>();//被重置的学生考号信息
        private void RunningTestingWindow_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            toolStripStatusLabel1.Text = "程序集版本:" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            SportProjectInfos = RunningTestingWindowSys.Instance.LoadingSportData();
            RunningTestingWindowSys.Instance.CloseAllSerial(_userControl1s, _serialReaders, connectPort);
            if (!InitUserControl())
            {
                this.Close();
            }
            else
            {
                uiComboBox2.Items.Clear();
                int roundCount = SportProjectInfos.RoundCount;
                if (roundCount > 0)
                {
                    for (int i = 0; i <roundCount; i++)
                    {
                        uiComboBox2.Items.Add($"第{i + 1}轮");
                    }
                    uiComboBox2.SelectedIndex = 0;
                }
                var li = RunningTestingWindowSys.Instance.GetLocalInfo();
                foreach (var item in li)
                {
                    localInfo.Add(item.key,item.value);
                }
                hfNfcHelp.AddUSBEventWatcher(USBEventHanlder, USBEventHanlder, new TimeSpan(0, 0, 1));
                RunningTestingWindowSys.Instance.InitListViewHeader(listView1, SportProjectInfos.RoundCount);
                RunningTestingWindowSys.Instance.UpDataGroup(groupName,comboBox1);
                RunningTestingWindowSys.Instance.SetRunningWindowCheck(checkBox1, checkBox2, checkBox3);
            }
        }

        private bool isMatchingDevice = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void USBEventHanlder(object sender, EventArrivedEventArgs e)
        {
            var watcher = sender as ManagementEventWatcher;
            watcher.Stop();

            if (e.NewEvent.ClassPath.ClassName == "__InstanceCreationEvent")
            {
                Console.WriteLine("设备连接");
                if (isMatchingDevice)
                {
                    ///扫描串口
                    RefreshComPorts();
                }
            }
            else if (e.NewEvent.ClassPath.ClassName == "__InstanceDeletionEvent")
            {
                if (!isMatchingDevice)
                {
                    Console.WriteLine("设备断开");
                    List<string> list = CheckPortISConnected();
                    if (list.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var l in list)
                        {
                            sb.AppendLine($"{l}断开!");
                        }
                        MessageBox.Show(sb.ToString());
                    }
                    //检测断开,断开提示
                    //MessageBox.Show("设备断开请检查");
                }
            }

            watcher.Start();
        }
        /// <summary>
        /// 检查串口是否剖连接
        /// </summary>
        /// <returns></returns>
        private List<string> CheckPortISConnected()
        {
            List<string> ports = new List<string>();
            connectPort.Clear();
            int step = 0;
            foreach (var  item in  _serialReaders)
            {
                step++;
                if (item != null)
                {
                    try
                    {
                        if (item.IsComOpen())
                        {
                            connectPort.Add(item.iSerialPort.PortName);
                        }
                        else
                        {
                            ports.Add(item.iSerialPort.PortName);
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
                else
                {
                    ports.Add($"设备{step}号");
                }
            }

            return ports;
        }
        /// <summary>
        /// 刷新串口
        /// </summary>
        private void RefreshComPorts()
        {
            try
            {
                string[] portNames =  GetPortDeviceName();
                if (portNames.Length == 0)
                {
                    MessageBox.Show($"未找到{portNames}串口,请检查驱动");
                    MatchBtnSwitch(true);
                    return;
                }

                foreach (var port in portNames)
                {
                    CheckPortISConnected();
                    //已连接则跳过
                    if (connectPort.Contains(port)) continue;
                    int step = 0;
                    foreach (var sr in _serialReaders)
                    {
                        if (sr != null && !sr.IsComOpen())
                        {
                            string strException = string.Empty;
                            int nRet = sr.OpenCom(port, 9600, out strException);
                            if (nRet == 0)
                            {
                                //连接成功
                                _userControl1s[step].p_toolState = $"{port}已连接";
                               _userControl1s[step].p_toolState_color = Color.Green;
                               _userControl1s[step].p_title_Color = Color.MediumSpringGreen;
                            }
                            else
                            {
                                MessageBox.Show($"{port}连接失败\n错误:{strException},请检查");
                            }

                            break;
                        }

                        step++;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
            finally
            {
                CheckPortISConnected();
                if (connectPort.Count == _userControl1s.Count)
                {
                    HZH_Controls. ControlHelper.ThreadInvokerControl(this, () =>
                    {
                        MatchBtnSwitch(true);
                        string portSavePath = Application.StartupPath + "\\Data\\portSave.log";
                        File.WriteAllLines(portSavePath, connectPort);
                        MessageBox.Show("设备串口匹配完成");
                    });
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private string[] GetPortDeviceName(string port="")
        {
            if (string.IsNullOrEmpty(port)) port = portName;
            List<string> strs = new List<string>();
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PnPEntity where Name like '%(COM%'"))
            {
                var hardInfos = searcher.Get();
                foreach (var hardInfo in hardInfos)
                {
                    if (hardInfo.Properties["Name"].Value != null)
                    {
                        string deviceName = hardInfo.Properties["Name"].Value.ToString();
                        if (deviceName.Contains(portName))
                        {
                            int a = deviceName.IndexOf("(COM") + 1;//a会等于1
                            string str = deviceName.Substring(a, deviceName.Length - a);
                            a = str.IndexOf(")");//a会等于1
                            str = str.Substring(0, a);
                            strs.Add(str);
                        }
                    }
                }
            }
            return strs.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        private void MatchBtnSwitch(bool p)
        {
            if (p)
            {
                uiButton3.Text = "匹配设备";
                uiButton3.BackColor = Color.White;
                isMatchingDevice = false;
            }
            else
            {
                uiButton3.Text = "匹配中";
                uiButton3.BackColor = Color.Red;
                isMatchingDevice = true;
            }
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool InitUserControl()
        {
            try
            {
                if (!RunningTestingWindowSys.Instance.ShowRunMachineWindow())
                {
                    return false;
                }
                else
                {
                    int mach = RunningTestingWindowSys.Instance.GetMachineCount();
                    portName = RunningTestingWindowSys.Instance.GetPortName();
                    _userControl1s.Clear();
                    flowLayoutPanel1.Controls.Clear();
                    List<string> li = new List<string>();
                    for (int i = 0; i < SportProjectInfos.RoundCount; i++)
                    {
                        li.Add($"第{i+1}轮");
                    }

                    for (int i = 0; i < mach; i++)
                    {
                        UserControl1 use = new UserControl1();
                        use.p_title = $"{i + 1}号设备";
                        use.p_roundCbx_items = li;
                        _userControl1s.Add(use);
                        flowLayoutPanel1.Controls.Add(use);
                        SerialReader serialReader = new SerialReader(i);
                        serialReader.AnalyCallback = AnalyData;
                        serialReader.ReceiveCallback = ReceieveData;
                        serialReader.SendCallback = SendData;
                        _serialReaders.Add(serialReader);
                    }

                    int s = flowLayoutPanel1.Controls.Count;
                    Console.WriteLine(  s);
                    return true;
                }  
            }
            catch (Exception e)
            {
                LoggerHelper.Debug(e);
                return false;
            }
        }

        private void SendData(MachineMsgCode btarysenddata)
        {
             
        }
        private void ReceieveData(byte[] btaryreceivedata)
        {
             
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void AnalyData(SerialMsg  data)
        {
             if(data==null ) return;
             switch (data.mms.type)
             {
                 case 1:
                     string tmp = _userControl1s[data.number].p_toolState;
                     try
                     {
                         string st = connectPort[data.number];
                         _userControl1s[data.number].p_toolState = st + "已连接。登录成功！";
                     }
                     catch (Exception e)
                     {
                         _userControl1s[data.number].p_toolState = tmp;
                        LoggerHelper.Debug(e);
                     }
                     break;
                 case 2 :
                     if (data.mms.code == 1)
                     {
                         Console.WriteLine("测试开始！！");
                     }
                     break;
                 case 3:
                     GetScore(data);
                     break;
                 case 4 :
                     break;
                 default: break;
             }
        }

        private StringBuilder writeStringBuilder = new StringBuilder();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void GetScore(SerialMsg data)
        {
            try
            {
                int score = data.mms.fhl_result;
                int index = data.number;
                if (score > 0)
                {
                    string idNum = _userControl1s[index].p_IdNumber;
                    string stuNum = _userControl1s[index].p_Name;
                    writeStringBuilder.AppendLine($"考号：{idNum}，姓名：{stuNum}，成绩：{score}");
                }

                _userControl1s[index].p_Score = score.ToString();
                if (_userControl1s[index].p_stateCbx_selectIndex==0)
                {
                    _userControl1s[index].p_stateCbx_selectIndex = 1;
                }

                _serialReaders[index].SendMessage(data.mms);
            }
            catch (Exception exception)
            {
                LoggerHelper.Debug(exception);
            }
        }
        /// <summary>
        /// 导入最近匹配
        /// </summary>
         
        private void ImportRecentMatchData()
        {
            string portSavePath = Application.StartupPath + "\\data\\portSave.log";
            if (File.Exists(portSavePath))
            {
                string[] prots = File.ReadAllLines(portSavePath);
                connectPort.Clear();
                foreach (var prot in prots)
                {
                    CheckPortISConnected();
                    if (connectPort.Contains(prot)) continue;
                    else
                    {
                        int step = 0;
                        foreach (var sr in _serialReaders)
                        {
                            if (sr != null && !sr.IsComOpen())
                            {
                                string sy = string.Empty;
                                int ret = sr.OpenCom(prot, 9600, out sy);
                                if (ret == 0)
                                {
                                    _userControl1s[step].p_toolState = $"{prot}已连接";
                                    _userControl1s[step].p_toolState_color = Color.Green;
                                    _userControl1s[step].p_title_Color = Color.MediumSpringGreen;
                                    connectPort.Add(prot);
                                }
                                else
                                {
                                    UIMessageBox.ShowError($"{prot}连接失败\n错误:{sy},请检查");
                                    return;
                                }
                                break;

                            }
                            step++;
                        }
                    }
                }
            }
            else
            {
                UIMessageBox.ShowError("无法匹配最近的端口数据！！");
                return;
            }
        }
        /// <summary>
        /// 自动匹配
        /// </summary>
        private void AutoMatchStudent()
        {
            try
            {
                 
                    RunningTestingWindowSys.Instance.ClearMatchStudent(_userControl1s);
                    List<DbPersonInfos> dbPersonInfos =
                        RunningTestingWindowSys.Instance.UpDataListView(listView1, comboBox1, SportProjectInfos);
                    int nlen = _serialReaders.Count;
                    int step = 0;
                    foreach (var dpi in dbPersonInfos)
                    {
                        List<ResultInfos> resultInfos = RunningTestingWindowSys.Instance.GetResultInfos(dpi, _nowRound);
                        if (resultInfos.Count != 0) continue;
                        _userControl1s[step].p_IdNumber = dpi.IdNumber;
                        _userControl1s[step].p_Name = dpi.Name;
                        _userControl1s[step].p_Score = "0";
                        _userControl1s[step].p_roundCbx_selectIndex = resultInfos.Count;
                        _userControl1s[step].p_stateCbx_selectIndex = 0;
                        step++;
                        if (step >= _userControl1s.Count) break;
                    }

                    if (step == 0)
                    {
                        if (comboBox1.SelectedIndex >= 0)
                        {
                            comboBox1.SelectedIndex++;
                            int rcbindex = uiComboBox2.SelectedIndex;
                            rcbindex--;
                            if (rcbindex < 0) rcbindex = 0;
                            uiComboBox2.SelectedIndex = rcbindex;
                            AutoMatchStudent();
                        }
                    }
                
                
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }
        /// <summary>
         /// 选择匹配
         /// </summary>
        private void ChooseMatchStudent()
        {
            RunningTestingWindowSys.Instance. ClearMatchStudent(_userControl1s);
            try
            {
                if ( listView1.SelectedItems.Count==0)return;
                int step = 0;
                List<DbPersonInfos> dbPersonInfos = RunningTestingWindowSys.Instance.GetPersonInfo(comboBox1.Text);
                foreach ( ListViewItem item in listView1.SelectedItems)
                {
                    string idNumber = item.SubItems[3].Text;
                    List<ResultInfos> resultInfos = RunningTestingWindowSys.Instance.GetResultInfos(idNumber);
                    if (resultInfos.Count != 0) continue;
                    DbPersonInfos dbPersonInfos1 = dbPersonInfos.Find(a => a.IdNumber == idNumber);
                    _userControl1s[step].p_IdNumber = idNumber;
                    _userControl1s[step].p_Name = dbPersonInfos1.Name;
                    _userControl1s[step].p_Score = "0 ml";
                    _userControl1s[step].p_roundCbx_selectIndex = resultInfos.Count;
                    _userControl1s[step].p_stateCbx_selectIndex = 0;
                    step++;
                    if (step >= _userControl1s.Count) break;
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
         
        public void StarUpLoadStuGroupScore( )
        {
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(UpLoadStuGroupScore);
            Thread thread = new Thread(parameterizedThreadStart);
            thread.IsBackground = true;
            thread.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void UpLoadStuGroupScore(object obj)
        {
            LoadingWindowSys.Instance.ShowLoadingWindow();
            if (RunningTestingWindowSys.Instance.UpLoadStuGroupScore(obj))
            {
                HZH_Controls.ControlHelper.ThreadInvokerControl(this, () =>
                {
                    
                    LoadingWindowSys.Instance.SetLoadingWindowClose();
                    UIMessageBox.ShowSuccess("上传成功！！");
                    uiButton9.Text = "上传本组";
                    uiButton9.ForeColor = Color.Blue;
                });
            }
            else
            {
                HZH_Controls.ControlHelper.ThreadInvokerControl(this, () => { UIMessageBox.ShowError("上传失败！！");}); 
            }
        }
        
        #region  页面事件
        /// <summary>
        /// 按钮1 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.CloseAllSerial(_userControl1s,_serialReaders,connectPort);
            InitUserControl();
        }
        /// <summary>
        ///  导入最近匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton2_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.CloseAllSerial(_userControl1s,_serialReaders,connectPort);
            int len = _userControl1s.Count;
            for (int i = 0; i < len; i++)
            {
                SerialReader serialReader = new SerialReader(i);
                serialReader.AnalyCallback = AnalyData;
                serialReader.ReceiveCallback = ReceieveData;
                serialReader.SendCallback = SendData;
                _serialReaders.Add(serialReader);
            }
            ImportRecentMatchData();
        }
        /// <summary>
         /// 匹配设备
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
          
        private void uiButton3_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.CloseAllSerial(_userControl1s,_serialReaders,connectPort);
            int len = _userControl1s.Count;
            for (int i = 0; i < len; i++)
            {
                //初始化访问读写器实例
                SerialReader reader = new SerialReader(i);
                //回调函数
                reader.AnalyCallback = AnalyData;
                reader.ReceiveCallback = ReceieveData;
                reader.SendCallback = SendData;
                _serialReaders.Add(reader);
            }
            MatchBtnSwitch(isMatchingDevice);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.UpDataListView(listView1, comboBox1, SportProjectInfos);
        }
        /// <summary>
        ///刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         
        private void uiButton4_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.RefreshGrroupData(comboBox1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
      
        private void uiComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uiComboBox2.SelectedIndex >= 0)
            {
                _nowRound = uiComboBox2.SelectedIndex;
            }
            else
                return;
        }
        /// <summary>
        /// 自动匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton5_Click(object sender, EventArgs e)
        {
            AutoMatchStudent( );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton6_Click(object sender, EventArgs e)
        {
            ChooseMatchStudent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void uiButton7_Click(object sender, EventArgs e)
        {
            List<string> list = CheckPortISConnected();
            if (list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var l in list)
                {
                    sb.AppendLine($"{l}断开!");
                }
                MessageBox.Show(sb.ToString());
                return;
            }
            MachineMsgCode mmc = new MachineMsgCode();
            mmc.type = 2;
            int step = 0;
            foreach (var sr in _serialReaders)
            {
                if (string.IsNullOrEmpty(_userControl1s[step].p_IdNumber)
                    || _userControl1s[step].p_IdNumber == "未分配") continue;
                if (sr != null && sr.IsComOpen())
                {
                    sr.SendMessage(mmc);
                }
                step++;
            }
        }
        /// <summary>
        /// 重取成绩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton8_Click(object sender, EventArgs e)
        {
             
        }
        /// <summary>
         /// 保存成绩
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         /// <exception cref="NotImplementedException"></exception>
        private void uiButton11_Click(object sender, EventArgs e)
        {
           RunningTestingWindowSys.Instance. WriteScoreIntoDb(_userControl1s,comboBox1,SportProjectInfos,listView1);
            if (checkBox1.Checked) AutoMatchStudent();
        }

        /// <summary>
        /// 上传本组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton9_Click(object sender, EventArgs e)
        {
            HZH_Controls.ControlHelper.ThreadInvokerControl(this, () =>
            {
                uiButton9.Text = "上传中";
                uiButton9.ForeColor = Color.Red;
            });
            StarUpLoadStuGroupScore( );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void uiButton10_Click(object sender, EventArgs e)
        {
            string group = comboBox1.Text;
            new Thread((ThreadStart)delegate
            {
                RunningTestingWindowSys.Instance.PrintScore(group, SportProjectInfos);
            }).Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 缺考ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.SetErrorState(listView1,_nowRound, "缺考",comboBox1,SportProjectInfos);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 中退ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.SetErrorState(listView1, _nowRound,"中退",comboBox1,SportProjectInfos);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 犯规ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.SetErrorState(listView1, _nowRound,"犯规",comboBox1,SportProjectInfos);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 弃权ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.SetErrorState(listView1,_nowRound, "弃权",comboBox1,SportProjectInfos);
        } 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修正成绩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0) return;
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    string idnumber = listView1.SelectedItems[i].SubItems[3].Text;
                    ChooseRoundWindow fcr = new ChooseRoundWindow();
                    fcr._idnumber = idnumber;
                    fcr.mode = 1;
                    fcr.ShowDialog();
                }
                RunningTestingWindowSys.Instance.UpDataListView(listView1, comboBox1, SportProjectInfos);
                 
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        } 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 成绩重测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0) return;
                else
                {
                    for (int i = 0; i < listView1.SelectedItems.Count; i++)
                    {
                        string idnumber = listView1.SelectedItems[i].SubItems[3].Text;
                        ResetStudentExamIDData.Add(idnumber);
                        ChooseRoundWindow fcr = new ChooseRoundWindow();
                        fcr._idnumber = idnumber;
                        fcr.mode = 0;
                        fcr.ShowDialog();
                    }
                     RunningTestingWindowSys.Instance.UpDataListView(listView1, comboBox1, SportProjectInfos);
                    
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (writeStringBuilder.Length > 0)
            {
                File.AppendAllText("成绩日志.txt", writeStringBuilder.ToString());
                writeStringBuilder.Clear();
            }
        }
        /// <summary>
         /// 
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            ListView listView =(ListView) sender;
            ListViewItem listViewItem = listView.GetItemAt(e.X, e.Y);
            if (listViewItem != null && e.Button == MouseButtons.Right)
            {
                this.cmsListViewItem.Show(listView,e.X,e.Y);
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.CheckChange1(checkBox1);

        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.CheckChange2(checkBox2);
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.CheckChange3(checkBox3);
        }
        #endregion


        
    }
}
