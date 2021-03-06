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

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Favorites : BaseFragment
    {
        private ViewPager mVppTab;
        private TabLayout mTabLayout;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public static Fragment_Favorites NewInstance()
        {
            var frag1 = new Fragment_Favorites { Arguments = new Bundle() };
            return frag1;
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_favorites, null);
            mVppTab = view.FindViewById<ViewPager>(Resource.Id.pager);

            FavoriteThread thread = new FavoriteThread();
            thread.OnSyncSuccess += () =>
            {
                FavoritesAdapter adapter = new FavoritesAdapter(getActivity(), getActivity().SupportFragmentManager);
                mVppTab.Adapter = adapter;
                mTabLayout = view.FindViewById<TabLayout>(Resource.Id.tabLayout);
                mTabLayout.SetupWithViewPager(mVppTab);
                mTabLayout.TabMode = TabLayout.ModeScrollable;
            };
            thread.AutoSyncFavorite();
            
            /*
            for (int i=0; i < adapter.Count; i ++)
            {
                TabLayout.Tab tab = mTabLayout.GetTabAt(i);
                if (tab != null)
                {
                    tab.SetCustomView(Resource.Layout.custom_tab);
                    tab.CustomView.FindViewById<TextView>(Resource.Id.txtName).Text = tab.Text;
                }
            }*/
            return view;
        }
    }
}