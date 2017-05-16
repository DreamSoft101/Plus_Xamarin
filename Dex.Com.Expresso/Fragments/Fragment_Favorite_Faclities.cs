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
using static EXPRESSO.Models.EnumType;
using Android.Support.V7.Widget;
using EXPRESSO.Models;
using Dex.Com.Expresso.Adapters.Listview;
using Dex.Com.Expresso.Activities;
using EXPRESSO.Threads;
using RecyclerViewAnimators.Adapters;
using EXPRESSO.Models.Database;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Favorite_Faclities : BaseFragment
    {
        private FacilitiesType mOtherTypes;
        private RecyclerView mLstItems;
        RecyclerView.LayoutManager mLayoutManager;
        private int mIntResult = 99;
        private BaseItem mCurentItem;
        private BaseFacilityAdapter baseFacilityAdapter;
        private RSAAdapter rSAAdapter;



        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_Favorite_Faclities NewInstance()
        {

            var frag1 = new Fragment_Favorite_Faclities {  };
            return frag1;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_list, null);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);
            FavoriteThread thread = new FavoriteThread();
            List<BaseItem> lstItem;
            thread.OnLoadFacilities += (result) =>
            {
                lstItem = result;
                baseFacilityAdapter = new BaseFacilityAdapter(getActivity(), lstItem);
                baseFacilityAdapter.ItemClick += Adapter_ItemClick;
                var adapterAnimator = new ScaleInAnimationAdapter(baseFacilityAdapter);
                mLstItems.SetAdapter(adapterAnimator);
            };
            thread.loadFacilities();
            return view;
        }


        private void Adapter_ItemClick(object sender, BaseItem e)
        {
            mCurentItem = e;
            Intent intent = new Intent(getActivity(), typeof(FacilityDetailActivity));
            intent.PutExtra(FacilityDetailActivity.MODELTYPE, (int)FacilityDetailActivity.ModelType.Facility);
            //intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.model_fa);
            intent.PutExtra(FacilityDetailActivity.MODELISFAVORITE, (bool)e.getTag(EXPRESSO.Models.BaseItem.TagName.IsFavorite));
            intent.PutExtra(FacilityDetailActivity.MODELData, EXPRESSO.Utils.StringUtils.Object2String(e.Item as TblFacilities));
            StartActivityForResult(intent, mIntResult);
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == mIntResult)
            {
                bool isFavorite = data.GetBooleanExtra(FacilityDetailActivity.MODELISFAVORITE, false);
                mCurentItem.setTag(BaseItem.TagName.IsFavorite, isFavorite);
                if (rSAAdapter != null)
                {
                    rSAAdapter.NotifyItemChanged((int)mCurentItem.getTag(BaseItem.TagName.Position));
                }
                if (baseFacilityAdapter != null)
                {
                    baseFacilityAdapter.NotifyItemChanged((int)mCurentItem.getTag(BaseItem.TagName.Position));
                }
                // adapter.NotifyItemChanged((int)mCurentItem.getTag(BaseItem.TagName.Position));
            }
        }
    }
}