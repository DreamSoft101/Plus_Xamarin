using EXPRESSO.Models;
using EXPRESSO.Utils;
using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PLUS.iOS.Helpers
{
    public static class AccountHelper
    {
        public static MyEntity getMyEntity()
        {
            NSUserDefaults.StandardUserDefaults.Synchronize();
            var data = NSUserDefaults.StandardUserDefaults.StringForKey("MYENTITY");
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            var entity = JsonConvert.DeserializeObject<MyEntity>(data);
            Cons.api_key = entity.api_key;
            Cons.IdEntity = entity.idEntity;
            Cons.myEntity = entity;
            Loyalty.Utils.Cons.IdEntity = entity.idEntity;
            Loyalty.Utils.Cons.mMemberCredentials = entity.User.LoyaltyAccount;
            return entity;
        }

        public static void setMyEntity(MyEntity myentity)
        {
            string strJson = JsonConvert.SerializeObject(myentity);
            NSUserDefaults.StandardUserDefaults.SetString(strJson, "MYENTITY");
            NSUserDefaults.StandardUserDefaults.Synchronize();
            getMyEntity();
        }
    }
}