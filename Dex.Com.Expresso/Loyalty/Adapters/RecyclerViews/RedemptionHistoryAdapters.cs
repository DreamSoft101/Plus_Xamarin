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
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.RecyclerViews
{
    public class RedemptionHistoryAdapters : RecyclerView.Adapter
    {
        public event EventHandler<RedemptionHistory> ItemClick;
        private Context mContext;
        private List<RedemptionHistory> mLstItems;
        private int intPositionRemoved = -1;

        public RedemptionHistoryAdapters(Context conext, List<RedemptionHistory> lstItem)
        {
            this.mContext = conext;
            this.mLstItems = lstItem.OrderByDescending(p => p.RedeemDate).ToList();
        }

        public override int ItemCount
        {
            get
            {
                return mLstItems.Count;
            }
        }



        public RedemptionHistory GetBaseItem(int pos)
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
            public RedemptionHistory mBaseItem;
            public TextView txtNo;
            public TextView txtDate;
            public TextView txtPoints;
            public TextView txtAddress;
            public ImageView imgLogo;
            public TextView txtStatus;
            public View mRoot;
            public RecyclerView.Adapter mAdapter;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                this.txtNo = itemView.FindViewById<TextView>(Resource.Id.txtNo);
                this.txtDate = itemView.FindViewById<TextView>(Resource.Id.txtDate);
                this.txtPoints = itemView.FindViewById<TextView>(Resource.Id.txtPoints);
                this.txtAddress = itemView.FindViewById<TextView>(Resource.Id.txtAddress);
                this.txtStatus = ItemView.FindViewById<TextView>(Resource.Id.txtStartus);
                this.mRoot = itemView;
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
            baseHolder.txtAddress.Text = item.Address;
            baseHolder.txtDate.Text = item.RedeemDate.ToString(this.mContext.GetString(Resource.String.loy_format_date_time));
            baseHolder.txtPoints.Text = string.Format(this.mContext.GetString(Resource.String.loy_format_point), item.TotalPoint);
            baseHolder.txtNo.Text = item.strRedemptionNo;

            if (position % 2 == 0)
            {
                baseHolder.mRoot.SetBackgroundColor(mContext.Resources.GetColor(Resource.Color.loy_bg_gray));
            }
            else
            {
                baseHolder.mRoot.SetBackgroundColor(mContext.Resources.GetColor(Resource.Color.White));
            }

            switch (item.RedemptionStatus)
            {
                case 0:
                    baseHolder.txtStatus.Text = mContext.GetString(Resource.String.loy_txt_Pending);
                    break;
                case 1:
                    baseHolder.txtStatus.Text = mContext.GetString(Resource.String.loy_text_inprogress);
                    break;
                case 2:
                    baseHolder.txtStatus.Text = mContext.GetString(Resource.String.loy_text_cancel);
                    break;
                case 3:
                    baseHolder.txtStatus.Text = mContext.GetString(Resource.String.loy_text_closed);
                    break;
            }
            //var gdetail = mLstMemberGroupDetail.Where(p => p.MemberTypeID == item.idMemberType).FirstOrDefault();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.loy_item_redemption_history, parent, false);
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