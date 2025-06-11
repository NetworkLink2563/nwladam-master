using NWL_AdAm.Controllers.Base;
using NWL_AdAm_BLL;
using NWL_AdAm_DML;
using NWL_AdAm_DML.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NWL_AdAm.Controllers
{
    public class AddressController : BaseController
    {
        [HttpGet]
        public JsonResult getProvince()
        {
            FetchRespModel<List<ProvinceItemViewModel>> responseViewModel = new FetchRespModel<List<ProvinceItemViewModel>>();

            try
            {
                AddressLogic addressLogic = new AddressLogic();
                List<ProvinceItemViewModel> result = addressLogic.getProvinceList();
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

            return Json(responseViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getDistrictByProvinceCode(string provinceCode)
        {
            FetchRespModel<List<DistrictItemViewModel>> responseViewModel = new FetchRespModel<List<DistrictItemViewModel>>();

            try
            {
                AddressLogic addressLogic = new AddressLogic();
                List <DistrictItemViewModel> result = addressLogic.getDistrictByProvinceCode(provinceCode);
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

            return Json(responseViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getSubDistrictByDistrictCode(string districtCode)
        {
            FetchRespModel<List<SubDistrictItemViewModel>> responseViewModel = new FetchRespModel<List<SubDistrictItemViewModel>>();

            try
            {
                AddressLogic addressLogic = new AddressLogic();
                List<SubDistrictItemViewModel> result = addressLogic.getSubDistrictByDistrictCode(districtCode);
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

            return Json(responseViewModel, JsonRequestBehavior.AllowGet);
        }
    }
}
