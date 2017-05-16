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
    public class HighwaySettingAdapter : BaseAdapter
    {
        private Context mContext;
        private List<TblHighway> mLstItem;
        private List<SettingHighway> mLstSettings;
        private List<bool> mLstPosition = new List<bool>();
        public HighwaySettingAdapter(Context conext, List<TblHighway> lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
            mLstSettings = (conext as BaseActivity).getMySetting();
            foreach (var item in lstItem)
            {
                mLstPosition.Add(false);
            }
        }

        public void Toggle(int pos)
        {
            mLstSettings[pos].isEnable = !mLstSettings[pos].isEnable;
            this.NotifyDataSetChanged();
        }


        public List<SettingHighway> getSaveData()
        {
            return mLstSettings;
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
            public CheckBox swtEnable;
            public bool IsAuto = false;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder = null;
            var item = GetHighway(position);
            if (convertView == null)
            {
                holder = new ViewHolder();
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.item_highway_setting, null);
                holder.txtName = convertView.FindViewById<TextView>(Resource.Id.txtName);
                holder.swtEnable = convertView.FindViewById<CheckBox>(Resource.Id.swtEnable);
                holder.swtEnable.CheckedChange += SwtEnable_CheckedChange;
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }
            holder.txtName.Text = item.strName;
            var setting = mLstSettings.Where(p => p.idHighway == item.idHighway).FirstOrDefault();
            holder.swtEnable.Tag = position;
            holder.swtEnable.Checked = setting == null ? false : setting.isEnable;
           
            return convertView;
        }


        private void SwtEnable_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            int pos = (int)(sender as CheckBox).Tag;
            var highway = mLstItem[pos];
            var item = mLstSettings.Where(p => p.idHighway == highway.idHighway).FirstOrDefault();
            
            if (e.IsChecked == (item == null ? false : item.isEnable))
            {
                return;
            }
            if (item == null)
            {
                item = new SettingHighway();
                item.idHighway = highway.idHighway;
                item.isEnable = e.IsChecked;
                item.strName = highway.strName;
                mLstSettings.Add(item);
            }
            else
            {
                item.isEnable = e.IsChecked;
            }

        }
    }
}