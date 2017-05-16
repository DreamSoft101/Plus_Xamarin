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
using Dex.Com.Expresso.Loyalty.Droid.Utils;
using Square.Picasso;
using Dex.Com.Expresso;
using Loyalty.Utils;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.Listviews
{
    public class RedemptionProductAdapter : MyBaseAdapter
    {
        private List<RedemptionProduct> mLstItem;
        private List<RedemptionProductDetail> mLstDetail;
        private List<Document> mLstDocument;
        public delegate void onItemClick(int position);
        public onItemClick OnItemClick;
        //private List<MerchantProduct> mLstItem;
        public RedemptionProductAdapter(Context conext, List<RedemptionProduct> lstItem, List<RedemptionProductDetail> lstDetail, List<Document> lstDocument)
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
            public TextView mTxtName;
            public TextView mTxtPrice;
            public ImageView mImgProduct;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder viewHoder = null;
            var item = GetBaseItem(position);
            if (convertView == null)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_redemption, null);
                viewHoder = new ViewHolder();
                viewHoder.mTxtName = convertView.FindViewById<TextView>(Resource.Id.txtName);
                viewHoder.mTxtPrice = convertView.FindViewById<TextView>(Resource.Id.txtPrice);
                viewHoder.mImgProduct = convertView.FindViewById<ImageView>(Resource.Id.imgProduct);
                viewHoder.mImgProduct.Click += MImgProduct_Click;
                convertView.Tag = viewHoder;
            }
            else
            {
                viewHoder = convertView.Tag as ViewHolder;
            }

            viewHoder.mTxtName.Text = item.ProductName;
            if (item.ThumbnailID != null)
            {
                Picasso.With(mContext).Load(Cons.API_IMG_URL + item.ThumbnailID).Error(Resource.Drawable.loy_img_food01).Into(viewHoder.mImgProduct);

            }
            else
            {
                Picasso.With(mContext).Load(Resource.Drawable.loy_img_food01).Into(viewHoder.mImgProduct);
            }

            var detail = mLstDetail.Where(p => p.RedemptionProductID == item.RedemptionProductID).FirstOrDefault();
            if (detail != null)
            {
                viewHoder.mTxtPrice.Text = string.Format(Activity.GetString(Resource.String.loy_format_point), detail.RedeemPoint); 
            }
            else
            {
                viewHoder.mTxtPrice.Text = string.Format(Activity.GetString(Resource.String.loy_format_point),0);
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