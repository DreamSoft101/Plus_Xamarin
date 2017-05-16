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
using EXPRESSO.Models;
using Square.Picasso;
using EXPRESSO.Threads;
using Dex.Com.Expresso.Adapters.Listview;
using Newtonsoft.Json;
using Android.Graphics;
using Dex.Com.Expresso.Fragments;
using Android.Gms.Maps;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "TollPlazaActivity")]
    public class TollPlazaActivity : BaseActivity
    {
        public readonly static string DATA = "DATA";
        public readonly static string DATA_CCTV = "CCTV";
        public readonly static string DATA_ISFAVORITE = "ISFAVORITE";

        private TextView mTxtHighway, mTxtExit, mTxtLocation;
        private ImageView mImgFavorite, mImgMap, mImgCCTV, mImgMoney;
        private TblTollPlaza mTollPlaza;
        private List<TollPlazaCCTV> mLstCCTV;
      
        private Adapters.Spinner.TollPlazaAdapter adapterTollPlaza;
        private bool IsFavorite;
        private ImageView mImgShare;

        private int mIntOldPage = - 1;
        private BaseFragment mFragment;


        public void ChangeToDetail(int page)
        {
            if (mIntOldPage == page)
            {
                return;
            }
            mIntOldPage = page;
            Android.Support.V4.App.FragmentTransaction fragmentTransaction = SupportFragmentManager.BeginTransaction();

            switch (mIntOldPage)
            {
                case 0:
                    {
                        mFragment = Fragment_TollPlaza_CCTV.NewInstance(mLstCCTV);
                        fragmentTransaction.Replace(Resource.Id.frgContent, mFragment);
                        fragmentTransaction.Commit();
                        break;
                    }
                case 1:
                    {
                        mFragment = Fragment_Maps.NewInstance(new Android.Gms.Maps.Model.LatLng(mTollPlaza.decLat, mTollPlaza.decLong), mTollPlaza.strName);
                        (mFragment as Fragment_Maps).onGetMap += OnGetMap;
                        fragmentTransaction.Replace(Resource.Id.frgContent, mFragment);
                        fragmentTransaction.Commit();
                        break;
                    }
                case 2:
                    {
                        mFragment = Fragment_TollPlaza_TollFare.NewInstance(mTollPlaza);
                        fragmentTransaction.Replace(Resource.Id.frgContent, mFragment);
                        fragmentTransaction.Commit();
                        break;
                    }
            }
          
            
        }

        public void OnGetMap(GoogleMap map, SupportMapFragment mapFrag)
        {
            map.UiSettings.MyLocationButtonEnabled = true;
            //map.UiSettings.MapToolbarEnabled = false;

            View toolbar = ((View)mapFrag.View.FindViewById(1).Parent).FindViewById(4);
            //getParent()).findViewById(Integer.parseInt("4"));

            //// and next place it, for example, on bottom right (as Google Maps app)
            RelativeLayout.LayoutParams rlp = (RelativeLayout.LayoutParams)toolbar.LayoutParameters;
            //// position on right bottom
            rlp.AddRule(LayoutRules.AlignParentTop, 0);
            rlp.AddRule(LayoutRules.AlignParentBottom, 0);
            //rlp.addRule(RelativeLayout.ALIGN_PARENT_BOTTOM, RelativeLayout.TRUE);
            rlp.SetMargins(30, 30, 30, 30);
        }

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.exp_activity_tollplaza_detail;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            mTollPlaza = JsonConvert.DeserializeObject<TblTollPlaza>(this.Intent.GetStringExtra(DATA));
            mLstCCTV = JsonConvert.DeserializeObject<List<TollPlazaCCTV>>(this.Intent.GetStringExtra(DATA_CCTV));
            mLstCCTV = mLstCCTV == null ? new List<TollPlazaCCTV>() : mLstCCTV;
            if (mLstCCTV.Count == 0)
            {

            }
            IsFavorite = this.Intent.GetBooleanExtra(DATA_ISFAVORITE, false);
            this.Title = mTollPlaza.strName;

           
            mTxtHighway = FindViewById<TextView>(Resource.Id.txtHighway);
            mTxtExit = FindViewById<TextView>(Resource.Id.txtExit);
            mTxtLocation = FindViewById<TextView>(Resource.Id.txtLocation);

            mImgMap = FindViewById<ImageView>(Resource.Id.imgMap);
            mImgCCTV = FindViewById<ImageView>(Resource.Id.imgCCTV);
            mImgMoney = FindViewById<ImageView>(Resource.Id.imgMoney);

            mImgFavorite = FindViewById<ImageView>(Resource.Id.imgFavorite);
            mImgShare = FindViewById<ImageView>(Resource.Id.imgShare);
            mTxtExit.Visibility = mTollPlaza.strExit == null ? ViewStates.Gone : ViewStates.Visible;
            mTxtExit.Text = mTollPlaza.strExit == null ? "" : "EXIT " + mTollPlaza.strExit;
            mTxtLocation.Text = string.Format(GetString(Resource.String.facilities_location_format), mTollPlaza.decLocation); //itemTollPlaza.decLocation + " KM";

            mImgMap.Click += MImgMap_Click;

            mImgCCTV.Click += MImgCCTV_Click;

            mImgMoney.Click += MImgMoney_Click;
            if (IsFavorite)
            {
                mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite);
            }
            else
            {
                mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
            }


            MastersThread threadM = new MastersThread();
            threadM.OnLoadHighway += (TblHighway highway) =>
            {
                this.mTxtHighway.Text = highway.strName;
            };
            threadM.loadHighway(mTollPlaza.idHighway);

           
            mImgFavorite.Click += MImgFavorite_Click;
            mImgShare.Click += MImgShare_Click;

            if (mLstCCTV.Count > 0)
            {
                mImgCCTV.Visibility = ViewStates.Visible;
                ChangeToDetail(0);

            }
            else
            {
                mImgCCTV.Visibility = ViewStates.Gone;
                ChangeToDetail(1);
            }
            //mTxtHighway.Text = 
            // Create your application here
        }

        private void MImgMoney_Click(object sender, EventArgs e)
        {
            ChangeToDetail(2);
        }

        private void MImgCCTV_Click(object sender, EventArgs e)
        {
            ChangeToDetail(0);
        }

        private void MImgMap_Click(object sender, EventArgs e)
        {
            ChangeToDetail(1);
        }

        private void MImgShare_Click(object sender, EventArgs e)
        {
            if (mFragment is Fragment_TollPlaza_CCTV)
            {
                Bitmap bitmap = (mFragment as Fragment_TollPlaza_CCTV).getBitmap();
                string filename1 = Expresso.Utils.ImageUtils.ExportBitmapAsPNG(bitmap);
                var imageUri = Android.Net.Uri.Parse("file://" + filename1);
                var intent = new Intent(Intent.ActionSendMultiple);
                intent.SetType("image/jpeg");
                intent.PutExtra(Intent.ExtraStream, imageUri);
                intent.PutExtra(Intent.ExtraSubject, mTollPlaza.strName);
                intent.PutExtra(Intent.ExtraTitle, mTollPlaza.strName);
                intent.PutExtra(Intent.ExtraText, mTollPlaza.strName);
                StartActivity(intent);
            }
            else if (mFragment is Fragment_Maps)
            {
                string uri = "";
                uri = String.Format("http://maps.google.com/maps?daddr={0},{1}", mTollPlaza.decLat, mTollPlaza.decLong);
                Intent intent = new Intent(Intent.ActionSend);
                intent.SetType("text/plain");
                intent.PutExtra(Intent.ExtraSubject, mTollPlaza.strName);
                intent.PutExtra(Intent.ExtraTitle, mTollPlaza.strName);
                intent.PutExtra(Intent.ExtraText, uri);
                StartActivity(intent);
            }
            else if (mFragment is Fragment_TollPlaza_TollFare)
            {
                (mFragment as Fragment_TollPlaza_TollFare).Share();
            }
            //throw new NotImplementedException();
            if (mLstCCTV.Count > 0)
            {
                //mImgCamera1.BuildDrawingCache(true);
                //mImgCamera2.BuildDrawingCache(true);
                //// throw new NotImplementedException();
                //Bitmap bitmap1 = mImgCamera1.GetDrawingCache(true);
                //Bitmap bitmap2 = mImgCamera2.GetDrawingCache(true);
                //string filename1 = Expresso.Utils.ImageUtils.ExportBitmapAsPNG(bitmap1);
                //string filename2 = Expresso.Utils.ImageUtils.ExportBitmapAsPNG(bitmap2);
             
                //var imageUri = Android.Net.Uri.Parse("file://" + filename1);
                //var intent = new Intent(Intent.ActionSendMultiple);
                //intent.SetType("image/jpeg");
                //intent.PutExtra(Intent.ExtraStream, imageUri);
                //intent.PutExtra(Intent.ExtraSubject, mTollPlaza.strName);
                //intent.PutExtra(Intent.ExtraTitle, mTollPlaza.strName);
                //intent.PutExtra(Intent.ExtraText, mTollPlaza.strName);
                //StartActivity(intent);

            }
            else
            {
                var intent = new Intent(Intent.ActionSendMultiple);
                intent.SetType("text/plain");
                intent.PutExtra(Intent.ExtraSubject, mTollPlaza.strName);
                intent.PutExtra(Intent.ExtraTitle, mTollPlaza.strName);
                intent.PutExtra(Intent.ExtraText, mTollPlaza.strName);
                StartActivity(intent);
            }
            
        }

        private void MImgFavorite_Click(object sender, EventArgs e)
        {
            FavoriteThread thread = new FavoriteThread();
           
            thread.IsToggle(mTollPlaza);
            
            IsFavorite = !IsFavorite;
            if (IsFavorite)
            {
                mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite);
            }
            else
            {
                mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
            }
        }

     


        public override void OnBackPressed()
        {
            Intent intent = new Intent();
            intent.PutExtra(DATA_ISFAVORITE, IsFavorite);
            SetResult(Result.Ok, intent);
            base.OnBackPressed();
        }
    }
}