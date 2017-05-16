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
using Java.Lang;
using Dex.Com.Expresso.Loyalty.Droid.Utils;

namespace Dex.Com.Expresso.Adapters.Spinner
{
    public class TollPlazaAdapter : BaseAdapter, IFilterable
    {
        private Context mContext;
        private List<TblTollPlaza> mLstItem;
        private ItemFilter filter;

        public TollPlazaAdapter(Context conext, List<TblTollPlaza> lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;

            filter = new ItemFilter(mLstItem);
            filter.OnFiltered += (List<TblTollPlaza> data) =>
            {
                mLstItem = data;
                this.NotifyDataSetChanged();
            };

        }

        public override int Count
        {
            get
            {
                return mLstItem.Count;
            }
        }

        public Filter Filter
        {
            get
            {
                return filter;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            //return (mLstItem[position]).Cast<VehicleClasses>();
            return null;
        }

        public TblTollPlaza GetTollPlaza(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.spn_item, null);
            }
            TextView txtText = (TextView)convertView.FindViewById(Resource.Id.txtText);
            TblTollPlaza item = GetTollPlaza(position);
            txtText.Text = item.strName;
            return convertView;
        }


        private class ItemFilter : Filter
        {
            private List<TblTollPlaza> mOriginalData;
            private List<TblTollPlaza> mLstItem;

            public delegate void onFiltered(List<TblTollPlaza> data);
            public onFiltered OnFiltered;

            public ItemFilter(List<TblTollPlaza> originalData)
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
                    string strQuery = constraint.ToString() == null ? "" : constraint.ToString().ToLower();

                    if (mLstItem != null)
                    {
                        mLstItem.Clear();
                    }
                    else
                    {
                        mLstItem = new List<TblTollPlaza>();
                    }


                    foreach (var item in mOriginalData)
                    {
                        if (item.strName.ToLower().StartsWith(strQuery) || string.IsNullOrEmpty(strQuery))
                        {
                            mLstItem.Add(item);
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
                    List<TblTollPlaza> data = results.Values.ToNetObject<List<TblTollPlaza>>();
                    if (OnFiltered != null)
                    {
                        OnFiltered(data);
                    }
                }
            }
        }

    }
}