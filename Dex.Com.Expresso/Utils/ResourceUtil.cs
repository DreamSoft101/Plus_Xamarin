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
using Android.Support.V4.Content;
using EXPRESSO.Models;

namespace Dex.Com.Expresso.Utils
{
    public static class ResourceUtil
    {
        public static Android.Graphics.Color getColor(this Context context, int resource)
        {
            return new Android.Graphics.Color(ContextCompat.GetColor(context, resource));
        }


        public static int GetResourceID(this Context context, string name)
        {
            int resID = context.Resources.GetIdentifier(name, "drawable", context.PackageName);
            return resID;
        }

    }
}