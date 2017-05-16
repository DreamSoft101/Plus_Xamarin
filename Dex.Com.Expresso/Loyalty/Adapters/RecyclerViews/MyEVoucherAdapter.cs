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
using Loyalty.Models.ServiceOutput;
using Square.Picasso;

namespace Dex.Com.Expresso.Loyalty.Adapters.RecyclerViews
{
    public class MyEVoucherAdapter : RecyclerView.Adapter
    {
        public event EventHandler<MemberVoucherInfo> ItemClick;
        private Context mContext;
        private List<MemberVoucherInfo> mLstItems;
        private int intPositionRemoved = -1;

        public MyEVoucherAdapter(Context conext, List<MemberVoucherInfo> lstItem)
        {
            this.mContext = conext;
            this.mLstItems = lstItem;//.OrderByDescending(p => p.RedeemDate).ToList();
        }

        public override int ItemCount
        {
            get
            {
                return mLstItems.Count;
            }
        }



        public MemberVoucherInfo GetBaseItem(int pos)
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
            public MemberVoucherInfo mBaseItem;
            public ImageView mImgImage;
            public TextView mTxtName, mtxtDetail, mTxtVoucherCode, mTxtPoints, mTxtStatus;
            public View mRoot;
            public RecyclerView.Adapter mAdapter;

            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                this.mImgImage = itemView.FindViewById<ImageView>(Resource.Id.imgImage);
                this.mRoot = itemView;
                this.mTxtName = itemView.FindViewById<TextView>(Resource.Id.txtName);
                this.mTxtPoints = itemView.FindViewById<TextView>(Resource.Id.txtPoints);
                this.mTxtVoucherCode = itemView.FindViewById<TextView>(Resource.Id.txtVoucherCode);
                this.mTxtStatus = itemView.FindViewById<TextView>(Resource.Id.txtStatus);
                this.mtxtDetail = itemView.FindViewById<TextView>(Resource.Id.txtDetail);
                itemView.Click += (sender, e) => itemClick(base.Position);
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);
            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.mContext = mContext;
            baseHolder.mBaseItem = item;
            baseHolder.mAdapter = this;
            
            baseHolder.mTxtPoints.Visibility = ViewStates.Gone;
            baseHolder.mTxtName.Text = item.strName;
            baseHolder.mTxtStatus.Text = "Status - " + (item.intStatus == 1 ? "Active" : item.intStatus == 2 ? "Used" : item.intStatus == 3 ? "Expired" : "Dismiss");
            baseHolder.mTxtVoucherCode.Text = "Voucher Code - " + item.strVoucherNo;

            Picasso.With(mContext).Load(item.strImgPicture).Error(Resource.Drawable.loy_img_food01).Into(baseHolder.mImgImage);

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