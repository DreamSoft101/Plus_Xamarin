
using Loyalty.Models;
using Loyalty.Models.Database;
using Loyalty.Models.ServiceOutput.Database;
using Loyalty.Threads;
using Loyalty.Utils;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Processing.LocalData
{
    public static class LocalData
    {

        public static  void updateData(BaseItem baseItem, MasterThreads.Action action)
        {

            lock (BaseDatabase.locker)
            {
                try
                {
                    if (action == MasterThreads.Action.AddNew)
                    {
                        BaseDatabase.getDB().Insert(baseItem.Item);
                    }
                    else if (action == MasterThreads.Action.Update)
                    {
                        BaseDatabase.getDB().Update(baseItem.Item);
                    }
                    else if (action == MasterThreads.Action.Delete)
                    {

                    }
                }
                catch (Exception ex)
                {
                }
            }
        }


        public static void updateData(List<BaseItem> lstItems, List<MasterThreads.Action> actions)
        {

            lock (BaseDatabase.locker)
            {
                try
                {
                    for (int i = 0; i < lstItems.Count; i++)
                    {
                        var action = actions[i];
                        var baseItem = lstItems[i];
                        if (action == MasterThreads.Action.AddNew)
                        {
                            BaseDatabase.getDB().Insert(baseItem.Item);
                        }
                        else if (action == MasterThreads.Action.Update)
                        {
                            BaseDatabase.getDB().Update(baseItem.Item);
                        }
                        else if (action == MasterThreads.Action.Delete)
                        {

                        }
                    }

                }
                catch (Exception ex)
                {
                }
            }
        }


        public static List<MerchantProduct> getListMerchantProduct()
        {
            lock (BaseDatabase.locker)
            {
                try
                {

                    var listMemberType = Cons.Preferences == null ? new List<Guid>() : Cons.Preferences.Select(p => p.mMemberType.MemberTypeID).ToList();
                    if (listMemberType.Count == 0)
                    {
                        var lstItem = BaseDatabase.getDB().Table<MerchantProduct>().Where(p => p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now).OrderBy(p => p.ProductName).ToList();
                        lstItem = lstItem.Where(p => ((p.StartDate != null && p.StartDate < DateTime.Now) || p.StartDate == null) && ((p.EndDate != null && p.EndDate >= DateTime.Now) || p.EndDate == null)).ToList();
                        return lstItem;
                    }
                    else
                    {
                        var offer = BaseDatabase.getDB().Table<MerchantProductMemberType>().Where(p => listMemberType.Contains(p.idMemberType) || p.idMemberType == null).Select(p => p.idMerchantProduct).ToList();
                        var lstItem = BaseDatabase.getDB().Table<MerchantProduct>().Where(p => offer.Contains(p.MerchantProductID) && p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now).OrderBy(p => p.ProductName).ToList();
                        lstItem = lstItem.Where(p => ((p.StartDate != null && p.StartDate < DateTime.Now) || p.StartDate == null) && ((p.EndDate != null && p.EndDate >= DateTime.Now) || p.EndDate == null)).ToList();
                        return lstItem;
                    }
                    
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("getListMerchantProduct", ex.Message);
                    return null;
                }
            }
        }


        public static List<MerchantProduct> getListMerchantProductFavorite()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    
                    var favorites = BaseDatabase.getDB().Table<Favorites>().Where(p => p.intType == Favorites.intMerchantProduct).OrderByDescending(p => p.strTime).ToList();
                    var lstID = favorites.Select(p1 => p1.IDObject).ToList();
                    var lstItem = BaseDatabase.getDB().Table<MerchantProduct>().Where(p => lstID.Contains(p.MerchantProductID)).OrderBy(p => p.ProductName).ToList();
                    var result = from fav in favorites
                                 join pro in lstItem
                                 on fav.IDObject equals pro.MerchantProductID
                                 select pro;
                    return lstItem;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("getListMerchantProductFavorite", ex.Message);
                    return null;
                }
            }
        }

        public static List<MerchantProduct> getListMerchantProductRecent()
        {
            lock (BaseDatabase.locker)
            {
                try
                {

                    var favorites = BaseDatabase.getDB().Table<Recent>().Where(p => p.intType == Favorites.intMerchantProduct).OrderByDescending(p => p.strTime).Take(20).ToList();
                    var lstID = favorites.Select(p1 => p1.IDObject).ToList();
                    var lstItem = BaseDatabase.getDB().Table<MerchantProduct>().Where(p => lstID.Contains(p.MerchantProductID)).OrderBy(p => p.ProductName).ToList();
                    //var result = from fav in favorites
                    //             join pro in lstItem
                    //             on fav.IDObject equals pro.MerchantProductID
                    //             select pro;
                    return lstItem;
                    /*
                    var favorites = BaseDatabase.getDB().Table<Favorites>().Where(p => p.intType == Favorites.intMerchantProduct).Select(p => p.IDObject).ToList();
                    var lstItem = BaseDatabase.getDB().Table<MerchantProduct>().Where(p => favorites.Contains(p.MerchantProductID)).OrderBy(p => p.ProductName).ToList();
                    return lstItem;*/
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("getListMerchantProductRecent", ex.Message);
                    return null;
                }
            }
        }

        public static List<Document> getDocument()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var lstItem = BaseDatabase.getDB().Table<Document>().ToList();
                    return lstItem;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("getDocument", ex.Message);
                    return null;
                }
            }
        }

        public static Merchant getMerchantByID(Guid id)
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var lstItem = BaseDatabase.getDB().Table<Merchant>().Where(p => p.MerchantID == id).FirstOrDefault();
                    return lstItem;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("getListMerchantProduct", ex.Message);
                    return null;
                }
            }
        }

        public static List<Merchant> getMerchant()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var lstItem = BaseDatabase.getDB().Table<Merchant>().OrderBy(p => p.MerchantName).ToList();
                    return lstItem;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("getListMerchantProduct", ex.Message);
                    return null;
                }
            }
        }

        public static List<MerchantProductMemberType> getMerchantProductMemberType()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var lstItem = BaseDatabase.getDB().Table<MerchantProductMemberType>().ToList();
                    return lstItem;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("MerchantProductMemberType", ex.Message);
                    return null;
                }
            }
        }

        public static List<MerchantLocation> getMerchantLocation()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var lstItem = BaseDatabase.getDB().Table<MerchantLocation>().ToList();
                    return lstItem;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("getMerchantLocation", ex.Message);
                    return null;
                }
            }
        }


        public static List<Favorites> getListFavoriteByType(int type)
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var lstItem = BaseDatabase.getDB().Table<Favorites>().Where(p => p.intType == type).ToList();
                    return lstItem;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("getListFavoriteByType", ex.Message);
                    return null;
                }
            }
        }

        public static Favorites insertFavorite(int type, Guid id)
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var favorite = new Favorites();
                    favorite.IDObject = id;
                    favorite.intType = type;
                    favorite.strTime = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    favorite.ID = Guid.NewGuid();
                    BaseDatabase.getDB().Insert(favorite);
                    return favorite;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("insertFavorite", ex.Message);
                    return null;
                }
            }
        }

        public static Guid deleteFavorite(Guid id)
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    BaseDatabase.getDB().Table<Favorites>().Delete(p => p.ID == id);
                    return id;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("insertFavorite", ex.Message);
                    return Guid.Empty;
                }
            }
        }


        public static List<RedemptionCategory> getListRedemptionCategory()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var result = BaseDatabase.getDB().Table<RedemptionCategory>().OrderBy(p => p.intSorting).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("getListRedemptionCategory", ex.Message);
                }
                return null;
            }
        }

        public static List<RedemptionProduct> getListRedemptionProduct(Guid? MemberTypeID, Guid? idRedemptionCategory)
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    
                    var listDetail = BaseDatabase.getDB().Table<RedemptionProductDetail>().Where(p => p.MemberTypeID == MemberTypeID && p.intRedeemMode == 1 ).Select(p => p.RedemptionProductID).ToList();
                    var result = BaseDatabase.getDB().Table<RedemptionProduct>().Where(p => listDetail.Contains(p.RedemptionProductID) && (p.intProductType == 1 || p.intProductType == 6)).OrderBy(p => p.intSorting).ToList();
                    if (idRedemptionCategory != null)
                    {
                        result = result.Where(p => p.RedemptionCategoryID == idRedemptionCategory).ToList();
                    }

                    result = result.Where(p => ((p.dtStartDate != null && p.dtStartDate < DateTime.Now) || p.dtStartDate == null) && ((p.dtEndDate != null && p.dtEndDate >= DateTime.Now) || p.dtEndDate == null)).ToList();
                    result = result.Where(p => p.Available == true).ToList();

                    return result;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("getListRedemptionProduct", ex.Message);
                }
                return null;
            }
        }

        public static List<RedemptionProductDetail> getListRedemptionProductDetail(Guid? MemberTypeID, Guid? RedemptionProductID)
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    if (RedemptionProductID != null)
                    {
                        var result = BaseDatabase.getDB().Table<RedemptionProductDetail>().Where(p => p.RedemptionProductID == RedemptionProductID && p.intRedeemMode == 1).ToList();
                        return result;
                    }
                    else
                    {
                        var result = BaseDatabase.getDB().Table<RedemptionProductDetail>().ToList();
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("getListRedemptionProduct", ex.Message);
                }
                return null;
            }
        }

        public static void AddRecent(Guid ID, int type)
        {
            lock (BaseDatabase.locker)
            {
                var item = BaseDatabase.getDB().Table<Recent>().Where(p => p.intType == type && p.IDObject == ID).FirstOrDefault();
                if (item == null)
                {
                    item = new Recent();
                    item.ID = Guid.NewGuid();
                    item.IDObject = ID;
                    item.strTime = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    item.intType = type;
                    BaseDatabase.getDB().Insert(item);
                }
                else
                {
                    item.strTime = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    BaseDatabase.getDB().Update(item);
                }
            }
        }

        public static List<MemberType> GetMemberType()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var result = BaseDatabase.getDB().Table<MemberType>().OrderBy(p => p.MemberType1).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("GetMemberType", ex.Message);
                }
                return null;
            }
        }

        public static List<MemberGroup> GetMemberGroup()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var result = BaseDatabase.getDB().Table<MemberGroup>().ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("GetMemberGroup", ex.Message);
                }
                return null;
            }
        }

        public static List<MemberGroupDetail> GetListMemberGroupDetail()
        {
            lock(BaseDatabase.locker)
            {
                try
                {
                    var result = BaseDatabase.getDB().Table<MemberGroupDetail>().ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("GetListMemberGroupDetail", ex.Message);
                }
                return null;
            }
        }

        public static void AddProductToCart(MemberRedeemInfoProduct item)
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var result = BaseDatabase.getDB().Table<MemberRedeemInfoProduct>().Where(p => p.RedemptionProductDetailId == item.RedemptionProductDetailId).FirstOrDefault();
                    if (result != null)
                    {
                        result.Quantity++;
                        BaseDatabase.getDB().Update(item);
                    }
                    else
                    {
                        BaseDatabase.getDB().Insert(item);
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("AddProductToCart", ex.Message);
                }
                
            }
        }

        public static void RemoveProductFromCart(MemberRedeemInfoProduct item, bool isDelete)
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var result = BaseDatabase.getDB().Table<MemberRedeemInfoProduct>().Where(p => p.RedemptionProductDetailId == item.RedemptionProductDetailId).FirstOrDefault();
                    if (result != null)
                    {
                        result.Quantity--;
                        if (isDelete)
                        {
                            result.Quantity = 0;
                        }
                        if (result.Quantity <= 0)
                        {
                            BaseDatabase.getDB().Delete(result);
                        }
                        else
                        {
                            BaseDatabase.getDB().Update(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("RemoveProductFromCart", ex.Message);
                }
                
            }
        }

        public static List<MemberRedeemInfoProduct> GetCart()
        {
            lock (BaseDatabase.locker)
            {
                var result = BaseDatabase.getDB().Table<MemberRedeemInfoProduct>().ToList();
                return result;
            }
        }

        public static RedemptionProduct GetRedemptionProduct(Guid id)
        {
            lock (BaseDatabase.locker)
            {
                var result = BaseDatabase.getDB().Table<RedemptionProduct>().Where(p => p.RedemptionProductID == id).FirstOrDefault();
                return result;
            }
        }

        public static RedemptionProductDetail GetRedemptionProductDetail(Guid id)
        {
            lock (BaseDatabase.locker)
            {
                var result = BaseDatabase.getDB().Table<RedemptionProductDetail>().Where(p => p.RedemptionProductDetailID == id).FirstOrDefault();
                return result;
            }
        }

        public static Document GetDocument(Guid? id)
        {
            if (id == null)
            {
                return null;
            }
            lock (BaseDatabase.locker)
            {
                var result = BaseDatabase.getDB().Table<Document>().Where(p => p.ID == id).FirstOrDefault();
                return result;
            }
        }

        public static void UnLockEVoucher(int id)
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var result = BaseDatabase.getDB().Table<MemberRedeemInfoProduct>().Where(p => p.Id == id).FirstOrDefault();
                    if (result != null)
                    {
                        result.IsLock = false;
                        BaseDatabase.getDB().Update(result);
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("UnLockEVoucher", ex.Message);
                }
            }
        }

        public static List<State> GetState()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var result = BaseDatabase.getDB().Table<State>().ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("GetState", ex.Message);
                }
                return null;
            }
        }

        public static List<Country> GetCountry()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var result = BaseDatabase.getDB().Table<Country>().ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("GetCountry", ex.Message);
                }
                return null;
            }
        }

        public static void ClearCart()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var result = BaseDatabase.getDB().Table<MemberRedeemInfoProduct>().Delete(p => 1 == 1);
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("GetCountry", ex.Message);
                }
            }
        }

        public static List<RedemptionProduct> GetEvoucher(Guid? MemberTypeID)
        {
            lock (BaseDatabase.locker)
            {
                try
                {

                    var listDetail = BaseDatabase.getDB().Table<RedemptionProductDetail>().Where(p => p.MemberTypeID == MemberTypeID && p.intRedeemMode == 1).Select(p => p.RedemptionProductID).ToList();
                    var result = BaseDatabase.getDB().Table<RedemptionProduct>().Where(p => listDetail.Contains(p.RedemptionProductID) && ( p.intProductType == 6)).OrderBy(p => p.intSorting).ToList();
                    result = result.Where(p => ((p.dtStartDate != null && p.dtStartDate < DateTime.Now) || p.dtStartDate == null) && ((p.dtEndDate != null && p.dtEndDate >= DateTime.Now) || p.dtEndDate == null)).ToList();

                    return result;
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("GetEvoucher", ex.Message);
                }
                return null;
            }
        }
        //public static List<> 
    }
}
