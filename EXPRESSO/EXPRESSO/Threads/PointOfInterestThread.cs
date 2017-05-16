using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using EXPRESSO.Processing.Connections;
using EXPRESSO.Processing.LocalData;
using EXPRESSO.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EXPRESSO.Models.EnumType;

namespace EXPRESSO.Threads
{
    public class PointOfInterestThread
    {
        
        public delegate void onLoadCSC(List<BaseItem> result);
        public onLoadCSC OnLoadCSC;
        public async void loadCSC()
        {
            try
            {
             
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadListAnnoucementScroller", ex.Message);
            }
            var result = await Task.Run(() => LoadData.getCSCWithHeader());
            //return result;
            if (OnLoadCSC != null)
            {
                OnLoadCSC(result);
            }
        }

        public delegate void onLoadPertrolStation(List<BaseItem> result);
        public onLoadPertrolStation OnLoadPertrolStation;
        public async void loadPertrolStation( )
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadListAnnoucementScroller", ex.Message);
            }
            var result = await Task.Run(() => LoadData.getListPertrolStation());
            //return result;
            if (OnLoadPertrolStation != null)
            {
                OnLoadPertrolStation(result);
            }
        }

        public delegate void onLoadRSA(List<BaseItem> result);
        public onLoadRSA OnLoadRSA;
        public async void loadRSA(FacilitiesType type)
        {
            try
            {
                var result = await Task.Run(() => LoadData.getListRSA(type));
                //return result;
                if (OnLoadRSA != null)
                {
                    OnLoadRSA(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadRSA", ex.Message);
            }
          
        }

        public delegate void onLoadTollPlaza(ServiceResult result);
        public onLoadTollPlaza OnLoadTollPlaza;
        public async void loadTollPlaza(List<string> idHighway)
        {
            try
            {
                var resultHighway = await Task.Run(() => LiveTrafficConnections.GetTollPlaza(idHighway));
                //return result;
                if (OnLoadTollPlaza != null)
                {
                    OnLoadTollPlaza(resultHighway);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadTollPlaza", ex.Message);
            }
          
        }


        public delegate void onLoadTollPlazaInfo(TblTollPlaza toll);
        public onLoadTollPlazaInfo OnLoadTollPlazaInfo;
        public async void loadTollPlaza(string idHighway, string idTollPlaza)
        {
            try
            {
                var resultHighway = await Task.Run(() => LiveTrafficConnections.GetTollPlaza(new List<string>() { idHighway }, idTollPlaza));
                //return result;
                if (OnLoadTollPlazaInfo != null)
                {
                    OnLoadTollPlazaInfo(resultHighway);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadTollPlaza", ex.Message);
            }

        }


        public delegate void onLoadNearBy(List<BaseItem> result);
        public onLoadNearBy OnLoadNearBy;
        public async void loadNearby(string idCategory)
        {
            try
            {
                var result = await Task.Run(() => LoadData.getNearby(idCategory));
                //return result;
                if (OnLoadNearBy != null)
                {
                    OnLoadNearBy(result);
                }

            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadListAnnoucementScroller", ex.Message);
            }
            
        }

        public delegate void onLoadNearbyCategory(List<TblNearbyCatg> result);
        public onLoadNearbyCategory OnLoadNearbyCategory;

        public delegate void onLoadFacilityType(List<TblFacilityType> result);
        public onLoadFacilityType OnLoadFacilityType;
        public async void loadHomePointOfInterest()
        {
            try
            {
                var lstFacilityType = LoadData.getListFacilityType();
                if (OnLoadFacilityType != null)
                {
                    OnLoadFacilityType(lstFacilityType);
                }

                var result = LoadData.getNearbyCategory();
                //return result;
                if (OnLoadNearbyCategory != null)
                {
                    OnLoadNearbyCategory(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadHomePointOfInterest", ex.Message);
            }
            
        }

        public async void loadFavoriteType()
        {
            try
            {
                var lstFacilityType = LoadData.getListFacilityType();
                if (OnLoadFacilityType != null)
                {
                    OnLoadFacilityType(lstFacilityType);
                }

                var result = LoadData.getNearbyCategory();
                //return result;
                if (OnLoadNearbyCategory != null)
                {
                    OnLoadNearbyCategory(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadFavoriteType", ex.Message);
            }

        }


        public delegate void onGetNearbyCategoryItem(TblNearbyCatg item);
        public onGetNearbyCategoryItem OnGetNearbyCategoryItem;
        public async void getNearbyCategoryItem(string ID)
        {
            try
            {
                var result = LoadData.getNearbyCategory().Where(p => p.idNearbyCatg == ID).FirstOrDefault();
                if (OnGetNearbyCategoryItem != null)
                {
                    OnGetNearbyCategoryItem(result);
                }

            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadHomePointOfInterest", ex.Message);
            }

        }


        public delegate void onLoadPlusSmile(List<BaseItem> result);
        public onLoadPlusSmile OnLoadPlusSmile;

        public async void loadPLUSSmile()
        {
            try
            {
                var result = await Task.Run(() => LoadData.getPlusSmile());
                if (OnLoadPlusSmile != null)
                {
                    OnLoadPlusSmile(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadPLUSSmile", ex.Message);
            }
            
        }

        public delegate void onInterchange(List<BaseItem> result);
        public onLoadPlusSmile OnInterchange;

        public async void loadInterchange()
        {
            try
            {
                var result = await Task.Run(() => LoadData.getInterchange());
                if (OnInterchange != null)
                {
                    OnInterchange(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadInterchange", ex.Message);
            }
            
        }


        public delegate void onLoadTNGReload(List<BaseItem> result);
        public onLoadTNGReload OnLoadTNGReload;

        public async void loadTNGReload()
        {
            try
            {
                var result = await Task.Run(() => LoadData.getTNGReload());
                if (OnLoadTNGReload != null)
                {
                    OnLoadTNGReload(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadTNGReload", ex.Message);
            }
            
        }

        public delegate void onLoadSSK(List<BaseItem> result);
        public onLoadSSK OnLoadSSK;

        public async void loadSSK()
        {
            try
            {
                var result = await Task.Run(() => LoadData.getSSK());
                if (OnLoadSSK != null)
                {
                    OnLoadSSK(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadSSK", ex.Message);
            }
            
        }

        public delegate void onLoadTunnel(List<BaseItem> result);
        public onLoadTunnel OnLoadTunnel;

        public async void loadTunnel()
        {
            try
            {
                var result = await Task.Run(() => LoadData.getTunnel());
                if (OnLoadTunnel != null)
                {
                    OnLoadTunnel(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadTunnel", ex.Message);
            }
            
        }

        public delegate void onLoadVistaPoint(List<BaseItem> result);
        public onLoadVistaPoint OnLoadVistaPoint;

        public async void loadVistaPoint()
        {
            try
            {
                var result = await Task.Run(() => LoadData.getVistapoint());
                if (OnLoadVistaPoint != null)
                {
                    OnLoadVistaPoint(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadVistaPoint", ex.Message);
            }
            
        }


        public delegate void onLoadRSACCTV(ServiceResult result);
        public onLoadRSACCTV OnLoadRSACCTV;

        public async void loadCCTVRSA(string id)
        {
            try
            {
                var result = await Task.Run(() => PointsOfInterestConnection.getRSACCTV(id));
                if (OnLoadRSACCTV != null)
                {
                    OnLoadRSACCTV(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadCCTVRSA", ex.Message);
            }

        }

    

        public delegate void onCheckCCTVRSA(bool result);
        public onCheckCCTVRSA OnCheckCCTVRSA;
        public async void isCCTVRSA(string id)
        {
            try
            {
                var result = await Task.Run(() => LoadData.getListRSACCTV(id));
                if (OnCheckCCTVRSA != null)
                {
                    OnCheckCCTVRSA(result.Count > 0);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("isCCTVRSA", ex.Message);
            }
        }

        //
        public async void loadPOIDetail(string id, string type, string poitype)
        {
            try
            {
                var result = await Task.Run(() => PointsOfInterestConnection.POIDetail(id, type, poitype));
               
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadCCTVRSA", ex.Message);
            }

        }



        public delegate void onloadHighwayResult(TblHighway tblHighway);
        public onloadHighwayResult OnloadHighwayResult;

        public delegate void onloadListFacilityPageByParent(List<BaseItem> result);
        public onloadListFacilityPageByParent OnloadListFacilityPageByParent;

        public delegate void onLoadListCCTVRSA(string idRSA);
        public onLoadListCCTVRSA OnLoadListCCTVRSA;
        public enum ModelType
        {
            RSA = 1,
            Layby = 2,
            Pertrol = 3,
            CSC = 4, 
        }
        public enum LoadType
        {
            RSA = 1,
            TollPlaza = 2,
            Other = 0
        }
        public async void loadFacilityDetail(string id,string idHighway, LoadType type)
        {
            try
            {
                var resultHighway = await Task.Run(() => Processing.LocalData.LoadData.getHighway(idHighway));
                if (OnloadHighwayResult != null)
                {
                    OnloadHighwayResult(resultHighway);
                }
                var resultFacility = await Task.Run(() => LoadData.getListFacilityByParent(id));
                if (OnloadListFacilityPageByParent != null)
                {
                    OnloadListFacilityPageByParent(resultFacility);
                }
                if (type != LoadType.Other)
                {
                    if (type == LoadType.RSA)
                    {
                        //var resultCCTV = await Task.Run(() => PointsOfInterestConnection.getRSACCTV(id));
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadFacilityDetail", ex.Message);
            }
            
        }

        public async void LoadFacilityDetail(string id, int type)
        {

        }

        public delegate void onGetFacilityImageRSA(List<TblFacilityImage> images);
        public onGetFacilityImageRSA OnGetFacilityImageRSA;

        public async void LoadFacilityImagesRSA(string idRSA)
        {
            try
            {
                var images = await Task.Run(() => Processing.LocalData.LoadData.getFoodImage(idRSA));
                
                if (OnGetFacilityImageRSA != null)
                {
                    OnGetFacilityImageRSA(images);
                }


            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadFacilityDetail", ex.Message);
            }

        }



        public delegate void onGetFacilities(List<BaseItem> items);
        public onGetFacilities OnGetFacilities;

        public async void GetFacilities(List<string> lstHighway, List<string> lstFacilities, List<POITypeSetting> lstPOI)
        {
            try
            {
                var items = await Task.Run(() => Processing.LocalData.LoadData.getListFacilities(lstHighway, lstFacilities, lstPOI));
                if (OnGetFacilities != null)
                {
                    OnGetFacilities(items);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadFacilityDetail", ex.Message);
            }

        }
    }
}
