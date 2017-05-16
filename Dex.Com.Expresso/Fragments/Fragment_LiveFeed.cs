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
using Android.Support.V7.Widget;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_LiveFeed : BaseFragment
    {
        private LiveFeedAdapter adapter;
        private ViewPager mVppTab;
        private TabLayout mTabLayout;
        public delegate void onInitSearch(Filter filter);
        public onInitSearch OnInitSearch;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public static Fragment_LiveFeed NewInstance()
        {
            var frag1 = new Fragment_LiveFeed { Arguments = new Bundle() };
            return frag1;
        }

        

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_favorites, null);
            mVppTab = view.FindViewById<ViewPager>(Resource.Id.pager);


            adapter = new LiveFeedAdapter(getActivity(), getActivity().SupportFragmentManager);
            mVppTab.Adapter = adapter;
            mTabLayout = view.FindViewById<TabLayout>(Resource.Id.tabLayout);
            mTabLayout.SetupWithViewPager(mVppTab);
            mTabLayout.TabMode = TabLayout.ModeScrollable;
            mVppTab.PageSelected += MVppTab_PageSelected;
            adapter.OnInitData += (Dex.Com.Expresso.Adapters.RecyclerViews.LiveFeedAdapter a) =>
            {
                if (OnInitSearch != null)
                {
                    OnInitSearch(a.Filter);
                }
            };
            return view;
        }

        private void MVppTab_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                var fragment = adapter.GetItem(e.Position);
                if (fragment is Fragment_LiveFeed_Highway)
                {
                    Fragment_LiveFeed_Highway fH = fragment as Fragment_LiveFeed_Highway;
                    if (OnInitSearch != null)
                    {
                        OnInitSearch(fH.getAdapter().Filter);
                    }
                    
                }
                else if (fragment is Fragment_LiveFeed_TollPlaza)
                {
                    Fragment_LiveFeed_TollPlaza fH = fragment as Fragment_LiveFeed_TollPlaza;
                    if (OnInitSearch != null)
                    {
                        OnInitSearch(fH.getAdapter().Filter);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}