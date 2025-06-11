using System;
using System.Configuration;

namespace NWL_AdAm_SPL
{
    public class Variables
    {
        public const string USER_PROFILE = "USER_PROFILE";

        public static int ITEM_PER_PAGE
        {
            get
            {
                string itemPerPage = ConfigurationManager.AppSettings["itemPerPage"];
                return String.IsNullOrEmpty(itemPerPage) ? 20 : Convert.ToInt32(itemPerPage);
            }
        }

        public const string PROJECT_TYPE_LORA = "1";
        public const string PROJECT_TYPE_EMM = "2";
        public const string PROJECT_TYPE_NB_NODE = "3";

        public const string ROLE_VIEWER = "1";
        public const string ROLE_ADMIN = "2";
        public const string ROLE_ENGINEER = "3";

        public const string CONFIG_EMM_HEALTH_CHECK = "900";


        //relate to ../MQTTClient/endpoint.json
        public const int MQTT_MODE_DEBUG = -1;
        public const int MQTT_MODE_CONFIG = 0;
        public const int MQTT_MODE_MANUAL = 1;
        public const int MQTT_MODE_SET_SCHEDULE = 2;
        public const int MQTT_MODE_AMBIENT_LIGHT = 3;
        public const int MQTT_MODE_SCHEDULER_WITH_AMBIENT_LIGHT = 4;



    }
}
