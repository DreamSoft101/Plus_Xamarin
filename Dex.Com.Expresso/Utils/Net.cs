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
using Android.Net;

namespace Dex.Com.Expresso.Utils
{
    public static class Net
    {
        public static bool IsEnable(Context ctx)
        {
            ConnectivityManager conMgr = (ConnectivityManager)ctx.GetSystemService(Context.ConnectivityService);
            bool ret = true;
            if (conMgr != null)
            {
                NetworkInfo i = conMgr.ActiveNetworkInfo;
                if (i != null)
                {
                    if (!i.IsConnected)
                        ret = false;
                    if (!i.IsAvailable)
                        ret = false;
                }

                if (i == null)
                    ret = false;

            }
            else
                ret = false;
            return ret;
        }

    }
}