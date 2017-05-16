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
using EXPRESSO.Models;
using Square.Picasso;
using EXPRESSO.Models.Database;
using EXPRESSO.Threads;
using Dex.Com.Expresso.Loyalty.Droid.Utils;

namespace Dex.Com.Expresso.Adapters.RecyclerViews
{
    public class FacilitiesAdapter : RecyclerView.Adapter, IFilterable
    {
        public event EventHandler<BaseItem> ItemClick;
        private Context mContext;
        private List<BaseItem> mLstItem;
        private ItemFilter mFilter;
        public delegate void onErrorImage(string uri, int position);
        public onErrorImage OnErroImage;

        public FacilitiesAdapter(Context conext, List<BaseItem> urls)
        {
            this.mContext = conext;
            mLstItem = urls;
            mFilter = new ItemFilter(mLstItem);
            mFilter.OnFiltered += (List<BaseItem> items) =>
            {
                this.mLstItem = items;
                this.NotifyDataSetChanged();
            };
        }
        public Filter Filter
        {
            get
            {
                return mFilter;
            }
        }



        public override int ItemCount
        {
            get
            {
                return mLstItem.Count;
            }
        }



        public BaseItem GetBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        private class BaseViewHolder : RecyclerView.ViewHolder
        {

            //public event EventHandler<TblRSA> ItemClick;
            public Context mContext;
            public LinearLayout mLnlHighway, mLnlContent;
            public TextView mTxtHighwayName, mTxtName;
            public ImageView mImgPicture;
            public RecyclerView mLstIcon;
            public LinearLayoutManager mLnlManager;
            public ImageView mImgFavorite;
            public BaseItem mBaseItem;
            public RecyclerView.Adapter mAdapter;
            public IconFacilitiesAdapter mIconAdapter;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                // Locate and cache view references:
                this.mImgPicture = itemView.FindViewById<ImageView>(Resource.Id.imgIcon);
                this.mLnlHighway = itemView.FindViewById<LinearLayout>(Resource.Id.lnlHighway);
                this.mLnlContent = itemView.FindViewById<LinearLayout>(Resource.Id.lnlContent);
                this.mTxtHighwayName = ItemView.FindViewById<TextView>(Resource.Id.txtHighwayName);
                this.mTxtName = ItemView.FindViewById<TextView>(Resource.Id.txtName);
                this.mImgPicture = ItemView.FindViewById<ImageView>(Resource.Id.imgMain);
                this.mLstIcon = ItemView.FindViewById<RecyclerView>(Resource.Id.lstItems);
                this.mLnlManager = new LinearLayoutManager(mContext, LinearLayoutManager.Horizontal, false);
                this.mLstIcon.SetLayoutManager(this.mLnlManager);
                this.mImgFavorite = ItemView.FindViewById<ImageView>(Resource.Id.imgFavorite);
                this.mImgFavorite.Click += MImgFavorite_Click;
                itemView.Click += (sender, e) => itemClick(base.Position);


            }

            private void MImgFavorite_Click(object sender, EventArgs e)
            {
                //throw new NotImplementedException();
                var row = Convert.ToInt32((sender as ImageView).Tag.ToString());
                if (mBaseItem.Item is FacilityItem)
                {
                    var faciliti = mBaseItem.Item as FacilityItem;
                    if (faciliti.Data is TblRSA)
                    {
                        var rsa = faciliti.Data as TblRSA;
                        FavoriteThread thread = new FavoriteThread();
                        thread.IsToggle(rsa);
                        faciliti.IsFavorite = !faciliti.IsFavorite;
                        mAdapter.NotifyItemChanged(row);
                    }
                    else if (faciliti.Data is TblCSC)
                    {
                        var csc = faciliti.Data as TblCSC;
                        FavoriteThread thread = new FavoriteThread();
                        thread.IsToggle(csc);
                        faciliti.IsFavorite = !faciliti.IsFavorite;
                        mAdapter.NotifyItemChanged(row);
                    }
                    else if (faciliti.Data is TblTollPlaza)
                    {
                        var toll = faciliti.Data as TblTollPlaza;
                        FavoriteThread thread = new FavoriteThread();
                        thread.IsToggle(toll);
                        faciliti.IsFavorite = !faciliti.IsFavorite;
                        mAdapter.NotifyItemChanged(row);
                    }
                    else if (faciliti.Data is TblPetrolStation)
                    {
                        var petrol = faciliti.Data as TblPetrolStation;
                        FavoriteThread thread = new FavoriteThread();
                        thread.IsToggle(petrol);
                        faciliti.IsFavorite = !faciliti.IsFavorite;
                        mAdapter.NotifyItemChanged(row);
                    }
                    else if (faciliti.Data is TblNearby)
                    {
                        var nearby = faciliti.Data as TblNearby;
                        FavoriteThread thread = new FavoriteThread();
                        thread.IsToggle(nearby);
                        faciliti.IsFavorite = !faciliti.IsFavorite;
                        mAdapter.NotifyItemChanged(row);
                    }
                }
            }
        }


