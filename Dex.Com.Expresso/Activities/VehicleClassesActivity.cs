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
    [Activity(Label = "VehicleClassesActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class VehicleClassesActivity : BaseActivity
    {
        private ListView mLstCar;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.activity_vehicleclasses;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Toolbar.NavigationClick += Toolbar_NavigationClick;
            this.Title = GetString(Resource.String.title_vehicle_classes);
            this.mLstCar = FindViewById<ListView>(Resource.Id.lstCar);

            TollFareThread thread = new TollFareThread();
            thread.OnLoadVehicleClasses += (result) =>
            {
                VehicleClassesAdapter adapter = new VehicleClassesAdapter(this, result);
                this.mLstCar.Adapter = adapter;
            };
            thread.loadVehicleClasses();
            // Create your application here
        }

        private void Toolbar_NavigationClick(object sender, Android.Support.V7.Widget.Toolbar.NavigationClickEventArgs e)
        {
            OnBackPressed();
        }
    }
}