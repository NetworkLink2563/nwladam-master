namespace NWL_AdAm_DML.ViewModel
{
    //public class LampViewModel : BaseViewModel
    //{
    //    public string projectCode { get; set; } = string.Empty;
    //    public string controllerCode { get; set; } = string.Empty;
    //}

    public class LampListDataRowViewModel
    {
        public string lampCode { get; set; } = string.Empty;
        public string lampName { get; set; } = string.Empty;
        public int lampStatus { get; set; }
        public string lampStatusText
        {
            get
            {
                switch (lampStatus)
                {
                    case 0:
                        return "ปิด";
                    case 1:
                        return "เปิด";
                    case 2:
                        return "ไม่เชื่อมต่อ";
                    default:
                        return "n/a";
                }
            }
        }
    }

    public class LampInfoViewModel
    {
        public string lampCode { get; set; } = string.Empty;
        public string lampName { get; set; } = string.Empty;
        public string lampDescription { get; set; } = string.Empty;
        public string lampSerialNo { get; set; } = string.Empty;
        public string controllerCode { get; set; } = string.Empty;
        public string latitude { get; set; } = string.Empty;
        public string longitude { get; set; } = string.Empty;
        public bool? allowNotify { get; set; } = false;
        public string customerCode { get; set; } = string.Empty;
    }
}
