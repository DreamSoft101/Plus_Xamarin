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
using Android.Locations;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Dex.Com.Expresso.Utils;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Maps : BaseFragment
    {
        private Location mCurrentLocation;
        private SupportMapFragment mapFrag;
        private GoogleMap map;
        private Marker mMarker;
        private Marker mMarkerItem;
        private LatLng locationItem;
        private string mStrTitle;

        public bool isRunAuto = true;
        public delegate void OnGetMap(GoogleMap map, SupportMapFragment mapFrag);
        public OnGetMap onGetMap;

        public void UpdateMyLocation(Location currentLocation)
        {
            if (map == null)
            {
                return;
            }
            this.mCurrentLocation = currentLocation;
            if (mMarker == null)
            {
                MarkerOptions option = new MarkerOptions();
                option.SetPosition(new LatLng(currentLocation.Latitude, currentLocation.Longitude));
                option.SetTitle("My Location");
                //option.ti
                mMarker = map.AddMarker(option);
            }
            else
            {
                mMarker.Position = new LatLng(currentLocation.Latitude, currentLocation.Longitude);
            }
            
        }

        public void UpdateItemLocation(LatLng location,string strTitle)
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
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            
            // Create your fragment here
        }

        public static Fragment_Maps NewInstance(LatLng latlng, string strTitle)
        {
            var frag1 = new Fragment_Maps { locationItem = latlng, mStrTitle = strTitle };
            return frag1;
        }
       
        public GoogleMap getMap()
        {
            return map;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_maps, null);
        
            mapFrag = (SupportMapFragment)ChildFragmentManager.FindFragmentById(Resource.Id.map);
            if (mapFrag != null)
            {
                map = mapFrag.Map;
                if(onGetMap != null)
                {
                    onGetMap(map, mapFrag);
                }
                if (isRunAuto)
                {
                    Location location = GPSUtils.getLastBestLocation(getActivity());
                    if (location != null)
                    {
                        //UpdateMyLocation(location);
                    }
                    if (locationItem != null)
                    {
                        UpdateItemLocation(locationItem, mStrTitle);
                    }
                }
                
            }


            return view;
        }
    }
}