using Loyalty.Models;
using Loyalty.Models.ServiceOutput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Loyalty.Utils
{
    public static class Cons
    {
        //public static string API_URL = "http://plusloyaltyapi.dexperien.com/Json/";
        //public static string API_IMG_URL = "http://plusloyaltyapi.dexperien.com/Images?id=";
        public static string API_URL = "http://backend.plus.dexperien.com:6789/Json/";
        public static string API_IMG_URL = "http://backend.plus.dexperien.com:6789/Images?id=";
        public static string API_PDF_URL = "http://backend.plus.dexperien.com:6789/Images/DownEstatement/?id=";
        public static Guid APIid = new Guid("c08e2228-5f2f-4095-a3bc-888ef4d078ca");

        //public static string API_URL = "http://beetlebuck.dexperien.com:88//Json/";
        //public static string API_IMG_URL = "http://beetlebuck.dexperien.com:88/Images?id=";
        //public static string API_PDF_URL = "http://beetlebuck.dexperien.com:88/Images/DownEstatement/?id=";
        //public static Guid APIid = new Guid("c08e2228-5f2f-4095-a3bc-888ef4d078ca");
        public static string APIKey = "2222";

        public static List<ItemPreferences> Preferences;
        public static MValidateMemberCredentials mMemberCredentials;
        
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
    }
}