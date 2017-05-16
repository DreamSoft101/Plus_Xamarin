using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Utils
{
    public static class StringUtils
    {
        public static string RandomString(int Len = 5, bool isNumber = false)
        {
            try
            {
                string result = "";
                Random rand = new Random();
                while (result.Length < Len)
                {
                    if (isNumber)
                    {
                        result += rand.Next(1, 3) == 1 ? 
                                (char)rand.Next(48,58) : 
                                rand.Next(1, 3) == 1 ? (char)rand.Next(65, 91) : (char)rand.Next(97, 123);
                    }
                    else
                    {
                        result += rand.Next(1, 3) == 1 ? (char)rand.Next(65, 91) : (char)rand.Next(97, 123);
                    }
                }
                LogUtils.WriteLog("RandomString", result);
                return result;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("RandomString", ex.Message);
            }
            return "";
        }
    }
}