        void OnClick(int position)
        {
            if (ItemClick != null)
            {
                ItemClick(this, GetBaseItem(position));
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);
            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.mBaseItem = item;
            baseHolder.mAdapter = this;
            baseHolder.mContext = mContext;
            baseHolder.mImgFavorite.Tag = position;
            if (item.Item is TblHighway)
            {
                baseHolder.mLnlHighway.Visibility = ViewStates.Visible;
                baseHolder.mLnlContent.Visibility = ViewStates.Gone;
                baseHolder.mTxtHighwayName.Text = (item.Item as TblHighway).strName;
            }
            else
            {
                var itemf = item.Item as FacilityItem;
                baseHolder.mLnlHighway.Visibility = ViewStates.Gone;
                baseHolder.mLnlContent.Visibility = ViewStates.Visible;
                baseHolder.mTxtName.Text = itemf.strName;
                if(string.IsNullOrEmpty(itemf.UrlImg))
                {
                    Picasso.With(mContext).Load(Resource.Drawable.img_error).Into(baseHolder.mImgPicture);
                }
                else
                {
                    Picasso.With(mContext).Load(itemf.UrlImg).Error(Resource.Drawable.img_error).Into(baseHolder.mImgPicture);
                }
                
                if (itemf.IsFavorite)
                {
                    baseHolder.mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite);
                }
                else
                {
                    baseHolder.mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
                }

                baseHolder.mLstIcon.Visibility = ViewStates.Gone;
                if (itemf.SubUrlImg != null)
                {
                    if (itemf.SubUrlImg.Count > 0)
                    {
                        if (baseHolder.mIconAdapter == null)
                        {
                            baseHolder.mIconAdapter = new IconFacilitiesAdapter(mContext, itemf.SubUrlImg);
                            baseHolder.mLstIcon.SetAdapter(baseHolder.mIconAdapter);
                            
                        }
                        else
                        {
                            baseHolder.mIconAdapter.SetData(itemf.SubUrlImg);
                        }
                    
                        baseHolder.mLstIcon.Visibility = ViewStates.Visible;
                    }
                }
            }


        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.exp_item_facilities, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
            return holder;
        }

        private class ItemFilter : Filter
        {
            private List<BaseItem> mOriginalData;
            private List<BaseItem> mLstItem;

            public delegate void onFiltered(List<BaseItem> data);
            public onFiltered OnFiltered;

            public ItemFilter(List<BaseItem> originalData)
            {
                this.mOriginalData = originalData;
            }
            protected override FilterResults PerformFiltering(Java.Lang.ICharSequence constraint)
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
                    try
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
                        if (string.IsNullOrEmpty(strQuery))
                        {
                            results.Count = mOriginalData.Count;
                            results.Values = mOriginalData.ToJavaObject();
                            return results;
                        }
                        if (mLstItem != null)
                        {
                            mLstItem.Clear();
                        }
                        else
                        {
                            mLstItem = new List<BaseItem>();
                        }


                        foreach (var item in mOriginalData)
                        {
                            if (item.Item is FacilityItem)
                            {
                                var itemf = item.Item as FacilityItem;
                                if (itemf.strName.ToLower().StartsWith(strQuery) || string.IsNullOrEmpty(strQuery))
                                {
                                    mLstItem.Add(item);
                                }
                            }
                        }

                        results.Count = mLstItem.Count;
                        results.Values = mLstItem.ToJavaObject();
                        return results;
                    }
                    catch (System.Exception ex)
                    {
                        results.Count = mOriginalData.Count;
                        results.Values = mOriginalData.ToJavaObject();
                        return results;
                    }

                }

            }

            protected override void PublishResults(Java.Lang.ICharSequence constraint, FilterResults results)
            {
                if (results.Count > 0)
                {
                    List<BaseItem> data = results.Values.ToNetObject<List<BaseItem>>();
                    if (OnFiltered != null)
                    {
                        OnFiltered(data);
                    }
                }
            }
        }
    }
}