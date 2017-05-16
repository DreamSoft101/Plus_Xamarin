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
using XamarinItemTouchHelper;
using Loyalty.Models.ServiceOutput;
using Loyalty.Models.Database;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.RecyclerViews
{
    public class MemberDetailAdapters : RecyclerView.Adapter
    {
        public delegate void onClickEStatement(MIMX_AccountDetails mBaseItem);
        public onClickEStatement OnClickEStatement;

        public event EventHandler<MIMX_AccountDetails> ItemClick;
        private Context mContext;
        private List<MIMX_AccountDetails> mLstItems;
        private int intPositionRemoved = -1;

        private List<MemberType> mLstMemberType;
        private List<MemberGroup> mLstMemberGroup;
        private List<MemberGroupDetail> mLstMemberGroupDetail;

        public MemberDetailAdapters(Context conext, List<MIMX_AccountDetails> lstItem)
        {
            this.mContext = conext;
            this.mLstItems = lstItem;
        }

        public override int ItemCount
        {
            get
            {
                return mLstItems.Count;
            }
        }

        

        public MIMX_AccountDetails GetBaseItem(int pos)
        {
            return mLstItems[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }


        private class BaseViewHolder : RecyclerView.ViewHolder
        {
            public delegate void onClickEStatement(MIMX_AccountDetails mBaseItem);
            public onClickEStatement OnClickEStatement;
            public Context mContext;
            public MIMX_AccountDetails mBaseItem;
            public TextView eStatement;
            public TextView txtCardNum;
            public TextView txtMemberType;
            public TextView txtPoints;
            public TextView txtStatus;
            public TextView txtExpDate;
            public ImageView imgLogo;
            public MemberType membertype;
            public TextView txtOpen, mTxtEarn, mTxtRedeem, mTxtExpired, mTxtTotal;
            public RecyclerView.Adapter mAdapter;
            public View root;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                this.txtCardNum = itemView.FindViewById<TextView>(Resource.Id.txtCardNumber);
                this.txtMemberType = itemView.FindViewById<TextView>(Resource.Id.txtMembertype);
                this.txtStatus = itemView.FindViewById<TextView>(Resource.Id.txtStatus);
                this.txtExpDate = ItemView.FindViewById<TextView>(Resource.Id.txtExpDate);
                this.txtOpen = ItemView.FindViewById<TextView>(Resource.Id.txtOpen);
                this.mTxtEarn = ItemView.FindViewById<TextView>(Resource.Id.txtEarned);
                this.mTxtRedeem = ItemView.FindViewById<TextView>(Resource.Id.txtRedeemed);
                this.mTxtExpired = ItemView.FindViewById<TextView>(Resource.Id.txtExpired);
                this.mTxtTotal = ItemView.FindViewById<TextView>(Resource.Id.txtTotal);
                this.eStatement = ItemView.FindViewById<TextView>(Resource.Id.txteStatement);
                this.eStatement.Click += EStatement_Click;
                this.root = itemView;
                itemView.Click += (sender, e) => itemClick(base.Position);
            }

            private void EStatement_Click(object sender, EventArgs e)
            {
                //throw new NotImplementedException();
                if (OnClickEStatement != null)
                {
                    OnClickEStatement(mBaseItem);
                }
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);
            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.mContext = mContext;
            baseHolder.mBaseItem = item;
            baseHolder.mAdapter = this;
            baseHolder.txtCardNum.Text = mContext.GetString(Resource.String.loy_text_cardnumber) + ": "  +  item.strMaskedCardNumber;
            baseHolder.txtMemberType.Text = item.strMemberTypeName;
            baseHolder.txtStatus.Text = mContext.GetString(Resource.String.loy_text_status) + ": " + (item.intStatus == 1 ? mContext.GetString(Resource.String.loy_txt_Active) : mContext.GetString(Resource.String.loy_txt_Inactive));
            try
            {
                baseHolder.txtExpDate.Text = mContext.GetString(Resource.String.loy_text_expdate) + ": " + ((DateTime.ParseExact(item.dteExpiryDate, "yyyyMMdd", null)).ToString("dd/MM/yyyy"));
            }
            catch (Exception ex)
            {
                baseHolder.txtExpDate.Visibility = ViewStates.Gone;
            }
            
            if (position % 2 == 0)
            {
                baseHolder.root.SetBackgroundColor(mContext.Resources.GetColor(Resource.Color.loy_bg_gray));
            }
            else
            {
                baseHolder.root.SetBackgroundColor(mContext.Resources.GetColor(Resource.Color.loy_bg_no));
            }
            if (item.Summary !=null)
            {
                baseHolder.txtOpen.Text = item.Summary.OpenBalancePoints.ToString();
                baseHolder.mTxtEarn.Text = item.Summary.PointsEarned.ToString();
                baseHolder.mTxtRedeem.Text = item.Summary.PointsRedeem.ToString();
                baseHolder.mTxtExpired.Text = item.Summary.PointsExpired.ToString();
                baseHolder.mTxtTotal.Text = (item.Summary.OpenBalancePoints - item.Summary.PointsRedeem - item.Summary.PointsExpired + item.Summary.PointsEarned).ToString();
            }
            //var gdetail = mLstMemberGroupDetail.Where(p => p.MemberTypeID == item.idMemberType).FirstOrDefault();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.loy_item_members_cardview, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
            //holder.OnClickEStatement
            if (OnClickEStatement != null)
            {
                holder.OnClickEStatement += (b) =>
                {
                    OnClickEStatement(b);
                };
            }
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