using Newtonsoft.Json.Linq;
using NWL_AdAm_DML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace NWL_AdAm_DAL
{
    public class TSysSConfigDao
    {
        public JObject getMqttPublisherEndpoint()
        {
            try
            {
                if (File.Exists(HostingEnvironment.MapPath("~/MQTTClient/endpoint.json")))
                {
                    return JObject.Parse(File.ReadAllText(HostingEnvironment.MapPath("~/MQTTClient/endpoint.json")));
                }
                else
                {
                    throw new Exception("ไม่พบข้อมูล MQTT Client Enpoint");
                }
                
                //using (AdAmEntities db = new AdAmEntities())
                //{
                //    return db.TSysSConfig.Where(it => it.XVSysName.ToUpper().Contains("MQTTPUBLISHER")).ToList();
                //}
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TSysSConfig getConfigByCode(string configCode)
        {
            try
            {
                using (AdAmEntities db = new AdAmEntities())
                {
                    return db.TSysSConfig.Where(it => it.XVSysCode == configCode).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
