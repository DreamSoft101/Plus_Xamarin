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
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    [Activity(Label = "ListMemberActivity")]
    public class ListMemberActivity : BaseActivity
    {
        private ListView mLstData;

        protected override int LayoutResource
        {
            get
            {
                ///return Resource.Layout.loy_activity_listmember;
                ///ret
                /// 


                return 0;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLstData = FindViewById<ListView>(Resource.Id.lstData);

           
            // Create your application here
        }
    }
}