using MiniExcelLibs.Attributes;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLADCoreDataModel.GameModel
{
    public class ExcelUtils
    {

        public class Test
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
        }
        

        /// <summary>
        /// 芯片导入模板
        /// </summary>
        public class InputChipData
        {
            [ExcelColumnName("序号")]
            public int Id { get; set; }
            [ExcelColumnName("芯片标签号码")]
            public string ChipLabel { get; set; }
            [ExcelColumnName("芯片内部编号")]
            public string ChipNO { get; set; }
            [ExcelColumnName("组号")]
            public string GroupName { get; set; }

        }

        public static List<InputData> ReadExcel_mini(string path)
        {
            var rows = MiniExcel.Query<InputData>(path).ToList();

            return rows;

        }
        private IEnumerable<Dictionary<string, object>> GetIEnumberAble(InputData id)
        {
            var newCompanyPrepareds = new Dictionary<string, object>();
            newCompanyPrepareds.Add("序号", id.Id);
            newCompanyPrepareds.Add("学校", id.School);
            newCompanyPrepareds.Add("年级", id.GradeName);
            newCompanyPrepareds.Add("班级", id.ClassName);
            newCompanyPrepareds.Add("姓名", id.Name);

            newCompanyPrepareds.Add("性别", id.Sex);
            newCompanyPrepareds.Add("准考证号", id.IdNumber);
            newCompanyPrepareds.Add("组别名称", id.GroupName);

            yield return newCompanyPrepareds;
        }

        private IEnumerable<Dictionary<string, object>> GetOrders(List<InputData> test)
        {
            foreach (var item in test)
            {
                var newCompanyPrepareds = new Dictionary<string, object>();
                newCompanyPrepareds.Add("序号", item.Id);
                newCompanyPrepareds.Add("学校", item.School);
                newCompanyPrepareds.Add("年级", item.GradeName);
                newCompanyPrepareds.Add("班级", item.ClassName);
                newCompanyPrepareds.Add("姓名", item.Name);

                newCompanyPrepareds.Add("性别", item.Sex);
                newCompanyPrepareds.Add("准考证号", item.IdNumber);
                newCompanyPrepareds.Add("组别名称", item.GroupName);

                yield return newCompanyPrepareds;

            }
        }

        /// <summary>
        /// MiniExcel导出格式
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        public static void MiniExcel_OutPutExcel(string path, object value)
        {
            MiniExcel.SaveAs(path, value);
        }


        public class Supply
        {
            public string Value1 { get; set; }
            public string Value2 { get; set; }
            public string Value3 { get; set; }
        }
    }
}

