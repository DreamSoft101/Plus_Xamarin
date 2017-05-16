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

namespace Dex.Com.Expresso.Loyalty.Droid.Widgets.AsymmetricGridView
{
    public  class AsymmetricRecyclerViewAdapter<T> : RecyclerView.Adapter, AGVBaseAdapter<T> where T : RecyclerView.ViewHolder
    {
        private AsymmetricRecyclerView recyclerView;
        private AGVRecyclerViewAdapter<T> wrappedAdapter;
        private AdapterImpl adapterImpl;

        public override int ItemCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int getActualItemCount()
        {
            throw new NotImplementedException();
        }

        public AsymmetricItem getItem(int position)
        {
            throw new NotImplementedException();
        }

        public void notifyDataSetChanged()
        {
            throw new NotImplementedException();
        }

        public int getItemViewType(int actualIndex)
        {
            throw new NotImplementedException();
        }

        public AsymmetricViewHolder<T> onCreateAsymmetricViewHolder(int position, ViewGroup parent, int viewType)
        {
            throw new NotImplementedException();
        }

        public void onBindAsymmetricViewHolder(AsymmetricViewHolder<T> holder, ViewGroup parent, int position)
        {
            throw new NotImplementedException();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            throw new NotImplementedException();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            throw new NotImplementedException();
        }
    }
}