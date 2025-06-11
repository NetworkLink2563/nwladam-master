using NWL_AdAm_DAL;
using NWL_AdAm_DML;
using NWL_AdAm_DML.ViewModel;
using NWL_AdAm_DML.ViewModel.Extended;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NWL_AdAm_BLL
{
    public class LampLogic
    {
        public List<LampListDataRowViewModel> getLampListByController(string controllerCode, string customerCode, string searchText)
        {
            TMstMLampDao lampDao = new TMstMLampDao();
            DashboardDao dashboardDao = new DashboardDao();

            try
            {
                List<TMstMLampExtModel> lamps = lampDao.getLampListByController(controllerCode, customerCode, searchText);
                if (lamps != null)
                {
                    lamps = lamps.FindAll(it => it.XVCstCode == customerCode);
                    List<LampListDataRowViewModel> result = (from it in lamps
                                                             select new LampListDataRowViewModel()
                                                            {
                                                                lampCode = it.XVLmpCode,
                                                                lampName = it.XVLmpName
                                                            }).ToList();
                    if (result.Count > 0)
                    {
                        List<STP_NWLShowOnOffStatus_Result> lampStatusList = dashboardDao.getLampStatusDashboardData(customerCode, lamps[0].XVPrjCode);
                        foreach (LampListDataRowViewModel lampData in result)
                        {
                            STP_NWLShowOnOffStatus_Result dashboardData = lampStatusList.Find(it => it.XVLmpCode == lampData.lampCode);
                            if (dashboardData != null)
                            {
                                lampData.lampStatus = dashboardData.XILmpStatus;
                            }
                        }
                    }

                    return result;
                }
                else
                {
                    return new List<LampListDataRowViewModel>();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                lampDao = null;
            }
        }

        public LampInfoViewModel getLampInfoByCode(string lampCode, string customerCode)
        {
            TMstMLampDao lampDao = new TMstMLampDao();

            try
            {
                TMstMLampExtModel lampData = lampDao.getLampByCode(lampCode);
                if (lampData != null)
                {
                    if (!string.IsNullOrEmpty(lampData.XVCstCode) && lampData.XVCstCode != customerCode)
                    {
                        throw new Exception("ไม่มีสิทธิ์เข้าถึงข้อมูลของกล่องควบคุม \"" + lampCode + "\"");
                    }

                    return new LampInfoViewModel
                    {
                        lampCode = lampData.XVLmpCode,
                        lampName = lampData.XVLmpName,
                        lampDescription = lampData.XVLmpDescription,
                        lampSerialNo = lampData.XVLmpSN,
                        controllerCode = lampData.XVCtlCode,
                        latitude = lampData.XVLmpLat,
                        longitude = lampData.XVLmpLong,
                        allowNotify = (lampData.XBLmpIsWarning.HasValue ? lampData.XBLmpIsWarning.Value : false),
                        customerCode = lampData.XVCstCode
                    };
                }

                throw new Exception("ไม่พบข้อมูลกล่องควบคุม \"" + lampCode + "\"");
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                lampDao = null;
            }
        }

        public bool createLamp(LampInfoViewModel viewModel, string userCode)
        {
            TMstMLampDao lampDao = new TMstMLampDao();

            try
            {
                if (string.IsNullOrEmpty(viewModel.lampCode) ||
                    string.IsNullOrEmpty(viewModel.lampName) ||
                    string.IsNullOrEmpty(viewModel.controllerCode) ||
                    string.IsNullOrEmpty(viewModel.lampSerialNo))
                {
                    throw new Exception("กรุณากรอกข้อมูลให้ครบถ้วน");
                }

                if (lampDao.checkExistsLampCode(viewModel.lampCode))
                {
                    throw new Exception("มีหลอดไฟ " + viewModel.controllerCode + " ในระบบแล้ว");
                }

                TMstMLamp newLampModel = new TMstMLamp();
                newLampModel.XVLmpCode = viewModel.lampCode;
                newLampModel.XVLmpName = viewModel.lampName;
                newLampModel.XVLmpDescription = viewModel.lampDescription;
                newLampModel.XVLmpSN = viewModel.lampCode;
                newLampModel.XVCtlCode = viewModel.controllerCode;
                newLampModel.XVLmpLat = (!string.IsNullOrEmpty(viewModel.latitude) ? viewModel.latitude : "");
                newLampModel.XVLmpLong = (!string.IsNullOrEmpty(viewModel.longitude) ? viewModel.longitude : "");
                newLampModel.XBLmpIsWarning = viewModel.allowNotify;
                newLampModel.XVWhoCreate = userCode;
                newLampModel.XVWhoEdit = userCode;
                newLampModel.XTWhenCreate = DateTime.Now;
                newLampModel.XTWhenEdit = DateTime.Now;

                return lampDao.createLamp(newLampModel);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                lampDao = null;
            }
        }

        public bool updateLamp(LampInfoViewModel viewModel, string userCode)
        {
            TMstMLampDao lampDao = new TMstMLampDao();

            try
            {
                if (string.IsNullOrEmpty(viewModel.lampCode) ||
                    string.IsNullOrEmpty(viewModel.lampName) ||
                    string.IsNullOrEmpty(viewModel.lampSerialNo))
                {
                    throw new Exception("กรุณากรอกข้อมูลให้ครบถ้วน");
                }

                if (lampDao.checkExistsLampCode(viewModel.controllerCode))
                {
                    throw new Exception("มีหลอดไฟ " + viewModel.controllerCode + " ในระบบแล้ว");
                }

                TMstMLamp lampModel = new TMstMLamp();
                lampModel.XVLmpCode = viewModel.lampCode;
                lampModel.XVLmpName = viewModel.lampName;
                lampModel.XVLmpDescription = viewModel.lampDescription;
                //lampModel.XVCtlCode = viewModel.XVCtlCode;
                lampModel.XVLmpLat = viewModel.latitude;
                lampModel.XVLmpLong = viewModel.longitude;
                lampModel.XBLmpIsWarning = viewModel.allowNotify;
                lampModel.XVWhoEdit = userCode;
                lampModel.XTWhenEdit = DateTime.Now;

                return lampDao.updateLamp(lampModel);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                lampDao = null;
            }
        }

        public bool deleteLamp(string lampCode)
        {
            TMstMLampDao lampDao = new TMstMLampDao();

            try
            {
                if (string.IsNullOrEmpty(lampCode))
                {
                    throw new Exception("กรุณากรอกข้อมูลให้ครบถ้วน");
                }

                if (!lampDao.checkExistsLampCode(lampCode))
                {
                    throw new Exception("ไม่มีกล่องควบคุม " + lampCode + " ในระบบแล้ว");
                }

                return lampDao.deleteLamp(lampCode);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                lampDao = null;
            }
        }

        public LampStatusViewModel getLampStatus(string lampCode)
        {
            TMstMLampDao lampDao = new TMstMLampDao();

            try
            {
                if (string.IsNullOrEmpty(lampCode))
                {
                    throw new Exception("กรุณาระบุ S/N หลอดไฟ");
                }

                V_LampStatus lampStatusModel = lampDao.getLampStatus(lampCode);
                if (lampStatusModel != null)
                {
                    return new LampStatusViewModel()
                    {
                        projectType = lampStatusModel.XVPrjType,
                        controllerCode = lampStatusModel.XVCtlCode,
                        lampSerialNo = lampStatusModel.XVLmpCode,
                        lampName = lampStatusModel.XVLmpName,
                        relay = lampStatusModel.XIStaRelay.Value,
                        pwm1 = lampStatusModel.XIStaPWM1.Value,
                        pwm2 = lampStatusModel.XIStaPWM2.Value,
                        signal = lampStatusModel.XIStaSignal.Value,
                        current = lampStatusModel.XIStaCurrent.Value,
                        ambientLight = lampStatusModel.XIStaAmLight.Value,
                        mode = lampStatusModel.XIStaMode.Value,
                        updatedAt = lampStatusModel.XTStaUpdate.Value
                    };
                }
                else
                {
                    throw new Exception("ไม่สามารถเชื่อมต่อหลอดไฟ \"" + lampCode + "\"");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                lampDao = null;
            }
        }
    }
}
