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

namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    [Activity(Label = "EVoucherDetailActivity")]
    public class EVoucherDetailActivity : BaseActivity
    {
        public RedemptionProduct mProduct;
        public RedemptionProductDetail mProductDetail;
        public Document mDocument;
        public static string JsonData = "JSONDATA";

        private MRedemptionDetailVoucher mMRedemptionDetailVoucher;
        private LinearLayout lnlData;
        private ProgressBar prbLoading;
        private TextView mTxtName, mTxtDescription, mTxtPoints;
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
                    Height = 300
                }
            };
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
                    Height = 300
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

            mBtnUse.Click += MBtnUse_Click;
            mBtnCancel.Click += MBtnCancel_Click;
            string jsondata = this.Intent.GetStringExtra(JsonData);
            mMRedemptionDetailVoucher = JsonConvert.DeserializeObject<MRedemptionDetailVoucher>(jsondata);
            this.Title = mMRedemptionDetailVoucher.ProductName;

            mImgBarCode.SetImageBitmap(GetBarCode());

            if (mMRedemptionDetailVoucher.Status == 1)
            {
                mBtnUse.Visibility = ViewStates.Visible;
            }
            else
            {
                mBtnUse.Visibility = ViewStates.Gone;
            }

            RedemptionThreads thread = new RedemptionThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                if (result.Data is RedemptionProduct)
                {
                    mProduct = result.Data as RedemptionProduct;
                    mTxtName.Text = mProduct.ProductName;
                    mTxtDescription.Text = mProduct.ProductDesc;
                    mTxtPoints.Text = string.Format(GetString(Resource.String.loy_format_point), mMRedemptionDetailVoucher.RedeemPoints);

                    mImgQRCode.SetImageBitmap(GetQRCode(mProduct.strBarcodeMessage));
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
                        Picasso.With(this).Load( document.FileName).Into(mImgProduct);
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
            Finish();
        }

        private void MBtnUse_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (mMRedemptionDetailVoucher.Status == 1)
            {
                RedemptionThreads thread = new RedemptionThreads();
                thread.UseEVoucher(mMRedemptionDetailVoucher.strVoucherNo, 2);
                SetResult(Android.App.Result.Ok);
                Finish();
            }
        }
    }
}