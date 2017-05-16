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
using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using Dex.Com.Expresso.Utils;
using EXPRESSO.Threads;
using Dex.Com.Expresso.Activities;

namespace Dex.Com.Expresso.Adapters.RecyclerViews
{
    class SettingHighwayNameAdapter : RecyclerView.Adapter
    {
        public event EventHandler<SettingHighway> ItemClick;
        private Context mContext;
        private List<SettingHighway> mLstItem;
        public SettingHighwayNameAdapter(Context conext)
        {
            this.mContext = conext;
            mLstItem = (conext as BaseActivity).getMySetting();
            mLstItem = mLstItem.Where(p => p.isEnable == true).ToList();
        }

        public override int ItemCount
        {
            get
            {
                return mLstItem.Count;
            }
        }



        public SettingHighway GetBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        private class BaseViewHolder : RecyclerView.ViewHolder
        {

            //public event EventHandler<TblRSA> ItemClick;
            public CardView Root;
            public TextView txtName;
            public TextView txtDelete;
            public SettingHighway mBaseItem;
            public RecyclerView.Adapter mAdapter;

            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                // Locate and cache view references:
                this.Root = itemView.FindViewById<CardView>(Resource.Id.cvRoot);
                this.txtDelete = itemView.FindViewById<TextView>(Resource.Id.txtDelete);
                this.txtName = itemView.FindViewById<TextView>(Resource.Id.txtName);
             
                itemView.Click += (sender, e) => itemClick(base.Position);

            }
        }


        void OnClick(int position)
        {
            if (ItemClick != null)
            {
                ItemClick(this, GetBaseItem(position));
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);
       
            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.mBaseItem = item;
            baseHolder.mAdapter = this;

            string name = item.strName;
            name = name == null ? "" : name;
            if (name.Contains("("))
            {
                name = name.Substring(name.IndexOf("(")+1, name.IndexOf(")") - name.IndexOf("(") - 1);
            }
            else
            {
                string[] split = name.Split(' ');
                
                if (split.Length >= 2)
                {
                    name = "";
                    foreach (var s in split)
                    {
                        name += s.ToUpper()[0];
                    }
                }
            }

            baseHolder.txtName.Text = name;
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.exp_item_highway_spn_filter, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
            return holder;
        }

    }
}