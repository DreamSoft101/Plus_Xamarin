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
using Android.Graphics;
using Android.Util;

namespace Dex.Com.Expresso.Loyalty.Droid.Utils
{
    public static class ImageUtils
    {
        public static Bitmap CreateDrawableFromView(Context context, View view)
        {

            DisplayMetrics displayMetrics = new DisplayMetrics();
            ((Activity)context).WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            view.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            view.Measure(displayMetrics.WidthPixels, displayMetrics.HeightPixels);
            view.Layout(0, 0, displayMetrics.WidthPixels, displayMetrics.HeightPixels);
            view.DrawingCacheEnabled = true;
            view.BuildDrawingCache();
            Bitmap bitmap = Bitmap.CreateBitmap(view.MeasuredWidth, view.MeasuredHeight, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(bitmap);
            view.Draw(canvas);
            return bitmap;
        }
    }
}