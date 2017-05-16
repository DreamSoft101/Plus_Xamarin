using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using EXPRESSO.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EXPRESSO.Models.EnumType;

namespace EXPRESSO.Processing.Connections
{
    public static class UpdateConnections
    {
        public static async Task<string> getUpdateData(string strLastUpdate)
        {
            try
            {
                MyEntity entity = Cons.myEntity;

                List<TblEntities> result = new List<TblEntities>();
                BaseClient client = new BaseClient();
                client.StrMethod = "getNewData";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                client.AddParameter("dtLastUpdateDate", strLastUpdate);
                string strJson = await client.getData();
                return strJson;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<ServiceResult> GenerateData(string strJson)
        {
            var itemJson = JObject.Parse(strJson);
            var itemResult = itemJson["result"];
            if (itemResult["intStatus"].AsInt() == 1)
            {
                var data = itemResult["data"];
                List<BaseItem> lstUpdate = new List<BaseItem>();

                if (data == null)
                {
                    return new ServiceResult() { Data = lstUpdate, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
                if (data["tblBrand"] != null)
                {
                    foreach (var item in data["tblBrand"])
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TblBrand itemUpdate = new TblBrand();
                            string idBrand = item["idBrand"].AsString();
                            int intBrandType = item["intBrandType"].AsInt();
                            string strName = item["strName"].AsString();
                            string strDescription = item["strDescription"].AsString();
                            string strPicture = item["strPicture"].AsString();
                            int intStatus = item["intStatus"].AsInt();
                            itemUpdate.idBrand = idBrand;
                            itemUpdate.intBrandType = intBrandType;
                            itemUpdate.strName = strName;
                            itemUpdate.strDescription = strDescription;
                            itemUpdate.strPicture = strPicture;
                            itemUpdate.intStatus = intStatus;
                            itemUpdate.idEntity = Cons.IdEntity;
                            bItem.Item = itemUpdate;
                            lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("itemJson.result.data.tblBrand", ex.Message);
                        }
                    }
                }

                if (data["tblHighway"] != null)
                    foreach (var item in data["tblHighway"])
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TblHighway itemUpdate = new TblHighway();
                            string idHighway = item["idHighway"].AsString();
                            string strName = item["strName"].AsString();
                            string strDescription = item["strDescription"].AsString();
                            double floStartLong = item["floStartLong"].AsDouble();
                            double floStartLat = item["floStartLat"].AsDouble();
                            double floEndLong = item["floEndLong"].AsDouble();
                            double floEndLat = item["floEndLat"].AsDouble();
                            string strURLPath = item["strURLPath"].AsString();
                            int intStatus = item["intStatus"].AsInt();
                            itemUpdate.idHighway = idHighway;
                            itemUpdate.strName = strName;
                            itemUpdate.strDescription = strDescription;
                            itemUpdate.decStartLong = floStartLong;
                            itemUpdate.decStartLat = floEndLat;
                            itemUpdate.decEndLong = floEndLong;
                            itemUpdate.decEndLat = floEndLat;
                            itemUpdate.intStatus = intStatus;
                            itemUpdate.idEntity = Cons.IdEntity;
                            bItem.Item = itemUpdate;
                            lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("getUpdateData", ex.Message);
                        }
                    }

                if (data["tblTollPlaza"] != null)
                    foreach (var item in data["tblTollPlaza"])
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TblTollPlaza itemUpdate = new TblTollPlaza();
                            string idTollPlaza = item["idTollPlaza"].AsString();
                            string strType = item["strType"].AsString();
                            string strName = item["strName"].AsString();
                            string idHighway = item["idHighway"].AsString();
                            string strSection = item["strSection"].AsString();
                            string strExit = item["strExit"].AsString();
                            string strReloadTimeFrom = item["strReloadTimeFrom"].AsString();
                            string strReloadTimeTo = item["strReloadTimeTo"].AsString();
                            double decLocation = item["decLocation"].AsDouble();
                            double floLong = item["floLong"].AsDouble();
                            double floLat = item["floLat"].AsDouble();
                            string strPicture = item["strPicture"].AsString();
                            int intSort = item["intSort"].AsInt();
                            int intStatus = item["intStatus"].AsInt();
                            itemUpdate.idTollPlaza = idTollPlaza;
                            itemUpdate.strType = strType;
                            itemUpdate.strName = strName;
                            itemUpdate.idHighway = idHighway;
                            itemUpdate.strSection = strSection;
                            itemUpdate.strExit = strExit;
                            itemUpdate.strReloadTimeFrom = strReloadTimeFrom;
                            itemUpdate.strReloadTimeTo = strReloadTimeTo;
                            itemUpdate.decLocation = decLocation;
                            itemUpdate.decLong = floLong;
                            itemUpdate.decLat = floLat;
                            itemUpdate.strPicture = strPicture;
                            itemUpdate.intSort = intSort;
                            itemUpdate.intStatus = intStatus;
                            itemUpdate.idEntity = Cons.IdEntity;
                            bItem.Item = itemUpdate;
                            lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("itemJson.result.data.tblTollPlaza", ex.Message);
                        }
                    }

