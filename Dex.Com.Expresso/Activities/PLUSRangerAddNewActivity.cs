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
using Android.Locations;
using EXPRESSO.Models;
using Dex.Com.Expresso.Adapters.Spinner;
using EXPRESSO.Utils;
using EXPRESSO.Threads;
using EXPRESSO.Models.Database;
using System.Threading;
using Android.Database;
using Android.Provider;
//using Dex.Com.Expresso.Adapters.Galerry;
using Dex.Com.Expresso.Dialogs;
using System.IO;
using Android.Support.V7.App;
using Dex.Com.Expresso.Utils;
using Android.Graphics;
using Java.IO;
using Dex.Com.Expresso.Adapters.RecyclerViews;
using Android.Support.V7.Widget;
using Dex.Com.Expresso.Animations;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Dex.Com.Expresso.Fragments;
using System.Threading.Tasks;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "@string/title_plusranger_add")]
    public class PLUSRangerAddNewActivity : BaseActivity, ILocationListener
    {
        private Spinner mSpnCategory;
        private EditText mTxtTitle, mTxtDescription, mTxtEmail, mTxtPhone, mTxtAddress;
        private Button mBtnSubmit;
        private PLUSRangerCategoryAdapter adpCategory;
        private Location mMyLocation;
        private Location _currentLocation;
        private string _locationProvider;
        private LocationManager _locationManager;
        private ImageView mImgCamera, mImgGalerry;
        public static readonly int PickImageId = 1000;
        public static readonly int CaptureImageId = 1001;

        private TblMediaAdapter mMediaAdapter;
        //private Gallery mGallery;
        private LinearLayout mLnlLoading, mLnlData;
        private Button mBtnLogin;
        public RecyclerView mLstItems;

        public LinearLayoutManager mLnlManager;

        public LinearLayout mLnlMaps;
        public TextView mTxtCurrentYourLocation;
        private bool IsExpaned = false;

        //private SupportMapFragment mapFrag;
        private GoogleMap mMap;
        private Marker mMarker;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.exp_activity_plusranger_addnew;
            }
        }

        public void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.Toolbar.Title = GetString(Resource.String.title_plusranger_add);
            InitializeLocationManager();

            this.mLnlManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);

            FindViewById<TextView>(Resource.Id.txtAddress).Text = "";
            mLnlLoading = FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            mLnlData = FindViewById<LinearLayout>(Resource.Id.lnlData);

            mLstItems = FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLnlManager);

            mBtnLogin = FindViewById<Button>(Resource.Id.btnLogin);

            mSpnCategory = FindViewById<Spinner>(Resource.Id.spnCategory);
            mTxtTitle = FindViewById<EditText>(Resource.Id.txtTitle);
            mTxtDescription = FindViewById<EditText>(Resource.Id.txtDescription);
            mTxtEmail = FindViewById<EditText>(Resource.Id.txtEmail);
            mTxtPhone = FindViewById<EditText>(Resource.Id.txtMobile);
            mBtnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            mImgCamera = FindViewById<ImageView>(Resource.Id.imgCamera);
            mImgGalerry = FindViewById<ImageView>(Resource.Id.imgGalerry);
            //mGallery = FindViewById<Gallery>(Resource.Id.gallery2);
            mMediaAdapter = new TblMediaAdapter(this, new List<TblMedia>());
            mLstItems.SetAdapter(mMediaAdapter);
            mMediaAdapter.ItemClick += MMediaAdapter_ItemClick;
            
            //mGallery.Adapter = mMediaAdapter;


            mBtnSubmit.Click += MBtnSubmit_Click;
            mImgCamera.Click += MImgCamera_Click;
            mImgGalerry.Click += MImgGalerry_Click;

            mTxtCurrentYourLocation = FindViewById<TextView>(Resource.Id.txtYourLocation);
            mTxtCurrentYourLocation.Click += MTxtCurrentYourLocation_Click;
            FindViewById<View>(Resource.Id.imgMarker).Click += MTxtCurrentYourLocation_Click;
            mLnlMaps = FindViewById<LinearLayout>(Resource.Id.lnlMaps);

            //mGallery.ItemClick += MGallery_ItemClick;
            PLUSRangerThreads thread = new PLUSRangerThreads();
            mMyLocation = Utils.GPSUtils.getLastBestLocation(this);
            if (mMyLocation != null)
            {
                new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        var geo = new Geocoder(this);
                        var addresses = geo.GetFromLocation(mMyLocation.Latitude, mMyLocation.Longitude, 1);
                        RunOnUiThread(() =>
                        {
                            var addressText = FindViewById<TextView>(Resource.Id.txtAddress);

                            Address address = addresses.FirstOrDefault();
                            if (address != null)
                            {
                                StringBuilder deviceAddress = new StringBuilder();
                                for (int i = 0; i < address.MaxAddressLineIndex; i++)
                                {
                                    if (i == address.MaxAddressLineIndex - 1)
                                    {
                                        deviceAddress.Append(address.GetAddressLine(i));
                                    }
                                    else
                                    {
                                        deviceAddress.Append(address.GetAddressLine(i) + ", ");
                                    }
                                }
                                addressText.Text = deviceAddress.ToString();
                            }

                        });
                    }
                    catch (Exception ex)
                    {

                    }
                })).Start();
            }

            if (Cons.myEntity.User != null)
            {
                mTxtEmail.Text = Cons.myEntity.User.strUserName;
                mTxtPhone.Text = Cons.myEntity.User.strMobileNo;
            }

            // Create your application here

            thread.OnGetCategory += (ServiceResult result) =>
            {
                if (result.intStatus == 1)
                {
                    if (result.Data is List<TblCategory>)
                    {
                        var data = result.Data as List<TblCategory>;
                        adpCategory = new PLUSRangerCategoryAdapter(this, data);
                        mSpnCategory.Adapter = adpCategory;
                    }

                }
                else
                {
                    Toast.MakeText(this, result.strMess, ToastLength.Short).Show();
                }

            };
            thread.GetCategory();

            //new Thread(new ThreadStart(() => {
            //    var geo = new Geocoder(this);
            //    //var addresses = geo.GetFromLocation();
            //    RunOnUiThread(() => {
            //            var addressText = FindViewById<TextView>(Resource.Id.txtAddress);
            //            addresses.ToList().ForEach((addr) => {addressText.Append(addr.ToString() + ", ");
            //        });
            //    });
            //})).Start();

            //mBtnLogin.Click += MBtnLogin_Click;


            var mCurrentFragment = Fragment_Maps.NewInstance(null, null);
            mCurrentFragment.onGetMap += (GoogleMap map, SupportMapFragment mapFrag) =>
            {
                mMap = map;
                mMap.Clear();
                var location = GPSUtils.getLastBestLocation(this);
                LatLng latlng = null;
                MarkerOptions option = new MarkerOptions();
                option.SetTitle("My Location");

                //3.1358284,101.6905293
                if (location == null)
                {
                    latlng = new LatLng(3.1358284, 101.6905293);
                }
                else
                {
                    latlng = new LatLng(location.Latitude, location.Longitude);
                }

                option.SetPosition(latlng);
                mMarker = map.AddMarker(option);
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(latlng);
                builder.Zoom(10);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                if (map != null)
                {
                    map.MoveCamera(cameraUpdate);
                }

                map.MapClick += Map_MapClick;
                map.UiSettings.ZoomGesturesEnabled = true;
            };
            Android.Support.V4.App.FragmentTransaction fragmentTransaction = SupportFragmentManager.BeginTransaction();
            fragmentTransaction.Replace(Resource.Id.frgContent, mCurrentFragment);
            fragmentTransaction.Commit();
        }

        private void Map_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            // throw new NotImplementedException();
            mMarker.Position = e.Point;
            Location location = new Location("Test");
            location.Latitude = e.Point.Latitude;
            location.Longitude = e.Point.Longitude;
            mMyLocation = location;
            new Thread(new ThreadStart(() => {
                try
                {
                    var geo = new Geocoder(this);
                    var addresses = geo.GetFromLocation(mMyLocation.Latitude, mMyLocation.Longitude, 1);
                    RunOnUiThread(() =>
                    {
                        var addressText = FindViewById<TextView>(Resource.Id.txtAddress);

                        Address address = addresses.FirstOrDefault();
                        if (address != null)
                        {
                            StringBuilder deviceAddress = new StringBuilder();
                            for (int i = 0; i < address.MaxAddressLineIndex; i++)
                            {
                                if (i == address.MaxAddressLineIndex - 1)
                                {
                                    deviceAddress.Append(address.GetAddressLine(i));
                                }
                                else
                                {
                                    deviceAddress.Append(address.GetAddressLine(i) + ", ");
                                }

                            }
                            addressText.Text = deviceAddress.ToString();
                        }
                    });
                }
                catch (Exception ex)
                {

                }

            })).Start();


        }

        private void MTxtCurrentYourLocation_Click(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            IsExpaned = !IsExpaned;
            if (IsExpaned)
            {
                ResizeAnimation resize = new ResizeAnimation(mLnlMaps, mLnlMaps.Width, mLnlMaps.Height, mLnlMaps.Width, 700);
                mLnlMaps.StartAnimation(resize);
            }
            else
            {
                ResizeAnimation resize = new ResizeAnimation(mLnlMaps, mLnlMaps.Width, mLnlMaps.Height, mLnlMaps.Width, 0);
                mLnlMaps.StartAnimation(resize);
            }
        }

        private void MMediaAdapter_ItemClick(object sender, TblMedia e)
        {
            TblMedia item = e;

            FragmentTransaction ft = FragmentManager.BeginTransaction();
            Fragment prev = FragmentManager.FindFragmentByTag("dialog_comment");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
            MediaDialog newFragment = MediaDialog.NewInstance(null, item);
            newFragment.OnSave += (string data) =>
            {
                item.mStrComment = data;
                this.mMediaAdapter.NotifyDataSetChanged();
                this.mMediaAdapter.UpdateMedia(item);
            };
            newFragment.OnDelete += (string id) =>
            {
                mMediaAdapter.DeleteItem(item);
            };
            newFragment.Show(ft, "dialog_comment");
        }
   
        private void MGallery_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
           
        }
       

        private async void MImgGalerry_Click(object sender, EventArgs e)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
        }

       

     
        private void MImgCamera_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            //intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));
            StartActivityForResult(intent, CaptureImageId); ;
        }

      
        private void MBtnSubmit_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

            if (!GPSUtils.IsEnable(this))
            {
                Toast.MakeText(this, Resource.String.text_gps, ToastLength.Short).Show();
                return;
            }
            if (!Net.IsEnable(this))
            {
                Toast.MakeText(this, Resource.String.text_network, ToastLength.Short).Show();
                return;
            }

            if (mMyLocation == null)
            {
                Toast.MakeText(this, Resource.String.text_gps, ToastLength.Short).Show();
                return;
            }
            if (mSpnCategory.SelectedItemPosition == -1)
            {
                return;
            }

            string idCategory = adpCategory.GetHighway(mSpnCategory.SelectedItemPosition).idCategory;
            string title = mTxtTitle.Text;
            string description = mTxtDescription.Text;
            string email = mTxtEmail.Text;
            string phone = mTxtPhone.Text;

            int notifyID = DateTime.Now.Millisecond;
            NotificationCompat.Builder mBuilder = new NotificationCompat.Builder(this);
            mBuilder.SetWhen(DateTime.Now.Ticks);
            mBuilder.SetSmallIcon(Resource.Drawable.Icon);
            mBuilder.SetContentTitle(GetString(Resource.String.app_name) + " - " + title);
            mBuilder.SetContentText("Data is uploading to server...");
            mBuilder.SetAutoCancel(true);

            NotificationManager mNotificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            mNotificationManager.Notify(notifyID, mBuilder.Build());

            PLUSRangerThreads thread = new PLUSRangerThreads();
            thread.OnPost += (ServiceResult result) =>
            {
                if (result.intStatus == 1)
                {
                    if (mMediaAdapter.ItemCount > 0)
                    {
                        mBuilder.SetContentText("Uploading image to server");
                        mNotificationManager.Notify(notifyID, mBuilder.Build());
                        Task.Run(new Action(() =>
                        {
                            foreach (var item in mMediaAdapter.getListItems())
                            {
                                Dex.Com.Expresso.Utils.FtpClient.sendAPicture(item.RandomName, item.Path);
                            }
                        }));
                      
                    }
                    mBuilder.SetContentText("Data is updated to server");
                    mNotificationManager.Notify(notifyID, mBuilder.Build());
                }
                else
                {
                    Toast.MakeText(this, result.strMess, ToastLength.Short).Show();
                    mBuilder.SetContentText(result.strMess);
                    mNotificationManager.Notify(notifyID, mBuilder.Build());
                }
            };
            thread.SavePostToLocal("", title, description, email, phone, idCategory , mMyLocation == null ? 0 : mMyLocation.Latitude, mMyLocation.Longitude == null ? 0 : mMyLocation.Longitude, mMediaAdapter.getListItems());
            Finish();
            //thread.
        }

        public void OnLocationChanged(Location location)
        {
          
            if (!IsExpaned)
            {
                mMyLocation = location;
                new Thread(new ThreadStart(() => {
                    try
                    {
                        var geo = new Geocoder(this);
                        var addresses = geo.GetFromLocation(mMyLocation.Latitude, mMyLocation.Longitude, 1);
                        RunOnUiThread(() =>
                        {
                            var addressText = FindViewById<TextView>(Resource.Id.txtAddress);

                            Address address = addresses.FirstOrDefault();
                            if (address != null)
                            {
                                StringBuilder deviceAddress = new StringBuilder();
                                for (int i = 0; i < address.MaxAddressLineIndex; i++)
                                {
                                    if (i == address.MaxAddressLineIndex - 1)
                                    {
                                        deviceAddress.Append(address.GetAddressLine(i));
                                    }
                                    else
                                    {
                                        deviceAddress.Append(address.GetAddressLine(i) + ", ");
                                    }

                                }
                                addressText.Text = deviceAddress.ToString();
                            }
                        });
                    }
                    catch (Exception ex)
                    {

                    }

                })).Start();
            }
        }

        public void OnProviderDisabled(string provider)
        {
            //Finish();
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {

        }

        protected override void OnResume()
        {
            base.OnResume();
            _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
            var myentity = getMyEntity().Where(p => p.User.strToken != null).FirstOrDefault();
            Cons.myEntity = myentity;
            if (myentity != null)
            {
                mLnlLoading.Visibility = ViewStates.Gone;
                mLnlData.Visibility = ViewStates.Visible;

                mTxtEmail.Text = myentity.User.strUserName;
                mTxtPhone.Text = myentity.User.strMobileNo;
            }
            else
            {
                mLnlLoading.Visibility = ViewStates.Visible;
                mLnlData.Visibility = ViewStates.Gone;

             
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {
                Android.Net.Uri uri = data.Data;
                string path = Dex.Com.Expresso.Utils.ImageUtils.GetFilePathFromContentUri(uri, this);
                TblMedia media = new TblMedia();
                //media.Data = Dex.Com.Expresso.Utils.ImageUtils.UriToByteArray(this, uri);
                media.idMedia = Guid.NewGuid().ToString();
                media.Path = path;
                //media.idopscomm 
                media.intPosted = 0;
                media.strURL = uri.ToString();
                media.RandomName = "PLUS_" + DateTime.UtcNow.Ticks.ToString();
                mMediaAdapter.AddMedia(media);
                mMediaAdapter.NotifyDataSetChanged();

                //_imageView.SetImageURI(uri);
            }
            else if ((requestCode == CaptureImageId) && (resultCode == Result.Ok) && (data != null))
            {
               
                string path = Android.OS.Environment.ExternalStorageDirectory.ToString();
                Java.IO.File file = new Java.IO.File(path, "PLUS_"+ DateTime.UtcNow.Ticks + ".jpg");
                var bitmap = (Bitmap)data.Extras.Get("data");

                byte[] databyte = Dex.Com.Expresso.Utils.ImageUtils.BitmapToByteArray(this, bitmap);

                OutputStream fOut = new FileOutputStream(file);
                fOut.Write(databyte);
                fOut.Flush();
                fOut.Close();
                //MediaStore.Images.Media.InsertImage(this.ContentResolver, file.AbsolutePath, file.Name, file.Name);

                var uri = Android.Net.Uri.FromFile(file);// "file:/" + file.AbsolutePath );

                TblMedia media = new TblMedia();
                //media.Data = databyte;// Dex.Com.Expresso.Utils.ImageUtils.BitmapToByteArray(this, bitmap);
                media.idMedia = Guid.NewGuid().ToString();
                media.Path = file.Path;
                //media.idopscomm 
                media.intPosted = 0;
                media.strURL = uri.ToString();
                media.RandomName = "PLUS_" + DateTime.UtcNow.Ticks.ToString();
                mMediaAdapter.AddMedia(media);
                mMediaAdapter.NotifyDataSetChanged();
            }
        }

        private void TakePicture()
        {
            // Specify some metadata for the picture
            var contentValues = new ContentValues();
            contentValues.Put(MediaStore.Images.ImageColumnsConsts.Description, "A sample image");
            // Specify where to put the image
            var _currentImageUri = ContentResolver.Insert(MediaStore.Images.Media.ExternalContentUri, contentValues);
            // Specify the message to send to the OS
            var takePictureIntent = new Intent(MediaStore.ActionImageCapture);
            takePictureIntent.PutExtra(MediaStore.ExtraOutput, _currentImageUri);
            // Start the requested activity
            StartActivityForResult(takePictureIntent, CaptureImageId);
        }


        public  string GetPathToImage(Android.Net.Uri uri)
        {
            string path = null;
            // The projection contains the columns we want to return in our query.
            string[] projection = new[] { Android.Provider.MediaStore.Audio.Media.InterfaceConsts.Data };
            using (ICursor cursor = ManagedQuery(uri, projection, null, null, null))
            {
                if (cursor != null)
                {
                    int columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Audio.Media.InterfaceConsts.Data);
                    cursor.MoveToFirst();
                    path = cursor.GetString(columnIndex);
                }
            }
            return path;
        }
    }
}