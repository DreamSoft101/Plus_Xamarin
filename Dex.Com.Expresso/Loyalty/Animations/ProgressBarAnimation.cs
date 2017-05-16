using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Views.Animations;
using Android.Widget;

namespace Dex.Com.Expresso.Loyalty.Droid.Animations
{
    public class ProgressBarAnimation : Animation
    {
        private ProgressBar mProgressBar;
        private int mTo;
        private int mFrom;
        private long mStepDuration;

        /**
         * @param fullDuration - time required to fill progress from 0% to 100%
         */
        public ProgressBarAnimation(ProgressBar progressBar, long fullDuration) : base()
        {
            mProgressBar = progressBar;
            mStepDuration = fullDuration / progressBar.Max;
        }

        public int Progress
        {
            get { return mProgressBar.Progress; }
            set
            {
                setProgress(value);
            }
        }

        public void setProgress(int progress)
        {
            if (progress < 0)
            {
                progress = 0;
            }

            if (progress > mProgressBar.Max)
            {
                progress = mProgressBar.Max;
            }

            mTo = progress;

            mFrom = mProgressBar.Progress;
            Duration = (Math.Abs(mTo - mFrom) * mStepDuration);
            mProgressBar.StartAnimation(this);
        }

        protected override void ApplyTransformation(float interpolatedTime, Transformation t)
        {
            float value = mFrom + (mTo - mFrom) * interpolatedTime;
            mProgressBar.Progress = ((int)value);
        }
    }
}