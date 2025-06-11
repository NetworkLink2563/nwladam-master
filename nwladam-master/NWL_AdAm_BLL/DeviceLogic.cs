using NWL_AdAm_DAL;
using NWL_AdAm_DML;
using NWL_AdAm_DML.ViewModel;
using NWL_AdAm_DML.ViewModel.Extended;
using NWL_AdAm_SPL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NWL_AdAm_BLL
{
    public class DeviceLogic
    {
        public DeviceInfoViewModel getControllerInfoByCode(string controllerCode, string customerCode)
        {
            TMstMControllerDao controllerDao = new TMstMControllerDao();

            try
            {
                TMstMControllerExtModel controllerData = controllerDao.getControllerByCode(controllerCode);
                if (controllerData != null)
                {
                    if (!string.IsNullOrEmpty(controllerData.XVCstCode) && controllerData.XVCstCode != customerCode)
                    {
                        throw new Exception("ไม่มีสิทธิ์เข้าถึงข้อมูลของกล่องควบคุม \"" + controllerCode + "\"");
                    }

                    return new DeviceInfoViewModel
                    {
                        controllerCode = controllerData.XVCtlCode,
                        controllerName = controllerData.XVCtlName,
                        controllerDescription = controllerData.XVCtlDescription,
                        controllerSerialNo = controllerData.XVCtlSN,
                        latitude = controllerData.XVCtlLat,
                        longitude = controllerData.XVCtlLong,
                        projectCode = controllerData.XVPrjCode,
                        customerCode = controllerData.XVCstCode,
                        controllerPhone = controllerData.XVCtlPhone,
                        lampQty = (controllerData.XICtlLampQty.HasValue ? controllerData.XICtlLampQty.Value : 0),
                        ampMin = (controllerData.XFCtlAmpMin.HasValue ? controllerData.XFCtlAmpMin.Value : 0.0),
                        ampMax = (controllerData.XFCtlAmpMax.HasValue ? controllerData.XFCtlAmpMax.Value : 0.0),
                        boxNo = controllerData.XVBoxNo,
                        boxKmFrom = controllerData.XVBoxKmFrom,
                        boxKmTo = controllerData.XVBoxKmTo,
                    };
                }

                throw new Exception("ไม่พบข้อมูลกล่องควบคุม \"" + controllerCode + "\"");
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                controllerDao = null;
            }
        }

        public bool createController(DeviceInfoViewModel viewModel, string userCode, bool autoGenerate = false)
        {
            TMstMControllerDao controllerDao = new TMstMControllerDao();
            TMstMProjectDao projectDao = new TMstMProjectDao();

            try
            {
                if (string.IsNullOrEmpty(viewModel.controllerCode) ||
                    string.IsNullOrEmpty(viewModel.controllerName) ||
                    string.IsNullOrEmpty(viewModel.projectCode) ||
                    string.IsNullOrEmpty(viewModel.controllerSerialNo))
                {
                    throw new Exception("กรุณากรอกข้อมูลให้ครบถ้วน");
                }

                if (controllerDao.checkExistsControllerCode(viewModel.controllerCode))
                {
                    throw new Exception("มีกล่องควบคุม " + viewModel.controllerCode + " ในระบบแล้ว");
                }

                if (!autoGenerate)
                {
                    TMstMProjectExtModel projectData = projectDao.getProjectByCode(viewModel.projectCode);
                    if (projectData != null && projectData.XVPrjType == Variables.PROJECT_TYPE_NB_NODE)
                    {
                        throw new Exception("เนื่องจากโครงการนี้ใช้กล่องควบคุมประเภท NB Node จึงไม่สามารถเพิ่มกล่องควบคุมใหม่ หรือลบกล่องควบคุมที่มีอยู่แล้วได้");
                    }
                }

                TMstMController newControllerModel = new TMstMController();
                newControllerModel.XVCtlCode = viewModel.controllerCode;
                newControllerModel.XVCtlName = viewModel.controllerName;
                newControllerModel.XVCtlDescription = viewModel.controllerDescription;
                newControllerModel.XVCtlSN = viewModel.controllerCode;
                newControllerModel.XVPrjCode = viewModel.projectCode;
                newControllerModel.XVCtlLat = (!string.IsNullOrEmpty(viewModel.latitude) ? viewModel.latitude : "");
                newControllerModel.XVCtlLong = (!string.IsNullOrEmpty(viewModel.longitude) ? viewModel.longitude : "");
                newControllerModel.XVCtlPhone = viewModel.controllerPhone;
                newControllerModel.XICtlLampQty = viewModel.lampQty;
                newControllerModel.XFCtlAmpMin = viewModel.ampMin;
                newControllerModel.XFCtlAmpMax = viewModel.ampMax;
                newControllerModel.XVBoxNo = viewModel.boxNo;
                newControllerModel.XVBoxKmFrom = viewModel.boxKmFrom;
                newControllerModel.XVBoxKmTo = viewModel.boxKmTo;
                newControllerModel.XVWhoCreate = userCode;
                newControllerModel.XVWhoEdit = userCode;
                newControllerModel.XTWhenCreate = DateTime.Now;
                newControllerModel.XTWhenEdit = DateTime.Now;

                return controllerDao.createController(newControllerModel);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                controllerDao = null;
            }
        }

        public bool updateController(DeviceInfoViewModel viewModel, string userCode)
        {
            TMstMControllerDao controllerDao = new TMstMControllerDao();

            try
            {
                if (string.IsNullOrEmpty(viewModel.controllerCode) ||
                    string.IsNullOrEmpty(viewModel.controllerName) ||
                    string.IsNullOrEmpty(viewModel.controllerSerialNo))
                {
                    throw new Exception("กรุณากรอกข้อมูลให้ครบถ้วน");
                }

                if (!controllerDao.checkExistsControllerCode(viewModel.controllerCode))
                {
                    throw new Exception("ไม่มีกล่องควบคุม " + viewModel.controllerCode + " ในระบบแล้ว");
                }

                TMstMController controllerModel = new TMstMController();
                controllerModel.XVCtlCode = viewModel.controllerCode;
                controllerModel.XVCtlName = viewModel.controllerName;
                controllerModel.XVCtlDescription = viewModel.controllerDescription;
                controllerModel.XVCtlSN = viewModel.controllerCode;
                //controllerModel.XVPrjCode = viewModel.XVPrjCode;
                controllerModel.XVCtlLat = viewModel.latitude;
                controllerModel.XVCtlLong = viewModel.longitude;
                controllerModel.XVCtlPhone = viewModel.controllerPhone;
                controllerModel.XICtlLampQty = viewModel.lampQty;
                controllerModel.XFCtlAmpMin = viewModel.ampMin;
                controllerModel.XFCtlAmpMax = viewModel.ampMax;
                controllerModel.XVBoxNo = viewModel.boxNo;
                controllerModel.XVBoxKmFrom = viewModel.boxKmFrom;
                controllerModel.XVBoxKmTo = viewModel.boxKmTo;
                controllerModel.XVWhoEdit = userCode;
                controllerModel.XTWhenEdit = DateTime.Now;

                return controllerDao.updateController(controllerModel);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                controllerDao = null;
            }
        }

        public bool deleteController(string controllerCode)
        {
            TMstMControllerDao controllerDao = new TMstMControllerDao();

            try
            {
                if (string.IsNullOrEmpty(controllerCode))
                {
                    throw new Exception("กรุณากรอกข้อมูลให้ครบถ้วน");
                }

                if (!controllerDao.checkExistsControllerCode(controllerCode))
                {
                    throw new Exception("ไม่มีกล่องควบคุม " + controllerCode + " ในระบบแล้ว");
                }

                return controllerDao.deleteController(controllerCode);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                controllerDao = null;
            }
        }

        public List<DeviceListDataRowViewModel> getControllerListByProject(string projectCode, string customerCode, string searchText, bool includeStatus = false)
        {
            TMstMControllerDao controllerDao = new TMstMControllerDao();
            DashboardDao dashboardDao = new DashboardDao();
            TSysSConfigDao configDao = new TSysSConfigDao();

            try
            {
                List<TMstMControllerExtModel> controllers = controllerDao.getControllerListByProject(projectCode, customerCode, searchText);
                if (controllers != null)
                {
                    controllers = controllers.FindAll(it => it.XVCstCode == customerCode);
                    List<DeviceListDataRowViewModel> result = (from it in controllers
                                                               select new DeviceListDataRowViewModel()
                                                                {
                                                                    controllerCode = it.XVCtlCode,
                                                                    controllerName = it.XVCtlName
                                                                }).ToList();

                    if (includeStatus)
                    {
                        if (!string.IsNullOrEmpty(projectCode) &&
                            controllers.Count > 0 &&
                            controllers[0].XVPrjType != Variables.PROJECT_TYPE_NB_NODE)
                        {
                            int configEMMHealthCheck = 180;
                            TSysSConfig configModel = configDao.getConfigByCode(Variables.CONFIG_EMM_HEALTH_CHECK);
                            if (configModel != null)
                            {
                                configEMMHealthCheck = int.Parse(configModel.XVSysValue);
                            }

                            List<STP_NWLShowEMMStatus_Result> emmStatusList = dashboardDao.getEMMStatusDashboardData(customerCode, controllers[0].XVPrjCode);
                            foreach (DeviceListDataRowViewModel controllerData in result)
                            {
                                STP_NWLShowEMMStatus_Result dashboardData = emmStatusList.Find(it => it.XVCtlCode == controllerData.controllerCode);
                                if (dashboardData != null)
                                {
                                    if (dashboardData.XILastWait < configEMMHealthCheck)
                                    {
                                        controllerData.controllerStatus = 1;
                                    }
                                    else
                                    {
                                        controllerData.controllerStatus = 0;
                                    }
                                }
                            }
                        }
                    }

                    return result;
                }
                else
                {
                    return new List<DeviceListDataRowViewModel>();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                controllerDao = null;
            }
        }

        public string generateControllerCode()
        {
            TMstMControllerDao controllerDao = new TMstMControllerDao();

            try
            {
                return controllerDao.generateControllerCode();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                controllerDao = null;
            }
        }
    }
}
