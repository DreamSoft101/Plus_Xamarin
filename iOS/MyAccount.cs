using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PLUS.iOS
{
    public class MyAccount : Xamarin.Auth.Account
    {
        public string idEntity;
        public UserInfos User;
        public TblEntities Entity;
        public string api_id;
        public string api_key;
        public string cctv_encryption;
        public string sub_domain;
        public string cctv_secret_key;
        public Settings mSettings;
    }
}