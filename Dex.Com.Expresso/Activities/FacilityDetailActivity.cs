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
using Newtonsoft.Json;
using EXPRESSO.Models.Database;
using EXPRESSO.Threads;
using Dex.Com.Expresso.Fragments;
using Android.Locations;
using Android.Gms.Maps.Model;
using Dex.Com.Expresso.Utils;
using Dex.Com.Expresso.Adapters.Listview;
using EXPRESSO.Models;
using Android.Transitions;
using Square.Picasso;
using Android.Util;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "FacilityDetailActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class FacilityDetailActivity : BaseActivity, ILocationListener
    {
        private BaseFragment mCurrentFragment;
        private LocationManager mLocationManager;
        private string mLocationProvider;
        public static string MODELPOSITION = "MODELPOSITION";
        public static string MODELTYPE = "ModelType";
        public static string MODELData = "ModelData";
        public static string MODELISFAVORITE = "ISFAVORITE";
        public static string MODELSOURCE = "ModelSource";
        public static string MODEL_RSA = "RSA";
        public static string MODEL_LAYBY = "LAYBY";
        public static string MODEL_INTERCHANGE = "INTERCHANGE";
        public static string MODEL_TUNNEL = "TUNNEL";
        public static string MODEL_VISTAPOINT = "VISTAPOINT";
        public static string MODEL_TOLLPLAZA = "TOLLPLAZA";
        public static string MODEL_PETROLSTATION = "PETROLSTATION";
        public static string MODEL_CSC = "CSC";
        public static string MODEL_SSK = "SSK";
        public static string MODEL_PLUS_SMILE = "PLUS_SMILE";

        public static string MODEL_HASCCTV = "HASCCTV";
        public static string MODEL_HASFOOD = "HASFOOD";

        private bool mIsCCTV = false;
        private bool mIsFOOD = false;

        private TblRSA mTblRSA;
        private ModelType mType;
        private string mStrJsonData;
        private TextView mTxtOpenTime;
        private TextView mTxtName;
        private TextView mTxtHighway;
        private TextView mTxtType;
        private TextView mTxtLocation;
        private TextView mTxtDistance;
        private LinearLayout lnlcons;
        private string mCurrentType;
        private int mIntPage = -1;
        private HorizontalScrollView scrollIcon;
        private LatLng mItemLatLng;
        private string mStrMarkerTitle = "";
        private ImageView imgFavorite;
        private ImageView imgFeedback;
        private bool mIsFavorite;
        private int mIntPosition = -1;
        private ImageView mImgSign;
        private BaseItem mCurrentItem = new BaseItem();
        private ImageView mImgShare;

        private ImageView mImgFood;


        public enum ModelType
        {
            Petrol = 1,
            RSA = 0,
            Facility = 4,
            CSC = 2,
            TollPlaza = 3
        }

        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }

        public int dpToPx(int dp)
        {
            DisplayMetrics displayMetrics = Resources.DisplayMetrics;
            int px = (int)Math.Round(dp * (displayMetrics.Density ));
            return px;
        }

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.activity_facility_detail;
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
            mIsCCTV = this.Intent.GetBooleanExtra(MODEL_HASCCTV, false);
            mIsFOOD = this.Intent.GetBooleanExtra(MODEL_HASFOOD, false);



            mCurrentType = this.Intent.GetStringExtra(MODELSOURCE);
            imgFavorite = FindViewById<ImageView>(Resource.Id.imgFavorite);
            imgFeedback = FindViewById<ImageView>(Resource.Id.imgFeedback);
            mTxtName = FindViewById<TextView>(Resource.Id.txtName);
            mTxtHighway = FindViewById<TextView>(Resource.Id.txtHighwayName);
            mTxtType = FindViewById<TextView>(Resource.Id.txtType);
            mTxtLocation = FindViewById<TextView>(Resource.Id.txtLocation);
            mTxtDistance = FindViewById<TextView>(Resource.Id.txtDistance);
            mTxtOpenTime = FindViewById<TextView>(Resource.Id.txtOpenHour);
            lnlcons = FindViewById<LinearLayout>(Resource.Id.lnlcons);
            mImgSign = FindViewById<ImageView>(Resource.Id.imgSign);
            mImgShare = FindViewById<ImageView>(Resource.Id.imgShare);
            scrollIcon = FindViewById<HorizontalScrollView>(Resource.Id.horizontalScrollView);

            var type = this.Intent.GetIntExtra(MODELTYPE, 0);
            mType = (ModelType)type;
            mStrJsonData = this.Intent.GetStringExtra(MODELData);

            mIsFavorite = this.Intent.GetBooleanExtra(MODELISFAVORITE, false);
            mIntPosition = this.Intent.GetIntExtra(MODELPOSITION, -1);

            mImgShare.Click += MImgShare_Click;

            mImgFood = FindViewById<ImageView>(Resource.Id.imgFood);

           

            if (mIsFOOD)
            {
                mImgFood.Visibility = ViewStates.Visible;
                mImgFood.Click += MImgFood_Click;
            }
            else
            {

            }


            if (mIsFavorite)
            {
                imgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite);
            }
            else
            {
                imgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
            }
            imgFavorite.Click += ImgFavorite_Click;

            mTxtOpenTime.Visibility = ViewStates.Gone;
            if (mType == ModelType.RSA)
            {
                var itemRSA = JsonConvert.DeserializeObject<TblRSA>(mStrJsonData);

                if (!mIsCCTV)
                {
                    mIsCCTV = !string.IsNullOrEmpty(itemRSA.strPicture);
                }
           

                //PointOfInterestThread thread1 = new PointOfInterestThread();
                //thread1.loadPOIDetail(itemRSA.idRSA, "1", "1");

                mCurrentItem.Item = itemRSA;
                mTblRSA = itemRSA;
                mItemLatLng = new LatLng((double)itemRSA.decLat, (double)itemRSA.decLong);
                mStrMarkerTitle = itemRSA.strName;
                
               this.Title = itemRSA.strName;

                if (mCurrentType == MODEL_RSA)
                {
                    mTxtType.Text = GetString(Resource.String.facilities_type_rsa);
                }
                else if (mCurrentType == MODEL_LAYBY)
                {
                    mTxtType.Text = GetString(Resource.String.facilities_type_lay_by);
                }
                else if (mCurrentType == MODEL_INTERCHANGE)
                {
                    mTxtType.Text = GetString(Resource.String.facilities_type_interchange);
                }
                else if (mCurrentType == MODEL_TUNNEL)
                {
                    mTxtType.Text = GetString(Resource.String.facilities_type_tunnel);
                }
                else if (mCurrentType == MODEL_VISTAPOINT)
                {
                    mTxtType.Text = GetString(Resource.String.facilities_type_vistapoint);
                }
                
                mTxtLocation.Text = string.Format(GetString(Resource.String.facility_detail_location_format), itemRSA.decLocation);

              

                PointOfInterestThread thread = new PointOfInterestThread();
                thread.OnloadHighwayResult += (resultHighway) =>
                {
                    mTxtHighway.Text = (resultHighway as TblHighway).strName;
                };

               
                thread.OnloadListFacilityPageByParent += (resultFacility) =>
                {
                    if (resultFacility.Count == 0)
                    {
                        scrollIcon.Visibility = ViewStates.Gone;
                    } 
                    else
                    {
                        List<string> idIcons = new List<string>();
                        foreach (BaseItem item in resultFacility)
                        {
                            var itemFac = item.Item as TblFacilities;
                            string icon = (string)item.getTag(BaseItem.TagName.StrICon);
                            if (!string.IsNullOrEmpty(icon))
                            {
                                idIcons.Add(icon);
                                ImageView imgIcon = new ImageView(this);
                                imgIcon.LayoutParameters = new ViewGroup.LayoutParams(dpToPx(48), dpToPx(48));
                                imgIcon.SetPadding(dpToPx(11), dpToPx(11), dpToPx(11), dpToPx(11));
                                imgIcon.SetScaleType(ImageView.ScaleType.CenterInside);
                                //imgIcon.SetImageResource(idIcon);
                                lnlcons.AddView(imgIcon);
                                
                                Picasso.With(this).Load(icon).Error(Resource.Drawable.img_error).Into(imgIcon);
                            }
                        }
                    }
                };
                thread.loadFacilityDetail(itemRSA.idRSA, itemRSA.idHighway, PointOfInterestThread.LoadType.RSA);

                if (!string.IsNullOrEmpty(itemRSA.strSignatureName))
                {
                    mImgSign.Visibility = ViewStates.Visible;
                }
                else
                {
                    mImgSign.Visibility = ViewStates.Gone;
                }

                //mTxtOpenTime.Text = string.Format ( GetString(Resource.String.txt_format_openhour), itemRSA.str)

                mTxtOpenTime.Visibility = ViewStates.Gone;
            }
            else if (mType == ModelType.Petrol)
            {
                var itemPertrol = JsonConvert.DeserializeObject<TblPetrolStation>(mStrJsonData);
                if (!mIsCCTV)
                {
                    mIsCCTV = !string.IsNullOrEmpty(itemPertrol.strPicture);
                }
                mCurrentItem.Item = itemPertrol;
                mItemLatLng = new LatLng((double)itemPertrol.decLat, (double)itemPertrol.decLong);
                mStrMarkerTitle = itemPertrol.strName;
                this.Title = itemPertrol.strName;
                mTxtType.Text = GetString(Resource.String.facilities_type_petrol_station);
                mTxtLocation.Text = string.Format(GetString(Resource.String.facility_detail_location_format), itemPertrol.decLocation);

                if (mCurrentType == MODEL_PETROLSTATION)
                {
                    mTxtType.Text = "- " + GetString(Resource.String.facilities_type_petrol_station);
                }

                PointOfInterestThread thread = new PointOfInterestThread();
                thread.OnloadHighwayResult += (resultHighway) =>
                {
                    mTxtHighway.Text = "- " + (resultHighway as TblHighway).strName;
                };

                thread.OnloadListFacilityPageByParent += (resultFacility) =>
                {
                    if (resultFacility.Count == 0)
                    {
                        scrollIcon.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        foreach (BaseItem item in resultFacility)
                        {
                            var itemFac = item.Item as TblFacilities;
                            int idIcon = this.GetResourceID("ic_facility_type" + itemFac.intFacilityType);
                            if (itemFac.intFacilityType == 0 || itemFac.intFacilityType == 2)
                            {
                                if (item.getTag(BaseItem.TagName.Facility_BrandID) != null)
                                {
                                    var value = item.getTag(BaseItem.TagName.Facility_BrandID);
                                    string idBrand = (string)value;
                                    if (!string.IsNullOrEmpty(idBrand))
                                    {
                                        idIcon = this.GetResourceID(idBrand.ToLower());
                                    }
                                }

                            }
                            if (idIcon == 0)
                            {
                                continue;
                            }
                            ImageView imgIcon = new ImageView(this);
                            imgIcon.LayoutParameters = new ViewGroup.LayoutParams(FrameLayout.LayoutParams.WrapContent, FrameLayout.LayoutParams.MatchParent);
                            imgIcon.SetScaleType(ImageView.ScaleType.Center);
                            imgIcon.SetImageResource(idIcon);
                            lnlcons.AddView(imgIcon);
                        }
                    }
                };
                thread.loadFacilityDetail(itemPertrol.idRSA, itemPertrol.idHighway, PointOfInterestThread.LoadType.Other);

                //mTxtOpenTime.Text = string.Format(GetString(Resource.String.txt_format_openhour), itemPertrol.)
            }
            else if (mType == ModelType.Facility)
            {
                var itemFacility = JsonConvert.DeserializeObject<TblFacilities>(mStrJsonData);
                mCurrentItem.Item = itemFacility;
                mStrMarkerTitle = itemFacility.strName;

                if (!mIsCCTV)
                {
                    mIsCCTV = !string.IsNullOrEmpty(itemFacility.strPicture);
                }


                mItemLatLng = new LatLng((double)itemFacility.decLat, (double)itemFacility.decLong);
                this.Title = itemFacility.strName;

                if (mCurrentType == MODEL_PLUS_SMILE)
                {
                    mTxtType.Text = "- " + GetString(Resource.String.facilities_type_plussmie);
                }
                if (mCurrentType == MODEL_SSK)
                {
                    mTxtType.Text = "- " +  GetString(Resource.String.facilities_type_ssk);
                }

                
                mTxtLocation.Text = string.Format(GetString(Resource.String.facility_detail_location_format), itemFacility.decLocation);

                PointOfInterestThread thread = new PointOfInterestThread();
                thread.OnloadHighwayResult += (resultHighway) =>
                {
                    mTxtHighway.Text = "- " + (resultHighway as TblHighway).strName;
                };

                thread.OnloadListFacilityPageByParent += (resultFacility) =>
                {
                    if (resultFacility.Count == 0)
                    {
                        scrollIcon.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        foreach (BaseItem item in resultFacility)
                        {
                            var itemFac = item.Item as TblFacilities;
                            int idIcon = this.GetResourceID("ic_facility_type" + itemFac.intFacilityType);
                            if (itemFac.intFacilityType == 0 || itemFac.intFacilityType == 2)
                            {
                                if (item.getTag(BaseItem.TagName.Facility_BrandID) != null)
                                {
                                    var value = item.getTag(BaseItem.TagName.Facility_BrandID);
                                    string idBrand = (string)value;
                                    if (!string.IsNullOrEmpty(idBrand))
                                    {
                                        idIcon = this.GetResourceID(idBrand.ToLower());
                                    }
                                }

                            }
                            if (idIcon == 0)
                            {
                                continue;
                            }
                            ImageView imgIcon = new ImageView(this);
                            imgIcon.LayoutParameters = new ViewGroup.LayoutParams(FrameLayout.LayoutParams.WrapContent, FrameLayout.LayoutParams.MatchParent);
                            imgIcon.SetScaleType(ImageView.ScaleType.Center);
                            imgIcon.SetImageResource(idIcon);
                            lnlcons.AddView(imgIcon);
                        }
                    }
                };
                thread.loadFacilityDetail(itemFacility.idFacilities, itemFacility.idHighway, PointOfInterestThread.LoadType.Other);
            }
            else if (mType == ModelType.CSC)
            {
                var itemCSC = JsonConvert.DeserializeObject<TblCSC>(mStrJsonData);
             

                mCurrentItem.Item = itemCSC;
                mStrMarkerTitle = itemCSC.strName;
                mItemLatLng = new LatLng((double)itemCSC.decLat, (double)itemCSC.decLong);
                this.Title = itemCSC.strName;
                mTxtType.Text = GetString(Resource.String.facilities_type_csc);
                mTxtLocation.Text = string.Format(GetString(Resource.String.facility_detail_location_format), itemCSC.decLocation);

                PointOfInterestThread thread = new PointOfInterestThread();
                thread.OnloadHighwayResult += (resultHighway) =>
                {
                    mTxtHighway.Text = "- " + (resultHighway as TblHighway).strName;
                };

                thread.OnloadListFacilityPageByParent += (resultFacility) =>
                {
                    if (resultFacility.Count == 0)
                    {
                        scrollIcon.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        foreach (BaseItem item in resultFacility)
                        {
                            var itemFac = item.Item as TblFacilities;
                            int idIcon = this.GetResourceID("ic_facility_type" + itemFac.intFacilityType);
                            if (itemFac.intFacilityType == 0 || itemFac.intFacilityType == 2)
                            {
                                if (item.getTag(BaseItem.TagName.Facility_BrandID) != null)
                                {
                                    var value = item.getTag(BaseItem.TagName.Facility_BrandID);
                                    string idBrand = (string)value;
                                    if (!string.IsNullOrEmpty(idBrand))
                                    {
                                        idIcon = this.GetResourceID(idBrand.ToLower());
                                    }
                                }

                            }
                            if (idIcon == 0)
                            {
                                continue;
                            }
                            ImageView imgIcon = new ImageView(this);
                            imgIcon.LayoutParameters = new ViewGroup.LayoutParams(FrameLayout.LayoutParams.WrapContent, FrameLayout.LayoutParams.MatchParent);
                            imgIcon.SetScaleType(ImageView.ScaleType.Center);
                            imgIcon.SetImageResource(idIcon);
                            lnlcons.AddView(imgIcon);
                        }
                    }
                };
                thread.loadFacilityDetail(itemCSC.idCSC, itemCSC.idHighway, PointOfInterestThread.LoadType.Other);
            }
            else if (mType == ModelType.TollPlaza)
            {
                var itemTollPlaza = JsonConvert.DeserializeObject<TblTollPlaza>(mStrJsonData);
                mCurrentItem.Item = itemTollPlaza;
                mStrMarkerTitle = itemTollPlaza.strName;
                mItemLatLng = new LatLng((double)itemTollPlaza.decLat, (double)itemTollPlaza.decLong);
                this.Title = itemTollPlaza.strName;
                mTxtType.Text = GetString(Resource.String.facilities_type_toll_plaza);
                mTxtLocation.Text = string.Format(GetString(Resource.String.facility_detail_location_format), itemTollPlaza.decLocation);

                PointOfInterestThread thread = new PointOfInterestThread();
                thread.OnloadHighwayResult += (resultHighway) =>
                {
                    mTxtHighway.Text = "- " + (resultHighway as TblHighway).strName;
                };

                thread.OnloadListFacilityPageByParent += (resultFacility) =>
                {
                    if (resultFacility.Count == 0)
                    {
                        scrollIcon.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        foreach (BaseItem item in resultFacility)
                        {
                            var itemFac = item.Item as TblFacilities;
                            int idIcon = this.GetResourceID("ic_facility_type" + itemFac.intFacilityType);
                            if (itemFac.intFacilityType == 0 || itemFac.intFacilityType == 2)
                            {
                                if (item.getTag(BaseItem.TagName.Facility_BrandID) != null)
                                {
                                    var value = item.getTag(BaseItem.TagName.Facility_BrandID);
                                    string idBrand = (string)value;
                                    if (!string.IsNullOrEmpty(idBrand))
                                    {
                                        idIcon = this.GetResourceID(idBrand.ToLower());
                                    }
                                }

                            }
                            if (idIcon == 0)
                            {
                                continue;
                            }
                            ImageView imgIcon = new ImageView(this);
                            imgIcon.LayoutParameters = new ViewGroup.LayoutParams(FrameLayout.LayoutParams.WrapContent, FrameLayout.LayoutParams.MatchParent);
                            imgIcon.SetScaleType(ImageView.ScaleType.Center);
                            imgIcon.SetImageResource(idIcon);
                            lnlcons.AddView(imgIcon);
                        }
                    }
                };
                thread.loadFacilityDetail(itemTollPlaza.idTollPlaza, itemTollPlaza.idHighway, PointOfInterestThread.LoadType.TollPlaza);
            }
            Location locFrom = GPSUtils.getLastBestLocation(this);
            if (locFrom == null)
            {
                mTxtDistance.Text = string.Format(GetString(Resource.String.facility_detail_distance_format), "0");
            }
            else
            {
                LatLng from = new LatLng(locFrom.Latitude, locFrom.Longitude);
                LatLng to = new LatLng(mItemLatLng.Latitude, mItemLatLng.Longitude);
                mTxtDistance.Text = string.Format(GetString(Resource.String.facility_detail_distance_format), GPSUtils.Distance(from, to, GPSUtils.DistanceUnit.Kilometers));
            }


            //
            

            ImageView imgCCTV = FindViewById<ImageView>(Resource.Id.imgCCTV);
            imgCCTV.Click += ImgCCTV_Click;

            if (!mIsCCTV)
            {
                imgCCTV.Visibility = ViewStates.Gone;
                ChangeToMap();
            }
            else
            {
                imgCCTV.Visibility = ViewStates.Visible;
                ChangeToCCTV();
            }
            ImageView imgMap = FindViewById<ImageView>(Resource.Id.imgMap);
            imgMap.Click += ImgMap_Click;

            ImageView imgDirection = FindViewById<ImageView>(Resource.Id.imgDirection);
            imgDirection.Click += ImgDirection_Click;

            imgFeedback.Click += ImgFeedback_Click;

            mLocationManager = (LocationManager)this.GetSystemService(Context.LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = mLocationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                mLocationProvider = acceptableLocationProviders.First();
            }
            else
            {
                mLocationProvider = string.Empty;
            }
            /*
            if (!string.IsNullOrEmpty(mLocationProvider))
            {
                mLocationManager.RequestLocationUpdates(mLocationProvider, 0, 0, this);
            }*/
            
            // Create your application here
        }

        private void MImgFood_Click(object sender, EventArgs e)
        {
            ChangeToImages();
        }

        private void MImgShare_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (mTblRSA != null)
            {
                if (mIntPage == 1)
                {
                    Fragment_Facility_Detail_CCTV cctv = mCurrentFragment as Fragment_Facility_Detail_CCTV;
                    var bitmap = cctv.GetBitmap();
                    if (bitmap != null)
                    {
                        Intent intent = new Intent(Intent.ActionSend);
                        intent.PutExtra(Intent.ExtraSubject, mTblRSA.strName);
                        intent.PutExtra(Intent.ExtraTitle, mTblRSA.strName);
                        intent.PutExtra(Intent.ExtraText, mTblRSA.strName);
                        intent.SetType("image/jpeg");
                        string filename = Expresso.Utils.ImageUtils.ExportBitmapAsPNG(bitmap);
                        intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + filename));
                        StartActivity(intent);
                    }
                }
                else if (mIntPage == 0)
                {
                    Intent intent = new Intent(Intent.ActionSend);
                    var uri = String.Format("http://maps.google.com/maps?daddr={0},{1}", mTblRSA.decLat, mTblRSA.decLong);
                    intent.PutExtra(Intent.ExtraText, uri);
                    intent.PutExtra(Intent.ExtraSubject, mTblRSA.strName);
                    intent.PutExtra(Intent.ExtraTitle, mTblRSA.strName);
                    intent.PutExtra(Intent.ExtraText, mTblRSA.strName);
                    intent.SetType("text/plain");
                    StartActivity(intent);
                }
            }
        }

        private void ImgFeedback_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            ChangeToFeedBack();
        }

        private void ImgFavorite_Click(object sender, EventArgs e)
        {
            mIsFavorite = !mIsFavorite;
            if (mIsFavorite)
            {
                imgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite);
            }
            else
            {
                imgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
            }
           
        }

        private void ImgDirection_Click(object sender, EventArgs e)
        {
            string uri = "";
            uri = String.Format("http://maps.google.com/maps?daddr={0},{1}", mItemLatLng.Latitude, mItemLatLng.Longitude);
            Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri));
            StartActivity(intent);
        }

        private void ImgMap_Click(object sender, EventArgs e)
        {
            
            ChangeToMap();
        }

        private void ChangeToMap()
        {
            if (mIntPage == 0)
            {
                return;
            }
            mIntPage = 0;
            mCurrentFragment = Fragment_Maps.NewInstance(mItemLatLng, mStrMarkerTitle);
                Android.Support.V4.App.FragmentTransaction fragmentTransaction = SupportFragmentManager.BeginTransaction();
            fragmentTransaction.Replace(Resource.Id.frgContent, mCurrentFragment);
            fragmentTransaction.Commit();
        }

        public void ChangeToFeedBack()
        {
            if (mIntPage == 2)
            {
                return;
            }
            mIntPage = 2;
            Android.Support.V4.App.FragmentTransaction fragmentTransaction = SupportFragmentManager.BeginTransaction();
            fragmentTransaction.Replace(Resource.Id.frgContent, Fragment_Feedback.NewInstance(mCurrentItem));
            fragmentTransaction.Commit();
        }

        private void ChangeToCCTV()
        {
            if (mIntPage == 1)
            {
                return;
            }
            if (mTblRSA == null)
            {
                return;
            }
            mIntPage = 1;
            Android.Support.V4.App.FragmentTransaction fragmentTransaction = SupportFragmentManager.BeginTransaction();

            mCurrentFragment = Fragment_Facility_Detail_CCTV.NewInstance(mTblRSA);

            fragmentTransaction.Replace(Resource.Id.frgContent, mCurrentFragment);
            fragmentTransaction.Commit();
        }

        private void ChangeToImages()
        {
            if (mIntPage == 3)
            {
                return;
            }
            if (mTblRSA == null)
            {
                return;
            }
            mIntPage = 3;
            Android.Support.V4.App.FragmentTransaction fragmentTransaction = SupportFragmentManager.BeginTransaction();

            mCurrentFragment = Fragment_Rsa_FoodSignature.NewInstance(mTblRSA);

            fragmentTransaction.Replace(Resource.Id.frgContent, mCurrentFragment);
            fragmentTransaction.Commit();
        }



        protected override void OnResume()
        {
            base.OnResume();
            if (!string.IsNullOrEmpty(mLocationProvider))
            {
                mLocationManager.RequestLocationUpdates(mLocationProvider, 0, 0, this);
            }
            
        }

        protected override void OnPause()
        {
            base.OnPause();
            mLocationManager.RemoveUpdates(this);
        }

        private void ImgCCTV_Click(object sender, EventArgs e)
        {
            ChangeToCCTV();
        }

        private Location mCurrentLocation;
        public void OnLocationChanged(Location location)
        {
            mCurrentLocation = location;
            LatLng from = new LatLng(location.Latitude, location.Longitude);
            LatLng to = new LatLng(mItemLatLng.Latitude, mItemLatLng.Longitude);
            mTxtDistance.Text = string.Format(GetString(Resource.String.facility_detail_distance_format), GPSUtils.Distance(from, to, GPSUtils.DistanceUnit.Kilometers));

            if (mIntPage == 0)
            {
                Fragment_Maps map = SupportFragmentManager.FindFragmentById(Resource.Id.frgContent) as Fragment_Maps;
                map.UpdateMyLocation(location);
            }
            //throw new NotImplementedException();
        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
          
        }

        public override void OnBackPressed()
        {
            Intent intent = new Intent();
            intent.PutExtra(MODELPOSITION, mIntPosition);
            intent.PutExtra(MODELISFAVORITE, mIsFavorite);
            SetResult(Result.Ok, intent);
            base.OnBackPressed();
        }


    }
}