using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWL_AdAm_SPL
{
    public class Converter
    {
        public static string convertBoolToString(bool? value)
        {
            if (!value.HasValue)
            {
                return "0";
            }

            return value.Value ? "1" : "0";
        }

    }
}
