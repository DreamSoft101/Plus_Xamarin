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
using Android.Locations;
using Dex.Com.Expresso.Utils;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using EXPRESSO.Utils;
using EXPRESSO.Threads;
using EXPRESSO.Models;
using Android.Graphics;
using Dex.Com.Expresso.Fragments;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "LiveTrafficDetailActivity")]
    public class LiveTrafficDetailActivity :   BaseActivity, ILocationListener
    {
        private TextView mTxtTitle;
        private TextView mTxtLocation;
        private TextView mTxtTime;
        private TextView mTxtAway;
        private ImageView mImgIcon;


        public static string DATA = "Data";
        public TrafficUpdate mUpdate;

        private MapFragment mapFrag;
        private GoogleMap map;
        private Marker mMarker;

        private string _locationProvider;
        private LocationManager _locationManager;

        private ImageView mImgLiveTraffic6;
        private ImageView mImgDirection;


        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.exp_activity_livetraffic_detail_v2;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            InitializeLocationManager();
            this.Toolbar.Title = GetString(Resource.String.title_livetrafficdetail);
            string data = this.Intent.GetStringExtra(DATA);
            mUpdate = JsonConvert.DeserializeObject<TrafficUpdate>(data);
            if (mUpdate == null)
            {
                Finish();
            }

            mImgIcon = FindViewById<ImageView>(Resource.Id.imgIcon);

            mTxtTitle = FindViewById<TextView>(Resource.Id.txtTitle);
            mTxtLocation = FindViewById<TextView>(Resource.Id.txtLocation);
            mTxtTime = FindViewById<TextView>(Resource.Id.txtDate);
            mTxtAway = FindViewById<TextView>(Resource.Id.txtAWay);
            mImgLiveTraffic6 = FindViewById<ImageView>(Resource.Id.imgLiveTraffic6);
            mImgDirection = FindViewById<ImageView>(Resource.Id.imgDirection);
            mTxtTitle.Text = mUpdate.strTitle;
            mTxtLocation.Text = string.Format(GetString(Resource.String.facility_detail_location_format), mUpdate.decLocation);
            mTxtTime.Text = mUpdate.dtStartDateTime.ToString(GetString(Resource.String.loy_format_date_time));

            Location location = GPSUtils.getLastBestLocation(this);
            Android.Gms.Maps.Model.LatLng myTarget = new Android.Gms.Maps.Model.LatLng(mUpdate.decLat, mUpdate.decLng);
            if (location != null)
            {
                Android.Gms.Maps.Model.LatLng myLoc = new Android.Gms.Maps.Model.LatLng(location.Latitude, location.Longitude);
                double distince = GPSUtils.Distance(myLoc, myTarget, GPSUtils.DistanceUnit.Kilometers);
                mTxtAway.Text = string.Format(GetString(Resource.String.text_away), distince);
            }
            else
            {
                mTxtAway.Text = "";
            }

            int icon = ResourceUtil.GetResourceID(this, "ic_menu_traffic_type" + mUpdate.intType);
            if (icon != 0)
            {
                mImgIcon.SetImageResource(icon);
            }
            else
            {
                LogUtils.WriteError("LiveTrafficAdapter", "Type: " + mUpdate.intType);
            }

            addMap(mUpdate);
            //map.UiSettings.MapToolbarEnabled = false;
            //if (myTarget != null)
            //{
            //    UpdateItemLocation(myTarget, mUpdate.strTitle);
            //}

           

            mImgLiveTraffic6.Visibility = ViewStates.Gone;
            if (mUpdate.intType != 6)
            {
                LiveTrafficThread thread = new LiveTrafficThread();
                thread.OnLoadLiveTrafficResult += (ServiceResult result) =>
                {
                    if (result.intStatus == 1)
                    {
                        List<BaseItem> lstItem = result.Data as List<BaseItem>;
                        var fistItem = lstItem.Where(p => p.Item is TrafficUpdate).FirstOrDefault();
                        if (fistItem != null)
                        {
                            mImgLiveTraffic6.Visibility = ViewStates.Visible;
                            var item = fistItem.Item as TrafficUpdate;
                            mImgLiveTraffic6.Click += (object sender, EventArgs e) =>
                            {
                                Intent intent = new Intent(this, typeof(LiveTrafficDetailActivity));
                                intent.PutExtra(LiveTrafficDetailActivity.DATA, JsonConvert.SerializeObject(item));
                                StartActivity(intent);
                            };

                        }
                    }
                };
                thread.loadLiveTraffic(new List<int>() { 6 }, null, mUpdate.idTrafficUpdate);


            }
            else
            {


                LiveTrafficThread thread = new LiveTrafficThread();
                thread.OnLoadLiveTrafficDetail += (ServiceResult result) =>
                {
                    if (result.intStatus == 1)
                    {
                        TrafficUpdate item = result.Data as TrafficUpdate;
                        List<LatLng> points = PolyUtils.DecodePolylinePoints(item.overview_polyline);
                        //map.draw
                        var po = new PolylineOptions();
                        po.Add(points.ToArray());
                        po.InvokeColor(Color.Red);
                        po.InvokeWidth(3);
                    
                        Polyline line = map.AddPolyline(po);
                    }
                };
                thread.loadLiveTrafficDetail(mUpdate.idTrafficUpdate);
                //PolyUtil
                //thread.load
            }

            mImgDirection.Click += MImgDirection_Click;
            // Create your application here
        }

        private void MImgDirection_Click(object sender, EventArgs e)
        {
            string uri = "";
            uri = String.Format("http://maps.google.com/maps?daddr={0},{1}", mUpdate.decLat, mUpdate.decLng);
            Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri));
            StartActivity(intent);
        }

        protected override void OnResume()
        {
            base.OnResume();
            _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
        }

        public void UpdateItemLocation(LatLng location, string strTitle)
        {
            MarkerOptions option = new MarkerOptions();
            option.SetPosition(new LatLng(location.Latitude, location.Longitude));
            option.SetTitle(strTitle);
            map.AddMarker(option);

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
            Android.Gms.Maps.Model.LatLng myTarget = new Android.Gms.Maps.Model.LatLng(mUpdate.decLat, mUpdate.decLng);
            if (location != null)
            {
                Android.Gms.Maps.Model.LatLng myLoc = new Android.Gms.Maps.Model.LatLng(location.Latitude, location.Longitude);
                double distince = GPSUtils.Distance(myLoc, myTarget, GPSUtils.DistanceUnit.Kilometers);
                mTxtAway.Text = string.Format(GetString(Resource.String.text_away), distince);
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

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

      

        private void addMap(TrafficUpdate mUpdate)
        {
            Fragment_Maps f = Fragment_Maps.NewInstance(new LatLng(mUpdate.decLat, mUpdate.decLng), mUpdate.strTitle);
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
            map.UiSettings.MapToolbarEnabled = false;
            Android.Gms.Maps.Model.LatLng myTarget = new Android.Gms.Maps.Model.LatLng(mUpdate.decLat, mUpdate.decLng);
            if (myTarget != null)
            {
                UpdateItemLocation(myTarget, mUpdate.strTitle);
            }
        }
    }
}