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
using Android.Support.V7.Widget;

namespace Dex.Com.Expresso.Adapters.Viewpager
{
    public class LiveFeedAdapter : FragmentStatePagerAdapter
    {
        private List<string> titles;
        private Context mContext;
        public delegate void onInitData(Dex.Com.Expresso.Adapters.RecyclerViews.LiveFeedAdapter adapter);
        public onInitData OnInitData;
        public LiveFeedAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }


        public LiveFeedAdapter(Context context, Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
            mContext = context;
            titles = new List<string>();
            titles.Add(mContext.GetString(Resource.String.txt_highway));
            titles.Add(mContext.GetString(Resource.String.facilities_type_toll_plaza));
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

        private Fragment_LiveFeed_Highway f1;
        private Fragment_LiveFeed_TollPlaza f2;
        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            if (position == 0)
            {
                if (f1 == null)
                {
                    f1 = Fragment_LiveFeed_Highway.NewInstance();
                    f1.OnInitData += () =>
                    {
                        if (OnInitData != null)
                        {
                            OnInitData(f1.getAdapter());
                        }
                    };
                }
                return f1;
            }
            else if (position == 1)
            {
                if (f2 == null)
                {
                    f2 = new Fragment_LiveFeed_TollPlaza();
                }
                return f2;
            }
            return null;
        }
    }
}