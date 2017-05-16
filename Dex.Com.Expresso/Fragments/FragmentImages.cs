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
using Square.Picasso;
using Android.Graphics;
using Android.Graphics.Drawables;
using Dex.Com.Expresso.Activities;
using Newtonsoft.Json;

namespace Dex.Com.Expresso.Fragments
{
    public class FragmentImages : BaseFragment , ITarget
    {
        private int mIntPosition;
        private int mIntResult = 99;
        private string mImgURL;
        private ImageView mImg;
        private Bitmap mBitmap;
        private List<string> mUrls;
        public delegate void onLoadedImage(Bitmap img);
        public onLoadedImage OnLoadedImage;

        public delegate void onClick(int position);
        public onClick OnClick;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public static FragmentImages NewInstance(string url, int intPosition)
        {
            var frag1 = new FragmentImages { Arguments = new Bundle(), mImgURL = url, mIntPosition = intPosition };
            return frag1;
        }

        public Bitmap GetBitmap()
        {
            return mBitmap;
            //mImg.BuildDrawingCache(true);
            //return mImg.GetDrawingCache(true);

        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.item_image_scale_width, null);
            mImg = view.FindViewById<ImageView>(Resource.Id.imgImage);

            if (string.IsNullOrEmpty(mImgURL))
            {
                view.Visibility = ViewStates.Gone;
                //Picasso.With(this.getActivity()).Load(Resource.Drawable.loy_img_food01).Error(Resource.Drawable.img_error).Into(mImg);
            }
            else
            {
                Picasso.With(this.getActivity()).Load(mImgURL).Error(Resource.Drawable.img_error).Into(this);
            }

            mImg.Click += MImg_Click;
            
            return view;
        }

        private void MImg_Click(object sender, EventArgs e)
        {
           if (OnClick != null)
            {
                OnClick(mIntPosition);
            }
        }

        public void OnBitmapFailed(Drawable p0)
        {
            //throw new NotImplementedException();
        }

        public void OnBitmapLoaded(Bitmap p0, Picasso.LoadedFrom p1)
        {
            //throw new NotImplementedException();
            mBitmap = p0;
            mImg.SetImageBitmap(mBitmap);
            if (OnLoadedImage != null)
            {
                OnLoadedImage(mBitmap);
            }
        }

        public void OnPrepareLoad(Drawable p0)
        {
            //throw new NotImplementedException();
        }
    }
}