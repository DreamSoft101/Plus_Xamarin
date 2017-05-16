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
using Loyalty.Models.Database;
using Newtonsoft.Json;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Square.Picasso;
//using ZXing.Mobile;
using Dex.Com.Expresso;
namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    [Activity(Label = "AuthencationEvoucher")]
    public class AuthencationEvoucher : BaseActivity
    {
        public static string JSONDATA = "JSONDATA";
        public static string ID = "ID";
        public RedemptionProduct mProductRedemption;
        public RedemptionProductDetail mProductDetail;
        public Document mDocument;
        public MemberRedeemInfoProduct mProduct;
        private TextView mTxtName, mTxtPoints, mTxtDescription, mTxtTextQRCode;
        private ImageView mImgProduct, mImgQRCode;
        private EditText mTxtPassword;
        private Button mBtnCOnfirm;
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.loy_activity_authentication_evoucher;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ZXing.Mobile.MobileBarcodeScanner.Initialize(Application);
            string jsondata = this.Intent.GetStringExtra(JSONDATA);
            mProduct = JsonConvert.DeserializeObject<MemberRedeemInfoProduct>(jsondata);
            mImgProduct = FindViewById<ImageView>(Resource.Id.imgProduct);
            mTxtName = FindViewById<TextView>(Resource.Id.txtName);
            mTxtPoints = FindViewById<TextView>(Resource.Id.txtPoints);
            mTxtDescription = FindViewById<TextView>(Resource.Id.txtDescription);
            mTxtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            mTxtTextQRCode = FindViewById<TextView>(Resource.Id.txtTextQRCode);
            mImgQRCode = FindViewById<ImageView>(Resource.Id.imgQRCode);
            mBtnCOnfirm = FindViewById<Button>(Resource.Id.btnConfirm);
            RedemptionThreads thread = new RedemptionThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                if (result.Data is RedemptionProduct)
                {
                    mProductRedemption = result.Data as RedemptionProduct;

                    mTxtName.Text = mProductRedemption.ProductName;
                    mTxtDescription.Text = mProductRedemption.ProductDesc;
                    mTxtPoints.Text = string.Format(GetString(Resource.String.loy_format_point), mProduct.Points);
                    //

                    switch (mProductRedemption.intAuthOnsite)
                    {
                        case 0:
                            {
                                //None
                                break;
                            }
                        case 1:
                            {
                                //2DBarcode
                                mTxtTextQRCode.Visibility = ViewStates.Gone;
                                mTxtPassword.Visibility = ViewStates.Gone;
                                break;
                            }
                        case 2:
                            {
                                //Password
                                mTxtTextQRCode.Visibility = ViewStates.Gone;
                                mImgQRCode.Visibility = ViewStates.Gone;
                                break;
                            }
                        case 3:
                            {
                                //All
                                break;
                            }
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
            };
            thread.AuthencationEVoucher(mProduct);

            mBtnCOnfirm.Click += MBtnCOnfirm_Click;

            mImgQRCode.Click += MImgQRCode_Click;
            // Create your application here
        }

       
        private void MBtnCOnfirm_Click(object sender, EventArgs e)
        {
            string result = mTxtPassword.Text;
            if (string.IsNullOrEmpty(mProductRedemption.strAuthOnsiteCode) || result == mProductRedemption.strAuthOnsiteCode)
            {
                RedemptionThreads thread = new RedemptionThreads();
                thread.UnLockEVoucher(mProduct.Id);
              

                Intent intentResult = new Intent();
                intentResult.PutExtra(ID, mProduct.Id);
                this.SetResult(Result.Ok, intentResult);
                Finish();
                return;
            }
            else
            {
                //Fail
                Toast.MakeText(this, Resource.String.loy_authen_mess_wrongqrcode, ToastLength.Short).Show();
            }
        }

        private async void MImgQRCode_Click(object sender, EventArgs e)
        {
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();
            if (result != null)
            {
                //Toast.MakeText(this, result.Text, ToastLength.Short).Show();
                if (mProductRedemption.strAuthOnsiteCode == result.Text)
                {
                    //Success
                    RedemptionThreads thread = new RedemptionThreads();
                    thread.UnLockEVoucher(mProduct.Id);

                    Intent intentResult = new Intent();
                    intentResult.PutExtra(ID, mProduct.Id);
                    this.SetResult(Result.Ok,intentResult);
                    Finish();
                    return;
                }
                else
                {
                    //Fail
                    Toast.MakeText(this, Resource.String.loy_authen_mess_wrongqrcode, ToastLength.Short).Show();
                }
            }
        }
    }
}