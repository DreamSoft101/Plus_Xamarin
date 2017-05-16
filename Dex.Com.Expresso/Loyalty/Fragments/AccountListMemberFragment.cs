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
using Dex.Com.Expresso.Loyalty.Droid.Adapters.RecyclerViews;
using Loyalty.Utils;
using Dex.Com.Expresso.Loyalty.Droid.Activities;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Fragments
{
    public class AccountListMemberFragment : BaseFragment
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

        public static AccountListMemberFragment NewInstance()
        {
            var frag1 = new AccountListMemberFragment { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_listmember, null);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);
            mPrbLoading = view.FindViewById<ProgressBar>(Resource.Id.prbLoading);
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
                    var data = result.Data as MGetMemberProfile;
                    mPrbLoading.Visibility = ViewStates.Gone;
                    mLstItems.Visibility = ViewStates.Visible;
                    var lstData = data.AccountDetails;
                    MemberDetailAdapters adapter = new MemberDetailAdapters(this.Activity, lstData.ToList());
                    adapter.ItemClick += Adapter_ItemClick;
                    adapter.OnClickEStatement += OnClickEStatement;
                    mLstItems.SetAdapter(adapter);
                }
                else
                {
                    Toast.MakeText(this.Activity, result.Mess, ToastLength.Short).Show();
                }
            };
            thread.GetMemberProfile(Cons.mMemberCredentials.LoginParams.authSessionId, Cons.mMemberCredentials.LoginParams.strLoginId,true);
        }

        public void OnClickEStatement(MIMX_AccountDetails item)
        {
            Intent intent = new Intent(this.Activity, typeof(AccountEStatementFileList));
            intent.PutExtra(AccountEStatementFileList.MemberID, item.idMember.ToString());
            intent.PutExtra(AccountEStatementFileList.MemberName, item.strMaskedCardNumber);
            StartActivity(intent);
        }

        private void Adapter_ItemClick(object sender, MIMX_AccountDetails e)
        {
            //Intent intent = new Intent(this.Activity, typeof(AccountEStatementFileList));
            //intent.PutExtra(AccountEStatementFileList.MemberID, e.idMember.ToString());
            //intent.PutExtra(AccountEStatementFileList.MemberName, e.strMaskedCardNumber);
            //StartActivity(intent);
        }

        public override void OnResume()
        {
            base.OnResume();
        }
    }
}