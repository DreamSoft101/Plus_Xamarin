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
using Dex.Com.Expresso.Fragments;
using Android.Webkit;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Dex.Com.Expresso.Loyalty.Adapters.RecyclerViews;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;

namespace Dex.Com.Expresso.Loyalty.Fragments
{
    public class Fragment_AboutProgram_Fag : BaseFragment
    {
        private int RESULT_LOGIN = 99;
        private LinearLayout mLnlData, mLnlLoading;
        private WebView mWebView;
        private RecyclerView mLstItems;
        private SwipeRefreshLayout swipeRefreshLayout;
        private RecyclerView.LayoutManager mLayoutManager;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }
        public static Fragment_AboutProgram_Fag NewInstance()
        {
            var frag1 = new Fragment_AboutProgram_Fag { Arguments = new Bundle() };
            return frag1;
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_faq, null);
            mLnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            mLnlData = view.FindViewById<LinearLayout>(Resource.Id.lnlData);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            // mWebView = view.FindViewById<WebView>(Resource.Id.wvData);
            mLstItems.SetLayoutManager(mLayoutManager);
            swipeRefreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);

            swipeRefreshLayout.Refresh += SwipeRefreshLayout_Refresh;

            LoadData();
            return view;
        }

        private void SwipeRefreshLayout_Refresh(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            swipeRefreshLayout.Refreshing = true;
            LoadData();
        }


        private void LoadData()
        {
            mLnlData.Visibility = ViewStates.Gone;
            mLnlLoading.Visibility = ViewStates.Visible;
            swipeRefreshLayout.Refreshing = true;
            ContentThreads thread = new ContentThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                swipeRefreshLayout.Refreshing = false;
                if (result.StatusCode == 1)
                {
                    mLnlLoading.Visibility = ViewStates.Gone;
                    mLnlData.Visibility = ViewStates.Visible;
                    MGetFAQ data = result.Data as MGetFAQ;
                    if (data.FAQ.Length > 1)
                    {
                        FaqAdapter adapter = new FaqAdapter(this.getActivity(), data.FAQ.ToList());
                        //mls
                        mLstItems.SetAdapter(adapter);
                    }
                }
                else
                {
                    Toast.MakeText(getActivity(), result.Mess, ToastLength.Short).Show();
                }
            };
            thread.GetFaq(null, 1, "en", 1);
        }
        


        public override void OnResume()
        {
            base.OnResume();
        }
    }
}