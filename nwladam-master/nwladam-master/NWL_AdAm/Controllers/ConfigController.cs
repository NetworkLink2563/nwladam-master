using Newtonsoft.Json.Linq;
using NWL_AdAm.Controllers.Base;
using NWL_AdAm_BLL;
using NWL_AdAm_DML;
using NWL_AdAm_DML.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NWL_AdAm.Controllers
{
    public class ConfigController : BaseController
    {
        
        [HttpPost]
        public JsonResult getMqttPublisherEndpoint(string method)
        {
            FetchRespModel<string> responseViewModel = new FetchRespModel<string>();

            try
            {
                ConfigLogic configLogic = new ConfigLogic();
                string result = configLogic.getMqttPublisherEndpoint(method);
                if (!string.IsNullOrEmpty(result))
                {
                    responseViewModel.state = FetchRespModel.success;
                    responseViewModel.data = result;
                }
                else
                {
                    responseViewModel.state = FetchRespModel.warning;
                    responseViewModel.message = "ไม่มีข้อมูล";
                }
            }
            catch (Exception e)
            {
                responseViewModel.state = FetchRespModel.error;
                responseViewModel.message = e.Message;
            }

            return Json(responseViewModel);
        }
    }
}