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
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.Content.Res;
using Android.Net;

namespace Dex.Com.Expresso.Utils
{
    public static class GPSUtils
    {
        public enum DistanceUnit
        {
            Miles,
            Kilometers
        };

        public static bool IsEnable(Context ctx)
        {
            LocationManager conMgr = (LocationManager)ctx.GetSystemService(Context.LocationService);
            if (!conMgr.IsProviderEnabled(LocationManager.GpsProvider))
                return false;
            else
                return true;
        }
       

        public static double Distance(LatLng coord1, LatLng coord2, DistanceUnit unit)
        {
            double R = (unit == DistanceUnit.Miles) ? 3960 : 6371;
            var lat = (coord2.Latitude - coord1.Latitude).ToRadian();
            var lng = (coord2.Longitude - coord1.Longitude).ToRadian();

            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                     Math.Cos(coord1.Latitude.ToRadian()) * Math.Cos(coord2.Latitude.ToRadian()) *
                     Math.Sin(lng / 2) * Math.Sin(lng / 2);

            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));

            return R * h2;
        }
        public static double Distance(Location coord1, Location coord2, DistanceUnit unit)
        {
            double R = (unit == DistanceUnit.Miles) ? 3960 : 6371;
            var lat = (coord2.Latitude - coord1.Latitude).ToRadian();
            var lng = (coord2.Longitude - coord1.Longitude).ToRadian();

            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                     Math.Cos(coord1.Latitude.ToRadian()) * Math.Cos(coord2.Latitude.ToRadian()) *
                     Math.Sin(lng / 2) * Math.Sin(lng / 2);

            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));

            return R * h2;
        }
        public static double ToRadian(this double value)
        {
            return (Math.PI / 180) * value;
        }

        public static Location getLastBestLocation(Context cotext)
        {
            LocationManager locationManager = (LocationManager)cotext.GetSystemService(Context.LocationService);
            Location locationGPS = locationManager.GetLastKnownLocation(LocationManager.GpsProvider);
            Location locationNet = locationManager.GetLastKnownLocation(LocationManager.NetworkProvider);

            long GPSLocationTime = 0;
            if (null != locationGPS) { GPSLocationTime = locationGPS.Time; }

            long NetLocationTime = 0;

            if (null != locationNet)
            {
                NetLocationTime = locationNet.Time;
            }

            if (0 < GPSLocationTime - NetLocationTime)
            {
                return locationGPS;
            }
            else
            {
                return locationNet;
            }
        }
    }
}