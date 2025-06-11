using NWL_AdAm_DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWL_AdAm_DAL
{
    public class DashboardDao
    {
        public List<STP_NWLShowOnOffStatus_Result> getLampStatusDashboardData(string customerCode, string projectCode)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.STP_NWLShowOnOffStatus(customerCode, projectCode).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<STP_NWLShowJobStatus_Result> getJobStatusDashboardData(string customerCode, string projectCode)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.STP_NWLShowJobStatus(customerCode, projectCode).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<STP_NWLShowEMMStatus_Result> getEMMStatusDashboardData(string customerCode, string projectCode)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.STP_NWLShowEMMStatus(customerCode, projectCode).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
