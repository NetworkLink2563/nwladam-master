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
    public class TMstMControllerDao
    {
        public List<TMstMController> getControllerList()
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.TMstMController.ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string generateControllerCode()
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("SELECT 'CTL' + CONVERT(VARCHAR(2), GETDATE(), 12) + '-' + RIGHT('00000' + CAST((RIGHT(ISNULL(MAX(XVCtlCode), 0), 5) + 1) AS VARCHAR(5)), 5)");
                query.AppendLine("FROM TMstMController WITH(NOLOCK)");
                query.AppendLine("WHERE XVCtlCode LIKE 'CTL' + CONVERT(VARCHAR(4), CONVERT(VARCHAR(2), GETDATE(), 12)) + '-%'");
                query.AppendLine("AND LEN(XVCtlCode) = 11");
                query.AppendLine("AND ISNUMERIC(RIGHT(XVCtlCode, 5)) = 1");

                using (AdAmEntities db = new AdAmEntities())
                {
                    string controllerCode = db.Database.SqlQuery<string>(query.ToString()).FirstOrDefault();
                    if (!string.IsNullOrEmpty(controllerCode))
                    {
                        return controllerCode;
                    }
                    else
                    {
                        throw new Exception("สร้างรหัสกล่องควบคุมใหม่ไม่สำเร็จ");
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

        public TMstMControllerExtModel getControllerByCode(string controllerCode)
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("SELECT");
                query.AppendLine("  Ctl.XVCtlCode, Ctl.XVCtlName, Ctl.XVCtlDescription, Ctl.XVCtlSN");
                query.AppendLine("  , Ctl.XVPrjCode, Ctl.XVCtlLat, Ctl.XVCtlLong");
                query.AppendLine("  , Ctl.XVCtlPhone, Ctl.XICtlLampQty, Ctl.XFCtlAmpMin, Ctl.XFCtlAmpMax");
                query.AppendLine("  , Ctl.XVBoxNo, Ctl.XVBoxKmFrom, Ctl.XVBoxKmTo");
                query.AppendLine("  , Prj.XVPrjName, Prj.XVCstCode");
                query.AppendLine("FROM TMstMController Ctl");
                query.AppendLine("LEFT JOIN TMstMProject Prj ON Prj.XVPrjCode = Ctl.XVPrjCode");
                query.AppendLine("WHERE Ctl.XVCtlCode = '" + controllerCode + "'");

                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.Database.SqlQuery<TMstMControllerExtModel>(query.ToString()).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool checkExistsControllerCode(string controllerCode)
        {
            string existsControllerCode = string.Empty;

            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    existsControllerCode = db.TMstMController
                    .Where(it => it.XVCtlCode == controllerCode)
                    .Select(it => it.XVCtlCode)
                    .FirstOrDefault();
                }

                if (string.IsNullOrEmpty(existsControllerCode))
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
                existsControllerCode = null;
            }
        }

        public bool createController(TMstMController newControllerModel)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    db.TMstMController.Add(newControllerModel);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool updateController(TMstMController controllerModel)
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("UPDATE TMstMController");
                query.AppendLine("SET");
                query.AppendLine("  XVCtlName = '" + controllerModel.XVCtlName + "'");
                query.AppendLine("  , XVCtlDescription = '" + controllerModel.XVCtlDescription + "'");
                query.AppendLine("  , XVCtlSN = '" + controllerModel.XVCtlSN + "'");
                query.AppendLine("  , XVCtlLat = '" + controllerModel.XVCtlLat + "'");
                query.AppendLine("  , XVCtlLong = '" + controllerModel.XVCtlLong + "'");
                query.AppendLine("	, XVCtlPhone = '" + controllerModel.XVCtlPhone + "'");
                query.AppendLine("	, XICtlLampQty = '" + controllerModel.XICtlLampQty + "'");
                query.AppendLine("	, XFCtlAmpMin = '" + controllerModel.XFCtlAmpMin + "'");
                query.AppendLine("	, XFCtlAmpMax = '" + controllerModel.XFCtlAmpMax + "'");
                query.AppendLine("	, XVBoxNo = '" + controllerModel.XVBoxNo + "'");
                query.AppendLine("	, XVBoxKmFrom = '" + controllerModel.XVBoxKmFrom + "'");
                query.AppendLine("	, XVBoxKmTo = '" + controllerModel.XVBoxKmTo + "'");
                query.AppendLine("  , XVWhoEdit = '" + controllerModel.XVWhoEdit + "'");
                query.AppendLine("  , XTWhenEdit = GETDATE()");
                query.AppendLine("WHERE XVCtlCode = '" + controllerModel.XVCtlCode + "'");

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

        public bool deleteController(string controllerCode)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    TMstMController controllerData = db.TMstMController.Find(controllerCode);
                    if (controllerData != null)
                    {
                        List<TMstMLamp> lampData = db.TMstMLamp.Where(it => it.XVCtlCode == controllerCode).ToList();

                        if (lampData.Count > 0)
                        {
                            db.TMstMLamp.RemoveRange(lampData);
                        }

                        db.TMstMController.Remove(controllerData);
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

        public List<TMstMControllerExtModel> getControllerListByProject(string projectCode, string customerCode, string searchText)
        {
            StringBuilder query = new StringBuilder();
            
            try
            {
                query.AppendLine("SELECT");
                query.AppendLine("  Ctl.XVCtlCode, Ctl.XVCtlName, Cst.XVCstCode");
                query.AppendLine("  , Ctl.XVPrjCode");
                query.AppendLine("  , Prj.XVPrjType");
                query.AppendLine("FROM TMstMController Ctl");
                query.AppendLine("INNER JOIN TMstMProject Prj ON Prj.XVPrjCode = Ctl.XVPrjCode");
                query.AppendLine("INNER JOIN TMstMCustomer Cst ON Cst.XVCstCode = Prj.XVCstCode");
                query.AppendLine("                          AND Cst.XVCstCode = '" + customerCode + "'");
                query.AppendLine("WHERE Ctl.XVPrjCode = '" + projectCode + "'");

                if (!string.IsNullOrEmpty(searchText))
                {
                    query.AppendLine("AND (");
                    query.AppendLine("          Ctl.XVCtlCode LIKE '%" + searchText + "%'");
                    query.AppendLine("          OR Ctl.XVCtlName LIKE '%" + searchText + "%'");
                    query.AppendLine("          OR Ctl.XVCtlDescription LIKE '%" + searchText + "%'");
                    query.AppendLine("          OR Ctl.XVCtlSN LIKE '%" + searchText + "%'");
                    query.AppendLine(")");
                }

                query.AppendLine("ORDER BY Ctl.XTWhenCreate ASC");

                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.Database.SqlQuery<TMstMControllerExtModel>(query.ToString()).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
