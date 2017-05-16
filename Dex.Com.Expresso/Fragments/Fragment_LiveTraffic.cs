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
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Dex.Com.Expresso.Adapters.Viewpager;
using EXPRESSO.Threads;
using Dex.Com.Expresso.Adapters.Spinner;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_LiveTraffic : BaseFragment
    {
        private Spinner mSpnHighway;
        private ViewPager mVppTab;
        private TabLayout mTabLayout;
        private HighwayAdapter mHighwayAdapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public static Fragment_LiveTraffic NewInstance()
        {
            var frag1 = new Fragment_LiveTraffic { Arguments = new Bundle() };
            return frag1;
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_livetraffic, null);
            mVppTab = view.FindViewById<ViewPager>(Resource.Id.pager);
            mVppTab.OffscreenPageLimit = 0;
            LiveTrafficAdapter adapter = new LiveTrafficAdapter(getActivity(), getActivity().SupportFragmentManager);
            mVppTab.Adapter = adapter;
            mTabLayout = view.FindViewById<TabLayout>(Resource.Id.tabLayout);
            mTabLayout.SetupWithViewPager(mVppTab);
            mTabLayout.TabMode = TabLayout.ModeScrollable;


            mSpnHighway = view.FindViewById<Spinner>(Resource.Id.spnHighway);

            LiveTrafficThread thread = new LiveTrafficThread();
            thread.OnLoadHighwayResult += (result) =>
            {
                mHighwayAdapter = new HighwayAdapter(getActivity(), result);
                mSpnHighway.Adapter = mHighwayAdapter;
            };
            thread.loadHighway();

            return view;
        }
    }
}