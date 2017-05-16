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
using Dex.Com.Expresso.Adapters.Spinner;
using EXPRESSO.Threads;
using Android.Support.V7.Widget;
using Android.Gms.Maps;
using Android.Locations;
using Dex.Com.Expresso.Utils;
using Android.Gms.Maps.Model;
using EXPRESSO.Models.Database;
using EXPRESSO.Models;
using Dex.Com.Expresso.Adapters.Listview;
using EXPRESSO.Utils;
using Dex.Com.Expresso.Dialogs;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_LiveTraffic_AIO : BaseFragment
    {
        private Spinner mSpnHighway;
        private HighwayAdapter mHighwayAdapter;
        private RecyclerView mLstItems;
        private RecyclerView.LayoutManager mLayoutManager;
        private SupportMapFragment mapFrag;
        private GoogleMap map;
        private ProgressBar prbLoading;
        private List<Marker> lstMarker = new List<Marker>();
        private LiveTrafficThread thread;
        private List<int> mLstFilter = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 , 14};
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_LiveTraffic_AIO NewInstance()
        {
            var frag1 = new Fragment_LiveTraffic_AIO { Arguments = new Bundle() };
            return frag1;
        }


       

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_livetraffic_aio, null);
            prbLoading = view.FindViewById<ProgressBar>(Resource.Id.prbLoading);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);
            mSpnHighway = view.FindViewById<Spinner>(Resource.Id.spnHighway);
            mSpnHighway.ItemSelected += MSpnHighway_ItemSelected;
            LiveTrafficThread thread = new LiveTrafficThread();
            thread.OnLoadHighwayResult += (result) =>
            {
                mHighwayAdapter = new HighwayAdapter(getActivity(), result);
                mSpnHighway.Adapter = mHighwayAdapter;
            };
            thread.loadHighway();

            mapFrag = (SupportMapFragment)ChildFragmentManager.FindFragmentById(Resource.Id.map);
            if (mapFrag != null)
            {
                map = mapFrag.Map;
                Location location = GPSUtils.getLastBestLocation(getActivity());

                if (location != null)
                {
                    //UpdateMyLocation(location);
                }
               
            }

            view.FindViewById<View>(Resource.Id.imgSetting).Click += Fragment_LiveTraffic_AIO_Click;

            

            

            return view;
        }

        private void Fragment_LiveTraffic_AIO_Click(object sender, EventArgs e)
        {
            FragmentTransaction ft = getActivity(). FragmentManager.BeginTransaction();
            Fragment prev = getActivity().FragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
            FilterLiveTrafficDialog newFragment = FilterLiveTrafficDialog.NewInstance(null, mLstFilter);
            newFragment.OnSave += (int[] result) =>
            {
                TblHighway highway = mHighwayAdapter.GetHighway(mSpnHighway.SelectedItemPosition);
                LiveTrafficThread thread = new LiveTrafficThread();
                //thread.OnLoadLiveTrafficResult += (ServiceResult resultx) =>
                //{
                //    setDataResult(resultx);
                //};
                //mLstItems.Visibility = ViewStates.Invisible;
                //prbLoading.Visibility = ViewStates.Visible;
                //mLstFilter = result.ToList();
                //thread.loadLiveTraffic(result.ToList(), highway.idHighway);
            };
            newFragment.Show(ft, "dialog");
        }

        private void setDataResult(ServiceResult result)
        {
            List<BaseItem> lstItem = result.Data as List<BaseItem>;
            LiveTrafficAdapter adapter = new LiveTrafficAdapter(getActivity(), lstItem);
            prbLoading.Visibility = ViewStates.Gone;
            mLstItems.Visibility = ViewStates.Visible;
            mLstItems.SetAdapter(adapter);

            adapter.ItemClick += Adapter_ItemClick;
            map.Clear();
            lstMarker.Clear();
         
            foreach (var bitem in lstItem)
            {
                //continue;
                try
                {
                    var item = bitem.Item as TrafficUpdate;
                    MarkerOptions option = new MarkerOptions();
                    string name = "ic_map_traffic_type" + item.intType;
                    int idResoure = ResourceUtil.GetResourceID(getActivity(), name);
                    if (idResoure != 0)
                    {
                        option.SetIcon(BitmapDescriptorFactory.FromResource(idResoure));
                    }


                    option.SetPosition(new LatLng(item.decLat, item.decLng));
                    string strTitleMarker = "";
                    if (!string.IsNullOrEmpty(item.strTitle))
                    {
                        option.SetTitle(item.strTitle);
                        strTitleMarker = item.strTitle;
                    }
                    else
                    {
                        option.SetTitle("#NA");
                        strTitleMarker = this.getActivity().GetString(Resource.String.live_traffic_notitle);
                    }
                    
                    if (string.IsNullOrEmpty(item.strDescription))
                    {
                        option.SetSnippet(this.getActivity().GetString(Resource.String.live_traffic_nodescription));
                    }
                    else
                    {
                        option.SetSnippet(item.strDescription);
                    }
                   // option.SetTitle(strTitleMarker);
                    Marker mk = map.AddMarker(option);
                    lstMarker.Add(mk);
                    LogUtils.WriteLog("Add Marker", "Title: " + (item.strTitle == null ? "NULL" : item.strTitle) + "/Lat-Lng: " + item.decLat + "-" + item.decLng);

                }
                catch (Exception ex)
                {
                    LogUtils.WriteLog("Add Marker", ex.Message);
                }
            }
            if (lstMarker.Count > 0)
            {
                Marker marker = lstMarker[0];

                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(marker.Position);
                builder.Zoom(10);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

                if (map != null)
                {
                    map.AnimateCamera(cameraUpdate);
                }
                marker.ShowInfoWindow();
            }
        }

        private void Adapter_ItemClick(object sender, BaseItem e)
        {
            int posision = (int) e.getTag(BaseItem.TagName.Position);
            Marker marker = lstMarker[posision];

            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(marker.Position);
            builder.Zoom(10);
            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            
            if (map != null)
            {
                map.AnimateCamera(cameraUpdate);
            }
            marker.ShowInfoWindow();
        }

        private void MSpnHighway_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            TblHighway highway = mHighwayAdapter.GetHighway(e.Position);
            if (highway != null)
            {
                string idHighway = highway.idHighway;
                LiveTrafficThread thread = new LiveTrafficThread();
                //thread.OnLoadLiveTrafficResult += (ServiceResult result) =>
                //{
                //    setDataResult(result);
                //};
                //mLstItems.Visibility = ViewStates.Invisible;
                //prbLoading.Visibility = ViewStates.Visible;
                //thread.loadLiveTraffic(mLstFilter , idHighway);



                FavoriteThread threadf = new FavoriteThread();
                threadf.GetFavoriteLocation(highway.idHighway);
            }
        }

        public void UpdateMyLocation(Location currentLocation)
        {
            if (map == null)
            {
                return;
            }
           
            MarkerOptions option = new MarkerOptions();
            //option.SetPosition(new LatLng(currentLocation.Latitude, currentLocation.Longitude));
            option.SetPosition(new LatLng(100.420532 , 6.510962));
            option.SetTitle("My Location");
            //option.ti
            Marker mMarker = map.AddMarker(option);
            

        }
    }
}