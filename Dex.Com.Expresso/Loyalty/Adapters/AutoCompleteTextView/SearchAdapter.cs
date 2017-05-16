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
using Loyalty.Models;
using Loyalty.Models.Database;
using Android.Locations;
using Dex.Com.Expresso.Loyalty.Droid.Utils;
using Android.Gms.Maps.Model;
using System.Collections;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.AutoCompleteTextView
{
    public class SearchAdapter : MyBaseAdapter, IFilterable
    {
        public delegate void onItemClick(BaseItem item);
        public onItemClick OnItemClick;
        private List<BaseItem> mLstItem;
        private List<Merchant> mLstMerchant;
        private List<MerchantProduct> mLstProduct;
        private List<MerchantLocation> mLstLocation;
        private List<MerchantProductMemberType> mLstProductMemberType;
        private Location mLocation;
        private LatLng mMyLatLng;
        private ItemFilter mFilter;
        public SearchAdapter(Context conext, List<MerchantProduct> lstProduct, List<Merchant> lstMerchants, List<MerchantLocation> lstLocation, List<MerchantProductMemberType> lstMemberTypeOffer)
        {
            this.mContext = conext;
            this.mLstMerchant = lstMerchants;
            this.mLstLocation = lstLocation;
            this.mLstProductMemberType = lstMemberTypeOffer;
            this.mLstProduct = lstProduct;

            mLstItem = new List<BaseItem>();
            foreach (var item in lstMerchants)
            {
                BaseItem bitem = new BaseItem();
                bitem.Item = item;
                mLstItem.Add(bitem);
            }
            foreach (var item in mLstLocation)
            {
                BaseItem bitem = new BaseItem();
                bitem.Item = item;
                mLstItem.Add(bitem);
            }
            foreach (var item in mLstProduct)
            {
                BaseItem bitem = new BaseItem();
                bitem.Item = item;
                mLstItem.Add(bitem);
            }

            mLocation = GPSUtils.getLastBestLocation(mContext);
            if (mLocation != null)
            {
                mMyLatLng = new LatLng(mLocation.Latitude, mLocation.Longitude);
            }

            mFilter = new ItemFilter(mLstItem,this);
        }

        //public void Filter(BaseItem item)
        //{

        //}

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
                //throw new NotImplementedException();
                return mFilter;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            //throw new NotImplementedException();
            return null;
        }

        public BaseItem GetBaseItem(int position)
        {
            return mLstItem[position];
        }

        public override long GetItemId(int position)
        {
            // throw new NotImplementedException();
            return 0;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public ImageView mImgIcon;
            public TextView mTxtName;
            public TextView mTxtSub;
            public View root;
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder viewHoder = null;
            var item = GetBaseItem(position);
            if (convertView == null)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_search, null);
                viewHoder = new ViewHolder();

                viewHoder.mTxtName = convertView.FindViewById<TextView>(Resource.Id.txtMain);
                viewHoder.mTxtSub = convertView.FindViewById<TextView>(Resource.Id.txtSub);
                viewHoder.mImgIcon = convertView.FindViewById<ImageView>(Resource.Id.ic_type);
                viewHoder.root = convertView.FindViewById<View>(Resource.Id.lnlroot);
                convertView.Tag = viewHoder;
            }
            else
            {
                viewHoder = convertView.Tag as ViewHolder;
            }

            viewHoder.mTxtSub.Visibility = ViewStates.Gone;
            if (item.Item is Merchant)
            {
                Merchant iMerchant = item.Item as Merchant;
                viewHoder.mTxtName.Text = iMerchant.MerchantName;
                viewHoder.mImgIcon.SetImageResource(Resource.Drawable.loy_ic_merchant);
            }
            else if (item.Item is MerchantLocation)
            {
                MerchantLocation iLocation = item.Item as MerchantLocation;
                viewHoder.mTxtName.Text = iLocation.strLocationName;
                viewHoder.mImgIcon.SetImageResource(Resource.Drawable.loy_ic_location);

                if (mMyLatLng != null)
                {
                    try
                    {
                        var distince = GPSUtils.Distance(mMyLatLng, new LatLng(double.Parse(iLocation.strLat), double.Parse(iLocation.strLng)), GPSUtils.DistanceUnit.Kilometers);
                        viewHoder.mTxtSub.Visibility = ViewStates.Visible;
                        viewHoder.mTxtSub.Text = string.Format(mContext.GetString(Resource.String.loy_format_distince), distince);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else if (item.Item is MerchantProduct)
            {
                MerchantProduct iProduct = item.Item as MerchantProduct;
                viewHoder.mTxtName.Text = iProduct.ProductName;
                viewHoder.mImgIcon.SetImageResource(Resource.Drawable.loy_ic_product);

                var offer = mLstProductMemberType.Where(p => p.idMerchantProduct == iProduct.MerchantProductID).OrderByDescending(p => p.decOffer).FirstOrDefault();
                if (offer != null)
                {
                    viewHoder.mTxtSub.Visibility = ViewStates.Visible;
                    viewHoder.mTxtSub.Text = string.Format(mContext.GetString(Resource.String.loy_format_offer), offer.decOffer);
                }
            }
            viewHoder.root.Tag = item.ToJavaObject();
            viewHoder.root.Click += Root_Click;
            return convertView;
        }

        private void Root_Click(object sender, EventArgs e)
        {
            try
            {
                var item = (sender as View).Tag.ToNetObject<BaseItem>();
                //Toast.MakeText(mContext, item.Item.GetType().Name, ToastLength.Short).Show();
                if (OnItemClick != null)
                {
                    OnItemClick(item);
                }
            }
            catch (Exception ex)
            {

            }
            
            
        }

        private class ItemFilter : Filter
        {
            private List<BaseItem> mOriginalData;
            public delegate void onPublicResult(List<BaseItem> lstItems);
            public onPublicResult OnPublicResult;
            private SearchAdapter adapter;
            public ItemFilter(List<BaseItem> data, SearchAdapter ad)
            {
                mOriginalData = data;
                this.adapter = ad;
            }
            protected override FilterResults PerformFiltering(Java.Lang.ICharSequence constraint)
            {
                FilterResults results = new FilterResults();
                if (constraint == null)
                {
                    results.Count = 0;
                    results.Values = null;
                    return results;
                }
                string strQuery = constraint.ToString().ToLower().Trim();
                //FilterResults results = new FilterResults();
                List< BaseItem > list = mOriginalData;
                List<BaseItem> lstResult = new List<BaseItem>();
                int count = list.Count;

                foreach (var item in list)
                {
                    if (item.Item is Merchant)
                    {
                        Merchant iMerchant = item.Item as Merchant;
                        if (iMerchant.MerchantName.ToLower().Contains(strQuery))
                        {
                            lstResult.Add(item);
                        }
                    }
                    else if (item.Item is MerchantLocation)
                    {
                        MerchantLocation iLocation = item.Item as MerchantLocation;
                        if (iLocation.strLocationName.ToLower().Contains(strQuery))
                        {
                            lstResult.Add(item);
                        }
                    }
                    else if (item.Item is MerchantProduct)
                    {
                        MerchantProduct iProduct = item.Item as MerchantProduct;
                        if (iProduct.ProductName.ToLower().Contains(strQuery))
                        {
                            lstResult.Add(item);
                        }
                    }
                }
                results.Values = lstResult.ToJavaObject();
                results.Count = lstResult.Count;
                return results;
            }

            protected override void PublishResults(Java.Lang.ICharSequence constraint, FilterResults results)
            {
                if (results.Values != null)
                {
                    using (var values = results.Values)
                    {
                        System.Object obj = results.Values.ToNetObject<System.Object>();
                        List<BaseItem> lstobj = obj as List<BaseItem>;
                        adapter.mLstItem = lstobj;
                        adapter.NotifyDataSetChanged();
                        if (OnPublicResult != null)
                        {
                            OnPublicResult(lstobj);
                        }
                        //List<BaseItem> data = values.ToArray<Object>().Select(a => a.to).ToArray();
                    }
                    
                }
                
            }
        }
    }
}