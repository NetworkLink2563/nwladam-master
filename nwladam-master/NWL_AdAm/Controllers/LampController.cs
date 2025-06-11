using NWL_AdAm.Controllers.Base;
using NWL_AdAm_BLL;
using NWL_AdAm_DML;
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
    public class LampController : BaseController
    {

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Info(string lampCode)
        {
            FetchRespModel<LampInfoViewModel> responseModel = new FetchRespModel<LampInfoViewModel>();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;

                LampLogic lampLogic = new LampLogic();
                LampInfoViewModel result = lampLogic.getLampInfoByCode(lampCode, customerCode);
                if (result != null)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.data = result;
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "ไม่มีหลอดไฟ " + lampCode + " ในระบบแล้ว";
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
        public JsonResult Create(LampInfoViewModel viewModel)
        {
            FetchRespModel responseModel = new FetchRespModel();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string userCode = sessionData.userCode;

                LampLogic lampLogic = new LampLogic();
                bool result = lampLogic.createLamp(viewModel, userCode);
                if (result)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.message = "เพิ่มหลอดไฟใหม่สำเร็จ";
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "เพิ่มหลอดไฟใหม่ไม่สำเร็จ";
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
        public JsonResult Update(LampInfoViewModel viewModel)
        {
            FetchRespModel responseModel = new FetchRespModel();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string userCode = sessionData.userCode;

                LampLogic lampLogic = new LampLogic();
                bool result = lampLogic.updateLamp(viewModel, userCode);
                if (result)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.message = "แก้ไขข้อมูลหลอดไฟสำเร็จ";
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "แก้ไขข้อมูลหลอดไฟไม่สำเร็จ";
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
        public JsonResult Delete(string lampCode)
        {
            FetchRespModel responseModel = new FetchRespModel();

            try
            {
                LampLogic lampLogic = new LampLogic();
                bool result = lampLogic.deleteLamp(lampCode);
                if (result)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.message = "ลบหลอดไฟสำเร็จ";
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "ลบหลอดไฟไม่สำเร็จ";
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
        public JsonResult getLampOnPagedList(string controllerCode, int page, string searchText)
        {
            FetchRespPagingModel<IPagedList<LampListDataRowViewModel>> responseViewModel = new FetchRespPagingModel<IPagedList<LampListDataRowViewModel>>();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;

                LampLogic lampLogic = new LampLogic();
                List<LampListDataRowViewModel> result = lampLogic.getLampListByController(controllerCode, customerCode, searchText);
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
        public JsonResult getLampListByController(string controllerCode)
        {
            FetchRespModel<List<LampListDataRowViewModel>> responseViewModel = new FetchRespModel<List<LampListDataRowViewModel>>();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;

                LampLogic lampLogic = new LampLogic();
                List<LampListDataRowViewModel> result = lampLogic.getLampListByController(controllerCode, customerCode, "");
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

        [HttpPost]
        public JsonResult getLampStatus(string lampCode)
        {
            FetchRespModel<LampStatusViewModel> responseViewModel = new FetchRespModel<LampStatusViewModel>();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;

                LampLogic lampLogic = new LampLogic();
                LampStatusViewModel result = lampLogic.getLampStatus(lampCode);
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