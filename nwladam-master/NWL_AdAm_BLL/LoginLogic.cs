using NWL_AdAm_DAL;
using NWL_AdAm_DML;
using NWL_AdAm_DML.Model;
using NWL_AdAm_DML.ViewModel;
using System;

namespace NWL_AdAm_BLL
{
    public class LoginLogic
    {
        public SessionModel loginToSystem(LoginViewModel viewModel)
        {
            TMstMUserDao userDao = new TMstMUserDao();
            TMstMCustomerDao customerDao = new TMstMCustomerDao();

            try
            {
                TMstMUser userData = userDao.getUserByCode(viewModel.usercode);
                if (userData != null)
                {
                    if (userData.XVUsrPwd == viewModel.password)
                    {
                        TMstMCustomer customerData = customerDao.getCustomerByCode(userData.XVCstCode);
                        if (customerData != null)
                        {
                            return new SessionModel()
                            {
                                userCode = userData.XVUsrCode,
                                userName = userData.XVUsrName,
                                customerCode = customerData.XVCstCode,
                                customerName = customerData.XVCstName,
                                role = userData.XVCstRol
                            };
                        }
                    }
                }

                throw new Exception("ไม่พบข้อมูลผู้ใช้งานหรือรหัสผ่านไม่ถูกต้อง");
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                userDao = null;
                customerDao = null;
            }
        }
    }
}
