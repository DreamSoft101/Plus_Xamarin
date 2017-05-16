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
using Loyalty.Models.Database;
using Loyalty.Threads;
using Dex.Com.Expresso.Loyalty.Droid.Activities;
using Newtonsoft.Json;
using XamarinItemTouchHelper;
using Dex.Com.Expresso;
using Square.Picasso;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.RecyclerViews
{
    public class MemberRedeemInfoProductAdapters : RecyclerView.Adapter, IItemTouchHelperAdapter
    {
        public delegate void onStartUnlock(Intent intent);
        public onStartUnlock OnStartUnlock;
        public event EventHandler<MemberRedeemInfoProduct> ItemClick;
        private Context mContext;
        private List<MemberRedeemInfoProduct> mLstItems;
        private int intPositionRemoved = -1;
        public static int REQUEST_UNLOCK = 123;
        public delegate void onCountChange(int totalPoint);
        public onCountChange OnCountChange;
        public MemberRedeemInfoProductAdapters(Context conext, List<MemberRedeemInfoProduct> lstItem)
        {
            this.mContext = conext;
            this.mLstItems = lstItem;
        }

        public bool IsUnLockAll()
        {
            if (mLstItems.Where(p => p.IsLock == true).Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsCanRedeemp()
        {
            if (mLstItems.Select(p => p.intType).Distinct().Count() > 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int ProductType()
        {
            if (mLstItems.Select(p => p.intType).Distinct().Count() > 1)
            {
                return 1;
            }
            else
            {
                return mLstItems.Select(p => p.intType).Distinct().ToList()[0];
            }
            
        }

        public override int ItemCount
        {
            get
            {
                return mLstItems.Count;
            }
        }



        public MemberRedeemInfoProduct GetBaseItem(int pos)
        {
            return mLstItems[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public void UnLock(int id)
        {
            var item = mLstItems.Where(p => p.Id == id).FirstOrDefault();
            item.IsLock = false;
            RedemptionThreads thread = new RedemptionThreads();
            thread.UnLockEVoucher(id);
            this.NotifyDataSetChanged();
        }

        private class BaseViewHolder : RecyclerView.ViewHolder
        {
            public delegate void onStartUnlock(Intent intent);
            public onStartUnlock OnStartUnlock;
            public delegate void onRemove(MemberRedeemInfoProduct item);
            public onRemove OnRemoved;
            public Context mContext;
            public MemberRedeemInfoProduct mBaseItem;
            public TextView txtProductName;
            public TextView txtQuanity;
            public TextView txtPoints;
            public TextView txtTotalPrice;
            public ImageView mImgMore;
            public ImageView mImgSubtract;
            public RecyclerView.Adapter mAdapter;
            public ImageView mImgLock;
            public ImageView imgProduct;
            public delegate void onCountChange();
            public onCountChange OnCountChange;

            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                this.txtProductName = itemView.FindViewById<TextView>(Resource.Id.txtProductName);
                this.txtTotalPrice = itemView.FindViewById<TextView>(Resource.Id.txtTotal);
                this.txtQuanity = itemView.FindViewById<TextView>(Resource.Id.txtQuanity);
                this.txtPoints = itemView.FindViewById<TextView>(Resource.Id.txtPoints);
                this.mImgMore = ItemView.FindViewById<ImageView>(Resource.Id.imgMore);
                this.mImgSubtract = ItemView.FindViewById<ImageView>(Resource.Id.imgSub);
                this.mImgLock = itemView.FindViewById<ImageView>(Resource.Id.imgLock);
                this.mImgMore.Click += MImgMore_Click;
                this.mImgSubtract.Click += MImgSubtract_Click;
                this.imgProduct = itemView.FindViewById<ImageView>(Resource.Id.imgProduct);
                itemView.Click += (sender, e) => itemClick(base.Position);
                this.mImgLock.Click += MImgLock_Click;

            }

            private void MImgLock_Click(object sender, EventArgs e)
            {
                //throw new NotImplementedException();
                Intent intent = new Intent(mContext, typeof(AuthencationEvoucher));
                string jsonData = JsonConvert.SerializeObject(mBaseItem);
                intent.PutExtra(AuthencationEvoucher.JSONDATA, jsonData);
                ((Dex.Com.Expresso.Activities. BaseActivity)mContext).StartActivityForResult(intent, REQUEST_UNLOCK);
                //if (OnStartUnlock != null)
                //{
                //    OnStartUnlock(intent);
                //}
            }

          

            private void MImgSubtract_Click(object sender, EventArgs e)
            {
                //throw new NotImplementedException();
                RedemptionThreads thread = new RedemptionThreads();
                thread.OnResult += (result) =>
                {
                    mBaseItem.Quantity--;
                    if (mBaseItem.Quantity <= 0)
                    {
                        //this.lst
                        if (OnRemoved != null)
                        {
                            OnRemoved(mBaseItem);
                        }
                    }
                    else
                    {
                        this.txtQuanity.Text = mBaseItem.Quantity.ToString();
                        this.txtTotalPrice.Text = string.Format(mContext.GetString(Resource.String.loy_format_point), (mBaseItem.Quantity * mBaseItem.Points));
                    }

                    if (OnCountChange != null)
                    {
                        OnCountChange();
                    }
                    
                };
                thread.RemoveProductFromCart(mBaseItem);
                
            }

            private void MImgMore_Click(object sender, EventArgs e)
            {
                //throw new NotImplementedERedemptionThreads thread = new RedemptionThreads();
                RedemptionThreads thread = new RedemptionThreads();
                thread.OnResult += (result) =>
                {
                    mBaseItem.Quantity++;
                    this.txtQuanity.Text = mBaseItem.Quantity.ToString();
                    this.txtTotalPrice.Text = string.Format(mContext.GetString(Resource.String.loy_format_point), (mBaseItem.Quantity * mBaseItem.Points));
                    if (OnCountChange != null)
                    {
                        OnCountChange();
                    }
                };
                thread.AddProductToCart(mBaseItem);
                
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);
            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.mContext = mContext;
            baseHolder.mBaseItem = item;
            baseHolder.mAdapter = this;
            baseHolder.txtTotalPrice.Text = string.Format(mContext.GetString(Resource.String.loy_format_point), (item.Quantity * item.Points));
            baseHolder.txtPoints.Text = string.Format(mContext.GetString(Resource.String.loy_format_point), item.Points);
            baseHolder.txtProductName.Text = item.ProductName;
            baseHolder.txtQuanity.Text = item.Quantity.ToString();
            baseHolder.mImgLock.Visibility = item.IsLock ? ViewStates.Visible : ViewStates.Gone;
            if (baseHolder.OnRemoved == null)
            {
                baseHolder.OnRemoved += OnRemove;
            }
            if (baseHolder.OnCountChange == null)
            {
                baseHolder.OnCountChange += () =>
                {
                    if (OnCountChange != null)
                    {
                        OnCountChange(mLstItems.Sum(p => (p.Quantity * p.Points)));
                    }
                };
            }
            if (string.IsNullOrEmpty(item.strURLImage))
            {
                Picasso.With(mContext).Load(Resource.Drawable.loy_img_food01).Into(baseHolder.imgProduct);
            }
            else
            {
                Picasso.With(mContext).Load(item.strURLImage).Into(baseHolder.imgProduct);
            }
            
            //var gdetail = mLstMemberGroupDetail.Where(p => p.MemberTypeID == item.idMemberType).FirstOrDefault();
        }

        public void OnRemove(MemberRedeemInfoProduct item)
        {
            mLstItems.Remove(item);
            this.NotifyDataSetChanged();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.loy_item_cart, parent, false);
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

        public bool OnItemMove(int fromPosition, int toPosition)
        {
            //throw new NotImplementedException();
            return false;
        }

        public void OnItemDismiss(int position)
        {
            //throw new NotImplementedException();
            RedemptionThreads thread = new RedemptionThreads();
            var mBaseItem = mLstItems[position];
            thread.OnResult += (result) =>
            {
                mLstItems.RemoveAt(position);
                this.NotifyDataSetChanged();
                if (OnCountChange != null)
                {
                    OnCountChange(mLstItems.Sum(p => (p.Quantity * p.Points)));
                }
            };
            thread.RemoveProductFromCart(mBaseItem,true);
        }
    }


}