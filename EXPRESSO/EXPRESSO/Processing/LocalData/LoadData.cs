using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using EXPRESSO.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EXPRESSO.Models.EnumType;

namespace EXPRESSO.Processing.LocalData
{

    /// <summary>
    /// Only get data from localdb
    /// </summary>
    public static class LoadData
    {

        public static int mStatusActive = (int)EnumType.StatusType.Active;
        /// <summary>
        /// Get all highway from localdb
        /// </summary>
        /// <returns></returns>
        public static List<TblHighway> getListHighway()
        {
            lock (BaseDatabase.locker)
            {
                var result = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idEntity == Cons.IdEntity);
                return result.ToList();
            }
            //return null;
        }

        /// <summary>
        /// Get detail highway from local db
        /// </summary>
        /// <param name="idHighway"></param>
        /// <returns></returns>
        public static TblHighway getHighway(string idHighway)
        {
            lock (BaseDatabase.locker)
            {
                var result = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idEntity == Cons.IdEntity && p.idHighway == idHighway).FirstOrDefault();
                return result;
            }
        }

        /// <summary>
        /// Get list CSC by Highway
        /// </summary>
        /// <param name="idHighway"></param>
        /// <returns></returns>
        public static List<TblCSC> getListCSC(string idHighway)
        {
            return null;
        }

        /// <summary>
        /// Get list Classes used by toll fare
        /// </summary>
        /// <returns></returns>
        public static List<VehicleClasses> getListVehicleClasses()
        {
            List<VehicleClasses> result = new List<VehicleClasses>();
            result.Add(new VehicleClasses() { strIcon = "ic_vehicleclasses_1", strName = string.IsNullOrEmpty(Cons.myEntity.mSettings.vehicle_class_1_label) ? "Class 1" : Cons.myEntity.mSettings.vehicle_class_1_label, intValue = 1 });
            result.Add(new VehicleClasses() { strIcon = "ic_vehicleclasses_2", strName = string.IsNullOrEmpty(Cons.myEntity.mSettings.vehicle_class_2_label) ? "Class 1" : Cons.myEntity.mSettings.vehicle_class_2_label, intValue = 2 });
            result.Add(new VehicleClasses() { strIcon = "ic_vehicleclasses_3", strName = string.IsNullOrEmpty(Cons.myEntity.mSettings.vehicle_class_3_label) ? "Class 1" : Cons.myEntity.mSettings.vehicle_class_3_label, intValue = 3 });
            result.Add(new VehicleClasses() { strIcon = "ic_vehicleclasses_4", strName = string.IsNullOrEmpty(Cons.myEntity.mSettings.vehicle_class_4_label) ? "Class 1" : Cons.myEntity.mSettings.vehicle_class_4_label, intValue = 4 });
            result.Add(new VehicleClasses() { strIcon = "ic_vehicleclasses_5", strName = string.IsNullOrEmpty(Cons.myEntity.mSettings.vehicle_class_5_label) ? "Class 1" : Cons.myEntity.mSettings.vehicle_class_5_label, intValue = 5 });

            return result;
        }


        public static List<TblLog> getAllLog()
        {
            lock (BaseDatabase.locker)
            {
                var lst = BaseDatabase.getDB().Table<TblLog>().ToList();
                return lst;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<TblTollPlaza> getListTollPlaza()
        {
            lock (BaseDatabase.locker)
            {
                return BaseDatabase.getDB().Table<TblTollPlaza>().Where(p => p.idEntity == Cons.IdEntity && p.intStatus == mStatusActive).OrderBy(p => p.idHighway).ThenBy(p => p.intSort).ToList();

            }
        }

        public static List<BaseItem> getListTollPlazaWithHeader()
        {
            lock (BaseDatabase.locker)
            {
                var listItem = BaseDatabase.getDB().Table<TblTollPlaza>().Where(p => p.idEntity == Cons.IdEntity && p.intStatus == mStatusActive).OrderBy(p => p.idHighway).ThenBy(p => p.intSort).ToList();
                string idLastHighway = "";
                List<BaseItem> result = new List<BaseItem>();
                foreach (var item in listItem.ToList())
                {
                    if (item.idHighway != idLastHighway)
                    {
                        idLastHighway = item.idHighway;
                        TblHighway highway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == item.idHighway).FirstOrDefault();
                        BaseItem bItemHighway = new BaseItem();
                        bItemHighway.Item = highway;
                        result.Add(bItemHighway);
                    }

                    BaseItem bItem = new BaseItem();
                    bItem.setTag(BaseItem.TagName.IsFavorite, IsFavorite(item.idTollPlaza, FavoriteType.TollPlaza));
                    bItem.Item = item;
                    result.Add(bItem);
                }
                return result;
            }
        }

        public static TblTollPlaza getTollPlaza(string idTollPlaza)
        {
            lock (BaseDatabase.locker)
            {
                return BaseDatabase.getDB().Table<TblTollPlaza>().Where(p => p.idEntity == Cons.IdEntity && p.idTollPlaza == idTollPlaza && p.intStatus == mStatusActive).FirstOrDefault();
            }
        }


        public static bool IsFavorite(string idItem, FavoriteType type)
        {
            lock (BaseDatabase.locker)
            {
                var item = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idEntity == Cons.IdEntity && p.idObject == idItem && p.intObjectType == (int)type).FirstOrDefault();
                bool result = item != null;
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<BaseItem> getListRSA(FacilitiesType type)
        {
            lock (BaseDatabase.locker)
            {
                //string id = Cons.IdEntity;
                var listItem = BaseDatabase.getDB().Table<TblRSA>().Where(p => p.idEntity == Cons.IdEntity && p.intStatus == mStatusActive).OrderBy(p => p.idHighway).ThenBy(p => p.intSort).ToList();
                if (type == FacilitiesType.RSA)
                {
                    listItem = listItem.Where(p => p.strType == "2" || p.strType == "3").ToList();
                }
                else if (type == FacilitiesType.LAYBY)
                {
                    listItem = listItem.Where(p => p.strType == "0" || p.strType == "1").ToList();
                }
                bool isLastFavorite = false;
                List<BaseItem> lstResult = new List<BaseItem>();
                string idLastHighway = "";
                foreach (var item in listItem)
                {
                    var itemDetail = item;



                    BaseItem bItem = new BaseItem();
                    bItem.Item = itemDetail;
                    string idParent = Convert.ToInt32(itemDetail.idRSA.Replace("RSA", "")).ToString();
                    //bItem.setTag(BaseItem.TagName.IsFavorite, true);
                    if (BaseDatabase.getDB().Table<TblRSACctv>().Where(p => p.idParent == idParent).Count() > 0)
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsCCTV, true);
                    }
                    else
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsCCTV, false);
                    }

                    if (itemDetail.strType == "1" || itemDetail.strType == "3")
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsSign, true);
                    }
                    else
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsSign, false);
                    }

                    if (BaseDatabase.getDB().Table<TblFacilities>().Where(p => p.idParent == itemDetail.idRSA && p.idEntity == Cons.IdEntity && p.intFacilityType == 11).Count() > 0)
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsFood, true);
                    }
                    else
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsFood, false);
                    }

                    if (LocalData.LoadData.IsFavorite(itemDetail.idRSA, FavoriteType.RSA))
                    {
                        isLastFavorite = true;
                        bItem.setTag(BaseItem.TagName.IsFavorite, true);
                    }
                    else
                    {
                        bItem.setTag(BaseItem.TagName.IsFavorite, false);
                    }

                    lstResult.Add(bItem);
                }

                lstResult = lstResult.OrderByDescending(p => (bool)p.getTag(BaseItem.TagName.IsFavorite)).ThenBy(p => (p.Item as TblRSA).idHighway).ThenBy(p => (p.Item as TblRSA).intSort).ToList();

                var result = new List<BaseItem>();

                foreach (var item in lstResult)
                {
                    var itemDetail = item.Item as TblRSA;
                    if (idLastHighway != itemDetail.idHighway || isLastFavorite != (bool)item.getTag(BaseItem.TagName.IsFavorite))
                    {
                        var itemHighway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == itemDetail.idHighway).FirstOrDefault();
                        idLastHighway = itemDetail.idHighway;
                        BaseItem bItemHighway = new BaseItem();
                        bItemHighway.Item = itemHighway;
                        result.Add(bItemHighway);
                        if (isLastFavorite != (bool)item.getTag(BaseItem.TagName.IsFavorite))
                        {
                            isLastFavorite = (bool)item.getTag(BaseItem.TagName.IsFavorite);
                        }
                    }

                    result.Add(item);
                }


                return result;
            }
        }

        public static TblRSA getRSA(string idRSA)
        {
            lock (BaseDatabase.locker)
            {
                return BaseDatabase.getDB().Table<TblRSA>().Where(p => p.idRSA == idRSA && p.idEntity == Cons.IdEntity).FirstOrDefault();
            }
        }

        public static List<BaseItem> getListFacilityByParent(string id)
        {
            lock (BaseDatabase.locker)
            {
                var list = BaseDatabase.getDB().Table<TblFacilities>().Where(p => p.idParent == id && p.idEntity == Cons.IdEntity).OrderBy(p => p.idHighway).ThenBy(p => p.intSort).ToList();
                List<BaseItem> results = new List<BaseItem>();
                foreach (TblFacilities item in list)
                {
                    var result = new BaseItem();
                    result.Item = item;
                    result.setTag(BaseItem.TagName.IsFavorite, IsFavorite(item.idFacilities, FavoriteType.Facilities));
                    var itemCategory = BaseDatabase.getDB().Table<TblFacilityType>().Where(p => p.intFacilityType == item.intFacilityType).FirstOrDefault();
                    if (itemCategory != null)
                    {
                        string jsonDataCategory = Utils.StringUtils.Object2String(itemCategory);
                        result.setTag(BaseItem.TagName.Facility_Type, jsonDataCategory);
                        result.setTag(BaseItem.TagName.Facility_TypeName, itemCategory.strName);
                        if (!string.IsNullOrEmpty(itemCategory.strPicture))
                        {
                            result.setTag(BaseItem.TagName.StrICon, itemCategory.strPicture);
                        }
                            
                    }
                    var itemBrand = BaseDatabase.getDB().Table<TblBrand>().Where(p => p.idBrand == item.idBrand).FirstOrDefault();
                    if (itemBrand != null)
                    {
                        string jsonBand = Utils.StringUtils.Object2String(itemBrand);
                        result.setTag(BaseItem.TagName.Facility_BrandID, itemBrand.idBrand);
                        result.setTag(BaseItem.TagName.Facility_BrandName, itemBrand.strName);
                        result.setTag(BaseItem.TagName.Facility_Brand, jsonBand);
                        if (!string.IsNullOrEmpty(itemBrand.strPicture))
                        {
                            result.setTag(BaseItem.TagName.StrICon, itemBrand.strPicture);
                        }
                    }
                    results.Add(result);
                }
                return results;
            }
        }

        public static List<BaseItem> getListPertrolStation()
        {
            lock (BaseDatabase.locker)
            {
                var listItem = BaseDatabase.getDB().Table<TblPetrolStation>().Where(p => p.idEntity == Cons.IdEntity && p.intStatus == mStatusActive).OrderBy(p => p.idHighway).ThenBy(p => p.intSort).ToList();
                string idLastHighway = "";
                List<BaseItem> result = new List<BaseItem>();
                foreach (var item in listItem)
                {
                    if (item.idHighway != idLastHighway)
                    {
                        idLastHighway = item.idHighway;
                        TblHighway highway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == item.idHighway).FirstOrDefault();
                        BaseItem bItemHighway = new BaseItem();
                        bItemHighway.Item = highway;
                        result.Add(bItemHighway);
                    }

                    BaseItem bItem = new BaseItem();

                    if (!string.IsNullOrEmpty(item.idBrand))
                    {
                        var brand = BaseDatabase.getDB().Table<TblBrand>().Where(p => p.idBrand == item.idBrand).FirstOrDefault();
                        if (brand != null)
                        {
                            bItem.setTag(BaseItem.TagName.Pertrol_BrandIMG, brand.strPicture);
                        }
                        else
                        {
                            bItem.setTag(BaseItem.TagName.Pertrol_BrandIMG, "");
                        }
                        //bItem.setTag()
                    }
                    else
                    {
                        bItem.setTag(BaseItem.TagName.Pertrol_BrandIMG, "");
                    }


                    bItem.setTag(BaseItem.TagName.IsFavorite, IsFavorite(item.idPetrol, FavoriteType.PertrolStation));
                    bItem.Item = item;
                    result.Add(bItem);
                }
                return result;
            }
        }

        public static TblPetrolStation getPertrolStation(string idPetrolStation)
        {
            lock (BaseDatabase.locker)
            {
                return BaseDatabase.getDB().Table<TblPetrolStation>().Where(p => p.idPetrol == idPetrolStation && p.idEntity == Cons.IdEntity).FirstOrDefault();
            }
        }

        public static TblCSC getCSC(string idCSC)
        {
            lock (BaseDatabase.locker)
            {
                return BaseDatabase.getDB().Table<TblCSC>().Where(p => p.idCSC == idCSC && p.idEntity == Cons.IdEntity).FirstOrDefault();
            }
        }

        public static List<BaseItem> getCSCWithHeader()
        {
            lock (BaseDatabase.locker)
            {
                int intActive = (int)EnumType.StatusType.Active;
                var listItem = BaseDatabase.getDB().Table<TblCSC>().Where(p => p.idEntity == Cons.IdEntity && p.intStatus == intActive).OrderBy(p => p.idHighway).ThenBy(p => p.intSort).ToList();
                string idLastHighway = "";
                List<BaseItem> result = new List<BaseItem>();
                foreach (var item in listItem)
                {
                    if (item.idHighway != idLastHighway)
                    {
                        idLastHighway = item.idHighway;
                        TblHighway highway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == item.idHighway).FirstOrDefault();
                        BaseItem bItemHighway = new BaseItem();
                        bItemHighway.Item = highway;
                        result.Add(bItemHighway);
                    }

                    BaseItem bItem = new BaseItem();
                    bItem.setTag(BaseItem.TagName.IsFavorite, IsFavorite(item.idCSC, FavoriteType.CSC));
                    bItem.Item = item;
                    result.Add(bItem);
                }
                return result;
            }
        }

        public static List<TblNearbyCatg> getNearbyCategory()
        {
            lock (BaseDatabase.locker)
            {
                int intActive = (int)EnumType.StatusType.Active;
                var listItem = BaseDatabase.getDB().Table<TblNearbyCatg>().Where(p => p.idEntity == Cons.IdEntity && p.intStatus == intActive).ToList();
                return listItem;
            }
        }

        public static List<BaseItem> getNearby(string idCategory)
        {
            lock (BaseDatabase.locker)
            {
                int intActive = (int)EnumType.StatusType.Active;
                var listItem = BaseDatabase.getDB().Table<TblNearby>().Where(p => p.idEntity == Cons.IdEntity && p.intStatus == intActive && p.idNearbyCatg == idCategory).ToList();
                string idLastHighway = "";
                List<BaseItem> result = new List<BaseItem>();
                foreach (var item in listItem)
                {
                    BaseItem bItem = new BaseItem();
                    bItem.Item = item;
                    bItem.setTag(BaseItem.TagName.IsFavorite, IsFavorite(item.idNearby, FavoriteType.Nearby));
                    result.Add(bItem);
                }
                return result;
            }
        }

        public static List<TblFacilityType> getListFacilityType()
        {
            try
            {
                lock (BaseDatabase.locker)
                {
                    int intActive = (int)EnumType.StatusType.Active;
                    var list = BaseDatabase.getDB().Table<TblFacilityType>().ToList();
                    var result = list.Where(p => p.idEntity.ToString() == Cons.IdEntity && p.intStatus == intActive).ToList();
                    return result;
                    // return BaseDatabase.getDB().Table<TblFacilityType>().Where(p => p.idEntity.ToString() == Cons.IdEntity && p.intStatus == intActive).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getListFacilityType", ex.Message);
                return null;
            }
        }

        public static List<BaseItem> getListFacilityByType(int idType)
        {
            try
            {
                lock (BaseDatabase.locker)
                {
                    int intActive = (int)EnumType.StatusType.Active;
                    var listItem = BaseDatabase.getDB().Table<TblFacilities>().Where(p => p.idEntity == Cons.IdEntity && p.intFacilityType == idType && p.intStatus == intActive).ToList();
                    string idLastHighway = "";
                    List<BaseItem> result = new List<BaseItem>();
                    foreach (var item in listItem)
                    {
                        if (item.idHighway != idLastHighway)
                        {
                            idLastHighway = item.idHighway;
                            TblHighway highway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == item.idHighway).FirstOrDefault();
                            BaseItem bItemHighway = new BaseItem();
                            bItemHighway.Item = highway;
                            result.Add(bItemHighway);
                        }

                        BaseItem bItem = new BaseItem();
                        bItem.setTag(BaseItem.TagName.IsFavorite, IsFavorite(item.idFacilities, FavoriteType.Facilities));
                        bItem.Item = item;
                        result.Add(bItem);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getListFacilityType", ex.Message);
                return null;
            }
        }

        public static List<BaseItem> getPlusSmile()
        {
            lock (BaseDatabase.locker)
            {
                int intActive = (int)EnumType.StatusType.Active;
                var listItem = BaseDatabase.getDB().Table<TblFacilities>().Where(p => p.idEntity == Cons.IdEntity && p.intStatus == intActive && p.intFacilityType == 8 || p.intFacilityType == 9 || p.intFacilityType == 10).OrderBy(p => p.idHighway).ThenBy(p => p.intSort).ToList();
                string idLastHighway = "";
                List<BaseItem> result = new List<BaseItem>();
                foreach (var item in listItem)
                {
                    if (item.idHighway != idLastHighway)
                    {
                        idLastHighway = item.idHighway;
                        TblHighway highway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == item.idHighway).FirstOrDefault();
                        BaseItem bItemHighway = new BaseItem();
                        bItemHighway.Item = highway;
                        result.Add(bItemHighway);
                    }

                    BaseItem bItem = new BaseItem();
                    bItem.setTag(BaseItem.TagName.IsFavorite, IsFavorite(item.idFacilities, FavoriteType.Facilities));
                    bItem.Item = item;
                    result.Add(bItem);
                }
                return result;
            }
        }

        public static List<BaseItem> getSSK()
        {
            lock (BaseDatabase.locker)
            {
                int intActive = (int)EnumType.StatusType.Active;
                var listItem = BaseDatabase.getDB().Table<TblFacilities>().Where(p => p.idEntity == Cons.IdEntity && p.intStatus == intActive && p.intFacilityType == 12).OrderBy(p => p.idHighway).ThenBy(p => p.intSort).ToList();
                string idLastHighway = "";
                List<BaseItem> result = new List<BaseItem>();
                foreach (var item in listItem)
                {
                    if (item.idHighway != idLastHighway)
                    {
                        idLastHighway = item.idHighway;
                        TblHighway highway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == item.idHighway).FirstOrDefault();
                        BaseItem bItemHighway = new BaseItem();
                        bItemHighway.Item = highway;
                        result.Add(bItemHighway);
                    }

                    BaseItem bItem = new BaseItem();
                    bItem.Item = item;
                    bItem.setTag(BaseItem.TagName.IsFavorite, IsFavorite(item.idFacilities, FavoriteType.Facilities));
                    result.Add(bItem);
                }
                return result;
            }
        }

        public static List<BaseItem> getTunnel()
        {
            lock (BaseDatabase.locker)
            {
                int intActive = (int)EnumType.StatusType.Active;
                var listItem = BaseDatabase.getDB().Table<TblRSA>().Where(p => p.idEntity == Cons.IdEntity && p.intStatus == intActive && p.strType == "5").OrderBy(p => p.idHighway).ThenBy(p => p.intSort).ToList();
                string idLastHighway = "";
                List<BaseItem> result = new List<BaseItem>();
                foreach (var item in listItem)
                {
                    if (item.idHighway != idLastHighway)
                    {
                        idLastHighway = item.idHighway;
                        TblHighway highway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == item.idHighway).FirstOrDefault();
                        BaseItem bItemHighway = new BaseItem();
                        bItemHighway.Item = highway;
                        result.Add(bItemHighway);
                    }

                    BaseItem bItem = new BaseItem();
                    bItem.Item = item;
                    bItem.setTag(BaseItem.TagName.IsFavorite, IsFavorite(item.idRSA, FavoriteType.RSA));
                    string idParent = Convert.ToInt32(item.idRSA.Replace("RSA", "")).ToString();

                    if (BaseDatabase.getDB().Table<TblRSACctv>().Where(p => p.idParent == idParent).Count() > 0)
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsCCTV, true);
                    }
                    else
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsCCTV, false);
                    }

                    if (item.strType == "1" || item.strType == "3")
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsSign, true);
                    }
                    else
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsSign, false);
                    }

                    if (BaseDatabase.getDB().Table<TblFacilities>().Where(p => p.idParent == item.idRSA && p.idEntity == Cons.IdEntity && p.intFacilityType == 11).Count() > 0)
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsFood, true);
                    }
                    else
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsFood, false);
                    }
                    result.Add(bItem);
                }
                return result;
            }
        }

        public static List<BaseItem> getInterchange()
        {
            lock (BaseDatabase.locker)
            {
                int intActive = (int)EnumType.StatusType.Active;
                var listItem = BaseDatabase.getDB().Table<TblRSA>().Where(p => p.idEntity == Cons.IdEntity && p.intStatus == intActive && p.strType == "4").OrderBy(p => p.idHighway).ThenBy(p => p.intSort).ToList();
                string idLastHighway = "";
                List<BaseItem> result = new List<BaseItem>();
                foreach (var item in listItem)
                {
                    if (item.idHighway != idLastHighway)
                    {
                        idLastHighway = item.idHighway;
                        TblHighway highway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == item.idHighway).FirstOrDefault();
                        BaseItem bItemHighway = new BaseItem();
                        bItemHighway.Item = highway;
                        result.Add(bItemHighway);
                    }

                    BaseItem bItem = new BaseItem();
                    bItem.setTag(BaseItem.TagName.IsFavorite, IsFavorite(item.idRSA, FavoriteType.RSA));
                    bItem.Item = item;
                    string idParent = Convert.ToInt32(item.idRSA.Replace("RSA", "")).ToString();

                    if (BaseDatabase.getDB().Table<TblRSACctv>().Where(p => p.idParent == idParent).Count() > 0)
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsCCTV, true);
                    }
                    else
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsCCTV, false);
                    }

                    if (item.strType == "1" || item.strType == "3")
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsSign, true);
                    }
                    else
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsSign, false);
                    }

                    if (BaseDatabase.getDB().Table<TblFacilities>().Where(p => p.idParent == item.idRSA && p.idEntity == Cons.IdEntity && p.intFacilityType == 11).Count() > 0)
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsFood, true);
                    }
                    else
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsFood, false);
                    }
                    result.Add(bItem);
                }
                return result;
            }
        }

        public static List<BaseItem> getVistapoint()
        {
            lock (BaseDatabase.locker)
            {
                int intActive = (int)EnumType.StatusType.Active;
                var listItem = BaseDatabase.getDB().Table<TblRSA>().Where(p => p.idEntity == Cons.IdEntity && p.intStatus == intActive && p.strType == "6").OrderBy(p => p.idHighway).ThenBy(p => p.intSort).ToList();
                string idLastHighway = "";
                List<BaseItem> result = new List<BaseItem>();
                foreach (var item in listItem)
                {
                    if (item.idHighway != idLastHighway)
                    {
                        idLastHighway = item.idHighway;
                        TblHighway highway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == item.idHighway).FirstOrDefault();
                        BaseItem bItemHighway = new BaseItem();
                        bItemHighway.Item = highway;
                        result.Add(bItemHighway);
                    }

                    BaseItem bItem = new BaseItem();
                    bItem.setTag(BaseItem.TagName.IsFavorite, IsFavorite(item.idRSA, FavoriteType.RSA));
                    bItem.Item = item;
                    string idParent = Convert.ToInt32(item.idRSA.Replace("RSA", "")).ToString();

                    if (BaseDatabase.getDB().Table<TblRSACctv>().Where(p => p.idParent == idParent).Count() > 0)
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsCCTV, true);
                    }
                    else
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsCCTV, false);
                    }

                    if (item.strType == "1" || item.strType == "3")
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsSign, true);
                    }
                    else
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsSign, false);
                    }

                    if (BaseDatabase.getDB().Table<TblFacilities>().Where(p => p.idParent == item.idRSA && p.idEntity == Cons.IdEntity && p.intFacilityType == 11).Count() > 0)
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsFood, true);
                    }
                    else
                    {
                        bItem.setTag(BaseItem.TagName.RSA_IsFood, false);
                    }
                    result.Add(bItem);
                }
                return result;
            }
        }

        public static List<BaseItem> getTNGReload()
        {
            lock (BaseDatabase.locker)
            {
                int intActive = (int)EnumType.StatusType.Active;
                var listItem = BaseDatabase.getDB().Table<TblRSA>().Where(p => p.idEntity == Cons.IdEntity && p.intStatus == intActive && p.strType == "8").OrderBy(p => p.idHighway).ThenBy(p => p.intSort).ToList();
                string idLastHighway = "";
                List<BaseItem> result = new List<BaseItem>();
                foreach (var item in listItem)
                {
                    if (item.idHighway != idLastHighway)
                    {
                        idLastHighway = item.idHighway;
                        TblHighway highway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == item.idHighway).FirstOrDefault();
                        BaseItem bItemHighway = new BaseItem();
                        bItemHighway.Item = highway;
                        result.Add(bItemHighway);
                    }
                    BaseItem bItem = new BaseItem();
                    bItem.Item = item;
                    result.Add(bItem);
                }
                return result;
            }
        }

        public static List<TblRSACctv> getListRSACCTV(string idRSA)
        {
            lock (BaseDatabase.locker)
            {
                string id = Convert.ToInt32(idRSA.Replace("RSA", "")).ToString();
                int intActive = (int)EnumType.StatusType.Active;
                var listItem = BaseDatabase.getDB().Table<TblRSACctv>().Where(p => p.idEntity == Cons.IdEntity && p.intStatus == intActive && p.idParent == id).ToList();
                return listItem;
            }
        }


        public static List<TblEntities> getListEntity()
        {
            return null;
        }

        public static List<TblRouteDetails> getListByRoute(TblTollPlaza toll, int type)
        {

            lock (BaseDatabase.locker)
            {

                try
                {
                    int intStatus = (int)EnumType.StatusType.Active;
                    var result = BaseDatabase.getDB().Table<TblRouteDetails>().Where(p => p.intStatus == intStatus && p.idRouteItem == toll.idTollPlaza && p.intType == type && p.idEntity == Cons.IdEntity);
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    Utils.LogUtils.WriteError("getListByRoute", ex.Message);
                    return new List<TblRouteDetails>();
                }

            }
        }

        public static decimal getRate(TblTollPlaza from, TblTollPlaza to, VehicleClasses vehicleclasses)
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    var result = BaseDatabase.getDB().Table<TblTollFare>().Where(p => p.idTollPlazaFrom == from.idTollPlaza && p.idTollPlazaTo == to.idTollPlaza && p.idEntity == Cons.IdEntity).FirstOrDefault();
                    if (result != null)
                    {
                        switch (vehicleclasses.intValue)
                        {
                            case 1:
                                return result.decTollAmt1;
                            case 2:
                                return result.decTollAmt2;
                            case 3:
                                return result.decTollAmt3;
                            case 4:
                                return result.decTollAmt4;
                            case 5:
                                return result.decTollAmt5;
                            default:
                                return 0;
                        }
                    }
                    else
                    {
                        Utils.LogUtils.WriteLog("getRate", "Not found");
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    Utils.LogUtils.WriteError("getRate", ex.Message);
                    return -1;
                }
            }
        }

        public static List<TblRouteDetails> getListByRoute(string idRoute, int intStart, int intEnd)
        {
            lock (BaseDatabase.locker)
            {

                try
                {
                    int intStatus = (int)EnumType.StatusType.Active;
                    int min = intStart;
                    int max = intEnd;
                    if (intStart > intEnd)
                    {
                        min = intEnd;
                        max = intStart;
                    }
                    var result = BaseDatabase.getDB().Table<TblRouteDetails>().Where(p => p.intStatus == intStatus && p.idRoute == idRoute
                                                                            && p.intSeq > min && p.intSeq < max && p.idEntity == Cons.IdEntity);

                    if (intStart > intEnd)
                    {
                        return result.OrderBy(p => p.intSeq).ToList();
                    }
                    else
                    {
                        return result.OrderByDescending(p => p.intSeq).ToList();
                    }
                }
                catch (Exception ex)
                {
                    Utils.LogUtils.WriteError("getListByRoute", ex.Message);
                    return new List<TblRouteDetails>();
                }
            }
        }


        public static List<BaseItem> getListFavoriteRSA()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    int intType = (int)EnumType.FavoriteType.RSA;
                    var lst = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idEntity == Cons.IdEntity && p.intObjectType == intType).ToList();

                    bool isLastFavorite = false;
                    List<BaseItem> lstResult = new List<BaseItem>();
                    string idLastHighway = "";
                    foreach (var item in lst)
                    {
                        var itemDetail = JsonConvert.DeserializeObject<TblRSA>(item.strDescription);

                       

                        BaseItem bItem = new BaseItem();
                        bItem.Item = itemDetail;
                        string idParent = Convert.ToInt32(itemDetail.idRSA.Replace("RSA", "")).ToString();
                        //bItem.setTag(BaseItem.TagName.IsFavorite, true);
                        if (BaseDatabase.getDB().Table<TblRSACctv>().Where(p => p.idParent == idParent).Count() > 0)
                        {
                            bItem.setTag(BaseItem.TagName.RSA_IsCCTV, true);
                        }
                        else
                        {
                            bItem.setTag(BaseItem.TagName.RSA_IsCCTV, false);
                        }

                        if (itemDetail.strType == "1" || itemDetail.strType == "3")
                        {
                            bItem.setTag(BaseItem.TagName.RSA_IsSign, true);
                        }
                        else
                        {
                            bItem.setTag(BaseItem.TagName.RSA_IsSign, false);
                        }

                        if (BaseDatabase.getDB().Table<TblFacilities>().Where(p => p.idParent == itemDetail.idRSA && p.idEntity == Cons.IdEntity && p.intFacilityType == 11).Count() > 0)
                        {
                            bItem.setTag(BaseItem.TagName.RSA_IsFood, true);
                        }
                        else
                        {
                            bItem.setTag(BaseItem.TagName.RSA_IsFood, false);
                        }

                        if (LocalData.LoadData.IsFavorite(itemDetail.idRSA, FavoriteType.RSA))
                        {
                            isLastFavorite = true;
                            bItem.setTag(BaseItem.TagName.IsFavorite, true);
                        }
                        else
                        {
                            bItem.setTag(BaseItem.TagName.IsFavorite, false);
                        }

                        lstResult.Add(bItem);
                    }

                    lstResult = lstResult.OrderBy(p => (bool)p.getTag(BaseItem.TagName.IsFavorite)).ThenBy(p => (p.Item as TblRSA).idHighway).ThenBy(p => (p.Item as TblRSA).intSort).ToList();

                    var result = new List<BaseItem>();

                    foreach (var item in lstResult)
                    {
                        var itemDetail = item.Item as TblRSA;
                        if (idLastHighway != itemDetail.idHighway || isLastFavorite != (bool)item.getTag(BaseItem.TagName.IsFavorite))
                        {
                            var itemHighway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == itemDetail.idHighway).FirstOrDefault();
                            idLastHighway = itemDetail.idHighway;
                            BaseItem bItemHighway = new BaseItem();
                            bItemHighway.Item = itemHighway;
                            result.Add(bItemHighway);
                        }
                      
                        result.Add(item);
                    }


                    return lstResult;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        //public static List<BaseItem> getListFavoriteLayBy()
        //{
        //    lock (BaseDatabase.locker)
        //    {
        //        try
        //        {
        //            int intType = (int)EnumType.FacilitiesType.LAYBY;
        //            var lst = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idEntity == Cons.IdEntity && p.intObjectType == intType).ToList();
        //            List<BaseItem> lstResult = new List<BaseItem>();
        //            string idLastHighway = "";
        //            foreach (var item in lst)
        //            {
        //                var itemDetail = JsonConvert.DeserializeObject<TblRSA>(item.strDescription);

        //                if (itemDetail.idHighway != idLastHighway)
        //                {
        //                    idLastHighway = itemDetail.idHighway;
        //                    var itemHighway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == itemDetail.idHighway).FirstOrDefault();
        //                    BaseItem bItemHighway = new BaseItem();
        //                    bItemHighway.Item = itemHighway;
        //                    lstResult.Add(bItemHighway);
        //                }

        //                BaseItem bItem = new BaseItem();
        //                bItem.Item = itemDetail;
        //                string idParent = Convert.ToInt32(itemDetail.idRSA.Replace("RSA", "")).ToString();
        //                bItem.setTag(BaseItem.TagName.IsFavorite, true);
        //                if (BaseDatabase.getDB().Table<TblRSACctv>().Where(p => p.idParent == idParent).Count() > 0)
        //                {
        //                    bItem.setTag(BaseItem.TagName.RSA_IsCCTV, true);
        //                }
        //                else
        //                {
        //                    bItem.setTag(BaseItem.TagName.RSA_IsCCTV, false);
        //                }

        //                if (itemDetail.strType == "1" || itemDetail.strType == "3")
        //                {
        //                    bItem.setTag(BaseItem.TagName.RSA_IsSign, true);
        //                }
        //                else
        //                {
        //                    bItem.setTag(BaseItem.TagName.RSA_IsSign, false);
        //                }

        //                if (BaseDatabase.getDB().Table<TblFacilities>().Where(p => p.idParent == itemDetail.idRSA && p.idEntity == Cons.IdEntity && p.intFacilityType == 11).Count() > 0)
        //                {
        //                    bItem.setTag(BaseItem.TagName.RSA_IsFood, true);
        //                }
        //                else
        //                {
        //                    bItem.setTag(BaseItem.TagName.RSA_IsFood, false);
        //                }

        //                lstResult.Add(bItem);
        //            }
        //            return lstResult;
        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //}

        public static List<BaseItem> getListFavoriteTollPlazaWithHeader()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    int intType = (int)EnumType.FavoriteType.TollPlaza;
                    var lst = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idEntity == Cons.IdEntity && p.intObjectType == intType).ToList();
                    List<BaseItem> lstResult = new List<BaseItem>();
                    string idLastHighway = "";
                    foreach (var item in lst)
                    {
                        var itemDetail = JsonConvert.DeserializeObject<TblTollPlaza>(item.strDescription);

                        if (itemDetail.idHighway != idLastHighway)
                        {
                            idLastHighway = itemDetail.idHighway;
                            var itemHighway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == itemDetail.idHighway).FirstOrDefault();
                            BaseItem bItemHighway = new BaseItem();
                            bItemHighway.Item = itemHighway;
                            lstResult.Add(bItemHighway);
                        }

                        BaseItem bItem = new BaseItem();
                        bItem.Item = itemDetail;
                        bItem.setTag(BaseItem.TagName.IsFavorite, true);
                        lstResult.Add(bItem);
                    }
                    return lstResult;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public static List<BaseItem> getListFavoritePertrolStationHeader()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    int intType = (int)EnumType.FavoriteType.PertrolStation;
                    var lst = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idEntity == Cons.IdEntity && p.intObjectType == intType).ToList();
                    List<BaseItem> lstResult = new List<BaseItem>();
                    string idLastHighway = "";
                    foreach (var item in lst)
                    {
                        var itemDetail = JsonConvert.DeserializeObject<TblPetrolStation>(item.strDescription);

                        if (itemDetail.idHighway != idLastHighway)
                        {
                            idLastHighway = itemDetail.idHighway;
                            var itemHighway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == itemDetail.idHighway).FirstOrDefault();
                            BaseItem bItemHighway = new BaseItem();
                            bItemHighway.Item = itemHighway;
                            lstResult.Add(bItemHighway);
                        }

                        BaseItem bItem = new BaseItem();
                        bItem.Item = itemDetail;
                        bItem.setTag(BaseItem.TagName.IsFavorite, true);
                        lstResult.Add(bItem);
                    }
                    return lstResult;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public static List<BaseItem> getListFavoriteCSCHeader()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    int intType = (int)EnumType.FavoriteType.CSC;
                    var lst = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idEntity == Cons.IdEntity && p.intObjectType == intType).ToList();
                    List<BaseItem> lstResult = new List<BaseItem>();
                    string idLastHighway = "";
                    foreach (var item in lst)
                    {
                        var itemDetail = JsonConvert.DeserializeObject<TblCSC>(item.strDescription);

                        if (itemDetail.idHighway != idLastHighway)
                        {
                            idLastHighway = itemDetail.idHighway;
                            var itemHighway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == itemDetail.idHighway).FirstOrDefault();
                            BaseItem bItemHighway = new BaseItem();
                            bItemHighway.Item = itemHighway;
                            lstResult.Add(bItemHighway);
                        }

                        BaseItem bItem = new BaseItem();
                        bItem.Item = itemDetail;
                        bItem.setTag(BaseItem.TagName.IsFavorite, true);
                        lstResult.Add(bItem);
                    }
                    return lstResult;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public static List<BaseItem> getListCCTV()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    
                    int intType = (int)EnumType.FavoriteType.Facilities;
                    var lst = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idEntity == Cons.IdEntity && (p.intObjectType == (int)FavoriteType.LiveFeed_TollPlaza || p.intObjectType == (int)FavoriteType.LiveFeed)).ToList();

                    //var lstHighway = getListHighway();

                    var result = new List<BaseItem>();


                    foreach (var item in lst)
                    {
                        
                        if (item.intObjectType == (int)FavoriteType.LiveFeed)
                        {
                            var traffic = JsonConvert.DeserializeObject<TrafficUpdate>(item.strDescription);

                            BaseItem itemB = new BaseItem() { Item = traffic };
                            itemB.setTag(BaseItem.TagName.IdHighway, traffic.idHighway);
                            itemB.setTag(BaseItem.TagName.IsFavorite, true);
                            result.Add(itemB);
                           
                        }
                        else
                        {
                            var itemcctv = JsonConvert.DeserializeObject<TollPlazaCCTV>(item.strDescription);
                            BaseItem itemB = new BaseItem() { Item = itemcctv };
                            itemB.setTag(BaseItem.TagName.IdHighway, itemcctv.idHighway);
                            itemB.setTag(BaseItem.TagName.IsFavorite, true);
                            result.Add(itemB);
                        }
                    }
                    result = result.OrderBy(p => (string)p.getTag(BaseItem.TagName.IdHighway)).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }

        public static List<BaseItem> getListFavoriteFacilitiesHeader()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    int intType = (int)EnumType.FavoriteType.Facilities;
                    var lst = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idEntity == Cons.IdEntity && p.intObjectType == intType).ToList();
                    List<BaseItem> lstResult = new List<BaseItem>();
                    string idLastHighway = "";
                    foreach (var item in lst)
                    {
                        var itemDetail = JsonConvert.DeserializeObject<TblFacilities>(item.strDescription);

                        if (itemDetail.idHighway != idLastHighway)
                        {
                            idLastHighway = itemDetail.idHighway;
                            var itemHighway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == itemDetail.idHighway).FirstOrDefault();
                            BaseItem bItemHighway = new BaseItem();
                            bItemHighway.Item = itemHighway;
                            lstResult.Add(bItemHighway);
                        }

                        BaseItem bItem = new BaseItem();
                        bItem.Item = itemDetail;
                        bItem.setTag(BaseItem.TagName.IsFavorite, true);
                        lstResult.Add(bItem);
                    }
                    return lstResult;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
      

        public static List<BaseItem> getListFavoriteNearByWithHeader()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    int intType = (int)EnumType.FavoriteType.Nearby;
                    var lst = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idEntity == Cons.IdEntity && p.intObjectType == intType).ToList();
                    List<BaseItem> lstResult = new List<BaseItem>();
                    string idLastHighway = "";
                    foreach (var item in lst)
                    {
                        var itemDetail = JsonConvert.DeserializeObject<TblNearby>(item.strDescription);
                        /*
                        if (itemDetail.idHighway != idLastHighway)
                        {
                            idLastHighway = itemDetail.idHighway;
                            var itemHighway = BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == itemDetail.idHighway).FirstOrDefault();
                            BaseItem bItemHighway = new BaseItem();
                            bItemHighway.Item = itemHighway;
                            lstResult.Add(bItemHighway);
                        }
                        */
                        BaseItem bItem = new BaseItem();
                        bItem.Item = itemDetail;
                        bItem.setTag(BaseItem.TagName.IsFavorite, true);
                        lstResult.Add(bItem);
                    }
                    return lstResult;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public static List<TblFavLoc> getAllFavorite()
        {
            try
            {
                var lst = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idEntity == Cons.IdEntity).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getAllFavorite", ex.Message);
                return null;
            }
        }

        public delegate void onUpdate(BaseItem item);

        public static void updateData(BaseItem baseItem)
        {

            lock (BaseDatabase.locker)
            {
                try
                {
                    if (baseItem.Item is TblBrand)
                    {
                        TblBrand item = baseItem.Item as TblBrand;

                        if (item.intStatus == 2)
                        {
                            BaseDatabase.getDB().Table<TblBrand>().Delete(p => p.idBrand == item.idBrand);
                        }
                        else
                        {
                            if (BaseDatabase.getDB().Table<TblBrand>().Where(p => p.idBrand == item.idBrand).Count() > 0)
                            {
                                BaseDatabase.getDB().Update(item);
                            }
                            else
                            {
                                BaseDatabase.getDB().Insert(item);
                            }
                        }
                    }
                    else if (baseItem.Item is TblHighway)
                    {
                        TblHighway item = baseItem.Item as TblHighway;
                        if (item.intStatus == 2)
                        {
                            BaseDatabase.getDB().Table<TblHighway>().Delete(p => p.idHighway == item.idHighway);
                        }
                        else
                        {
                            if (BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == item.idHighway).Count() > 0)
                            {
                                BaseDatabase.getDB().Update(item);
                            }
                            else
                            {
                                BaseDatabase.getDB().Insert(item);
                            }
                        }
                    }
                    else if (baseItem.Item is TblTollPlaza)
                    {
                        TblTollPlaza item = baseItem.Item as TblTollPlaza;
                        if (item.intStatus == 2)
                        {
                            BaseDatabase.getDB().Table<TblTollPlaza>().Delete(p => p.idTollPlaza == item.idTollPlaza);
                        }
                        else
                        {
                            if (BaseDatabase.getDB().Table<TblTollPlaza>().Where(p => p.idTollPlaza == item.idTollPlaza).Count() > 0)
                            {
                                BaseDatabase.getDB().Update(item);
                            }
                            else
                            {
                                BaseDatabase.getDB().Insert(item);
                            }
                        }
                    }
                    else if (baseItem.Item is TblPetrolStation)
                    {
                        TblPetrolStation item = baseItem.Item as TblPetrolStation;
                        if (item.intStatus == 2)
                        {
                            BaseDatabase.getDB().Table<TblPetrolStation>().Delete(p => p.idPetrol == item.idPetrol);
                        }
                        else
                        {
                            if (BaseDatabase.getDB().Table<TblPetrolStation>().Where(p => p.idPetrol == item.idPetrol).Count() > 0)
                            {
                                BaseDatabase.getDB().Update(item);
                            }
                            else
                            {
                                BaseDatabase.getDB().Insert(item);
                            }
                        }
                    }
                    else if (baseItem.Item is TblRSA)
                    {
                        TblRSA item = baseItem.Item as TblRSA;
                        if (item.intStatus == 2)
                        {
                            BaseDatabase.getDB().Table<TblRSA>().Delete(p => p.idRSA == item.idRSA);
                        }
                        else
                        {
                            if (BaseDatabase.getDB().Table<TblRSA>().Where(p => p.idRSA == item.idRSA).Count() > 0)
                            {
                                BaseDatabase.getDB().Update(item);
                            }
                            else
                            {
                                BaseDatabase.getDB().Insert(item);
                            }
                        }
                    }
                    else if (baseItem.Item is TblCSC)
                    {
                        TblCSC item = baseItem.Item as TblCSC;
                        if (item.intStatus == 2)
                        {
                            BaseDatabase.getDB().Table<TblCSC>().Delete(p => p.idCSC == item.idCSC);
                        }
                        else
                        {
                            if (BaseDatabase.getDB().Table<TblCSC>().Where(p => p.idCSC == item.idCSC).Count() > 0)
                            {
                                BaseDatabase.getDB().Update(item);
                            }
                            else
                            {
                                BaseDatabase.getDB().Insert(item);
                            }
                        }
                    }
                    else if (baseItem.Item is TblFacilities)
                    {
                        TblFacilities item = baseItem.Item as TblFacilities;
                        if (item.intStatus == 2)
                        {
                            BaseDatabase.getDB().Table<TblFacilities>().Delete(p => p.idFacilities == item.idFacilities);
                        }
                        else
                        {
                            if (BaseDatabase.getDB().Table<TblFacilities>().Where(p => p.idFacilities == item.idFacilities).Count() > 0)
                            {
                                BaseDatabase.getDB().Update(item);
                            }
                            else
                            {
                                BaseDatabase.getDB().Insert(item);
                            }
                        }
                    }
                    else if (baseItem.Item is TblTollFare)
                    {
                        TblTollFare item = baseItem.Item as TblTollFare;
                        if (item.intStatus == 2)
                        {
                            BaseDatabase.getDB().Table<TblTollFare>().Delete(p => p.idTollFare == item.idTollFare);
                        }
                        else
                        {
                            if (BaseDatabase.getDB().Table<TblTollFare>().Where(p => p.idTollFare == item.idTollFare).Count() > 0)
                            {
                                BaseDatabase.getDB().Update(item);
                            }
                            else
                            {
                                BaseDatabase.getDB().Insert(item);
                            }
                        }
                    }
                    else if (baseItem.Item is TblRoute)
                    {
                        TblRoute item = baseItem.Item as TblRoute;
                        if (item.intStatus == 2)
                        {
                            BaseDatabase.getDB().Table<TblRoute>().Delete(p => p.idRoute == item.idRoute);
                        }
                        else
                        {
                            if (BaseDatabase.getDB().Table<TblRoute>().Where(p => p.idRoute == item.idRoute).Count() > 0)
                            {
                                BaseDatabase.getDB().Update(item);
                            }
                            else
                            {
                                BaseDatabase.getDB().Insert(item);
                            }
                        }
                    }
                    else if (baseItem.Item is TblRouteDetails)
                    {
                        TblRouteDetails item = baseItem.Item as TblRouteDetails;
                        if (item.intStatus == 2)
                        {
                            BaseDatabase.getDB().Table<TblRouteDetails>().Delete(p => p.idRouteItem == item.idRouteItem);
                        }
                        else
                        {
                            if (BaseDatabase.getDB().Table<TblRouteDetails>().Where(p => p.idRouteItem == item.idRouteItem).Count() > 0)
                            {
                                BaseDatabase.getDB().Update(item);
                            }
                            else
                            {
                                BaseDatabase.getDB().Insert(item);
                            }
                        }
                    }
                    else if (baseItem.Item is TblNearbyCatg)
                    {
                        TblNearbyCatg item = baseItem.Item as TblNearbyCatg;
                        if (item.intStatus == 2)
                        {
                            BaseDatabase.getDB().Table<TblNearbyCatg>().Delete(p => p.idNearbyCatg == item.idNearbyCatg);
                        }
                        else
                        {
                            if (BaseDatabase.getDB().Table<TblNearbyCatg>().Where(p => p.idNearbyCatg == item.idNearbyCatg).Count() > 0)
                            {
                                BaseDatabase.getDB().Update(item);
                            }
                            else
                            {
                                BaseDatabase.getDB().Insert(item);
                            }
                        }
                    }

                    else if (baseItem.Item is TblNearby)
                    {
                        TblNearby item = baseItem.Item as TblNearby;
                        if (item.intStatus == 2)
                        {
                            BaseDatabase.getDB().Table<TblNearby>().Delete(p => p.idNearby == item.idNearby);
                        }
                        else
                        {
                            if (BaseDatabase.getDB().Table<TblNearby>().Where(p => p.idNearby == item.idNearby).Count() > 0)
                            {
                                BaseDatabase.getDB().Update(item);
                            }
                            else
                            {
                                BaseDatabase.getDB().Insert(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }


        public static void updateData(List<BaseItem> baseItems)
        {
            lock (BaseDatabase.locker)
            {
                List<Object> addItems = new List<object>();
                List<Object> updateItems = new List<object>();
                List<Object> deleteItems = new List<object>();
                BaseDatabase.getDB().BeginTransaction();
                foreach (var baseItem in baseItems)
                {
                    try
                    {
                        if (baseItem.Item is TblBrand)
                        {
                            TblBrand itemBrand = baseItem.Item as TblBrand;

                            if (itemBrand.intStatus == 2)
                            {
                                BaseDatabase.getDB().Table<TblBrand>().Delete(p => p.idBrand == itemBrand.idBrand);

                            }
                            else
                            {
                                if (BaseDatabase.getDB().Table<TblBrand>().Where(p => p.idBrand == itemBrand.idBrand).Count() > 0)
                                {
                                    BaseDatabase.getDB().Update(itemBrand);
                                }
                                else
                                {
                                    BaseDatabase.getDB().Insert(itemBrand);
                                }
                            }
                        }
                        else if (baseItem.Item is TblHighway)
                        {
                            TblHighway itemHighway = baseItem.Item as TblHighway;
                            if (itemHighway.intStatus == 2)
                            {
                                BaseDatabase.getDB().Table<TblHighway>().Delete(p => p.idHighway == itemHighway.idHighway);
                            }
                            else
                            {
                                if (BaseDatabase.getDB().Table<TblHighway>().Where(p => p.idHighway == itemHighway.idHighway).Count() > 0)
                                {
                                    BaseDatabase.getDB().Update(itemHighway);
                                }
                                else
                                {
                                    BaseDatabase.getDB().Insert(itemHighway);
                                }
                            }
                        }
                        else if (baseItem.Item is TblTollPlaza)
                        {
                            TblTollPlaza itemTollPlaza = baseItem.Item as TblTollPlaza;
                            if (itemTollPlaza.intStatus == 2)
                            {
                                BaseDatabase.getDB().Table<TblTollPlaza>().Delete(p => p.idTollPlaza == itemTollPlaza.idTollPlaza);
                            }
                            else
                            {
                                if (BaseDatabase.getDB().Table<TblTollPlaza>().Where(p => p.idTollPlaza == itemTollPlaza.idTollPlaza).Count() > 0)
                                {
                                    BaseDatabase.getDB().Update(itemTollPlaza);
                                }
                                else
                                {
                                    BaseDatabase.getDB().Insert(itemTollPlaza);
                                }
                            }
                        }
                        else if (baseItem.Item is TblPetrolStation)
                        {
                            TblPetrolStation itemPertrol = baseItem.Item as TblPetrolStation;
                            if (itemPertrol.intStatus == 2)
                            {
                                BaseDatabase.getDB().Table<TblPetrolStation>().Delete(p => p.idPetrol == itemPertrol.idPetrol);
                            }
                            else
                            {
                                if (BaseDatabase.getDB().Table<TblPetrolStation>().Where(p => p.idPetrol == itemPertrol.idPetrol).Count() > 0)
                                {
                                    BaseDatabase.getDB().Update(itemPertrol);
                                }
                                else
                                {
                                    BaseDatabase.getDB().Insert(itemPertrol);
                                }
                            }
                        }
                        else if (baseItem.Item is TblRSA)
                        {
                            TblRSA itemRSA = baseItem.Item as TblRSA;
                            if (itemRSA.intStatus == 2)
                            {
                                BaseDatabase.getDB().Table<TblRSA>().Delete(p => p.idRSA == itemRSA.idRSA);
                            }
                            else
                            {
                                if (BaseDatabase.getDB().Table<TblRSA>().Where(p => p.idRSA == itemRSA.idRSA).Count() > 0)
                                {
                                    BaseDatabase.getDB().Update(itemRSA);
                                }
                                else
                                {
                                    BaseDatabase.getDB().Insert(itemRSA);
                                }
                            }
                        }
                        else if (baseItem.Item is TblCSC)
                        {
                            TblCSC itemCSC = baseItem.Item as TblCSC;
                            if (itemCSC.intStatus == 2)
                            {
                                BaseDatabase.getDB().Table<TblCSC>().Delete(p => p.idCSC == itemCSC.idCSC);
                            }
                            else
                            {
                                if (BaseDatabase.getDB().Table<TblCSC>().Where(p => p.idCSC == itemCSC.idCSC).Count() > 0)
                                {
                                    BaseDatabase.getDB().Update(itemCSC);
                                }
                                else
                                {
                                    BaseDatabase.getDB().Insert(itemCSC);
                                }
                            }
                        }
                        else if (baseItem.Item is TblFacilities)
                        {
                            TblFacilities itemFacilities = (TblFacilities)baseItem.Item;
                            if (itemFacilities.intStatus == 2)
                            {
                                BaseDatabase.getDB().Table<TblFacilities>().Delete(p => p.idFacilities == itemFacilities.idFacilities);
                            }
                            else
                            {
                                if (BaseDatabase.getDB().Table<TblFacilities>().Where(p => p.idFacilities == itemFacilities.idFacilities).Count() > 0)
                                {
                                    BaseDatabase.getDB().Update(itemFacilities);
                                }
                                else
                                {
                                    BaseDatabase.getDB().Insert(itemFacilities);
                                }
                            }
                        }
                        else if (baseItem.Item is TblFacilityType)
                        {
                            TblFacilityType itemFacilitiestype = (TblFacilityType)baseItem.Item;
                            if (itemFacilitiestype.intStatus == 2)
                            {
                                BaseDatabase.getDB().Table<TblFacilityType>().Delete(p => p.intFacilityType == itemFacilitiestype.intFacilityType && p.idEntity.ToString() == Cons.myEntity.idEntity);
                            }
                            else
                            {
                                if (BaseDatabase.getDB().Table<TblFacilityType>().Where(p => p.idEntity == itemFacilitiestype.idEntity && p.intFacilityType == itemFacilitiestype.intFacilityType).Count() > 0)
                                {
                                    BaseDatabase.getDB().Update(itemFacilitiestype);
                                }
                                else
                                {
                                    BaseDatabase.getDB().Insert(itemFacilitiestype);
                                }
                            }
                        }
                        else if (baseItem.Item is TblTollFare)
                        {
                            TblTollFare itemTollFare = baseItem.Item as TblTollFare;
                            if (itemTollFare.intStatus == 2)
                            {
                                BaseDatabase.getDB().Table<TblTollFare>().Delete(p => p.idTollFare == itemTollFare.idTollFare);
                            }
                            else
                            {
                                if (BaseDatabase.getDB().Table<TblTollFare>().Where(p => p.idTollFare == itemTollFare.idTollFare).Count() > 0)
                                {
                                    BaseDatabase.getDB().Update(itemTollFare);
                                }
                                else
                                {
                                    BaseDatabase.getDB().Insert(itemTollFare);
                                }
                            }
                        }
                        else if (baseItem.Item is TblRoute)
                        {
                            TblRoute itemRoute = baseItem.Item as TblRoute;
                            if (itemRoute.intStatus == 2)
                            {
                                BaseDatabase.getDB().Table<TblRoute>().Delete(p => p.idRoute == itemRoute.idRoute);
                            }
                            else
                            {
                                if (BaseDatabase.getDB().Table<TblRoute>().Where(p => p.idRoute == itemRoute.idRoute).Count() > 0)
                                {
                                    BaseDatabase.getDB().Update(itemRoute);
                                }
                                else
                                {
                                    BaseDatabase.getDB().Insert(itemRoute);
                                }
                            }
                        }
                        else if (baseItem.Item is TblRouteDetails)
                        {
                            TblRouteDetails itemRouteDetail = baseItem.Item as TblRouteDetails;
                            if (itemRouteDetail.intStatus == 2)
                            {
                                BaseDatabase.getDB().Table<TblRouteDetails>().Delete(p => p.idRouteItem == itemRouteDetail.idRouteItem);
                            }
                            else
                            {
                                if (BaseDatabase.getDB().Table<TblRouteDetails>().Where(p => p.idRouteItem == itemRouteDetail.idRouteItem).Count() > 0)
                                {
                                    BaseDatabase.getDB().Update(itemRouteDetail);


                                }
                                else
                                {
                                    BaseDatabase.getDB().Insert(itemRouteDetail);
                                }
                            }
                        }
                        else if (baseItem.Item is TblNearbyCatg)
                        {
                            TblNearbyCatg itemNearbyCatg = baseItem.Item as TblNearbyCatg;
                            if (itemNearbyCatg.intStatus == 2)
                            {
                                BaseDatabase.getDB().Table<TblNearbyCatg>().Delete(p => p.idNearbyCatg == itemNearbyCatg.idNearbyCatg);
                            }
                            else
                            {
                                if (BaseDatabase.getDB().Table<TblNearbyCatg>().Where(p => p.idNearbyCatg == itemNearbyCatg.idNearbyCatg).Count() > 0)
                                {
                                    BaseDatabase.getDB().Update(itemNearbyCatg);
                                }
                                else
                                {
                                    BaseDatabase.getDB().Insert(itemNearbyCatg);
                                }
                            }
                        }

                        else if (baseItem.Item is TblNearby)
                        {
                            TblNearby itemNearby = baseItem.Item as TblNearby;
                            if (itemNearby.intStatus == 2)
                            {
                                BaseDatabase.getDB().Table<TblNearby>().Delete(p => p.idNearby == itemNearby.idNearby);
                            }
                            else
                            {
                                if (BaseDatabase.getDB().Table<TblNearby>().Where(p => p.idNearby == itemNearby.idNearby).Count() > 0)
                                {
                                    BaseDatabase.getDB().Update(itemNearby);
                                }
                                else
                                {
                                    BaseDatabase.getDB().Insert(itemNearby);
                                }
                            }
                        }
                        else if (baseItem.Item is TblRSACctv)
                        {
                            TblRSACctv itemTblRSACCTV = baseItem.Item as TblRSACctv;
                            if (itemTblRSACCTV.intStatus == 2)
                            {
                                BaseDatabase.getDB().Table<TblRSACctv>().Delete(p => p.idRSAcctv == itemTblRSACCTV.idRSAcctv);
                            }
                            else
                            {
                                if (BaseDatabase.getDB().Table<TblRSACctv>().Where(p => p.idRSAcctv == itemTblRSACCTV.idRSAcctv).Count() > 0)
                                {
                                    BaseDatabase.getDB().Update(itemTblRSACCTV);
                                }
                                else
                                {
                                    BaseDatabase.getDB().Insert(itemTblRSACCTV);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        LogUtils.WriteError("updateData", ex.Message);
                    }
                }
                BaseDatabase.getDB().Commit();
            }
        }

        public static List<TblCategory> getListCategory()
        {
            lock (BaseDatabase.locker)
            {
                return BaseDatabase.getDB().Table<TblCategory>().ToList();
            }
        }

        public static TblTollFare getTollFare(string id)
        {
            lock (BaseDatabase.locker)
            {
                return BaseDatabase.getDB().Table<TblTollFare>().Where(p => p.idTollPlazaFrom == id).FirstOrDefault();
            }
        }

        public static TblTollFare getTollFare(string id, string idto)
        {
            lock (BaseDatabase.locker)
            {
                return BaseDatabase.getDB().Table<TblTollFare>().Where(p => p.idTollPlazaFrom == id && p.idTollPlazaTo == idto).FirstOrDefault();
            }
        }

        public static List<TblFacilityImage> getFoodImage(string idRSA)
        {
            lock (BaseDatabase.locker)
            {
                var idFac = BaseDatabase.getDB().Table<TblFacilities>().Where(p => p.intFacilityType == 11 && p.idParent == idRSA && p.intParentType == 0).Select(p => p.idFacilities).ToList();
                return BaseDatabase.getDB().Table<TblFacilityImage>().Where(p => idFac.Contains(p.idFacilities)).ToList();
            }
        }


        public static List<BaseItem> getListFavorite()
        {
            lock (BaseDatabase.locker)
            {
                try
                {
                    List<BaseItem> mlstItem = new List<BaseItem>();
                    mlstItem.Add(null);
                    mlstItem.Add(null);

                    var liveFeed = getListCCTV();
                    if (liveFeed.Count > 0)
                    {
                        FavoriteHeader itemHeader = new FavoriteHeader() { strType = "LiveFeed" };
                        LiveFeedFavorite itemDetail = new LiveFeedFavorite() { LiveFeed = liveFeed };

                        BaseItem item1 = new BaseItem();
                        item1.Item = itemHeader;
                        mlstItem.Add(item1);
                        BaseItem item2 = new BaseItem();
                        item2.Item = itemDetail;
                        mlstItem.Add(item2);
                    }


                    var lstRSA = getListFavoriteRSA();
                    if (lstRSA.Count > 0)
                    {
                        FavoriteHeader rsaHeader = new FavoriteHeader() { strType = "RSA" };
                        RSAFavorite rsaFa = new RSAFavorite() { RSA = lstRSA };

                        BaseItem item1 = new BaseItem();
                        item1.Item = rsaHeader;
                        mlstItem.Add(item1);
                        BaseItem item2 = new BaseItem();
                        item2.Item = rsaFa;
                        mlstItem.Add(item2);

                    }

                    var tollPlaza = getListFavoriteTollPlazaWithHeader();
                    if (tollPlaza.Count > 0)
                    {
                        FavoriteHeader itemHeader = new FavoriteHeader() { strType = "TollPlaza" };
                        TollPlazaFavorite itemDetail = new TollPlazaFavorite() { TollPlaza = tollPlaza };

                        BaseItem item1 = new BaseItem();
                        item1.Item = itemHeader;
                        mlstItem.Add(item1);
                        BaseItem item2 = new BaseItem();
                        item2.Item = itemDetail;
                        mlstItem.Add(item2);

                    }
                    var nearby = getListFavoriteNearByWithHeader();
                    if (nearby.Count > 0)
                    {
                        FavoriteHeader itemHeader = new FavoriteHeader() { strType = "Nearby" };
                        NearbyFavorite itemDetail = new NearbyFavorite() { Nearby = nearby };

                        BaseItem item1 = new BaseItem();
                        item1.Item = itemHeader;
                        mlstItem.Add(item1);
                        BaseItem item2 = new BaseItem();
                        item2.Item = itemDetail;
                        mlstItem.Add(item2);

                    }

                    return mlstItem;

                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("getListFavorite", ex.Message);
                }
            }
            return null;
        }

        public static List<BaseItem> getListFacilities(List<string> lstHighway, List<string> lstFacilities, List<POITypeSetting> lstPOI)
        {
            var PARENT_RSA = 0;
            var PARENT_PETROL = 1;
            var PARENT_TollPlaza = 3;
            var PARENT_CSC = 2;
            var PARENT_FAC = 2;

            lock (BaseDatabase.locker)
            {
                var lstRSA = BaseDatabase.getDB().Table<TblRSA>().Where(p => p.idEntity == Cons.IdEntity).ToList();
                var lstCSC = BaseDatabase.getDB().Table<TblCSC>().Where(p => p.idEntity == Cons.IdEntity).ToList();
                var lstPetrol = BaseDatabase.getDB().Table<TblPetrolStation>().Where(p => p.idEntity == Cons.IdEntity).ToList();
                var lstTollPlaza = BaseDatabase.getDB().Table<TblTollPlaza>().Where(p => p.idEntity == Cons.IdEntity).ToList();
                var listFacilities = BaseDatabase.getDB().Table<TblFacilities>().Where(p => p.idEntity == Cons.IdEntity).ToList();
                var lstFacilitiType = BaseDatabase.getDB().Table<TblFacilityType>().ToList().Where(p => p.idEntity.ToString() == Cons.IdEntity).ToList();
                var listFacilitiesO = BaseDatabase.getDB().Table<TblFacilities>().Where(p => p.idEntity == Cons.IdEntity).ToList();
                var lstNearby = BaseDatabase.getDB().Table<TblNearby>().Where(p => p.idEntity == Cons.IdEntity).ToList();
                var lstNearbyCat = BaseDatabase.getDB().Table<TblNearbyCatg>().Where(p => p.idEntity == Cons.IdEntity).ToList();
                var lstBrand = BaseDatabase.getDB().Table<TblBrand>().Where(p => p.idEntity == Cons.IdEntity).ToList();


                if (lstHighway.Count > 0)
                {
                    lstRSA = lstRSA.Where(p => lstHighway.Contains(p.idHighway)).ToList();
                    lstCSC = lstCSC.Where(p => lstHighway.Contains(p.idHighway)).ToList();
                    lstPetrol = lstPetrol.Where(p => lstHighway.Contains(p.idHighway)).ToList();
                    lstTollPlaza = lstTollPlaza.Where(p => lstHighway.Contains(p.idHighway)).ToList();
                    listFacilities = listFacilities.Where(p => lstHighway.Contains(p.idHighway)).ToList();
                }

                if (lstFacilities.Count > 0)
                {
                    listFacilities = listFacilities.Where(p => lstFacilities.Contains(p.intFacilityType.ToString())).ToList();

                    lstRSA = lstRSA.Where(p => listFacilities.Where(p1 => p1.intParentType == PARENT_RSA).Select(p1 => p1.idParent).Contains(p.idRSA)).ToList();
                    lstCSC = lstCSC.Where(p => listFacilities.Where(p1 => p1.intParentType == PARENT_CSC).Select(p1 => p1.idParent).Contains(p.idCSC)).ToList();
                    lstTollPlaza = lstTollPlaza.Where(p => listFacilities.Where(p1 => p1.intParentType == PARENT_TollPlaza).Select(p1 => p1.idParent).Contains(p.idTollPlaza)).ToList();
                    lstPetrol = lstPetrol.Where(p => listFacilities.Where(p1 => p1.intParentType == PARENT_PETROL).Select(p1 => p1.idParent).Contains(p.idPetrol)).ToList();
                }


                if (lstPOI.Count > 0)
                {
                    if (lstPOI.Where(p => p.intNearby == 0 && p.intType == 3 && p.intDataPOIType == 1).Count() > 0)
                    {
                        //tollplaza
                        //var lstFTollPlaza = lstPOI.Where(p => p.intNearby == 0 && p.intType == 3 && p.intDataPOIType == 1).ToList();
                    }
                    else
                    {
                        lstTollPlaza.Clear();
                    }
                    if (lstPOI.Where(p => p.intNearby == 0 && p.intType == 0 && p.intDataPOIType == 1).Count() > 0)
                    {
                        List<string> lstFilter = new List<string>();
                        //rsa
                        if (lstPOI.Where(p => p.intNearby == 0 && p.intType == 0 && p.intDataPOIType == 1 && p.intRSAType == 5).Count() > 0)
                        {
                            //tunnel
                            lstFilter.Add("5");
                        }
                        if (lstPOI.Where(p => p.intNearby == 0 && p.intType == 0 && p.intDataPOIType == 1 && p.intRSAType == 6).Count() > 0)
                        {
                            //vista point
                            lstFilter.Add("6");
                        }
                        if (lstPOI.Where(p => p.intNearby == 0 && p.intType == 0 && p.intDataPOIType == 1 && p.intRSAType == 4).Count() > 0)
                        {
                            //interchange
                            lstFilter.Add("4");
                        }
                        if (lstFilter.Count > 0)
                        {
                            lstRSA = lstRSA.Where(p => lstFilter.Contains(p.strType)).ToList();
                        }
                       
                    }
                    else
                    {
                        lstRSA.Clear();
                    }
                    if (lstPOI.Where(p => p.intNearby == 0 && p.intType == 1 && p.intDataPOIType == 1).Count() == 0)
                    {
                        lstPetrol.Clear();
                    }
                    if (lstPOI.Where(p => p.intNearby == 0 && p.intType == 2 && p.intDataPOIType == 1).Count() == 0)
                    {
                        lstCSC.Clear();
                    }
                    if (lstPOI.Where(p => p.intNearby == 1 ).Count() > 0)
                    {
                        var lstType = lstPOI.Where(p => p.intNearby == 1).Select(p => p.intID).ToList();
                        lstNearby = lstNearby.Where(p => lstType.Contains(p.idNearbyCatg)).ToList();
                    }
                    else
                    {
                        lstNearby.Clear();
                    }
                }

                //listFacilities = 
                /*
                var listFacilitiesClone = listFacilities.ToList();
                //listFacilitiesClone.RemoveAll

                var lstremoversa = listFacilitiesClone.Where(p => lstRSA.Select(p1 => p1.idRSA).Contains(p.idParent) && p.intParentType == PARENT_RSA).ToList();
                foreach (var item in lstremoversa)
                {
                    listFacilitiesClone.Remove(item);
                }
                var lstremovecsc = listFacilitiesClone.Where(p => lstPetrol.Select(p1 => p1.idPetrol).Contains(p.idParent) && p.intParentType == PARENT_PETROL).ToList();
                foreach (var item in lstremovecsc)
                {
                    listFacilitiesClone.Remove(item);
                }
                var lstremovepertrol = listFacilitiesClone.Where(p => lstCSC.Select(p1 => p1.idCSC).Contains(p.idParent) && p.intParentType == PARENT_CSC).ToList();
                foreach (var item in lstremovepertrol)
                {
                    listFacilitiesClone.Remove(item);
                }
                var lstremovetollplaza = listFacilitiesClone.Where(p => lstTollPlaza.Select(p1 => p1.idTollPlaza).Contains(p.idParent) && p.intParentType == PARENT_TollPlaza).ToList();
                foreach (var item in lstremovetollplaza)
                {
                    listFacilitiesClone.Remove(item);
                }*/


                var result = new List<FacilityItem>();
                foreach (var itemRSA in lstRSA)
                {
                    try
                    {
                        FacilityItem itemF = new FacilityItem();
                        itemF.ID = itemRSA.idRSA;
                        itemF.IsFavorite = IsFavorite(itemF.ID, FavoriteType.RSA);
                        itemF.IdHighway = itemRSA.idHighway;
                        itemF.IsNearby = 0;
                        itemF.strName = itemRSA.strName;
                        itemF.UrlImg = Cons.myEntity.mSettings.rsa_icon;
                        itemF.Data = itemRSA;
                        //itemF.SubUrlImg 
                        itemF.Type = PARENT_RSA;

                        var lstf = listFacilitiesO.Where(p => p.intParentType == itemF.Type && p.idParent == itemF.ID).Select(p => p.intFacilityType).ToList();
                        itemF.SubUrlImg = lstFacilitiType.Where(p => !string.IsNullOrEmpty(p.strPicture)).Where(p => lstf.Contains(p.intFacilityType)).Select(p => p.strPicture).ToList();
                        result.Add(itemF);
                    }
                    catch (Exception ex)
                    {

                    }
                   
                }

                foreach (var itemPertrol in lstPetrol)
                {
                    try
                    {
                        var brand = lstBrand.Where(p => p.idBrand == itemPertrol.idBrand).FirstOrDefault();

                        FacilityItem itemF = new FacilityItem();
                        itemF.ID = itemPertrol.idPetrol;
                        itemF.IsFavorite = IsFavorite(itemF.ID, FavoriteType.PertrolStation);
                        itemF.IdHighway = itemPertrol.idHighway;
                        itemF.IsNearby = 0;
                        itemF.strName = itemPertrol.strName;
                        itemF.UrlImg = brand == null ? "" : brand.strPicture;
                        itemF.Data = itemPertrol;
                        //itemF.SubUrlImg 
                        itemF.Type = PARENT_PETROL;
                        var lstf = listFacilitiesO.Where(p => p.intParentType == itemF.Type && p.idParent == itemF.ID).Select(p => p.intFacilityType).ToList();
                        itemF.SubUrlImg = lstFacilitiType.Where(p => !string.IsNullOrEmpty(p.strPicture)).Where(p => lstf.Contains(p.intFacilityType)).Select(p => p.strPicture).ToList();
                        result.Add(itemF);
                    }
                    catch (Exception ex)
                    {

                    }
                   
                }

                foreach (var itemCSC in lstCSC)
                {
                    try
                    {
                        FacilityItem itemF = new FacilityItem();
                        itemF.ID = itemCSC.idCSC;
                        itemF.IdHighway = itemCSC.idHighway;
                        itemF.IsNearby = 0;
                        itemF.strName = itemCSC.strName;
                        itemF.UrlImg = Cons.myEntity.mSettings.csc_label;
                        itemF.Data = itemCSC;
                        //itemF.SubUrlImg 
                        itemF.Type = PARENT_CSC;
                        var lstf = listFacilitiesO.Where(p => p.intParentType == itemF.Type && p.idParent == itemF.ID).Select(p => p.intFacilityType).ToList();
                        itemF.SubUrlImg = lstFacilitiType.Where(p => !string.IsNullOrEmpty(p.strPicture)).Where(p => lstf.Contains(p.intFacilityType)).Select(p => p.strPicture).ToList();
                        result.Add(itemF);
                    }
                    catch (Exception ex)
                    {

                    }
                    
                }
                foreach (var itemTollPlaza in lstTollPlaza)
                {
                    try
                    {
                        FacilityItem itemF = new FacilityItem();
                        itemF.ID = itemTollPlaza.idTollPlaza;
                        itemF.IsFavorite = IsFavorite(itemF.ID, FavoriteType.TollPlaza);
                        itemF.IdHighway = itemTollPlaza.idHighway;
                        itemF.IsNearby = 0;
                        itemF.strName = itemTollPlaza.strName;
                        itemF.UrlImg = Cons.myEntity.mSettings.toll_plaza_icon;
                        itemF.Data = itemTollPlaza;
                        //itemF.SubUrlImg 
                        itemF.Type = PARENT_PETROL;
                        var lstf = listFacilitiesO.Where(p => p.intParentType == itemF.Type && p.idParent == itemF.ID).Select(p => p.intFacilityType).ToList();
                        itemF.SubUrlImg = lstFacilitiType.Where(p => !string.IsNullOrEmpty(p.strPicture)).Where(p => lstf.Contains(p.intFacilityType)).Select(p => p.strPicture).ToList();
                        result.Add(itemF);
                    }
                    catch (Exception ex)
                    {

                    }
                    
                }
                foreach (var itemNearby in lstNearby)
                {
                    try
                    {
                        var type = lstNearbyCat.Where(p => p.idNearbyCatg == itemNearby.idNearbyCatg).FirstOrDefault();
                        FacilityItem itemF = new FacilityItem();
                        itemF.ID = itemNearby.idNearby;
                        itemF.strCategoryNearbyName = type == null ? "#" : type.strNearbyCatgName;
                        itemF.IsFavorite = IsFavorite(itemF.ID, FavoriteType.Nearby);
                        //itemF.IdHighway = item.idh;
                        itemF.IsNearby = 1;
                        itemF.Data = itemNearby;
                        itemF.strName = itemNearby.strTitle;
                        itemF.UrlImg = type == null ? "": type.strNearbyCatgImg;
                        result.Add(itemF);

                    }
                    catch (Exception ex)
                    {

                    }
                   
                }

                //foreach (var item in listFacilitiesClone)
                //{
                //    var type = lstFacilitiType.Where(p => p.intFacilityType == item.intFacilityType).FirstOrDefault();
                //    //var type = lstNearbyCat.Where(p => p.idNearbyCatg == item.idNearbyCatg).FirstOrDefault();
                //    FacilityItem itemF = new FacilityItem();
                //    itemF.ID = item.idFacilities;
                //    itemF.IdHighway = item.idHighway;
                //    itemF.IsNearby = 0;
                //    itemF.strName = item.strName;
                //    itemF.Type = PARENT_FAC;
                //    itemF.UrlImg = type == null ? "" : type.strPicture;
                //    result.Add(itemF);
                //}

                var listBaseItem = new List<BaseItem>();
                result = result.OrderBy(p => p.IdHighway).ThenBy(p => p.IsNearby).ThenBy(p => p.Type).ToList();
                string lastIdHighway = "";
                foreach (var item in result)
                {
                    if (lastIdHighway != item.IdHighway)
                    {
                        if (string.IsNullOrEmpty(item.IdHighway))
                        {
                            BaseItem itemH = new BaseItem();
                            TblHighway highway = new TblHighway();
                            highway.strName = "No Highway";
                            itemH.Item =  highway;// getHighway(item.IdHighway.ToString());
                            listBaseItem.Add(itemH);
                            lastIdHighway = item.IdHighway;
                        }
                        else
                        {
                            BaseItem itemH = new BaseItem();
                            itemH.Item = getHighway(item.IdHighway.ToString());
                            listBaseItem.Add(itemH);
                            lastIdHighway = item.IdHighway;
                        }
                        
                    }

                    BaseItem itemB = new BaseItem();
                    itemB.Item = item;
                    listBaseItem.Add(itemB);

                }
                string jsonData = JsonConvert.SerializeObject(listBaseItem);
                return listBaseItem;
            }
        }

    }
}
