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
using Java.Lang;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "EMagazineAlbumDetail", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class EMagazineAlbumDetail : BaseActivity
    {
        public static string FILENAME = "FILENAME";
        public static string TITLE = "TITLE";
        private ListView mLstItems;
        private LinearLayout mLnlData, mLnlLoading;
        private EmzMagazineAdapter mAdapter;
        private bool isRunning = true;
        private MyDownloadReceiver down;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.emz_activity_album;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = Intent.GetStringExtra(TITLE);
            mLnlLoading = FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            mLnlData = FindViewById<LinearLayout>(Resource.Id.lnlData);
            mLstItems = FindViewById<ListView>(Resource.Id.lstItems);

            string filename = Intent.GetStringExtra(FILENAME);
            LoadData(filename);

            //new Thread(new Action(() =>
            //{
            //    while (isRunning)
            //    {
            //        Thread.Sleep(1000);
            //        if (mAdapter != null)
            //        {
            //            mAdapter.NotifyDataSetChanged();
            //        }
            //    }
            //})).Run();


            // Create your application here
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.emz_download, menu);
            
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
          
            switch (item.ItemId)
            {
                case Resource.Id.menu_downloaded:
                    Intent intent = new Intent(this, typeof(EmzDownloadedActivity));
                    StartActivity(intent);
                    break;
            }
            return base.OnOptionsItemSelected(item);
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

        private void LoadData(string filename)
        {
            mLnlData.Visibility = ViewStates.Gone;
            mLnlLoading.Visibility = ViewStates.Visible;

            EmzThread thread = new EmzThread();
            thread.OnGetEmagazine += (List<Emagazine> result) =>
            {
                mLnlData.Visibility = ViewStates.Visible;
                mLnlLoading.Visibility = ViewStates.Gone;
                if (result != null)
                {
                    mAdapter = new EmzMagazineAdapter(this, result);
                    mLstItems.Adapter = mAdapter;
                }
            };
            thread.GetEmagazine(filename);
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