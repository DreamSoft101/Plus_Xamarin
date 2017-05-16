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
using Dex.Com.Expresso.Adapters.Listview;
using EXPRESSO.Models;
using Newtonsoft.Json;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "EmzDownloadedActivity" , ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class EmzDownloadedActivity : BaseActivity
    {
        private ListView mLstItems;
        private LinearLayout mLnlData, mLnlLoading;
        private EmzMagazineDownloadedAdapter mAdapter;
        private bool isRunning = true;
        private MyDownloadReceiver down;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.emz_activity_downloaded;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = "Downloads";
           
            mLnlLoading = FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            mLnlData = FindViewById<LinearLayout>(Resource.Id.lnlData);
            mLstItems = FindViewById<ListView>(Resource.Id.lstItems);
            mLstItems.ItemClick += MLstItems_ItemClick;
            LoadData();
        
        }

        private void MLstItems_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ///throw new NotImplementedException();
            var item = mAdapter.getBaseItem(e.Position);
            string path = System.IO.Path.Combine((string)Android.OS.Environment.ExternalStorageDirectory, "PLUS");
            string fileName = path + "/" + item.FileName;
            Java.IO.File file = new Java.IO.File(fileName);
            if (file.Exists())
            {
                var uri = Android.Net.Uri.FromFile(file);
                Intent intent = new Intent(Intent.ActionView);
                intent.SetDataAndType(uri, "application/pdf");
                intent.SetFlags(ActivityFlags.ClearTop);
                //intent.setFlags(Intent.FLAG_ACTIVITY_NO_HISTORY);
                try
                {
                    StartActivity(intent);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "No Application Available to View PDF", ToastLength.Short).Show();
                }

            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (down != null)
            {
                this.UnregisterReceiver(down);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (mAdapter != null)
            {
                mAdapter.NotifyDataSetChanged();
            }

            IntentFilter filter = new IntentFilter(DownloadManager.ActionDownloadComplete);
            if (down == null)
            {
                down = new MyDownloadReceiver();
                down.OnRe += () =>
                {
                    if (mAdapter != null)
                    {
                        mAdapter.NotifyDataSetChanged();
                    }
                };
            }

            this.RegisterReceiver(down, filter);

        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            isRunning = false;
        }

        private void LoadData()
        {
            //mLnlData.Visibility = ViewStates.Gone;
            //mLnlLoading.Visibility = ViewStates.Visible;

            mAdapter = new EmzMagazineDownloadedAdapter(this);
            mLstItems.Adapter = mAdapter;

        }



        public class MyDownloadReceiver : BroadcastReceiver
        {
            private BaseAdapter adapter;
            public delegate void onRe();
            public onRe OnRe;

            public override void OnReceive(Context context, Intent intent)
            {
                if (OnRe != null)
                {
                    OnRe();
                }

            }
        }
    }
}