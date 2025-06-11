using NWL_AdAm.Controllers.Base;
using NWL_AdAm_BLL;
using NWL_AdAm_DML.Model;
using NWL_AdAm_DML.ViewModel;
using NWL_AdAm_DML.ViewModel.Extended;
using NWL_AdAm_SPL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using X.PagedList;

namespace NWL_AdAm.Controllers
{
    public class JobController : BaseController
    {
        [OutputCache(NoStore = true, Duration = 0)]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Info(string jobCode)
        {
            FetchRespModel<JobInfoViewModel> responseModel = new FetchRespModel<JobInfoViewModel>();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;
                string role = sessionData.role;
                string userName = sessionData.userName;

                JobLogic jobLogic = new JobLogic();
                JobInfoViewModel result = jobLogic.getJobInfoByCode(jobCode, customerCode, role, userName);
                if (result != null)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.data = result;
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "ไม่มีงานแจ้งซ่อม " + jobCode + " ในระบบแล้ว";
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
        public JsonResult Create(JobInfoViewModel viewModel)
        {
            FetchRespModel responseModel = new FetchRespModel();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;
                string userCode = sessionData.userCode;

                JobLogic jobLogic = new JobLogic();
                bool result = jobLogic.createJob(viewModel, customerCode, userCode);
                if (result)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.message = "แจ้งงานซ่อมใหม่สำเร็จ";
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "แจ้งงานซ่อมใหม่ไม่สำเร็จ";
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
        public JsonResult Update(JobInfoViewModel viewModel)
        {
            FetchRespModel responseModel = new FetchRespModel();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string userCode = sessionData.userCode;

                JobLogic jobLogic = new JobLogic();
                bool result = jobLogic.updateJob(viewModel, userCode);
                if (result)
                {
                    responseModel.state = FetchRespModel.success;
                    responseModel.message = "บันทึกงานแจ้งซ่อมสำเร็จ";
                }
                else
                {
                    responseModel.state = FetchRespModel.warning;
                    responseModel.message = "บันทึกงานแจ้งซ่อมไม่สำเร็จ";
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
        public JsonResult getJobOnPagedList(int page, string searchText, string projectCode)
        {
            FetchRespPagingModel<IPagedList<JobListDataRowViewModel>> responseViewModel = new FetchRespPagingModel<IPagedList<JobListDataRowViewModel>>();

            try
            {
                SessionModel sessionData = Session[Variables.USER_PROFILE] as SessionModel;
                string customerCode = sessionData.customerCode;

                JobLogic jobLogic = new JobLogic();
                List<TDocTJobExtModel> jobs = jobLogic.getJobListByCustomerCode(customerCode, searchText, projectCode);
                if (jobs != null)
                {
                    int totalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(jobs.Count) / Convert.ToDouble(Variables.ITEM_PER_PAGE)));
                    if (totalPage > 0 && page > totalPage)
                    {
                        page = totalPage;
                    }
                    List<JobListDataRowViewModel> data = (from it in jobs
                                                          select new JobListDataRowViewModel()
                                                          {
                                                              jobDocNo = it.XVJobDocNo,
                                                              docStatus = it.XVJobDocStatus,
                                                              jobProblem = it.XVJobProblem,
                                                              problemTime = it.XTJobProblemTime.Value.ToString("yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US", true)),
                                                              projectName = it.XVPrjName,
                                                              jobResolve=it.XVJobResolve
                                                          }).ToList();
                    responseViewModel.state = FetchRespModel.success;
                    responseViewModel.pagingTotalPage = totalPage;
                    responseViewModel.data = data.ToPagedList(page, Variables.ITEM_PER_PAGE);
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