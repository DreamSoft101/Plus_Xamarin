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
using Dex.Com.Expresso.Adapters.Viewpager;
using Android.Support.Design.Widget;
using Android.Support.V4.View;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_RSA_Main : BaseFragment
    {
        private ViewPager mVppTab;
        private TabLayout mTabLayout;
        public delegate void onInitSearch(Filter filter);
        public onInitSearch OnInitSearch;
        private RSAAdapter adapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public static Fragment_RSA_Main NewInstance()
        {
            var frag1 = new Fragment_RSA_Main { Arguments = new Bundle() };
            return frag1;
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_favorites, null);
            mVppTab = view.FindViewById<ViewPager>(Resource.Id.pager);


            adapter = new RSAAdapter(getActivity(), getActivity().SupportFragmentManager);
            mVppTab.Adapter = adapter;
            mTabLayout = view.FindViewById<TabLayout>(Resource.Id.tabLayout);
            mTabLayout.SetupWithViewPager(mVppTab);
            mTabLayout.TabMode = TabLayout.ModeScrollable;
            adapter.OnInitData += (Filter filter) =>
            {
                if (OnInitSearch != null)
                {
                    OnInitSearch(filter);
                }
            };

            mVppTab.PageSelected += MVppTab_PageSelected;
            return view;
        }

        private void MVppTab_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                var fragment = adapter.GetItem(e.Position);
                if (fragment is Fragment_RSA)
                {
                    Fragment_RSA fH = fragment as Fragment_RSA;
                    if (OnInitSearch != null)
                    {
                        OnInitSearch(fH.getAdapter().Filter);
                    }

                }
                else if (fragment is Fragment_Lay_By)
                {
                    Fragment_Lay_By fH = fragment as Fragment_Lay_By;
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