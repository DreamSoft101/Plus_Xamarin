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
using Dex.Com.Expresso.Loyalty.Droid.Adapters;
using Loyalty.Models.Database;
using Square.Picasso;

namespace Dex.Com.Expresso.Loyalty.Adapters.Listviews
{
    public class EVoucherProductAdapter : MyBaseAdapter
    {
        private List<RedemptionProduct> mLstItem;
        private List<RedemptionProductDetail> mLstDetail;
        private List<Document> mLstDocument;
        public delegate void onItemClick(int position);
        public onItemClick OnItemClick;
        //private List<MerchantProduct> mLstItem;
        public EVoucherProductAdapter(Context conext, List<RedemptionProduct> lstItem, List<RedemptionProductDetail> lstDetail, List<Document> lstDocument)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
            this.mLstDetail = lstDetail;
            this.mLstDocument = lstDocument;
        }

        public override int Count
        {
            get
            {
                return mLstItem.Count;
            }
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return null;// mLstItem[position].ToJavaObject();
        }

        public RedemptionProduct GetBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public ImageView mImgProduct;
            public TextView mTxtName, mtxtDetail, mTxtVoucherCode, mTxtPoints, mTxtStatus;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder viewHoder = null;
            var item = GetBaseItem(position);
            if (convertView == null)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_evoucher, null);
                viewHoder = new ViewHolder();
                viewHoder.mTxtName = convertView.FindViewById<TextView>(Resource.Id.txtName);
                viewHoder.mTxtPoints = convertView.FindViewById<TextView>(Resource.Id.txtPoints);
                viewHoder.mTxtVoucherCode = convertView.FindViewById<TextView>(Resource.Id.txtVoucherCode);
                viewHoder.mTxtStatus = convertView.FindViewById<TextView>(Resource.Id.txtStatus);
                viewHoder.mtxtDetail = convertView.FindViewById<TextView>(Resource.Id.txtDetail);
                viewHoder.mImgProduct = convertView.FindViewById<ImageView>(Resource.Id.imgImage);
                viewHoder.mImgProduct.Click += MImgProduct_Click;
                convertView.Tag = viewHoder;
            }
            else
            {
                viewHoder = convertView.Tag as ViewHolder;
            }

            viewHoder.mTxtVoucherCode.Visibility = ViewStates.Gone;
            viewHoder.mTxtStatus.Visibility = ViewStates.Gone;
           
            viewHoder.mTxtName.Text = item.ProductName;
            var detail = mLstDetail.Where(p => p.RedemptionProductID == item.RedemptionProductID).OrderBy(p => p.RedeemPoint).FirstOrDefault();
            if (detail != null)
            {
                viewHoder.mTxtPoints.Text = string.Format(mContext.GetString(Resource.String.loy_format_point), detail.RedeemPoint);
            }

            if (item.ImageID != null)
            {
                var document = mLstDocument.Where(p => p.ID == item.ImageID).FirstOrDefault();
                if (document != null)
                {
                    Picasso.With(mContext).Load(document.FileName).Into(viewHoder.mImgProduct);
                }
                else
                {
                    Picasso.With(mContext).Load(Resource.Drawable.loy_img_food01).Into(viewHoder.mImgProduct);
                }
            }
            else
            {
                Picasso.With(mContext).Load(Resource.Drawable.loy_img_food01).Into(viewHoder.mImgProduct);
            }

            viewHoder.mImgProduct.Tag = position.ToString();


            //viewHoder.mTxtPrice = mLstDetail.Where(p => p.RedemptionProductID == item.RedemptionProductID).FirstOrDefault().RedeemPoint;
            //viewHoder.
            return convertView;
        }

        private void MImgProduct_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (OnItemClick != null)
            {
                View view = sender as View;
                int position = int.Parse(view.Tag.ToString());
                OnItemClick(position);
            }
        }
    }
}