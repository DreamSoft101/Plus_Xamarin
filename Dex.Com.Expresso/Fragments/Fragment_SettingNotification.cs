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

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_SettingNotification : BaseFragment
    {
        private ViewPager mVppTab;
        private TabLayout mTabLayout;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public static Fragment_SettingNotification NewInstance()
        {
            var frag1 = new Fragment_SettingNotification { Arguments = new Bundle() };
            return frag1;
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_favorites, null);
            mVppTab = view.FindViewById<ViewPager>(Resource.Id.pager);


            SettingNotificationAdapter adapter = new SettingNotificationAdapter(getActivity(), getActivity().SupportFragmentManager);
            mVppTab.Adapter = adapter;
            mTabLayout = view.FindViewById<TabLayout>(Resource.Id.tabLayout);
            mTabLayout.SetupWithViewPager(mVppTab);
            mTabLayout.TabMode = TabLayout.ModeScrollable;

            return view;
        }
    }
}