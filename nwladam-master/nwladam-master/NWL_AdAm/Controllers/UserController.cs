using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using NWL_AdAm.Controllers.Base;
using NWL_AdAm_BLL;
using NWL_AdAm_DML;
using NWL_AdAm_DML.Model;
using NWL_AdAm_DML.ViewModel;
using NWL_AdAm_SPL;
using X.PagedList;

namespace NWL_AdAm.Controllers
{
    public class UserController : BaseController
    {
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Info(string userCode)
        {
            FetchRespModel<UserInfoViewModel> responseModel = new FetchRespModel<UserInfoViewModel>();

            try
            {
                UserLogic userLogic = new UserLogic();
                UserInfoViewModel result = userLogic.getUserInfoByCode(userCode);
                if (result != null)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.data = result;
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "ไม่มีผู้ใช้ " + userCode + " ในระบบแล้ว";
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
        public JsonResult Create(UserInfoViewModel viewModel)
        {
            FetchRespModel responseModel = new FetchRespModel();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;
                string userCode = sessionData.userCode;

                UserLogic userLogic = new UserLogic();
                bool result = userLogic.createUser(viewModel, userCode, customerCode);
                if (result)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.message = "เพิ่มผู้ใช้ใหม่สำเร็จ";
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "เพิ่มผู้ใช้ใหม่ไม่สำเร็จ";
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
        public JsonResult Update(UserInfoViewModel viewModel)
        {
            FetchRespModel responseModel = new FetchRespModel();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;
                string userCode = sessionData.userCode;

                UserLogic userLogic = new UserLogic();
                bool result = userLogic.updateUser(viewModel, userCode);
                if (result)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.message = "แก้ไขข้อมูลผู้ใช้สำเร็จ";
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "แก้ไขข้อมูลผู้ใช้ไม่สำเร็จ";
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
        public JsonResult Delete(string userCode)
        {
            FetchRespModel responseModel = new FetchRespModel();

            try
            {
                UserLogic userLogic = new UserLogic();
                bool result = userLogic.deleteUser(userCode);
                if (result)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.message = "ลบผู้ใช้สำเร็จ";
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "ลบผู้ใช้ไม่สำเร็จ";
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
        public JsonResult getUserOnPagedList(int page, string searchText)
        {
            FetchRespPagingModel<IPagedList<UserListDataRowViewModel>> responseViewModel = new FetchRespPagingModel<IPagedList<UserListDataRowViewModel>>();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;

                UserLogic userLogic = new UserLogic();
                List<UserListDataRowViewModel> result = userLogic.getUserOnPagedList(customerCode, searchText);
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

    }
}
