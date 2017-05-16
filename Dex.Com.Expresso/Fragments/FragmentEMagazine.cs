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
using EXPRESSO.Models;
using Dex.Com.Expresso.Adapters.GridView;
using EXPRESSO.Threads;
using Dex.Com.Expresso.Activities;
using Java.Lang;

namespace Dex.Com.Expresso.Fragments
{
    public class FragmentEMagazine : BaseFragment
    {
        private int mIntResult = 99;
        private EmzAlbumAdapter mAdapter;
        private GridView mGridView;
        private LinearLayout mLnlData, mLnlLoading;
       
   
        public static FragmentEMagazine NewInstance()
        {
            var frag1 = new FragmentEMagazine { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.emz_fragment_emagazine, null);
            mLnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            mLnlData = view.FindViewById<LinearLayout>(Resource.Id.lnlData);
            mGridView = view.FindViewById<GridView>(Resource.Id.gridView1);
         
            LoadData();

       
            return view;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
         
        }
        




        private void LoadData()
        {
            mLnlData.Visibility = ViewStates.Gone;
            mLnlLoading.Visibility = ViewStates.Visible;

            EmzThread thread = new EmzThread();
            thread.OnGetAlbum += (List<EmzAlbum> result) =>
            {
                if (result != null)
                {
                    mLnlData.Visibility = ViewStates.Visible;
                    mLnlLoading.Visibility = ViewStates.Gone;

                    mAdapter = new EmzAlbumAdapter(this.getActivity(), result);

                    mGridView.SetAdapter(mAdapter);
                    mGridView.ItemClick += MGridView_ItemClick;

                   
                }

            };
            thread.GetAlbum();
        }

        private void MGridView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //throw new NotImplementedException();
            var item = mAdapter.getBaseItem(e.Position);
            Intent intent = new Intent(this.getActivity(), typeof(EMagazineAlbumDetail));
            intent.PutExtra(EMagazineAlbumDetail.FILENAME, item.FileName);
            intent.PutExtra(EMagazineAlbumDetail.TITLE, item.Title);
            StartActivity(intent);
        }

      

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == mIntResult)
            {
            }
        }
    }


}