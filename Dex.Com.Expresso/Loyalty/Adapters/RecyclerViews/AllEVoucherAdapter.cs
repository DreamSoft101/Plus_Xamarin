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
using Android.Support.V7.Widget;
using Loyalty.Models.Database;
using Square.Picasso;

namespace Dex.Com.Expresso.Loyalty.Adapters.RecyclerViews
{
    public class AllEVoucherAdapter : RecyclerView.Adapter
    {
        public event EventHandler<RedemptionProduct> ItemClick;
        public List<Document> mLstDocument = new List<Document>();
        public List<RedemptionProductDetail> mLstDetail;
        private Context mContext;
        private List<RedemptionProduct> mLstItems;
        private int intPositionRemoved = -1;
        
        public AllEVoucherAdapter(Context conext, List<RedemptionProduct> lstItem, List<Document> lstDocument, List<RedemptionProductDetail> mLstDetail)
        {
            this.mContext = conext;
            this.mLstDocument = lstDocument;
            this.mLstItems = lstItem;//.OrderByDescending(p => p.RedeemDate).ToList();
            this.mLstDetail = mLstDetail;
        }

        public override int ItemCount
        {
            get
            {
                return mLstItems.Count;
            }
        }



        public RedemptionProduct GetBaseItem(int pos)
        {
            return mLstItems[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }


        private class BaseViewHolder : RecyclerView.ViewHolder
        {
            public Context mContext;
            public RedemptionProduct mBaseItem;
            public ImageView mImgImage;
            public TextView mTxtName, mtxtDetail, mTxtVoucherCode, mTxtPoints, mTxtStatus;
            public View mRoot;
            public RecyclerView.Adapter mAdapter;

            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                this.mImgImage = itemView.FindViewById<ImageView>(Resource.Id.imgImage);
                this.mTxtName = itemView.FindViewById<TextView>(Resource.Id.txtName);
                this.mTxtPoints = itemView.FindViewById<TextView>(Resource.Id.txtVoucherCode);
                this.mTxtVoucherCode = itemView.FindViewById<TextView>(Resource.Id.txtVoucherCode);
                this.mTxtStatus = itemView.FindViewById<TextView>(Resource.Id.txtStatus);
                this.mtxtDetail = itemView.FindViewById<TextView>(Resource.Id.txtDetail);
                this.mRoot = itemView;
                itemView.Click += (sender, e) => itemClick(base.Position);

                this.mTxtVoucherCode.Visibility = ViewStates.Gone;
                this.mTxtStatus.Visibility = ViewStates.Gone;
                this.mtxtDetail.Click += (sender, e) => itemClick(base.Position);
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);
            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.mContext = mContext;
            baseHolder.mBaseItem = item;
            baseHolder.mAdapter = this;
            baseHolder.mTxtName.Text = item.ProductName;
            //baseHolder.mTxtPoints = 


            var document = mLstDocument.Where(p => p.ID == item.ImageID).FirstOrDefault();
            if (document != null)
            {
                Picasso.With(mContext).Load(document.FileName).Error(Resource.Drawable.loy_img_food01).Into(baseHolder.mImgImage);
            }
            else
            {
                Picasso.With(mContext).Load(Resource.Drawable.loy_img_food01).Into(baseHolder.mImgImage);
            }

            var detail = mLstDetail.Where(p => p.RedemptionProductID == item.RedemptionProductID).OrderBy(p => p.RedeemPoint).FirstOrDefault();
            if (detail != null)
            {
                baseHolder.mTxtPoints.Text = string.Format(mContext.GetString(Resource.String.loy_format_point), detail.RedeemPoint);
            }
            //
            //var gdetail = mLstMemberGroupDetail.Where(p => p.MemberTypeID == item.idMemberType).FirstOrDefault();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.loy_item_evoucher, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
            return holder;
        }

        void OnClick(int position)
        {
            if (ItemClick != null)
            {
                ItemClick(this, GetBaseItem(position));
            }
        }

    }


}