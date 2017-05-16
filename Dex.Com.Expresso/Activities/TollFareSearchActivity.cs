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
using Dex.Com.Expresso.Fragments;
using EXPRESSO.Threads;
using EXPRESSO.Models;
using Dex.Com.Expresso.Adapters.Listview;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "TollFareSearchActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class TollFareSearchActivity : BaseActivity
    {
        private string mIdRoute;
        private int mIntStartSeq;
        private int mIntEndSeq;
        private double mDbRate;
        private int mIntVheClass;
        private TextView mTxtRate;
        private ListView mLsvStreet;
        private ListView mLsvJourney;
        private ImageView mImgCar;
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.activity_tollfare_search_result;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Toolbar.NavigationClick += Toolbar_NavigationClick;
            this.Toolbar.Title = GetString(Resource.String.title_tollfare_result);
            // Create your application here

            this.mLsvStreet = FindViewById<ListView>(Resource.Id.lv_base);
            this.mLsvJourney = FindViewById<ListView>(Resource.Id.lv_journey);
            this.mImgCar = FindViewById<ImageView>(Resource.Id.iv_car);
            mIdRoute = this.Intent.GetStringExtra(Fragment_TollFare.IDROUND);
            mIntStartSeq = this.Intent.GetIntExtra(Fragment_TollFare.INTSTARTSEQ,0);
            mIntEndSeq = this.Intent.GetIntExtra(Fragment_TollFare.INTENDSEQ, 0);
            mDbRate = this.Intent.GetDoubleExtra(Fragment_TollFare.DECRATE, 0);
            mIntVheClass = this.Intent.GetIntExtra(Fragment_TollFare.INTVHECLASS,0);

            if (mIntEndSeq == 0 || mIntEndSeq == 0 || mIntVheClass == 0 || mDbRate == 0 || mDbRate == -1)
            {
                Toast.MakeText(this, GetString(Resource.String.toll_mess_notfound), ToastLength.Short).Show();
                Finish();
            }
            else
            {
                mTxtRate = FindViewById<TextView>(Resource.Id.txtRate);
                mTxtRate.Text = String.Format(GetString(Resource.String.toll_fare_rate_format), mDbRate);

                TollFareThread threadTollFare = new TollFareThread();
                threadTollFare.OnSearch += (result) =>
                {
                    JourneyStreetAdapter streetAdapter = new JourneyStreetAdapter(this);
                    mLsvStreet.Adapter = streetAdapter;
                    JourneyAdapter journeyAdapter = new JourneyAdapter(this, result);
                    mLsvJourney.Adapter = journeyAdapter;

                    switch (mIntVheClass)
                    {
                        case 1:
                            this.mImgCar.SetImageResource(Resource.Drawable.ic_car_class1);
                            break;
                        case 2:
                            this.mImgCar.SetImageResource(Resource.Drawable.ic_car_class2);
                            break;
                        case 3:
                            this.mImgCar.SetImageResource(Resource.Drawable.ic_car_class3);
                            break;
                        case 4:
                            this.mImgCar.SetImageResource(Resource.Drawable.ic_car_class4);
                            break;
                        case 5:
                            this.mImgCar.SetImageResource(Resource.Drawable.ic_car_class5);
                            break;
                    }

                };
                threadTollFare.doSearch(mIdRoute, mIntStartSeq, mIntEndSeq);
            }

            
        }

        private void Toolbar_NavigationClick(object sender, Android.Support.V7.Widget.Toolbar.NavigationClickEventArgs e)
        {
            OnBackPressed();
        }
    }
}