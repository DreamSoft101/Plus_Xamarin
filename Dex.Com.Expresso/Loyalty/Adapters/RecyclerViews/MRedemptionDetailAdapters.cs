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
using Newtonsoft.Json;
using Dex.Com.Expresso;
using Loyalty.Models.Database;
using Square.Picasso;
using Loyalty.Utils;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.RecyclerViews
{
    public class MRedemptionDetailAdapters : RecyclerView.Adapter
    {

        public event EventHandler<MRedemptionDetail> ItemClick;
        private Context mContext;
        private List<MRedemptionDetail> mLstItems;
        private List<RedemptionProduct> mLstPros;
        private int intPositionRemoved = -1;

        public MRedemptionDetailAdapters(Context conext, List<MRedemptionDetail> lstItem, List<RedemptionProduct> pro)
        {
            this.mContext = conext;
            this.mLstItems = lstItem;
            this.mLstPros = pro;

        }

        public override int ItemCount
        {
            get
            {
                return mLstItems.Count;
            }
        }



        public MRedemptionDetail GetBaseItem(int pos)
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
            public MRedemptionDetail mBaseItem;
            
            public RecyclerView.Adapter mAdapter;

            public LinearLayout mLnlPhysical;
            public TextView mPhysicalName;
            public TextView mPhysicalStatus;
            public TextView mPhysicalPoints;
            public TextView mPhysicalQuanity;
            public TextView mPhysicalTotalPoints;
            public ImageView mImgPhysical;

            public LinearLayout mLnlEVoucher;
            public TextView mEVoucherName;
            public TextView mEVoucherStatus;
            public TextView mEVoucherPoints;
            public TextView mEVoucherCode;
            public ImageView mImgEVoucher;

            public LinearLayout mLnlCashReward;
            public TextView mCashName;
            public TextView mCashCardNumber;
            public TextView mCashCardType;
            public TextView mCashAmount;
            public TextView mCashPoints;

            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                this.mLnlPhysical = itemView.FindViewById<LinearLayout>(Resource.Id.lnlPhysical);
                this.mPhysicalName = itemView.FindViewById<TextView>(Resource.Id.txtPhysicalName);
                this.mPhysicalTotalPoints = itemView.FindViewById<TextView>(Resource.Id.txtPhysicalTotalPoints);
                this.mPhysicalPoints = itemView.FindViewById<TextView>(Resource.Id.txtPhysicalPoints);
                this.mPhysicalQuanity = itemView.FindViewById<TextView>(Resource.Id.txtPhysicalQuanity);
                this.mPhysicalStatus = itemView.FindViewById<TextView>(Resource.Id.txtPhysicalStartus);
                this.mImgPhysical = ItemView.FindViewById<ImageView>(Resource.Id.imgPhysical);

                this.mLnlEVoucher = itemView.FindViewById<LinearLayout>(Resource.Id.lnlEVoucher);
                this.mEVoucherCode = itemView.FindViewById<TextView>(Resource.Id.txtEVoucherCode);
                this.mEVoucherName = itemView.FindViewById<TextView>(Resource.Id.txtVoucherName);
                this.mEVoucherPoints = itemView.FindViewById<TextView>(Resource.Id.txtEVoucherPoints);
                this.mEVoucherStatus = itemView.FindViewById<TextView>(Resource.Id.txtEVoucherStartus);
                this.mImgEVoucher = ItemView.FindViewById<ImageView>(Resource.Id.imgEVoucher);


                this.mLnlCashReward = itemView.FindViewById<LinearLayout>(Resource.Id.lnlCashReward);
                this.mCashAmount = itemView.FindViewById<TextView>(Resource.Id.txtCashAmount);
                this.mCashName = itemView.FindViewById<TextView>(Resource.Id.txtCashName);
                this.mCashPoints = itemView.FindViewById<TextView>(Resource.Id.txtCashPoints);
                this.mCashCardNumber = itemView.FindViewById<TextView>(Resource.Id.txtCashCardNumber);
                this.mCashCardType = itemView.FindViewById<TextView>(Resource.Id.txtCashCartType);
                
                itemView.Click += (sender, e) => itemClick(base.Position);
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);
            BaseViewHolder baseHolder = holder as BaseViewHolder;

            baseHolder.mLnlCashReward.Visibility = ViewStates.Gone;
            baseHolder.mLnlPhysical.Visibility = ViewStates.Gone;
            baseHolder.mLnlEVoucher.Visibility = ViewStates.Gone;


            baseHolder.mContext = mContext;
            baseHolder.mBaseItem = item;
            baseHolder.mAdapter = this;


            var pro = mLstPros.Where(p => p.RedemptionProductID.ToString() == item.RedemptionProductId).FirstOrDefault();


            if (item is MRedemptionDetailPhysic)
            {
                var detail = item as MRedemptionDetailPhysic;
                baseHolder.mLnlPhysical.Visibility = ViewStates.Visible;

                baseHolder.mPhysicalName.Text = detail.ProductName;
                baseHolder.mPhysicalPoints.Text = string.Format(mContext.GetString(Resource.String.loy_format_point), detail.RedeemPoints);
                baseHolder.mPhysicalQuanity.Text = string.Format(mContext.GetString(Resource.String.loy_format_quantity), detail.Quantity);
                baseHolder.mPhysicalTotalPoints.Text = string.Format(mContext.GetString(Resource.String.loy_format_amount), string.Format(mContext.GetString(Resource.String.loy_format_point), detail.RedeemPoints * detail.Quantity));
                baseHolder.mPhysicalStatus.Text = string.Format(mContext.GetString(Resource.String.loy_format_status), detail.Status == 0 ? mContext.GetString(Resource.String.loy_txt_Pending) : detail.Status == 1 ? mContext.GetString(Resource.String.loy_text_inprogress) : detail.Status == 2 ? mContext.GetString(Resource.String.loy_text_cancel) : detail.Status == 3 ? mContext.GetString(Resource.String.loy_text_shipping) : mContext.GetString(Resource.String.loy_text_deliveried));

                if (pro != null)
                {
                    Picasso.With(mContext).Load(Cons.API_IMG_URL + pro.ThumbnailID).Error(Resource.Drawable.loy_img_food01).Into(baseHolder.mImgPhysical);
                }
            }
            else if (item is MRedemptionInstant)
            {
                var detail = item as MRedemptionInstant;
            }
            else if (item is MRedemptionDetailFee)
            {
                var detail = item as MRedemptionDetailFee;
            }
            else if (item is MRedemptionDetailExchange)
            {
                var detail = item as MRedemptionDetailExchange;
            }
            else if (item is MRedemptionDetailCash)
            {
                var detail = item as MRedemptionDetailCash;
                baseHolder.mLnlCashReward.Visibility = ViewStates.Visible;
                baseHolder.mCashAmount.Text = detail.Amount;
                baseHolder.mCashCardNumber.Text = detail.CardNumber;
                baseHolder.mCashCardType.Text = detail.CardType;
                baseHolder.mCashName.Text = detail.ProductName;
                baseHolder.mCashPoints.Text = string.Format(mContext.GetString(Resource.String.loy_format_point), detail.RedeemPoints);
                
            }
            else if (item is MRedemptionDetailVoucher)
            {
                var detail = item as MRedemptionDetailVoucher;

                baseHolder.mLnlEVoucher.Visibility = ViewStates.Visible;
                baseHolder.mEVoucherCode.Text = string.Format(mContext.GetString(Resource.String.loy_format_voucherno), detail.strVoucherNo);
                baseHolder.mEVoucherName.Text = detail.ProductName;
                baseHolder.mEVoucherPoints.Text = string.Format(mContext.GetString(Resource.String.loy_format_amount), string.Format(mContext.GetString(Resource.String.loy_format_point), detail.RedeemPoints));
                //baseHolder.mEVoucherStatus.Text = detail.ToString 
                
                switch (detail.Status)
                {
                    case 1:
                        {
                            baseHolder.mEVoucherStatus.Text = string.Format(mContext.GetString(Resource.String.loy_format_status), mContext.GetString(Resource.String.loy_txt_Active));
                            break;
                        }
                    case 2:
                        {
                            baseHolder.mEVoucherStatus.Text = string.Format(mContext.GetString(Resource.String.loy_format_status), mContext.GetString(Resource.String.loy_txt_Used));
                            break;
                        }
                    case 3:
                        {
                            baseHolder.mEVoucherStatus.Text = string.Format(mContext.GetString(Resource.String.loy_format_status), mContext.GetString(Resource.String.loy_txt_Expired));
                            break;
                        }
                    case 4:
                        {
                            baseHolder.mEVoucherStatus.Text = string.Format(mContext.GetString(Resource.String.loy_format_status), mContext.GetString(Resource.String.loy_txt_Dismissed));
                            break;
                        }
                }

                if (pro != null)
                {
                    Picasso.With(mContext).Load(Cons.API_IMG_URL + pro.ThumbnailID).Error(Resource.Drawable.loy_img_food01).Into(baseHolder.mImgEVoucher);
                }
            }
            //var gdetail = mLstMemberGroupDetail.Where(p => p.MemberTypeID == item.idMemberType).FirstOrDefault();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.loy_item_redemption_productdetail, parent, false);
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