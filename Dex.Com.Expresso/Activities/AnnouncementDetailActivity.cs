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
using EXPRESSO.Threads;
using EXPRESSO.Models;
using Dex.Com.Expresso.Adapters.Listview;
using Dex.Com.Expresso.Widgets;
using Android.Support.V4.View;
using Android.Graphics;
using System.IO;
using Newtonsoft.Json;
using Android.Support.Design.Widget;
using Dex.Com.Expresso.Utils;
using Square.Picasso;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "AnnouncementDetailActivity")]
    public class AnnouncementDetailActivity : BaseActivity
    {
        public static string ANNOUNCEMENT = "DATA";
        private Announcement mAnnouncement;
        private View mLnlLoading, mLnlData;
        private TextView mTxtTitle, mTxtDescription, mTxtDate;
        private ImageView mImgShare;
        private ViewPager mPager;
        private Bitmap[] mLstBitmaps;
        private ImageView mImgLeft, mImgRight;
        private RelativeLayout mRllImages;
        private Dex.Com.Expresso.Adapters.Viewpager.ImagesAdapter mImageAdapter;
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.activity_announcement_detail;
            }
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = GetString(Resource.String.title_tite_announcement_detail);
            mTxtTitle = this.FindViewById<TextView>(Resource.Id.txtTitle);
            mTxtDescription = this.FindViewById<TextView>(Resource.Id.txtContent);
            mImgShare = this.FindViewById<ImageView>(Resource.Id.imgShare);
            mLnlLoading = this.FindViewById<View>(Resource.Id.lnlLoading);
            mLnlData = this.FindViewById<View>(Resource.Id.srvData);
            mPager = this.FindViewById<ViewPager>(Resource.Id.pager);
            mImgLeft = this.FindViewById<ImageView>(Resource.Id.imgLeft);
            mImgRight = this.FindViewById<ImageView>(Resource.Id.imgRight);
            mTxtDate = this.FindViewById<TextView>(Resource.Id.txtDate);
            this.mRllImages = this.FindViewById<RelativeLayout>(Resource.Id.rllImage);
            var data = this.Intent.GetStringExtra(ANNOUNCEMENT);
            mAnnouncement = JsonConvert.DeserializeObject<Announcement>(data);
            this.Title = mAnnouncement.strTitle;

            this.mTxtDate.Text = mAnnouncement.dtStart.ToString(this.GetString(Resource.String.loy_format_date_time));
            mTxtTitle.Text = mAnnouncement.strTitle;
            mTxtDescription.Text = mAnnouncement.strDescription;
            if (mAnnouncement.images.Count > 0)
            {
                if (mAnnouncement.images.Count == 1)
                {
                    mImgRight.Visibility = ViewStates.Gone;
                    mImgLeft.Visibility = ViewStates.Gone;
                }
                mImageAdapter = new Dex.Com.Expresso.Adapters.Viewpager.ImagesAdapter(this, this.SupportFragmentManager, mAnnouncement.images.ToList());
                mImageAdapter.OnClick += (int postion) =>
                {
                    Intent intent = new Intent(this, typeof(ImageViewerActivity));
                    intent.PutExtra(ImageViewerActivity.DATA, JsonConvert.SerializeObject(mAnnouncement.images));
                    intent.PutExtra(ImageViewerActivity.DATA_DESCRIPTION, mAnnouncement.strDescription);
                    intent.PutExtra(ImageViewerActivity.DATA_TITLE, mAnnouncement.strTitle);
                    intent.PutExtra(ImageViewerActivity.DATA_POSITION, postion);
                    StartActivity(intent);
                };
                mPager.Adapter = mImageAdapter;
                mPager.PageSelected += MPager_PageSelected;
            }
            else
            {
                this.mRllImages.Visibility = ViewStates.Gone;
                //mImgRight.Visibility = ViewStates.Gone;
                //mImgLeft.Visibility = ViewStates.Gone;
                //var adapter = new Dex.Com.Expresso.Adapters.Viewpager.ImagesAdapter(this, this.SupportFragmentManager,new List<string>() { null });
                //mPager.Adapter = adapter;
            }

            mImgLeft.Visibility = ViewStates.Gone;
            mLnlLoading.Visibility = ViewStates.Gone;
            mImgShare.Click += MImgShare_Click;
            mImgLeft.Click += MImgLeft_Click;
            mImgRight.Click += MImgRight_Click;

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
            if (e.Position == mAnnouncement.images.Count - 1)
            {
                mImgRight.Visibility = ViewStates.Gone;
            }
              
        }


        private void MImgRight_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
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

        //private void MImgShareImage_Click(object sender, EventArgs e)
        //{
        //    if (mAnnouncement != null)
        //    {
        //        Intent intent = new Intent(Intent.ActionSend);
        //        intent.SetType("*/*");
        //        //Uri uri = new Uri("http://linkto.com/your_image.png");
        //        if (mLstBitmaps.Length > 0)
        //        {
        //            var position = mPager.CurrentItem;
        //            if (mLstBitmaps[position] == null)
        //            {
        //                Toast.MakeText(this, Resource.String.mess_please_waiting, ToastLength.Short).Show();
        //                return;
        //            }
        //            string[] split = mAnnouncement.lstImages[position].Split('/');
        //            string path = ExportBitmapAsPNG(mLstBitmaps[position], split[split.Length - 1]);
        //            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + path));

        //        }

        //        intent.PutExtra(Intent.ExtraSubject, mAnnouncement.strTitle);
        //        intent.PutExtra(Intent.ExtraText, mAnnouncement.strDescription);
        //        intent.PutExtra(Intent.ExtraTitle, mAnnouncement.strTitle);
        //        //startActivity(intent);
        //        StartActivity(intent);
        //    }
        //}

        private void MImgShare_Click(object sender, EventArgs e)
        {
            if (mAnnouncement != null)
            {
                Intent intent = new Intent(Intent.ActionSend);
                
                //Uri uri = new Uri("http://linkto.com/your_image.png");
                intent.PutExtra(Intent.ExtraSubject, mAnnouncement.strTitle);
                intent.PutExtra(Intent.ExtraText, mAnnouncement.strDescription);
                intent.PutExtra(Intent.ExtraTitle, mAnnouncement.strTitle);
                if (mAnnouncement.images.Count > 0)
                {
                    intent.SetType("image/jpeg");
                    //fra.BuildDrawingCache(true);
                    Bitmap bitmap = mImageAdapter.GetBitmap();
                    string filename = Expresso.Utils.ImageUtils.ExportBitmapAsPNG(bitmap);
                    intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + filename));
                }
                else
                {
                    intent.SetType("text/plain");
                }
                StartActivity(intent);
            }
        }

        public string ExportBitmapAsPNG(Bitmap bitmap, string name)
        {
            var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var filePath = System.IO.Path.Combine(sdCardPath, name);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            var stream = new FileStream(filePath, FileMode.Create);
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            stream.Close();
            return filePath;
        }

        private void MImages_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            mPager.CurrentItem = e.Position;
           
        }

    }
}