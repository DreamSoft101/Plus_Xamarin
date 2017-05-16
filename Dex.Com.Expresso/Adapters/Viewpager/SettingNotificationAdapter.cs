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
using Dex.Com.Expresso.Fragments;

namespace Dex.Com.Expresso.Adapters.Viewpager
{
    public class SettingNotificationAdapter : FragmentStatePagerAdapter
    {
        private List<string> titles;
        private Context mContext;
        public SettingNotificationAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }


        public SettingNotificationAdapter(Context context, Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
            mContext = context;
            titles = new List<string>();
            titles.Add(mContext.GetString(Resource.String.title_livetraffic));
            titles.Add(mContext.GetString(Resource.String.text_cctv));
            //titles.Add(mContext.GetString(Resource.String.facilities_type_plussmie));
            //titles.Add(mContext.GetString(Resource.String.facilities_type_ssk));

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
                return Fragment_Notification_Setting_LiveTraffic.NewInstance();
            }
            else if (position == 1)
            {
                return Fragment_Notification_Setting_LiveFeed.NewInstance();
            }
            return null;
        }
    }
}