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
using EXPRESSO.Models.Database;

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class HighwayAdapter : BaseAdapter
    {
        private Context mContext;
        private List<TblHighway> mLstItem;
        public HighwayAdapter(Context conext, List<TblHighway> lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
        }

       
        public override int Count
        {
            get
            {
                return mLstItem.Count;
            }
        }


        public override Java.Lang.Object GetItem(int position)
        {
            //return (mLstItem[position]).Cast<VehicleClasses>();
            return null;
        }

        public TblHighway GetHighway(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public TextView txtName;
            public Switch swtEnable;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder = null;
            var item = GetHighway(position);
            if (convertView == null)
            {
                holder = new ViewHolder();
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.exp_item_highway_spn_filter, null);
                holder.txtName = convertView.FindViewById<TextView>(Resource.Id.txtName);
                holder.swtEnable = convertView.FindViewById<Switch>(Resource.Id.swtEnable);
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }
            holder.txtName.Text = item.strName;
           

            return convertView;
        }


     
    }
}