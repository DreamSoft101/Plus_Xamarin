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
    public class MRewardDetailsAdapters : RecyclerView.Adapter
    {
        public event EventHandler<MRewardDetails> ItemClick;
        private Context mContext;
        private List<MRewardDetails> mLstItems;
        private int intPositionRemoved = -1;

        public MRewardDetailsAdapters(Context conext, List<MRewardDetails> lstItem)
        {
            this.mContext = conext;
            this.mLstItems = new List<MRewardDetails>();
            var listMonth = (from lst in lstItem
                             orderby lst.Year, lst.Month
                             select lst.Year + "_" + lst.Month).Distinct().ToList();
            foreach (var month in listMonth)
            {
                var months = lstItem.Where(p => (p.Year + "_" + p.Month) == month).ToList();
                MRewardDetails item = new MRewardDetails();
                item.Month = months[0].Month;
                item.Year = months[0].Year;
                item.OpenPointBalance = months.Sum(p => p.OpenPointBalance);
                item.PointsRedeem = months.Sum(p => p.PointsRedeem);
                item.PointsTransfer = months.Sum(p => p.PointsTransfer);
                item.PointsAdjusted = months.Sum(p => p.PointsAdjusted);
                item.PointsExpired = months.Sum(p => p.PointsExpired);
                item.PointBalance = months.Sum(p => p.PointBalance);
                mLstItems.Add(item);
            }
        }

        public override int ItemCount
        {
            get
            {
                return mLstItems.Count;
            }
        }



        public MRewardDetails GetBaseItem(int pos)
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
            public MRewardDetails mBaseItem;
            public TextView txtDate;
            public TextView txtOpen;
            public TextView txtEnd;
            public RecyclerView.Adapter mAdapter;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                this.txtDate = itemView.FindViewById<TextView>(Resource.Id.txtDate);
                this.txtOpen = itemView.FindViewById<TextView>(Resource.Id.txtOpenPoints);
                this.txtEnd = itemView.FindViewById<TextView>(Resource.Id.txtEndPoints);
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
            baseHolder.txtDate.Text = item.Month + "/" + item.Year;
            baseHolder.txtOpen.Text = string.Format(mContext.GetString(Resource.String.loy_format_point), item.OpenPointBalance);
            baseHolder.txtEnd.Text = string.Format(mContext.GetString(Resource.String.loy_format_point), item.PointBalance);

            //var gdetail = mLstMemberGroupDetail.Where(p => p.MemberTypeID == item.idMemberType).FirstOrDefault();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.loy_item_rewarddetails_cardview, parent, false);
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