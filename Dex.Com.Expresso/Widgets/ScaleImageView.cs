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
using Java.Lang;

namespace Dex.Com.Expresso.Widgets
{

    public interface ImageChangeListener
    {
        // a callback for when a change has been made to this imageView
        void changed(bool isEmpty);
    }

    /*
    public class ScaleImageViewGestureDetector : GestureDetector.SimpleOnGestureListener
    {
        private readonly ScaleImageView m_ScaleImageView;
        public ScaleImageViewGestureDetector(ScaleImageView imageView)
        {
            m_ScaleImageView = imageView;
        }

        public override bool OnDown(MotionEvent e)
        {
            return true;
        }

        public override bool OnDoubleTap(MotionEvent e)
        {
            m_ScaleImageView.MaxZoomTo((int)e.GetX(), (int)e.GetY());
            m_ScaleImageView.Cutting();
            return true;
        }
    }*/

    public class ScaleImageView : ImageView
    {
        private ImageChangeListener imageChangeListener;
        private bool scaleToWidth = false;
        public ScaleImageView(Context context) : base(context)
        {
            init();
        }

        public ScaleImageView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            init();
        }

        public ScaleImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            init();
        }

        private void init()
        {
            this.SetScaleType(ScaleType.CenterInside);
        }

        public ImageChangeListener getImageChangeListener()
        {
            return imageChangeListener;
        }

        public void setImageChangeListener(ImageChangeListener imageChangeListener)
        {
            this.imageChangeListener = imageChangeListener;
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            //base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            var widthMode = MeasureSpec.GetMode(widthMeasureSpec);
            var heightMode = MeasureSpec.GetMode(heightMeasureSpec);
            var width = MeasureSpec.GetSize(widthMeasureSpec);
            var height = MeasureSpec.GetSize(heightMeasureSpec);

            if (widthMode == MeasureSpecMode.Exactly || widthMode == MeasureSpecMode.AtMost)
            {
                scaleToWidth = true;
            }
            else if (heightMode == MeasureSpecMode.Exactly || heightMode == MeasureSpecMode.AtMost)
            {
                scaleToWidth = false;
            }
            else
            {
                scaleToWidth = true;
            }
            //throw new IllegalStateException("width or height needs to be set to match_parent or a specific dimension");

            if (this.Drawable == null || this.Drawable.IntrinsicWidth == 0)
            {
                // nothing to measure
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
                return;
            }
            else
            {
                if (scaleToWidth)
                {
                    int iw = this.Drawable.IntrinsicWidth;
                    int ih = this.Drawable.IntrinsicHeight;
                    int heightC = width * ih / iw;
                    if (height > 0)
                        if (heightC > height)
                        {
                            // dont let hegiht be greater then set max
                            heightC = height;
                            width = heightC * iw / ih;
                        }

                    this.SetScaleType(ScaleType.CenterCrop);
                    SetMeasuredDimension(width, heightC);
                }
                else
                {
                    // need to scale to height instead
                    int marg = 0;
                    if (this.Parent != null)
                    {
                        if (this.Parent.Parent != null)
                        {
                            marg += ((View)this.Parent.Parent).PaddingTop;
                            marg += ((View)this.Parent.Parent).PaddingBottom;
                        }
                    }

                    int iw = this.Drawable.IntrinsicWidth;
                    int ih = this.Drawable.IntrinsicHeight;

                    width = height * iw / ih;
                    height -= marg;
                    SetMeasuredDimension(width, height);
                }
            }
        }

    }
}