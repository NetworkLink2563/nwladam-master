using NWL_AdAm_SPL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWL_AdAm_DML.ViewModel
{
    public class LampStatusViewModel
    {
        public string projectType { get; set; } = string.Empty;
        public string controllerCode { get; set; } = string.Empty;
        public string lampSerialNo { get; set; } = string.Empty;
        public string lampName { get; set; } = string.Empty;
        public DateTime updatedAt { get; set; } = DateTime.MinValue;
        public int relay { get; set; } = 0;
        public string relayName
        {
            get
            {
                switch (relay)
                {
                    case 0:
                        return "ปิด";
                    case 1:
                        return "เปิด";
                    default:
                        return "ไม่เชื่อมต่อ";
                }
            }
        }
        public int pwm1 { get; set; } = 0;
        public int pwm2 { get; set; } = 0;
        public int signal { get; set; } = 0;
        public int current { get; set; } = 0;
        public int ambientLight { get; set; } = 0;
        public int mode { get; set; } = 0;
        public string modeName
        {
            get
            {
                switch (mode)
                {
                    case Variables.MQTT_MODE_DEBUG:
                        return "DEBUG";
                    case Variables.MQTT_MODE_CONFIG:
                        return "CONFIG";
                    case Variables.MQTT_MODE_MANUAL:
                        return "MANUAL";
                    case Variables.MQTT_MODE_SET_SCHEDULE:
                        return "SET_SCHEDULE";
                    case Variables.MQTT_MODE_AMBIENT_LIGHT:
                        return "AMBIENT_LIGHT";
                    case Variables.MQTT_MODE_SCHEDULER_WITH_AMBIENT_LIGHT:
                        return "SCHEDULER_WITH_AMBIENT_LIGHT";
                    default:
                        return mode.ToString();
                }
            }
        }
        public string modeDescription
        {
            get
            {
                switch (mode)
                {
                    case Variables.MQTT_MODE_DEBUG:
                        return "DEBUG";
                    case Variables.MQTT_MODE_CONFIG:
                        return "CONFIG";
                    case Variables.MQTT_MODE_MANUAL:
                        return "กำหนดการทำงานด้วยตนเอง";
                    case Variables.MQTT_MODE_SET_SCHEDULE:
                        return "ตั้งเวลาทำงานอัตโนมัติ";
                    case Variables.MQTT_MODE_AMBIENT_LIGHT:
                        return "AMBIENT_LIGHT";
                    case Variables.MQTT_MODE_SCHEDULER_WITH_AMBIENT_LIGHT:
                        return "SCHEDULER_WITH_AMBIENT_LIGHT";
                    default:
                        return mode.ToString();
                }
            }
        }
    }
}
