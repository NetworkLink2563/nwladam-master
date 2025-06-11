using NWL_AdAm_DAL;
using NWL_AdAm_DML;
using NWL_AdAm_DML.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWL_AdAm_BLL
{
    public class AddressLogic
    {
        public List<ProvinceItemViewModel> getProvinceList()
        {
            TSysSProvinceDao provinceDao = new TSysSProvinceDao();
            List<TSysSProvince> provinceList = new List<TSysSProvince>();
            List<ProvinceItemViewModel> viewModel = new List<ProvinceItemViewModel>();

            try
            {
                provinceList = provinceDao.getProvinceList();
                viewModel = (from it in provinceList
                             orderby it.XVPvnName 
                             select new ProvinceItemViewModel
                             {
                                 provinceCode = it.XVPvnCode,
                                 provinceNameTH = it.XVPvnName
                             }).ToList();
                return viewModel;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                provinceDao = null;
                provinceList = null;
                viewModel = null;
            }
        }

        public List<DistrictItemViewModel> getDistrictByProvinceCode(string provinceCode)
        {
            TSysSDistrictDao districtDao = new TSysSDistrictDao();
            List<TSysSDistrict> districtList = new List<TSysSDistrict>();
            List<DistrictItemViewModel> viewModel = new List<DistrictItemViewModel>();

            try
            {
                districtList = districtDao.getDistrictByProvinceCode(provinceCode);
                viewModel = (from it in districtList
                             orderby it.XVDstName
                             select new DistrictItemViewModel
                             {
                                 districtCode = it.XVDstCode,
                                 districtNameTH = it.XVDstName,
                                 provinceCode = it.XVPvnCode
                             }).ToList();
                return viewModel;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                districtDao = null;
                districtList = null;
                viewModel = null;
            }
        }

        public List<SubDistrictItemViewModel> getSubDistrictByDistrictCode(string districtCode)
        {
            TSysSSubDistrictDao subDistrictDao = new TSysSSubDistrictDao();
            List<TSysSSubDistrict> subDistrictList = new List<TSysSSubDistrict>();
            List<SubDistrictItemViewModel> viewModel = new List<SubDistrictItemViewModel>();

            try
            {
                subDistrictList = subDistrictDao.getSubDistrictByDistrictCode(districtCode);
                viewModel = (from it in subDistrictList
                             orderby it.XVSdtName
                             select new SubDistrictItemViewModel
                             {
                                 subDistrictCode = it.XVSdtCode,
                                 subDistrictNameTH = it.XVSdtName,
                                 postCode = it.XVSdtPostCode,
                                 districtCode = it.XVDstCode
                             }).ToList();
                return viewModel;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                subDistrictDao = null;
                subDistrictList = null;
                viewModel = null;
            }
        }
    }
}
