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
    public class LiveTrafficAdapter : FragmentStatePagerAdapter
    {
        private List<string> titles;
        private Context mContext;
        public LiveTrafficAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }


        public LiveTrafficAdapter(Context context, Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
            mContext = context;
            titles = new List<string>();
            titles.Add(mContext.GetString(Resource.String.facilities_type_rsa));
            titles.Add(mContext.GetString(Resource.String.facilities_type_lay_by));
            titles.Add(mContext.GetString(Resource.String.facilities_type_toll_plaza));
            titles.Add(mContext.GetString(Resource.String.facilities_type_petrol_station));
            titles.Add(mContext.GetString(Resource.String.facilities_type_csc));
            titles.Add(mContext.GetString(Resource.String.facilities_type_plussmie));
            titles.Add(mContext.GetString(Resource.String.facilities_type_ssk));
            titles.Add(mContext.GetString(Resource.String.facilities_type_interchange));
            titles.Add(mContext.GetString(Resource.String.facilities_type_vistapoint));
            titles.Add(mContext.GetString(Resource.String.facilities_type_tunnel));
        }

        public override int Count
        {
            get
            {
                return 1;
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
                return Fragment_LiveTraffic_All.NewInstance();
            }
            else if (position == 1)
            {
                return Fragment_Lay_By.NewInstance();
            }
            return null;
        }
    }
}