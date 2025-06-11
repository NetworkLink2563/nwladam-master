using NWL_AdAm_DAL;
using NWL_AdAm_DML;
using NWL_AdAm_DML.ViewModel;
using NWL_AdAm_DML.ViewModel.Extended;
using System;
using System.Collections.Generic;

namespace NWL_AdAm_BLL
{
    public class JobLogic
    {
        private string generateJobDocNo()
        {
            TDocTJobDao jobDao = new TDocTJobDao();

            try
            {
                return jobDao.generateJobDocNo();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                jobDao = null;
            }
        }

        public JobInfoViewModel getJobInfoByCode(string jobCode, string customerCode, string role, string userName = "")
        {
            TDocTJobDao jobDao = new TDocTJobDao();

            try
            {
                TDocTJobExtModel jobData = jobDao.getJobByCode(jobCode);
                if (jobData != null)
                {
                    if (!string.IsNullOrEmpty(jobData.XVCstCode) && jobData.XVCstCode != customerCode)
                    {
                        throw new Exception("ไม่มีสิทธิ์เข้าถึงข้อมูลของงานซ่อม \"" + jobCode + "\"");
                    }

                    JobInfoViewModel viewModel = new JobInfoViewModel();
                    viewModel.jobDocNo = jobData.XVJobDocNo;
                    viewModel.jobDocDate = jobData.XDJobDocDate.Value;
                    viewModel.docStatus = jobData.XVJobDocStatus;
                    viewModel.projectCode = jobData.XVPrjCode;
                    viewModel.controllerCode = jobData.XVCtlCode;
                    viewModel.lampCode = jobData.XVLmpCode;
                    viewModel.jobProblem = jobData.XVJobProblem;
                    viewModel.jobLocation = jobData.XVJobLocation;
                    viewModel.problemTime = jobData.XTJobProblemTime.Value;
                    viewModel.jobResolve = jobData.XVJobResolve;

                    if (jobData.XVJobDocStatus == "2" || role == "3")
                    {
                        if (jobData.XTJobResolveTime.HasValue)
                        {
                            viewModel.resolveTime = jobData.XTJobResolveTime.Value;
                        }

                        if (jobData.XBJobIsComplete.HasValue)
                        {
                            viewModel.isComplete = jobData.XBJobIsComplete.Value;
                        }

                        viewModel.engineerName = userName;
                    }

                    return viewModel;
                }

                throw new Exception("ไม่พบข้อมูลงานซ่อม \"" + jobCode + "\"");
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                jobDao = null;
            }
        }

        public List<TDocTJobExtModel> getJobListByCustomerCode(string customerCode, string searchText, string projectCode)
        {
            TDocTJobDao jobDao = new TDocTJobDao();

            try
            {
                return jobDao.getJobListByCustomerCode(customerCode, searchText, projectCode);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                jobDao = null;
            }
        }

        public bool createJob(JobInfoViewModel viewModel, string customerCode, string userCode)
        {
            TDocTJobDao jobDao = new TDocTJobDao();

            try
            {
                if (string.IsNullOrEmpty(viewModel.projectCode) ||
                        string.IsNullOrEmpty(viewModel.jobProblem) ||
                        (string.IsNullOrEmpty(viewModel.controllerCode) && string.IsNullOrEmpty(viewModel.lampCode)))
                {
                    throw new Exception("กรุณากรอกข้อมูลให้ครบถ้วน");
                }

                string jobDocNo = generateJobDocNo();
                if (string.IsNullOrEmpty(jobDocNo))
                {
                    throw new Exception("สร้างเลขที่งานแจ้งซ่อมใหม่ไม่สำเร็จ");
                }

                if (!string.IsNullOrEmpty(viewModel.lampCode))
                {
                    TMstMLampDao lampDao = new TMstMLampDao();
                    TMstMLampExtModel lampData = lampDao.getLampByCode(viewModel.lampCode);
                    if (lampData != null)
                    {
                        if (string.IsNullOrEmpty(lampData.XVLmpLat))
                        {
                            lampData.XVLmpLat = "-";
                        }

                        if (string.IsNullOrEmpty(lampData.XVLmpLong))
                        {
                            lampData.XVLmpLong = "-";
                        }

                        viewModel.jobLocation = lampData.XVLmpLat + "," + lampData.XVLmpLong;
                    }
                }

                TDocTJob newJobModel = new TDocTJob();
                newJobModel.XVJobDocNo = jobDocNo;
                newJobModel.XDJobDocDate = viewModel.problemTime;
                newJobModel.XVJobDocStatus = "1";
                newJobModel.XVCstCode = customerCode;
                newJobModel.XVPrjCode = viewModel.projectCode;
                newJobModel.XVCtlCode = viewModel.controllerCode;
                newJobModel.XVLmpCode = viewModel.lampCode;
                newJobModel.XVJobProblem = viewModel.jobProblem;
                newJobModel.XVJobLocation = viewModel.jobLocation;
                newJobModel.XTJobProblemTime = viewModel.problemTime;
                newJobModel.XVJobResolve = viewModel.jobResolve;
                newJobModel.XVWhoCreate = userCode;
                newJobModel.XVWhoEdit = userCode;

                return jobDao.createJob(newJobModel);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                jobDao = null;
            }
        }

        public bool updateJob(JobInfoViewModel viewModel, string userCode)
        {
            TDocTJobDao jobDao = new TDocTJobDao();

            try
            {
                if (string.IsNullOrEmpty(viewModel.projectCode) ||
                        string.IsNullOrEmpty(viewModel.jobResolve) ||
                        (string.IsNullOrEmpty(viewModel.controllerCode) && string.IsNullOrEmpty(viewModel.lampCode)))
                {
                    throw new Exception("กรุณากรอกข้อมูลให้ครบถ้วน");
                }

                TDocTJob existsJobModel = jobDao.getJobByCode(viewModel.jobDocNo);
                if (existsJobModel != null)
                {
                    if (existsJobModel.XVJobDocStatus == "2")
                    {
                        throw new Exception("งานซ่อม " + viewModel.jobDocNo + " ได้บันทึกการแก้ไขแล้ว");
                    }
                }
                else
                {
                    throw new Exception("ไม่มีเลขที่งานซ่อม " + viewModel.jobDocNo + " ในระบบแล้ว");
                }
                

                TDocTJob jobModel = new TDocTJob();
                jobModel.XVJobDocNo = viewModel.jobDocNo;
                jobModel.XVJobDocStatus = "2";
                jobModel.XVJobResolve = viewModel.jobResolve;
                jobModel.XTJobResolveTime = viewModel.resolveTime;
                jobModel.XBJobIsComplete = viewModel.isComplete;
                jobModel.XVWhoEdit = userCode;

                return jobDao.updateJob(jobModel);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                jobDao = null;
            }
        }
    }
}
