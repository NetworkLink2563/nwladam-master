namespace NWL_AdAm_DML.ViewModel
{
    public class ProjectListDataRowViewModel
    {
        public string projectCode { get; set; } = string.Empty;
        public string projectName { get; set; } = string.Empty;
        public string projectType { get; set; } = string.Empty;
        public string projectTypeName
        {
            get
            {
                switch (projectType)
                {
                    case "1":
                        return "LoRa";
                    case "2":
                        return "EMM";
                    case "3":
                        return "NB Node";
                    default:
                        return this.projectType;
                }
            }
        }
    }

    public class ProjectInfoViewModel
    {
        public string projectCode { get; set; } = string.Empty;
        public string projectName { get; set; } = string.Empty;
        public string projectAddress { get; set; } = string.Empty;
        public string projectType { get; set; } = string.Empty;
        public string customerCode { get; set; } = string.Empty;
        public string subDistrictCode { get; set; } = string.Empty;
        public string lineToken1 { get; set; } = string.Empty;
        public string lineToken2 { get; set; } = string.Empty;
        public string districtCode { get; set; } = string.Empty;
        public string provinceCode { get; set; } = string.Empty;

    }

}
