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
using Dex.Com.Expresso.Loyalty.Droid.Adapters.Spinners;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Dex.Com.Expresso;
using Loyalty.Utils;
using Newtonsoft.Json;

namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    [Activity(Label = "LinkAccountActivity")]
    public class LinkAccountActivity : BaseActivity
    {
        private Button mBtnCancel, mBtnConfirm;
        private EditText mTxtReferenceNo;
        private Spinner mSpnRefercentType;
        private LinearLayout mLnlLoading;
        private ReferenceTypeAdapter adapter;
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.loy_activity_linkaccount;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = GetString(Resource.String.loy_title_linkaccount);
            mBtnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            mBtnConfirm = FindViewById<Button>(Resource.Id.btnConfirm);
            mTxtReferenceNo = FindViewById<EditText>(Resource.Id.txtReferenceNo);
            mSpnRefercentType = FindViewById<Spinner>(Resource.Id.spnReferenceType);
            mLnlLoading = FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            mLnlLoading.Visibility = ViewStates.Gone;
            adapter = new ReferenceTypeAdapter(this);
            mSpnRefercentType.Adapter = adapter;

            mBtnConfirm.Click += MBtnConfirm_Click;
            mBtnCancel.Click += MBtnCancel_Click;
            // Create your application here
        }

        private void MBtnCancel_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Finish();
        }

        private void MBtnConfirm_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

            string refNo = this.mTxtReferenceNo.Text;
            int refType = adapter.GetBaseItem(mSpnRefercentType.SelectedItemPosition).Type;
            mLnlLoading.Visibility = ViewStates.Visible;
            UserThreads thread = new UserThreads();
            thread.OnResult += (ServiceResult result) =>
            {
              
                if (result.StatusCode == 1)
                {


                    UserThreads threadaccount = new UserThreads();
                    threadaccount.OnResult += (ServiceResult resultmember) =>
                    {
                        if (resultmember.StatusCode == 1)
                        {
                            MGetMemberProfile profile = resultmember.Data as MGetMemberProfile;

                            Cons.mMemberCredentials.MemberProfile = profile;

                            string data = JsonConvert.SerializeObject(Cons.mMemberCredentials);
                            setCacheString(MyAuth, data);
                            Finish();
                        }
                        else
                        {
                            mLnlLoading.Visibility = ViewStates.Gone;
                            Toast.MakeText(this, resultmember.Mess, ToastLength.Short).Show();
                        }
                    };

                    threadaccount.GetMemberProfile(Cons.mMemberCredentials.LoginParams.authSessionId, Cons.mMemberCredentials.LoginParams.strLoginId, false);


                }
                else
                {
                    mLnlLoading.Visibility = ViewStates.Gone;
                    Toast.MakeText(this, result.Mess, ToastLength.Short).Show();
                }
            };
            thread.LinkToMasterAccount(refNo, refType);
        }
    }
}