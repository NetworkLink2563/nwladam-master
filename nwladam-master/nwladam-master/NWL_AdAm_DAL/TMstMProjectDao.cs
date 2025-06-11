using NWL_AdAm_DML;
using NWL_AdAm_DML.ViewModel;
using NWL_AdAm_DML.ViewModel.Extended;
using NWL_AdAm_SPL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace NWL_AdAm_DAL
{
    public class TMstMProjectDao
    {
        public List<TMstMProject> getProjectList(string customerCode)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.TMstMProject.Where(it => it.XVCstCode == customerCode).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TMstMProjectExtModel getProjectByCode(string projectCode)
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("SELECT");
                query.AppendLine("  Prj.XVPrjCode, Prj.XVPrjName, Prj.XVPrjAddress, Prj.XVPrjType");
                query.AppendLine("  , Prj.XVCstCode, Prj.XVSdtCode, Prj.XVPrjLineToken1, Prj.XVPrjLineToken2");
                query.AppendLine("  , Dst.XVDstCode, Pvn.XVPvnCode");
                query.AppendLine("FROM TMstMProject Prj");
                query.AppendLine("INNER JOIN TSysSSubDistrict Sdt ON Sdt.XVSdtCode = Prj.XVSdtCode");
                query.AppendLine("INNER JOIN TSysSDistrict Dst ON Dst.XVDstCode = Sdt.XVDstCode");
                query.AppendLine("INNER JOIN TSysSProvince Pvn ON Pvn.XVPvnCode = Dst.XVPvnCode");
                query.AppendLine("WHERE Prj.XVPrjCode = '" + projectCode + "'");

                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.Database.SqlQuery<TMstMProjectExtModel>(query.ToString()).FirstOrDefault();
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

        public bool checkExistsProjectCode(string projectCode)
        {
            string existsProjectCode = string.Empty;

            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    existsProjectCode = db.TMstMProject
                    .Where(it => it.XVPrjCode == projectCode)
                    .Select(it => it.XVPrjCode)
                    .FirstOrDefault();
                }

                if (string.IsNullOrEmpty(existsProjectCode))
                {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                existsProjectCode = null;
            }
        }

        public bool createProject(TMstMProject newProjectModel)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    db.TMstMProject.Add(newProjectModel);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool updateProject(TMstMProject projectModel)
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("UPDATE TMstMProject");
                query.AppendLine("SET");
                query.AppendLine("  XVPrjName = '" + projectModel.XVPrjName + "'");
                query.AppendLine("  , XVPrjAddress = '" + projectModel.XVPrjAddress + "'");
                query.AppendLine("  , XVPrjType = '" + projectModel.XVPrjType + "'");
                query.AppendLine("  , XVSdtCode = '" + projectModel.XVSdtCode + "'");
                query.AppendLine("  , XVPrjLineToken1 = '" + projectModel.XVPrjLineToken1 + "'");
                query.AppendLine("  , XVPrjLineToken2 = '" + projectModel.XVPrjLineToken2 + "'");
                query.AppendLine("  , XVWhoEdit = '" + projectModel.XVWhoEdit + "'");
                query.AppendLine("  , XTWhenEdit = GETDATE()");
                query.AppendLine("WHERE XVPrjCode = '" + projectModel.XVPrjCode + "'");

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

        public bool deleteProject(string projectCode)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    TMstMProject userData = db.TMstMProject.Find(projectCode);
                    if (projectCode != null)
                    {
                        List<TMstMController> controllers = db.TMstMController.Where(it => it.XVPrjCode == projectCode).ToList();
                        controllers.ForEach(it => {
                            db.TMstMLamp.RemoveRange(db.TMstMLamp.Where(lamp => lamp.XVCtlCode == it.XVCtlCode));
                            db.TMstMController.Remove(it);
                        });

                        db.TMstMProject.Remove(userData);
                        db.SaveChanges();
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string generateProjectCode()
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("SELECT 'PRJ' + CONVERT(VARCHAR(2), GETDATE(), 12) + '-' + RIGHT('00000' + CAST((RIGHT(ISNULL(MAX(XVPrjCode), 0), 5) + 1) AS VARCHAR(5)), 5)");
                query.AppendLine("FROM TMstMProject WITH(NOLOCK)");
                query.AppendLine("WHERE XVPrjCode LIKE 'PRJ' + CONVERT(VARCHAR(4), CONVERT(VARCHAR(2), GETDATE(), 12)) + '-%'");
                query.AppendLine("AND LEN(XVPrjCode) = 11");
                query.AppendLine("AND ISNUMERIC(RIGHT(XVPrjCode, 5)) = 1");

                using (AdAmEntities db = new AdAmEntities())
                {
                    string projectCode = db.Database.SqlQuery<string>(query.ToString()).FirstOrDefault();
                    if (!string.IsNullOrEmpty(projectCode))
                    {
                        return projectCode;
                    }
                    else
                    {
                        throw new Exception("สร้างรหัสโครงการใหม่ไม่สำเร็จ");
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

        public List<TMstMProject> getProjectListByCustomer(string customerCode, string searchText, string projectType)
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("SELECT");
                query.AppendLine("  Prj.*");
                query.AppendLine("FROM TMstMProject Prj");
                query.AppendLine("INNER JOIN TMstMCustomer Cst ON Cst.XVCstCode = Prj.XVCstCode");
                query.AppendLine("WHERE Prj.XVCstCode = '" + customerCode + "'");

                if (!string.IsNullOrEmpty(searchText))
                {
                    query.AppendLine("AND (");
                    query.AppendLine("          Prj.XVPrjCode LIKE '%" + searchText + "%'");
                    query.AppendLine("          OR Prj.XVPrjName LIKE '%" + searchText + "%'");
                    query.AppendLine(")");
                }

                if (!string.IsNullOrEmpty(projectType))
                {
                    query.AppendLine("AND Prj.XVPrjType = '" + projectType + "'");
                }

                query.AppendLine("ORDER BY Prj.XVPrjCode ASC");

                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.Database.SqlQuery<TMstMProject>(query.ToString()).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
