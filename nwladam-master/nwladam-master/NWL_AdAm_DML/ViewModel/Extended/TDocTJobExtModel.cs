using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWL_AdAm_DML.ViewModel.Extended
{
    public class TDocTJobExtModel: TDocTJob
    {
        public string XVUsrName { get; set; } = string.Empty;
        public string XVPrjName { get; set; } = string.Empty;
    }
}
