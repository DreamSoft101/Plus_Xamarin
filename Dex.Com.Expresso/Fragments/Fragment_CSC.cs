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
using EXPRESSO.Models.Database;
using EXPRESSO.Models;
using Android.Views.Animations;
using RecyclerViewAnimators.Animators;
using RecyclerViewAnimators.Adapters;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_CSC : BaseFragment
    {
        private RecyclerView mLstItems;
        private RecyclerView.LayoutManager mLayoutManager;
        private BaseFacilityAdapter adapter;
        private int mIntResult = 99;
        private BaseItem mCurentItem;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_CSC NewInstance()
        {
            var frag1 = new Fragment_CSC { Arguments = new Bundle() };
            return frag1;
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_list, null);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);
            var animator = new SlideInUpAnimator(new OvershootInterpolator(1f));
            mLstItems.SetItemAnimator(animator);

            //mLstItems.SetOnClickListener()
            PointOfInterestThread thread = new PointOfInterestThread();
            thread.OnLoadCSC += (result) =>
            {
                adapter = new BaseFacilityAdapter(getActivity(), result);
                var adapterAnimator = new ScaleInAnimationAdapter(adapter);
                mLstItems.SetAdapter(adapterAnimator);
                adapter.ItemClick += Adapter_ItemClick;

            };
            thread.loadCSC();
            return view;
        }

        private void Adapter_ItemClick(object sender, EXPRESSO.Models.BaseItem e)
        {
            mCurentItem = e;
            Intent intent = new Intent(getActivity(), typeof(FacilityDetailActivity));
            intent.PutExtra(FacilityDetailActivity.MODELTYPE, (int)Activities.FacilityDetailActivity.ModelType.CSC);
            intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_CSC);
            intent.PutExtra(FacilityDetailActivity.MODELISFAVORITE,(bool) e.getTag(EXPRESSO.Models.BaseItem.TagName.IsFavorite));
            intent.PutExtra(FacilityDetailActivity.MODELPOSITION, (int)e.getTag(EXPRESSO.Models.BaseItem.TagName.Position));
            intent.PutExtra(FacilityDetailActivity.MODELData, EXPRESSO.Utils.StringUtils.Object2String(e.Item as TblCSC));
            StartActivityForResult(intent, mIntResult);
            //StartActivity(intent);
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
                    thread.IsToggle(mCurentItem.Item as TblCSC);
                }
                mCurentItem.setTag(BaseItem.TagName.IsFavorite, isFavorite);
                adapter.NotifyItemChanged((int)mCurentItem.getTag(BaseItem.TagName.Position));
                
            }
        }
    }
}