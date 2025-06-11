using NWL_AdAm_DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWL_AdAm_DAL
{
    public class TSysSSubDistrictDao
    {
        public List<TSysSSubDistrict> getSubDistrictByDistrictCode(string districtCode)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.TSysSSubDistrict
                        .Where(it => it.XVDstCode == districtCode)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
