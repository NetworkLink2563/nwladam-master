using NWL_AdAm_DML.Model;
using NWL_AdAm_SPL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NWL_AdAm.Controllers.Base
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            initAppController(filterContext);
            base.OnActionExecuting(filterContext);
        }

        private void initAppController(ActionExecutingContext filterContext)
        {
            if (Session[Variables.USER_PROFILE] == null)
            {
                filterContext.Result = RedirectToAction("Index", "Login");
            }
            else
            {
                if ((Session[Variables.USER_PROFILE] as SessionModel).role == "3")
                {
                    string[] restrictMenu = { "USER", "PROJECT", "DEVICE", "LAMP" };
                    if (restrictMenu.Any(it => filterContext.Controller.ToString().ToUpper().Contains(it)))
                    {
                        string[] allowedMethod = { 
                            "getProjectListByCustomer",
                            "getControllerListByProject",
                            "getLampListByController",
                            "getLampStatus"
                        };

                        if (!allowedMethod.Any(it => filterContext.ActionDescriptor.ActionName.Contains(it)))
                        {
                            filterContext.Result = Redirect("~/Home/Unauthorize");
                        }
                    }
                }
            }
        }
    }
}