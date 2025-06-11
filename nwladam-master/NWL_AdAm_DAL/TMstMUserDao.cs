using NWL_AdAm_DML;
using NWL_AdAm_DML.ViewModel;
using NWL_AdAm_SPL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using X.PagedList;

namespace NWL_AdAm_DAL
{
    public class TMstMUserDao
    {        
        public TMstMUser getUserByCode(string userCode)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.TMstMUser
                    .Where(it => it.XVUsrCode == userCode)
                    .FirstOrDefault();
                }
            } 
            catch(Exception e)
            {
                throw e;
            }
        }

        public List<TMstMUser> getUserListByCustomer(string customerCode, string searchText)
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("SELECT");
                query.AppendLine("  Usr.*");
                query.AppendLine("FROM TMstMUser Usr");
                query.AppendLine("WHERE Usr.XVCstCode = '" + customerCode + "'");

                if (!string.IsNullOrEmpty(searchText))
                {
                    query.AppendLine("AND (");
                    query.AppendLine("          Usr.XVUsrCode LIKE '%" + searchText + "%'");
                    query.AppendLine("          OR Usr.XVUsrName LIKE '%" + searchText + "%'");
                    query.AppendLine(")");
                }

                query.AppendLine("ORDER BY Usr.XTWhenCreate ASC");

                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.Database.SqlQuery<TMstMUser>(query.ToString()).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool checkExistsUserCode(string userCode)
        {
            string existsUserCode = string.Empty;

            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    existsUserCode = db.TMstMUser
                    .Where(it => it.XVUsrCode == userCode)
                    .Select(it => it.XVUsrCode)
                    .FirstOrDefault();
                }

                if (string.IsNullOrEmpty(existsUserCode))
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
                existsUserCode = null;
            }
        }

        public bool createUser(TMstMUser newUserModel)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    db.TMstMUser.Add(newUserModel);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool updateUser(TMstMUser userModel)
        {
            StringBuilder query = new StringBuilder();

            try
            {
                query.AppendLine("UPDATE TMstMUser");
                query.AppendLine("SET");
                query.AppendLine("  XVUsrPwd = '" + userModel.XVUsrPwd + "'");
                query.AppendLine("  , XVUsrName = '" + userModel.XVUsrName + "'");
                query.AppendLine("  , XVCstRol = '" + userModel.XVCstRol + "'");
                query.AppendLine("  , XVWhoEdit = '" + userModel.XVWhoEdit + "'");
                query.AppendLine("  , XTWhenEdit = GETDATE()");
                query.AppendLine("WHERE XVUsrCode = '" + userModel.XVUsrCode + "'");

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

        public bool deleteUser(string userCode)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    TMstMUser userData = db.TMstMUser.Find(userCode);
                    if (userCode != null)
                    {
                        db.TMstMUser.Remove(userData);
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
    }
}
