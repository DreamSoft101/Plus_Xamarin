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
using Android.Support.V4.View;
using EXPRESSO.Threads;
using Dex.Com.Expresso.Adapters.Viewpager;
using EXPRESSO.Models;
using Dex.Com.Expresso.Adapters.RecyclerViews;
using Android.Support.V7.Widget;
using Dex.Com.Expresso.Activities;
using Newtonsoft.Json;
using Android.Graphics;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Rsa_FoodSignature : BaseFragment
    {
        private TblRSA mTblRSA;
        private ViewPager mVppTab;
        private RecyclerView mLstItems;
        private RecyclerView.LayoutManager mLayoutManager;
        private LinearLayout mLnlLoading;
        private ImageAdapter adapter;
        private ImagesAdapter adapterV;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static Fragment_Rsa_FoodSignature NewInstance(TblRSA item)
        {
            var frag1 = new Fragment_Rsa_FoodSignature { mTblRSA = item };
            return frag1;
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_facility_detail_cctv, null);
            mVppTab = view.FindViewById<ViewPager>(Resource.Id.pager);

            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);

            mLayoutManager = new LinearLayoutManager(getActivity(), LinearLayoutManager.Horizontal, false);

            mLstItems.SetLayoutManager(mLayoutManager);

            mLnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);

            PointOfInterestThread thread = new PointOfInterestThread();
            thread.OnGetFacilityImageRSA += (List<TblFacilityImage> result) =>
            {
                mLnlLoading.Visibility = ViewStates.Gone;

                var lstImage = result.Select(p => p.strPicture).ToList();

                adapter = new ImageAdapter(this.getActivity(), lstImage);
                mLstItems.SetAdapter(adapter);
                adapter.ItemClick += Adapter_ItemClick;
                adapterV = new ImagesAdapter(this.getActivity(), this.getActivity().SupportFragmentManager, lstImage);
                mVppTab.Adapter = adapterV;
                adapterV.OnClick += (int pos) =>
                {
                    Intent intent = new Intent(this.getActivity(), typeof(ImageViewerActivity));
                    intent.PutExtra(ImageViewerActivity.DATA, JsonConvert.SerializeObject(lstImage));
                    intent.PutExtra(ImageViewerActivity.DATA_DESCRIPTION, mTblRSA.strName);
                    intent.PutExtra(ImageViewerActivity.DATA_POSITION, pos);
                    intent.PutExtra(ImageViewerActivity.DATA_TITLE, mTblRSA.strName);
                    StartActivity(intent);
                };
            };
            thread.LoadFacilityImagesRSA(mTblRSA.idRSA);

            return view;
        }

        public Bitmap GetBitmap()
        {
            return adapterV.GetBitmap(mVppTab.CurrentItem);
        }

        private void Adapter_ItemClick(object sender, int e)
        {
            //throw new NotImplementedException();
            mVppTab.CurrentItem = e;
            //mLstItems.sets
            //mLstItems.SmoothScrollToPosition(e);
        }
    }
}