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
using EXPRESSO.Models.Database;
using Dex.Com.Expresso.Utils;
using EXPRESSO.Threads;
using Dex.Com.Expresso.Loyalty.Droid.Utils;

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class RSAAdapter : RecyclerView.Adapter, IFilterable
    {
        public event EventHandler<BaseItem> ItemClick;
        private Context mContext;
        private List<BaseItem> mLstItem;
        private ItemFilter mFilter;
        public RSAAdapter(Context conext, List<BaseItem> lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
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
            public CardView Root;
            public TextView txtLocationF;
            public BaseItem mBaseItem;
            public RecyclerView.Adapter mAdapter;
            public TextView txtTitle;
            public LinearLayout mLnlHighway, mLnlContent, mLnlFavorite;
            public TextView txtHighway, mTxtFavoriteTitle, mTxtTitle;
            public Context mContext;
            public ImageView mImgFavorite, mImgFavoriteF;
            public ImageView imgFood;
            public ImageView imgCCTV;
            public ImageView imgSign;
            public ImageView imgFoodF;
            public ImageView imgCCTVF;
            public ImageView imgSignF;


            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                // Locate and cache view references:
                this.Root = itemView.FindViewById<CardView>(Resource.Id.cvRoot);
                this.txtHighway = itemView.FindViewById<TextView>(Resource.Id.txtHighway);
                this.mTxtFavoriteTitle = itemView.FindViewById<TextView>(Resource.Id.txtFavoriteTitle);
                this.mTxtTitle = itemView.FindViewById<TextView>(Resource.Id.txtTitle);
                this.txtLocationF = itemView.FindViewById<TextView>(Resource.Id.txtLocationF);
                this.imgCCTV = itemView.FindViewById<ImageView>(Resource.Id.imgCCTV);
                this.imgFood = itemView.FindViewById<ImageView>(Resource.Id.imgFood);
                this.imgSign = itemView.FindViewById<ImageView>(Resource.Id.imgRSASign);
                this.imgCCTVF = itemView.FindViewById<ImageView>(Resource.Id.imgCCTVF);
                this.imgFoodF = itemView.FindViewById<ImageView>(Resource.Id.imgFoodF);
                this.imgSignF = itemView.FindViewById<ImageView>(Resource.Id.imgRSASignF);

                this.mLnlHighway = itemView.FindViewById<LinearLayout>(Resource.Id.lnlHighway);
                this.mLnlContent = itemView.FindViewById<LinearLayout>(Resource.Id.lnlContent);
                this.mImgFavorite = itemView.FindViewById<ImageView>(Resource.Id.imgFavorite);


                this.mLnlFavorite = ItemView.FindViewById<LinearLayout>(Resource.Id.lnlFavorite);
                this.mLnlContent = itemView.FindViewById<LinearLayout>(Resource.Id.lnlContent);
                this.mLnlHighway = itemView.FindViewById<LinearLayout>(Resource.Id.lnlHighway);

                this.mImgFavoriteF = itemView.FindViewById<ImageView>(Resource.Id.imgFavoriteF);
                this.mImgFavorite = itemView.FindViewById<ImageView>(Resource.Id.imgFavorite);

            

                itemView.Click += (sender, e) => itemClick(base.Position);

                this.mImgFavoriteF.Click += Favorite_Click;
                this.mImgFavorite.Click += Favorite_Click;
            }

            private void Favorite_Click(object sender, EventArgs e)
            {
                var item = mBaseItem.Item as TblRSA;
                var baseItem = this.mBaseItem;
                if ((bool)baseItem.getTag(BaseItem.TagName.IsFavorite))
                {
                    baseItem.setTag(BaseItem.TagName.IsFavorite, false);
                }
                else
                {
                    baseItem.setTag(BaseItem.TagName.IsFavorite, true);
                }
                FavoriteThread thread = new FavoriteThread();
                thread.IsToggle(item);
                this.mAdapter.NotifyItemChanged(this.Position);
            }
        }
      

        void OnClick(int position)
        {
            if (ItemClick != null)
            {
                if (GetBaseItem(position).Item is TblHighway)
                {

                }
                else
                {
                    ItemClick(this, GetBaseItem(position));
                }
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);
            item.setTag(BaseItem.TagName.Position, position);
            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.mBaseItem = item;
            baseHolder.mAdapter = this;

            baseHolder.mLnlHighway.Visibility = ViewStates.Gone;
            baseHolder.mLnlContent.Visibility = ViewStates.Gone;
            baseHolder.mLnlFavorite.Visibility = ViewStates.Gone;
            if (item.Item is TblHighway)
            {
                //Header
                var itemHighway = item.Item as TblHighway;
                baseHolder.txtHighway.Text = itemHighway.strName;
                baseHolder.mLnlHighway.Visibility = ViewStates.Visible;
            }
            else if (item.Item is TblRSA)
            {
                var itemRSA = item.Item as TblRSA;

                bool isFavorite =  (bool)item.getTag(BaseItem.TagName.IsFavorite);
                bool IsCCTV = (bool)item.getTag(BaseItem.TagName.RSA_IsCCTV);
                bool IsFood = ((bool)item.getTag(BaseItem.TagName.RSA_IsFood));
                bool IsSign = (bool)item.getTag(BaseItem.TagName.RSA_IsSign);

                if (IsCCTV)
                {
                    baseHolder.imgCCTV.Visibility = ViewStates.Visible;
                    baseHolder.imgCCTVF.Visibility = ViewStates.Visible;
                }
                else
                {
                    baseHolder.imgCCTV.Visibility = ViewStates.Gone;
                    baseHolder.imgCCTVF.Visibility = ViewStates.Gone;
                }

                if (IsFood)
                {
                    baseHolder.imgFood.Visibility = ViewStates.Visible;
                    baseHolder.imgFoodF.Visibility = ViewStates.Visible;
                }
                else
                {
                    baseHolder.imgFood.Visibility = ViewStates.Gone;
                    baseHolder.imgFoodF.Visibility = ViewStates.Gone;
                }


                if (IsSign)
                {
                    baseHolder.imgSignF.Visibility = ViewStates.Visible;
                    baseHolder.imgSign.Visibility = ViewStates.Visible;
                }
                else
                {
                    baseHolder.imgSign.Visibility = ViewStates.Gone;
                    baseHolder.imgSignF.Visibility = ViewStates.Gone;
                }

                if (isFavorite)
                {
                    baseHolder.mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite);
                    baseHolder.mImgFavoriteF.SetImageResource(Resource.Drawable.loy_ic_favorite);
                    baseHolder.mLnlFavorite.Visibility = ViewStates.Visible;
                }
                else
                {
                    baseHolder.mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
                    baseHolder.mImgFavoriteF.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
                    baseHolder.mLnlContent.Visibility = ViewStates.Visible;
                }

                baseHolder.mTxtFavoriteTitle.Text = itemRSA.strName;
                baseHolder.mTxtTitle.Text = itemRSA.strName;
                baseHolder.txtLocationF.Text = string.Format(mContext.GetString(Resource.String.facilities_location_format), itemRSA.decLocation);// itemRSA.decLocation + " KM";
            }
        }
        

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.exp_item_rsa, parent, false);
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
                            if (item.Item is TblRSA)
                            {
                                var itemf = item.Item as TblRSA;
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