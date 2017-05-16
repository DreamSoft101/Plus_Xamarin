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
    public class FavoriteThread
    {
        //public async void AddFavorite( TblRSA item )
        //{
        //    AddData.addFavorite(item);
        //}

        //public async void AddFavorite(TblTollPlaza item)
        //{
        //    AddData.addFavorite(item);
        //}

        //public async void AddFavorite(TblNearby item)
        //{
        //    AddData.addFavorite(item);
        //}

        //public async void AddFavorite(TblCSC item)
        //{
        //    AddData.addFavorite(item);
        //}

        //public async void AddFavorite(TblPetrolStation item)
        //{
        //    AddData.addFavorite(item);
        //}

        //public async void AddFavorite(TblFacilities item)
        //{
        //    AddData.addFavorite(item);
        //}

        //public async void IsFavorite(string idItem)
        //{
        //    // 
        //}

        public async void Update(object itemFavorite)
        {

            try
            {
                if (itemFavorite is TblRSA)
                {
                    var item = itemFavorite as TblRSA;
                    bool isFavorite = LoadData.IsFavorite(item.idRSA, FavoriteType.RSA);
                    if (!isFavorite)
                    {
                        AddData.addFavorite(item);
                    }
                }
                else if (itemFavorite is TblTollPlaza)
                {
                    var item = itemFavorite as TblTollPlaza;
                    bool isFavorite = LoadData.IsFavorite(item.idTollPlaza, FavoriteType.TollPlaza);
                    if (!isFavorite)
                    {
                        AddData.addFavorite(item);
                    }
                }
                else if (itemFavorite is TblPetrolStation)
                {
                    var item = itemFavorite as TblPetrolStation;
                    bool isFavorite = LoadData.IsFavorite(item.idPetrol, FavoriteType.PertrolStation);
                    if (!isFavorite)
                    {
                        AddData.addFavorite(item);
                    }
                }
                else if (itemFavorite is TblCSC)
                {
                    var item = itemFavorite as TblCSC;
                    bool isFavorite = LoadData.IsFavorite(item.idCSC, FavoriteType.CSC);
                    if (!isFavorite)
                    {
                        AddData.addFavorite(item);
                    }
                }
                else if (itemFavorite is TblFacilities)
                {
                    var item = itemFavorite as TblFacilities;
                    bool isFavorite = LoadData.IsFavorite(item.idFacilities, FavoriteType.Facilities);
                    if (!isFavorite)
                    {
                        AddData.addFavorite(item);
                    }
                }
                else if (itemFavorite is TblNearby)
                {
                    var item = itemFavorite as TblNearby;
                    bool isFavorite = LoadData.IsFavorite(item.idNearby, FavoriteType.Nearby);
                    if (!isFavorite)
                    {
                        AddData.addFavorite(item);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Update", ex.Message);
            }
        }
        
        public async void IsToggle(TblRSA tblRSA)
        {
            try
            {
                bool isFavorite = LoadData.IsFavorite(tblRSA.idRSA, FavoriteType.RSA);
                TblFavLoc item = null;
                if (isFavorite)
                {
                    item = DeleteData.DeleteFavorite(tblRSA);
                }
                else
                {
                    item = AddData.addFavorite(tblRSA);

                }

                SyncServer(new List<TblFavLoc>() { item }, false);
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("IsToggle", ex.Message);
            }

           
        }
        public async void IsToggle(TblFacilities tblFacility)
        {

            try
            {
                bool isFavorite = LoadData.IsFavorite(tblFacility.idFacilities, FavoriteType.Facilities);
                TblFavLoc item = null;
                if (isFavorite)
                {
                    item = DeleteData.DeleteFavorite(tblFacility);
                }
                else
                {
                    item = AddData.addFavorite(tblFacility);
                }
                SyncServer(new List<TblFavLoc>() { item }, false);
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("IsToggle", ex.Message);
            }
            
        }
        public async void IsToggle(TblCSC tblCSC)
        {
            try
            {
                bool isFavorite = LoadData.IsFavorite(tblCSC.idCSC, FavoriteType.CSC);
                TblFavLoc item = null;
                if (isFavorite)
                {
                    item = DeleteData.DeleteFavorite(tblCSC);
                }
                else
                {
                    item = AddData.addFavorite(tblCSC);
                }
                SyncServer(new List<TblFavLoc>() { item }, false);
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("IsToggle", ex.Message);
            }
        }


        public async void IsToggle(TblTollPlaza tblTollPlaza)
        {
            try
            {
                bool isFavorite = LoadData.IsFavorite(tblTollPlaza.idTollPlaza, FavoriteType.TollPlaza);
                TblFavLoc item = null;
                if (isFavorite)
                {
                    item = DeleteData.DeleteFavorite(tblTollPlaza);
                }
                else
                {
                    item = AddData.addFavorite(tblTollPlaza);
                }
                SyncServer(new List<TblFavLoc>() { item }, false);
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("IsToggle", ex.Message);
            }
        }
        public async void IsToggle(TblNearby tblNearby)
        {
            try
            {
                bool isFavorite = LoadData.IsFavorite(tblNearby.idNearby, FavoriteType.Nearby);
                TblFavLoc item = null;
                if (isFavorite)
                {
                    item = DeleteData.DeleteFavorite(tblNearby);
                }
                else
                {
                    item = AddData.addFavorite(tblNearby);
                }
                SyncServer(new List<TblFavLoc>() { item }, false);
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("IsToggle", ex.Message);
            }
            
        }

        public async void IsToggle(TblPetrolStation tblPetrolStation)
        {
            try
            {
                bool isFavorite = LoadData.IsFavorite(tblPetrolStation.idPetrol, FavoriteType.PertrolStation);
                TblFavLoc item = null;
                if (isFavorite)
                {
                    item = DeleteData.DeleteFavorite(tblPetrolStation);
                }
                else
                {
                    item = AddData.addFavorite(tblPetrolStation);
                }
                SyncServer(new List<TblFavLoc>() { item }, false);
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("IsToggle", ex.Message);
            }
            
        }

        public async void IsToggle(TrafficUpdate items)
        {
            try
            {
                bool isFavorite = LoadData.IsFavorite(items.idTrafficUpdate, FavoriteType.LiveFeed);
                TblFavLoc item = null;
                if (isFavorite)
                {
                    item = DeleteData.DeleteFavorite(items);
                }
                else
                {
                    item = AddData.addFavorite(items);
                }
                SyncServer(new List<TblFavLoc>() { item }, false);
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("IsToggle", ex.Message);
            }

        }

        public async void IsToggle(TollPlazaCCTV items)
        {
            try
            {
                bool isFavorite = LoadData.IsFavorite(items.idTollPlazaCctv, FavoriteType.LiveFeed_TollPlaza);
                TblFavLoc item = null;
                if (isFavorite)
                {
                    item = DeleteData.DeleteFavorite(items);
                }
                else
                {
                    item = AddData.addFavorite(items);
                }
                SyncServer(new List<TblFavLoc>() { item }, false);
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("IsToggle", ex.Message);
            }

        }

    


        public delegate void onLoadCCTV(List<BaseItem> result);
        public onLoadCCTV OnLoadCCTV;

        public async void loadCCTV()
        {
            try
            {
                var result = await Task.Run(() => LoadData.getListCCTV());
                //return result;
                if (OnLoadCCTV != null)
                {
                    OnLoadCCTV(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadCCTV", ex.Message);
            }
        }

        public delegate void onLoadRSA(List<BaseItem> result);
        public onLoadRSA OnLoadRSA;
        public async void loadRSA(FavoriteType type)
        {
            try
            {
                if (type == FavoriteType.RSA)
                {
                    var result = await Task.Run(() => LoadData.getListFavoriteRSA());
                    //return result;
                    if (OnLoadRSA != null)
                    {
                        OnLoadRSA(result);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadRSA", ex.Message);
            }

            

        }


        public delegate void onLoadTollPlaza(List<BaseItem> result);
        public onLoadTollPlaza OnLoadTollPlaza;
        public async void loadTollPlaza()
        {
            try
            {
                var result = await Task.Run(() => LoadData.getListFavoriteTollPlazaWithHeader());
                //return result;
                if (OnLoadTollPlaza != null)
                {
                    OnLoadTollPlaza(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadTollPlaza", ex.Message);
            }
            
        }

        public delegate void onLoadPertrolStation(List<BaseItem> result);
        public onLoadPertrolStation OnLoadPertrolStation;
        public async void loadPertrolStation()
        {
            try
            {
                var result = await Task.Run(() => LoadData.getListFavoritePertrolStationHeader());
                //return result;
                if (OnLoadPertrolStation != null)
                {
                    OnLoadPertrolStation(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadPertrolStation", ex.Message);
            }
            
        }

        public delegate void onLoadCSC(List<BaseItem> result);
        public onLoadCSC OnLoadCSC;
        public async void loadCSC()
        {
            try
            {
                var result = await Task.Run(() => LoadData.getListFavoriteCSCHeader());
                //return result;
                if (OnLoadCSC != null)
                {
                    OnLoadCSC(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadCSC", ex.Message);
            }
            
        }

        public delegate void onLoadFacilities(List<BaseItem> result);
        public onLoadFacilities OnLoadFacilities;
        public async void loadFacilities()
        {
            try
            {
                var result = await Task.Run(() => LoadData.getListFavoriteFacilitiesHeader());
                //return result;
                if (OnLoadFacilities != null)
                {
                    OnLoadFacilities(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadFacilities", ex.Message);
            }

        }


        //public delegate void onLoadPlusSmile(List<BaseItem> result);
        //public onLoadPlusSmile OnLoadPlusSmile;

        //public async void loadPLUSSmile()
        //{
        //    var result = await Task.Run(() => LoadData.getListFavoritePlusSmileHeader());
        //    if (OnLoadPlusSmile != null)
        //    {
        //        OnLoadPlusSmile(result);
        //    }
        //}

        //public delegate void onLoadSSK(List<BaseItem> result);
        //public onLoadSSK OnLoadSSK;

        //public async void loadSSK()
        //{
        //    var result = await Task.Run(() => LoadData.getListFavoriteSSKHeader());
        //    if (OnLoadSSK != null)
        //    {
        //        OnLoadSSK(result);
        //    }
        //}


        public delegate void onLoadNearBy(List<BaseItem> result);
        public onLoadNearBy OnLoadNearBy;
        public async void loadNearby()
        {
            try
            {
                var result = await Task.Run(() => LoadData.getListFavoriteNearByWithHeader());
                //return result;
                if (OnLoadNearBy != null)
                {
                    OnLoadNearBy(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadNearby", ex.Message);
            }
            
        }



        public delegate void onSyncFav(ServiceResult result);
        public onSyncFav OnSyncFav;
        public async void SyncServer(List<TblFavLoc> lstItem, bool isReturn)
        {
            try
            {
                if (string.IsNullOrEmpty(Cons.myEntity.User.strToken))
                {
                    return;
                }
                bool isConnected = await MasterDataConnections.CheckConnection();
                if (!isConnected)
                {
                    //  return;
                }
                List<ServerFavorite> lstItemServer = new List<ServerFavorite>();
                foreach (var item in lstItem)
                {
                    ServerFavorite itemServer = new ServerFavorite();
                    itemServer.intStatus = item.intStatus;
                    itemServer.intType = item.intObjectType;
                    itemServer.idObject = item.idObject;
                    lstItemServer.Add(itemServer);
                }
                var result = await Task.Run(() => FavoriteConnections.UpdateFavorite(lstItemServer, isReturn));
                if (OnSyncFav != null)
                {
                    OnSyncFav(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("SyncServer", ex.Message);
            }
            
        }


        public delegate void onSyncSuccess();
        public onSyncSuccess OnSyncSuccess;
        public async void AutoSyncFavorite()
        {

            try
            {
                if (string.IsNullOrEmpty(Cons.myEntity.User.strToken))
                {
                    if (OnSyncSuccess != null)
                    {
                        OnSyncSuccess();
                    }
                    return;
                }
                try
                {
                    var listFavorite = LoadData.getAllFavorite();
                    List<ServerFavorite> lstItemServer = new List<ServerFavorite>();
                    foreach (var item in listFavorite)
                    {
                        ServerFavorite itemServer = new ServerFavorite();
                        itemServer.intStatus = item.intStatus;
                        itemServer.intType = item.intObjectType;
                        itemServer.idObject = item.idObject;
                        lstItemServer.Add(itemServer);
                    }
                    var result = await Task.Run(() => FavoriteConnections.UpdateFavorite(lstItemServer, true));
                    if (result.intStatus == 1)
                    {
                        List<ServerFavorite> lstFavoriteServer = result.Data as List<ServerFavorite>;
                        DeleteData.DeleteAllFavorite(); //Clear all old data
                        foreach (var item in lstFavoriteServer)
                        {
                            FavoriteType type = (FavoriteType)item.intType;
                            if (item.intStatus == 2)
                            {
                                //Delete
                                DeleteData.DeleteFavorite(item.idObject, type);
                            }
                            else
                            {
                                object itemObject = null;
                                switch (type)
                                {
                                    case FavoriteType.RSA:
                                        {
                                            itemObject = LoadData.getRSA(item.idObject);

                                            break;
                                        }
                                    case FavoriteType.TollPlaza:
                                        {
                                            itemObject = LoadData.getTollPlaza(item.idObject);
                                            break;
                                        }
                                    case FavoriteType.PertrolStation:
                                        {
                                            itemObject = LoadData.getPertrolStation(item.idObject);
                                            break;
                                        }
                                    case FavoriteType.CSC:
                                        {
                                            itemObject = LoadData.getCSC(item.idObject);
                                            break;
                                        }
                                    case FavoriteType.Facilities:
                                        {
                                            //var itemFacility = LoadData.getf(item.idObject);
                                            break;
                                        }
                                    case FavoriteType.Nearby:
                                        {
                                            itemObject = LoadData.getNearby(item.idObject);
                                            break;
                                        }
                                }
                                if (itemObject != null)
                                {
                                    Update(itemObject);
                                }

                            }
                        }
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    LogUtils.WriteError("AutoSyncFavorite", ex.Message);
                }
                if (OnSyncSuccess != null)
                {
                    OnSyncSuccess();
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("AutoSyncFavorite", ex.Message);
            }
            
        }


        public delegate void onGetFavoriteLocation(ServiceResult result);
        public onGetFavoriteLocation OnGetFavoriteLocation;
        public async void GetFavoriteLocation(string idHighway)
        {
            try
            {
                var result = await Task.Run(() => FavoriteConnections.GetFavoriteLocation(idHighway));
                if (OnGetFavoriteLocation != null)
                {
                    OnGetFavoriteLocation(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetFavoriteLocation", ex.Message);
            }
        }

        public delegate void onGetFavoriteItem(List<BaseItem> result);
        public onGetFavoriteItem OnGetFavoriteItem;
        public async void GetAllItemFavorite()
        {
            try
            {
                var result = await Task.Run(() => LoadData.getListFavorite());
                if (OnGetFavoriteItem != null)
                {
                    OnGetFavoriteItem(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetFavoriteLocation", ex.Message);
            }
        }

        public delegate void onGetIsFavorite(bool isFavorite);
        public onGetIsFavorite OnGetIsFavorite;

        public async void GetIsFavorite(string id, FavoriteType type)
        {
            try
            {
                var result = await Task.Run(() => LoadData.IsFavorite(id, type));
                if (OnGetIsFavorite != null)
                {
                    OnGetIsFavorite(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetIsFavorite", ex.Message);
            }
        }
    }
}
