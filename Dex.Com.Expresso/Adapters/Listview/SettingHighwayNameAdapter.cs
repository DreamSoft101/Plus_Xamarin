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
using EXPRESSO.Models;
using Dex.Com.Expresso.Activities;

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class SettingHighwayNameAdapter : BaseAdapter
    {
        private Context mContext;
        private List<SettingHighway> mLstItem;
        public SettingHighwayNameAdapter(Context conext)
        {
            this.mContext = conext;
            mLstItem = (conext as BaseActivity).getMySetting();
        }

       
        public List<SettingHighway> getSaveData()
        {
            return mLstItem;
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

        public SettingHighway GetHighway(int pos)
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
            public TextView txtDelete;
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
                holder.txtDelete = convertView.FindViewById<TextView>(Resource.Id.txtDelete);
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