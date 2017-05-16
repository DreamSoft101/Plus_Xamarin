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
using Loyalty.Models.ServiceOutput;
using Newtonsoft.Json;
using Loyalty.Threads;
using Loyalty.Models.Database;
using Square.Picasso;
using ZXing;
using Android.Graphics;
using ZXing.QrCode;
using Dex.Com.Expresso;
using Dex.Com.Expresso.Loyalty.Droid.Activities;
using EXPRESSO.Utils;

namespace Dex.Com.Expresso.Loyalty.Activities
{
    [Activity(Label = "EVoucherDetailActivity_v2")]
    public class EVoucherDetailActivity_v2 : BaseActivity
    {
        public RedemptionProduct mProduct;
        public RedemptionProductDetail mProductDetail;
        public Document mDocument;
        public static string JsonData = "JSONDATA";

        private MemberVoucherInfo mMRedemptionDetailVoucher;
        private LinearLayout lnlData;
        private ProgressBar prbLoading;
        private TextView mTxtName, mTxtDescription, mTxtPoints, mtxtDateUse, mTxtStatus, mTxtDateRedeem, mTxtDateUse, mTxtExpired;
        private ImageView mImgProduct, mImgQRCode, mImgBarCode;
        private Button mBtnUse;
        private Button mBtnCancel;
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.loy_activity_evoucherdetail;
            }
        }

        private Bitmap GetQRCode(string content)
        {
            var barcodeWriter = new ZXing.Mobile.BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 300,
                    Margin = 0
                }
            };

            var member = Cons.myEntity.User.LoyaltyAccount.MemberProfile;

            if (content.Contains("#MemberName#"))
            {
                content = content.Replace("#MemberName#", Cons.myEntity.User.LoyaltyAccount.MemberProfile.strDisplayName);
            }
            if (content.Contains("#ICNo#"))
            {
                content = content.Replace("#ICNo#", Cons.myEntity.User.LoyaltyAccount.MemberProfile.strMasterAccountReferenceNo);
            }
            if (content.Contains("#MasterAccountId#"))
            {
                content = content.Replace("#MasterAccountId#", Cons.myEntity.User.LoyaltyAccount.MemberProfile.idMasterAccount  + "");
            }
            if (content.Contains("#RedeemDate#"))
            {
                content = content.Replace("#RedeemDate#", string.Format(GetString(Resource.String.loy_format_date_time), mMRedemptionDetailVoucher.dtRedeem.Value));
            }
            if (content.Contains("#VoucherNo#"))
            {
                content = content.Replace("#VoucherNo#", mMRedemptionDetailVoucher.strVoucherNo);
            }
            var barcode = barcodeWriter.Write(content);
            return barcode;
        }

        private Bitmap GetBarCode()
        {
            var barcodeWriter = new ZXing.Mobile.BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 600,
                    Height = 300,
                    Margin = 0
                }
            };
            var barcode = barcodeWriter.Write(mMRedemptionDetailVoucher.strVoucherNo);
            return barcode;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            mImgProduct = FindViewById<ImageView>(Resource.Id.imgProduct);
            mImgQRCode = FindViewById<ImageView>(Resource.Id.imgQRCode);
            mImgBarCode = FindViewById<ImageView>(Resource.Id.imgBarCode);
            prbLoading = FindViewById<ProgressBar>(Resource.Id.prbLoading);
            lnlData = FindViewById<LinearLayout>(Resource.Id.lnlData);
            mTxtName = FindViewById<TextView>(Resource.Id.txtName);
            mTxtDescription = FindViewById<TextView>(Resource.Id.txtDescription);
            lnlData.Visibility = ViewStates.Gone;
            mTxtPoints = FindViewById<TextView>(Resource.Id.txtPoints);
            mBtnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            mBtnUse = FindViewById<Button>(Resource.Id.btnUseNow);
            mtxtDateUse = FindViewById<TextView>(Resource.Id.txtDateUse);
            mBtnUse.Click += MBtnUse_Click;
            mBtnCancel.Click += MBtnCancel_Click;
            string jsondata = this.Intent.GetStringExtra(JsonData);
            mMRedemptionDetailVoucher = JsonConvert.DeserializeObject<MemberVoucherInfo>(jsondata);
            mTxtStatus = FindViewById<TextView>(Resource.Id.txtStatus);
            mTxtDateRedeem = FindViewById<TextView>(Resource.Id.txtDateRedeem);
            mTxtExpired = FindViewById<TextView>(Resource.Id.txtExpired);


            mBtnCancel.Text = "Dismiss";



            mImgBarCode.SetImageBitmap(GetBarCode());

            if (mMRedemptionDetailVoucher.intStatus == 1)
            {
                mTxtStatus.Text = "Status - Active";
                mTxtDateRedeem.Text = "Redeem : " + mMRedemptionDetailVoucher.dtRedeem.Value.ToString(GetString(Resource.String.loy_format_date_time));
                mTxtExpired.Text = mMRedemptionDetailVoucher.dtExpiry == null ? "Expire by: No Expire" : "Expire by: " + mMRedemptionDetailVoucher.dtExpiry.Value.ToString(GetString(Resource.String.loy_format_date_time));
                mtxtDateUse.Visibility = ViewStates.Gone;
                mBtnUse.Visibility = ViewStates.Visible;
                mBtnCancel.Visibility = ViewStates.Gone;


            }
            else if (mMRedemptionDetailVoucher.intStatus == 2)
            {
                mTxtStatus.Text = "Status - Used";
                mTxtDateRedeem.Text = "Redeem : " + mMRedemptionDetailVoucher.dtRedeem.Value.ToString(GetString(Resource.String.loy_format_date_time));
                mTxtExpired.Text = mMRedemptionDetailVoucher.dtExpiry == null ? "Expire by: No Expire" : "Expire by: " + mMRedemptionDetailVoucher.dtExpiry.Value.ToString(GetString(Resource.String.loy_format_date_time));
                mtxtDateUse.Text = mMRedemptionDetailVoucher.dtUse == null ? "Used On" : "Used On: " + mMRedemptionDetailVoucher.dtUse.Value.ToString(GetString(Resource.String.loy_format_date_time));
                mtxtDateUse.Visibility = ViewStates.Visible;
                mBtnCancel.Visibility = ViewStates.Visible;
                mBtnUse.Visibility = ViewStates.Gone;
            }
            else if (mMRedemptionDetailVoucher.intStatus == 3)
            {
                mTxtStatus.Text = "Status - Expired";
                mTxtDateRedeem.Text = "Redeem : " + mMRedemptionDetailVoucher.dtRedeem.Value.ToString(GetString(Resource.String.loy_format_date_time));
                mTxtExpired.Text = mMRedemptionDetailVoucher.dtExpiry == null ? "Expire by: No Expire" : "Expire by: " + mMRedemptionDetailVoucher.dtExpiry.Value.ToString(GetString(Resource.String.loy_format_date_time));

                mtxtDateUse.Visibility = ViewStates.Gone;
                //mtxtDateUse.Text = mMRedemptionDetailVoucher.dtDismiss == null ? "Dismiss On: " : "Dismiss On: " + mMRedemptionDetailVoucher.dtDismiss.Value.ToString(GetString(Resource.String.loy_format_date_time));
                //mtxtDateUse.Visibility = ViewStates.Visible;
                mBtnCancel.Visibility = ViewStates.Gone;
                mBtnUse.Visibility = ViewStates.Gone;
            }
            else if (mMRedemptionDetailVoucher.intStatus == 4)
            {
                mTxtStatus.Text = "Status - Dismiss";
                mTxtDateRedeem.Text = "Redeem : " + mMRedemptionDetailVoucher.dtRedeem.Value.ToString(GetString(Resource.String.loy_format_date_time));
                mTxtExpired.Text = mMRedemptionDetailVoucher.dtExpiry == null ? "Expire by: No Expire" : "Expire by: " + mMRedemptionDetailVoucher.dtExpiry.Value.ToString(GetString(Resource.String.loy_format_date_time));
                mtxtDateUse.Text = mMRedemptionDetailVoucher.dtDismiss == null ? "Dismiss On: " : "Dismiss On: " + mMRedemptionDetailVoucher.dtDismiss.Value.ToString(GetString(Resource.String.loy_format_date_time));
                mtxtDateUse.Visibility = ViewStates.Visible;
                mBtnCancel.Visibility = ViewStates.Gone;
                mBtnUse.Visibility = ViewStates.Gone;
            }

             RedemptionThreads thread = new RedemptionThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                if (result.Data is RedemptionProduct)
                {
                    mProduct = result.Data as RedemptionProduct;
                    this.Title = mProduct.ProductName;
                    mTxtName.Text = mProduct.ProductName;
                    mTxtDescription.Text = mProduct.ProductDesc;
                    mTxtPoints.Text = string.Format(GetString(Resource.String.loy_format_point), mMRedemptionDetailVoucher.intPoint);
                    if (!string.IsNullOrEmpty(mProduct.strBarcodeMessage))
                    {
                        mImgQRCode.SetImageBitmap(GetQRCode(mProduct.strBarcodeMessage));

                    }
                    else
                    {
                        mImgQRCode.Visibility = ViewStates.Gone;
                    }
                    
                }
                else if (result.Data is RedemptionProductDetail)
                {
                    mProductDetail = result.Data as RedemptionProductDetail;

                }
                else if (result.Data is Document)
                {
                    var document = result.Data as Document;
                    if (document != null)
                    {
                        Picasso.With(this).Load(document.FileName).Into(mImgProduct);
                    }
                }
                else if (result.Data is string)
                {
                    lnlData.Visibility = ViewStates.Visible;
                }
            };
            thread.EVoucherDetail(mMRedemptionDetailVoucher);

            // Create your application here
        }

        private void MBtnCancel_Click(object sender, EventArgs e)
        {
            //Finish();
            if (mMRedemptionDetailVoucher.intStatus == 2)
            {
                RedemptionThreads thread = new RedemptionThreads();
                thread.UseEVoucher(mMRedemptionDetailVoucher.strVoucherNo, 4);
                SetResult(Android.App.Result.Ok);
                Finish();
            }
        }

        private void MBtnUse_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (mMRedemptionDetailVoucher.intStatus == 1)
            {
                RedemptionThreads thread = new RedemptionThreads();
                thread.UseEVoucher(mMRedemptionDetailVoucher.strVoucherNo, 2);
                SetResult(Android.App.Result.Ok);
                Finish();
            }
        }
    }
}