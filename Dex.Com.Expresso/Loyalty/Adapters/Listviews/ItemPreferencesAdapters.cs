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
using Loyalty.Models.Database;
using Loyalty.Models;
using Square.Picasso;
using Java.Lang;
using Dex.Com.Expresso.Loyalty.Droid.Utils;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.Listviews
{
    public class ItemPreferencesAdapters : MyBaseAdapter , IFilterable
    {
        private List<ItemPreferences> mLstItems;
        private ItemFilter filter;
        public ItemPreferencesAdapters(Context context, List<ItemPreferences> lstItems)
        {
            this.mContext = context;
            mLstItems = lstItems;
            filter = new ItemFilter(mLstItems);
            filter.OnFiltered += (List<ItemPreferences> data) =>
            {
                mLstItems = data;
                this.NotifyDataSetChanged();
            };

        }

        public void AddItem(ItemPreferences item)
        {
            mLstItems.Add(item);
            this.NotifyDataSetChanged();
            this.Activity.setSettingMemberType(mLstItems);
        }

        public void DeleteItem(int position)
        {
            mLstItems.RemoveAt(position);
            this.NotifyDataSetChanged();
            this.Activity.setSettingMemberType(mLstItems);
        }

        public bool IsExist(ItemPreferences item)
        {
            var items = mLstItems.Where(p => p.mMemberType.MemberTypeID == item.mMemberType.MemberTypeID);
            if (items.Count() > 0)
            {
                if (item.mMemberGroup != null)
                {
                    if (items.Where(p => p.mMemberGroup.MemberGroupID == item.mMemberGroup.MemberGroupID).Count() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (items.Where(p => p.mMemberGroup == null).Count() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                
            }
            else
            {
                return false;
            }
        }

        public override int Count
        {
            get
            {
                return mLstItems.Count();
            }
        }

        public Filter Filter
        {
            get
            {
                return filter;
            }
        }

        public ItemPreferences GetBaseItem(int position)
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
            public View mRoot;
            public TextView mTxtName;
            public TextView mTxtBank;
            public ImageView mImgLogo;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder viewHoder = null;
            var item = GetBaseItem(position);
            if (convertView == null)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_preferences, null);
                viewHoder = new ViewHolder();
                viewHoder.mRoot = convertView;
                viewHoder.mTxtName = convertView.FindViewById<TextView>(Resource.Id.txtName);
                viewHoder.mTxtBank = convertView.FindViewById<TextView>(Resource.Id.txtBank);
                viewHoder.mImgLogo = convertView.FindViewById<ImageView>(Resource.Id.imgLogo);
                convertView.Tag = viewHoder;
            }
            else
            {
                viewHoder = convertView.Tag as ViewHolder;
            }

            viewHoder.mTxtName.Text = item.mMemberType.MemberType1;

            if (item.mMemberGroup != null)
            {
                viewHoder.mTxtBank.Text = item.mMemberGroup.MemberGroup1;
                if (item.mDocument != null)
                {
                    Picasso.With(this.mContext).Load("file://" + item.mDocument.FileName).Into(viewHoder.mImgLogo);
                }
                else
                {
                    Picasso.With(this.mContext).Load(Resource.Drawable.loy_ic_bank).Into(viewHoder.mImgLogo);
                }
            }
            else
            {
                viewHoder.mTxtBank.Text = "";
                Picasso.With(this.mContext).Load(Resource.Drawable.loy_ic_bank).Into(viewHoder.mImgLogo);
            }
            //var group = mLstMemberGroup.Where(p => p.)


            return convertView;
        }


        private class ItemFilter : Filter
        {
            private List<ItemPreferences> mOriginalData;
            private List<ItemPreferences> mLstItem;

            public delegate void onFiltered(List<ItemPreferences> data);
            public onFiltered OnFiltered;

            public ItemFilter(List<ItemPreferences> originalData)
            {
                this.mOriginalData = originalData;
            }
            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                FilterResults results = new FilterResults();
                if (constraint == null)
                {
                    results.Values = null;
                    results.Count = 0;
                    return results;
                }
                else
                {
                    if (constraint.ToString() == null)
                    {
                        if (mLstItem == null)
                        {
                            mLstItem = mOriginalData;
                            results.Count = mLstItem.Count;
                            results.Values = mLstItem.ToJavaObject();
                            return results;
                        }
                        else
                        {
                            results.Count = mLstItem.Count;
                            results.Values = mLstItem.ToJavaObject();
                            return results;
                        }
                    }
                    string strQuery = constraint.ToString().ToLower();
                    if (mLstItem != null)
                    {
                        mLstItem.Clear();
                    }
                    else
                    {
                        mLstItem = new List<ItemPreferences>();
                    }


                    foreach (var item in mOriginalData)
                    {
                        if (item.mMemberType.MemberType1.ToLower().StartsWith(strQuery))
                        {
                            mLstItem.Add(item);
                        }
                        else
                        {
                            if (item.mMemberGroup != null)
                            {
                                if (item.mMemberGroup.MemberGroup1.ToLower().StartsWith(strQuery))
                                {
                                    mLstItem.Add(item);
                                }
                            }
                        }
                    }

                    results.Count = mLstItem.Count;
                    results.Values = mLstItem.ToJavaObject();
                    return results;
                }

            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                if (results.Count > 0)
                {
                    List<ItemPreferences> data = results.Values.ToNetObject<List<ItemPreferences>>();
                    if (OnFiltered != null)
                    {
                        OnFiltered(data);
                    }
                }
            }
        }

    }
}