                if (data["tblPetrolStation"] != null)
                    foreach (var item in data["tblPetrolStation"])
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TblPetrolStation itemUpdate = new TblPetrolStation();
                            string idPetrol = item["idPetrol"].AsString();
                            string strName = item["strName"].AsString();
                            string idBrand = item["idBrand"].AsString();
                            string idRSA = item["idRSA"].AsString();
                            string idHighway = item["idHighway"].AsString();
                            string strSection = item["strSection"].AsString();
                            string strDirection = item["strDirection"].AsString();
                            double decLocation = item["decLocation"].AsDouble();
                            double floLong = item["floLong"].AsDouble();
                            double floLat = item["floLat"].AsDouble();
                            string strPicture = item["strPicture"].AsString();
                            int intSort = item["intSort"].AsInt();
                            int intStatus = item["intStatus"].AsInt();
                            itemUpdate.idPetrol = idPetrol;
                            itemUpdate.strName = strName;
                            itemUpdate.idBrand = idBrand;
                            itemUpdate.idRSA = idRSA;
                            itemUpdate.idHighway = idHighway;
                            itemUpdate.strSection = strSection;
                            itemUpdate.strDirection = strDirection;
                            itemUpdate.decLocation = decLocation;
                            itemUpdate.decLong = floLong;
                            itemUpdate.decLat = floLat;
                            itemUpdate.strPicture = strPicture;
                            itemUpdate.intSort = intSort;
                            itemUpdate.intStatus = intStatus;
                            itemUpdate.idEntity = Cons.IdEntity;
                            bItem.Item = itemUpdate;
                            lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("itemJson.result.data.tblPetrolStation", ex.Message);
                        }
                    }


                if (data["tblRSA"] != null)
                    foreach (var item in data["tblRSA"])
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TblRSA itemUpdate = new TblRSA();

                            string idRSA = item["idRSA"].AsString();
                            string strType = item["strType"].AsString();
                            string strName = item["strName"].AsString();
                            string idHighway = item["idHighway"].AsString();
                            string strSection = item["strSection"].AsString();
                            string strDirection = item["strDirection"].AsString();
                            double decLocation = item["decLocation"].AsDouble();
                            double floLong = item["floLong"].AsDouble();
                            double floLat = item["floLat"].AsDouble();
                            string strPicture = item["strPicture"].AsString();
                            string strSignatureName = item["strSignatureName"].AsString();
                            int intSort = item["intSort"].AsInt();
                            int intStatus = item["intStatus"].AsInt();
                            itemUpdate.idRSA = idRSA;
                            itemUpdate.strType = strType;
                            itemUpdate.strName = strName;
                            itemUpdate.idHighway = idHighway;
                            itemUpdate.strSection = strSection;
                            itemUpdate.strDirection = strDirection;
                            itemUpdate.decLocation = decLocation;
                            itemUpdate.decLong = floLong;
                            itemUpdate.decLat = floLat;
                            itemUpdate.strPicture = strPicture;
                            itemUpdate.intSort = intSort;
                            itemUpdate.intStatus = intStatus;
                            itemUpdate.strSignatureName = strSignatureName;
                            itemUpdate.idEntity = Cons.IdEntity;
                            bItem.Item = itemUpdate;
                            lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {

                            LogUtils.WriteError("itemJson.result.data.tblRSA", ex.Message);
                        }
                    }

                if (data["tblCSC"] != null)
                    foreach (var item in data["tblCSC"])
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TblCSC itemUpdate = new TblCSC();
                            string idCSC = item["idCSC"].AsString();
                            string strType = item["strType"].AsString();
                            string strName = item["strName"].AsString();
                            string idHighway = item["idHighway"].AsString();
                            string strSection = item["strSection"].AsString();
                            string strDirection = item["strDirection"].AsString();
                            double decLocation = item["decLocation"].AsDouble();
                            double floLong = item["floLong"].AsDouble();
                            double floLat = item["floLat"].AsDouble();
                            string strPicture = item["strPicture"].AsString();
                            string strOperationHour = item["strOperationHour"].AsString();
                            string strContactNumber = item["strContactNumber"].AsString();
                            int intSort = item["intSort"].AsInt();
                            int intStatus = item["intStatus"].AsInt();
                            itemUpdate.idCSC = idCSC;
                            itemUpdate.strType = strType;
                            itemUpdate.strName = strName;
                            itemUpdate.idHighway = idHighway;
                            itemUpdate.strSection = strSection;
                            itemUpdate.strDirection = strDirection;
                            itemUpdate.decLocation = decLocation;
                            itemUpdate.decLong = floLong;
                            itemUpdate.decLat = floLat;
                            //itemUpdate.strPicture = strPicture;
                            itemUpdate.intSort = intSort;
                            itemUpdate.intStatus = intStatus;
                            itemUpdate.idEntity = Cons.IdEntity;
                            bItem.Item = itemUpdate;
                            lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("itemJson.result.data.tblCSC", ex.Message);
                        }
                    }

                if (data["tblFacilityType"] != null)
                    foreach (var item in data["tblFacilityType"])
                    {
                        try
                        {
                            BaseItem bItemFacilityType = new BaseItem();
                            TblFacilityType itemFacilityType = new TblFacilityType();
                            int intFacilityType = item["intFacilityType"].AsInt();
                            string strName = item["strName"].AsString();
                            string strPicture = item["strPicture"].AsString();
                            int intBrandType = 0;
                            try
                            {
                                intBrandType = item["intBrandType"].AsInt();
                            }
                            catch (Exception ex)
                            {

                            }
                            int intFeatured = 0;
                            try
                            {
                                intFeatured = item["intFeatured"].AsInt();
                            }
                            catch (Exception ex)
                            {

                            }
                            int intSort = 0;
                            try
                            {
                                intSort = item["intSort"].AsInt();
                            }
                            catch (Exception ex)
                            {

                            }
                            int intStatus = item["intStatus"].AsInt();

                            itemFacilityType.idEntity = Convert.ToInt32(Cons.myEntity.idEntity);
                            itemFacilityType.intFacilityType = intFacilityType;
                            itemFacilityType.strName = strName;
                            itemFacilityType.strPicture = strPicture;
                            itemFacilityType.intBrandType = intBrandType;
                            itemFacilityType.intFeatured = intFeatured;
                            itemFacilityType.intSort = intSort;
                            itemFacilityType.intStatus = intStatus;


                            bItemFacilityType.Item = itemFacilityType;
                            lstUpdate.Add(bItemFacilityType);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("itemJson.result.data.tblFacilityType", ex.Message);
                            //LogUtils.WriteError("itemJson.result.data.tblFacilityType", item.ToString());
                        }
                    }
                if (data["tblFacilities"] != null)
                    foreach (var item in data["tblFacilities"])
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TblFacilities itemUpdate = new TblFacilities();
                            string idFacilities = item["idFacilities"].AsString();
                            int intFacilityType = item["intFacilityType"].AsInt();
                            string strName = item["strName"].AsString();
                            string strDescription = item["strDescription"].AsString();
                            string strPicture = item["strPicture"].AsString();
                            string strManager = item["strManager"].AsString();
                            string strContactNumber = item["strContactNumber"].AsString();
                            int intParentType = item["intParentType"].AsInt();
                            string idParent = item["idParent"].AsString();
                            string idHighway = item["idHighway"].AsString();
                            string strSection = item["strSection"].AsString();
                            string strDirection = item["strDirection"].AsString();
                            double decLocation = 0;
                            try
                            {
                                decLocation = item["decLocation"].AsInt();
                            }
                            catch(Exception ex)
                            {

                            }
                            double floLong = 0;
                            try
                            {
                                decLocation = item["floLong"].AsDouble();
                            }
                            catch (Exception ex)
                            {

                            }
                            double floLat = 0;
                            try
                            {
                                decLocation = item["floLat"].AsDouble();
                            }
                            catch (Exception ex)
                            {

                            }

                            int intSort = item["intSort"].AsInt(); ;
                            int intStatus = item["intStatus"].AsInt();
                            itemUpdate.idFacilities = idFacilities;
                            itemUpdate.intFacilityType = intFacilityType;
                            itemUpdate.strName = strName;
                            itemUpdate.strDescription = strDescription;
                            itemUpdate.strManager = strManager;
                            itemUpdate.strContactNumber = strContactNumber;
                            itemUpdate.intParentType = intParentType;
                            itemUpdate.idParent = idParent;
                            itemUpdate.idHighway = idHighway;
                            itemUpdate.strSection = strSection;
                            itemUpdate.strDirection = strDirection;
                            itemUpdate.decLocation = decLocation;
                            itemUpdate.decLong = floLong;
                            itemUpdate.decLat = floLat;
                            itemUpdate.strPicture = strPicture;
                            itemUpdate.intSort = intSort;
                            itemUpdate.intStatus = intStatus;
                            itemUpdate.idEntity = Cons.IdEntity;
                            bItem.Item = itemUpdate;
                            lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("itemJson.result.data.tblFacilities", ex.Message);
                            //LogUtils.WriteError("itemJson.result.data.tblFacilities", item.ToString());
                        }
                    }

                if (data["tblTollFare"] != null)
                    foreach (var item in data["tblTollFare"])
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TblTollFare itemUpdate = new TblTollFare();
                            string idTollFare = item["idTollFare"].AsString();
                            string idTollPlazaFrom = item["idTollPlazaFrom"].AsString();
                            string idTollPlazaTo = item["idTollPlazaTo"].AsString();
                            decimal decTollAmt1 = item["decTollAmt1"].AsDecimal();
                            decimal decTollAmt2 = item["decTollAmt2"].AsDecimal();
                            decimal decTollAmt3 = item["decTollAmt3"].AsDecimal();
                            decimal decTollAmt4 = item["decTollAmt4"].AsDecimal();
                            decimal decTollAmt5 = item["decTollAmt5"].AsDecimal();
                            int intStatus = item["intStatus"].AsInt();
                            itemUpdate.idTollFare = idTollFare;
                            itemUpdate.idTollPlazaFrom = idTollPlazaFrom;
                            itemUpdate.idTollPlazaTo = idTollPlazaTo;
                            itemUpdate.decTollAmt1 = decTollAmt1;
                            itemUpdate.decTollAmt2 = decTollAmt2;
                            itemUpdate.decTollAmt3 = decTollAmt3;
                            itemUpdate.decTollAmt4 = decTollAmt4;
                            itemUpdate.decTollAmt5 = decTollAmt5;
                            itemUpdate.intStatus = intStatus;
                            itemUpdate.idEntity = Cons.IdEntity;
                            bItem.Item = itemUpdate;
                            lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("itemJson.result.data.tblTollFare", ex.Message);
                        }
                    }

                if (data["tblRoute"] != null)
                    foreach (var item in data["tblRoute"])
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TblRoute itemUpdate = new TblRoute();
                            string idRoute = item["idRoute"].AsString();
                            string strRouteName = item["strRouteName"].AsString();
                            int intStatus = item["intStatus"].AsInt();
                            itemUpdate.idRoute = idRoute;
                            itemUpdate.strRouteName = strRouteName;
                            itemUpdate.intStatus = intStatus;
                            itemUpdate.idEntity = Cons.IdEntity;
                            bItem.Item = itemUpdate;
                            lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("itemJson.result.data.tblRoute", ex.Message);
                        }
                    }

                if (data["tblRouteDetail"] != null)
                    foreach (var item in data["tblRouteDetails"])
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TblRouteDetails itemUpdate = new TblRouteDetails();
                            string idRoute = item["idRoute"].AsString();
                            int intSeq = item["intSeq"].AsInt();
                            int intType = item["intType"].AsInt();
                            string idRouteItem = item["idRouteItem"].AsString();
                            double decLocation = item["decLocation"].AsDouble();
                            string strExit = item["strExit"].AsString();
                            int intStatus = item["intStatus"].AsInt();

                            itemUpdate.idRoute = idRoute;
                            itemUpdate.intSeq = intSeq;
                            itemUpdate.intType = intType;
                            itemUpdate.idRouteItem = idRouteItem;
                            itemUpdate.decLocation = decLocation;
                            itemUpdate.strExit = strExit;
                            itemUpdate.intStatus = intStatus;
                            itemUpdate.idEntity = Cons.IdEntity;
                            bItem.Item = itemUpdate;
                            //dont use more
                            //nthoa
                            //lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("itemJson.result.data.tblRouteDetails", ex.Message);
                        }
                    }

                if (data["tblNearbyCatg"] != null)
                    foreach (var item in data["tblNearbyCatg"])
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TblNearbyCatg itemUpdate = new TblNearbyCatg();
                            string idNearbyCatg = item["idNearbyCatg"].AsString();
                            string strNearbyCatgName = item["strNearbyCatgName"].AsString();
                            string strNearbyCatgImg = item["strNearbyCatgImg"].AsString();
                            int intStatus = item["intStatus"].AsInt();
                            itemUpdate.idNearbyCatg = idNearbyCatg;
                            itemUpdate.strNearbyCatgName = strNearbyCatgName;
                            itemUpdate.strNearbyCatgImg = strNearbyCatgImg;
                            itemUpdate.intStatus = intStatus;
                            itemUpdate.idEntity = Cons.IdEntity;
                            bItem.Item = itemUpdate;
                            lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("itemJson.result.data.tblNearbyCatg", ex.Message);
                        }
                    }

                if (data["tblNearby"] != null)
                    foreach (var item in data["tblNearby"])
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TblNearby itemUpdate = new TblNearby();
                            string idNearby = item["idNearby"].AsString();
                            string idNearbyCatg = item["idNearbyCatg"].AsString();
                            string strTitle = item["strTitle"].AsString();
                            string strAddress = item["strAddress"].AsString();
                            string strContactNo = item["strContactNo"].AsString();
                            string strWebsite = item["strWebsite"].AsString();
                            string strEmail = item["strEmail"].AsString();
                            double floLatitude = item["floLatitude"].AsDouble();
                            double floLongitude = item["floLongitude"].AsDouble();
                            string strDescription = item["strDescription"].AsString();
                            string strLocationImg = item["strLocationImg"].AsString();
                            DateTime? dtValidFromDate = item["dtValidFromDate"].AsDateTime();
                            DateTime? dtValidToDate = item["dtValidToDate"].AsDateTime();
                            DateTime? dtDisplayFromDate = item["dtDisplayFromDate"].AsDateTime();
                            DateTime? dtDisplayToDate = item["dtDisplayToDate"].AsDateTime();
                            string strTermsConditions = item["strTermsConditions"].AsString();
                            string strInternalComments = item["strInternalComments"].AsString();
                            string strSMSMsg = item["strSMSMsg"].AsString();
                            string strEmailMsg = item["strEmailMsg"].AsString();
                            string strFacebookMsg = item["strFacebookMsg"].AsString();
                            string strTwitterMsg = item["strTwitterMsg"].AsString();
                            int intStatus = item["intStatus"].AsInt();

                            itemUpdate.idNearby = idNearby;
                            itemUpdate.idNearbyCatg = idNearbyCatg;
                            itemUpdate.strTitle = strTitle;
                            itemUpdate.strAddress = strAddress;
                            itemUpdate.strContactNo = strContactNo;
                            itemUpdate.strWebsite = strWebsite;
                            itemUpdate.strEmail = strEmail;
                            itemUpdate.floLatitude = floLatitude;
                            itemUpdate.floLongitude = floLongitude;
                            itemUpdate.strDescription = strDescription;
                            itemUpdate.strLocationImg = strLocationImg;
                            itemUpdate.dtValidFromDate = dtValidFromDate;
                            itemUpdate.dtValidToDate = dtValidToDate;
                            itemUpdate.dtDisplayFromDate = dtDisplayFromDate;
                            itemUpdate.dtDisplayToDate = dtDisplayToDate;
                            itemUpdate.strTermsConditions = strTermsConditions;
                            //itemUpdate.strInternalComments = strInternalComments;
                            itemUpdate.strSMSMsg = strSMSMsg;
                            itemUpdate.strEmailMsg = strEmailMsg;
                            itemUpdate.strFacebookMsg = strFacebookMsg;
                            itemUpdate.strTwitterMsg = strTwitterMsg;
                            itemUpdate.intStatus = intStatus;

                            itemUpdate.idEntity = Cons.IdEntity;
                            bItem.Item = itemUpdate;
                            lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("itemJson.result.data.tblNearby", ex.Message);
                        }
                    }

                if (data["tblFacilityImage"] != null)
                    foreach (var item in data["tblFacilityImage"])
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TblFacilityImage itemUpdate = new TblFacilityImage();
                            string idFacilityImg = item["idFacilityImg"].AsString();
                            string idFacilities = item["idFacilities"].AsString();
                            string strPicture = item["strPicture"].AsString();
                            string strDescription = item["strDescription"].AsString();
                            int intSort = item["intSort"].AsInt();
                            int intStatus = item["intStatus"].AsInt();



                            itemUpdate.idFacilityImg = idFacilityImg;
                            itemUpdate.idFacilities = idFacilities;
                            itemUpdate.strPicture = strPicture;
                            itemUpdate.strDescription = strDescription;
                            itemUpdate.intSort = intSort;
                            itemUpdate.intStatus = intStatus;
                           

                            itemUpdate.idEntity = Cons.IdEntity;
                            bItem.Item = itemUpdate;
                            lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("itemJson.result.data.tblFacilityImage", ex.Message);
                        }
                    }
                if (data["tblRSACctv"] != null)
                    foreach (var item in data["tblRSACctv"])
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            string jsonItem = item.ToString();
                            TblRSACctv itemRSACCTV = new TblRSACctv();
                            string idRSAcctv = item["idRSAcctv"].AsString();
                            string idParent = item["idParent"].AsString();
                            int intParentType = item["intParentType"].AsInt();
                            string strTitle = item["strTitle"].AsString();
                            string strURL = item["strURL"].AsString();
                            string strDescription = item["strDescription"].AsString();
                            string intFrequency = item["intFrequency"].AsString();
                            int intVisible = item["intVisible"].AsInt();
                            int intStatus = item["intStatus"].AsInt();


                            itemRSACCTV.idRSAcctv = idRSAcctv;
                            itemRSACCTV.idParent = idParent;
                            itemRSACCTV.intParentType = intParentType;
                            itemRSACCTV.strTitle = strTitle;
                            itemRSACCTV.strURL = strURL;
                            itemRSACCTV.strDescription = strDescription;
                            itemRSACCTV.intVisible = intVisible;
                            itemRSACCTV.intStatus = intStatus;

                            itemRSACCTV.idEntity = Cons.IdEntity;

                            bItem.Item = itemRSACCTV;
                            lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("itemJson.result.data.tblRSACctv", ex.Message);
                        }
                    }
                return new ServiceResult() { Data = lstUpdate, intStatus = 1 };
            }
            else
            {
                LogUtils.WriteError("getUpdateData", "intStatus:" + itemJson["result"]["intStatus"].AsInt());
                return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
            }
        }
    }
}
