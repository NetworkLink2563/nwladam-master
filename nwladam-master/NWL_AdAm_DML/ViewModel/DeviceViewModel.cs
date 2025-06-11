namespace NWL_AdAm_DML.ViewModel
{
    public class DeviceListDataRowViewModel
    {
        public string controllerCode { get; set; } = string.Empty;
        public string controllerName { get; set; } = string.Empty;
        public int controllerStatus { get; set; }
        public string controllerStatusText
        {
            get
            {
                switch (controllerStatus)
                {
                    case 0:
                        return "ไม่เชื่อมต่อ";
                    case 1:
                        return "เชื่อมต่อ";
                    default:
                        return "n/a";
                }
            }
        }
    }

    public class DeviceInfoViewModel
    {
        public string controllerCode { get; set; } = string.Empty;
        public string controllerName { get; set; } = string.Empty;
        public string controllerDescription { get; set; } = string.Empty;
        public string controllerSerialNo { get; set; } = string.Empty;
        public string latitude { get; set; } = string.Empty;
        public string longitude { get; set; } = string.Empty;
        public string projectCode { get; set; } = string.Empty;
        public string customerCode { get; set; } = string.Empty;
        public string controllerPhone { get; set; } = string.Empty;
        public int lampQty { get; set; } = 0;
        public double ampMin { get; set; } = 0.0;
        public double ampMax { get; set; } = 0.0;
        public string boxNo { get; set; } = string.Empty;
        public string boxKmFrom { get; set; } = string.Empty;
        public string boxKmTo { get; set; } = string.Empty;
    }
}
