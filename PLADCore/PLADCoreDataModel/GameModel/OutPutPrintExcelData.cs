using MiniExcelLibs.Attributes;

namespace PLADCoreDataModel.GameModel
{
    public class OutPutPrintExcelData
    {
        [ExcelColumnName("序号")]
        public int Id { get; set; }

        [ExcelColumnName("日期")]
        public string examTime { get; set; }

        [ExcelColumnName("学校")]
        public string School { get; set; }

        [ExcelColumnName("姓名")]
        public string Name { get; set; }

        [ExcelColumnName("性别")]
        public string Sex { get; set; }

        [ExcelColumnName("准考证号")]
        public string IdNumber { get; set; }

        [ExcelColumnName("组别")]
        public string GroupName { get; set; }

        [ExcelColumnName("最终成绩")]
        public string Result { get; set; }
    }
}