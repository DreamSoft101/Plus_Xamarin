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
    public abstract class AGVRecyclerViewAdapter<T> : RecyclerView.Adapter where T : RecyclerView.ViewHolder
    {
        public abstract AsymmetricItem getItem(int position);
    }
}