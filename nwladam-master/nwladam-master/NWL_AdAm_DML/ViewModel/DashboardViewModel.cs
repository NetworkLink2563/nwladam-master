using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWL_AdAm_DML.ViewModel
{
    public class LampStatusDashboardModel
    {
        public string projectCode { get; set; } = string.Empty;
        public string projectName { get; set; } = string.Empty;
        public int lampOn { get; set; } = 0;
        public int lampOff { get; set; } = 0;
        public int lampUnreachable { get; set; } = 0;
    }

    public class JobStatusDashboardModel
    {
        public string projectCode { get; set; } = string.Empty;
        public string projectName { get; set; } = string.Empty;
        public int completedJob { get; set; } = 0;
        public int notCompletedJob { get; set; } = 0;
    }

    public class EMMStatusDashboardModel
    {
        public string projectCode { get; set; } = string.Empty;
        public string projectName { get; set; } = string.Empty;
        public string controllerSerialNo { get; set; } = string.Empty;
        public double current { get; set; } = 0.0;
        public double voltage { get; set; } = 0.0;
        public double power { get; set; } = 0.0;
    }

    public class WeatherDashboardModel
    {
        public string imgPath { get; set; } = string.Empty;
        public string weatherStatus { get; set; } = string.Empty;
        public string lowTemp { get; set; } = string.Empty;
        public string highTemp { get; set; } = string.Empty;
    }

    public class GeoLocationModel
    {
        public string latitude { get; set; } = string.Empty;
        public string longitude { get; set; } = string.Empty;
    }
}
