using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using Plugin.Connectivity;
using EXPRESSO.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EXPRESSO.Processing.Connections
{
    public class MasterDataConnections
    {
        

        public static async Task<ServiceResult> getAnnouncement(int page)
        {
            try
            {
                MyEntity entity = Cons.myEntity;

                List<TblEntities> result = new List<TblEntities>();
                BaseClient client = new BaseClient();
                client.StrMethod = "GetAnnouncementList";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                client.AddParameter("intPage", page);
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    var data = itemResult["data"];
                    List<Announcement> lstUpdate = new List<Announcement>();

                    foreach (var item in data)
                    {
                        try
                        {
                            Announcement itemAnnouncement = new Announcement();
                            itemAnnouncement.idAnnouncement = item["idAnnouncement"].AsString();
                            itemAnnouncement.strTitle = item["strTitle"].AsString();
                            itemAnnouncement.strDescription = item["strDescription"].AsString();
                            itemAnnouncement.dtStart = DateTime.Parse(item["dtStart"].AsString());

                            string image = item["images"].AsString();

                            itemAnnouncement.images = JsonConvert.DeserializeObject <List<string>>(image);
                            lstUpdate.Add(itemAnnouncement);
                        }
                        catch (Exception ex)
                        {
                            LogUtils.WriteError("getAnnouncement", ex.Message);
                        }

                    }

                    return new ServiceResult() { Data = lstUpdate, intStatus = 1 };
                }
                else
                {
                    LogUtils.WriteError("getAnnouncement", "intStatus:" + itemJson["result"]["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getAnnouncement", ex.Message);
            }
            return null;
        }

        public static async Task<ServiceResult> getAnnouncementDetail(string idAnnouncement)
        {
            try
            {
                MyEntity entity = Cons.myEntity;

                List<TblEntities> result = new List<TblEntities>();
                BaseClient client = new BaseClient();
                client.StrMethod = "GetAnnouncementDetail";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                client.AddParameter("idAnnouncement", idAnnouncement);
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    var data = itemResult["data"];

                    Announcement itemAnnouncement = new Announcement();
                    itemAnnouncement.idAnnouncement = data["idAnnouncement"].AsString();
                    itemAnnouncement.strTitle = data["strTitle"].AsString();
                    itemAnnouncement.strDescription = data["strDescription"].AsString();

                    itemAnnouncement.largerPhotoW = data["largerPhotoW"].AsInt();
                    itemAnnouncement.largerPhotoH = data["largerPhotoH"].AsInt();
                    itemAnnouncement.largerPhotoFit = data["largerPhotoFit"].AsString();
                    itemAnnouncement.thumbPhotoW = data["thumbPhotoW"].AsInt();
                    itemAnnouncement.thumbPhotoH = data["thumbPhotoH"].AsInt();
                    itemAnnouncement.thumbPhotoFit = data["thumbPhotoFit"].AsString();

                    itemAnnouncement.dtStart = DateTime.Parse(data["dtStart"].AsString());

                    itemAnnouncement.lstImages = new List<string>();
                    foreach (var item in data["images"])
                    {
                        itemAnnouncement.lstImages.Add(item.AsString());
                    }





                    return new ServiceResult() { Data = itemAnnouncement, intStatus = 1 };
                }
                else
                {
                    LogUtils.WriteError("getAnnouncementDetail", "intStatus:" + itemJson["result"]["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getAnnouncementDetail", ex.Message);
            }
            return null;
        }

        public static async Task<bool> CheckConnection()
        {
            return true;
            /*
            // ...
            if (CrossConnectivity.Current.IsConnected)
            {
                bool isRemoteReachable = await CrossConnectivity.Current.IsReachable(Cons.API_URL).ConfigureAwait(false);
                return isRemoteReachable;
            }
            else
            {
                return false;
            }*/
        }
    }
}
