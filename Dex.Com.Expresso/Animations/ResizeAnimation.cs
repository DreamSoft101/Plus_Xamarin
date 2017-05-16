using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using static Android.Resource;

namespace Dex.Com.Expresso.Animations
{
    public class ResizeAnimation : Android.Views.Animations.Animation
    {
        private View mView;
        private float mToHeight;
        private float mFromHeight;

        private float mToWidth;
        private float mFromWidth;

        public ResizeAnimation(View v, float fromWidth, float fromHeight, float toWidth, float toHeight)
        {

            mToHeight = toHeight;
            mToWidth = toWidth;
            mFromHeight = fromHeight;
            mFromWidth = fromWidth;
            mView = v;
            this.Duration = 300;
        }

        public ResizeAnimation(View v, float fromWidth, float fromHeight, float toWidth, float toHeight, int Duration)
        {

            mToHeight = toHeight;
            mToWidth = toWidth;
            mFromHeight = fromHeight;
            mFromWidth = fromWidth;
            mView = v;
            this.Duration = 300;
        }


        protected override void ApplyTransformation(float interpolatedTime, Transformation t)
        {
            float height = (mToHeight - mFromHeight) * interpolatedTime + mFromHeight;
            float width = (mToWidth - mFromWidth) * interpolatedTime + mFromWidth;
            ViewGroup.LayoutParams p = mView.LayoutParameters;
            p.Height = (int)height;
            p.Width = (int)width;
            mView.RequestLayout();
            base.ApplyTransformation(interpolatedTime, t);
        }


    }
}