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
using Loyalty.Models.Database;
using Newtonsoft.Json;
using Square.Picasso;
using System.IO;
using Java.Nio.Channels;
using Java.IO;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Android.Locations;
using Android.Gms.Maps;
using Dex.Com.Expresso.Loyalty.Droid.Utils;
using Android.Gms.Maps.Model;
using static Android.Views.View;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    [Activity(Label = "MerchantOfferDetails")]
    public class MerchantOfferDetailsActivity : BaseActivity, IOnTouchListener
    {
        
        public static string DATA = "data";
        public static string DOCUMENT = "document";
        public static string FAVORITE = "favorite";
        public static string LOCATION = "location";
        public static string LOGOS = "logos";
        public static string OFFER = "offer";

        public static string ID = "ID";
        public static string ISFAVORITE = "ISFAVORITE";

        private MerchantProductMemberType mOffer;
        private MerchantProduct mProduct;
        private Document mDocument;
        private Favorites mFavorite;
        private ImageView mImgFavorite;
        private MerchantLocation location;
        private List<Document> mLstDocumentLogo;
        private Location mCurrentLocation;
        private SupportMapFragment mapFrag;
        private GoogleMap map;

        private ImageView mImgView;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.loy_activity_offerdetail;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            string jsonData = this.Intent.GetStringExtra(DATA);

            mProduct = JsonConvert.DeserializeObject<MerchantProduct>(jsonData);
            FavoriteThreads thread = new FavoriteThreads();
            thread.AddRecent(mProduct.MerchantProductID, Recent.intMerchantProduct);

            this.Title = mProduct.ProductName;
            jsonData = this.Intent.GetStringExtra(DOCUMENT);

            if (!string.IsNullOrEmpty(jsonData))
            {
                mDocument = JsonConvert.DeserializeObject<Document>(jsonData);
            }

            jsonData = this.Intent.GetStringExtra(FAVORITE);

            if (!string.IsNullOrEmpty(jsonData))
            {
                mFavorite = JsonConvert.DeserializeObject<Favorites>(jsonData);
            }

            jsonData = this.Intent.GetStringExtra(LOCATION);
            if (!string.IsNullOrEmpty(jsonData))
            {
                location = JsonConvert.DeserializeObject<MerchantLocation>(jsonData);
            }

            jsonData = this.Intent.GetStringExtra(LOGOS);
            if (!string.IsNullOrEmpty(jsonData))
            {
                mLstDocumentLogo = JsonConvert.DeserializeObject<List<Document>>(jsonData);
            }

            jsonData = this.Intent.GetStringExtra(OFFER);
            if (!string.IsNullOrEmpty(jsonData))
            {
                mOffer = JsonConvert.DeserializeObject<MerchantProductMemberType>(jsonData);
            }
            //MerchantProductMemberType

            FindViewById<TextView>(Resource.Id.txtDescription).Text = mProduct.ProductDesc;
            FindViewById<TextView>(Resource.Id.txtName).Text = mProduct.ProductName;
            FindViewById<RatingBar>(Resource.Id.rtbRating).Rating = (float)mProduct.decRating;
            mImgView = FindViewById<ImageView>(Resource.Id.imgPicture);
            if (mDocument != null)
            {
                Picasso.With(this).Load(mDocument.FileName).Resize(800,0).Into(mImgView);
            }

            if (mFavorite != null)
            {
                FindViewById<ImageView>(Resource.Id.imgFavorite).SetImageResource(Resource.Drawable.loy_ic_favorite);
            }
            else
            {
                FindViewById<ImageView>(Resource.Id.imgFavorite).SetImageResource(Resource.Drawable.loy_ic_favorite_un);
            }


            if (location != null)
            {
                FindViewById<TextView>(Resource.Id.txtAddress).Text = location.strAddress;
            }
            else
            {
                FindViewById<TextView>(Resource.Id.txtAddress).Text = "";
            }

            FindViewById<RatingBar>(Resource.Id.rtbRating).Rating = (float)mProduct.decRating;
            mImgFavorite = FindViewById<ImageView>(Resource.Id.imgFavorite);
            FindViewById<View>(Resource.Id.imgLike).Visibility = ViewStates.Gone;
            FindViewById<View>(Resource.Id.imgShare).Click += MerchantShareClick;
            mImgFavorite.Click += MerchantFavoriteClick;


            mapFrag = (SupportMapFragment) SupportFragmentManager.FindFragmentById(Resource.Id.map);
            if (mapFrag != null)
            {
                map = mapFrag.Map;

                Location myLocation = GPSUtils.getLastBestLocation(this);
                if (myLocation != null)
                {
                    MarkerOptions option = new MarkerOptions();
                    option.SetPosition(new LatLng(myLocation.Latitude, myLocation.Longitude));
                    option.SetTitle(GetString(Resource.String.loy_my_location));
                    option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.loy_my_location));
                    var myMarker = map.AddMarker(option);

                    //CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                    //builder.Target(new LatLng(myLocation.Latitude, myLocation.Longitude));
                    //builder.Zoom(15);
                    //CameraPosition cameraPosition = builder.Build();
                    //CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                    //if (map != null)
                    //{
                    //    map.MoveCamera(cameraUpdate);
                    //}
                }

                if (location != null)
                {
                    MarkerOptions option = new MarkerOptions();
                    option.SetPosition(new LatLng(double.Parse(location.strLat),double.Parse(location.strLng)));
                    option.SetTitle(location.strLocationName);
                    option.SetSnippet(location.strAddress);
                    option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.loy_ic_location));
                    var myMarker = map.AddMarker(option);

                    CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                    builder.Target(new LatLng(double.Parse(location.strLat), double.Parse(location.strLng)));
                    builder.Zoom(15);
                    CameraPosition cameraPosition = builder.Build();
                    CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                    if (map != null)
                    {
                        map.MoveCamera(cameraUpdate);
                    }
                }

                if (myLocation != null && location != null)
                {
                    double distince = GPSUtils.Distance(new LatLng(myLocation.Latitude, myLocation.Longitude), new LatLng(double.Parse(location.strLat), double.Parse(location.strLng)), GPSUtils.DistanceUnit.Kilometers);
                    FindViewById<TextView>(Resource.Id.txtDistince).Text = String.Format(GetString(Resource.String.loy_format_distince), distince);
                }
                else
                {
                    FindViewById<TextView>(Resource.Id.txtDistince).Text = GetString(Resource.String.loy_format_no_distince);
                }
            }

            FindViewById<ImageView>(Resource.Id.imgDirections).Click += MerchantOfferDetailsActivity_Click;
            // Create your application here


            FindViewById<RatingBar>(Resource.Id.rtbRating).SetOnTouchListener(this);
            LinearLayout lstLogo = FindViewById<LinearLayout>(Resource.Id.lnlLogo);
            if (lstLogo != null && mLstDocumentLogo != null)
            {
                List<Document> lstdocument = mLstDocumentLogo;
                foreach (var doc in lstdocument)
                {
                    ImageView img = new ImageView(this);
                    LinearLayout.LayoutParams pa = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    pa.SetMargins(10, 0, 0, 0);
                    img.LayoutParameters = pa;
                    img.RequestLayout();
                    lstLogo.AddView(img);
                    Picasso.With(this).Load("file://" + doc.FileName).Error(Resource.Drawable.img_error).Resize(600,0).Into(img);
                }
                //DocumentAdapters docadapter = new DocumentAdapters(this.mContext, lstdocument);
                //lstLogo.Adapter = docadapter;
            }

            if (mOffer != null)
            {
                FindViewById<TextView>(Resource.Id.txtOffer).Text = string.Format(GetString(Resource.String.loy_format_offer), mOffer.decOffer);
            }

        }

        private void RatingClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
          
        }

        private void MerchantOfferDetailsActivity_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //mess_no_location
            if (location == null)
            {
                Toast.MakeText(this, Resource.String.loy_mess_no_location, ToastLength.Short).Show();
            }
            else
            {
                Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(string.Format("http://maps.google.com/maps?daddr={0},{1}", location.strLat, location.strLng)));
                StartActivity(intent);
            }
        }

        private void MerchantFavoriteClick(object sender, EventArgs e)
        {
            var favorite = mFavorite;
            if (favorite != null)
            {
                FavoriteThreads thread = new FavoriteThreads();
                thread.OnResult += (ServiceResult result) =>
                {
                    Guid id = (Guid)result.Data;
                    if (id != Guid.Empty)
                    {
                        mFavorite = null;
                        mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
                        //mLstFavorites.Remove(favorite);
                    }
                    else
                    {

                    }
                };
                thread.deleteFavorite(favorite.ID);
            }
            else
            {
                FavoriteThreads thread = new FavoriteThreads();
                thread.OnResult += (ServiceResult result) =>
                {
                    Favorites favItem = result.Data as Favorites;
                    if (favItem != null)
                    {
                        mFavorite = favItem;
                        mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite);
                    }
                    else
                    {

                    }
                };
                thread.insertFavorite(Favorites.intMerchantProduct, mProduct.MerchantProductID);
            }
        }

        private void MerchantShareClick(object sender, EventArgs e)
        {
            Intent intent = new Intent(Intent.ActionSend);
            intent.SetType("*/*");
            /*
            var document = mDocument;
            if (document != null)
            {
                string path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
                string dbPath = Path.Combine(path, mProduct.ProductName + ".png");
                try
                {
                    if (System.IO.File.Exists(dbPath))
                    {
                        System.IO.File.Delete(dbPath);
                    }

                    Java.IO.File src = new Java.IO.File(document.FileName);
                    Java.IO.File dst = new Java.IO.File(dbPath);

                    FileChannel inChannel = new FileInputStream(src).Channel;
                    FileChannel outChannel = new FileOutputStream(dst).Channel;
                    try
                    {
                        inChannel.TransferTo(0, inChannel.Size(), outChannel);
                    }
                    finally
                    {
                        if (inChannel != null)
                            inChannel.Close();
                        if (outChannel != null)
                            outChannel.Close();
                    }
                }
                catch (Exception ex)
                {

                }
                intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + dbPath));
            }*/

            try
            {
                intent.SetType("image/jpeg");
                mImgView.BuildDrawingCache(true);
                var bitmap = mImgView.GetDrawingCache(true);
                string path = Dex.Com.Expresso.Utils.ImageUtils.ExportBitmapAsPNG(bitmap);
                intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + path));
            }
            catch (Exception ex)
            {
                intent.SetType("text/plane");
            }
            mImgView.BuildDrawingCache(true);
          
            intent.PutExtra(Intent.ExtraSubject, mProduct.ProductName);
            intent.PutExtra(Intent.ExtraText, mProduct.ProductDesc);
            intent.PutExtra(Intent.ExtraTitle, mProduct.ProductName);
            StartActivity(intent);
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            if (e.Action == MotionEventActions.Up)
            {
                Intent intent = new Intent(this, typeof(MerchantOfferCommentsActivity));
                intent.PutExtra(MerchantOfferCommentsActivity.ProductData, JsonConvert.SerializeObject(mProduct));
                StartActivity(intent);
            }
           
            return true;
        }
    }
}