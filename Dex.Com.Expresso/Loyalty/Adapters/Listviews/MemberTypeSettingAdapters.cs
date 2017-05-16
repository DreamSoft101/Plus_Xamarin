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
using Java.Lang;
using Loyalty.Models.Database;
using Loyalty.Models;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.Listviews
{
    public class MemberTypeSettingAdapters : MyBaseAdapter
    {
        private List<MemberType> mLstItems;
        private List<SettingMemberType> mLstSettings;


        public MemberTypeSettingAdapters(Context context, List<MemberType> lstItems)
        {
            this.mContext = context;
            mLstItems = lstItems;
            mLstSettings = this.Activity.getSettingMemberType();
        }

        public override int Count
        {
            get
            {
                return mLstItems.Count();
            }
        }

        public MemberType GetBaseItem(int position)
        {
            return mLstItems[position];
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            //return mLstItems[position].MemberTypeID
            return 0;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public Switch mCkeMemberType;
            public TextView mTxtName;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder viewHoder = null;
            var item = GetBaseItem(position);
            if (convertView == null)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_membertype, null);
                viewHoder = new ViewHolder();
                
                viewHoder.mCkeMemberType = convertView.FindViewById<Switch>(Resource.Id.ckeMemberType);
                //viewHoder.mCkeMemberType.CheckedChange += MCkeMemberType_CheckedChange;
                viewHoder.mCkeMemberType.Click += MCkeMemberType_Click;
                viewHoder.mTxtName = convertView.FindViewById<TextView>(Resource.Id.txtName);
                convertView.Tag = viewHoder;
            }
            else
            {
                viewHoder = convertView.Tag as ViewHolder;
            }

            viewHoder.mTxtName.Text = item.MemberType1; 
            var setting = mLstSettings.Where(p => p.IDMemberType == item.MemberTypeID).FirstOrDefault();
            if (setting == null)
            {
                viewHoder.mCkeMemberType.Checked = false;
            }
            else
            {
                viewHoder.mCkeMemberType.Checked = setting.isEnable;
            }

            viewHoder.mCkeMemberType.Tag = item.MemberTypeID.ToString();
            return convertView;
        }

        private void MCkeMemberType_Click(object sender, EventArgs e)
        {
            Switch sw = sender as Switch;
            var guid = new Guid((sender as Switch).Tag.ToString());
            var membertype = mLstItems.Where(p => p.MemberTypeID == guid).FirstOrDefault();

            var item = mLstSettings.Where(p => p.IDMemberType == membertype.MemberTypeID).FirstOrDefault();
            if (item == null)
            {
                item = new SettingMemberType();
                item.IDMemberType = membertype.MemberTypeID;
                item.isEnable = sw.Checked;

                mLstSettings.Add(item);
            }
            else
            {
                item.isEnable = sw.Checked;
            }
            this.Activity.setSettingMemberType(mLstSettings);
        }

        private void MCkeMemberType_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            var guid = new Guid((sender as Switch).Tag.ToString());
            var membertype = mLstItems.Where(p => p.MemberTypeID == guid).FirstOrDefault();

            var item = mLstSettings.Where(p => p.IDMemberType == membertype.MemberTypeID).FirstOrDefault();
            if (item == null)
            {
                item = new SettingMemberType();
                item.IDMemberType = membertype.MemberTypeID;
                if (e.IsChecked)
                {
                    item.isEnable = true;
                }
                else
                {
                    item.isEnable = false;
                }
                
                mLstSettings.Add(item);
            }
            else
            {
                if (e.IsChecked)
                {
                    item.isEnable = true;
                }
                else
                {
                    item.isEnable = false;
                }
            }

            this.Activity.setSettingMemberType(mLstSettings);
        }

      
    }
}