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
using Android.Support.V7.Widget.Helper;
using Android.Support.V7.Widget;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Loyalty.Utils;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.RecyclerViews;
using Dex.Com.Expresso.Loyalty.Droid.Activities;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Fragments
{
    public class AccountRedemptionHistoryFragment : BaseFragment
    {
        private ItemTouchHelper mItemTouchHelper;
        private RecyclerView mLstItems;
        private RecyclerView.LayoutManager mLayoutManager;
        private ProgressBar mPrbLoading;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static AccountRedemptionHistoryFragment NewInstance()
        {
            var frag1 = new AccountRedemptionHistoryFragment { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_account_redemptionhistory, null);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mPrbLoading = view.FindViewById<ProgressBar>(Resource.Id.prbLoading);
            mLstItems.SetLayoutManager(mLayoutManager);
            LoadData();
            return view;
        }

        private void LoadData()
        {
            
            mLstItems.Visibility = ViewStates.Gone;
            mPrbLoading.Visibility = ViewStates.Visible;
            UserThreads thread = new UserThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                if (result.StatusCode == 1)
                {
                    mPrbLoading.Visibility = ViewStates.Gone;
                    mLstItems.Visibility = ViewStates.Visible;
                    MGetRedemptionHistory data = result.Data as MGetRedemptionHistory;

                    //var data = result.Data as IMX

                    RedemptionHistoryAdapters adapter = new RedemptionHistoryAdapters(this.Activity, data.histories);
                    adapter.ItemClick += Adapter_ItemClick;
                    mLstItems.SetAdapter(adapter);
                }
                else
                {
                    Toast.MakeText(this.Activity, result.Mess, ToastLength.Short).Show();
                }
            };
            thread.GetMemberRdemptionHistory();
        }

        private void Adapter_ItemClick(object sender, RedemptionHistory e)
        {
            Intent intent = new Intent(this.Activity, typeof(RedemptionDetailsActivity));
            intent.PutExtra(RedemptionDetailsActivity.RedemptionID, e.MemberRedemptionID.ToString());
            StartActivity(intent);
        }

        public override void OnResume()
        {
            base.OnResume();
        }
    }
}