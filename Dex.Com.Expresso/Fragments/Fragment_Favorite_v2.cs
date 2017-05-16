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
using static Android.Support.V7.Widget.RecyclerView;
using Dex.Com.Expresso.Adapters.RecyclerViews;
using EXPRESSO.Threads;
using EXPRESSO.Models;
using EXPRESSO.Models.Database;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Favorite_v2 : BaseFragment
    {
        public RecyclerView mRecyclerView;
        public FavoriteAdapters mAdapter;
        public LayoutManager mLnlLayoutManager;
        public LinearLayout mLnlLoading, mLnlData;
        public int mIntRequestCode = 999;

      

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLnlLayoutManager = new LinearLayoutManager(this.getActivity());
            // Create your fragment here
        }

        public static Fragment_Favorite_v2 NewInstance()
        {
            var frag1 = new Fragment_Favorite_v2 { Arguments = new Bundle() };
            return frag1;
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.exp_fragment_favorite_v2, null);
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mRecyclerView.SetLayoutManager(mLnlLayoutManager);
            mLnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            mLnlData = view.FindViewById<LinearLayout>(Resource.Id.lnlData);
            LoadData();
            return view;
        }


        private void LoadData()
        {
            mLnlLoading.Visibility = ViewStates.Visible;
            mLnlData.Visibility = ViewStates.Gone;

            FavoriteThread thread = new FavoriteThread();
            thread.OnGetFavoriteItem += (List<BaseItem> result) =>
            {
                mLnlLoading.Visibility = ViewStates.Gone;
                mLnlData.Visibility = ViewStates.Visible;

                var listFavoriteLocation = this.getActivity().getFavoriteLocation();
                if (listFavoriteLocation.Count > 0)
                {

                    FavoriteHeader header = new FavoriteHeader();
                    header.strType = "Live traffic";
                    BaseItem itemheader = new BaseItem();
                    itemheader.Item = header;
                    result[0] = itemheader;
                }

                mAdapter = new FavoriteAdapters(this.getActivity(), result);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.OnStartActivity += StartAc;

                if (listFavoriteLocation.Count > 0)
                {

                    List<BaseItem> trafficDetails = new List<BaseItem>();

                    LiveTrafficThread trafficThread = new LiveTrafficThread();
                    var types = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14 };
                    MastersThread threadM = new MastersThread();
                    threadM.OnLoadListHighway += (List<TblHighway> resultx) =>
                    {
                        var highways = resultx.Select(p => p.idHighway).ToList();
                        trafficThread.OnLoadLiveTrafficResult += (ServiceResult resulttraffic) =>
                        {
                            if (resulttraffic.intStatus == 1)
                            {
                                List<TrafficUpdate> lstTraffic = resulttraffic.Data as List<TrafficUpdate>;
                                foreach (var itemlocation in listFavoriteLocation)
                                {
                                    var lst = lstTraffic.Where(p => itemlocation.detail.Contains(p.idHwayLocation));
                                    if (lst.Count() > 0)
                                    {
                                        BaseItem iteml = new BaseItem() { Item = itemlocation };
                                        trafficDetails.Add(iteml);
                                        foreach (var detail in lst)
                                        {
                                            trafficDetails.Add(new BaseItem() { Item = detail });
                                        }
                                    }
                                }

                                if (trafficDetails.Count > 0)
                                {
                                    //trafficDetails

                                    //result.Add(itemheader);

                                    TrafficUpdateFavorite traffics = new TrafficUpdateFavorite() { TrafficUpdate = trafficDetails };
                                    BaseItem itemdetail = new BaseItem();
                                    itemdetail.Item = traffics;
                                    //result.Add(item);

                                    mAdapter.UpdateTrafficUpdate(itemdetail);
                                }
                                else
                                {
                                    mAdapter.HideTrafficUpdate();
                                }
                            }
                        };
                        trafficThread.GetLiveTraffic(types, highways, null);
                    };
                    threadM.loadListHighway();
                }

                LiveFeedFavorite livefeed = null;
                foreach (var item in result)
                {
                    if (item != null)
                    {
                        if (item.Item is LiveFeedFavorite)
                        {
                            livefeed = item.Item as LiveFeedFavorite;
                            break;
                        }
                    }
                  
                }

                //var livefeed = result.Where(p => p.Item is LiveFeedFavorite).FirstOrDefault();
                if (livefeed != null)
                {
                    var livefeedfa = livefeed;// livefeed.Item as LiveFeedFavorite;
                    if (livefeedfa.LiveFeed.Where(p => p.Item is TrafficUpdate).Count() > 0)
                    {
                        List<BaseItem> trafficDetails = new List<BaseItem>();

                        LiveTrafficThread trafficThread = new LiveTrafficThread();
                        var types = new List<int>() { 13 };
                        MastersThread threadM = new MastersThread();
                        threadM.OnLoadListHighway += (List<TblHighway> resultx) =>
                        {
                            var highways = resultx.Select(p => p.idHighway).ToList();
                            trafficThread.OnLoadLiveTrafficResult += (ServiceResult resulttraffic) =>
                            {
                                if (resulttraffic.intStatus == 1)
                                {
                                    List<TrafficUpdate> lstTraffic = resulttraffic.Data as List<TrafficUpdate>;
                                    mAdapter.UpdateCCTV(lstTraffic);
                                }
                            };
                            trafficThread.GetLiveTraffic(types, highways, null);
                        };
                        threadM.loadListHighway();
                    }
                    if (livefeedfa.LiveFeed.Where(p => p.Item is TollPlazaCCTV).Count() > 0)
                    {
                        MastersThread threadM = new MastersThread();
                        threadM.OnLoadListHighway += (List<TblHighway> resultx) =>
                        {
                            var highways = resultx.Select(p => p.idHighway).ToList();
                            LiveTrafficThread threadtr = new LiveTrafficThread();
                            threadtr.OnLoadFacilityCCTV += (EXPRESSO.Models.ServiceResult resultxx) =>
                            {
                                if (resultxx.intStatus == 1)
                                {
                                    List<EXPRESSO.Models.BaseItem> lstItem = resultxx.Data as List<EXPRESSO.Models.BaseItem>;
                                    var items = lstItem.Where(p => p.Item is EXPRESSO.Models.TollPlazaCCTV).ToList();//


                                    List<TollPlazaCCTV> tolls = new List<TollPlazaCCTV>();
                                    foreach (var item in items)
                                    {
                                        tolls.Add(item.Item as TollPlazaCCTV);
                                    }
                                    mAdapter.UpdateCCTV(tolls);
                                }
                            };
                            threadtr.loadFacilityCCTV(highways, 2);
                        };
                        threadM.loadListHighway();
                    }
                }


            };
            thread.GetAllItemFavorite();
        }

        private void StartAc(Intent intent)
        {
            StartActivityForResult(intent, mIntRequestCode);

        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (mAdapter != null)
            {
                bool isFavorite = data.GetBooleanExtra("ISFAVORITE", false);
                mAdapter.ReUpdateIsFavorite(isFavorite);
            }
            //ReUpdateIsFavorite
        }

        
    }
}