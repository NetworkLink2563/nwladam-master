using System;

namespace NWL_AdAm_DML.ViewModel
{
    public class JobViewModel
    {
        public string searchText { get; set; } = string.Empty;
    }

    public class JobListDataRowViewModel
    {
        public string jobDocNo { get; set; } = string.Empty;
        public string docStatus { get; set; } = string.Empty;
        public string docStatusText
        {
            get
            {
                switch (docStatus)
                {
                    case "1":
                        return "รอแก้ไข";
                    case "2":
                        return "สำเร็จ";
                    default:
                        return docStatus;
                }
            }
        }
        public string jobProblem { get; set; } = string.Empty;
        public string jobResolve { get; set; } = string.Empty;
        public string problemTime { get; set; } = string.Empty;
        public string projectName { get; set; } = string.Empty;
    }

    public class JobInfoViewModel
    {
        public string jobDocNo { get; set; } = string.Empty;
        public DateTime jobDocDate { get; set; } = DateTime.MinValue;
        public string docStatus { get; set; } = string.Empty;
        public string docStatusText
        {
            get
            {
                switch (docStatus)
                {
                    case "1":
                        return "รอแก้ไข";
                    case "2":
                        return "สำเร็จ";
                    default:
                        return docStatus;
                }
            }
        }
        public string projectCode { get; set; } = string.Empty;
        public string controllerCode { get; set; } = string.Empty;
        public string lampCode { get; set; } = string.Empty;
        public string jobProblem { get; set; } = string.Empty;
        public string jobLocation { get; set; } = string.Empty;
        public DateTime problemTime { get; set; } = DateTime.MinValue;
        public string jobResolve { get; set; } = string.Empty;
        public DateTime? resolveTime { get; set; } = null;
        public bool? isComplete { get; set; } = null;
        public string engineerName { get; set; } = string.Empty;
    }
}
