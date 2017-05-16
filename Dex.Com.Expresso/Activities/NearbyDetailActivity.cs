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
using Newtonsoft.Json;
using Square.Picasso;
using Dex.Com.Expresso.Fragments;
using Android.Gms.Maps.Model;
using EXPRESSO.Threads;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "NearbyDetailActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class NearbyDetailActivity : BaseActivity
    {
        public static string ModelData = "ModelData";
        public static string ModelType = "ModelType";
        public static string MODELISFAVORITE = "ISFAVORITE";
        private TextView mTxtName;
        private TextView mTxtType;
        private TextView mTxtAddress;
        private ImageView mImgLogo;
        private ImageView mImgPhone;
        private ImageView mImgDirection;
        private ImageView mImgFavorite;
        private LatLng mItemLatLng;
        private TblNearby mTblNearby;
        private bool mIsFavorite;
        private Page mPage = Page.Map;


        public enum Page
        {
            Map = 0,
            Feedback = 1
        }
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.activity_nearby_detail;
            }
        }

        private void setupWindowAnimations()
        {
            this.OverridePendingTransition(Resource.Animation.zoom_enter, Resource.Animation.zoom_exit);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            setupWindowAnimations();
            this.Title = GetString(Resource.String.title_facility_detail);
            string json = this.Intent.GetStringExtra(ModelData);
            TblNearby item = JsonConvert.DeserializeObject<TblNearby>(json);
            mTblNearby = item;
            string typeName = this.Intent.GetStringExtra(ModelType);
            // Create your application here

           

            mTxtName = FindViewById<TextView>(Resource.Id.txtName);
            mTxtType = FindViewById<TextView>(Resource.Id.txtType);
            mTxtAddress = FindViewById<TextView>(Resource.Id.txtAddress);

            mImgLogo = FindViewById<ImageView>(Resource.Id.imgIcon);
            mImgPhone = FindViewById<ImageView>(Resource.Id.imgIcon);
            mImgDirection = FindViewById<ImageView>(Resource.Id.imgDirection);
            mImgFavorite = FindViewById<ImageView>(Resource.Id.imgFavorite);
            mIsFavorite = this.Intent.GetBooleanExtra(MODELISFAVORITE, false);
            if (mIsFavorite)
            {
                mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite);
            }
            else
            {
                mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
            }
            mImgFavorite.Click += MImgFavorite_Click;
            mTxtName.Text = item.strTitle;
            mTxtAddress.Text = item.strAddress;

            if (string.IsNullOrEmpty(typeName))
            {
                PointOfInterestThread thread = new PointOfInterestThread();
                thread.OnLoadNearbyCategory += (List<TblNearbyCatg>  result) =>
                {
                    var itemx = result.Where(p => p.idNearbyCatg == mTblNearby.idNearbyCatg).FirstOrDefault();
                    if (itemx != null)
                    {
                        mTxtType.Text = itemx.strNearbyCatgName;
                    }
                };
                thread.loadHomePointOfInterest();
            }
            else
            {
                mTxtType.Text = typeName;
            }
            
            Picasso.With(this).Load(item.strLocationImg).Error(Resource.Drawable.img_error).Into(mImgLogo);

            mItemLatLng = new LatLng((double)item.floLatitude, (double)item.floLongitude);
            Android.Support.V4.App.FragmentTransaction fragmentTransaction = SupportFragmentManager.BeginTransaction();
            fragmentTransaction.Replace(Resource.Id.frgContent, Fragment_Maps.NewInstance(mItemLatLng, item.strTitle));
            fragmentTransaction.Commit();

            FindViewById<View>(Resource.Id.imgPhone).Click += NearbyDetailActivity_Click_Phone; 
            FindViewById<View>(Resource.Id.imgDirection).Click += NearbyDetailActivity_Click_Direction;

            FindViewById<View>(Resource.Id.imgMap).Click += NearbyDetailActivity_Click;
            FindViewById<View>(Resource.Id.imgFeedback).Click += NearbyDetailActivity_Click1;
        }

        private void NearbyDetailActivity_Click1(object sender, EventArgs e)
        {
            changeToFeedback();
        }

        private void NearbyDetailActivity_Click(object sender, EventArgs e)
        {
            changeToMaps();
        }

        private void changeToMaps()
        {
            if (mPage == Page.Map)
                return;
            mPage = Page.Map;
            Android.Support.V4.App.FragmentTransaction fragmentTransaction = SupportFragmentManager.BeginTransaction();
            fragmentTransaction.Replace(Resource.Id.frgContent, Fragment_Maps.NewInstance(mItemLatLng, mTblNearby.strTitle));
            fragmentTransaction.Commit();
        }

        private void changeToFeedback()
        {
            if (mPage == Page.Feedback)
                return;
            mPage = Page.Feedback;
            Android.Support.V4.App.FragmentTransaction fragmentTransaction = SupportFragmentManager.BeginTransaction();
            fragmentTransaction.Replace(Resource.Id.frgContent, Fragment_Feedback.NewInstance(new EXPRESSO.Models.BaseItem() { Item = mTblNearby }));
            fragmentTransaction.Commit();
        }

        private void MImgFavorite_Click(object sender, EventArgs e)
        {
            mIsFavorite = !mIsFavorite;
            if (mIsFavorite)
            {
                mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite);
            }
            else
            {
                mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
            }
            FavoriteThread thread = new FavoriteThread();
            thread.IsToggle(mTblNearby);
        }

        private void NearbyDetailActivity_Click_Phone(object sender, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse("tel:" + mTblNearby.strContactNo);
            var intent = new Intent(Intent.ActionDial, uri);
            StartActivity(intent);
        }

        private void NearbyDetailActivity_Click_Direction(object sender, EventArgs e)
        {
            string uri = "";
            uri = String.Format("http://maps.google.com/maps?daddr={0},{1}", mItemLatLng.Latitude, mItemLatLng.Longitude);
            Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri));
            StartActivity(intent);
        }

        public override void OnBackPressed()
        {
            Intent intent = new Intent();
            intent.PutExtra(MODELISFAVORITE, mIsFavorite);
            SetResult(Result.Ok, intent);
            base.OnBackPressed();
        }

    }
}