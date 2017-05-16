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
using Android.Support.V7.Widget;
using EXPRESSO.Threads;
using Dex.Com.Expresso.Adapters.Listview;
using Dex.Com.Expresso.Activities;
using EXPRESSO.Utils;
using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using RecyclerViewAnimators.Adapters;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Nearby : BaseFragment
    {
        private int mIntResult = 99;
        private BaseItem mCurentItem;
        private string mStrCategoryName;
        private string mIdCategory;
        private RecyclerView mLstItems;
        RecyclerView.LayoutManager mLayoutManager;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_Nearby NewInstance(string type,string name)
        {
            var frag1 = new Fragment_Nearby { mIdCategory = type, mStrCategoryName = name };
            return frag1;
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_list, null);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);
            PointOfInterestThread thread = new PointOfInterestThread();
            thread.OnLoadNearBy += (result) =>
            {
                NearbyAdapter adapter = new NearbyAdapter(getActivity(), result);
                var adapterAnimator = new ScaleInAnimationAdapter(adapter);
                mLstItems.SetAdapter(adapterAnimator);
                adapter.ItemClick += Adapter_ItemClick;

            };
            thread.loadNearby(mIdCategory);
            return view;
        }

        private void Adapter_ItemClick(object sender, BaseItem e)
        {
            mCurentItem = e;
            Intent intent = new Intent(getActivity(), typeof(NearbyDetailActivity));
            intent.PutExtra(NearbyDetailActivity.ModelData, StringUtils.Object2String(e.Item as TblNearby));
            intent.PutExtra(NearbyDetailActivity.ModelType,  mStrCategoryName);
            intent.PutExtra(NearbyDetailActivity.MODELISFAVORITE, (bool)e.getTag(EXPRESSO.Models.BaseItem.TagName.IsFavorite));
            StartActivityForResult(intent, mIntResult);
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == mIntResult)
            {
                bool isFavorite = data.GetBooleanExtra(NearbyDetailActivity.MODELISFAVORITE, false);
                if (isFavorite != (bool)mCurentItem.getTag(BaseItem.TagName.IsFavorite))
                {
                    FavoriteThread thread = new FavoriteThread();
                    thread.IsToggle(mCurentItem.Item as TblNearby);
                }
                mCurentItem.setTag(BaseItem.TagName.IsFavorite, isFavorite);
                mLstItems.GetAdapter().NotifyItemChanged((int)mCurentItem.getTag(BaseItem.TagName.Position));
            }
        }
    }
}