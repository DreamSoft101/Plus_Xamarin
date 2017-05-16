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
    public class CommunicationConnection
    {
        public static async Task<ServiceResult> getFeedback(BaseItem bitem)
        {
            try
            {
                MyEntity entity = Cons.myEntity;

                BaseClient client = new BaseClient();
                client.StrMethod = "getFeedback";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                //client.AddParameter("strEmail", entity.User.strUserName);
                //client.AddParameter("strSessionID", entity.User.strToken);

                if (bitem.Item is TblRSA)
                {
                    client.AddParameter("idObject", (bitem.Item as TblRSA).idRSA);
                    client.AddParameter("intType", 1);
                }
                else if (bitem.Item is TblTollPlaza)
                {
                    client.AddParameter("idObject", (bitem.Item as TblTollPlaza).idTollPlaza);
                    client.AddParameter("intType", 2);
                }
                else if (bitem.Item is TblPetrolStation)
                {
                    client.AddParameter("idObject", (bitem.Item as TblPetrolStation).idPetrol);
                    client.AddParameter("intType", 3);
                }
                else if (bitem.Item is TblCSC)
                {
                    client.AddParameter("idObject", (bitem.Item as TblCSC).idCSC);
                    client.AddParameter("intType", 4);
                }
                else if (bitem.Item is TblFacilities)
                {
                    client.AddParameter("idObject", (bitem.Item as TblFacilities).idFacilities);
                    client.AddParameter("intType", 5);
                }
                else if (bitem.Item is TblNearby)
                {
                    client.AddParameter("idObject", (bitem.Item as TblNearby).idNearby);
                    client.AddParameter("intType", 6);
                }
                else if (bitem.Item is TrafficUpdate)
                {
                    client.AddParameter("idObject", (bitem.Item as TrafficUpdate).idTrafficUpdate);
                    client.AddParameter("intType", 13);
                }
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    List<GetFeedback> lstItem = new List<GetFeedback>();
                    var data = itemResult["data"];
                    foreach (var item in data)
                    {
                        try
                        {
                            GetFeedback feedback = new GetFeedback();
                            feedback.idFeedback = item["idFeedback"].AsInt();
                            feedback.dtCreatedDate = item["dtCreatedDate"].AsString();
                            feedback.strContent = item["strContent"].AsString();
                            feedback.strCreatedBy = item["strCreatedBy"].AsString();
                            lstItem.Add(feedback);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("getListLiveTrafficUpdate", ex.Message);
                        }

                    }
                    return new ServiceResult() { Data = lstItem, intStatus = 1 };
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

        public static async Task<ServiceResult> postFeedback(BaseItem bitem, string strContent)
        {
            try
            {
                MyEntity entity = Cons.myEntity;

                BaseClient client = new BaseClient();
                client.StrMethod = "postFeedback";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                client.AddParameter("strEmail", entity.User.strUserName);
                client.AddParameter("strSessionID", entity.User.strToken);

                PostFeedback feedback = new PostFeedback();
                feedback.images = new List<string>();
                feedback.dtCreatedDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                feedback.strContent =  strContent.Trim();
                if (bitem.Item is TblRSA)
                {
                    feedback.idObject = (bitem.Item as TblRSA).idRSA;
                    feedback.intType = 1;
                  
                }
                else if (bitem.Item is TblTollPlaza)
                {
                    feedback.idObject = (bitem.Item as TblTollPlaza).idTollPlaza;
                    feedback.intType = 2;
                }
                else if (bitem.Item is TblPetrolStation)
                {
                    feedback.idObject = (bitem.Item as TblPetrolStation).idPetrol;
                    feedback.intType = 3;
                }
                else if (bitem.Item is TblCSC)
                {
                    feedback.idObject = (bitem.Item as TblCSC).idCSC;
                    feedback.intType = 4;
                }
                else if (bitem.Item is TblFacilities)
                {
                    feedback.idObject = (bitem.Item as TblFacilities).idFacilities;
                    feedback.intType = 5;
                }
                else if (bitem.Item is TblNearby)
                {
                    feedback.idObject = (bitem.Item as TblNearby).idNearby;
                    feedback.intType = 6;
                }
                else if (bitem.Item is TrafficUpdate)
                {
                    feedback.idObject = (bitem.Item as TrafficUpdate).idTrafficUpdate;
                    feedback.intType = 13;
                }
                
                client.AddParameter("data", feedback);

                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    var data = itemResult["data"];
                    if (data != null)
                    {
                        GetFeedback feedbackrespone = new GetFeedback();
                        feedbackrespone.idFeedback = data["idFeedback"].AsInt();
                        feedbackrespone.dtCreatedDate = data["dtCreatedDate"].AsString();
                        feedbackrespone.strContent = data["strContent"].AsString();
                        feedbackrespone.strCreatedBy = data["strCreatedBy"].AsString();
                        return new ServiceResult() { Data = feedbackrespone, intStatus = 1 };
                    }
                    return new ServiceResult() { Data = null, intStatus = 1 };
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
    }
}
