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
using EXPRESSO.Models;
using EXPRESSO.Utils;
using Square.Picasso;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace Dex.Com.Expresso.Dialogs
{
    public class MediaViewDialog : DialogFragment
    {
        private Operations_Media media;
        private Context mContext;
        private TextView mTxtComment;

        public delegate void onLoadedImage(Bitmap bitmap);
        public onLoadedImage OnLoadedImage;

        public delegate void onSave(string data);
        public onSave OnSave;

        public delegate void onDelete(string id);
        public onDelete OnDelete;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            mContext = this.Activity;
            return base.OnCreateDialog(savedInstanceState);

        }
        public override void OnStart()
        {
            base.OnStart();
            this.Dialog.Window.SetLayout(WindowManagerLayoutParams.MatchParent, WindowManagerLayoutParams.WrapContent);
        }


        public static MediaViewDialog NewInstance(Bundle bundle, Operations_Media _media)
        {
            MediaViewDialog fragment = new MediaViewDialog() { media = _media };
            fragment.Arguments = bundle;
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.SetTitle(Resource.String.title_choose_entity);
            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.exp_dialog_media_view, container, false);
            mTxtComment = view.FindViewById<TextView>(Resource.Id.txtTitle);

            ImageView img = view.FindViewById<ImageView>(Resource.Id.imgImage);

            if (media.media_url.StartsWith("http"))
            {
                Picasso.With(mContext).Load(media.media_url).Error(Resource.Drawable.img_error).Into(new LoaderTarget(img,  OnLoadedImage));
            }
            else
            {
                Picasso.With(mContext).Load(Cons.IMG_URL_PLUS + media.media_url).Error(Resource.Drawable.img_error).Into(new LoaderTarget(img, OnLoadedImage));

            }

            mTxtComment.Text = media.strComments;
            return view;
        }

        private class LoaderTarget : Java.Lang.Object, ITarget
        {
            private ImageView img;
            private onLoadedImage OnLoadedImage;
            public LoaderTarget(ImageView view, onLoadedImage OnLoadedImage)
            {
                img = view;
                this.OnLoadedImage = OnLoadedImage;
            }


            public void OnBitmapFailed(Drawable p0)
            {
                img.SetImageResource(Resource.Drawable.img_error);
                if (OnLoadedImage != null)
                {
                    OnLoadedImage(null);
                }
            }

            public void OnBitmapLoaded(Bitmap p0, Picasso.LoadedFrom p1)
            {
                img.SetImageBitmap(p0);
                if (OnLoadedImage != null)
                {
                    OnLoadedImage(p0);
                }
            }


            public void OnPrepareLoad(Drawable p0)
            {

            }

        }


    }
}