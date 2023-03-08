using MiniExcelLibs;
using PLADCore.GameSystem.GameModel;
using PLADCore.GameSystem.GameWindow;
using PLADCoreDataModel.GameModel;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLADCore.GameSystem.GameWindowSys
{
    public    class ExportGradeWindowSys
    {
        private ExportGradeWindow ExportGradeWindow = null;
        public static ExportGradeWindowSys Instance;
        IFreeSql freeSql = DB.Sqlite;
         
        public void Awake()
        {
            Instance = this;
        }
        public void  SetExporWindowData(string groupName)
        {
           
            ExportGradeWindow = new ExportGradeWindow();
            ExportGradeWindow.groupName = groupName;
            ExportGradeWindow.ShowDialog();
        }

        public  SportProjectInfos LoadingInitData()
        {
            return freeSql.Select<SportProjectInfos>().ToOne();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sportProjectInfos"></param>
        /// <param name="isOnlyGroup"></param>
        /// <param name="groupName"></param>
        /// <param name="isAllTest"></param>
        /// <returns></returns>
        public bool OutPutScore(SportProjectInfos sportProjectInfos, bool isOnlyGroup, string groupName,bool  isAllTest)
        {
            bool isResult = false;
            try
            {
                if(sportProjectInfos == null)
                {
                    UIMessageBox.ShowError("项目设置有误！！");
                    return false;
                }
                SaveFileDialog saveImageDialog= new SaveFileDialog();
                saveImageDialog.Title = "导出成绩";
                saveImageDialog.Filter = "xlsx file(*.xlsx)|*.xlsx";
                saveImageDialog.RestoreDirectory = true;
                string path = Application.StartupPath + $"\\excel\\output{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                //saveImageDialog.FileName = $"output_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                if (isOnlyGroup)
                { 
                    saveImageDialog.FileName = $"{sportProjectInfos.Name}_{groupName}组成绩.xlsx";
                }
                else
                { 
                    saveImageDialog.FileName = $"{sportProjectInfos.Name}_成绩.xlsx";
                }
                if (saveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadingWindowSys.Instance.ShowLoadingWindow();
                    path=saveImageDialog.FileName ;
                    List<Dictionary<string ,string >> ldic = new List<Dictionary<string, string>> ();
                    List<DbPersonInfos> dbPersonInfos = new List<DbPersonInfos> (); 
                    if(isOnlyGroup)
                    {
                        dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == groupName).ToList();
                    }
                    else
                    {
                        dbPersonInfos = freeSql.Select<DbPersonInfos>().ToList();
                    }
                    List<OutPutExcelData> outPutExcelDatas= new List<OutPutExcelData> ();
                    int step = 1;
                    bool isBestScore = sportProjectInfos.BestScoreMode == 0;
                    foreach (var dpInfo in dbPersonInfos)
                    {
                        List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>().Where(a => a.PersonId == dpInfo.Id.ToString() && a.IsRemoved == 0).ToList();
                        if (resultInfos.Count == 0 && !isAllTest) continue;
                        OutPutExcelData opd = new OutPutExcelData();
                        opd.Id = step;
                        opd.examTime = dpInfo.CreateTime;
                        opd.School = dpInfo.SchoolName;
                        opd.GradeName = dpInfo.GradeName;
                        opd.ClassName = dpInfo.ClassNumber;
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
                        outPutExcelDatas.Add(opd);
                        step++;
                    }
                    //result = ExcelUtils.OutPutExcel(ldic, path);
                    MiniExcel.SaveAs(path, outPutExcelDatas);
                    isResult = true;
                }
                return isResult;
            }
            catch(Exception ex)
            {
                LoggerHelper.Debug(ex);
                return false;
            }
            finally
            {
                LoadingWindowSys.Instance.SetLoadingWindowClose();
            }
        }
    }
}
