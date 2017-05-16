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
using Dex.Com.Expresso.Activities;

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class PlusRangerCategoryAdapter : BaseAdapter
    {
        private Context mContext;
        private List<TblCategory> mLstItem;
        private List<TblCategory> mLstSettings;
        public PlusRangerCategoryAdapter(Context conext, List<TblCategory> lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
            mLstSettings = ((BaseActivity)mContext).getPlusRangerCategory();
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

        public TblCategory GetBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public ImageView imIcon;
            public TextView mTxtName;
            public CheckBox mCkeChoose;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder = null;
            var item = GetBaseItem(position);
            if (convertView == null)
            {
                holder = new ViewHolder();
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.exp_item_facilities_type, null);
                holder.imIcon = convertView.FindViewById<ImageView>(Resource.Id.imgIcon);
                holder.mCkeChoose = convertView.FindViewById<CheckBox>(Resource.Id.ckeChoose);
                holder.mTxtName = convertView.FindViewById<TextView>(Resource.Id.txtName);
                holder.mCkeChoose.CheckedChange += MCkeChoose_CheckedChange;
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }
            holder.imIcon.Visibility = ViewStates.Gone;

            var setting = mLstSettings.Where(p => p.idCategory == item.idCategory).FirstOrDefault();

            holder.mTxtName.Text = item.strTitle;
            holder.mCkeChoose.Tag = position;
            holder.mCkeChoose.Checked = setting == null ? false : true;
            return convertView;
        }

        private void MCkeChoose_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            int pos = (int)(sender as CheckBox).Tag;
            var chooseItem = mLstItem[pos];
            var item = mLstSettings.Where(p => p.idCategory == chooseItem.idCategory).FirstOrDefault();
            if (e.IsChecked == (item == null ? false : true))
            {
                return;
            }

            if (item == null && e.IsChecked)
            {
                TblCategory type = new TblCategory();
                type.idCategory = chooseItem.idCategory;
                type.strTitle = chooseItem.strTitle;
                mLstSettings.Add(type);
            }
            else
            {
                if (item != null)
                {
                    if (!e.IsChecked)
                    {
                        mLstSettings.Remove(item);
                    }
                }
                else
                {

                }

            }

            (mContext as BaseActivity).savePlusRangerCategory(mLstSettings);

        }
    }
}