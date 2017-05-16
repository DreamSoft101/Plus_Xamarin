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
using Android.Graphics;
using Dex.Com.Expresso.Loyalty.Droid.Utils;

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class AnnouncementAdapter : RecyclerView.Adapter, IFilterable
    {
        public event EventHandler<Announcement> ItemClick;
        private Context mContext;
        private List<Announcement> mLstItem;
        private ItemFilter mFilter;
        public AnnouncementAdapter(Context conext, List<Announcement> lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
            mFilter = new ItemFilter(mLstItem);
            mFilter.OnFiltered += (List<Announcement> items) =>
            {
                this.mLstItem = items;
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

        public void addItem(Announcement feedBack)
        {
            mLstItem.Add(feedBack);
            this.mFilter.AddItem(feedBack);
            
            this.NotifyItemChanged(mLstItem.Count - 1);
        }

        public void Clear()
        {
            mLstItem.Clear();
        }


        public Announcement GetBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        private class BaseViewHolder : RecyclerView.ViewHolder
        {
            public CardView Root;
            public Context mContext;
            public Announcement mBaseItem;
            public TextView txtTitle;
            public TextView txtTitleSub;
            public TextView txtDescription;
            public TextView txtDate;
            public ImageView mImgShare;
            public RecyclerView.Adapter mAdapter;
            public ImageView mImgPicture;
            public RelativeLayout mRllImage;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                // Locate and cache view references:
                this.Root = itemView.FindViewById<CardView>(Resource.Id.cvRoot);
                this.txtTitle = itemView.FindViewById<TextView>(Resource.Id.txtTitle);
                this.txtDescription = itemView.FindViewById<TextView>(Resource.Id.txtDescription);
                this.txtDate = itemView.FindViewById<TextView>(Resource.Id.txtDate);
                this.mImgShare = itemView.FindViewById<ImageView>(Resource.Id.imgShare);
                this.mImgPicture = ItemView.FindViewById<ImageView>(Resource.Id.imgPicture);
                this.mRllImage = ItemView.FindViewById<RelativeLayout>(Resource.Id.rllImage);
                this.txtTitleSub = itemView.FindViewById<TextView>(Resource.Id.txtTitleSub);
                this.mImgShare.Click += MImgShare_Click;

                itemView.Click += (sender, e) => itemClick(base.Position);

            }

            private void MImgShare_Click(object sender, EventArgs e)
            {
                // throw new NotImplementedException();
                if (mBaseItem != null)
                {
                    //var imageUri = Android.Net.Uri.Parse("file://" + filename);
                    var intent = new Intent();
                   
                   
                    intent.PutExtra(Intent.ExtraSubject, mBaseItem.strTitle);
                    intent.PutExtra(Intent.ExtraTitle, mBaseItem.strTitle);
                    intent.PutExtra(Intent.ExtraText, mBaseItem.strDescription);
                    if (mBaseItem.images.Count > 0)
                    {
                        intent.SetType("image/jpeg");
                        mImgPicture.BuildDrawingCache(true);
                        // throw new NotImplementedException();
                        Bitmap bitmap = mImgPicture.GetDrawingCache(true);
                        string filename = Expresso.Utils.ImageUtils.ExportBitmapAsPNG(bitmap);
                        intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + filename));
                    }
                    else
                    {
                        intent.SetType("text/plain");
                    }
                    mContext.StartActivity(intent);
                }
            }
        }


        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);

            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.mContext = mContext;
            baseHolder.mBaseItem = item;
            baseHolder.mAdapter = this;
            baseHolder.mBaseItem = item;
            baseHolder.txtTitle.Text = item.strTitle;
            baseHolder.txtTitleSub.Text = item.strTitle;
            baseHolder.txtDescription.Text = item.strDescription;
            baseHolder.txtDate.Text = item.dtStart.ToString(mContext.GetString(Resource.String.loy_format_date_time));
            //baseHolder.txtDate.Text = item.dtStart.ToString("dd/MM/yyyy");
            if (item.images.Count > 0)
            {
                Picasso.With(mContext).Load(item.images[0]).Error(Resource.Drawable.img_error).Into(baseHolder.mImgPicture);
                baseHolder.mRllImage.Visibility = ViewStates.Visible;
                baseHolder.txtTitleSub.Visibility = ViewStates.Gone;
            }
            else
            {
               
                //baseHolder.txtTitleSub.Visibility = ViewStates.Visible;
                baseHolder.txtTitleSub.Visibility = ViewStates.Gone;
                baseHolder.mRllImage.Visibility = ViewStates.Gone;
                Picasso.With(mContext).Load(Resource.Drawable.loy_img_food01).Into(baseHolder.mImgPicture);
            }
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_announcement, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
            return holder;
        }
        void OnClick(int position)
        {
            if (ItemClick != null)
            {
                ItemClick(this, GetBaseItem(position));
            }
        }

        private class ItemFilter : Filter
        {
            private List<Announcement> mOriginalData;
            private List<Announcement> mLstItem;

            public delegate void onFiltered(List<Announcement> data);
            public onFiltered OnFiltered;

            public void AddItem(Announcement item)
            {
                mLstItem.Add(item);
            }

            public ItemFilter(List<Announcement> originalData)
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
                            mLstItem = new List<Announcement>();
                        }


                        foreach (var item in mOriginalData)
                        {
                            if (item.strTitle.ToLower().StartsWith(strQuery) || item.strDescription.ToLower().Contains(strQuery))
                            {
                                mLstItem.Add(item);
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
                    List<Announcement> data = results.Values.ToNetObject<List<Announcement>>();
                    if (OnFiltered != null)
                    {
                        OnFiltered(data);
                    }
                }
            }
        }
    }
}