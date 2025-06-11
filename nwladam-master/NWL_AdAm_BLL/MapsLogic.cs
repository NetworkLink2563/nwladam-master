using NWL_AdAm_DAL;
using NWL_AdAm_DML;
using NWL_AdAm_DML.ViewModel;
using NWL_AdAm_SPL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NWL_AdAm_BLL
{
    public class MapsLogic
    {
        public MapsViewModel getDataToPaintOnMaps(string customerCode)
        {
            TMstMProjectDao projectDao = new TMstMProjectDao();
            TMstMControllerDao controllerDao = new TMstMControllerDao();
            TMstMLampDao lampDao = new TMstMLampDao();
            DashboardDao dashboardDao = new DashboardDao();
            TSysSConfigDao configDao = new TSysSConfigDao();

            try
            {
                List<TMstMProject> baseProjectModel = projectDao.getProjectList(customerCode);
                List<TMstMController> baseControllerModel = controllerDao.getControllerList();
                List<TMstMLamp> baseLampModel = lampDao.getLampList();
                List<STP_NWLShowEMMStatus_Result> emmStatusList = new List<STP_NWLShowEMMStatus_Result>();

                MapsViewModel viewModel = new MapsViewModel();
                viewModel.projects = (from project in baseProjectModel
                                      select new ProjectListMapsViewModel()
                                      {
                                          projectCode = project.XVPrjCode,
                                          projectName = project.XVPrjName,
                                          projectType = project.XVPrjType
                                      }).ToList();

                foreach (ProjectListMapsViewModel project in viewModel.projects)
                {
                    project.controllers = (from controller in baseControllerModel
                                           where controller.XVPrjCode == project.projectCode
                                           select new ControllerListMapsViewModel()
                                           {
                                               controllerCode = controller.XVCtlCode,
                                               controllerName = controller.XVCtlName,
                                               latitude = controller.XVCtlLat,
                                               longitude = controller.XVCtlLong
                                           }).ToList();

                    if (project.controllers.Count > 0)
                    {
                        int configEMMHealthCheck = 180;
                        TSysSConfig configModel = configDao.getConfigByCode(Variables.CONFIG_EMM_HEALTH_CHECK);
                        if (configModel != null)
                        {
                            configEMMHealthCheck = int.Parse(configModel.XVSysValue);
                        }

                        if (project.projectType != Variables.PROJECT_TYPE_NB_NODE)
                        {
                            emmStatusList = dashboardDao.getEMMStatusDashboardData(customerCode, project.projectCode);
                        }

                        foreach (ControllerListMapsViewModel controller in project.controllers)
                        {
                            controller.lamps = (from lamp in baseLampModel
                                                where lamp.XVCtlCode == controller.controllerCode
                                                select new LampListMapsViewModel()
                                                {
                                                    lampCode = lamp.XVLmpCode,
                                                    lampName = lamp.XVLmpName,
                                                    latitude = lamp.XVLmpLat,
                                                    longitude = lamp.XVLmpLong
                                                }).ToList();

                            if (emmStatusList != null)
                            {
                                STP_NWLShowEMMStatus_Result dashboardData = emmStatusList.Find(it => it.XVCtlCode == controller.controllerCode);
                                if (dashboardData != null)
                                {
                                    if (dashboardData.XILastWait < configEMMHealthCheck)
                                    {
                                        controller.controllerStatus = 1;
                                    }
                                    else
                                    {
                                        controller.controllerStatus = 0;
                                    }
                                }
                            }

                            if (controller.lamps.Count > 0)
                            {
                                List<STP_NWLShowOnOffStatus_Result> lampStatusList = dashboardDao.getLampStatusDashboardData(customerCode, project.projectCode);
                                foreach (LampListMapsViewModel lampData in controller.lamps)
                                {
                                    STP_NWLShowOnOffStatus_Result dashboardData = lampStatusList.Find(it => it.XVLmpCode == lampData.lampCode);
                                    if (dashboardData != null)
                                    {
                                        lampData.lampStatus = dashboardData.XILmpStatus;
                                    }
                                }
                            }
                        }
                    }
                }

                return viewModel;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
