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

namespace Dex.Com.Expresso.Loyalty.Droid.Widgets
{
    public class SquareImageView : ImageView
    {
        public SquareImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
         
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            int width = MeasuredWidth;
            SetMeasuredDimension(width, width);
        }
    }
}