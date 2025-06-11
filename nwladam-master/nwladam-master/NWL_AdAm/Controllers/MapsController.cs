using NWL_AdAm.Controllers.Base;
using NWL_AdAm_BLL;
using NWL_AdAm_DML.Model;
using NWL_AdAm_DML.ViewModel;
using NWL_AdAm_SPL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NWL_AdAm.Controllers
{
    public class MapsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getDataToPaintOnMaps()
        {
            FetchRespModel<MapsViewModel> responseModel = new FetchRespModel<MapsViewModel>();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;

                MapsLogic mapsLogic = new MapsLogic();
                MapsViewModel result = mapsLogic.getDataToPaintOnMaps(customerCode);
                if (result != null)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.data = result;
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.title = "ดำเนินการไม่สำเร็จ";
                }
            }
            catch (Exception e)
            {
                responseModel.state = FetchRespModel.error;
                responseModel.message = e.Message;
            }

            return Json(responseModel);
        }
    }
}