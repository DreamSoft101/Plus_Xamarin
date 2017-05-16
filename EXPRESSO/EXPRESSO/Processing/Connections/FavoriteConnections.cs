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

namespace EXPRESSO.Processing.Connections
{
    public class FavoriteConnections
    {
        public static async Task<ServiceResult> UpdateFavorite(List<ServerFavorite> data, bool isReturn)
        {
            try
            {
                MyEntity entity = Cons.myEntity;

                List<TblEntities> result = new List<TblEntities>();
                BaseClient client = new BaseClient();
                client.StrMethod = "getSyncFav";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                client.AddParameter("strEmail", entity.User.strUserName);
                client.AddParameter("strSessionID", entity.User.strToken);
                client.AddParameter("data", data);
                client.AddParameter("return", isReturn ? 1 : 0);
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    if (!isReturn)
                    {
                        return new ServiceResult() { Data = null, intStatus = 1 };
                    }
                    else
                    {
                        var dataJson = itemResult["data"];
                        List<ServerFavorite> lstUpdate = new List<ServerFavorite>();
                        foreach (var item in dataJson)
                        {
                            try
                            {
                                ServerFavorite itemFav = new ServerFavorite();
                                //itemFav.intStatus = item.intStatus;
                                itemFav.idObject = item["idObject"].AsString();
                                itemFav.intType = item["intType"].AsInt();
                                lstUpdate.Add(itemFav);
                            }
                            catch (Exception ex)
                            {
                                LogUtils.WriteError("getListLiveTrafficUpdate", ex.Message);
                            }
                        }

                        return new ServiceResult() { Data = lstUpdate, intStatus = 1 };
                    }
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


        public static async Task<ServiceResult> GetFavoriteLocation(string idHighway)
        {
            try
            {
                MyEntity entity = Cons.myEntity;

                var result = new List<FavoriteLocation>();
                BaseClient client = new BaseClient();
                client.StrMethod = "GetFavoriteLocation";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                client.AddParameter("idHighway", idHighway);
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    var dataJson = itemResult["data"];

                    foreach (var data in dataJson)
                    {
                        FavoriteLocation item = new FavoriteLocation();
                        item.idFavoriteLocation = data["idFavoriteLocation"].AsString();
                        item.idHighway = data["idHighway"].AsString();
                        item.strFavoriteLocationName = data["strFavoriteLocationName"].AsString();
                        item.detail = new List<string>();
                        foreach (var id in data["detail"])
                        {
                            item.detail.Add(id["Value"].AsString());
                        }
                        result.Add(item);
                    }

                    return new ServiceResult() { Data = result, intStatus = itemResult["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
                else
                {
                    LogUtils.WriteError("GetFavoriteLocation", "intStatus:" + itemResult["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemResult["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetFavoriteLocation", ex.Message);
            }
            return null;
        }
    }
}
