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
    public class TMstMLampDao
    {
        public List<TMstMLamp> getLampList()
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.TMstMLamp.ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TMstMLampExtModel getLampByCode(string lampCode)
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("SELECT");
                query.AppendLine("  Lmp.XVLmpCode, Lmp.XVLmpName, Lmp.XVLmpDescription, Lmp.XVLmpSN");
                query.AppendLine("  , Lmp.XVCtlCode, Lmp.XVLmpLat, Lmp.XVLmpLong, Lmp.XBLmpIsWarning");
                query.AppendLine("  , Cst.XVCstCode");
                query.AppendLine("FROM TMstMLamp Lmp");
                query.AppendLine("INNER JOIN TMstMController Ctl ON Ctl.XVCtlCode = Lmp.XVCtlCode");
                query.AppendLine("INNER JOIN TMstMProject Prj ON Prj.XVPrjCode = Ctl.XVPrjCode");
                query.AppendLine("INNER JOIN TMstMCustomer Cst ON Cst.XVCstCode = Prj.XVCstCode");
                query.AppendLine("WHERE Lmp.XVLmpCode = '" + lampCode + "'");

                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.Database.SqlQuery<TMstMLampExtModel>(query.ToString()).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TMstMLampExtModel> getLampListByController(object controllerCode, string customerCode, string searchText)
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("SELECT");
                query.AppendLine("  Lmp.XVLmpCode, Lmp.XVLmpName, Lmp.XVLmpDescription, Lmp.XVLmpSN");
                query.AppendLine("  , Lmp.XVCtlCode, Lmp.XVLmpLat, Lmp.XVLmpLong, Lmp.XBLmpIsWarning");
                query.AppendLine("  , Cst.XVCstCode, Prj.XVPrjCode");
                query.AppendLine("FROM TMstMLamp Lmp");
                query.AppendLine("INNER JOIN TMstMController Ctl ON Ctl.XVCtlCode = Lmp.XVCtlCode");
                query.AppendLine("INNER JOIN TMstMProject Prj ON Prj.XVPrjCode = Ctl.XVPrjCode");
                query.AppendLine("INNER JOIN TMstMCustomer Cst ON Cst.XVCstCode = Prj.XVCstCode");
                query.AppendLine("                          AND Cst.XVCstCode = '" + customerCode + "'");
                query.AppendLine("WHERE Lmp.XVCtlCode = '" + controllerCode + "'");

                if (!string.IsNullOrEmpty(searchText))
                {
                    query.AppendLine("AND (");
                    query.AppendLine("          Lmp.XVLmpCode LIKE '%" + searchText + "%'");
                    query.AppendLine("          OR Lmp.XVLmpName LIKE '%" + searchText + "%'");
                    query.AppendLine("          OR Lmp.XVLmpDescription LIKE '%" + searchText + "%'");
                    query.AppendLine("          OR Lmp.XVLmpSN LIKE '%" + searchText + "%'");
                    query.AppendLine(")");
                }

                query.AppendLine("ORDER BY Lmp.XTWhenCreate ASC");

                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.Database.SqlQuery<TMstMLampExtModel>(query.ToString()).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool checkExistsLampCode(string lampCode)
        {
            string existsLampCode = string.Empty;

            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    existsLampCode = db.TMstMLamp
                    .Where(it => it.XVLmpCode == lampCode)
                    .Select(it => it.XVLmpCode)
                    .FirstOrDefault();
                }

                if (string.IsNullOrEmpty(existsLampCode))
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
                existsLampCode = null;
            }
        }

        public bool createLamp(TMstMLamp newLampModel)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    db.TMstMLamp.Add(newLampModel);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool updateLamp(TMstMLamp lampModel)
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("UPDATE TMstMLamp");
                query.AppendLine("SET");
                query.AppendLine("  XVLmpName = '" + lampModel.XVLmpName + "'");
                query.AppendLine("  , XVLmpDescription = '" + lampModel.XVLmpDescription + "'");
                query.AppendLine("  , XVLmpSN = '" + lampModel.XVLmpSN + "'");
                query.AppendLine("  , XVLmpLat = '" + lampModel.XVLmpLat + "'");
                query.AppendLine("  , XVLmpLong = '" + lampModel.XVLmpLong + "'");
                query.AppendLine("  , XBLmpIsWarning = '" + Converter.convertBoolToString(lampModel.XBLmpIsWarning) + "'");
                query.AppendLine("  , XVWhoEdit = '" + lampModel.XVWhoEdit + "'");
                query.AppendLine("  , XTWhenEdit = GETDATE()");
                query.AppendLine("WHERE XVLmpCode = '" + lampModel.XVLmpCode + "'");

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

        public bool deleteLamp(string lampCode)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    TMstMLamp lampData = db.TMstMLamp.Find(lampCode);
                    if (lampData != null)
                    {
                        db.TMstMLamp.Remove(lampData);
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

        public V_LampStatus getLampStatus(string lampCode)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.V_LampStatus.Where(it => it.XVLmpCode == lampCode).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
