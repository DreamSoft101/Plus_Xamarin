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
using Square.Picasso;

namespace Dex.Com.Expresso.Adapters.RecyclerViews
{
    public class IconFacilitiesAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;
        private Context mContext;
        private List<string> mLstItem;

        public delegate void onErrorImage(string uri, int position);
        public onErrorImage OnErroImage;

        public IconFacilitiesAdapter(Context conext, List<string> urls)
        {
            this.mContext = conext;
            mLstItem = urls;

        }

        public void SetData(List<string> urls)
        {
            this.mLstItem = urls;
            this.NotifyDataSetChanged();
        }

        public override int ItemCount
        {
            get
            {
                return mLstItem.Count;
            }
        }



        public string GetBaseItem(int pos)
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
            public ImageView mImgPicture;
            public string mBaseItem;
            public RecyclerView.Adapter mAdapter;
            public ProgressBar prbLoading;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                // Locate and cache view references:
                this.mImgPicture = itemView.FindViewById<ImageView>(Resource.Id.imgIcon);
                this.prbLoading = itemView.FindViewById<ProgressBar>(Resource.Id.prbLoading);
                itemView.Click += (sender, e) => itemClick(base.Position);


            }
        }


        void OnClick(int position)
        {
            if (ItemClick != null)
            {
                ItemClick(this, position);
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);
            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.mBaseItem = item;
            baseHolder.mAdapter = this;
            string name = item;
            //baseHolder.txtName.Text = name;
            baseHolder.prbLoading.Visibility = ViewStates.Visible;
            Picasso.With(mContext).Load(name).Error(Resource.Drawable.img_error).Into(baseHolder.mImgPicture, new Action(() =>
             {
                 baseHolder.prbLoading.Visibility = ViewStates.Gone;
             }), new Action(() =>
            {
                baseHolder.prbLoading.Visibility = ViewStates.Gone;
                //this.mLstItem.Remove(name);
                //if (OnErroImage != null)
                //{
                //    OnErroImage(name, position);
                //}
            }));
            //{
            //    baseHolder.prbLoading.Visibility = ViewStates.Gone;
            //}), new Action(() =>
            //Picasso.With(mContext).Load(name).Error(Resource.Drawable.img_error).Into(baseHolder.mImgPicture, new Action(() =>
            //{
            //    baseHolder.prbLoading.Visibility = ViewStates.Gone;
            //}), new Action(() =>
            //{
            //    baseHolder.prbLoading.Visibility = ViewStates.Gone;
            //    this.mLstItem.Remove(name);
            //    if (OnErroImage != null)
            //    {
            //        OnErroImage(name, position);
            //    }
            //}));


        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.exp_item_facilities_icon, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
            return holder;
        }

    }
}