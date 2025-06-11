using NWL_AdAm_DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWL_AdAm_DAL
{
    public class TSysSDistrictDao
    {
        public List<TSysSDistrict> getDistrictByProvinceCode(string provinceCode)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.TSysSDistrict
                        .Where(it => it.XVPvnCode == provinceCode)
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
