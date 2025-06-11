using NWL_AdAm.Controllers.Base;
using NWL_AdAm_BLL;
using NWL_AdAm_DML.Model;
using NWL_AdAm_DML.ViewModel;
using NWL_AdAm_SPL;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace NWL_AdAm.Controllers
{
    public class LoginController : Controller
    {


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            Session.Clear();
            Session.Abandon();
            //FormsAuthentication.SignOut();

            return View();
        }

        [HttpPost]
        public JsonResult Index(LoginViewModel loginData)
        {
            FetchRespModel<string> responseViewModel = new FetchRespModel<string>();

            try
            {
                if (loginData != null)
                {
                    if (string.IsNullOrEmpty(loginData.usercode) || string.IsNullOrEmpty(loginData.password))
                    {
                        responseViewModel.state = FetchRespModel.warning;
                        responseViewModel.message = "กรุณากรอกข้อมูลให้ครบถ้วน";
                    }
                    else
                    {
                        LoginLogic loginLogic = new LoginLogic();
                        SessionModel sessionData = loginLogic.loginToSystem(loginData);
                        Session[Variables.USER_PROFILE] = sessionData;

                        responseViewModel.state = FetchRespModel.success;
                    }
                }
                else
                {
                    responseViewModel.state = FetchRespModel.warning;
                    responseViewModel.message = "กรุณากรอกข้อมูลให้ครบถ้วน";
                }
            }
            catch (Exception e)
            {
                responseViewModel.state = FetchRespModel.warning;
                responseViewModel.message = e.Message;
            }

            return Json(responseViewModel);
        }

    }
}