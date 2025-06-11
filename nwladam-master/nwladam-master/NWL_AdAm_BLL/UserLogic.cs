using NWL_AdAm_DAL;
using NWL_AdAm_DML;
using NWL_AdAm_DML.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NWL_AdAm_BLL
{
    public class UserLogic
    {
        public List<UserListDataRowViewModel> getUserOnPagedList(string customerCode, string searchText)
        {
            TMstMUserDao userDao = new TMstMUserDao();

            try
            {
                List<TMstMUser> users = userDao.getUserListByCustomer(customerCode, searchText);
                if (users != null)
                {
                    List<UserListDataRowViewModel> result = (from it in users
                                                             select new UserListDataRowViewModel()
                                                             {
                                                                 userCode = it.XVUsrCode,
                                                                 userName = it.XVUsrName,
                                                                 role = it.XVCstRol
                                                             }).ToList();
                    return result;
                }
                else
                {
                    return new List<UserListDataRowViewModel>();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                userDao = null;
            }
        }

        public UserInfoViewModel getUserInfoByCode(string userCode)
        {
            TMstMUserDao userDao = new TMstMUserDao();

            try
            {
                TMstMUser userData = userDao.getUserByCode(userCode);
                if (userData != null)
                {
                    return new UserInfoViewModel
                    {
                        userCode = userData.XVUsrCode,
                        userName = userData.XVUsrName,
                        password = userData.XVUsrPwd,
                        customerCode = userData.XVCstCode,
                        role = userData.XVCstRol
                    };
                }
                else
                {
                    throw new Exception("ไม่มีผู้ใช้ " + userCode + " ในระบบแล้ว");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                userDao = null;
            }
        }

        public bool checkExistsUserCode(string userCode)
        {
            TMstMUserDao userDao = new TMstMUserDao();

            try
            {
                if (string.IsNullOrEmpty(userCode))
                {
                    throw new Exception("กรุุณากรอกข้อมูลให้ครบถ้วน");
                }

                return userDao.checkExistsUserCode(userCode);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                userDao = null;
            }
        }

        public bool createUser(UserInfoViewModel viewModel, string userCode, string customerCode)
        {
            TMstMUserDao userDao = new TMstMUserDao();

            try
            {
                if (string.IsNullOrEmpty(viewModel.userCode) || 
                    string.IsNullOrEmpty(viewModel.userName) || 
                    string.IsNullOrEmpty(viewModel.password) || 
                    string.IsNullOrEmpty(viewModel.role))
                {
                    throw new Exception("กรุณากรอกข้อมูลให้ครบถ้วน");
                }

                if (userDao.checkExistsUserCode(viewModel.userCode))
                {
                    throw new Exception("มีผู้ใช้ " + viewModel.userCode + " ในระบบแล้ว");
                }

                TMstMUser newUserModel = new TMstMUser();
                newUserModel.XVUsrCode = viewModel.userCode;
                newUserModel.XVUsrPwd = viewModel.password;
                newUserModel.XVUsrName = viewModel.userName;
                newUserModel.XVCstCode = customerCode;
                newUserModel.XVCstRol = viewModel.role;
                newUserModel.XVWhoCreate = userCode;
                newUserModel.XTWhenCreate = DateTime.Now;
                newUserModel.XVWhoEdit = userCode;
                newUserModel.XTWhenEdit = DateTime.Now;

                return userDao.createUser(newUserModel);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                userDao = null;
            }
        }

        public bool deleteUser(string userCode)
        {
            TMstMUserDao userDao = new TMstMUserDao();

            try
            {
                if (string.IsNullOrEmpty(userCode))
                {
                    throw new Exception("กรุณากรอกข้อมูลให้ครบถ้วน");
                }

                if (!userDao.checkExistsUserCode(userCode))
                {
                    throw new Exception("ไม่มีผู้ใช้ " + userCode + " ในระบบแล้ว");
                }

                return userDao.deleteUser(userCode);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                userDao = null;
            }
        }

        public bool updateUser(UserInfoViewModel viewModel, string userCode)
        {
            TMstMUserDao userDao = new TMstMUserDao();

            try
            {
                if (string.IsNullOrEmpty(viewModel.userCode) ||
                    string.IsNullOrEmpty(viewModel.userName) ||
                    string.IsNullOrEmpty(viewModel.password) ||
                    string.IsNullOrEmpty(viewModel.role))
                {
                    throw new Exception("กรุณากรอกข้อมูลให้ครบถ้วน");
                }

                if (!userDao.checkExistsUserCode(viewModel.userCode))
                {
                    throw new Exception("ไม่มีผู้ใช้ " + viewModel.userCode + " ในระบบแล้ว");
                }

                TMstMUser userModel = new TMstMUser();
                userModel.XVUsrCode = viewModel.userCode;
                userModel.XVUsrPwd = viewModel.password;
                userModel.XVUsrName = viewModel.userName;
                userModel.XVCstRol = viewModel.role;
                userModel.XVWhoEdit = userCode;
                userModel.XTWhenEdit = DateTime.Now;

                return userDao.updateUser(userModel);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                userDao = null;
            }
        }
    }
}
