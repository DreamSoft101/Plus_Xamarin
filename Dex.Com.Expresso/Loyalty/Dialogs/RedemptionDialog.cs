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
using Square.Picasso;
using Loyalty.Threads;
using Dex.Com.Expresso;
using Loyalty.Utils;

namespace Dex.Com.Expresso.Loyalty.Droid.Dialogs
{
    public class RedemptionDialog : DialogFragment
    {
        private RedemptionProduct mProduct;
        private RedemptionProductDetail mDetail;
        private Document mDocument;
        public TextView mTxtName;
        private TextView mTxtDescription;
        public TextView mTxtPrice;
        public ImageView mImgProduct;
        private Button mBtnRedemption;

        public delegate void onChange();
        public onChange OnChange;

        public static RedemptionDialog NewInstance(Bundle bundle, RedemptionProduct product, RedemptionProductDetail detail, Document document)
        {
            RedemptionDialog fragment = new RedemptionDialog() { mProduct = product, mDetail = detail, mDocument = document };
            fragment.Arguments = bundle;
            return fragment;
        }

        public override void OnResume()
        {
            base.OnResume();

            try
            {
                this.Dialog.Window.SetLayout(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.WrapContent);
            }
            catch (Exception ex)
            {

            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            View view = inflater.Inflate(Resource.Layout.loy_dialog_redemption_product, container, false);

            mTxtName = view.FindViewById<TextView>(Resource.Id.txtName);
            mTxtPrice = view.FindViewById<TextView>(Resource.Id.txtPrice);
            mImgProduct = view.FindViewById<ImageView>(Resource.Id.imgProduct);
            mTxtDescription = view.FindViewById<TextView>(Resource.Id.txtDescription);
            mBtnRedemption = view.FindViewById<Button>(Resource.Id.btnRedemption);


            mTxtName.Text = mProduct.ProductName;
            mTxtPrice.Text = string.Format(Activity.GetString(Resource.String.loy_format_point), mDetail.RedeemPoint);
            mTxtDescription.Text = mProduct.ProductDesc;

            if (mProduct.ImageID != null)
            {
                Picasso.With(this.Activity).Load(Cons.API_IMG_URL + mProduct.ImageID).Error(Resource.Drawable.loy_img_food01).Into(mImgProduct);

            }
            else
            {
                Picasso.With(this.Activity).Load(Resource.Drawable.loy_img_food01).Into(mImgProduct);
            }


            mBtnRedemption.Click += MBtnRedemption_Click;
            return view;
        }

        private void MBtnRedemption_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            MemberRedeemInfoProduct item = new MemberRedeemInfoProduct();
            item.ProductName = mProduct.ProductName;
            item.RedemptionProductDetailId = mDetail.RedemptionProductDetailID;
            item.RedemptionProductId = mProduct.RedemptionProductID;
            item.Quantity = 1;
            item.intType = mProduct.intProductType == null ? -1 : mProduct.intProductType.Value;
            item.Points = mDetail.RedeemPoint;
            item.strOldPrice = mDetail.strOldPrice;
            item.strURLImage = mDocument == null ? "" : mDocument.FileName;
            if (mProduct.intProductType == 6)
            {
                //if (mProduct.intau)
                if (mProduct.intAuthOnsite != null)
                {
                    item.IsLock = mProduct.intAuthOnsite != 0;
                }
            }
            else
            {
                item.IsLock = false;
            }
            RedemptionThreads thread = new RedemptionThreads();
            thread.AddProductToCart(item);
            this.Dialog.Dismiss();
            if (OnChange != null)
            {
                OnChange();
            }
        }
    }
}