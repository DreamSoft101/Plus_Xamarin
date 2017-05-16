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
using EXPRESSO.Models.Database;
using Dex.Com.Expresso.Adapters.Galerry;
using Dex.Com.Expresso.Activities;
using Dex.Com.Expresso.Dialogs;
using Newtonsoft.Json;
using EXPRESSO.Utils;

namespace Dex.Com.Expresso.Adapters.RecyclerViews
{
    public class TblPostAdapter : RecyclerView.Adapter
    {
   
        public event EventHandler<TblPost> ItemClick;
        private Context mContext;
        private List<TblPost> mLstItem;
        public TblPostAdapter(Context conext, List<TblPost> lstItem)
        {
            this.mContext = conext;
            mLstItem = lstItem;
        }

        public override int ItemCount
        {
            get
            {
                return mLstItem.Count;
            }
        }

        public void AddItem(TblPost post)
        {
            if (mLstItem.Where(p => p.idOpsComm == post.idOpsComm).Count() > 0)
            {
                return;
            }
            mLstItem.Add(post);
            this.NotifyItemChanged(mLstItem.Count - 1);
        }

        public void Clear( )
        {
            mLstItem.Clear();
            this.NotifyDataSetChanged();
        }


        public TblPost GetBaseItem(int pos)
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
            public TextView txtTitle;
            public TextView txtReporter;
            public TextView txtDate;
            public TextView txtContent;
            public Gallery glrImages;
            public TblPost mBaseItem;
            public TblPostAdapter mAdapter;
            public Context mContext;
            public Operations_MediaAdapter mMediaAdapter;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                // Locate and cache view references:
                this.Root = itemView.FindViewById<CardView>(Resource.Id.cvRoot);
                this.txtTitle = itemView.FindViewById<TextView>(Resource.Id.txtTitle);
                this.txtReporter = itemView.FindViewById<TextView>(Resource.Id.txtReporter);
                this.txtDate = itemView.FindViewById<TextView>(Resource.Id.txtDate);
                this.txtContent = itemView.FindViewById<TextView>(Resource.Id.txtContent);
                this.glrImages = ItemView.FindViewById<Gallery>(Resource.Id.glrMedia);

                this.glrImages.ItemClick += GlrImages_ItemClick;
                itemView.Click += (sender, e) => itemClick(base.Position);

            }

            private void GlrImages_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
            {
                var position = Convert.ToInt16((sender as Gallery).Tag.ToString());
                var item = mAdapter.GetBaseItem(position);
                var media = item.operations_media[e.Position];
                Intent intent = new Intent(mContext, typeof(ImageViewerActivity));
                intent.PutExtra(ImageViewerActivity.DATA, JsonConvert.SerializeObject(item.operations_media.Select(p => Cons.IMG_URL_PLUS + p.media_url).ToList()));
                intent.PutExtra(ImageViewerActivity.DATA_TITLE, item.strTitle);
                intent.PutExtra(ImageViewerActivity.DATA_DESCRIPTION, item.strDescription);
                intent.PutExtra(ImageViewerActivity.DATA_POSITION, e.Position);
                mContext.StartActivity(intent);

                //OnClickImage(e.Position, item.operations_media.Select(p => p.media_url).ToList());


                //var position = Convert.ToInt16((sender as Gallery).Tag.ToString());
                ////throw new NotImplementedException();
                //var item = mAdapter.GetBaseItem(position);
                //var media = item.operations_media[e.Position];

                //FragmentTransaction ft = ( mContext as BaseActivity).FragmentManager.BeginTransaction();
                //Fragment prev = (mContext as BaseActivity).FragmentManager.FindFragmentByTag("dialig");
                //if (prev != null)
                //{
                //    ft.Remove(prev);
                //}
                //ft.AddToBackStack(null);
                //MediaViewDialog newFragment = MediaViewDialog.NewInstance(null, media);

                //newFragment.Show(ft, "dialig");
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
            baseHolder.mContext = mContext;
            baseHolder.mBaseItem = item;
            baseHolder.mAdapter = this;

            baseHolder.txtTitle.Text = item.strTitle;
            baseHolder.txtReporter.Text = item.strCustomerName;
            baseHolder.txtDate.Text = DateTime.ParseExact(item.dtCreateDate, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy HH:mm");
            baseHolder.txtContent.Text = item.strDescription;
            //baseHolder.glrImages.Visibility = ViewStates.Gone;
            //  string name = item.strFavoriteLocationName;
            if (item.operations_media.Count > 0)
            {
                baseHolder.glrImages.Visibility = ViewStates.Visible;
                if (baseHolder.mMediaAdapter == null)
                {
                    baseHolder.mMediaAdapter = new Operations_MediaAdapter(mContext, item.operations_media);
                }
                else
                {
                    baseHolder.mMediaAdapter.SetData(item.operations_media);
                }
             
                baseHolder.glrImages.Adapter = baseHolder.mMediaAdapter;
                baseHolder.glrImages.SetSelection(1);
            }
            else
            {
                baseHolder.glrImages.Visibility = ViewStates.Gone;
                if (baseHolder.mMediaAdapter == null)
                {
                    baseHolder.mMediaAdapter = new Operations_MediaAdapter(mContext, item.operations_media);
                }
                baseHolder.mMediaAdapter.SetData(new List<EXPRESSO.Models.Operations_Media>());
            }
            baseHolder.glrImages.Tag = position + "";

            //baseHolder.txtName.Text = ame;
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.exp_item_plusranger, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
         
            return holder;
        }

    }
}