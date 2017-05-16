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
using Square.Picasso;
using Android.Graphics;
using Java.Lang;
using Dex.Com.Expresso.Loyalty.Droid.Utils;

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class TollPlazaAdapter : RecyclerView.Adapter , IFilterable
    {
        public event EventHandler<BaseItem> ItemClick;
        private Context mContext;
        private List<BaseItem> mLstItem;
        private ItemFilter mFilter;

        public TollPlazaAdapter(Context conext, List<BaseItem> lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
            this.mFilter = new ItemFilter(lstItem);
            this.mFilter.OnFiltered += (List<BaseItem> items) =>
                {
                    mLstItem = items;
                    this.NotifyDataSetChanged();
                };
        }

        public override int ItemCount
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
                return mFilter;
            }
        }

        public string[] getFullName()
        {
            if (mLstItem != null)
            {
                return mLstItem.Where(p => p.Item is TblTollPlaza).Select(p => p.Item as TblTollPlaza).OrderBy(p => p.strName).Select(p => p.strName).ToArray();
            }
            else
            {
                return new string[0];
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

        public void getNewPosition(BaseItem item)
        {

        }

        private class BaseViewHolder : RecyclerView.ViewHolder
        {
            public TextView txtTitle;
            public LinearLayout mLnlHighway, mLnlContent, mLnlFavorite;
            public BaseItem mBaseItem;
            public TextView txtHighway, mTxtFavoriteTitle, mTxtAway, mTxtTitle, txtLocation, txtLocationF, txtExit, txtExitF;
            public RecyclerView.Adapter mAdapter;
            public LinearLayout mLnlShare, lnlImage;
            public Context mContext;
            public ImageView mImgFavorite, mImgShare, mImgFavoriteF, mImgCamera, mImgCamera1;



            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                // Locate and cache view references:

                this.txtTitle = itemView.FindViewById<TextView>(Resource.Id.txtTitle);
                this.txtHighway = itemView.FindViewById<TextView>(Resource.Id.txtHighway);
                this.mLnlHighway = itemView.FindViewById<LinearLayout>(Resource.Id.lnlHighway);
                this.mLnlContent = itemView.FindViewById<LinearLayout>(Resource.Id.lnlContent);
                this.mLnlShare = ItemView.FindViewById<LinearLayout>(Resource.Id.lnlShare);
                this.mImgFavorite = itemView.FindViewById<ImageView>(Resource.Id.imgFavorite);
                this.mLnlFavorite = ItemView.FindViewById<LinearLayout>(Resource.Id.lnlFavorite);
                this.mTxtFavoriteTitle = ItemView.FindViewById<TextView>(Resource.Id.txtFavoriteTitle);
                this.mImgFavoriteF = itemView.FindViewById<ImageView>(Resource.Id.imgFavoriteF);
                this.mImgShare = itemView.FindViewById<ImageView>(Resource.Id.imgShare);
                this.mTxtAway = itemView.FindViewById<TextView>(Resource.Id.txtAway);
                this.mImgCamera = ItemView.FindViewById<ImageView>(Resource.Id.imgCamera);
                this.mImgCamera1 = ItemView.FindViewById<ImageView>(Resource.Id.imgCameraExit);
                this.lnlImage = itemView.FindViewById<LinearLayout>(Resource.Id.lnlImage);
                this.txtLocation = itemView.FindViewById<TextView>(Resource.Id.txtLocation);
                this.txtExit = itemView.FindViewById<TextView>(Resource.Id.txtExit);
                this.txtLocationF = itemView.FindViewById<TextView>(Resource.Id.txtLocationF);
                this.txtExitF = itemView.FindViewById<TextView>(Resource.Id.txtExitF);

                itemView.Click += (sender, e) =>
                {
                    itemClick(base.Position);

                };
                this.mImgShare.Click += MImgShare_Click;
                this.mImgFavorite.Click += MImgFavorite_Click;
                this.mImgFavoriteF.Click += MImgFavorite_Click;
            }

            private void MImgShare_Click(object sender, EventArgs e)
            {
                if (mLnlFavorite.Visibility == ViewStates.Visible)
                {
                    var mUpdate = mBaseItem.Item as TblTollPlaza;
                    if (mUpdate.getCCTV().Count > 0)
                    {
                        mImgCamera.BuildDrawingCache(true);
                        // throw new NotImplementedException();
                        Bitmap bitmap = mImgCamera.GetDrawingCache(true);
                        string filename = Expresso.Utils.ImageUtils.ExportBitmapAsPNG(bitmap);

                        //var imageUri = Android.Net.Uri.Parse("file://" + filename);
                        var intent = new Intent();
                        intent.SetType("image/jpeg");
                        intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + filename));
                        intent.PutExtra(Intent.ExtraSubject, mUpdate.strName);
                        intent.PutExtra(Intent.ExtraTitle, mUpdate.strName);
                        intent.PutExtra(Intent.ExtraText, mUpdate.strName);
                        mContext.StartActivity(intent);
                    }
                    else
                    {
                        var intent = new Intent();
                        intent.SetType("text/plain");
                        intent.PutExtra(Intent.ExtraSubject, mUpdate.strName);
                        intent.PutExtra(Intent.ExtraTitle, mUpdate.strName);
                        intent.PutExtra(Intent.ExtraText, mUpdate.strName);
                        mContext.StartActivity(intent);
                    }
                   
                }
                else
                {
                   
                }
            }

            private void MImgFavorite_Click(object sender, EventArgs e)
            {
                //throw new NotImplementedException();

                bool isfavorite = (bool)mBaseItem.getTag(BaseItem.TagName.IsFavorite);
                FavoriteThread thread = new FavoriteThread();
                thread.IsToggle(mBaseItem.Item as TblTollPlaza);

                isfavorite = !isfavorite;
                mBaseItem.setTag(BaseItem.TagName.IsFavorite, isfavorite);
                if (isfavorite)
                {
                    this.mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite);
                }
                else
                {
                    this.mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
                }
                this.mAdapter.NotifyDataSetChanged();
            }

            private void Share_Click(object sender, EventArgs e)
            {
                var item = mBaseItem.Item as TrafficUpdate;
                //throw new NotImplementedException();
                Intent sendIntent = new Intent();
                sendIntent.SetAction(Intent.ActionSend);
                //sendIntent.PutExtra(Intent.ExtraTitle, item.strTitle);
                sendIntent.PutExtra(Intent.ExtraText, item.strTitle);
                sendIntent.SetType("text/plain");
                mContext.StartActivity(sendIntent);
            }

            private void ImgFeedback_Click(object sender, EventArgs e)
            {

            }


        }


        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);
            item.setTag(BaseItem.TagName.Position, position);
            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.mContext = mContext;
            baseHolder.mAdapter = this;
            baseHolder.mBaseItem = item;
            baseHolder.mLnlHighway.Visibility = ViewStates.Gone;
            baseHolder.mLnlContent.Visibility = ViewStates.Gone;
            baseHolder.mLnlFavorite.Visibility = ViewStates.Gone;


            if (item.Item is TblHighway)
            {
                //Header
                baseHolder.mLnlHighway.Visibility = ViewStates.Visible;
                var itemHighway = item.Item as TblHighway;
                baseHolder.txtHighway.Text = itemHighway.strName;

            }
            else if (item.Item is TblTollPlaza)
            {
                var itemTollPlaza = item.Item as TblTollPlaza;
                //itemTollPlaza.strExit = "88";
                baseHolder.txtTitle.Text = itemTollPlaza.strName;
                baseHolder.mTxtFavoriteTitle.Text = itemTollPlaza.strName;
                baseHolder.mTxtAway.Text = "";

                baseHolder.txtLocation.Text = string.Format(mContext.GetString(Resource.String.facilities_location_format), itemTollPlaza.decLocation); //itemTollPlaza.decLocation + " KM";
                baseHolder.txtExit.Visibility = !string.IsNullOrEmpty(itemTollPlaza.strExit) ? ViewStates.Visible : ViewStates.Invisible;

                baseHolder.txtLocationF.Text = string.Format(mContext.GetString(Resource.String.facilities_location_format), itemTollPlaza.decLocation); //itemTollPlaza.decLocation + " KM";
                baseHolder.txtExitF.Visibility = !string.IsNullOrEmpty(itemTollPlaza.strExit) ? ViewStates.Visible : ViewStates.Invisible;

                baseHolder.txtExitF.Text = string.IsNullOrEmpty(itemTollPlaza.strExit) ? "" : "EXIT " + itemTollPlaza.strExit;
                baseHolder.txtExit.Text = string.IsNullOrEmpty(itemTollPlaza.strExit) ? "" : "EXIT " + itemTollPlaza.strExit;

                if ((bool)item.getTag(BaseItem.TagName.IsFavorite))
                {
                    baseHolder.mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite);
                    baseHolder.mImgFavoriteF.SetImageResource(Resource.Drawable.loy_ic_favorite);
                    baseHolder.mLnlFavorite.Visibility = ViewStates.Visible;
                    baseHolder.mLnlContent.Visibility = ViewStates.Gone;
                    if (itemTollPlaza.getCCTV().Count > 0)
                    {
                        Picasso.With(mContext).Load(itemTollPlaza.getCCTV()[0].strCCTVImage).Into(baseHolder.mImgCamera);
                        Picasso.With(mContext).Load(itemTollPlaza.getCCTV()[1].strCCTVImage).Into(baseHolder.mImgCamera1);
                    }
                    else
                    {
                        baseHolder.lnlImage.Visibility = ViewStates.Gone;
                        Picasso.With(mContext).Load(Resource.Drawable.loy_img_food01).Into(baseHolder.mImgCamera);
                        Picasso.With(mContext).Load(Resource.Drawable.loy_img_food01).Into(baseHolder.mImgCamera1);
                    }
                }
                else
                {
                    baseHolder.mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
                    baseHolder.mImgFavoriteF.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
                    baseHolder.mLnlFavorite.Visibility = ViewStates.Gone;
                    baseHolder.mLnlContent.Visibility = ViewStates.Visible;
                }

            }
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.exp_item_tollplaza, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
            return holder;
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
                            if (item.Item is TblTollPlaza)
                            {
                                var toll = item.Item as TblTollPlaza;
                                if (toll.strName.ToLower().StartsWith(strQuery) || string.IsNullOrEmpty(strQuery))
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

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
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