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
using Android.Graphics;
using Android.Graphics.Drawables;

namespace Dex.Com.Expresso.Widgets
{
    public class ImageView16x9 : ImageView
    {
        public ImageView16x9(Context context, IAttributeSet attrs) : base(context, attrs)
        {

        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            int width = MeasuredWidth;
            SetMeasuredDimension(width, width * 9 / 16);
        }
    }
}