using NWL_AdAm.Controllers.Base;
using NWL_AdAm_BLL;
using NWL_AdAm_DML.Model;
using NWL_AdAm_DML.ViewModel;
using NWL_AdAm_SPL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NWL_AdAm.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Unauthorize()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getLampStatusDashboardData(string projectCode)
        {
            FetchRespModel<List<LampStatusDashboardModel>> responseModel = new FetchRespModel<List<LampStatusDashboardModel>>();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;

                DashboardLogic dashboardLogic = new DashboardLogic();
                List<LampStatusDashboardModel> result = dashboardLogic.getLampStatusDashboardData(customerCode, projectCode);
                if (result != null)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.data = result;
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                }
            }
            catch (Exception e)
            {
                responseModel.state = FetchRespModel.error;
                responseModel.message = e.Message;
            }

            return Json(responseModel);
        }

        [HttpPost]
        public JsonResult getJobStatusDashboardData(string projectCode)
        {
            FetchRespModel<List<JobStatusDashboardModel>> responseModel = new FetchRespModel<List<JobStatusDashboardModel>>();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;

                DashboardLogic dashboardLogic = new DashboardLogic();
                List<JobStatusDashboardModel> result = dashboardLogic.getJobStatusDashboardData(customerCode, projectCode);
                if (result != null)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.data = result;
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                }
            }
            catch (Exception e)
            {
                responseModel.state = FetchRespModel.error;
                responseModel.message = e.Message;
            }

            return Json(responseModel);
        }

        [HttpPost]
        public JsonResult getEMMStatusDashboardData(string projectCode)
        {
            FetchRespModel<List<EMMStatusDashboardModel>> responseModel = new FetchRespModel<List<EMMStatusDashboardModel>>();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;

                DashboardLogic dashboardLogic = new DashboardLogic();
                List<EMMStatusDashboardModel> result = dashboardLogic.getEMMStatusDashboardData(customerCode, projectCode);
                if (result != null)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.data = result;
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                }
            }
            catch (Exception e)
            {
                responseModel.state = FetchRespModel.error;
                responseModel.message = e.Message;
            }

            return Json(responseModel);
        }

        [HttpPost]
        public JsonResult getWeatherDashboardData(GeoLocationModel location)
        {
            FetchRespModel<WeatherDashboardModel> responseModel = new FetchRespModel<WeatherDashboardModel>();

            try
            {
                DashboardLogic dashboardLogic = new DashboardLogic();
                Task<WeatherDashboardModel> taskGetWeatherData = Task.Run(() => dashboardLogic.getWeatherDashboardData(location));
                taskGetWeatherData.Wait();

                if (taskGetWeatherData.Result != null)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.data = taskGetWeatherData.Result;
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
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