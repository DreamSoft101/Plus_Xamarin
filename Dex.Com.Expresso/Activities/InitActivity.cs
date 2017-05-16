using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EXPRESSO.Threads;
using EXPRESSO.Models;
using EXPRESSO.Utils;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "InitActivity")]
    public class InitActivity : BaseActivity
    {
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.exp_activity_init_fisttime;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            UsersThreads thread = new UsersThreads();
            thread.OnLoadAPIKey += (ServiceResult result) =>
            {
                if (result.intStatus == 1)
                {
                    var lstEntity = new List<MyEntity>();
                    MyEntity myentity = result.Data as MyEntity;
                    myentity.Entity = new EXPRESSO.Models.Database.TblEntities() { idEntity = myentity.idEntity, strName = "PLUS" };
                    myentity.User = new UserInfos();
                    myentity.User.strUserName = string.Format(GetString(Resource.String.useasanonymouse), myentity.Entity.strName);
                    lstEntity.Add(myentity);
                    saveMyEntity(lstEntity);
                    saveCurrentMyEntity(myentity);

                    Finish();
                }
                else
                {
                    Toast.MakeText(this, result.strMess, ToastLength.Short).Show();
                }
            };
            thread.loadEntityInfo(Cons.PLUSEntity.ToString());

            // Create your application here
        }
    }
}