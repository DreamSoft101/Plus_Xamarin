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
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Dex.Com.Expresso.Loyalty.Droid.Utils;
using Dex.Com.Expresso.Fragments;
using Android.Graphics.Drawables;
using Android.Graphics;
using Square.Picasso;
using EXPRESSO.Threads;
using EXPRESSO.Models;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "LiveFeedDetailActivity", LaunchMode = Android.Content.PM.LaunchMode.Multiple, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LiveFeedDetailActivity : BaseActivity, ILocationListener
    {
        public static string MODELISFAVORITE = "ISFAVORITE";
        public static string MODETYPE = "MODELTYPE";
        public static int MODETYPE_HIGHWAY = 1;
        public static int MODETYPE_TOLLPLAZA = 2;
        public static string DATA = "DATA";
        public static string DATA_FAVORITE = "ISFAVORITE";
        private TrafficUpdate mUpdate;
        private TollPlazaCCTV mCCTV;
        private ImageView mImgCamera;
        private MapFragment mapFrag;
        private GoogleMap map;
        private Marker mMarker;
        private LinearLayout mLnlLoading, mLnlImage;
        private string _locationProvider;
        private LocationManager _locationManager;
        private TextView mTxtAway;
        private ImageView mImgShare;
        private ImageView mImgFavorite;
        private Bitmap mBitmap;
        private bool isfavorite;
        private int typecctv;
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.exp_activity_livefeed_detail;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            InitializeLocationManager();
            string jsonData = this.Intent.GetStringExtra(DATA);

            typecctv = this.Intent.GetIntExtra(MODETYPE,0);
            if (typecctv == MODETYPE_HIGHWAY)
            {
                mUpdate = JsonConvert.DeserializeObject<TrafficUpdate>(jsonData);
            }
            else
            {
                mCCTV = JsonConvert.DeserializeObject<TollPlazaCCTV>(jsonData);
            }

          
            mImgCamera = FindViewById<ImageView>(Resource.Id.imgCamera);
            mLnlImage = FindViewById<LinearLayout>(Resource.Id.lnlImage);
            mLnlLoading = FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            mTxtAway = FindViewById<TextView>(Resource.Id.txtAway);
            mImgFavorite = FindViewById<ImageView>(Resource.Id.imgFavorite);
            mImgShare = FindViewById<ImageView>(Resource.Id.imgShare);
            this.Title = mUpdate != null ? mUpdate.strDescription : mCCTV.strDescription;

            Location location = GPSUtils.getLastBestLocation(this);
            Android.Gms.Maps.Model.LatLng myTarget = mUpdate != null ?  new Android.Gms.Maps.Model.LatLng(mUpdate.decLat, mUpdate.decLng) : new Android.Gms.Maps.Model.LatLng(mCCTV.decLat, mCCTV.decLng);
            if (location != null)
            {
                Android.Gms.Maps.Model.LatLng myLoc = new Android.Gms.Maps.Model.LatLng(location.Latitude, location.Longitude);
                double distince = GPSUtils.Distance(myLoc, myTarget, GPSUtils.DistanceUnit.Kilometers);
                //mTxtAway.Text = string.Format(GetString(Resource.String.text_away), distince);
                mTxtAway.Text = "";
            }
            else
            {
                mTxtAway.Text = "";
            }


         
            this.mLnlLoading.Visibility = ViewStates.Gone;
            this.mLnlImage.Visibility = ViewStates.Visible;

            

            Picasso.With(this).Load(Android.Net.Uri.Parse(mUpdate != null ? mUpdate.strURL : mCCTV.strCCTVImage)).Error(Resource.Drawable.img_error).Into(mImgCamera, new Action(() =>
            {
                this.mLnlLoading.Visibility = ViewStates.Gone;
                this.mLnlImage.Visibility = ViewStates.Visible;
            }), null);

            addMap(mUpdate);


            isfavorite = this.Intent.GetBooleanExtra(DATA_FAVORITE, false);
            if (isfavorite)
            {
                mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite);
            }
            else
            {
                mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
            }

            mImgFavorite.Click += MImgFavorite_Click;
            mImgShare.Click += MImgShare_Click;
            // Create your application here
        }

        private void MImgShare_Click(object sender, EventArgs e)
        {
            mImgCamera.BuildDrawingCache(true);
            // throw new NotImplementedException();
            Bitmap bitmap = mImgCamera.GetDrawingCache(true);
            string filename = Expresso.Utils.ImageUtils.ExportBitmapAsPNG(bitmap);

            //var imageUri = Android.Net.Uri.Parse("file://" + filename);
            var intent = new Intent();
            intent.SetType("image/jpeg");
            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + filename));
            intent.PutExtra(Intent.ExtraSubject, mUpdate != null ?  mUpdate.strDescription : mCCTV.strDescription);
            intent.PutExtra(Intent.ExtraTitle, mUpdate != null ? mUpdate.strDescription : mCCTV.strDescription);
            intent.PutExtra(Intent.ExtraText, mUpdate != null ? mUpdate.strDescription : mCCTV.strDescription);
            StartActivity(intent);

        }

        private void MImgFavorite_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            FavoriteThread thread = new FavoriteThread();
            if (mUpdate != null)
            {
                thread.IsToggle(mUpdate);
            }
            else
            {
                thread.IsToggle(mCCTV);
            }
            isfavorite = !isfavorite;
            if (isfavorite)
            {
                mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite);
            }
            else
            {
                mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
            }
        }

        private void addMap(TrafficUpdate mUpdate)
        {
            Fragment_Maps f = Fragment_Maps.NewInstance(new LatLng(mUpdate != null ? mUpdate.decLat : mCCTV.decLat, mUpdate != null ? mUpdate.decLng : mCCTV.decLng), mUpdate != null ? mUpdate.strDescription : mCCTV.strDescription);
            f.isRunAuto = false;
            f.onGetMap += OnGetMap;
            Android.Support.V4.App.FragmentTransaction fragmentTransaction = SupportFragmentManager.BeginTransaction();
            fragmentTransaction.Replace(Resource.Id.frgContent, f);
            fragmentTransaction.Commit();

            //return f.getMap();
        }

        public void OnGetMap(GoogleMap map, SupportMapFragment mapFrag)
        {
            this.map = map;
            map.UiSettings.MyLocationButtonEnabled = true;
            //map.UiSettings.MapToolbarEnabled = false;

            View toolbar = ((View)mapFrag.View.FindViewById(1).Parent).FindViewById(4);
            //getParent()).findViewById(Integer.parseInt("4"));

            //// and next place it, for example, on bottom right (as Google Maps app)
            RelativeLayout.LayoutParams rlp = (RelativeLayout.LayoutParams)toolbar.LayoutParameters;
            //// position on right bottom
            rlp.AddRule(LayoutRules.AlignParentTop, 0);
            rlp.AddRule(LayoutRules.AlignParentBottom, 0);
            //rlp.addRule(RelativeLayout.ALIGN_PARENT_BOTTOM, RelativeLayout.TRUE);
            rlp.SetMargins(30, 30, 30, 30);

            Android.Gms.Maps.Model.LatLng myTarget = mUpdate != null ? new Android.Gms.Maps.Model.LatLng(mUpdate.decLat, mUpdate.decLng) : new Android.Gms.Maps.Model.LatLng(mCCTV.decLat, mCCTV.decLng);
            if (myTarget != null)
            {
                UpdateItemLocation(myTarget, mUpdate != null ? mUpdate.strDescription : mCCTV.strDescription);
            }
        }

        private class LoaderTarget : Java.Lang.Object, ITarget
        {
            private ImageView img;
            //private int intPosition;
            //private onLoadedImage OnLoadedImage;
            public LoaderTarget(ImageView view)
            {
                img = view;
            }


            public void OnBitmapFailed(Drawable p0)
            {
                img.SetImageResource(Resource.Drawable.img_error);
                //if (OnLoadedImage != null)
                //{
                //    OnLoadedImage(null, intPosition);
                //}
            }

            public void OnBitmapLoaded(Bitmap p0, Picasso.LoadedFrom p1)
            {
                img.SetImageBitmap(p0);
            
            }

            public void OnPrepareLoad(Drawable p0)
            {

            }
        }

        public void UpdateItemLocation(LatLng location, string strTitle)
        {
            MarkerOptions option = new MarkerOptions();
            option.SetPosition(new LatLng(location.Latitude, location.Longitude));
            option.SetTitle(strTitle);
            var mk = map.AddMarker(option);
            mk.ShowInfoWindow();
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(10);
            //builder.Bearing(155);
            //builder.Tilt(65);
            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            if (map != null)
            {
                map.MoveCamera(cameraUpdate);
            }


        }

        public void OnLocationChanged(Location location)
        {
            Android.Gms.Maps.Model.LatLng myTarget = mUpdate != null ? new Android.Gms.Maps.Model.LatLng(mUpdate.decLat, mUpdate.decLng) : new Android.Gms.Maps.Model.LatLng(mCCTV.decLat, mCCTV.decLng);
            if (location != null)
            {
                Android.Gms.Maps.Model.LatLng myLoc = new Android.Gms.Maps.Model.LatLng(location.Latitude, location.Longitude);
                double distince = GPSUtils.Distance(myLoc, myTarget, GPSUtils.DistanceUnit.Kilometers);
                //mTxtAway.Text = string.Format(GetString(Resource.String.text_away), distince);
                mTxtAway.Text = "";
            }
            else
            {
                mTxtAway.Text = "";
            }
        }

        public void OnProviderDisabled(string provider)
        {

        }

        public void OnProviderEnabled(string provider)
        {

        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {

        }

        public void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
        }

        public override void OnBackPressed()
        {
            Intent intent = new Intent();
            intent.PutExtra(MODELISFAVORITE, isfavorite);
            SetResult(Result.Ok, intent);
            base.OnBackPressed();
        }
    }
}