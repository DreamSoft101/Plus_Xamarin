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
using Loyalty.Models.Database;
using Square.Picasso;
using Android.Locations;
using Dex.Com.Expresso.Loyalty.Droid.Utils;
using Android.Gms.Maps.Model;
using Android.Text;
using Dex.Com.Expresso.Loyalty.Droid.Activities;
using Newtonsoft.Json;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Dialogs
{
    public class MerchantOfferDialog : DialogFragment
    {
        private Context mContext;
        public delegate void onDismiss();
        public onDismiss EventOnDismiss;
        private MerchantProduct item;
        private MerchantProductMemberType offer;
        private MerchantLocation location;
        private Document document;
        private Merchant merchant;
        private Favorites favorite;
        private List<Document> mLstLogo;
        

        public static int REQUEST_CODE = 99;
        public static MerchantOfferDialog NewInstance(Bundle bundle , MerchantProduct _item, MerchantProductMemberType _offer, MerchantLocation _location, Document _document, Merchant _merchant, Favorites _favorite, List<Document> lstDoc)
        {
            MerchantOfferDialog fragment = new MerchantOfferDialog() { item = _item, offer = _offer, location = _location, document = _document , merchant = _merchant, favorite = _favorite, mLstLogo = lstDoc};
            fragment.Arguments = bundle;
            return fragment;
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            if (EventOnDismiss != null)
            {
                EventOnDismiss();
            }
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            return base.OnCreateDialog(savedInstanceState);
        }

        public override void OnResume()
        {
            base.OnResume();

            try
            {
                this.Dialog.Window.SetLayout(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.WrapContent);
            }
            catch (Exception ex)
            {

            }
        }

        private void BindingProductView(View view, MerchantProduct item, MerchantProductMemberType offer, MerchantLocation location, Document document)
        {

            var merchant = item;
            view.FindViewById<TextView>(Resource.Id.txtName).Text = item.ProductName;
            view.FindViewById<RatingBar>(Resource.Id.rtbRating).Rating = (float)item.decRating;
            ImageView imgView = view.FindViewById<ImageView>(Resource.Id.imgPicture);
            if (document != null)
            {
                Picasso.With(mContext).Load( document.FileName).Into(imgView);
            }
            Location mMyLocation = GPSUtils.getLastBestLocation(this.Activity);
            if (mMyLocation != null)
            {
                try
                {
                    double distince = GPSUtils.Distance(new LatLng(mMyLocation.Latitude, mMyLocation.Longitude), new LatLng(Double.Parse(location.strLat), Double.Parse(location.strLng)), GPSUtils.DistanceUnit.Kilometers);
                    view.FindViewById<TextView>(Resource.Id.txtDistince).Text = String.Format(mContext.GetString(Resource.String.loy_format_distince), distince);
                }
                catch (Exception ex)
                {
                    view.FindViewById<TextView>(Resource.Id.txtDistince).Text = mContext.GetString(Resource.String.loy_format_no_distince);
                }
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.txtDistince).Text = mContext.GetString(Resource.String.loy_format_no_distince);
            }

            view.FindViewById<ImageView>(Resource.Id.imgFavorite).Visibility = ViewStates.Gone;

            if (offer != null)
            {
                view.FindViewById<TextView>(Resource.Id.txtOffer).Text = String.Format(mContext.GetString(Resource.String.loy_format_offer), offer.decOffer);
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.txtOffer).Text = String.Format(mContext.GetString(Resource.String.loy_format_offer), 0);
            }

            //view.FindViewById<TextView>(Resource.Id.txtDistince)

            view.FindViewById<View>(Resource.Id.imgLike).Visibility = ViewStates.Gone;
            view.FindViewById<View>(Resource.Id.imgShare).Visibility = ViewStates.Gone;
            view.Tag = item.MerchantProductID.ToString();
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            View view = inflater.Inflate(Resource.Layout.loy_dialog_popup_marker, container, false);
            mContext = this.Activity;
            BindingProductView(view, item, offer, location, document);
            view.FindViewById<TextView>(Resource.Id.txtTitle).Text = merchant.MerchantName;
            view.FindViewById<TextView>(Resource.Id.txtDescription).Text = item.ProductDesc; // Html.FromHtml(

            view.FindViewById<View>(Resource.Id.btnDetail).Click += MerchantOfferDialog_Click;
            view.FindViewById<View>(Resource.Id.btnDirection).Click += DirectionClick; ;
            return view;
        }

        private void DirectionClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

            Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(string.Format("http://maps.google.com/maps?daddr={0},{1}", location.strLat, location.strLng)));
            this.Activity.StartActivity(intent);


        }

        private void MerchantOfferDialog_Click(object sender, EventArgs e)
        {
            //var favorite = this.favorite(p => p.IDObject == mctProduct.MerchantProductID && p.intType == Favorites.intMerchantProduct).FirstOrDefault();
            

            Intent intent = new Intent(mContext, typeof(MerchantOfferDetailsActivity));
            string jsonData = JsonConvert.SerializeObject(item);
            intent.PutExtra(MerchantOfferDetailsActivity.DATA, jsonData);

            jsonData = JsonConvert.SerializeObject(favorite);
            intent.PutExtra(MerchantOfferDetailsActivity.FAVORITE, jsonData);

            jsonData = JsonConvert.SerializeObject(document);
            intent.PutExtra(MerchantOfferDetailsActivity.DOCUMENT, jsonData);

            jsonData = JsonConvert.SerializeObject(location);
            intent.PutExtra(MerchantOfferDetailsActivity.LOCATION, jsonData);

            jsonData = JsonConvert.SerializeObject(offer);
            intent.PutExtra(MerchantOfferDetailsActivity.OFFER, jsonData);

            jsonData = JsonConvert.SerializeObject(mLstLogo);
            intent.PutExtra(MerchantOfferDetailsActivity.LOGOS, jsonData);

            ((BaseActivity)mContext).StartActivityForResult(intent, REQUEST_CODE);
        }
    }
}
