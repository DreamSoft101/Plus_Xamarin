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

namespace Dex.Com.Expresso.Widgets
{
    public class ImageViewExt : ImageView
    {
        private static int INVALID_POINTER_ID = -1;
        private float mPosX;
        private float mPosY;

        private float mLastTouchX;
        private float mLastTouchY;

        private float mLastGestureX;
        private float mLastGestureY;
        private int mActivePointerId = INVALID_POINTER_ID;

        private ScaleGestureDetector mScaleDetector;
        private static float mScaleFactor = 1.0f;

        public ImageViewExt(Context context) : base(context)
        {
            mScaleDetector = new ScaleGestureDetector(Context, new ScaleListener());
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            mScaleDetector.OnTouchEvent(e);

            //int action = e.Action;
            switch (e.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Down:
                    if (!mScaleDetector.IsInProgress)
                    {
                        float x = e.GetX();
                        float y = e.GetY();

                        mLastTouchX = x;
                        mLastTouchY = y;
                        mActivePointerId = e.GetPointerId(0);
                    }
                    break;
                case MotionEventActions.Pointer1Down:
                    if (mScaleDetector.IsInProgress)
                    {
                        float gx = mScaleDetector.FocusX;
                        float gy = mScaleDetector.FocusY;

                        mLastGestureX = gx;
                        mLastGestureY = gy;
                    }
                    break;
                case MotionEventActions.Move:
                    if (!mScaleDetector.IsInProgress)
                    {
                        int pointerIdx = e.FindPointerIndex(mActivePointerId);
                        float x = e.GetX(pointerIdx);
                        float y = e.GetY(pointerIdx);

                        float dx = x - mLastTouchX;
                        float dy = y - mLastTouchY;

                        mPosX += dx;
                        mPosY += dy;

                        Invalidate();

                        mLastTouchX = x;
                        mLastTouchY = y;
                    }
                    else
                    {
                        float gx = mScaleDetector.FocusX;
                        float gy = mScaleDetector.FocusY;

                        float gdx = gx - mLastGestureX;
                        float gdy = gy - mLastGestureY;

                        mPosX += gdx;
                        mPosY += gdy;

                        Invalidate();

                        mLastGestureX = gx;
                        mLastGestureY = gy;
                    }
                    break;
                case MotionEventActions.Up:
                    mActivePointerId = INVALID_POINTER_ID;
                    break;
                case MotionEventActions.Cancel:
                    mActivePointerId = INVALID_POINTER_ID;
                    break;
                case MotionEventActions.PointerUp:

                    int pointerIdx2 = (int)(e.Action & MotionEventActions.PointerIndexMask) >> (int)MotionEventActions.PointerIndexShift;
                    int pointerId = e.GetPointerId(pointerIdx2);

                    if (pointerId == mActivePointerId)
                    {
                        int NewPointerIndex = pointerIdx2 == 0 ? 1 : 0;
                        mLastTouchX = e.GetX(NewPointerIndex);
                        mLastTouchY = e.GetY(NewPointerIndex);
                        mActivePointerId = e.GetPointerId(NewPointerIndex);
                    }
                    else
                    {
                        int TempPointerIdx = e.FindPointerIndex(mActivePointerId);
                        mLastTouchX = e.GetX(TempPointerIdx);
                        mLastTouchY = e.GetY(TempPointerIdx);
                    }
                    break;
            }

            return true;
        }

        protected override void OnDraw(Canvas canvas)
        {
            canvas.Save();

            canvas.Translate(mPosX, mPosY);
            if (mScaleDetector.IsInProgress)
            {
                canvas.Scale(mScaleFactor, mScaleFactor, mScaleDetector.FocusX, mScaleDetector.FocusY);
            }
            else
            {
                canvas.Scale(mScaleFactor, mScaleFactor, mLastGestureX, mLastGestureY);
            }
            base.OnDraw(canvas);
            canvas.Restore();
        }


        private class ScaleListener : ScaleGestureDetector.SimpleOnScaleGestureListener
        {
            public override bool OnScale(ScaleGestureDetector detector)
            {
                mScaleFactor *= detector.ScaleFactor;

                mScaleFactor = Math.Max(0.1f, Math.Min(mScaleFactor, 10.0f));

                return true;
            }

        }
    }
}