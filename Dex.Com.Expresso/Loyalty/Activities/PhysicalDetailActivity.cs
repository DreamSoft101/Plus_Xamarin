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
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    [Activity(Label = "PhysicalDetailActivity")]
    public class PhysicalDetailActivity : BaseActivity
    {
        public RedemptionProduct mProduct;
        public RedemptionProductDetail mProductDetail;
        public Document mDocument;
        public static string JsonData = "JSONDATA";
        private MRedemptionDetailPhysic mMRedemptionDetailPhysic;
        private LinearLayout lnlData;
        private ProgressBar prbLoading;
        private TextView mTxtName, mTxtDescription, mTxtPoints;
        private ImageView mImgProduct;
        private Button mBtnCancel;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.loy_activity_physicaldetail;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            mImgProduct = FindViewById<ImageView>(Resource.Id.imgProduct);
            prbLoading = FindViewById<ProgressBar>(Resource.Id.prbLoading);
            lnlData = FindViewById<LinearLayout>(Resource.Id.lnlData);
            mTxtName = FindViewById<TextView>(Resource.Id.txtName);
            mTxtDescription = FindViewById<TextView>(Resource.Id.txtDescription);
            lnlData.Visibility = ViewStates.Gone;
            mTxtPoints = FindViewById<TextView>(Resource.Id.txtPoints);
            mBtnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            mBtnCancel.Click += MBtnCancel_Click;

            string jsondata = this.Intent.GetStringExtra(JsonData);
            mMRedemptionDetailPhysic = JsonConvert.DeserializeObject<MRedemptionDetailPhysic>(jsondata);
            this.Title = mMRedemptionDetailPhysic.ProductName;
            RedemptionThreads thread = new RedemptionThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                if (result.Data is RedemptionProduct)
                {
                    mProduct = result.Data as RedemptionProduct;
                    mTxtName.Text = mProduct.ProductName;
                    mTxtDescription.Text = mProduct.ProductDesc;
                    mTxtPoints.Text = string.Format(GetString(Resource.String.loy_format_point), mMRedemptionDetailPhysic.RedeemPoints);

         
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
            thread.PhysicalDetail(mMRedemptionDetailPhysic);
            // Create your application here
        }

        private void MBtnCancel_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}