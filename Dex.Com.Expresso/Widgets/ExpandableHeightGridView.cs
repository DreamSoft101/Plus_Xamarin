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

namespace Dex.Com.Expresso.Widgets
{
    public class ExpandableHeightGridView : GridView
    {

        private bool expanded = false;

        public ExpandableHeightGridView(Context context) : base(context)
        {
        }

        public ExpandableHeightGridView(Context context, IAttributeSet attrs) : base(context, attrs)
        {

        }

        public ExpandableHeightGridView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {

        }

        public bool IsExpanded
        {
            get { return expanded; }
            set { this.expanded = value; }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            if (IsExpanded)
            {
                int expandSpec = MeasureSpec.MakeMeasureSpec(int.MaxValue >> 2, MeasureSpecMode.AtMost);
                base.OnMeasure(widthMeasureSpec, expandSpec);
                ViewGroup.LayoutParams lparam = LayoutParameters;
                lparam.Height = MeasuredHeight;

            }
            else
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            }
        }
    }
}