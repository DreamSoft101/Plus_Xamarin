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
    public  class AsymmetricViewHolder<T> : RecyclerView.ViewHolder  where T : RecyclerView.ViewHolder
    {
        T wrappedViewHolder;

        public AsymmetricViewHolder(T wrappedViewHolder) : base(wrappedViewHolder.ItemView)
        {
            this.wrappedViewHolder = wrappedViewHolder;
        }

        public AsymmetricViewHolder(View view) : base(view)
        {
            wrappedViewHolder = null;
        }
    }
}