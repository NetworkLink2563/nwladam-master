using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWL_AdAm_DML.ViewModel
{

    public class ProvinceListViewModel
    {
        public List<ProvinceItemViewModel> data { get; set; }
        public string error { get; set; } = string.Empty;
    }

    public class ProvinceItemViewModel
    {
        public string provinceCode { get; set; } = string.Empty;
        public string provinceNameTH { get; set; } = string.Empty;
    }

    public class DistrictListViewModel
    {
        public List<DistrictItemViewModel> data { get; set; }
        public string error { get; set; } = string.Empty;
    }

    public class DistrictItemViewModel
    {
        public string districtCode { get; set; } = string.Empty;
        public string districtNameTH { get; set; } = string.Empty;
        public string provinceCode { get; set; } = string.Empty;
    }

    public class SubDistrictListViewModel
    {
        public List<SubDistrictItemViewModel> data { get; set; }
        public string error { get; set; } = string.Empty;
    }

    public class SubDistrictItemViewModel
    {
        public string subDistrictCode { get; set; } = string.Empty;
        public string subDistrictNameTH { get; set; } = string.Empty;
        public string postCode { get; set; } = string.Empty;
        public string districtCode { get; set; } = string.Empty;
    }
}
