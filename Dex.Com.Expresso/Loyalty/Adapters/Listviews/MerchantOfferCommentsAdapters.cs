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
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.Listviews
{
    public class MerchantOfferCommentsAdapters : MyBaseAdapter
    {
        private List<Merchant_Rating_Paging_Info> mLstItem;

        public void AddOrUpdateItem(Merchant_Rating_Paging_Info item)
        {
            var cItem = mLstItem.Where(p => p.UserName == item.UserName).FirstOrDefault();
            if (cItem != null)
            {
                cItem.dtCreated = item.dtCreated;
                cItem.intRate = item.intRate;
                cItem.strReview = item.strReview;
            }
            else
            {
                mLstItem.Add(item);
                
            }
            mLstItem = mLstItem.OrderByDescending(p => p.dtCreated).ToList();
            this.NotifyDataSetChanged();
        }
        public MerchantOfferCommentsAdapters(Context conext, List<Merchant_Rating_Paging_Info> lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
            mLstItem = mLstItem.OrderByDescending(p => p.dtCreated).ToList();
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

        public Merchant_Rating_Paging_Info GetBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public TextView mTxtUserName;
            public TextView mStrComment;
            public RatingBar rtbRating;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder viewHoder = null;
            var item = GetBaseItem(position);
            if (convertView == null)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_comments, null);
                viewHoder = new ViewHolder();
                viewHoder.mTxtUserName = convertView.FindViewById<TextView>(Resource.Id.txtUserName);
                viewHoder.mStrComment = convertView.FindViewById<TextView>(Resource.Id.txtContent);
                viewHoder.rtbRating = convertView.FindViewById<RatingBar>(Resource.Id.rtbRating);
                convertView.Tag = viewHoder;
            }
            else
            {
                viewHoder = convertView.Tag as ViewHolder;
            }

            //Picasso.With(this.mContext).Load("file://" + item.FileName).Into(viewHoder.imgLogo);

            viewHoder.mTxtUserName.Text = item.UserName;
            viewHoder.mStrComment.Text = item.strReview;
            viewHoder.rtbRating.Rating = item.intRate;
            return convertView;
        }
    }
}