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
using Android.Support.V4.View;
using Newtonsoft.Json;
using Dex.Com.Expresso.Fragments;
using Android.Graphics;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "ImageViewerActivity")]
    public class ImageViewerActivity : BaseActivity
    {
        public static string DATA = "DATA";
        public static string DATA_TITLE = "TITLE";
        public static string DATA_DESCRIPTION = "DESCRIPTION";
        public static string DATA_POSITION = "POSITION";

        private List<string> mURLs;
        private ImageView mImgShare;
        private Dex.Com.Expresso.Adapters.Viewpager.ImagesAdapter mImageAdapter;
        private ViewPager mPager;
        private ImageView mImgLeft, mImgRight;

        private string mStrTite;
        private string mStrDescription;
        private int mIntPosition;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.exp_activity_imageviewer;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var data = this.Intent.GetStringExtra(DATA);
            mURLs = JsonConvert.DeserializeObject<List<string>>(data);
            mStrTite = this.Intent.GetStringExtra(DATA_TITLE);
            mStrDescription = this.Intent.GetStringExtra(DATA_DESCRIPTION);
            mIntPosition = this.Intent.GetIntExtra(DATA_POSITION, 0);

            mImgLeft = this.FindViewById<ImageView>(Resource.Id.imgLeft);
            mImgRight = this.FindViewById<ImageView>(Resource.Id.imgRight);
            mImgShare = FindViewById<ImageView>(Resource.Id.imgShare);
            mPager = this.FindViewById<ViewPager>(Resource.Id.pager);
            mImageAdapter = new Dex.Com.Expresso.Adapters.Viewpager.ImagesAdapter(this, this.SupportFragmentManager, mURLs);
            mPager.Adapter = mImageAdapter;

            mPager.CurrentItem = mIntPosition;

            mImgLeft.Visibility = ViewStates.Gone;
            if (mURLs.Count == 1)
            {
                mImgRight.Visibility = ViewStates.Gone;
            }

            mImgLeft.Click += MImgLeft_Click;
            mImgRight.Click += MImgRight_Click;
            mImgShare.Click += MImgShare_Click;
            mPager.PageSelected += MPager_PageSelected;
            // Create your application here
        }

        private void MPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            //throw new NotImplementedException();
            mImgLeft.Visibility = ViewStates.Visible;
            mImgRight.Visibility = ViewStates.Visible;
            if (e.Position == 0)
            {
                mImgLeft.Visibility = ViewStates.Gone;
            }
            if (e.Position == mURLs.Count - 1)
            {
                mImgRight.Visibility = ViewStates.Gone;
            }

        }


        private void MImgShare_Click(object sender, EventArgs e)
        {
            FragmentImages fragment = (FragmentImages)mImageAdapter.GetItem(mPager.CurrentItem);
            Bitmap bitmap = fragment.GetBitmap();
            Intent intent = new Intent(Intent.ActionSend);
            intent.PutExtra(Intent.ExtraSubject, mStrTite);
            intent.PutExtra(Intent.ExtraText, mStrDescription);
            intent.PutExtra(Intent.ExtraTitle, mStrTite);
            intent.SetType("image/jpeg");
            string filename = Expresso.Utils.ImageUtils.ExportBitmapAsPNG(bitmap);
            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + filename));
            StartActivity(intent);
        }

        private void MImgRight_Click(object sender, EventArgs e)
        {
            mPager.CurrentItem++;
            mImgLeft.Visibility = ViewStates.Visible;
            if (mPager.CurrentItem == mPager.ChildCount)
            {
                mImgRight.Visibility = ViewStates.Gone;
            }
        }

        private void MImgLeft_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            mPager.CurrentItem--;
            mImgRight.Visibility = ViewStates.Visible;
            if (mPager.CurrentItem == 0)
            {
                mImgLeft.Visibility = ViewStates.Gone;
            }
        }
    }
}