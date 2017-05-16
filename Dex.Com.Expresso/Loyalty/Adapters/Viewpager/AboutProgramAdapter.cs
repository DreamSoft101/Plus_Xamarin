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
using Android.Support.V4.App;
using Java.Lang;
using Dex.Com.Expresso.Loyalty.Fragments;
using Dex.Com.Expresso.Loyalty.Droid.Fragments;

namespace Dex.Com.Expresso.Loyalty.Adapters.Viewpager
{
    public class AboutProgramAdapter : FragmentStatePagerAdapter
    {
        private List<string> titles;
        private Context mContext;
        public AboutProgramAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public override int GetItemPosition(Java.Lang.Object objectValue)
        {
            return PositionNone;
        }


        public AboutProgramAdapter(Context context, Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
            mContext = context;
            titles = new List<string>();
            titles.Add(mContext.GetString(Resource.String.text_ourprogram));
            titles.Add(mContext.GetString(Resource.String.text_faq));
            //title_accountsummary
        }

        public override int Count
        {
            get
            {
                return 2;
            }
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(titles[position]);
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            if (position == 0)
            {
                return AboutProgramFragment.NewInstance();
            }
            else if (position == 1)
            {
                return Fragment_AboutProgram_Fag.NewInstance();
            }
            else
            {
                return null;
            }
        }
    }
}