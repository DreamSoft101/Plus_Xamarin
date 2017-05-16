using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Utils
{
    public static class StringExtentions
    {
    

        public static string AsString(this JToken value)
        {
            if (value == null)
            {
                return "";
            }
            else
            {
                return (string)value.ToString();
            }
        }

        public static bool AsBool(this JToken value)
        {
            try
            {
                if (value == null)
                {
                    return false;
                }
                else
                {

                    return (bool)value;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }

        public static int AsInt(this JToken value)
        {
            try
            {
                if (value == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(value.ToString());
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
          
        }

        public static double AsDouble(this JToken value)
        {
            try
            {
                if (value == null)
                {
                    return 0;
                }
                else
                {
                    return double.Parse(value.ToString());
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
           
        }

        public static decimal AsDecimal(this JToken value)
        {
            try
            {
                if (value == null)
                {
                    return 0;
                }
                else
                {
                    return decimal.Parse(value.ToString());
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
          
        }

        public static DateTime AsDateTime(this JToken value)
        {
            if (value == null)
            {
                return new DateTime();
            }
            else
            {
                return DateTime.Parse(value.ToString());
            }
        }

    }
}
