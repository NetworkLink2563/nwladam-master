using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWL_AdAm_DML.ViewModel
{
    public class MapsViewModel
    {
        public List<ProjectListMapsViewModel> projects = new List<ProjectListMapsViewModel>();
    }


    public class ProjectListMapsViewModel
    {
        public string projectCode { get; set; } = string.Empty;
        public string projectName { get; set; } = string.Empty;
        public string projectType { get; set; } = string.Empty;
        //public string latitude { get; set; } = string.Empty;
        //public string longitude { get; set; } = string.Empty;
        public List<ControllerListMapsViewModel> controllers = new List<ControllerListMapsViewModel>();
    }

    public class ControllerListMapsViewModel
    {
        public string controllerCode { get; set; } = string.Empty;
        public string controllerName { get; set; } = string.Empty;
        public string latitude { get; set; } = string.Empty;
        public string longitude { get; set; } = string.Empty;
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
        public List<LampListMapsViewModel> lamps = new List<LampListMapsViewModel>();

    }

    public class LampListMapsViewModel
    {
        public string lampCode { get; set; } = string.Empty;
        public string lampName { get; set; } = string.Empty;
        public string latitude { get; set; } = string.Empty;
        public string longitude { get; set; } = string.Empty;
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
}
