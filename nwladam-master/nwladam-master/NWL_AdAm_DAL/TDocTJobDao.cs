using NWL_AdAm_DML;
using NWL_AdAm_DML.ViewModel.Extended;
using NWL_AdAm_SPL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWL_AdAm_DAL
{
    public class TDocTJobDao
    {
        public TDocTJobExtModel getJobByCode(string jobCode)
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("SELECT");
                query.AppendLine("  Job.XVJobDocNo, Job.XDJobDocDate, Job.XVJobDocStatus, Job.XVCstCode");
                query.AppendLine("  , Job.XVPrjCode, Job.XVCtlCode, Job.XVLmpCode");
                query.AppendLine("  , Job.XVJobProblem, Job.XVJobLocation, Job.XTJobProblemTime");
                query.AppendLine("  , Job.XVJobResolve, Job.XTJobResolveTime, Job.XBJobIsComplete");
                query.AppendLine("  , Eng.XVUsrName");
                query.AppendLine("FROM TDocTJob Job");
                query.AppendLine("INNER JOIN TMstMUser Eng ON Eng.XVUsrCode = Job.XVWhoEdit");
                query.AppendLine("WHERE Job.XVJobDocNo = '" + jobCode + "'");

                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.Database.SqlQuery<TDocTJobExtModel>(query.ToString()).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string generateJobDocNo()
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("SELECT 'JB' + CONVERT(VARCHAR(4), GETDATE(), 12) + '-' + RIGHT('00000' + CAST((RIGHT(ISNULL(MAX(XVJobDocNo), 0), 6) + 1) AS VARCHAR(6)), 6)");
                query.AppendLine("FROM TDocTJob WITH(NOLOCK)");
                query.AppendLine("WHERE XVJobDocNo LIKE 'JB' + CONVERT(VARCHAR(4), CONVERT(VARCHAR(4), GETDATE(), 12)) + '-%'");
                query.AppendLine("AND LEN(XVJobDocNo) = 13");
                query.AppendLine("AND ISNUMERIC(RIGHT(XVJobDocNo, 6)) = 1");

                using (AdAmEntities db = new AdAmEntities())
                {
                    string jobDocNo = db.Database.SqlQuery<string>(query.ToString()).FirstOrDefault();
                    if (!string.IsNullOrEmpty(jobDocNo))
                    {
                        return jobDocNo;
                    }
                    else
                    {
                        throw new Exception("สร้างเลขที่งานแจ้งซ่อมใหม่ไม่สำเร็จ");
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                query = null;
            }
        }

        public List<TDocTJobExtModel> getJobListByCustomerCode(string customerCode, string searchText, string projectCode)
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("SELECT");
                query.AppendLine("  Job.XVJobDocNo, Job.XDJobDocDate, Job.XVJobDocStatus");
                query.AppendLine("  , Job.XVJobProblem, Job.XTJobProblemTime, Job.XVJobResolve");
                query.AppendLine("  , Prj.XVPrjName");
                query.AppendLine("FROM TDocTJob Job");
                query.AppendLine("LEFT JOIN TMstMProject Prj ON Prj.XVPrjCode = Job.XVPrjCode");
                query.AppendLine("LEFT JOIN TMstMController Ctl ON Ctl.XVCtlCode = Job.XVCtlCode");
                query.AppendLine("LEFT JOIN TMstMLamp Lmp ON Lmp.XVLmpCode = Job.XVLmpCode");
                query.AppendLine("WHERE Job.XVCstCode = '" + customerCode + "'");
                
                if (!string.IsNullOrEmpty(searchText))
                {
                    query.AppendLine("AND (");
                    query.AppendLine("          Job.XVJobDocNo LIKE '%" + searchText + "%'");
                    query.AppendLine("          Or CONVERT(VARCHAR(10), Job.XTJobProblemTime, 111) LIKE '%" + searchText + "%'");
                    query.AppendLine("          Or ISNULL(Prj.XVPrjName, '') LIKE '%" + searchText + "%'");
                    query.AppendLine("          Or Job.XVCtlCode LIKE '%" + searchText + "%'");
                    query.AppendLine("          Or ISNULL(Ctl.XVCtlName, '') LIKE '%" + searchText + "%'");
                    query.AppendLine("          Or Job.XVLmpCode LIKE '%" + searchText + "%'");
                    query.AppendLine("          Or ISNULL(Lmp.XVLmpName, '') LIKE '%" + searchText + "%'");
                    query.AppendLine("          Or Job.XVJobProblem LIKE '%" + searchText + "%'");
                    query.AppendLine(")");
                }

                if (!string.IsNullOrEmpty(projectCode))
                {
                    query.AppendLine("AND Job.XVPrjCode = '" + projectCode + "'");
                }

                query.AppendLine("ORDER BY Job.XVJobDocStatus ASC, Job.XTJobProblemTime DESC");

                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.Database.SqlQuery<TDocTJobExtModel>(query.ToString()).ToList();
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public bool createJob(TDocTJob newJobModel)
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("INSERT INTO TDocTJob");
                query.AppendLine("(");
                query.AppendLine("	XVJobDocNo");
                query.AppendLine("	, XDJobDocDate");
                query.AppendLine("	, XVJobDocStatus");
                query.AppendLine("	, XVCstCode");
                query.AppendLine("	, XVPrjCode");
                query.AppendLine("	, XVCtlCode");
                query.AppendLine("	, XVLmpCode");
                query.AppendLine("	, XVJobProblem");
                query.AppendLine("	, XVJobLocation");
                query.AppendLine("	, XTJobProblemTime");
                query.AppendLine("	, XVJobResolve");
                query.AppendLine("	, XTJobResolveTime");
                query.AppendLine("	, XBJobIsComplete");
                query.AppendLine("	, XVWhoCreate");
                query.AppendLine("	, XVWhoEdit");
                query.AppendLine("	, XTWhenCreate");
                query.AppendLine("	, XTWhenEdit");
                query.AppendLine(")");
                query.AppendLine("VALUES");
                query.AppendLine("(");
                query.AppendLine("	'" + newJobModel.XVJobDocNo + "'");
                query.AppendLine("	, '" + newJobModel.XDJobDocDate.Value.ToString("yyyyMMdd") + "'");
                query.AppendLine("	, '" + newJobModel.XVJobDocStatus + "'");
                query.AppendLine("	, '" + newJobModel.XVCstCode + "'");
                query.AppendLine("	, '" + newJobModel.XVPrjCode + "'");
                query.AppendLine("	, '" + newJobModel.XVCtlCode + "'");
                query.AppendLine("	, '" + newJobModel.XVLmpCode + "'");
                query.AppendLine("	, '" + newJobModel.XVJobProblem + "'");
                query.AppendLine("	, '" + newJobModel.XVJobLocation + "'");
                query.AppendLine("	, '" + newJobModel.XTJobProblemTime.Value.ToString("yyyyMMdd HH:mm:ss") + "'");
                query.AppendLine("	, NULL");
                query.AppendLine("	, NULL");
                query.AppendLine("	, NULL");
                query.AppendLine("	, '" + newJobModel.XVWhoCreate + "'");
                query.AppendLine("	, '" + newJobModel.XVWhoEdit + "'");
                query.AppendLine("	, GETDATE()");
                query.AppendLine("	, GETDATE()");
                query.AppendLine(")");

                using (AdAmEntities db = new AdAmEntities())
                {
                    int affectedRow = db.Database.ExecuteSqlCommand(query.ToString());
                    if (affectedRow > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }   
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool updateJob(TDocTJob jobModel)
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("UPDATE TDocTJob");
                query.AppendLine("SET");
                query.AppendLine("  XVJobDocStatus = '" + jobModel.XVJobDocStatus + "'");
                query.AppendLine("  , XVJobResolve = '" + jobModel.XVJobResolve + "'");
                query.AppendLine("  , XTJobResolveTime = '" + jobModel.XTJobResolveTime.Value.ToString("yyyyMMdd HH:mm:ss") + "'");
                query.AppendLine("  , XBJobIsComplete = '" + Converter.convertBoolToString(jobModel.XBJobIsComplete.Value) + "'");
                query.AppendLine("  , XVWhoEdit = '" + jobModel.XVWhoEdit + "'");
                query.AppendLine("  , XTWhenEdit = GETDATE()");
                query.AppendLine("WHERE XVJobDocNo = '" + jobModel.XVJobDocNo + "'");

                using (AdAmEntities db = new AdAmEntities())
                {
                    int affectedRow = db.Database.ExecuteSqlCommand(query.ToString());
                    if (affectedRow > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                query = null;
            }
        }
    }
}
