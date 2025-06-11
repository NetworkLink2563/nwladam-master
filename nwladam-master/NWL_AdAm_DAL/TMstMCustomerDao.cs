using NWL_AdAm_DML;
using NWL_AdAm_DML.ViewModel;
using NWL_AdAm_SPL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace NWL_AdAm_DAL
{
    public class TMstMCustomerDao
    {
        private AdAmEntities db;

        public TMstMCustomer getCustomerByCode(string customerCode)
        {
            try
            {
                db = new AdAmEntities();
                return db.TMstMCustomer
                    .Where(it => it.XVCstCode == customerCode)
                    .FirstOrDefault<TMstMCustomer>();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                db.Dispose();
            }
        }
        
    }
}
