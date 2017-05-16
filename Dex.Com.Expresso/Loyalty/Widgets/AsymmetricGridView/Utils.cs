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
using Android.Util;

namespace Dex.Com.Expresso.Loyalty.Droid.Widgets.AsymmetricGridView
{
    public static class Utils
    {
        public static int dpToPx(Context context, float dp)
        {
            float scale = context.Resources.DisplayMetrics.Density;
            return (int)((dp * scale) + 0.5f);
        }

        public static int getScreenWidth(Context context)
        {
            if (context == null)
            {
                return 0;
            }
            return getDisplayMetrics(context).WidthPixels;
        }

        public static DisplayMetrics getDisplayMetrics(Context context)
        {
            IWindowManager windowManager = (IWindowManager)context.GetSystemService(Context.WindowService);
            DisplayMetrics metrics = new DisplayMetrics();
            windowManager.DefaultDisplay.GetMetrics(metrics);
            return metrics;
        }
    }
}