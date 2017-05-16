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
using Android.Support.V7.Widget.Helper;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Dex.Com.Expresso.Loyalty.Adapters.RecyclerViews;
using Dex.Com.Expresso.Loyalty.Activities;
using Newtonsoft.Json;

namespace Dex.Com.Expresso.Loyalty.Fragments
{
    public class FragmentMyEvoucher : BaseFragment
    {
        private ItemTouchHelper mItemTouchHelper;
        private RecyclerView mLstItems;
        private RecyclerView.LayoutManager mLayoutManager;
        private LinearLayout mLnlData, mLnlLoading;
        private SwipeRefreshLayout mSwipe;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static FragmentMyEvoucher NewInstance()
        {
            var frag1 = new FragmentMyEvoucher { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_evoucher, null);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItem);
            mLstItems.SetLayoutManager(mLayoutManager);
            mLnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            mLnlData = view.FindViewById<LinearLayout>(Resource.Id.lnlData);
            mSwipe = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
            mSwipe.Refresh += MSwipe_Refresh;

            LoadData();
            return view;
        }

        private void MSwipe_Refresh(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            mLnlData.Visibility = ViewStates.Gone;
            mLnlLoading.Visibility = ViewStates.Visible;
            mSwipe.Refreshing = true;

            RedemptionThreads thread = new RedemptionThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                if (result.StatusCode == 1)
                {
                    mLnlData.Visibility = ViewStates.Visible;
                    mLnlLoading.Visibility = ViewStates.Gone;
                    mSwipe.Refreshing = false;
                    var data = (result.Data as MBB_GetListEVoucher);
                    MyEVoucherAdapter adapter = new MyEVoucherAdapter(this.getActivity(), data.ListVoucher);
                    adapter.ItemClick += Adapter_ItemClick;
                    mLstItems.SetAdapter(adapter);
                }
                else
                {
                    Toast.MakeText(this.getActivity(), result.Mess, ToastLength.Short).Show();
                }
            };
            thread.GetListEVoucher(0, 99);
        }

        private void Adapter_ItemClick(object sender, MemberVoucherInfo e)
        {
            //throw new NotImplementedException();
            Intent intent = new Intent(this.getActivity(), typeof(EVoucherDetailActivity_v2));
            intent.PutExtra(EVoucherDetailActivity_v2.JsonData, JsonConvert.SerializeObject(e));
            StartActivity(intent);
        }

        public override void OnResume()
        {
            base.OnResume();
            LoadData();
        }
    }
}