using NWL_AdAm_DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWL_AdAm_DAL
{
    public class TSysSProvinceDao
    {
        public List<TSysSProvince> getProvinceList()
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.TSysSProvince.ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
