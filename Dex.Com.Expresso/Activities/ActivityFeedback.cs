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
using EXPRESSO.Models.Database;
using Newtonsoft.Json;
using EXPRESSO.Utils;
using Dex.Com.Expresso.Fragments;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "LoginActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ActivityFeedback : BaseActivity
    {
        private TrafficUpdate mItem;
        public static string MODEL_DATA = "modeldata";

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.activity_feedback;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = GetString(Resource.String.title_feedback);

            string jsonData = this.Intent.GetStringExtra(MODEL_DATA);
            if (string.IsNullOrEmpty(jsonData))
            {
                Finish();
                return;
            }

            mItem = StringUtils.String2Object<TrafficUpdate>(jsonData);
            if (mItem == null)
            {
                Finish();
                return;
            }

            Android.Support.V4.App.FragmentTransaction fragmentTransaction = SupportFragmentManager.BeginTransaction();
            fragmentTransaction.Replace(Resource.Id.frgContent, Fragment_Feedback.NewInstance(new EXPRESSO.Models.BaseItem() { Item = mItem }));
            fragmentTransaction.Commit();
        }
    }
}