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
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.RecyclerViews;
using Android.Support.V7.Widget.Helper;
using Android.Support.V7.Widget;
using Newtonsoft.Json;
using Dex.Com.Expresso;
using Loyalty.Models.Database;
using Dex.Com.Expresso.Loyalty.Activities;

namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    [Activity(Label = "RedemptionDetails")]
    public class RedemptionDetailsActivity : BaseActivity
    {
        public static string RedemptionID = "RedemptionID";
        private Guid mIdRedemption;
        private TextView mTxtRedemptionDate;
        private TextView mTxtRedemptionNo;
        private TextView mTxtPointsRedeem;
        private TextView mTxtStatus;
        private LinearLayout mLnlData;
        private ProgressBar mPrbLoading;
        private ItemTouchHelper mItemTouchHelper;
        private RecyclerView mLstItems;
        private RecyclerView.LayoutManager mLayoutManager;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.loy_activity_redemptiondetail;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(this);

            this.Title = string.Format(GetString(Resource.String.loy_title_redemptiondetail), "LOADING");
            mIdRedemption = new Guid(this.Intent.GetStringExtra(RedemptionID));
            mTxtRedemptionDate = FindViewById<TextView>(Resource.Id.txtRedemptionDate);
            mTxtRedemptionNo = FindViewById<TextView>(Resource.Id.txtReferenceNo);
            mTxtPointsRedeem = FindViewById<TextView>(Resource.Id.txtTotalPoints);
            mTxtStatus = FindViewById<TextView>(Resource.Id.txtStatus);

            mPrbLoading = FindViewById<ProgressBar>(Resource.Id.prbLoading);
            mLnlData = FindViewById<LinearLayout>(Resource.Id.lnlData);

            mLstItems = FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);

            LoadData();
            
            // Create your application here
        }

        private void LoadData()
        {
            mPrbLoading.Visibility = ViewStates.Visible;
            mLnlData.Visibility = ViewStates.Gone;
            RedemptionThreads thread = new RedemptionThreads();

            MGetRedemptionDetails dataAPI = null;

            thread.OnResult += (ServiceResult result) =>
            {
             
                if (result.StatusCode == 1)
                {
                    if (result.Data is MGetRedemptionDetails)
                    {
                        dataAPI = result.Data as MGetRedemptionDetails;
                        this.Title = string.Format(GetString(Resource.String.loy_title_redemptiondetail), dataAPI.RedeemCode);
                        mTxtRedemptionDate.Text = dataAPI.RedeemDate.ToString(GetString(Resource.String.loy_format_date_time));
                        mTxtRedemptionNo.Text = dataAPI.RedeemCode;
                        mTxtPointsRedeem.Text = string.Format(GetString(Resource.String.loy_format_point), dataAPI.PointsRedeem);
                        switch (dataAPI.RedemptionStatus)
                        {
                            case 0:
                                mTxtStatus.Text = GetString(Resource.String.loy_txt_Pending);
                                break;
                            case 1:
                                mTxtStatus.Text = GetString(Resource.String.loy_text_inprogress);
                                break;
                            case 2:
                                mTxtStatus.Text = GetString(Resource.String.loy_text_cancel);
                                break;
                            case 3:
                                mTxtStatus.Text = GetString(Resource.String.loy_text_closed);
                                break;
                        }
                    }
                    else if (result.Data is List<RedemptionProduct>)
                    {
                        mPrbLoading.Visibility = ViewStates.Gone;
                        mLnlData.Visibility = ViewStates.Visible;

                        var listProduct = result.Data as List<RedemptionProduct>;

                        MRedemptionDetailAdapters adapter = new MRedemptionDetailAdapters(this, dataAPI.RedeemProduct, listProduct);
                        mLstItems.SetAdapter(adapter);
                        adapter.ItemClick += Adapter_ItemClick;
                    }
                }
                else
                {
                    Toast.MakeText(this, result.Mess, ToastLength.Short).Show();
                }
            };
            thread.RedemptionDetail(mIdRedemption);
        }
        private bool IsClick = false;
        private void Adapter_ItemClick(object sender, MRedemptionDetail e)
        {
            if (IsClick)
            {
                return;
            }
            if (e is MRedemptionDetailVoucher)
            {
                var item = e as MRedemptionDetailVoucher;
                RedemptionThreads thread = new RedemptionThreads();
                thread.OnResult += (ServiceResult result) =>
                {
                    IsClick = false;
                    if (result.StatusCode == 1)
                    {
                        var data = (result.Data as MBB_GetListEVoucher);
                        foreach (var itemv in data.ListVoucher)
                        {
                            if (itemv.strVoucherNo == itemv.strVoucherNo)
                            {
                                Intent intent = new Intent(this, typeof(EVoucherDetailActivity_v2));
                                intent.PutExtra(EVoucherDetailActivity_v2.JsonData, JsonConvert.SerializeObject(itemv));
                                StartActivity(intent);
                            }
                        }
                    }
                    else
                    {
                        Toast.MakeText(this, result.Mess, ToastLength.Short).Show();
                    }
                };
                IsClick = true;
                thread.GetListEVoucher(0, 99);
            }
            else if (e is MRedemptionDetailPhysic)
            {
                IsClick = false;
                var item = e as MRedemptionDetailPhysic;
                Intent intent = new Intent(this, typeof(PhysicalDetailActivity));
                intent.PutExtra(PhysicalDetailActivity.JsonData, JsonConvert.SerializeObject(item));
                StartActivityForResult(intent, 98);
                //
            }
            //
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                LoadData();
            }
        }
    }
}