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
using Loyalty.Models;
using Loyalty.Models.Database;
using Newtonsoft.Json;
using Loyalty.Utils;
using Dex.Com.Expresso.Loyalty.Droid.Utils;
using Square.Picasso;
using Android.Locations;
using Android.Gms.Maps.Model;
//using Java.IO;
using System.IO;
using Dex.Com.Expresso;
//using PCLStorage;
using Java.Nio.Channels;
using Java.IO;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Dex.Com.Expresso.Loyalty.Droid.Activities;
//using Square.Picasso;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.Listviews
{
    public class MerchantProductsItemsAdapter : MyBaseAdapter
    {
        private List<MerchantProductItem> mLstItem;
        private List<Merchant> mLstMerchants;
        private List<MerchantProduct> mLstMerchantsProduct;
        private List<Document> mLstDocument;
        private List<MerchantLocation> mLstLocation;
        private List<MemberGroupDetail> mLstMemberGroupDetail;
        private List<MemberGroup> mLstMemberGroup;
        private LatLng mMyLocation;
        private List<Favorites> mLstFavorites;
        private List<MerchantProductMemberType> mLstProductMemberType;
        public static int REQUEST_CODE = 99;

        private BaseItem mItemFilter;
        //private List<MerchantProduct> mLstItem;
        public MerchantProductsItemsAdapter(Context conext, List<MerchantProduct> lstItem, List<Merchant> lstMerchants, List<Document> lstDocument, List<MerchantLocation> lstLocation, List<Favorites> lstFavorites, List<MerchantProductMemberType> lstMemberTypeOffer, List<MemberGroup> lstMemberGroup, List<MemberGroupDetail> lstMemberGroupDetail)
        {
            this.mContext = conext;
            //  this.mLstItem = lstItem;
            mLstMemberGroup = lstMemberGroup;
            mLstMemberGroupDetail = lstMemberGroupDetail;
            mLstMerchants = lstMerchants;
            mLstFavorites = lstFavorites;
            mLstProductMemberType = lstMemberTypeOffer;
            mLstMerchantsProduct = new List<MerchantProduct>();
            foreach (var itemPro in lstItem)
            {
                mLstMerchantsProduct.Add(itemPro);
            }
            //mLstMerchantsProduct = mLstMerchantsProduct.OrderByDescending(p => p.decRating).ToList();
            mLstDocument = lstDocument;
            mLstLocation = lstLocation;
            GenerateItems();


            string jsonData = JsonConvert.SerializeObject(mLstItem);
            LogUtils.WriteLog("Convert List MerchantProduct", jsonData);
        }


        private void GenerateItems()
        {
            Merchant merchantFilter = null;
            MerchantLocation merchantLocationFilter = null;
            MerchantProduct merchantProduct = null;
            if (mItemFilter != null)
            {
                if (mItemFilter.Item is Merchant)
                {
                    merchantFilter = mItemFilter.Item as Merchant;
                }
                else if (mItemFilter.Item is MerchantProduct)
                {
                    merchantProduct = mItemFilter.Item as MerchantProduct;
                }
                else if (mItemFilter.Item is MerchantLocation)
                {
                    merchantLocationFilter = mItemFilter.Item as MerchantLocation;
                }
            }
            if (mLstItem == null)
                mLstItem = new List<MerchantProductItem>();
            mLstItem.Clear();
            MerchantProductItem lastItem = new MerchantProductItem();
            lastItem.LstItem = new List<BaseItem>();
            //BaseItem lastMerPro = null;
            var lstItem = new List<MerchantProduct>();
            foreach (var itemPro in mLstMerchantsProduct)
            {
                if (merchantProduct != null)
                {
                    if (itemPro.MerchantProductID != merchantProduct.MerchantProductID)
                    {
                        continue;
                    }
                }
                if (merchantFilter != null)
                {
                    if (merchantFilter.MerchantID != itemPro.MerchantID)
                    {
                        continue;
                    }
                }
                if (merchantLocationFilter != null)
                {
                    if (merchantLocationFilter.MerchantId != itemPro.MerchantID)
                    {
                        continue;
                    }
                }
                lstItem.Add(itemPro);
            }
            //if (merchantFilter != null)
            //{
            //    if (merchantFilter.MerchantID != merPro.MerchantID)
            //    {
            //        continue;
            //    }
            //}
            //if (merchantProduct != null)
            //{
            //    if (merchantProduct.MerchantProductID != merPro.MerchantProductID)
            //    {
            //        continue;
            //    }
            //}
            bool isFirst = true;
            while (lstItem.Count > 0)
            {
                for (int i = 0; i < lstItem.Count; i++)
                {
                    bool isBreak = false;
                    var merPro = lstItem[i];
                    
                    if (merPro.decRating > 4.5 || isFirst)
                    {
                        isFirst = false;
                        if (lastItem.NeedItem == -1)
                        {
                            lastItem.NeedItem = 1;
                            //item 2x2
                            BaseItem item = new BaseItem();
                            item.Item = merPro;
                            item.setTag(BaseItem.TagName.Item_Size, (int)MerchantProductItem.Size.Size_2x2);
                            lastItem.LstItem.Add(item);
                        }
                        else
                        {
                            //i++;
                            continue;
                        }
                    }
                    else if (merPro.decRating > 3)
                    {
                        if (lastItem.NeedItem == -1)
                        {
                            lastItem.NeedItem = 1;
                            //item 2x1
                            BaseItem item = new BaseItem();
                            item.Item = merPro;
                            item.setTag(BaseItem.TagName.Item_Size, (int)MerchantProductItem.Size.Size_2x1);
                            lastItem.LstItem.Add(item);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (merPro.decRating > 2)
                    {
                        if (lastItem.NeedItem == -1)
                        {
                            lastItem.NeedItem = 3;
                            //item 2x1 - 2 ( 1x1) / 
                            BaseItem item = new BaseItem();
                            item.Item = merPro;
                            item.setTag(BaseItem.TagName.Item_Size, (int)MerchantProductItem.Size.Size_1x2);
                            lastItem.LstItem.Add(item);
                        }
                        else
                        {
                            if (lastItem.LstItem.Where(p => ((p.getSize(BaseItem.TagName.Item_Size)) == MerchantProductItem.Size.Size_1x2)).Count() == 1 && lastItem.LstItem.Count() == 1)
                            {
                                //already has 1x2
                                BaseItem item = new BaseItem();
                                item.Item = merPro;
                                item.setTag(BaseItem.TagName.Item_Size, (int)MerchantProductItem.Size.Size_1x2);
                                lastItem.LstItem.Add(item);
                                lastItem.NeedItem = 2;
                            }
                            else if (lastItem.LstItem.Where(p => ((p.getSize(BaseItem.TagName.Item_Size)) == MerchantProductItem.Size.Size_1x1)).Count() == 2 && lastItem.LstItem.Count == 2)
                            {
                                BaseItem item = new BaseItem();
                                item.Item = merPro;
                                item.setTag(BaseItem.TagName.Item_Size, (int)MerchantProductItem.Size.Size_1x1);
                                lastItem.LstItem.Add(item);
                                lastItem.NeedItem = 3;
                            }
                            else if (lastItem.LstItem.Where(p => ((p.getSize(BaseItem.TagName.Item_Size)) == MerchantProductItem.Size.Size_1x1)).Count() == 1 && lastItem.LstItem.Count == 1)
                            {
                                BaseItem item = new BaseItem();
                                item.Item = merPro;
                                item.setTag(BaseItem.TagName.Item_Size, (int)MerchantProductItem.Size.Size_1x1);
                                lastItem.LstItem.Add(item);
                                lastItem.NeedItem = 3;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        BaseItem item = new BaseItem();
                        item.Item = merPro;
                        item.setTag(BaseItem.TagName.Item_Size, (int)MerchantProductItem.Size.Size_1x1);
                        lastItem.LstItem.Add(item);
                        if (lastItem.NeedItem == -1)
                            lastItem.NeedItem = 2;
                        isBreak = true;
                    }

                    lstItem.RemoveAt(i);
                    i--;
                    //lastMerPro = merPro;
                    if (lastItem.NeedItem == lastItem.LstItem.Count() || lstItem.Count == 0)
                    {
                        mLstItem.Add(lastItem);
                        lastItem = new MerchantProductItem();
                        lastItem.LstItem = new List<BaseItem>();
                    }

                    if (isBreak)
                    {
                        break;
                    }
                }
            }

        }

        public void Filter(BaseItem item)
        {
            mItemFilter = item;
            GenerateItems();
            this.NotifyDataSetChanged();
        }

        public void UpdateMyLocation(Location loc)
        {
            mMyLocation = new LatLng(loc.Latitude, loc.Longitude);
            this.NotifyDataSetChanged();
        }

        public override int Count
        {
            get
            {
                return mLstItem.Count;
            }
        }


        public override Java.Lang.Object GetItem(int position)
        {
            //return (mLstItem[position]).Cast<VehicleClasses>();
            return null;
        }

        public MerchantProductItem GetVehicleClasses(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public ImageView mImgCar;
            public TextView mTxtName;
        }

        private void BindingProductView(View view, MerchantProduct item)
        {
            LogUtils.WriteLog("Binding Product", JsonConvert.SerializeObject(item));
            var merchant = mLstMerchants.Where(p => p.MerchantID == item.MerchantID).FirstOrDefault();
            view.FindViewById<TextView>(Resource.Id.txtName).Text = item.ProductName;
           
            view.FindViewById<RatingBar>(Resource.Id.rtbRating).Rating = (float)item.decRating;
            LogUtils.WriteLog("Load image", Cons.API_IMG_URL + item.MainImageID);
            ImageView imgView = view.FindViewById<ImageView>(Resource.Id.imgPicture);


            var document = mLstDocument.Where(p => p.ID == item.MainImageID).FirstOrDefault();
            LogUtils.WriteLog("Document", JsonConvert.SerializeObject(document));
            if (document != null)
            {
                Picasso.With(mContext).Load( document.FileName).Resize(500,0).Into(imgView);
            }

            if (mMyLocation != null)
            {
                try
                {
                    var locations = mLstLocation.Where(p => p.MerchantId == item.MerchantID).ToList();
                    if (locations.Count > 0)
                    {
                        MerchantLocation nearLocation = null;
                        double dbNear = int.MaxValue;
                        foreach (var location in locations)
                        {
                            //dis
                            double distince = GPSUtils.Distance(mMyLocation, new LatLng(Double.Parse(location.strLat), Double.Parse(location.strLng)), GPSUtils.DistanceUnit.Kilometers);
                            if (distince < dbNear)
                            {
                                dbNear = distince;
                                nearLocation = location;
                            }
                        }
                        view.FindViewById<TextView>(Resource.Id.txtDistince).Text = String.Format(mContext.GetString(Resource.String.loy_format_distince), dbNear);
                    }
                    else
                    {
                        view.FindViewById<TextView>(Resource.Id.txtDistince).Text = mContext.GetString(Resource.String.loy_format_no_distince);
                    }
                    
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


            var favorite = mLstFavorites.Where(p => p.IDObject == item.MerchantProductID && p.intType == Favorites.intMerchantProduct).FirstOrDefault();
            if (favorite != null)
            {
                view.FindViewById<ImageView>(Resource.Id.imgFavorite).SetImageResource(Resource.Drawable.loy_ic_favorite);
            }
            else
            {
                view.FindViewById<ImageView>(Resource.Id.imgFavorite).SetImageResource(Resource.Drawable.loy_ic_favorite_un);
            }


            var offer = mLstProductMemberType.Where(p => p.idMerchantProduct == item.MerchantProductID).OrderByDescending(p => p.decOffer).FirstOrDefault();
            if (offer != null)
            {
                view.FindViewById<TextView>(Resource.Id.txtOffer).Text = String.Format(mContext.GetString(Resource.String.loy_format_offer), offer.decOffer);
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.txtOffer).Text = String.Format(mContext.GetString(Resource.String.loy_format_offer), 0);
            }


            if (offer != null)
            {
                LinearLayout lstLogo = view.FindViewById<LinearLayout>(Resource.Id.lnlLogo);
                if (lstLogo != null)
                {
                    var listIDGroup = mLstMemberGroupDetail.Where(p => p.MemberTypeID == offer.idMemberType).Select(p => p.MemberGroupID).ToList();
                    var lstgroup = mLstMemberGroup.Where(p => listIDGroup.Contains(p.MemberGroupID) && p.idDocument != null).Select(p => p.idDocument).ToList();
                    List<Document> lstdocument = mLstDocument.Where(p => lstgroup.Contains(p.ID)).ToList();
                    foreach (var doc in lstdocument)
                    {
                        ImageView img = new ImageView(this.mContext);
                        LinearLayout.LayoutParams pa = new LinearLayout.LayoutParams(80, 60);
                        pa.SetMargins(0, 10, 0, 0);
                        img.LayoutParameters = pa;
                        img.RequestLayout();
                        lstLogo.AddView(img);
                        Picasso.With(this.Activity).Load("file://" + doc.FileName).Into(img);
                    }
                    //DocumentAdapters docadapter = new DocumentAdapters(this.mContext, lstdocument);
                    //lstLogo.Adapter = docadapter;
                }
            }
            
          
            //lstLogo.Adapter = 


            //view.FindViewById<TextView>(Resource.Id.txtDistince)

            view.FindViewById<View>(Resource.Id.imgLike).Visibility = ViewStates.Gone;

            view.FindViewById<View>(Resource.Id.imgShare).Tag = item.MerchantProductID.ToString();
            view.FindViewById<View>(Resource.Id.imgShare).Click += MerchantShareClick;
            view.FindViewById<ImageView>(Resource.Id.imgFavorite).Tag = item.MerchantProductID.ToString();
            view.FindViewById<ImageView>(Resource.Id.imgFavorite).Click += MerchantFavoriteClick;
            view.Tag = item.MerchantProductID.ToString();
            view.Click += View_Click;

        }

        private void View_Click(object sender, EventArgs e)
        {
            View view = sender as View;
            var idMerchantProduct = new Guid(view.Tag.ToString());
            var mctProduct = mLstMerchantsProduct.Where(p => p.MerchantProductID == idMerchantProduct).FirstOrDefault();
            if (mctProduct != null)
            {
                var favorite = mLstFavorites.Where(p => p.IDObject == mctProduct.MerchantProductID && p.intType == Favorites.intMerchantProduct).FirstOrDefault();
                var document = mLstDocument.Where(p => p.ID == mctProduct.MainImageID).FirstOrDefault();

                var location = mLstLocation.Where(p => p.MerchantId == mctProduct.MerchantID).FirstOrDefault();
                if (mMyLocation != null)
                {
                    var locations = mLstLocation.Where(p => p.MerchantId == mctProduct.MerchantID).ToList();
                    if (locations.Count > 0)
                    {
                        double dbNear = int.MaxValue;
                        foreach (var locationx in locations)
                        {
                            //dis
                            double distince = GPSUtils.Distance(mMyLocation, new LatLng(Double.Parse(locationx.strLat), Double.Parse(locationx.strLng)), GPSUtils.DistanceUnit.Kilometers);
                            if (distince < dbNear)
                            {
                                location = locationx;
                            }
                        }
                    }
                }

                var offer = mLstProductMemberType.Where(p => p.idMerchantProduct == idMerchantProduct).OrderByDescending(p => p.decOffer).FirstOrDefault();
                


                Intent intent = new Intent(mContext, typeof(MerchantOfferDetailsActivity));
                string jsonData = JsonConvert.SerializeObject(mctProduct);
                intent.PutExtra(MerchantOfferDetailsActivity.DATA, jsonData);

                jsonData = JsonConvert.SerializeObject(favorite);
                intent.PutExtra(MerchantOfferDetailsActivity.FAVORITE, jsonData);

                jsonData = JsonConvert.SerializeObject(document);
                intent.PutExtra(MerchantOfferDetailsActivity.DOCUMENT, jsonData);

                jsonData = JsonConvert.SerializeObject(location);
                intent.PutExtra(MerchantOfferDetailsActivity.LOCATION, jsonData);

                jsonData = JsonConvert.SerializeObject(offer);
                intent.PutExtra(MerchantOfferDetailsActivity.OFFER, jsonData);

                if (offer != null)
                {
                    var listIDGroup = mLstMemberGroupDetail.Where(p => p.MemberTypeID == offer.idMemberType).Select(p => p.MemberGroupID).ToList();
                    var lstgroup = mLstMemberGroup.Where(p => listIDGroup.Contains(p.MemberGroupID) && p.idDocument != null).Select(p => p.idDocument).ToList();
                    List<Document> lstdocument = mLstDocument.Where(p => lstgroup.Contains(p.ID)).ToList();
                    jsonData = JsonConvert.SerializeObject(lstdocument);
                    intent.PutExtra(MerchantOfferDetailsActivity.LOGOS, jsonData);
                }

                ((BaseActivity) mContext).StartActivityForResult(intent,REQUEST_CODE);
            }
        }

        public void UpdateFavorite()
        {
            FavoriteThreads thread = new FavoriteThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                mLstFavorites = result.Data as List<Favorites>;
                this.NotifyDataSetChanged();
            };
            thread.GetListByType(Favorites.intMerchantProduct);
        }

        private void MerchantFavoriteClick(object sender, EventArgs e)
        {
            var imgFavorite = sender as ImageView;
            var idMerchantProduct = new Guid(imgFavorite.Tag.ToString());
            var mctProduct = mLstMerchantsProduct.Where(p => p.MerchantProductID == idMerchantProduct).FirstOrDefault();
            if (mctProduct != null)
            {

                var favorite = mLstFavorites.Where(p => p.intType == Favorites.intMerchantProduct && p.IDObject == mctProduct.MerchantProductID).FirstOrDefault();
                if (favorite != null)
                {
                    FavoriteThreads thread = new FavoriteThreads();
                    thread.OnResult += (ServiceResult result) =>
                    {
                        Guid id = (Guid)result.Data;
                        if (id != Guid.Empty)
                        {
                            imgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
                            mLstFavorites.Remove(favorite);
                        }
                        else
                        {
                            
                        }
                    };
                    thread.deleteFavorite(favorite.ID);
                }
                else
                {
                    FavoriteThreads thread = new FavoriteThreads();
                    thread.OnResult += (ServiceResult result) =>
                    {
                        Favorites favItem = result.Data as Favorites;
                        if (favItem != null)
                        {
                            mLstFavorites.Add(favItem);
                            //this.NotifyDataSetChanged();
                            imgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite);
                        }
                        else
                        {

                        }
                    };
                    thread.insertFavorite( Favorites.intMerchantProduct, mctProduct.MerchantProductID);
                }
                
            }
        }
        private async void MerchantShareClick(object sender, EventArgs e)
        {
            var idMerchantProduct = new Guid((sender as View).Tag.ToString());
            var mctProduct = mLstMerchantsProduct.Where(p => p.MerchantProductID == idMerchantProduct).FirstOrDefault();
            if (mctProduct != null)
            {
                Intent intent = new Intent(Intent.ActionSend);
                intent.SetType("*/*");

                var document = mLstDocument.Where(p => p.ID == mctProduct.MainImageID).FirstOrDefault();
                if (document != null)
                {
                    string path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
                    string dbPath = Path.Combine(path, mctProduct.ProductName + ".png");
                    try
                    {
                        if (System.IO.File.Exists(dbPath))
                        {
                            System.IO.File.Delete(dbPath);
                        }

                        Java.IO.File src = new Java.IO.File(document.FileName);
                        Java.IO.File dst = new Java.IO.File(dbPath);

                        FileChannel inChannel = new FileInputStream(src).Channel;
                        FileChannel outChannel = new FileOutputStream(dst).Channel;
                        try
                        {
                            inChannel.TransferTo(0, inChannel.Size(), outChannel);
                        }
                        finally
                        {
                            if (inChannel != null)
                                inChannel.Close();
                            if (outChannel != null)
                                outChannel.Close();
                        }

                        //using (var br = new BinaryReader(File.Open( document.FileName, FileMode.Open)))
                        //{
                        //    using (var bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                        //    {
                        //        byte[] buffer = new byte[2048];
                        //        int length = 0;
                        //        while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
                        //        {
                        //            bw.Write(buffer, 0, length);
                        //        }
                        //    }
                        //}
                    }
                    catch (Exception ex)
                    {

                    }
                    intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + dbPath));
                }
                intent.PutExtra(Intent.ExtraSubject, mctProduct.ProductName);
                intent.PutExtra(Intent.ExtraText, mctProduct.ProductDesc);
                intent.PutExtra(Intent.ExtraTitle, mctProduct.ProductName);
                mContext.StartActivity(intent);
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder viewHoder = null;
            var item = GetVehicleClasses(position);
            if (item.NeedItem == 1)
            {
                //2x2 - 2x1
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_1item, null);
                LinearLayout product = convertView.FindViewById<LinearLayout>(Resource.Id.lnlProducts);

                if ((item.LstItem[0].getSize(BaseItem.TagName.Item_Size)) == MerchantProductItem.Size.Size_2x2)
                {
                    View viewProduct = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_2x2, null);
                    product.AddView(viewProduct);

                    MerchantProduct productItem = item.LstItem[0].Item as MerchantProduct;
                    BindingProductView(viewProduct, productItem);


                }
                else
                {
                    View viewProduct = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_2x1, null);
                    product.AddView(viewProduct);
                    MerchantProduct productItem = item.LstItem[0].Item as MerchantProduct;
                    BindingProductView(viewProduct, productItem);
                }
            }
            else if (item.NeedItem == 2)
            {
                //1x1 - 1x1
                //1x2 - 1x2
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_2item, null);
                LinearLayout product1 = convertView.FindViewById<LinearLayout>(Resource.Id.lnlProducts1);
                LinearLayout product2 = convertView.FindViewById<LinearLayout>(Resource.Id.lnlProducts2);
                if ((item.LstItem[0].getSize(BaseItem.TagName.Item_Size)) == MerchantProductItem.Size.Size_1x1)
                {
                    View viewProduct1 = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_1x1, null);
                    View viewProduct2 = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_1x1, null);

                    product1.AddView(viewProduct1);
                    MerchantProduct productItem1 = item.LstItem[0].Item as MerchantProduct;
                    BindingProductView(viewProduct1, productItem1);

                    if (item.LstItem.Count> 1)
                    {
                        product2.AddView(viewProduct2);
                        MerchantProduct productItem2 = item.LstItem[1].Item as MerchantProduct;
                        BindingProductView(viewProduct2, productItem2);
                    }
                    
                }
                else
                {
                    View viewProduct1 = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_1x2, null);
                    View viewProduct2 = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_1x2, null);

                    product1.AddView(viewProduct1);
                    MerchantProduct productItem1 = item.LstItem[0].Item as MerchantProduct;
                    BindingProductView(viewProduct1, productItem1);
                    

                    if (item.LstItem.Count > 1)
                    {
                        product2.AddView(viewProduct2);
                        MerchantProduct productItem2 = item.LstItem[1].Item as MerchantProduct;
                        BindingProductView(viewProduct2, productItem2);
                    }
                }
            }
            else if (item.NeedItem == 3)
            {
                //1x2 - 2(1x1)

                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_2item, null);
                LinearLayout product1 = convertView.FindViewById<LinearLayout>(Resource.Id.lnlProducts1);
                LinearLayout product2 = convertView.FindViewById<LinearLayout>(Resource.Id.lnlProducts2);
                if ((item.LstItem[0].getSize(BaseItem.TagName.Item_Size)) == MerchantProductItem.Size.Size_1x1)
                {

                    View viewProduct1 = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_1x1, null);
                    MerchantProduct productItem1 = item.LstItem[0].Item as MerchantProduct;
                    if (item.LstItem.Count > 1)
                    {
                        if ((item.LstItem[1].getSize(BaseItem.TagName.Item_Size)) == MerchantProductItem.Size.Size_1x1)
                        {
                            View viewProduct2 = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_1x1, null);
                            View viewProduct3 = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_1x2, null);

                            product1.AddView(viewProduct1);
                            BindingProductView(viewProduct1, productItem1);

                            if (item.LstItem.Count > 1)
                            {
                                product1.AddView(viewProduct2);
                                MerchantProduct productItem2 = item.LstItem[1].Item as MerchantProduct;
                                BindingProductView(viewProduct2, productItem2);
                            }
                            if (item.LstItem.Count > 2)
                            {
                                product2.AddView(viewProduct3);
                                MerchantProduct productItem3 = item.LstItem[2].Item as MerchantProduct;
                                BindingProductView(viewProduct3, productItem3);
                            }
                        }
                        else
                        {
                            View viewProduct2 = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_1x2, null);
                            View viewProduct3 = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_1x1, null);

                            product2.AddView(viewProduct1);
                            BindingProductView(viewProduct1, productItem1);
                            if (item.LstItem.Count > 1)
                            {
                                product2.AddView(viewProduct2);
                                MerchantProduct productItem2 = item.LstItem[1].Item as MerchantProduct;
                                BindingProductView(viewProduct2, productItem2);
                            }
                            if (item.LstItem.Count > 2)
                            {
                                product1.AddView(viewProduct3);
                                MerchantProduct productItem3 = item.LstItem[2].Item as MerchantProduct;
                                BindingProductView(viewProduct3, productItem3);
                            }
                        }
                    }
                    else
                    {
                        product1.AddView(viewProduct1);
                        BindingProductView(viewProduct1, productItem1);
                    }
                }
                else
                {
                    View viewProduct1 = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_1x2, null);
                    View viewProduct2 = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_1x1, null);
                    View viewProduct3 = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_1x1, null);

                    product1.AddView(viewProduct1);
                    product2.AddView(viewProduct2);
                    product2.AddView(viewProduct3);

                    MerchantProduct productItem1 = item.LstItem[0].Item as MerchantProduct;
                    BindingProductView(viewProduct1, productItem1);
                    

                    if (item.LstItem.Count > 0)
                    {
                        MerchantProduct productItem2 = item.LstItem[1].Item as MerchantProduct;
                        BindingProductView(viewProduct2, productItem2);
                    }
                    if (item.LstItem.Count > 1)
                    {
                        MerchantProduct productItem3 = item.LstItem[2].Item as MerchantProduct;
                        BindingProductView(viewProduct3, productItem3);
                    }
                }
            }
            
            viewHoder = new ViewHolder();
            

            //if (convertView == null)
            //{
            //    if (item.NeedItem == 1)
            //    {
            //        //2x2 - 2x1
            //        convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_1item, null);
            //    }
            //    else if (item.NeedItem == 2)
            //    {
            //        //1x1 - 1x1
            //        //1x2 - 1x2
            //        convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_2item, null);
            //    }
            //    else if (item.NeedItem == 3)
            //    {
            //        //1x2 - 2(1x1)
            //        convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_2item, null);
            //    }
            //    //convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_vehicle_classes, null);
            //    viewHoder = new ViewHolder();
            //    convertView.Tag = viewHoder;
            //}
            //else
            //{
            //    viewHoder = convertView.Tag as ViewHolder;
            //}

            return convertView;
        }


    }
}