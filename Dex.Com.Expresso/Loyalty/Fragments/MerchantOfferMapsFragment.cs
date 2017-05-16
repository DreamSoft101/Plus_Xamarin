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
using Android.Locations;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Loyalty.Threads;
using Loyalty.Models.Database;
using Loyalty.Models.ServiceOutput;
using Dex.Com.Expresso.Loyalty.Droid.Utils;
using Square.Picasso;
using Android.Support.V4.Content;
using Dex.Com.Expresso.Loyalty.Droid.Dialogs;
using Loyalty.Models;
using Android.Graphics;
using Newtonsoft.Json;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Fragments
{
    public class MerchantOfferMapsFragment : BaseFragment , GoogleMap.IOnMarkerClickListener
    {
        private Location mCurrentLocation;
        private SupportMapFragment mapFrag;
        private GoogleMap map;
        private Marker mMarker;
        List<Merchant> lstMerchant = null;
        List<Document> lstDocument = null;
        List<MerchantLocation> lstLocation = null;
        private List<Document> mLstDocument;
        List<Favorites> lstFavorite = null;
        private List<MemberGroupDetail> mLstMemberGroupDetail;
        private List<MemberGroup> mLstMemberGroup;

        List<MerchantProductMemberType> lstProductMemberType = null;
        List<MerchantProduct> lstItem;
        List<Marker> mlstMarker = new List<Marker>();
        private Marker myMarker;
        Dictionary<string, MerchantProductMemberType> lstMarkerOffer = new Dictionary<string, MerchantProductMemberType>();
        Dictionary<string, MerchantLocation> lstMarkerLocation = new Dictionary<string, MerchantLocation>();
        Dictionary<string, MerchantProduct> lstMerkerProduct = new Dictionary<string, MerchantProduct>();

        private BaseItem mBaseItemFilter;
        
        public static MerchantOfferMapsFragment NewInstance()
        {
            var frag1 = new MerchantOfferMapsFragment();
            return frag1;
        }

        public void Filter(BaseItem item)
        {
            if (item == mBaseItemFilter)
            {
                return;
            }
            mBaseItemFilter = item;

            //GenerateData();
            Merchant merchantfilter = null;
            MerchantLocation merchantlocationfilter = null;
            MerchantProduct merchantproduct = null;
            if (mBaseItemFilter != null)
            {
                if (mBaseItemFilter.Item is Merchant)
                {
                    merchantfilter = mBaseItemFilter.Item as Merchant;
                }
                else if (mBaseItemFilter.Item is MerchantLocation)
                {
                    merchantlocationfilter = mBaseItemFilter.Item as MerchantLocation;
                }
                else if (mBaseItemFilter.Item is MerchantProduct)
                {
                    merchantproduct = mBaseItemFilter.Item as MerchantProduct;
                }
            }

            foreach (var marker in mlstMarker)
            {
                marker.Visible = true;
                var product = lstMerkerProduct[marker.Id];
                var location = lstMarkerLocation[marker.Id];
                var merchant = lstMarkerOffer[marker.Id];

                if (merchantfilter != null)
                {
                    if (merchantfilter.MerchantID != product.MerchantID)
                    {
                        marker.Visible = false;
                        continue;
                    }
                }
                if (merchantlocationfilter != null)
                {
                    if (merchantlocationfilter.MerchantId != location.MerchantId)
                    {
                        marker.Visible = false;
                        continue;
                    }
                }

                if (merchantproduct != null)
                {
                    if (merchantproduct.MerchantProductID != product.MerchantProductID)
                    {
                        marker.Visible = false;
                        continue;
                    }
                }
            }

            

            
            

            if (merchantlocationfilter != null)
            {
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(new LatLng(double.Parse(merchantlocationfilter.strLat), double.Parse(merchantlocationfilter.strLng)));
                builder.Zoom(15);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                if (map != null)
                {
                    map.AnimateCamera(cameraUpdate);
                }
            }
            else
            {
                if (item == null)
                {
                    return;
                }
                double distince = int.MaxValue;
                var nearMarker = myMarker;
                foreach (var marker in mlstMarker)
                {
                    if (marker.Visible)
                    {
                        double dis = GPSUtils.Distance(myMarker.Position, marker.Position, GPSUtils.DistanceUnit.Kilometers);
                        if (distince > dis)
                        {
                            distince = dis;
                            nearMarker = marker;
                        }
                    }
                }

                if (nearMarker != myMarker)
                {
                    CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                    builder.Target(nearMarker.Position);
                    builder.Zoom(15);
                    CameraPosition cameraPosition = builder.Build();
                    CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                    if (map != null)
                    {
                        map.AnimateCamera(cameraUpdate);
                    }
                }
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_merchantoffer_maps, null);

            mapFrag = (SupportMapFragment)ChildFragmentManager.FindFragmentById(Resource.Id.map);
            if (mapFrag != null)
            {
                map = mapFrag.Map;
                map.SetOnMarkerClickListener(this);
            }


            MerchantThreads thread = new MerchantThreads();
           
            //FavoriteThreads thread = new FavoriteThreads();

            thread.OnResult += (ServiceResult result) =>
            {
                if (result.Data is List<MerchantProduct>)
                {
                    lstItem = result.Data as List<MerchantProduct>;
                    GenerateData();
                }
                else if (result.Data is List<Document>)
                {
                    lstDocument = result.Data as List<Document>;
                }
                else if (result.Data is List<Merchant>)
                {
                    lstMerchant = result.Data as List<Merchant>;
                }
                else if (result.Data is List<MerchantLocation>)
                {
                    lstLocation = result.Data as List<MerchantLocation>;
                }
                else if (result.Data is List<Favorites>)
                {
                    lstFavorite = result.Data as List<Favorites>;
                }
                else if (result.Data is List<MerchantProductMemberType>)
                {
                    lstProductMemberType = result.Data as List<MerchantProductMemberType>;
                }
                else if (result.Data is List<MemberGroupDetail>)
                {
                    mLstMemberGroupDetail = result.Data as List<MemberGroupDetail>;
                }
                else if (result.Data is List<MemberGroup>)
                {
                    mLstMemberGroup = result.Data as List<MemberGroup>;
                }

            };
            thread.HomMerchantOffer();

            
            view.FindViewById<ImageView>(Resource.Id.imgShowHide).Click += ShowHideMyMarkerClick;
            view.FindViewById<ImageView>(Resource.Id.imgMyLocation).Click += MyLocationClick;
            return view;
        }

        private void MyLocationClick(object sender, EventArgs e)
        {
            if (myMarker != null)
            {
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(myMarker.Position);
                builder.Zoom(15);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                
                if (map != null)
                {
                    map.AnimateCamera(cameraUpdate);
                }
            }
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            System.GC.Collect();
        }

        private void ShowHideMyMarkerClick(object sender, EventArgs e)
        {
            ImageView view = sender as ImageView;
            myMarker.Visible = !myMarker.Visible;
            if (myMarker.Visible)
            {
                view.SetImageResource(Resource.Drawable.loy_my_location);
            }
            else
            {
                view.SetImageResource(Resource.Drawable.loy_my_location_hide);
            }
        }

        private void GenerateData()
        {
           
            lstMarkerOffer.Clear();
            lstMarkerLocation.Clear();
            map.Clear();
            
            Location myLocation = GPSUtils.getLastBestLocation(getActivity());
            if (myLocation != null)
            {
                MarkerOptions option = new MarkerOptions();
                option.SetPosition(new LatLng(myLocation.Latitude, myLocation.Longitude));
                option.SetTitle(GetString(Resource.String.loy_my_location));
                option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.loy_my_location));
                myMarker = map.AddMarker(option);
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(new LatLng(myLocation.Latitude, myLocation.Longitude));
                builder.Zoom(15);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                if (map != null)
                {
                    map.AnimateCamera(cameraUpdate);
                }
            }


           
            for (int i=0; i < lstMerchant.Count; i ++)
            {
               
                var merchant = lstMerchant[i];
               
                
                var lstLocation = this.lstLocation.Where(p => p.MerchantId == merchant.MerchantID).ToList();
                for (int j = 0; j < lstLocation.Count; j ++)
                {
                    var location = lstLocation[j];
                   
                    var lstMerchantProduct = this.lstItem.Where(p => p.MerchantID == merchant.MerchantID).Select(p => p.MerchantProductID).ToList();
                    var offer = this.lstProductMemberType.Where(p => lstMerchantProduct.Contains(p.idMerchantProduct)).OrderByDescending(p => p.decOffer).FirstOrDefault();
                    if (offer != null)
                    {
                        var product = this.lstItem.Where(p => p.MerchantProductID == offer.idMerchantProduct).FirstOrDefault();

                     

                        View markerView = ((LayoutInflater)this.Activity.GetSystemService(Context.LayoutInflaterService)).Inflate(Resource.Layout.loy_item_marker, null);
                        markerView.FindViewById<TextView>(Resource.Id.txtOffer).Text = String.Format(GetString(Resource.String.loy_format_offer), offer.decOffer);
                        markerView.FindViewById<TextView>(Resource.Id.txtLocationName).Text = product.ProductName; //location.strLocationName.Length > 15 ? location.strLocationName.Substring(0,12) + "..." : location.strLocationName;


                        if (offer.decOffer >= 25)
                        {
                            markerView.FindViewById<TextView>(Resource.Id.txtOffer).SetTextColor(Resources.GetColor(Resource.Color.loy_offer_high));
                        }
                        else if (offer.decOffer >= 15)
                        {
                            markerView.FindViewById<TextView>(Resource.Id.txtOffer).SetTextColor(Resources.GetColor(Resource.Color.loy_offer_normal));
                        }
                        else
                        {
                            markerView.FindViewById<TextView>(Resource.Id.txtOffer).SetTextColor(Resources.GetColor(Resource.Color.loy_offer_low));
                        }



                        MarkerOptions option = new MarkerOptions();
                        option.SetPosition(new LatLng(Double.Parse(location.strLat), Double.Parse(location.strLng)));
                        option.SetTitle(location.strLocationName);
                        Bitmap bitmap = ImageUtils.CreateDrawableFromView(getActivity(), markerView);
                        option.SetIcon(BitmapDescriptorFactory.FromBitmap(bitmap));
                        var mk = map.AddMarker(option);

                        lstMarkerOffer.Add(mk.Id, offer);
                        lstMarkerLocation.Add(mk.Id, location);
                        lstMerkerProduct.Add(mk.Id, product);
                        mlstMarker.Add(mk);
                        //mk. = offer.idMerchantProduct.ToString();
                    }
                    else
                    {
                       

                    }
                }
            }
            
        }
        
        public bool OnMarkerClick(Marker marker)
        {
            //throw new NotImplementedException();
            var offer = lstMarkerOffer[marker.Id];
            var location = lstMarkerLocation[marker.Id];
            if(offer != null)
            {
                List<Document> lstdocument = null;
                var product = lstItem.Where(p => p.MerchantProductID == offer.idMerchantProduct).FirstOrDefault();
                var merchant = lstMerchant.Where(p => p.MerchantID == product.MerchantID).FirstOrDefault();
                var document = lstDocument.Where(p => p.ID == product.MainImageID).FirstOrDefault();
                var favorite = lstFavorite.Where(p => p.ID == product.MerchantID && p.intType == Favorites.intMerchantProduct).FirstOrDefault();


                if (offer != null)
                {
                    var listIDGroup = mLstMemberGroupDetail.Where(p => p.MemberTypeID == offer.idMemberType).Select(p => p.MemberGroupID).ToList();
                    var lstgroup = mLstMemberGroup.Where(p => listIDGroup.Contains(p.MemberGroupID) && p.idDocument != null).Select(p => p.idDocument).ToList();
                    lstdocument = lstDocument.Where(p => lstgroup.Contains(p.ID)).ToList();
                }


                FragmentTransaction ft = getActivity().FragmentManager.BeginTransaction();
                Fragment prev = getActivity().FragmentManager.FindFragmentByTag("popup");
                if (prev != null)
                {
                    //UpdateDialog newFragment = (UpdateDialog)prev;
                    //newFragment.Show(ft, "update");
                    //ft.Remove(prev);
                }
                else
                {
                    ft.AddToBackStack(null);
                    MerchantOfferDialog newFragment = MerchantOfferDialog.NewInstance(null, product,offer,location,document,merchant, favorite, lstdocument);
                    newFragment.Show(ft, "popup");

                }
            }
            return true;
        }
    }
}