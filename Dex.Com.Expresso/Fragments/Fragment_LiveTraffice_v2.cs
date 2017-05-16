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
using EXPRESSO.Models;
using Dex.Com.Expresso.Adapters.Listview;
using RecyclerViewAnimators.Animators;
using Dex.Com.Expresso.Dialogs;
using EXPRESSO.Models.Database;
using Dex.Com.Expresso.Adapters.RecyclerViews;
using EXPRESSO.Threads;
using Dex.Com.Expresso.Activities;
using Newtonsoft.Json;
using Android.Support.V4.Widget;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_LiveTraffice_v2 : BaseFragment
    {
        private RecyclerView mLstItems;
        private RecyclerView mLstFavoriteLocation;
        private RecyclerView mLstLiveTraffic;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.LayoutManager mLayoutManager1;
        private RecyclerView.LayoutManager mLayoutManager2;
        private int mIntResult = 99;
        private BaseItem mCurentItem;
        private LiveTrafficAdapter adapter;
        private ImageView imgHighwayDropdown;
        private ImageView imgFavoriteLocationDropdown;
        private ListView mLstHighway;
        private Adapters.RecyclerViews.SettingHighwayNameAdapter mHighwayFilterAdapter;
        private RecyclerView lstHighway;
        private ImageView imgLiveTrafficDropdown;
        private List<int> mLstFilter = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14 };
        private List<ImageView> mLstCheckBoxTypes = new List<ImageView>();
        private FavoriteLocationNameAdapter mFavoriteLocationAdapter;
        private SwipeRefreshLayout swipeRefreshLayout;
        private LinearLayout lnlNoData, lnlData;
        private TextView mTxtSelectHighway, mTxtSelctType, mTxtSelectFavorite;
        private LinearLayout lnlLoading;
        private LinearLayout mLnlHighway, mLnlType, mLnlFavorite;

        public delegate void onInitSearch(Filter filter);
        public onInitSearch OnInitSearch;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_LiveTraffice_v2 NewInstance()
        {
            var frag1 = new Fragment_LiveTraffice_v2 { Arguments = new Bundle() };
            return frag1;
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.exp_fragment_livetraffic, null);
            mLnlHighway = view.FindViewById<LinearLayout>(Resource.Id.lnlHighway);
            mLnlType = view.FindViewById<LinearLayout>(Resource.Id.lnlType);
            mLnlFavorite = view.FindViewById<LinearLayout>(Resource.Id.lnlFavorite);

            lnlNoData = view.FindViewById<LinearLayout>(Resource.Id.lnlNoData);
            lnlData = view.FindViewById<LinearLayout>(Resource.Id.lnlData);
            swipeRefreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
            lnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            lstHighway = view.FindViewById<RecyclerView>(Resource.Id.lstHighway);
            mLstFavoriteLocation = view.FindViewById<RecyclerView>(Resource.Id.lstFavoriteLocation);
            mLstLiveTraffic = view.FindViewById<RecyclerView>(Resource.Id.lstLiveTraffic);

            mLayoutManager2 = new LinearLayoutManager(getActivity());
            mLayoutManager = new LinearLayoutManager(getActivity(), LinearLayoutManager.Horizontal, false);
            mLayoutManager1 = new LinearLayoutManager(getActivity(), LinearLayoutManager.Horizontal, false);

            mLstFavoriteLocation.SetLayoutManager(mLayoutManager);
            lstHighway.SetLayoutManager(mLayoutManager1);
            mLstLiveTraffic.SetLayoutManager(mLayoutManager2);

            //mLstHighway.Click += MLstHighway_Click;

            mTxtSelctType = view.FindViewById<TextView>(Resource.Id.txtSelectType);
            mTxtSelectFavorite = view.FindViewById<TextView>(Resource.Id.txtSelectFavorite);
            mTxtSelectHighway = view.FindViewById<TextView>(Resource.Id.txtSelectHighway);

            imgFavoriteLocationDropdown = view.FindViewById<ImageView>(Resource.Id.imgFavoriteDropDown);
            imgHighwayDropdown = view.FindViewById<ImageView>(Resource.Id.imgHighwayDropDown);
            imgLiveTrafficDropdown = view.FindViewById<ImageView>(Resource.Id.imgTypeDropDown);
            mLnlHighway.Click += ImgHighwayDropdown_Click;
            mLnlFavorite.Click += ImgFavoriteLocationDropdown_Click;
        
            mHighwayFilterAdapter = new Adapters.RecyclerViews.SettingHighwayNameAdapter(getActivity());
            lstHighway.SetAdapter(mHighwayFilterAdapter);
            if (mHighwayFilterAdapter.ItemCount > 0)
            {
                mTxtSelectHighway.Visibility = ViewStates.Gone;
            }



            mFavoriteLocationAdapter = new FavoriteLocationNameAdapter(this.getActivity());
            mLstFavoriteLocation.SetAdapter(mFavoriteLocationAdapter);
            if (mFavoriteLocationAdapter.ItemCount > 0)
            {
                mTxtSelectFavorite.Visibility = ViewStates.Gone;
            }


            view.FindViewById<ImageView>(Resource.Id.imgTrafficType1).Tag = 1;
            mLstCheckBoxTypes.Add(view.FindViewById<ImageView>(Resource.Id.imgTrafficType1));
            view.FindViewById<ImageView>(Resource.Id.imgTrafficType2).Tag = 2;
            mLstCheckBoxTypes.Add(view.FindViewById<ImageView>(Resource.Id.imgTrafficType2));
            view.FindViewById<ImageView>(Resource.Id.imgTrafficType3).Tag = 3;
            mLstCheckBoxTypes.Add(view.FindViewById<ImageView>(Resource.Id.imgTrafficType3));
            view.FindViewById<ImageView>(Resource.Id.imgTrafficType4).Tag = 4;
            mLstCheckBoxTypes.Add(view.FindViewById<ImageView>(Resource.Id.imgTrafficType4));
            view.FindViewById<ImageView>(Resource.Id.imgTrafficType5).Tag = 5;
            mLstCheckBoxTypes.Add(view.FindViewById<ImageView>(Resource.Id.imgTrafficType5));
            view.FindViewById<ImageView>(Resource.Id.imgTrafficType6).Tag = 6;
            mLstCheckBoxTypes.Add(view.FindViewById<ImageView>(Resource.Id.imgTrafficType6));
            view.FindViewById<ImageView>(Resource.Id.imgTrafficType7).Tag = 7;
            mLstCheckBoxTypes.Add(view.FindViewById<ImageView>(Resource.Id.imgTrafficType7));
            view.FindViewById<ImageView>(Resource.Id.imgTrafficType8).Tag =8;
            mLstCheckBoxTypes.Add(view.FindViewById<ImageView>(Resource.Id.imgTrafficType8));
            view.FindViewById<ImageView>(Resource.Id.imgTrafficType9).Tag = 9;
            mLstCheckBoxTypes.Add(view.FindViewById<ImageView>(Resource.Id.imgTrafficType9));
            view.FindViewById<ImageView>(Resource.Id.imgTrafficType10).Tag = 10;
            mLstCheckBoxTypes.Add(view.FindViewById<ImageView>(Resource.Id.imgTrafficType10));
            view.FindViewById<ImageView>(Resource.Id.imgTrafficType11).Tag = 11;
            mLstCheckBoxTypes.Add(view.FindViewById<ImageView>(Resource.Id.imgTrafficType11));
            view.FindViewById<ImageView>(Resource.Id.imgTrafficType12).Tag = 12;
            mLstCheckBoxTypes.Add(view.FindViewById<ImageView>(Resource.Id.imgTrafficType12));
            view.FindViewById<ImageView>(Resource.Id.imgTrafficType13).Tag = 13;
            mLstCheckBoxTypes.Add(view.FindViewById<ImageView>(Resource.Id.imgTrafficType13));
            view.FindViewById<ImageView>(Resource.Id.imgTrafficType14).Tag = 14;
            mLstCheckBoxTypes.Add(view.FindViewById<ImageView>(Resource.Id.imgTrafficType14));
            view.FindViewById<ImageView>(Resource.Id.imgTrafficType15).Tag = 15;
            mLstCheckBoxTypes.Add(view.FindViewById<ImageView>(Resource.Id.imgTrafficType15));

            setTrafficType();
            LoadLiveTraffic();
            mLnlType.Click += ImgLiveTrafficDropdown_Click;

            view.FindViewById<View>(Resource.Id.cvHighway).Click += ImgHighwayDropdown_Click;
            view.FindViewById<View>(Resource.Id.cvFavorite).Click += ImgFavoriteLocationDropdown_Click;
            view.FindViewById<View>(Resource.Id.cvType).Click += ImgLiveTrafficDropdown_Click;

            swipeRefreshLayout.Refresh += SwipeRefreshLayout_Refresh;
            return view;
        }

        private void SwipeRefreshLayout_Refresh(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            swipeRefreshLayout.Refreshing = true;
            LoadLiveTraffic();
        }

        private void LoadLiveTraffic()
        {
            lnlNoData.Visibility = ViewStates.Gone;
            lnlData.Visibility = ViewStates.Gone;

            swipeRefreshLayout.Refreshing = true;




            LiveTrafficThread thread = new LiveTrafficThread();
            thread.OnLoadLiveTrafficResult += (ServiceResult resultx) =>
            {
                lnlLoading.Visibility = ViewStates.Gone;
                //setDataResult(resultx);

                swipeRefreshLayout.Refreshing = false;

                List<FavoriteLocation> mLocation = getActivity().getFavoriteLocation();
                
                if (resultx.intStatus == 1)
                {
                    List<BaseItem> lstItem = resultx.Data as List<BaseItem>;
                    if (lstItem.Count > 0)
                    {
                       

                        var favoritelocation = getActivity().getFavoriteLocation();
                        List<string> lstLocation = new List<string>();
                        if (favoritelocation.Count > 0)
                        {
                            foreach (var item in favoritelocation)
                            {
                                if (item.detail != null)
                                {
                                    lstLocation.AddRange(item.detail);
                                }
                            }
                            if (favoritelocation.Count > 0)
                            {
                                for (int i = 0; i < lstItem.Count; i++)
                                {
                                    if (lstItem[i].Item is TrafficUpdate)
                                    {
                                        var item = lstItem[i].Item as TrafficUpdate;
                                        if (!lstLocation.Contains(item.idHwayLocation))
                                        {
                                            lstItem.RemoveAt(i);
                                            i--;
                                        }
                                    }
                                }
                            }

                            for (int i = 1; i < lstItem.Count; i++)
                            {
                                try
                                {
                                    if (lstItem[i].Item is TblHighway)
                                    {
                                        if (lstItem[i - 1].Item is TblHighway)
                                        {
                                            lstItem.RemoveAt(i - 1);
                                            i--;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            if (lstItem.Count == 1)
                            {
                                lstItem.Clear();
                            }
                        }
                        if (lstItem.Count > 0)
                        {
                            lnlNoData.Visibility = ViewStates.Gone;
                            lnlData.Visibility = ViewStates.Visible;

                            LiveTrafficAdapter adapter = new LiveTrafficAdapter(getActivity(), lstItem);
                            adapter.ItemClick += Adapter_ItemClick1;
                            if (OnInitSearch != null)
                            {
                                OnInitSearch(adapter.Filter);
                            }
                            mLstLiveTraffic.SetAdapter(adapter);
                        }
                        else
                        {
                            lnlNoData.Visibility = ViewStates.Visible;
                            lnlData.Visibility = ViewStates.Gone;
                        }
                       

                        

                    }
                    else
                    {
                        lnlNoData.Visibility = ViewStates.Visible;
                        lnlData.Visibility = ViewStates.Gone;
                    }
                    
                }
                else
                {
                    lnlNoData.Visibility = ViewStates.Visible;
                    lnlData.Visibility = ViewStates.Gone;
                }

            };
            //mLstItems.Visibility = ViewStates.Invisible;
            //prbLoading.Visibility = ViewStates.Visible;
            var types = getActivity().getTrafficType();
            if (types.Count == 0)
            {
                types = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14 }; 
            }
            var highways = getActivity().getMySetting().Where(p => p.isEnable == true).Select(p => p.idHighway).ToList();
            if (highways.Count > 0)
            {
                thread.loadLiveTraffic(types, highways, null);
            }
            else
            {
                MastersThread threadM = new MastersThread();
                threadM.OnLoadListHighway += (List<TblHighway> result) =>
                {
                    highways = result.Select(p => p.idHighway).ToList();
                    thread.loadLiveTraffic(types, highways, null);
                };
                threadM.loadListHighway();
            }
            
        }

        private void Adapter_ItemClick1(object sender, BaseItem e)
        {
            //throw new NotImplementedException();
            var item = e.Item as TrafficUpdate;
            Intent intent = new Intent(getActivity(), typeof(LiveTrafficDetailActivity));
            intent.PutExtra(LiveTrafficDetailActivity.DATA, JsonConvert.SerializeObject(item));
            StartActivity(intent);
        }

        private void ImgFavoriteLocationDropdown_Click(object sender, EventArgs e)
        {
            FragmentTransaction ft = getActivity().FragmentManager.BeginTransaction();
            Fragment prev = getActivity().FragmentManager.FindFragmentByTag("dialog_favorite");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
            ChooseFavoriteLocationDialog newFragment = ChooseFavoriteLocationDialog.NewInstance(null);
            newFragment.OnDismess += () =>
            {
                //mHighwayFilterAdapter = new Adapters.RecyclerViews.SettingHighwayNameAdapter(getActivity());
                //lstHighway.SetAdapter(mHighwayFilterAdapter);
                mFavoriteLocationAdapter = new FavoriteLocationNameAdapter(this.getActivity());
                if (mFavoriteLocationAdapter.ItemCount  > 0)
                {
                    mTxtSelectFavorite.Visibility = ViewStates.Gone;
                }
                else
                {
                    mTxtSelectFavorite.Visibility = ViewStates.Visible;
                }
                mLstFavoriteLocation.SetAdapter(mFavoriteLocationAdapter);
                LoadLiveTraffic();
            };
            newFragment.Show(ft, "dialog_favorite");
        }

        private void ImgLiveTrafficDropdown_Click(object sender, EventArgs e)
        {
            FragmentTransaction ft = getActivity().FragmentManager.BeginTransaction();
            Fragment prev = getActivity().FragmentManager.FindFragmentByTag("dialog_type");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
            FilterLiveTrafficDialog newFragment = FilterLiveTrafficDialog.NewInstance(null, mLstFilter);
            newFragment.OnSave += (int[] result) =>
            {
                getActivity().saveTrafficType(result.ToList());
                setTrafficType();
                LoadLiveTraffic();
            };
            newFragment.Show(ft, "dialog_type");
        }

        private void setTrafficType()
        {
            mLstFilter = getActivity().getTrafficType();

            if (mLstFilter.Count > 0)
            {
                mTxtSelctType.Visibility = ViewStates.Gone;
            }
            else
            {
                mTxtSelctType.Visibility = ViewStates.Visible;
            }

            foreach (var item in mLstCheckBoxTypes)
            {
                item.Visibility = ViewStates.Gone;
            }
            foreach(var item in mLstFilter)
            {
                var checkbox = mLstCheckBoxTypes.Where(p => p.Tag.ToString() == item.ToString()).FirstOrDefault();
                if (checkbox != null)
                {
                    checkbox.Visibility = ViewStates.Visible;
                }
                else
                {
                   // checkbox.Visibility = ViewStates.Gone;
                }
            }
        }

        private void ImgHighwayDropdown_Click(object sender, EventArgs e)
        {
            FragmentTransaction ft = getActivity().FragmentManager.BeginTransaction();
            Fragment prev = getActivity().FragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
            ChooseHighwayDialog newFragment = ChooseHighwayDialog.NewInstance(null);
            newFragment.OnDismess += () =>
            {
                mHighwayFilterAdapter = new Adapters.RecyclerViews.SettingHighwayNameAdapter(getActivity());
                lstHighway.SetAdapter(mHighwayFilterAdapter);

                if (mHighwayFilterAdapter.ItemCount > 0)
                {
                    mTxtSelectHighway.Visibility = ViewStates.Gone;
                }
                else
                {
                    mTxtSelectHighway.Visibility = ViewStates.Visible;
                }

                LoadLiveTraffic();
            };
            newFragment.Show(ft, "dialog");
        }


        private void Adapter_ItemClick(object sender, EXPRESSO.Models.BaseItem e)
        {
            mCurentItem = e;
            //Intent intent = new Intent(getActivity(), typeof(FacilityDetailActivity));
            //intent.PutExtra(FacilityDetailActivity.MODELTYPE, (int)Activities.FacilityDetailActivity.ModelType.CSC);
            //intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_CSC);
            //intent.PutExtra(FacilityDetailActivity.MODELISFAVORITE, (bool)e.getTag(EXPRESSO.Models.BaseItem.TagName.IsFavorite));
            //intent.PutExtra(FacilityDetailActivity.MODELPOSITION, (int)e.getTag(EXPRESSO.Models.BaseItem.TagName.Position));
            //intent.PutExtra(FacilityDetailActivity.MODELData, EXPRESSO.Utils.StringUtils.Object2String(e.Item as TblCSC));
            //StartActivityForResult(intent, mIntResult);
            //StartActivity(intent);
        }
        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            //base.OnActivityResult(requestCode, resultCode, data);
            //if (requestCode == mIntResult)
            //{
            //    bool isFavorite = data.GetBooleanExtra(FacilityDetailActivity.MODELISFAVORITE, false);
            //    mCurentItem.setTag(BaseItem.TagName.IsFavorite, isFavorite);
            //    adapter.NotifyItemChanged((int)mCurentItem.getTag(BaseItem.TagName.Position));
            //}
        }
    }
}
 