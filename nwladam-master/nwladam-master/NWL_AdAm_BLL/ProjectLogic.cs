using NWL_AdAm_DAL;
using NWL_AdAm_DML;
using NWL_AdAm_DML.ViewModel;
using NWL_AdAm_DML.ViewModel.Extended;
using NWL_AdAm_SPL;
using System;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace NWL_AdAm_BLL
{
    public class ProjectLogic
    {
        private string generateProjectCode()
        {
            TMstMProjectDao projectDao = new TMstMProjectDao();

            try
            {
                return projectDao.generateProjectCode();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                projectDao = null;
            }
        }

        public List<ProjectListDataRowViewModel> getProjectOnPagedList(string customerCode, string searchText, string projectType)
        {
            TMstMProjectDao projectDao = new TMstMProjectDao();

            try
            {
                List<TMstMProject> projects = projectDao.getProjectListByCustomer(customerCode, searchText, projectType);
                if (projects != null)
                {
                    List<ProjectListDataRowViewModel> result = (from it in projects
                                                                select new ProjectListDataRowViewModel()
                                                                {
                                                                    projectCode = it.XVPrjCode,
                                                                    projectName = it.XVPrjName,
                                                                    projectType = it.XVPrjType
                                                                }).ToList();

                    return result;
                }
                else
                {
                    return new List<ProjectListDataRowViewModel>();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                projectDao = null;
            }
        }

        public ProjectInfoViewModel getProjectInfoByCode(string projectCode)
        {
            TMstMProjectDao projectDao = new TMstMProjectDao();

            try
            {
                TMstMProjectExtModel projectData = projectDao.getProjectByCode(projectCode);
                if (projectData != null)
                {
                    return new ProjectInfoViewModel
                    {
                        projectCode = projectData.XVPrjCode,
                        projectName = projectData.XVPrjName,
                        projectAddress = projectData.XVPrjAddress,
                        projectType = projectData.XVPrjType,
                        customerCode = projectData.XVCstCode,
                        subDistrictCode = projectData.XVSdtCode,
                        lineToken1 = projectData.XVPrjLineToken1,
                        lineToken2 = projectData.XVPrjLineToken2,
                        districtCode = projectData.XVDstCode,
                        provinceCode = projectData.XVPvnCode
                    };
                }

                throw new Exception("ไม่พบข้อมูล");
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                projectDao = null;
            }
        }

        public bool createProject(ProjectInfoViewModel viewModel, string userCode, string customerCode)
        {
            TMstMProjectDao projectDao = new TMstMProjectDao();

            try
            {
                if (string.IsNullOrEmpty(viewModel.projectCode) ||
                    string.IsNullOrEmpty(viewModel.projectName) ||
                    string.IsNullOrEmpty(viewModel.projectAddress) ||
                    string.IsNullOrEmpty(viewModel.projectType) ||
                    string.IsNullOrEmpty(viewModel.subDistrictCode))
                {
                    throw new Exception("กรุณากรอกข้อมูลให้ครบถ้วน");
                }

                string projectCode = generateProjectCode();
                if (string.IsNullOrEmpty(projectCode))
                {
                    throw new Exception("สร้างรหัสโครงการใหม่ไม่สำเร็จ");
                }

                TMstMProject newProjectModel = new TMstMProject();
                newProjectModel.XVPrjCode = projectCode;
                newProjectModel.XVPrjName = viewModel.projectName;
                newProjectModel.XVPrjAddress = viewModel.projectAddress;
                newProjectModel.XVPrjType = viewModel.projectType;
                newProjectModel.XVCstCode = customerCode;
                newProjectModel.XVSdtCode = viewModel.subDistrictCode;
                newProjectModel.XVPrjLineToken1 = viewModel.lineToken1;
                newProjectModel.XVPrjLineToken2 = viewModel.lineToken2;
                newProjectModel.XVWhoCreate = userCode;
                newProjectModel.XVWhoEdit = userCode;
                newProjectModel.XTWhenCreate = DateTime.Now;
                newProjectModel.XTWhenEdit = DateTime.Now;

                if (viewModel.projectType == Variables.PROJECT_TYPE_NB_NODE)
                {
                    #region "Create Dummy Controller"
                    DeviceLogic deviceLogic = new DeviceLogic();
                    string controllerCode = deviceLogic.generateControllerCode();
                    if (string.IsNullOrEmpty(controllerCode))
                    {
                        throw new Exception("สร้างรหัสกล่องควบคุมจำลอง NB Node ไม่สำเร็จ");
                    }

                    DeviceInfoViewModel dummyControllerModel = new DeviceInfoViewModel();
                    dummyControllerModel.controllerCode = controllerCode;
                    dummyControllerModel.controllerName = "Dummy Controller for " + projectCode;
                    dummyControllerModel.controllerDescription = "Auto-generated controller for project " + projectCode;
                    dummyControllerModel.controllerSerialNo = controllerCode;
                    dummyControllerModel.latitude = "";
                    dummyControllerModel.longitude = "";
                    dummyControllerModel.projectCode = projectCode;
                    dummyControllerModel.customerCode = customerCode;
                    //dummyController.controllerPhone = "";
                    //dummyController.lampQty = 0;
                    //dummyController.ampMin = 0.0;
                    //dummyController.ampMax = 0.0;
                    //dummyController.boxNo = "";
                    //dummyController.boxKmFrom = "";
                    //dummyController.boxKmTo = "";

                    bool result = deviceLogic.createController(dummyControllerModel, userCode, true);
                    if (!result)
                    {
                        throw new Exception("สร้างกล่องควบคุมจำลองไม่สำเร็จ");
                    }
                    #endregion
                }

                return projectDao.createProject(newProjectModel);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                projectDao = null;
            }
        }

        public bool deleteProject(string projectCode)
        {
            TMstMProjectDao projectDao = new TMstMProjectDao();

            try
            {
                if (string.IsNullOrEmpty(projectCode))
                {
                    throw new Exception("กรุณากรอกข้อมูลให้ครบถ้วน");
                }

                if (!projectDao.checkExistsProjectCode(projectCode))
                {
                    throw new Exception("ไม่มีโครงการ " + projectCode + " ในระบบแล้ว");
                }

                return projectDao.deleteProject(projectCode);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                projectDao = null;
            }
        }

        public bool updateProject(ProjectInfoViewModel viewModel, string userCode)
        {
            TMstMProjectDao projectDao = new TMstMProjectDao();

            try
            {
                if (string.IsNullOrEmpty(viewModel.projectCode) ||
                    string.IsNullOrEmpty(viewModel.projectName) ||
                    string.IsNullOrEmpty(viewModel.projectAddress) ||
                    string.IsNullOrEmpty(viewModel.projectType) ||
                    string.IsNullOrEmpty(viewModel.subDistrictCode))
                {
                    throw new Exception("กรุณากรอกข้อมูลให้ครบถ้วน");
                }

                if (!projectDao.checkExistsProjectCode(viewModel.projectCode))
                {
                    throw new Exception("ไม่มีโครงการ " + viewModel.projectCode + " ในระบบแล้ว");
                }

                TMstMProject projectModel = new TMstMProject();
                projectModel.XVPrjCode = viewModel.projectCode;
                projectModel.XVPrjName = viewModel.projectName;
                projectModel.XVPrjAddress = viewModel.projectAddress;
                projectModel.XVPrjType = viewModel.projectType;
                projectModel.XVSdtCode = viewModel.subDistrictCode;
                projectModel.XVPrjLineToken1 = viewModel.lineToken1;
                projectModel.XVPrjLineToken2 = viewModel.lineToken2;
                projectModel.XVWhoEdit = userCode;
                projectModel.XTWhenEdit = DateTime.Now;

                return projectDao.updateProject(projectModel);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                projectDao = null;
            }
        }

        public List<ProjectListDataRowViewModel> getProjectListByCustomer(string customerCode)
        {
            TMstMProjectDao projectDao = new TMstMProjectDao();

            try
            {
                List<TMstMProject> projects = projectDao.getProjectListByCustomer(customerCode, "", "");
                if (projects != null)
                {
                    List<ProjectListDataRowViewModel> result = (from it in projects
                                                                select new ProjectListDataRowViewModel()
                                                                {
                                                                    projectCode = it.XVPrjCode,
                                                                    projectName = it.XVPrjName,
                                                                    projectType = it.XVPrjType
                                                                }).ToList();

                    return result;
                }
                else
                {
                    return new List<ProjectListDataRowViewModel>();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                projectDao = null;
            }
        }
    }
}
