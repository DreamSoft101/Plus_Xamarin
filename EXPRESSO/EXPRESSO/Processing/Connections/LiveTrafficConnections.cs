using EXPRESSO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXPRESSO.Models.Database;
using static EXPRESSO.Models.EnumType;
using EXPRESSO.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EXPRESSO.Processing.Connections
{
    public class LiveTrafficConnections
    {

        public static async Task<ServiceResult> getListLiveTrafficUpdate(List<int> types , List<string> idHighway, string idParent)
        {
            try
            {
                MyEntity entity = Cons.myEntity;

                List<TblEntities> result = new List<TblEntities>();
                BaseClient client = new BaseClient();
                client.StrMethod = "getLiveTraffic";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                client.AddParameter("idHighway", idHighway);
                client.AddParameter("intType", types);
                if (!string.IsNullOrEmpty(idParent))
                {
                    client.AddParameter("idParent", idParent);
                }
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    List<BaseItem> lstUpdate = new List<BaseItem>();
                    string lastIdHighway = "";
                    foreach (var item in itemResult["data"])
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TrafficUpdate update = new TrafficUpdate();
                            update.idTrafficUpdate = item["idTrafficUpdate"].AsString();
                            update.intType = item["intType"].AsInt();
                            update.idHighway = item["idHighway"].AsString();
                            update.decLat = item["floLat"].AsDouble();
                            update.decLng = item["floLong"].AsDouble();
                            update.strTitle = item["strTitle"].AsString();
                            update.intSpeedLimit = item["intSpeedLimit"].AsInt();
                            update.intWaterLevel = item["intWaterLevel"].AsInt();
                            update.strDescription = item["strDescription"].AsString();
                            update.dtStartDateTime = item["dtStartDateTime"].AsDateTime();
                            update.dtEndDateTime = item["dtEndDateTime"].AsDateTime();
                            update.intVisible = item["intVisibile"].AsInt();
                            update.intStatus = item["intStatus"].AsInt();
                            update.decLocation = item["decLocation"].AsDecimal();
                            update.idHwayLocation = item["idHwayLocation"].AsString();
                            update.strURL = item["strURL"].AsString();



                            bItem.Item = update;

                            if (update.intType == 16)
                            {
                                continue;
                            }
                            if (update.intType == 13 && !types.Contains(13))
                            {
                                continue;
                            }

                            if (update.intType == 13)
                            {
                                bool isfavorite = LocalData.LoadData.IsFavorite(update.idTrafficUpdate, FavoriteType.LiveFeed);
                                bItem.setTag(BaseItem.TagName.IsFavorite, isfavorite);
                            }

                            lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("getListLiveTrafficUpdate", ex.Message);
                        }
                    }


                    var lastResult = new List<BaseItem>();

                    if (types[0] == 13)
                    {
                        lstUpdate = lstUpdate.OrderByDescending(p => (bool)p.getTag(BaseItem.TagName.IsFavorite)).ThenBy(p => (p.Item as TrafficUpdate).idHighway).ThenByDescending(p => (p.Item as TrafficUpdate).decLat).ThenByDescending(p => (p.Item as TrafficUpdate).decLng).ToList();

                    }
                    else
                    {
                        lstUpdate = lstUpdate.OrderByDescending(p => (p.Item as TrafficUpdate).idHighway).ThenBy(p => (p.Item as TrafficUpdate).dtStartDateTime).ToList();
                    }
                    


                    foreach (var item in lstUpdate)
                    {
                        var hig = item.Item as TrafficUpdate;
                        if (hig.idHighway != lastIdHighway)
                        {
                            lastIdHighway = hig.idHighway;
                            TblHighway highway = LocalData.LoadData.getHighway(lastIdHighway);
                            BaseItem bitem = new BaseItem();
                            bitem.Item = highway;
                            lastResult.Add(bitem);
                        }
                        lastResult.Add(item);
                    }

                   
                    return new ServiceResult() { Data = lastResult, intStatus = 1 };
                }
                else
                {
                    LogUtils.WriteError("getListLiveTrafficUpdate", "intStatus:" + itemResult["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemResult["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getListLiveTrafficUpdate", ex.Message);
            }
            return null;
        }


        public static async Task<ServiceResult> GetLiveTraffic(List<int> types, List<string> idHighway, string idParent)
        {
            try
            {
                MyEntity entity = Cons.myEntity;

                List<TrafficUpdate> result = new List<TrafficUpdate>();
                BaseClient client = new BaseClient();
                client.StrMethod = "getLiveTraffic";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                client.AddParameter("idHighway", idHighway);
                client.AddParameter("intType", types);
                if (!string.IsNullOrEmpty(idParent))
                {
                    client.AddParameter("idParent", idParent);
                }
                string strJson = await client.getData();
                 var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    List<BaseItem> lstUpdate = new List<BaseItem>();
                    string lastIdHighway = "";
                    foreach (var item in itemResult["data"])
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TrafficUpdate update = new TrafficUpdate();
                            update.idTrafficUpdate = item["idTrafficUpdate"].AsString();
                            update.intType = item["intType"].AsInt();
                            update.idHighway = item["idHighway"].AsString();
                            update.decLat = item["floLat"].AsDouble();
                            update.decLng = item["floLong"].AsDouble();
                            update.strTitle = item["strTitle"].AsString();
                            update.intSpeedLimit = item["intSpeedLimit"].AsInt();
                            update.intWaterLevel = item["intWaterLevel"].AsInt();
                            update.strDescription = item["strDescription"].AsString();
                            update.dtStartDateTime = item["dtStartDateTime"].AsDateTime();
                            update.dtEndDateTime = item["dtEndDateTime"].AsDateTime();
                            update.intVisible = item["intVisibile"].AsInt();
                            update.intStatus = item["intStatus"].AsInt();
                            update.decLocation = item["decLocation"].AsDecimal();
                            update.idHwayLocation = item["idHwayLocation"].AsString();
                            update.strURL = item["strURL"].AsString();
                            //bItem.Item = update;

                            result.Add(update);

                       
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("GetLiveTraffic", ex.Message);
                        }
                    }

                    return new ServiceResult() { Data = result, intStatus = 1 };
                }
                else
                {
                    //itemJson["result"]["intStatus"].AsInt();
                    LogUtils.WriteError("GetLiveTraffic", "intStatus:" + itemJson["result"]["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemJson["result"]["strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetLiveTraffic", ex.Message);
            }
            return null;
        }


        public async static Task<string> getDirection(List<Waypoint> waypoints)
        {
            try
            {
                string waypoint = "waypoints=optimize:true";
                foreach (var item in waypoints)
                {
                    waypoint += "|" + item.lat + "," + item.lng;
                }
                string sensor = "sensor=false";
                string @params = waypoint + "&" + sensor;
                string output = "json";
                string origin = "origin=" + waypoints[0].lat + "," + waypoints[0].lng;
                string destination = "destination=" + waypoints[waypoints.Count - 1].lat + "," + waypoints[waypoints.Count - 1].lng;
                string url = "https://maps.googleapis.com/maps/api/directions/" + output + "?" +origin + "&" + destination + "&" + @params;
                BaseClient client = new BaseClient(url);
                string data = await client.getData();
                try
                {
                    dynamic jsonItem = JsonConvert.DeserializeObject(data);

                    //var points1 = jsonItem.routes;
                    //var points2 = jsonItem.routes.overview_polyline;
                    var points = jsonItem.routes[0].overview_polyline.points.Value;
                    return points;
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async static Task<ServiceResult> getLiveTrafficDetail(string id)
        {
            try
            {
                MyEntity entity = Cons.myEntity;

                List<TblEntities> result = new List<TblEntities>();
                BaseClient client = new BaseClient();
                client.StrMethod = "getLiveTrafficDetail";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                client.AddParameter("id", id);
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    var data = itemResult["data"];
                    List<BaseItem> lstUpdate = new List<BaseItem>();
                    string lastIdHighway = "";
                    try
                    {
                        TrafficUpdate update = new TrafficUpdate();
                        update.idTrafficUpdate = data["idTrafficUpdate"].AsString();
                        update.intType = data["intType"].AsInt();
                        update.idHighway = data["idHighway"].AsString();
                        update.decLat = data["floLat"].AsInt();
                        update.decLng = data["floLong"].AsInt();
                        update.strTitle = data["strTitle"].AsString();
                        update.intSpeedLimit = data["intSpeedLimit"].AsInt();
                        update.intWaterLevel = data["intWaterLevel"].AsInt();
                        update.strDescription = data["strDescription"].AsString();
                        update.dtStartDateTime = data["dtStartDateTime"].AsDateTime();
                        update.dtEndDateTime = data["dtEndDateTime"].AsDateTime();
                        update.intVisible = data["intVisibile"].AsInt();
                        update.dtEndDateTime = data["dtEndDateTime"].AsDateTime();
                        update.intStatus = data["intStatus"].AsInt();
                        update.decLocation = data["decLocation"].AsInt();
                        update.strWaypoint = data["strWaypoint"].AsString();
                        update.lstWaypoint = JsonConvert.DeserializeObject<List<Waypoint>>(update.strWaypoint);
                        if (update.lstWaypoint != null)
                        {
                            if (update.lstWaypoint.Count > 0)
                            {
                                var polyline = await getDirection(update.lstWaypoint);
                            }
                        }
                        return new ServiceResult() { Data = update, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                    }
                    catch (Exception ex)
                    {
                        LogUtils.WriteError("getLiveTrafficDetail", ex.Message);
                    }
                    //return new ServiceResult() { Data = lastResult, intStatus = 1 };
                }
                else
                {
                    LogUtils.WriteError("getLiveTrafficDetail", "intStatus:" + itemJson["result"]["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getLiveTrafficDetail", ex.Message);
            }
            return null;
        }

        public async static Task<ServiceResult> getFacilityCCTV(int type, List<string> idHighway)
        {
            try
            {
                MyEntity entity = Cons.myEntity;

                List<TblEntities> result = new List<TblEntities>();
                BaseClient client = new BaseClient();
                client.StrMethod = "GetFacilityCCTV";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                client.AddParameter("intFacilityType", type);
                client.AddParameter("idHighway", idHighway);
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    var data = itemResult["data"];
                    List<TblTollPlaza> lstUpdate = new List<TblTollPlaza>();
                    string lastIdHighway = "";
                    foreach (var item in data)
                    {
                        try
                        {
                            //BaseItem bItem = new BaseItem();
                            TblTollPlaza update = new TblTollPlaza();
                            update.idTollPlaza = item["idTollPlaza"].AsString();
                            update.idEntity = item["idEntity"].AsString();
                            update.strType = item["strType"].AsString();
                            update.strName = item["strName"].AsString();
                            update.idHighway = item["idHighway"].AsString();
                            update.strSection = item["strSection"].AsString();
                            update.strReloadTimeFrom = item["strReloadTimeFrom"].AsString();
                            update.strReloadTimeTo = item["strReloadTimeTo"].AsString();
                            update.strDirection = item["strDirection"].AsString();
                            update.intDirection = item["intDirection"].AsInt();
                            update.decLocation = item["decLocation"].AsDouble();
                            update.decLong = item["floLong"].AsDouble();
                            update.decLat = item["floLat"].AsDouble();
                            update.strExit = item["strExit"].AsString();
                            update.strPicture = item["strPicture"].AsString();
                            update.intSort = item["intSort"].AsInt();
                            update.intStatus = item["intStatus"].AsInt();

                            //var value = item.cctv;
                            var LstCCTV = new List<TollPlazaCCTV>();// JsonConvert.DeserializeObject<List<TollPlazaCCTV>>(item.cctv.Value.ToString());
                            foreach (var itemcctv in item["cctv"])
                            {
                                TollPlazaCCTV cctv = new TollPlazaCCTV();
                                cctv.decLat = update.decLat;
                                cctv.decLng = update.decLong;
                                cctv.idTollPlazaCctv = itemcctv["idTollPlazaCctv"].AsString();
                                cctv.idEntity = itemcctv["idEntity"].AsString();
                                cctv.idParent = itemcctv["idParent"].AsString();
                                cctv.intParentType = itemcctv["intParentType"].AsString();
                                cctv.strTitle = itemcctv["strTitle"].AsString();
                                cctv.strURL = itemcctv["strURL"].AsString();
                                cctv.strDescription = itemcctv["strDescription"].AsString();
                                cctv.intFrequency = itemcctv["intFrequency"].AsString();
                                cctv.intVisible = itemcctv["intVisible"].AsString();
                                cctv.intStatus = itemcctv["intStatus"].AsString();
                                cctv.strCCTVImage = itemcctv["strCCTVImage"].AsString();
                                cctv.idHighway = update.idHighway;
                                LstCCTV.Add(cctv);
                            }

                            if (LstCCTV == null || LstCCTV.Count == 0)
                            {
                                continue;
                            }
                            foreach (var itemcctv in LstCCTV)
                            {
                                itemcctv.idHighway = update.idHighway;
                            }
                            update.SetLstCCTV(LstCCTV);
                            lstUpdate.Add(update);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("getListLiveTrafficUpdate", ex.Message);
                        }
                    }


                    List<TollPlazaCCTV> tollPlazaCCTV = new List<TollPlazaCCTV>();
                    foreach (var item in lstUpdate)
                    {
                        tollPlazaCCTV.AddRange(item.getCCTV());
                    }

                    List<BaseItem> lstBItem = new List<BaseItem>();

                    foreach (var item in tollPlazaCCTV)
                    {
                        var isfavorite = LocalData.LoadData.IsFavorite(item.idTollPlazaCctv, FavoriteType.LiveFeed_TollPlaza);
                        BaseItem bitem = new BaseItem();
                        bitem.Item = item;
                        bitem.setTag(BaseItem.TagName.IsFavorite, isfavorite);
                        //var tollplaza = lstUpdate.Where(p => p.idTollPlaza == item.idParent).FirstOrDefault();
                        //bitem.setTag(BaseItem.TagName.IdHighway, tollplaza.idHighway);
                        lstBItem.Add(bitem);
                    }

                    lstBItem = lstBItem.OrderByDescending(p => (bool)p.getTag(BaseItem.TagName.IsFavorite)).ThenBy(p => (p.Item as TollPlazaCCTV).idHighway).ToList();

                    List<BaseItem> lastResult = new List<BaseItem>();
                    foreach (var item in lstBItem)
                    {
                        var hig = item.Item as TollPlazaCCTV;
                        if (hig.idHighway != lastIdHighway)
                        {
                            lastIdHighway = hig.idHighway;
                            TblHighway highway = LocalData.LoadData.getHighway(lastIdHighway);
                            BaseItem bitem = new BaseItem();
                            bitem.Item = highway;
                            lastResult.Add(bitem);
                        }
                        lastResult.Add(item);
                    }

                    return new ServiceResult() { Data = lastResult, intStatus = 1 };
                }
                else
                {
                    LogUtils.WriteError("getFacilityCCTV", "intStatus:" + itemJson["result"]["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getFacilityCCTV", ex.Message);
            }
            return null;
        }


        public async static Task<ServiceResult> GetTollPlaza( List<string> idHighway)
        {
            try
            {
                MyEntity entity = Cons.myEntity;

                List<TblEntities> result = new List<TblEntities>();
                BaseClient client = new BaseClient();
                client.StrMethod = "GetFacilityCCTV";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                client.AddParameter("intFacilityType", 2);
                client.AddParameter("idHighway", idHighway);
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    var data = itemResult["data"];
                    List<BaseItem> lstUpdate = new List<BaseItem>();
                    string lastIdHighway = "";
                    bool isFavrite = false;
                    foreach (var item in data)
                    {
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TblTollPlaza update = new TblTollPlaza();
                            update.idTollPlaza = item["idTollPlaza"].AsString();
                            update.idEntity = item["idEntity"].AsString();
                            update.strType = item["strType"].AsString();
                            update.strName = item["strName"].AsString();
                            update.idHighway = item["idHighway"].AsString();
                            update.strSection = item["strSection"].AsString();
                            update.strReloadTimeFrom = item["strReloadTimeFrom"].AsString();
                            update.strReloadTimeTo = item["strReloadTimeTo"].AsString();
                            update.strDirection = item["strDirection"].AsString();
                            update.intDirection = item["intDirection"].AsInt();
                            update.decLocation = item["decLocation"].AsDouble();
                            update.decLong = item["floLong"].AsDouble();
                            update.decLat = item["floLat"].AsDouble();
                            update.strExit = item["strExit"].AsString();
                            update.strPicture = item["strPicture"].AsString();
                            update.intSort = item["intSort"].AsInt();
                            update.intStatus = item["intStatus"].AsInt();

                            //var value = item.cctv;
                            var LstCCTV = new List<TollPlazaCCTV>();// JsonConvert.DeserializeObject<List<TollPlazaCCTV>>(item.cctv.Value.ToString());
                            foreach (var itemcctv in item["cctv"])
                            {
                                TollPlazaCCTV cctv = new TollPlazaCCTV();
                                cctv.decLat = update.decLat;
                                cctv.decLng = update.decLong;
                                cctv.idTollPlazaCctv = itemcctv["idTollPlazaCctv"].AsString();
                                cctv.idEntity = itemcctv["idEntity"].AsString();
                                cctv.idParent = itemcctv["idParent"].AsString();
                                cctv.intParentType = itemcctv["intParentType"].AsString();
                                cctv.strTitle = itemcctv["strTitle"].AsString();
                                cctv.strURL = itemcctv["strURL"].AsString();
                                cctv.strDescription = itemcctv["strDescription"].AsString();
                                cctv.intFrequency = itemcctv["intFrequency"].AsString();
                                cctv.intVisible = itemcctv["intVisible"].AsString();
                                cctv.intStatus = itemcctv["intStatus"].AsString();
                                cctv.strCCTVImage = itemcctv["strCCTVImage"].AsString();
                                cctv.idHighway = update.idHighway;
                                LstCCTV.Add(cctv);
                            }

                            update.SetLstCCTV(LstCCTV);

                            var isfavorite = LocalData.LoadData.IsFavorite(update.idTollPlaza, FavoriteType.TollPlaza);
                            if (isfavorite)
                            {
                                isFavrite = true;
                            }
                            bItem.setTag(BaseItem.TagName.IsFavorite, isfavorite);

                            bItem.Item = update;
                            lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("getListLiveTrafficUpdate", ex.Message);
                        }
                    }



                    foreach (var item in lstUpdate)
                    {
                       
                    }

                    lstUpdate = lstUpdate.OrderByDescending(p => (bool)p.getTag(BaseItem.TagName.IsFavorite)).ThenBy(p => (p.Item as TblTollPlaza).idHighway).ToList();

                    List<BaseItem> lastResult = new List<BaseItem>();

                  
                    foreach (var item in lstUpdate)
                    {
                        var hig = item.Item as TblTollPlaza;
                        if (hig.idHighway != lastIdHighway || isFavrite != (bool)item.getTag(BaseItem.TagName.IsFavorite))
                        {
                            lastIdHighway = hig.idHighway;
                            TblHighway highway = LocalData.LoadData.getHighway(lastIdHighway);
                            BaseItem bitem = new BaseItem();
                            bitem.Item = highway;
                            lastResult.Add(bitem);

                            if (isFavrite != (bool)item.getTag(BaseItem.TagName.IsFavorite))
                            {
                                isFavrite = false;
                            }
                        }
                        lastResult.Add(item);
                    }

                    return new ServiceResult() { Data = lastResult, intStatus = 1 };
                }
                else
                {
                    LogUtils.WriteError("GetTollPlaza", "intStatus:" + itemJson["result"]["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetTollPlaza", ex.Message);
            }
            return null;
        }


        public async static Task<TblTollPlaza> GetTollPlaza(List<string> idHighway, string idTollPlaza)
        {
            try
            {
                MyEntity entity = Cons.myEntity;

                List<TblEntities> result = new List<TblEntities>();
                BaseClient client = new BaseClient();
                client.StrMethod = "GetFacilityCCTV";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                client.AddParameter("intFacilityType", 2);
                client.AddParameter("idHighway", idHighway);
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    var data = itemResult["data"];
                    List<BaseItem> lstUpdate = new List<BaseItem>();
                    string lastIdHighway = "";
                    bool isFavrite = false;
                    foreach (var item in data)
                    {
                        if (item["idTollPlaza"].AsString() != idTollPlaza)
                        {
                            continue;
                        }
                        try
                        {
                            BaseItem bItem = new BaseItem();
                            TblTollPlaza update = new TblTollPlaza();
                            update.idTollPlaza = item["idTollPlaza"].AsString();
                            update.idEntity = item["idEntity"].AsString();
                            update.strType = item["strType"].AsString();
                            update.strName = item["strName"].AsString();
                            update.idHighway = item["idHighway"].AsString();
                            update.strSection = item["strSection"].AsString();
                            update.strReloadTimeFrom = item["strReloadTimeFrom"].AsString();
                            update.strReloadTimeTo = item["strReloadTimeTo"].AsString();
                            update.strDirection = item["strDirection"].AsString();
                            update.intDirection = item["intDirection"].AsInt();
                            update.decLocation = item["decLocation"].AsDouble();
                            update.decLong = item["floLong"].AsDouble();
                            update.decLat = item["floLat"].AsDouble();
                            update.strExit = item["strExit"].AsString();
                            update.strPicture = item["strPicture"].AsString();
                            update.intSort = item["intSort"].AsInt();
                            update.intStatus = item["intStatus"].AsInt();

                            //var value = item.cctv;
                            var LstCCTV = new List<TollPlazaCCTV>();// JsonConvert.DeserializeObject<List<TollPlazaCCTV>>(item.cctv.Value.ToString());
                            foreach (var itemcctv in item["cctv"])
                            {
                                TollPlazaCCTV cctv = new TollPlazaCCTV();
                                cctv.decLat = update.decLat;
                                cctv.decLng = update.decLong;
                                cctv.idTollPlazaCctv = itemcctv["idTollPlazaCctv"].AsString();
                                cctv.idEntity = itemcctv["idEntity"].AsString();
                                cctv.idParent = itemcctv["idParent"].AsString();
                                cctv.intParentType = itemcctv["intParentType"].AsString();
                                cctv.strTitle = itemcctv["strTitle"].AsString();
                                cctv.strURL = itemcctv["strURL"].AsString();
                                cctv.strDescription = itemcctv["strDescription"].AsString();
                                cctv.intFrequency = itemcctv["intFrequency"].AsString();
                                cctv.intVisible = itemcctv["intVisible"].AsString();
                                cctv.intStatus = itemcctv["intStatus"].AsString();
                                cctv.strCCTVImage = itemcctv["strCCTVImage"].AsString();
                                cctv.idHighway = update.idHighway;
                                LstCCTV.Add(cctv);
                            }

                            update.SetLstCCTV(LstCCTV);

                            var isfavorite = LocalData.LoadData.IsFavorite(update.idTollPlaza, FavoriteType.TollPlaza);
                            if (isfavorite)
                            {
                                isFavrite = true;
                            }

                            return update;
                            //bItem.setTag(BaseItem.TagName.IsFavorite, isfavorite);

                            //bItem.Item = update;
                            //lstUpdate.Add(bItem);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("getListLiveTrafficUpdate", ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetTollPlaza", ex.Message);
            }
            return null;
        }   
    }
}
