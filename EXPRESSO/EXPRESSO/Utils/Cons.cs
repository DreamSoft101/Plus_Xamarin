using EXPRESSO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Utils
{
    public static class Cons
    {
        public static string API_URL = "http://api.dev.expresso.cloud/service/v1";
        public static string IMG_URL_PLUS = "http://img-plus.aviven.net/img/operations/";
        public static MyEntity myEntity;
        public static int PLUSEntity = 15;
        //public static int PLUSEntity = 1;

        public static string Emz_URL = "https://plustrafik.plus.com.my/assets/magazines/";

        public enum DebugType
        {
            LogAll = 3,
            LogCat = 1,
            LogFile = 2,
            NoDebug = 0,
        }

        private static string mIdEntity;
        public static string IdEntity
        {
            get
            {
                return mIdEntity;
            }
            set
            {
                mIdEntity = value;
            }
        }


        public static string API_URL_PLUS = "http://plus-v3.aviven.net/push/service/v4/";
        public static string app_id = "c32d2e25"; //237392269769590
        public static string api_key = "f8a3f542424e727b48c1fbae96625ca6";

        public static FtpSettings mFtpSettings;
    }

    
}
