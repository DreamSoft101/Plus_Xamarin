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
using Loyalty.Models;
using Dex.Com.Expresso.Loyalty.Droid.Activities;
using Square.Picasso;
using XamarinItemTouchHelper;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.RecyclerViews
{
    public class ItemPreferencesAdapters : RecyclerView.Adapter, IItemTouchHelperAdapter
    {
        public event EventHandler<ItemPreferences> ItemClick;
        private Context mContext;
        private List<ItemPreferences> mLstItems;
        private int intPositionRemoved = -1;
        private ItemPreferences itemRemoved;
        public ItemPreferencesAdapters(Context conext, List<ItemPreferences> lstItem)
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

        public void AddItem(ItemPreferences item)
        {
            mLstItems.Add(item);
            this.NotifyDataSetChanged();
            ((BaseActivity)mContext).setSettingMemberType(mLstItems);
        }

        public void Undo()
        {
            if (intPositionRemoved != -1)
            {
                mLstItems.Insert(intPositionRemoved, itemRemoved);
                intPositionRemoved = -1;
            }
        }

        public void DeleteItem(int position)
        {
            itemRemoved = mLstItems[position];
            intPositionRemoved = position;
            mLstItems.RemoveAt(position);
            this.NotifyDataSetChanged();
            ((BaseActivity)mContext).setSettingMemberType(mLstItems);
        }

        public bool IsExist(ItemPreferences item)
        {
            var items = mLstItems.Where(p => p.mMemberType.MemberTypeID == item.mMemberType.MemberTypeID);
            if (items.Count() > 0)
            {
                if (item.mMemberGroup != null)
                {
                    if (items.Where(p => p.mMemberGroup.MemberGroupID == item.mMemberGroup.MemberGroupID).Count() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (items.Where(p => p.mMemberGroup == null).Count() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            else
            {
                return false;
            }
        }


        public ItemPreferences GetBaseItem(int pos)
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
            public ItemPreferences mBaseItem;
            public TextView txtName;
            public TextView txtBank;
            public ImageView imgLogo;
            public RecyclerView.Adapter mAdapter;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                this.txtName = itemView.FindViewById<TextView>(Resource.Id.txtName);
                this.txtBank = itemView.FindViewById<TextView>(Resource.Id.txtBank);
                this.imgLogo = itemView.FindViewById<ImageView>(Resource.Id.imgLogo);
                //this.mBaseItem - 

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

            baseHolder.txtName.Text = item.mMemberType.MemberType1;
           

            if (item.mMemberGroup != null)
            {
                baseHolder.txtBank.Text = item.mMemberGroup.MemberGroup1;
                if (item.mDocument != null)
                {
                    Picasso.With(this.mContext).Load("file://" + item.mDocument.FileName).Into(baseHolder.imgLogo);
                }
                else
                {
                    Picasso.With(this.mContext).Load(Resource.Drawable.loy_ic_bank).Into(baseHolder.imgLogo);
                }
            }
            else
            {
                baseHolder.txtBank.Text = "";
                Picasso.With(this.mContext).Load(Resource.Drawable.loy_ic_bank).Into(baseHolder.imgLogo);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.loy_item_preferences, parent, false);
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
            DeleteItem(position);
        }
    }


}