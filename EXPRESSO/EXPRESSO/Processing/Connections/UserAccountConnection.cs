using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using EXPRESSO.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EXPRESSO.Utils.StringExtentions;
using Newtonsoft.Json.Linq;

namespace EXPRESSO.Processing.Connections
{
    public static class UserAccountConnection
    {
        /// <summary>
        /// Login user to system
        /// Call API:
        /// </summary>
        /// <param name="UserName">depent on type</param>
        /// <param name="strPassword">depent on type</param>
        /// <param name="idEntry"></param>
        /// <param name="type">Type = 0: Anonymous, Type = 1: UserName/Password, Type = 2: G+, Type = 3: FB, Type = 3: Login by code</param>
        /// <returns></returns>
        public static async Task<ServiceResult> Login(string UserName,string strPassword, MyEntity entity)
        {
            try
            {
                BaseClient client = new BaseClient();
                client.StrMethod = "memberLogin";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                client.AddParameter("strEmail", UserName);
                client.AddParameter("strPassword", strPassword);
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
				if (itemResult["intStatus"].AsInt() == 1)
                {
                    var data = itemResult["data"];
                    UserInfos info = new UserInfos();
                    info.strToken = data["strSessionID"].AsString();
                    info.strUserName = data["profile"]["strEmail"].AsString();
                    info.strMobileNo = data["profile"]["strMobileNo"].AsString();
                    info.strFirstName = data["profile"]["strFirstName"].AsString();
                    info.strLastName = data["profile"]["strLastName"].AsString();
                    info.strAvatar = data["profile"]["strPicture"].AsString();


                    var loyLogin = await Loyalty.Processing.Connections.UserAccount.Login(info.strUserName, "", "a5309d3c348d152582424506a142a685");
                    if (loyLogin.StatusCode != 1)
                    {
                        var loyRegis = await Loyalty.Processing.Connections.UserAccount.Register(90, info.strUserName, "a5309d3c348d152582424506a142a685", "", info.strFirstName, info.strLastName, "", info.strUserName);
                        loyLogin = await Loyalty.Processing.Connections.UserAccount.Login(info.strUserName, "", "a5309d3c348d152582424506a142a685");
                    }
                    info.LoyaltyAccount = loyLogin;

                    return new ServiceResult() { Data = info, intStatus = 1 };
                }
                else
                {
                    LogUtils.WriteError("getEntityByDomain", "intStatus:" + itemJson["result"]["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemJson["result"][" strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Login",  ex.Message);
            }
            return null;
        }
        
        public static async Task<ServiceResult> getEntityByDomain(string domain)
        {
            try
            {
                BaseClient client = new BaseClient();
                client.StrMethod = "getEntityByDomain";
                client.AddParameter("domain", domain);
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    var settingJson = itemResult["data"]["settings"];
                    var setting = new Settings();
                    setting.maxtitle = settingJson["maxtitle"].AsInt();
                    setting.maxdescription = settingJson["maxdescription"].AsInt();
                    setting.maxdescription = settingJson["maxdescription"].AsInt();
                    setting.rowperpage = settingJson["rowperpage"].AsInt();
                    setting.thumblargew = settingJson["thumblargew"].AsInt();
                    setting.thumblargeh = settingJson["thumblargeh"].AsInt();
                    setting.thumbsmallw = settingJson["thumbsmallw"].AsInt();
                    setting.thumbsmallh = settingJson["thumbsmallh"].AsInt();
                    setting.compressed = settingJson["compressed"].AsInt();
                    setting.maxsizew = settingJson["maxsizew"].AsInt();
                    setting.maxsizeh = settingJson["maxsizeh"].AsInt();
                    setting.maximage = settingJson["maximage"].AsInt();
                    setting.traffic_alert_label = settingJson["traffic_alert_label"].AsString();
                    setting.road_closure_label = settingJson["road_closure_label"].AsString();
                    setting.poi_closure_label = settingJson["poi_closure_label"].AsString();
                    setting.major_disruption_label = settingJson["major_disruption_label"].AsString();
                    setting.incidents_congestion_label = settingJson["incidents_congestion_label"].AsString();
                    setting.roadworks_label = settingJson["roadworks_label"].AsString();
                    setting.alternative_route_label = settingJson["alternative_route_label"].AsString();
                    setting.speed_limit_label = settingJson["speed_limit_label"].AsString();
                    setting.speed_trap_label = settingJson["speed_trap_label"].AsString();
                    setting.future_event_label = settingJson["future_event_label"].AsString();
                    setting.future_roadwork_label = settingJson["future_roadwork_label"].AsString();
                    setting.future_road_closure_label = settingJson["future_road_closure_label"].AsString();
                    setting.future_poi_closure_label = settingJson["future_poi_closure_label"].AsString();
                    setting.vehicle_class_1_label = settingJson["vehicle_class_1_label"].AsString();
                    setting.vehicle_class_1_icon = settingJson["vehicle_class_1_icon"].AsString();
                    setting.vehicle_class_2_icon = settingJson["vehicle_class_2_icon"].AsString();
                    setting.vehicle_class_3_icon = settingJson["vehicle_class_3_icon"].AsString();
                    setting.vehicle_class_4_icon = settingJson["vehicle_class_4_icon"].AsString();
                    setting.vehicle_class_5_icon = settingJson["vehicle_class_5_icon"].AsString();
                    setting.vehicle_class_2_label = settingJson["vehicle_class_2_label"].AsString();
                    setting.vehicle_class_3_label = settingJson["vehicle_class_3_label"].AsString();
                    setting.vehicle_class_4_label = settingJson["vehicle_class_4_label"].AsString();
                    setting.vehicle_class_5_label = settingJson["vehicle_class_5_label"].AsString();
                    setting.toll_plaza_label = settingJson["toll_plaza_label"].AsString();
                    setting.vista_point_label = settingJson["vista_point_label"].AsString();
                    setting.csc_label = settingJson["csc_label"].AsString();
                    setting.rsa_label = settingJson["rsa_label"].AsString();
                    setting.petrol_station_label = settingJson["petrol_station_label"].AsString();
                    setting.tunnel_label = settingJson["tunnel_label"].AsString();
                    setting.interchange_label = settingJson["interchange_label"].AsString();
                    setting.rsa_icon = settingJson["rsa_icon"].AsString();
                    setting.toll_plaza_icon = settingJson["toll_plaza_icon"].AsString();
                    setting.traffic_alert_icon = settingJson["traffic_alert_icon"].AsString();
                    setting.road_closure_icon = settingJson["road_closure_icon"].AsString();
                    setting.poi_closure_icon = settingJson["poi_closure_icon"].AsString();
                    setting.vista_point_icon = settingJson["vista_point_icon"].AsString();
                    setting.csc_icon = settingJson["csc_icon"].AsString();
                    setting.petrol_station_icon = settingJson["petrol_station_icon"].AsString();
                    setting.tunnel_icon = settingJson["tunnel_icon"].AsString();
                    setting.interchange_icon = settingJson["interchange_icon"].AsString();

                    return new ServiceResult() { Data = setting, intStatus = 1 };
                }
                else
                {
                    LogUtils.WriteError("getEntityByDomain", "intStatus:" + itemJson["result"]["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemJson["result"][" strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getEntityByDomain", ex.Message);
            }
            return null;
        }

        public static async Task<ServiceResult> Register(string UserName, string strPassword, string strMobileNo, string strFirstName, string strLastName, MyEntity entity)
        {
            try
            {
                List<TblEntities> result = new List<TblEntities>();
                BaseClient client = new BaseClient();
                client.StrMethod = "memberRegister";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                client.AddParameter("strEmail", UserName);
                client.AddParameter("strPassword", strPassword);
                client.AddParameter("strMobileNo", strMobileNo);
                client.AddParameter("strFirstName", strFirstName);
                client.AddParameter("strLastName", strLastName);
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    var data = itemResult["data"];
                    UserInfos info = new UserInfos();
                    info.strToken = data["strSessionID"].AsString();
                    info.strUserName = data["profile"]["strEmail"].AsString();
                    info.strMobileNo = data["profile"]["strMobileNo"].AsString();
                    info.strFirstName = data["profile"]["strFirstName"].AsString();
                    info.strLastName = data["profile"]["strLastName"].AsString();
                    info.isActive = data["profile"]["isActive"].AsInt() == 1 ? true : false;
                    info.intStatus = data["profile"]["intStatus"].AsInt();
                    info.dtCreateDate = data["profile"]["dtCreateDate"].AsDateTime();

                    var loyRegis = await Loyalty.Processing.Connections.UserAccount.Register(90, info.strUserName, "a5309d3c348d152582424506a142a685", "", info.strFirstName, info.strLastName, "", info.strUserName);


                    //info.strAvatar = itemJson.result.data.profile.strPicture;
                    return new ServiceResult() { Data = info, intStatus = 1 };
                }
                else
                {
                    LogUtils.WriteError("Register", "intStatus:" + itemResult["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemResult["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Register", ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Only use for logined sysem by username/password
        /// Call API:
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="strSession"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public static Task<UserInfos> ChangePassword(string idUser,string strSession,string oldPassword, string newPassword)
        {
            return null;
        }

        /// <summary>
        /// Call API:
        /// </summary>
        /// <param name="strAppId"></param>
        /// <returns></returns>
        public static async Task<ServiceResult> GetListEntities( )
        {
            try
            {
                List<TblEntities> result = new List<TblEntities>();
                BaseClient client = new BaseClient();
                client.StrMethod = "getEntityList";
                string strJson =  await  client.getData();
                dynamic itemJson = JsonConvert.DeserializeObject(strJson);
                if (itemJson.result.intStatus == 1)
                {
                    foreach (var json in itemJson.result.data)
                    {
                        string id = json.idEntity;
                        string name = json.strName;
                        TblEntities itemEntity = new TblEntities();
                        itemEntity.idEntity = id;
                        itemEntity.strName = name;
                        result.Add(itemEntity);
                    }
                    return new ServiceResult() { Data = result, intStatus = 1 };
                }
                else
                {
                    LogUtils.WriteError("GetListEntities", "intStatus:" + itemJson.result.intStatus);
                    return new ServiceResult() { Data = null, intStatus = itemJson.result.intStatus, strMess = itemJson.result.strMessage };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetListEntities", ex.Message);
            }
            return null;
        }


        public static async Task<ServiceResult> getAPIKey(string idEntity)
        {
            try
            {
                List<TblEntities> result = new List<TblEntities>();
                BaseClient client = new BaseClient();
                client.StrMethod = "getApiKey";
                client.AddParameter("idEntity", idEntity);
                string strJson = await client.getData();
                dynamic itemJson = JsonConvert.DeserializeObject(strJson);
                var item = itemJson["result"];
                var intStatus = (long)item["intStatus"].Value;
                //var intStatus = int.Parse();

                //var testData = item.data;
               // var intStatus = 1;
                if (intStatus == 1)
                {
                    var data = item["data"];
                    string api_id = (string)data["api_id"].Value;
                    string api_key = (string)data["api_key"].Value;
                    string cctv_secret_key = (string)data["cctv_secret_key"].Value; 
                    string cctv_encryption = (string)data["cctv_encryption"].Value; 
                    string sub_domain = (string)data["sub_domain"].Value; 


                    MyEntity entity = new MyEntity();
                    entity.api_id = api_id;
                    entity.api_key = api_key;
                    entity.cctv_secret_key = cctv_secret_key;
                    entity.cctv_encryption = cctv_encryption;
                    entity.sub_domain = sub_domain;
                    entity.idEntity = idEntity;

                    var domain = await getEntityByDomain(entity.sub_domain);
                    if (domain.intStatus == 1)
                    {
                        entity.mSettings = (domain.Data as Settings);
                    }
                    return new ServiceResult() { intStatus = 1, Data = entity, strMess = "" };
                }
                else
                {
                    LogUtils.WriteError("getAPIKey", "intStatus:" + itemJson.result.intStatus);
                    return new ServiceResult() { Data = null, intStatus = itemJson.result.intStatus, strMess = itemJson.result.strMessage };
                }
            }
            catch (Exception ex)

            {
                LogUtils.WriteError("getAPIKey", ex.Message);
            }
            return null;
        }

        public static async Task<ServiceResult> MemberForgetpassword(string strEmail, MyEntity entity)
        {
            try
            {
                List<TblEntities> result = new List<TblEntities>();
                BaseClient client = new BaseClient();
                client.StrMethod = "memberForgotPassword";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                client.AddParameter("strEmail", strEmail);
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    return new ServiceResult() { Data = null, intStatus = itemResult["intStatus"].AsInt(), strMess = itemResult["strMessage"].AsString() };
                }
                else
                {
                    LogUtils.WriteError("MemberForgetpassword", "intStatus:" + itemResult["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemJson["result"]["intStatus"].AsInt(), strMess = itemJson["result"][" strMessage"].AsString() };
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("MemberForgetpassword", ex.Message);
            }
            return null;
        }

        public static async Task<ServiceResult> LoginSocial(string provider, string token, ServiceResult apiKey)
        {
            try
            {
                
                var entity = apiKey.Data as MyEntity;
                List<TblEntities> result = new List<TblEntities>();
                BaseClient client = new BaseClient();
                client.StrMethod = "socialLogin";
                client.AddParameter("api_id", entity.api_id);
                client.AddParameter("api_key", entity.api_key);
                client.AddParameter("provider", provider);
                client.AddParameter("access_token", token);
                string strJson = await client.getData();
                var itemJson = JObject.Parse(strJson);
                var itemResult = itemJson["result"];
                if (itemResult["intStatus"].AsInt() == 1)
                {
                    var data = itemResult["data"];
                    UserInfos info = new UserInfos();
                    info.strToken = data["strSessionID"].AsString();
                    info.strUserName = data["profile"]["strEmail"].AsString();
                    info.strMobileNo = data["profile"]["strMobileNo"].AsString();
                    info.strFirstName = data["profile"]["strFirstName"].AsString();
                    info.strLastName = data["profile"]["strLastName"].AsString();
                    info.strAvatar = data["profile"]["strPicture"].AsString();
                    int isNewRecord = data["isNewRecord"].AsInt();
                    if (isNewRecord == 1)  //0 Login, 1 Register
                    {
                        //register
                       var loyRegis = await Loyalty.Processing.Connections.UserAccount.Register(90, info.strUserName, "a5309d3c348d152582424506a142a685", "", info.strFirstName, info.strLastName, "", info.strUserName);
                       // var loyLogin = await Loyalty.Processing.Connections.UserAccount.Login(90, info.strUserName, "", "a5309d3c348d152582424506a142a685");
                    }
                    else
                    {
                        //var loyRegis = await Loyalty.Processing.Connections.UserAccount.Register(90, info.strUserName, "a5309d3c348d152582424506a142a685", "", info.strFirstName, info.strLastName, "", info.strUserName);
                    }

                    var loyLogin = await Loyalty.Processing.Connections.UserAccount.Login(info.strUserName, "", "a5309d3c348d152582424506a142a685");
                    if (loyLogin.StatusCode != 1)
                    {
                        var loyRegis = await Loyalty.Processing.Connections.UserAccount.Register(90, info.strUserName, "a5309d3c348d152582424506a142a685", "", info.strFirstName, info.strLastName, "", info.strUserName);
                        loyLogin = await Loyalty.Processing.Connections.UserAccount.Login(info.strUserName, "", "a5309d3c348d152582424506a142a685");
                    }
                    info.LoyaltyAccount = loyLogin;

                    return new ServiceResult() { Data = info, intStatus = 1 };
                }
                else
                {
                    LogUtils.WriteError("LoginSocial", "intStatus:" + itemResult["intStatus"].AsInt());
                    return new ServiceResult() { Data = null, intStatus = itemResult["intStatus"].AsInt(), strMess = itemResult["strMessage "].AsString()};
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("LoginSocial", ex.Message);
            }
            return null;
        }
    }
}
