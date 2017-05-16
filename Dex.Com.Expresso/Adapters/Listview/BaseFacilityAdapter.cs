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
using Android.Support.V7.Widget;
using EXPRESSO.Models;
using Android.Support.V4.Content;
using Dex.Com.Expresso.Utils;
using EXPRESSO.Threads;
using EXPRESSO.Utils;
using Square.Picasso;

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class BaseFacilityAdapter : RecyclerView.Adapter
    {
        public static string TAG = "BaseFacilityAdapter";
        private Context mContext;
        private List<BaseItem> mLstItem;
        public event EventHandler<BaseItem> ItemClick;
        
        public BaseFacilityAdapter(Context conext, List<BaseItem> lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
        }


        public int Count
        {
            get
            {
                return mLstItem.Count;
            }
        }

        public override int ItemCount
        {
            get
            {
                return Count;
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
            public RecyclerView.Adapter mAdapter;
            public Context mContext;
            public CardView Root;
            public TextView txtName;
            public ImageView imgIcon;
            public TextView txtLocation;
            public LinearLayout lnlShare;
            public LinearLayout lnlFavorite;
            public TextView txtHeaderName;
            public LinearLayout lnlHeader;
            public LinearLayout lnlContent;
            public ImageView imgFavorite;
            public BaseItem mBaseItem;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base (itemView)
            {
                // Locate and cache view references:
                this.Root = itemView.FindViewById<CardView>(Resource.Id.cvRoot);
                this.txtHeaderName = itemView.FindViewById<TextView>(Resource.Id.txtHeaderName);
                this.txtName = itemView.FindViewById<TextView>(Resource.Id.txtName);
                this.txtLocation = itemView.FindViewById<TextView>(Resource.Id.txtLocation);
                this.imgIcon = itemView.FindViewById<ImageView>(Resource.Id.imgIcon);
                this.lnlShare = itemView.FindViewById<LinearLayout>(Resource.Id.lnlShare);
                this.lnlFavorite = itemView.FindViewById<LinearLayout>(Resource.Id.lnlFavorite);
                this.lnlHeader = itemView.FindViewById<LinearLayout>(Resource.Id.lnlHeader);
                this.lnlContent = itemView.FindViewById<LinearLayout>(Resource.Id.lnlContent);
                this.imgFavorite = ItemView.FindViewById<ImageView>(Resource.Id.imgFavorite);
                itemView.Click += (sender, e) => itemClick(base.Position);
                this.imgFavorite.Click += Favorite_Click;
                this.lnlFavorite.Click += Favorite_Click;
                this.lnlShare.Click += LnlShare_Click;
                
            }

            private void LnlShare_Click(object sender, EventArgs e)
            {
                if (mBaseItem.Item is TblPetrolStation)
                {
                    var item = mBaseItem.Item as TblPetrolStation;
                    String uri = "http://maps.google.com/maps?saddr=" + item.decLat + "," + item.decLong;
                    Intent intent = new Intent(Intent.ActionSend,Android.Net.Uri.Parse(uri));
                    intent.SetType("*/*");
                    intent.PutExtra(Intent.ExtraSubject, item.strName);
                    intent.PutExtra(Intent.ExtraText, uri);
                    mContext.StartActivity(intent);
                }
                else if (mBaseItem.Item is TblFacilities)
                {
                    var item = mBaseItem.Item as TblFacilities;
                    String uri = "http://maps.google.com/maps?saddr=" + item.decLat + "," + item.decLong;
                    Intent intent = new Intent(Intent.ActionSend, Android.Net.Uri.Parse(uri));
                    intent.SetType("*/*");
                    intent.PutExtra(Intent.ExtraSubject, item.strName);
                    intent.PutExtra(Intent.ExtraText, uri);
                    mContext.StartActivity(intent);
                }
                else if (mBaseItem.Item is TblCSC)
                {
                    var item = mBaseItem.Item as TblCSC;
                    String uri = "http://maps.google.com/maps?saddr=" + item.decLat + "," + item.decLong;
                    Intent intent = new Intent(Intent.ActionSend, Android.Net.Uri.Parse(uri));
                    intent.SetType("*/*");
                    intent.PutExtra(Intent.ExtraSubject, item.strName);
                    intent.PutExtra(Intent.ExtraText, uri);
                    mContext.StartActivity(intent);
                }
            }

            private void Favorite_Click(object sender, EventArgs e)
            {
                if (mBaseItem.Item is TblPetrolStation)
                {
                    var item = mBaseItem.Item as TblPetrolStation;
                    var baseItem = mBaseItem;
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
                }
                else if (mBaseItem.Item is TblFacilities)
                {
                    var item = mBaseItem.Item as TblFacilities;
                    var baseItem = mBaseItem;
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
                }
                else if (mBaseItem.Item is TblCSC)
                {
                    var item = mBaseItem.Item as TblCSC;
                    var baseItem = mBaseItem;
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
                }
                this.mAdapter.NotifyDataSetChanged();
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);
            item.setTag(BaseItem.TagName.Position, position);
            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.mContext = mContext;
            baseHolder.mAdapter = this;
            baseHolder.lnlHeader.Visibility = ViewStates.Gone;
            baseHolder.lnlContent.Visibility = ViewStates.Gone;
            baseHolder.mBaseItem = item;
            if (item.Item is TblHighway)
            {
                //Header
                baseHolder.Root.SetCardBackgroundColor(mContext.getColor(Resource.Color.primary));
                var itemHighway = item.Item as TblHighway;
                baseHolder.txtHeaderName.Text = itemHighway.strName;
                baseHolder.lnlHeader.Visibility = ViewStates.Visible;
            }
            else if (item.Item is TblPetrolStation)
            {
                baseHolder.Root.SetCardBackgroundColor(mContext.getColor(Resource.Color.White));
                baseHolder.lnlContent.Visibility = ViewStates.Visible;
                var itemPertrol = item.Item as TblPetrolStation;
                baseHolder.txtName.Text = itemPertrol.strName;
                baseHolder.txtLocation.Text = string.Format(mContext.GetString(Resource.String.facilities_location_format), itemPertrol.decLocation);

                string img = (string)item.getTag(BaseItem.TagName.Pertrol_BrandIMG);

                
                if (string.IsNullOrEmpty(img))
                {
                    baseHolder.imgIcon.Visibility = ViewStates.Invisible;
                }
                else
                {
                    baseHolder.imgIcon.Visibility = ViewStates.Visible;
                    Picasso.With(mContext).Load(img).Into(baseHolder.imgIcon);
                }
               
                if ((bool)item.getTag(BaseItem.TagName.IsFavorite))
                {
                    baseHolder.imgFavorite.SetImageResource(Resource.Drawable.ic_favorite_black_48dp);
                }
                else
                {
                    baseHolder.imgFavorite.SetImageResource(Resource.Drawable.ic_favorite_border_black_48dp);
                }
            }
            else if (item.Item is TblCSC)
            {
                baseHolder.Root.SetCardBackgroundColor(mContext.getColor(Resource.Color.White));
                baseHolder.lnlContent.Visibility = ViewStates.Visible;
                var itemCSC = item.Item as TblCSC;
                baseHolder.txtName.Text = itemCSC.strName;
                baseHolder.txtLocation.Text = string.Format(mContext.GetString(Resource.String.facilities_location_format), itemCSC.decLocation); // itemCSC.decLocation + " KM";
                baseHolder.imgIcon.Visibility = ViewStates.Gone;
                if ((bool)item.getTag(BaseItem.TagName.IsFavorite))
                {
                    baseHolder.imgFavorite.SetImageResource(Resource.Drawable.ic_favorite_black_48dp);
                }
                else
                {
                    baseHolder.imgFavorite.SetImageResource(Resource.Drawable.ic_favorite_border_black_48dp);
                }
            } 
            else if (item.Item is TblFacilities)
            {
                baseHolder.Root.SetCardBackgroundColor(mContext.getColor(Resource.Color.White));
                baseHolder.lnlContent.Visibility = ViewStates.Visible;
                var itemTblFacility = item.Item as TblFacilities;
                baseHolder.txtName.Text = itemTblFacility.strName;
                baseHolder.txtLocation.Text = string.Format(mContext.GetString(Resource.String.facilities_location_format), itemTblFacility.decLocation); // itemCSC.decLocation + " KM";
                baseHolder.imgIcon.Visibility = ViewStates.Gone;
              
                if ((bool)item.getTag(BaseItem.TagName.IsFavorite))
                {
                    baseHolder.imgFavorite.SetImageResource(Resource.Drawable.ic_favorite_black_48dp);
                }
                else
                {
                    baseHolder.imgFavorite.SetImageResource(Resource.Drawable.ic_favorite_border_black_48dp);
                }
                
            }
           
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_base, parent, false);
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
    }
}