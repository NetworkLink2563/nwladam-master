using Newtonsoft.Json.Linq;
using NWL_AdAm_DAL;
using NWL_AdAm_DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWL_AdAm_BLL
{
    public class ConfigLogic
    {
        public string getMqttPublisherEndpoint(string method)
        {
            TSysSConfigDao configDao = new TSysSConfigDao();

            try
            {
                JObject jsonEndpoint = configDao.getMqttPublisherEndpoint();
                if (jsonEndpoint != null)
                {
                    if (!string.IsNullOrEmpty(method))
                    {
                        return jsonEndpoint[method].ToString();
                    }
                    else
                    {
                        throw new Exception("ไม่พบ Mqtt endpoint \"" + method + "\"");
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
