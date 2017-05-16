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
using Android.Content.PM;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    [Activity(Label = "FirstActivity")]
    public class FirstActivity : BaseActivity
    {
       

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.loy_activity_first;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            bool isFirst = getCacheInt(IsFistTime) == int.MinValue;

            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);

            if (isFirst)
            {
                Intent intent1 = new Intent(this, typeof(SplashActivity));
                StartActivity(intent1);
            }

            Finish();
              
            

            // Create your application here
        }
    }
}