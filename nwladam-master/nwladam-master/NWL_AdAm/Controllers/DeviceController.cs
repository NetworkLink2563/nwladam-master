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
using X.PagedList;

namespace NWL_AdAm.Controllers
{
    public class DeviceController : BaseController
    {

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Info(string controllerCode)
        {
            FetchRespModel<DeviceInfoViewModel> responseModel = new FetchRespModel<DeviceInfoViewModel>();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;

                DeviceLogic deviceLogic = new DeviceLogic();
                DeviceInfoViewModel result = deviceLogic.getControllerInfoByCode(controllerCode, customerCode);
                if (result != null)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.data = result;
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "ไม่มีกล่องควบคุม " + controllerCode + " อยู่ในระบบแล้ว";
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
        public JsonResult Create(DeviceInfoViewModel viewModel)
        {
            FetchRespModel responseModel = new FetchRespModel();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string userCode = sessionData.userCode;

                DeviceLogic deviceLogic = new DeviceLogic();
                bool result = deviceLogic.createController(viewModel, userCode);
                if (result)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.message = "เพิ่มกล่องควบคุมใหม่สำเร็จ";
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "เพิ่มกล่องควบคุมใหม่ไม่สำเร็จ";
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
        public JsonResult Update(DeviceInfoViewModel viewModel)
        {
            FetchRespModel responseModel = new FetchRespModel();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;
                string userCode = sessionData.userCode;

                DeviceLogic controllerLogic = new DeviceLogic();
                bool result = controllerLogic.updateController(viewModel, userCode);
                if (result)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.message = "แก้ไขข้อมูลกล่องควบคุมสำเร็จ";
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "แก้ไขข้อมูลกล่องควบคุมไม่สำเร็จ";
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
        public JsonResult Delete(string controllerCode)
        {
            FetchRespModel responseModel = new FetchRespModel();

            try
            {
                DeviceLogic deviceLogic = new DeviceLogic();
                bool result = deviceLogic.deleteController(controllerCode);
                if (result)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.message = "ลบกล่องควบคุมสำเร็จ";
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "ลบกล่องควบคุมไม่สำเร็จ";
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
        public JsonResult getControllerOnPagedList(string projectCode, int page, string searchText)
        {
            FetchRespPagingModel<IPagedList<DeviceListDataRowViewModel>> responseViewModel = new FetchRespPagingModel<IPagedList<DeviceListDataRowViewModel>>();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;

                DeviceLogic deviceLogic = new DeviceLogic();
                List<DeviceListDataRowViewModel> result = deviceLogic.getControllerListByProject(projectCode, customerCode, searchText, true);
                if (result != null)
                {
                    int totalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(result.Count) / Convert.ToDouble(Variables.ITEM_PER_PAGE)));
                    if (totalPage > 0 && page > totalPage)
                    {
                        page = totalPage;
                    }

                    responseViewModel.state = FetchRespModel.success;
                    responseViewModel.pagingTotalPage = totalPage;                    
                    responseViewModel.data = result.ToPagedList(page, Variables.ITEM_PER_PAGE);
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

        [HttpPost]
        public JsonResult getControllerListByProject(string projectCode)
        {
            FetchRespModel<List<DeviceListDataRowViewModel>> responseViewModel = new FetchRespModel<List<DeviceListDataRowViewModel>>();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;

                DeviceLogic deviceLogic = new DeviceLogic();
                List<DeviceListDataRowViewModel> result = deviceLogic.getControllerListByProject(projectCode, customerCode, "");
                if (result != null)
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