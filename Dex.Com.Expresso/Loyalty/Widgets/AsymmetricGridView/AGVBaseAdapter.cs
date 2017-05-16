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
    public interface AGVBaseAdapter<T> where T : RecyclerView.ViewHolder
    {
        int getActualItemCount();
        AsymmetricItem getItem(int position);
        void notifyDataSetChanged();
        int getItemViewType(int actualIndex);
        AsymmetricViewHolder<T> onCreateAsymmetricViewHolder(int position, ViewGroup parent, int viewType);
        void onBindAsymmetricViewHolder(AsymmetricViewHolder<T> holder, ViewGroup parent, int position);
    }
}