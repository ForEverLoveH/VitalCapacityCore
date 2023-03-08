namespace PLADCore.GameSystem 
{
    public class MachineMsgCode
    {
        //“type”:1,”mac”:”xxxxxxxxxxxx”,”itemtype”:” SGTZ”,”code”:1
        public int type;
        public string mac;
        public string itemtype;
        public int code;
        public int fhl_result;//身高成绩
        public int sg_result;//肺活量成绩
        public int tz_result;//体重成绩
        public int bmi_result;//BMI成绩
    }
    public class SerialMsg
    {
        public MachineMsgCode mms;
        public int number;
        public SerialMsg(MachineMsgCode m,int n)
        {
            mms = m;
            number = n;
        }
    }
}