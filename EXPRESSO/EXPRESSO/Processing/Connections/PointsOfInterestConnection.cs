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
    public class PointsOfInterestConnection
    {
        private static string TAG = "PointsOfInterestConnection";
        public static async Task<ServiceResult> getRSACCTV(string idRSA)
        {
            try
            {
                var myentity = Cons.myEntity;
                BaseClient client = new BaseClient();
                client.StrMethod = "GetPoiCCTV";
                client.AddParameter("api_id", myentity.api_id);
                client.AddParameter("api_key", myentity.api_key);
                client.AddParameter("intFacilityType", 1);
                client.AddParameter("idObject", idRSA);
                string json = await client.getData();
                var itemJson = JObject.Parse(json);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    var data = itemResult["data"];
                    List<RsaCCTV> mLstCCTV = new List<RsaCCTV>();
                    foreach (var item in data)
                    {
                        RsaCCTV itemCCTV = new RsaCCTV();
                        itemCCTV.idRSAcctv = item["idRSAcctv"].AsString();
                        itemCCTV.idEntity = item["idEntity"].AsInt();
                        itemCCTV.idParent = item["idParent"].AsString();
                        itemCCTV.intParentType = item["intParentType"].AsInt();
                        itemCCTV.strTitle = item["strTitle"].AsString();
                        itemCCTV.strURL = item["strURL"].AsString();
                        itemCCTV.strDescription = item["strDescription"].AsString();
                        itemCCTV.intFrequency = item["intFrequency"].AsInt();
                        itemCCTV.strCCTVImage = item["strCCTVImage"].AsString();
                        itemCCTV.intVisible = item["intVisible"].AsInt();
                        itemCCTV.intStatus = item["intStatus"].AsInt();
                        mLstCCTV.Add(itemCCTV);
                    }

                    return new ServiceResult() { Data = mLstCCTV, intStatus = 1 };
                }
                else
                {
                    Utils.LogUtils.WriteLog(TAG, itemResult["strMessage"].AsString());
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getRSACCTV", ex.Message);
            }
            return null;
        }

      

        public static async Task<ServiceResult> POIDetail(string id, string type, string poitype)
        {
            try
            {
                var myentity = Cons.myEntity;
                BaseClient client = new BaseClient();
                client.StrMethod = "getPointOfInterestDetail";
                client.AddParameter("api_id", myentity.api_id);
                client.AddParameter("api_key", myentity.api_key);
                client.AddParameter("poiType", poitype);
                client.AddParameter("id", id);
                client.AddParameter("type", type);

                string json = await client.getData();
                var itemJson = JObject.Parse(json);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {


                    return new ServiceResult() { Data = null, intStatus = 1 };
                }
                else
                {
                    Utils.LogUtils.WriteLog(TAG, itemResult["strMessage"].AsString());
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getRSACCTV", ex.Message);
            }
            return null;
        }
    }
}
