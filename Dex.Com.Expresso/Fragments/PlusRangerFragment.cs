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
using Android.Support.V7.Widget;
using Dex.Com.Expresso.Adapters.Listview;
using Android.Views.Animations;
using EXPRESSO.Threads;
using static EXPRESSO.Models.EnumType;
using Dex.Com.Expresso.Activities;
using RecyclerViewAnimators.Animators;
using RecyclerViewAnimators.Adapters;
using EXPRESSO.Models.Database;

namespace Dex.Com.Expresso.Fragments
{
    public class PlusRangerFragment : BaseFragment
    {
        private int mIntResult = 99;
        private BaseItem mCurentItem;
        private RecyclerView mLstItems;
        RecyclerView.LayoutManager mLayoutManager;
        private RSAAdapter adapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_Favorite_RSA NewInstance()
        {
            var frag1 = new Fragment_Favorite_RSA { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_list, null);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);

            var animator = new FadeInAnimator(new OvershootInterpolator(1f));
            mLstItems.SetItemAnimator(animator);
            mLstItems.SetLayoutManager(mLayoutManager);
            FavoriteThread thread = new FavoriteThread();
            thread.OnLoadRSA += (result) =>
            {
                adapter = new RSAAdapter(getActivity(), result);
                var adapterAnimator = new ScaleInAnimationAdapter(adapter);
                mLstItems.SetAdapter(adapterAnimator);
                adapter.ItemClick += Adapter_ItemClick;

            };
            thread.loadRSA(FavoriteType.RSA);


            return view;
        }

        private void Adapter_ItemClick(object sender, BaseItem e)
        {
            mCurentItem = e;
            Intent intent = new Intent(getActivity(), typeof(FacilityDetailActivity));
            intent.PutExtra(FacilityDetailActivity.MODELTYPE, (int)FacilityDetailActivity.ModelType.RSA);
            intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_RSA);
            intent.PutExtra(FacilityDetailActivity.MODELISFAVORITE, (bool)e.getTag(EXPRESSO.Models.BaseItem.TagName.IsFavorite));
            intent.PutExtra(FacilityDetailActivity.MODELData, EXPRESSO.Utils.StringUtils.Object2String(e.Item as TblRSA));
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
                    thread.IsToggle(mCurentItem.Item as TblRSA);
                }
                mCurentItem.setTag(BaseItem.TagName.IsFavorite, isFavorite);
                adapter.NotifyItemChanged((int)mCurentItem.getTag(BaseItem.TagName.Position));

            }
        }
    }
}