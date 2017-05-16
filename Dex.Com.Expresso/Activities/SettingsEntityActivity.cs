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
using Dex.Com.Expresso.Adapters.Listview;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "SettingsEntityActivity")]
    public class SettingsEntityActivity : BaseActivity
    {
        private ListView mLstHighway;
        private HighwaySettingAdapter mAdapter;
        
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.activity_setting_entity;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.mLstHighway = FindViewById<ListView>(Resource.Id.lstHighway);

            MastersThread thread = new MastersThread();
            thread.OnLoadHighway += (result) =>
            {
                mAdapter = new HighwaySettingAdapter(this, result);
                this.mLstHighway.Adapter = mAdapter;
            };
            thread.loadHighway();
            // Create your application here
        }
    }
}