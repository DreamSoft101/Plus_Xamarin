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
    public class PLUSRangerConnections
    {
        public static async Task<ServiceResult> getSettings()
        {
            try
            {

                BaseClientPLUS client = new BaseClientPLUS();
                client.StrMethod = "GetCommunicationSetting";
                client.AddParameter("api_id", Cons.app_id);
                client.AddParameter("api_key", Cons.api_key);
                client.AddParameter("strInternalDeviceId", "");
                client.AddParameter("strUserId", "");
                client.AddParameter("strSessionId", "");
           
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    var data = itemResult["data"];
                    string imagePath = data["imagePath"].AsString();
                    string ftpHost = data["ftpHost"].AsString();
                    string ftpUser = data["ftpUser"].AsString();
                    string ftpPassword = data["ftpPassword"].AsString();
                    string ftpRootPath = data["ftpRootPath"].AsString();
                    string strInternalDeviceId = data["strInternalDeviceId"].AsString();
                    string strUserId = data["strUserId"].AsString();
                    string strSessionId = data["strSessionId"].AsString();

                    FtpSettings setting = new FtpSettings();
                    setting.ftpHost = ftpHost;
                    setting.ftpUser = ftpUser;
                    setting.ftpPassword = ftpPassword;
                    setting.ftpRootPath = ftpRootPath;
                    setting.strInternalDeviceId = strInternalDeviceId;
                    setting.strUserId = strUserId;
                    setting.strSessionId = strSessionId;

                    return new ServiceResult() { Data = setting, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
                else
                {
                    LogUtils.WriteError("getListLiveTrafficUpdate", "intStatus:" + itemJson["result"]["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getListLiveTrafficUpdate", ex.Message);
            }
            return null;
        }

        public static async Task<ServiceResult> GetCategory()
        {
            try
            {
                if (Cons.mFtpSettings == null)
                {
                    var setting = await getSettings();
                    if (setting.intStatus == 1)
                    {
                        Cons.mFtpSettings = setting.Data as FtpSettings;
                    }
                    else
                    {

                    }
                }

                BaseClientPLUS client = new BaseClientPLUS();
                client.StrMethod = "GetCommunicationCategory";
                client.AddParameter("api_id", Cons.app_id);
                client.AddParameter("api_key", Cons.api_key);
                client.AddParameter("strInternalDeviceId", Cons.mFtpSettings == null ? "" : Cons.mFtpSettings.strInternalDeviceId);
                client.AddParameter("strUserId", Cons.mFtpSettings == null ? "" : Cons.mFtpSettings.strUserId);
                client.AddParameter("strSessionId", Cons.mFtpSettings == null ? "" : Cons.mFtpSettings.strSessionId);
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    var data = itemResult["data"];
                    List<TblCategory> result = new List<TblCategory>();
                    foreach (var item in data)
                    {
                        TblCategory cate = new TblCategory();
                        cate.idCategory = item["idCategory"].AsString();
                        cate.strTitle = item["strTitle"].AsString();
                        result.Add(cate);
                     
                    }
                    return new ServiceResult() { Data = result, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
                else
                {
                    LogUtils.WriteError("getListLiveTrafficUpdate", "intStatus:" + itemJson["result"]["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getListLiveTrafficUpdate", ex.Message);
            }
            return null;
        }

        public static async Task<ServiceResult> GetListOpsComm(List<string> category, int page)
        {
            try
            {

                if (Cons.mFtpSettings == null)
                {
                    var setting = await getSettings();
                    if (setting.intStatus == 1)
                    {
                        Cons.mFtpSettings = setting.Data as FtpSettings;
                    }
                    else
                    {

                    }
                }

                BaseClientPLUS client = new BaseClientPLUS();
                client.StrMethod = "GetCommunication";
                client.AddParameter("api_id", Cons.app_id);
                client.AddParameter("api_key", Cons.api_key);
                client.AddParameter("strInternalDeviceId", Cons.mFtpSettings == null ? "" : Cons.mFtpSettings.strInternalDeviceId);
                client.AddParameter("strUserId", Cons.mFtpSettings == null ? "" : Cons.mFtpSettings.strUserId);
                client.AddParameter("strSessionId", Cons.mFtpSettings == null ? "" : Cons.mFtpSettings.strSessionId);
                if (category == null)
                {
                    client.AddParameter("idCategory", "");
                }
                else if (category.Count == 0)
                {
                    client.AddParameter("idCategory", "");
                }
                else
                {
                    client.AddParameter("idCategory", category);
                }
                client.AddParameter("strCreateBy", Cons.myEntity.User.strUserName);
                client.AddParameter("intPage", page);
                var myName = Cons.myEntity.User.strUserName;
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    var data = itemResult["data"];
                    List<TblPost> result = new List<TblPost>();
                    foreach (var item in data["data"])
                    {
                        try
                        {


                            if (item["strCreateBy"].AsString() != myName)
                            {
                                continue;
                            }

                            TblPost post = new TblPost();
                            post.idOpsComm = item["idOpsComm"].AsString();
                            post.strTitle = item["strTitle"].AsString();
                            post.strDescription = item["strDescription"].AsString();
                            post.strCustomerName = item["strCustomerName"].AsString();
                            post.strContactEmail = item["strContactEmail"].AsString();
                            post.idCategory = item["idCategory"].AsString();
                            post.strCreateBy = item["strCreateBy"].AsString();
                            post.dtCreateDate = item["dtCreateDate"].AsString();
                            post.strLastUpdateBy = item["strLastUpdateBy"].AsString();
                            post.dtLastUpdate = item["dtLastUpdate"].AsString();
                            post.decLatitude = item["decLatitude"].AsString();
                            post.decLongitude = item["decLongitude"].AsString();
                            var value = item["intSource"].AsString();
                            post.intPosted = item["intPost"].AsInt();
                            post.intSource = item["intSource"].AsInt();// == null ? 0 : Convert.ToInt16(item["intSource.Value.ToString())"].AsString();
                            post.intStatus = item["intSource"].AsInt();// == null ? 0 : Convert.ToInt16(item["intStatus.Value.ToString())"].AsString();
                            post.strAddress = item["strAddress"].AsString();
                            post.strAllowAccess = item["strAllowAccess"].AsString();
                            post.strContactNo = item["strContactNo"].AsString();
                            post.strUserPicture = item["strUserPicture"].AsString();
                            post.strCategory = item["strCategory"].AsString();
                            post.operations_media = JsonConvert.DeserializeObject<List<Operations_Media>>(item["operations_media"].AsString());
                            result.Add(post);
                        }
                        catch (Exception ex)
                        {

                        }
                      
                        
                    }
                    return new ServiceResult() { Data = result, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
                else
                {
                    LogUtils.WriteError("getListLiveTrafficUpdate", "intStatus:" + itemJson["result"]["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getListLiveTrafficUpdate", ex.Message);
            }
            return null;
        }

        public static async Task<ServiceResult> NearestLocation(double Lat, double Lng)
        {
            try
            {
                if (Cons.mFtpSettings == null)
                {
                    var setting = await getSettings();
                    if (setting.intStatus == 1)
                    {
                        Cons.mFtpSettings = setting.Data as FtpSettings;
                    }
                    else
                    {

                    }
                }

                BaseClientPLUS client = new BaseClientPLUS();
                client.StrMethod = "GetNearestLocation";
                client.AddParameter("api_id", Cons.app_id);
                client.AddParameter("api_key", Cons.api_key);
                client.AddParameter("strInternalDeviceId", Cons.mFtpSettings == null ? "" : Cons.mFtpSettings.strInternalDeviceId);
                client.AddParameter("strUserId", Cons.mFtpSettings == null ? "" : Cons.mFtpSettings.strUserId);
                client.AddParameter("strSessinId", Cons.mFtpSettings == null ? "" : Cons.mFtpSettings.strSessionId);
                client.AddParameter("intDeviceType", 2);
                client.AddParameter("floLat", Lat);
                client.AddParameter("floLong", Lng);
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    var data = itemResult["strData"];
                    NearestLocation result = new Models.NearestLocation();
                    result.strSection = data["strSection"].AsString();
                    result.distance = data["distance"].AsDouble();
                    return new ServiceResult() { Data = result, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
                else
                {
                    LogUtils.WriteError("getListLiveTrafficUpdate", "intStatus:" + itemJson["result"]["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getListLiveTrafficUpdate", ex.Message);
            }
            return null;
        }
        
        public static async Task<ServiceResult> PostData(TblPost tblPost, List<TblMedia> lstMedia)
        {
            try
            {
                if (Cons.mFtpSettings == null)
                {
                    var setting = await getSettings();
                    if (setting.intStatus == 1)
                    {
                        Cons.mFtpSettings = setting.Data as FtpSettings;
                    }
                    else
                    {

                    }
                }

                BaseClientPLUS client = new BaseClientPLUS();
                client.StrMethod = "PostCommunication";
                client.AddParameter("api_id", Cons.app_id);
                client.AddParameter("api_key", Cons.api_key);
                client.AddParameter("strInternalDeviceId", Cons.mFtpSettings == null ? "" : Cons.mFtpSettings.strInternalDeviceId);
                client.AddParameter("strUserId", Cons.mFtpSettings == null ? "" : Cons.mFtpSettings.strUserId);
                client.AddParameter("strSessionId", Cons.mFtpSettings == null ? "" : Cons.mFtpSettings.strSessionId);
                client.AddParameter("intDeviceType", 2);


                //var imgs = new List<operations_media>();
                //foreach (var item in lstMedia)
                //{
                //    //operations_media me = new operations_media();
                //    //me.media_url = 
                //}
                List<Operations_Media> omedis = new List<Operations_Media>();
                foreach (var media in lstMedia)
                {
                    omedis.Add(new Operations_Media() { media_url = media.RandomName, strComments = media.mStrComment });
                }
                tblPost.operations_media = omedis;
                List<TblPost> posts = new List<TblPost>() { tblPost };
                client.AddParameter("data", posts);



                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
                else
                {
                    LogUtils.WriteError("PostData", "intStatus:" + itemJson["result"]["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("PostData", ex.Message);
            }
            return null;
        }

      
    }
}
