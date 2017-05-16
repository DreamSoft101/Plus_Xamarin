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
using Dex.Com.Expresso.Loyalty.Droid.Fragments;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.Viewpager
{
    public class AccountInformationAdapters : FragmentStatePagerAdapter
    {
        private List<string> titles;
        private Context mContext;
        public AccountInformationAdapters(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public override int GetItemPosition(Java.Lang.Object objectValue)
        {
            return PositionNone;
        }


        public AccountInformationAdapters(Context context, Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
            mContext = context;
            titles = new List<string>();
            titles.Add(mContext.GetString(Resource.String.loy_title_listmember));
            titles.Add(mContext.GetString(Resource.String.loy_title_redemptionhistory));
            //titles.Add(mContext.GetString(Resource.String.loy_title_accountsummary));
            //title_accountsummary
        }

        public override int Count
        {
            get
            {
                return titles.Count;
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
                return AccountListMemberFragment.NewInstance();
            }
            else if (position == 1)
            {
                return AccountRedemptionHistoryFragment.NewInstance();
            }
            else if (position == 2)
            {
                return AccountSummaryFragment.NewInstance();
            }
            else
            {
                return null;
            }
        }
    }
}