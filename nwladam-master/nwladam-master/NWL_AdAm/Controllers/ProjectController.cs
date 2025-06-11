using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NWL_AdAm.Controllers.Base;
using NWL_AdAm_BLL;
using NWL_AdAm_DML.Model;
using NWL_AdAm_DML.ViewModel;
using NWL_AdAm_SPL;
using X.PagedList;

namespace NWL_AdAm.Controllers
{
    public class ProjectController : BaseController
    {
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Info(string projectCode)
        {
            FetchRespModel<ProjectInfoViewModel> responseModel = new FetchRespModel<ProjectInfoViewModel>();

            try
            {
                ProjectLogic projectLogic = new ProjectLogic();
                ProjectInfoViewModel result = projectLogic.getProjectInfoByCode(projectCode);
                if (result != null)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.data = result;
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "ไม่มีโครงการ " + projectCode + " ในระบบแล้ว";
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
        public JsonResult Create(ProjectInfoViewModel viewModel)
        {
            FetchRespModel responseModel = new FetchRespModel();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;
                string userCode = sessionData.userCode;

                ProjectLogic projectLogic = new ProjectLogic();
                bool result = projectLogic.createProject(viewModel, userCode, customerCode);
                if (result)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.message = "เพิ่มโครงการใหม่สำเร็จ";
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "เพิ่มโครงการใหม่ไม่สำเร็จ";
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
        public JsonResult Update(ProjectInfoViewModel viewModel)
        {
            FetchRespModel responseModel = new FetchRespModel();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;
                string userCode = sessionData.userCode;

                ProjectLogic projectLogic = new ProjectLogic();
                bool result = projectLogic.updateProject(viewModel, userCode);
                if (result)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.message = "แก้ไขข้อมูลโครงการสำเร็จ";
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "แก้ไขข้อมูลโครงการไม่สำเร็จ";
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
        public JsonResult Delete(string projectCode)
        {
            FetchRespModel responseModel = new FetchRespModel();

            try
            {
                ProjectLogic projectLogic = new ProjectLogic();
                bool result = projectLogic.deleteProject(projectCode);
                if (result)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.message = "ลบโครงการสำเร็จ";
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "ลบโครงการไม่สำเร็จ";
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
        public JsonResult getProjectListByCustomer()
        {
            FetchRespModel<List<ProjectListDataRowViewModel>> responseModel = new FetchRespModel<List<ProjectListDataRowViewModel>>();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;

                ProjectLogic projectLogic = new ProjectLogic();
                List<ProjectListDataRowViewModel> result = projectLogic.getProjectListByCustomer(customerCode);
                if (result != null)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.data = result;
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "ยังไม่มีโครงการในระบบ";
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
        public JsonResult getProjectOnPagedList(int page, string searchText, string projectType)
        {
            FetchRespPagingModel<IPagedList<ProjectListDataRowViewModel>> responseViewModel = new FetchRespPagingModel<IPagedList<ProjectListDataRowViewModel>>();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;

                ProjectLogic projectLogic = new ProjectLogic();
                List<ProjectListDataRowViewModel> result = projectLogic.getProjectOnPagedList(customerCode, searchText, projectType);
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
