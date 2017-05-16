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
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    [Activity(Label = "AccountEStatementFileList")]
    public class AccountEStatementFileList : BaseActivity
    {
        public static string MemberID = "MemberID";
        public static string MemberName = "Name";
        private ItemTouchHelper mItemTouchHelper;
        private RecyclerView mLstItems;
        private RecyclerView.LayoutManager mLayoutManager;
        private ProgressBar mPrbLoading;
        private Guid mMemberID;
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.loy_activity_account_estatementfilelist;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(this);

            mMemberID = new Guid(this.Intent.GetStringExtra(MemberID));
            this.Title = this.Intent.GetStringExtra(MemberName);

            mLstItems = FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);
            mPrbLoading = FindViewById<ProgressBar>(Resource.Id.prbLoading);

            LoadData();

            // Create your application here
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
                    mLstItems.Visibility = ViewStates.Visible;
                    mPrbLoading.Visibility = ViewStates.Gone;
                    var data = result.Data as MGetStatementFileList;
                    MStatementFileAdapters adapter = new MStatementFileAdapters(this, data.Files);
                    mLstItems.SetAdapter(adapter);
                    adapter.ItemClick += Adapter_ItemClick;
                }
                else
                {
                    Toast.MakeText(this, result.Mess, ToastLength.Short).Show();
                }
            };
            thread.GetStatementFileList(mMemberID);
        }

        private void Adapter_ItemClick(object sender, MStatementFile e)
        {
            string url = Cons.API_PDF_URL + e.ID;
            Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
            StartActivity(intent);
        }
    }
}