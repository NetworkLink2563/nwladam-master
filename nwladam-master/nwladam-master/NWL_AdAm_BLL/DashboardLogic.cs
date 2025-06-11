using Newtonsoft.Json.Linq;
using NWL_AdAm_DAL;
using NWL_AdAm_DML;
using NWL_AdAm_DML.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace NWL_AdAm_BLL
{
    public class DashboardLogic
    {
        public List<LampStatusDashboardModel> getLampStatusDashboardData(string customerCode, string projectCode)
        {
            DashboardDao dashboardDao = new DashboardDao();

            try
            {
                if (string.IsNullOrEmpty(projectCode))
                {
                    projectCode = "";
                }

                List<STP_NWLShowOnOffStatus_Result> result = dashboardDao.getLampStatusDashboardData(customerCode, projectCode);
                if (result != null)
                {
                    ///--- LampStatus 0:On, 1:Off, 2:Unreachable
                    return result.GroupBy(it => it.XVPrjCode)
                        .Select(o => new LampStatusDashboardModel() 
                        { 
                            projectCode = o.Key,
                            projectName = o.FirstOrDefault().XVPrjName,
                            lampOff = o.Count(lamp => lamp.XILmpStatus == 0),
                            lampOn = o.Count(lamp => lamp.XILmpStatus == 1),
                            lampUnreachable = o.Count(lamp => lamp.XILmpStatus == 2)
                        }).ToList();
                }
                else
                {
                    return new List<LampStatusDashboardModel>();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<JobStatusDashboardModel> getJobStatusDashboardData(string customerCode, string projectCode)
        {
            DashboardDao dashboardDao = new DashboardDao();

            try
            {
                if (string.IsNullOrEmpty(projectCode))
                {
                    projectCode = "";
                }

                List<STP_NWLShowJobStatus_Result> result = dashboardDao.getJobStatusDashboardData(customerCode, projectCode);
                if (result != null)
                {
                    return (from it in result
                            select new JobStatusDashboardModel()
                            {
                                projectCode = it.XVPrjCode,
                                projectName = it.XVPrjName,
                                completedJob = it.XIJobComplete,
                                notCompletedJob = it.XIJobNotComplete
                            }).ToList();
                }
                else
                {
                    return new List<JobStatusDashboardModel>();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<EMMStatusDashboardModel> getEMMStatusDashboardData(string customerCode, string projectCode)
        {
            DashboardDao dashboardDao = new DashboardDao();

            try
            {
                if (string.IsNullOrEmpty(projectCode))
                {
                    projectCode = "";
                }

                List<STP_NWLShowEMMStatus_Result> result = dashboardDao.getEMMStatusDashboardData(customerCode, projectCode);
                if (result != null)
                {
                    return (from it in result
                            select new EMMStatusDashboardModel()
                            {
                                projectCode = it.XVPrjCode,
                                projectName = it.XVPrjName,
                                controllerSerialNo = it.XVCtlSN,
                                current = it.XFCstCurrent.HasValue ? it.XFCstCurrent.Value : 0.0,
                                voltage = it.XFCstVoltage.HasValue ? it.XFCstVoltage.Value : 0.0,
                                power = it.XFCstPower.HasValue ? it.XFCstPower.Value : 0.0,
                            }).ToList();
                }
                else
                {
                    return new List<EMMStatusDashboardModel>();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<WeatherDashboardModel> getWeatherDashboardData(GeoLocationModel location)
        {
            WebRequest httpRequest;
            WebResponse httpResponse;
            string response;
            JObject jsonResponse;
            JObject jsonWeatherForecasts;

            try
            {
                httpRequest = (HttpWebRequest)WebRequest.Create("https://data.tmd.go.th/nwpapi/v1/forecast/location/daily/at?lat=" + location.latitude + "&lon=" + location.longitude + "&fields=tc_max,tc_min,cond");
                httpRequest.Method = "GET";
                httpRequest.Headers.Add("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6IjkyYThiODg1ZDI0M2RmNDkwZGQ2YjZhMmE4ZWJiZGMxNDUxMzg0NTk0YjM0OGRiYWM3OTk5NDA0ODE0YjJiY2FkZTFhMDY3ZDc5OTI1ZjIyIn0.eyJhdWQiOiIyIiwianRpIjoiOTJhOGI4ODVkMjQzZGY0OTBkZDZiNmEyYThlYmJkYzE0NTEzODQ1OTRiMzQ4ZGJhYzc5OTk0MDQ4MTRiMmJjYWRlMWEwNjdkNzk5MjVmMjIiLCJpYXQiOjE2MzAwNTU3MzMsIm5iZiI6MTYzMDA1NTczMywiZXhwIjoxNjYxNTkxNzMzLCJzdWIiOiIxNTkzIiwic2NvcGVzIjpbXX0.aB3d_RJrnMllYTn2FA9mUycEQ3age4j2jV9e4Y6g-zwAo2R1MZLss5gZJUVsqmfQVW7es0JYlPpZnxPmwYXj1j2zi9Hbru1O4Egzhy1kazrUJdh6GKEqBDutz-Rc51NpWJhuRWrr-yU54B1nB_9JepYiIHeeaj1Ma9oKR_nCWGdU15KyY7KaZq_SUvyeFntxH0iZqgwfnRl2mBuCfJk4HtvQkJikUuHrM-cn8HbEyL-OIKeoiorb9jnoubWQ42FhuXXxG01F_d0SQ8UaBQcBiYvK520Djb8Bg3sdKw9kKxupIRAIwNWIv9Myz4pFA1KLMwXajzlqgC5VL5FUy021FVvu_risWfRmvgr9RjdgIdVKpUwgTBF_60W_ZbnxB6OwrSCLd6dE09jdinNTaFxgdISqgkb7UID23rb5m_G4s1TW3FKpxobncLOn4C3k9XdKKDBPJVWA4V0B_tSZOzmmVuPbygQ74LtmOcQ7lr4lXmM8uM0yxxZ0sF8CNXqJ3SgQxuglRgeC2xrpyBK1AEOfl83R2qLT4SkbiKySTuT8FxTc88c1VfUNEWKS8FVqBM3WhzWLduGfvAptj_VXC4o8TNJX6DZqgnj88SAwU-Mt8m-UEaWm8YmaZFBJoAdlgsCykFV4UiKsTNi7me1cOMg-5-DRHFuQL2N3WkUheHepOjg");

                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                response = new StreamReader(httpResponse.GetResponseStream()).ReadToEnd();

                jsonResponse = JObject.Parse(response);
                jsonWeatherForecasts = JObject.Parse(jsonResponse.SelectToken("WeatherForecasts[0].forecasts[0]")["data"].ToString());

                int currentHour = DateTime.Now.Hour;
                bool isNight = currentHour <= 5 || currentHour >= 18;

                WeatherDashboardModel result = new WeatherDashboardModel();
                result.lowTemp = jsonWeatherForecasts.Property("tc_min").Value.ToString();
                result.highTemp = jsonWeatherForecasts.Property("tc_max").Value.ToString();
                switch (jsonWeatherForecasts.Property("cond").Value.ToString())
                {
                    case "1":
                        result.weatherStatus = "ท้องฟ้าแจ่มใส (Clear)";
                        result.imgPath = "/images/weather_icon/1_" + (!isNight ? "day" : "night") + ".png";
                        break;
                    case "2":
                        result.weatherStatus = "มีเมฆบางส่วน (Partly cloud)";
                        result.imgPath = "/images/weather_icon/2_" + (!isNight ? "day" : "night") + ".png";
                        break;
                    case "3":
                        result.weatherStatus = "มีเมฆเป็นส่วนใหญ่ (Cloudy)";
                        result.imgPath = "/images/weather_icon/3_" + (!isNight ? "day" : "night") + ".png";
                        break;
                    case "4":
                        result.weatherStatus = "มีเมฆมาก (Overcast)";
                        result.imgPath = "/images/weather_icon/4.png";
                        break;
                    case "5":
                        result.weatherStatus = "ฝนตกเล็กน้อย (Light rain)";
                        result.imgPath = "/images/weather_icon/5.png";
                        break;
                    case "6":
                        result.weatherStatus = "ฝนตกปานกลาง (Moderate rain)";
                        result.imgPath = "/images/weather_icon/6.png";
                        break;
                    case "7":
                        result.weatherStatus = "ฝนตกหนัก (Heavy rain)";
                        result.imgPath = "/images/weather_icon/7.png";
                        break;
                    case "8":
                        result.weatherStatus = "ฝนฟ้าคะนอง (Thunderstorm)";
                        result.imgPath = "/images/weather_icon/8.png";
                        break;
                    case "9":
                        result.weatherStatus = "อากาศหนาวจัด (Very cold)";
                        result.imgPath = "/images/weather_icon/9.png";
                        break;
                    case "10":
                        result.weatherStatus = "อากาศหนาว (Cold)";
                        result.imgPath = "/images/weather_icon/10.png";
                        break;
                    case "11":
                        result.weatherStatus = "อากาศเย็น (Cool)";
                        result.imgPath = "/images/weather_icon/11.png";
                        break;
                    case "12":
                        result.weatherStatus = "อากาศร้อนจัด (Very hot)";
                        result.imgPath = "/images/weather_icon/12.png";
                        break;
                }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                httpRequest = null;
                httpResponse = null;
                response = null;
                jsonResponse = null;
                jsonWeatherForecasts = null;
            }
        }
    }
}
