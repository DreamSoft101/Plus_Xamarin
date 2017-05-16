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
using Dex.Com.Expresso.Activities;
using EXPRESSO.Models.Database;
using Android.Support.V7.Widget;
using Dex.Com.Expresso.Adapters.Listview;
using EXPRESSO.Threads;
using RecyclerViewAnimators.Adapters;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Favorites_PertrolStation : BaseFragment
    {
        private int mIntResult = 99;
        private BaseItem mCurentItem;
        private RecyclerView mLstItems;
        RecyclerView.LayoutManager mLayoutManager;
        private BaseFacilityAdapter adapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_Favorites_PertrolStation NewInstance()
        {
            var frag1 = new Fragment_Favorites_PertrolStation { Arguments = new Bundle() };
            return frag1;
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_list, null);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);
            FavoriteThread thread = new FavoriteThread();
            thread.OnLoadPertrolStation += (result) =>
            {
                adapter = new BaseFacilityAdapter(getActivity(), result);
                var adapterAnimator = new ScaleInAnimationAdapter(adapter);
                mLstItems.SetAdapter(adapterAnimator);
                adapter.ItemClick += Adapter_ItemClick;
            };
            thread.loadPertrolStation();
            return view;
        }

        private void Adapter_ItemClick(object sender, EXPRESSO.Models.BaseItem e)
        {
            mCurentItem = e;
            Intent intent = new Intent(getActivity(), typeof(FacilityDetailActivity));
            intent.PutExtra(FacilityDetailActivity.MODELTYPE, (int)Activities.FacilityDetailActivity.ModelType.Petrol);
            intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_PETROLSTATION);
            intent.PutExtra(FacilityDetailActivity.MODELData, EXPRESSO.Utils.StringUtils.Object2String(e.Item as TblPetrolStation));
            intent.PutExtra(FacilityDetailActivity.MODELISFAVORITE, (bool)e.getTag(EXPRESSO.Models.BaseItem.TagName.IsFavorite));
            StartActivityForResult(intent, mIntResult);
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == mIntResult)
            {
                bool isFavorite = data.GetBooleanExtra(FacilityDetailActivity.MODELISFAVORITE, false);
                if (isFavorite != (bool)mCurentItem.getTag(BaseItem.TagName.IsFavorite))
                {
                    FavoriteThread thread = new FavoriteThread();
                    thread.IsToggle(mCurentItem.Item as TblPetrolStation);
                }
                mCurentItem.setTag(BaseItem.TagName.IsFavorite, isFavorite);
                adapter.NotifyItemChanged((int)mCurentItem.getTag(BaseItem.TagName.Position));
            }
        }
    }
}