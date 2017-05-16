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

namespace Dex.Com.Expresso.Utils
{
    public static class PolyUtils
    {
        public static List<LatLng> DecodePolylinePoints(string encodedPoints)
        {
            if (encodedPoints == null || encodedPoints == "") return null;
            List<LatLng> poly = new List<LatLng>();
            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;
            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;
            try
            {
                while (index < polylinechars.Length)
                {
                    // calculate next latitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);
                    if (index >= polylinechars.Length)
                        break;
                    currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                    //calculate next longitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);
                    if (index >= polylinechars.Length && next5bits >= 32)
                        break;
                    currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                    LatLng p = new LatLng(Convert.ToDouble(currentLat) / 100000.0,
                        Convert.ToDouble(currentLng) / 100000.0);
                    poly.Add(p);
                }
            }
            catch (Exception ex)
            {
                //log
            }
            return poly;
        }
    }
}