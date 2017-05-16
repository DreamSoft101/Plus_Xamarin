using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EXPRESSO.Threads;
//using EXPRESSO.Utils;
//using EXPRESSO.Models;
using Java.Lang;
using Android.Support.V7.App;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Loyalty.Models;
using System.Threading;
using Newtonsoft.Json;
//using EXPRESSO.Models.Database;
using Dex.Com.Expresso.Utils;
using Square.Picasso;
using System.Net.Http;
using Android.Graphics;
using System.Net;
using Dex.Com.Expresso.Activities;
using Loyalty.Processing.Connections;
using Loyalty.Models.Database;
using Loyalty.Processing.LocalData;
using Loyalty.Models.ServiceOutput.Database;
using Loyalty.Utils;

namespace Dex.Com.Expresso.Services
{
    [Service]
    public class SynceService : Service
    {
        public static string ENTITYSETTINGPREFIX = "SETTINGENTITY_";
        public static string SETTING_NOTIFICATION = "NOTIFICATION";
        public static string FAVORITE_LOCATION = "LOCATION";
        public static string MYENTITY = "myentity";
        public static string ENTITYLASTUPDATEPREFIX = "LASTUPDATE_";
        public static string LastUpdate = "LASTUPDATE";
        public static string SYNCEDATA_LOYALTY = "DATA_LOYALTY";
        public static string SYNCEDATA_PLUS = "DATA_PLUS";
        public static string SYNCEDATA_LOYALTY_INDEX = "SYNCEDATA_LOYALTY_INDEX";
        public static string SYNCEDATA_PLUS_INDEX = "SYNCEDATA_LOYALTY_INDEX";
        private IBinder mBinder;
        private bool isRunning = true;
        private bool IsOld = false;
        private bool isOldLoyalty = false;
        public static DateTime FirstUpdate = new DateTime(1990, 11, 2);
        public ISharedPreferences mSharedPref;


        private NotificationCompat.Builder builderloyalty;
        private NotificationCompat.Builder builderExpresso;
        private NotificationManager notificationManager;

        private int notifyIDLoyalty = 19891931;
        private int notifyIDExpresso =  19891932;

        private DateTime LastCheck = DateTime.Now.AddHours(-1);
        
       

        public List<EXPRESSO.Models.SettingNotification> getSettingNotification()
        {
            string mycache = getCacheString(ENTITYSETTINGPREFIX + EXPRESSO.Utils.Cons.IdEntity + "_" + SETTING_NOTIFICATION);
            var items = EXPRESSO.Utils.StringUtils.String2Object<List<EXPRESSO.Models.SettingNotification>>(mycache);
            if (items == null)
            {
                items = new List<EXPRESSO.Models.SettingNotification>();
            }
            return items;
        }


        public DateTime getLastUpdate()
        {
            string mycache = getCacheString(ENTITYLASTUPDATEPREFIX + EXPRESSO.Utils.Cons.IdEntity);
            try
            {
                DateTime result = DateTime.Parse(mycache);
                return result;
            }
            catch
            {
                return new DateTime(1990, 11, 2);
            }
        }

        public void setLastUpdate(DateTime dtStart)
        {   
            string mycache = dtStart.ToString("yyyy-MM-dd HH:mm:ss");
            //string mycache = (new DateTime(2016,07,01).ToString("yyyy-MM-dd HH:mm:ss"));
            setCacheString(ENTITYLASTUPDATEPREFIX + EXPRESSO.Utils.Cons.IdEntity, mycache);
            EXPRESSO.Utils.LogUtils.WriteLog("setLastUpdate", mycache);
        }

        public string getCacheString(string key)
        {
            return mSharedPref.GetString(key, "");
        }

        public int getCacheInt(string key)
        {
            return mSharedPref.GetInt(key, 0);
        }

        public void setCacheInt(string key, int value)
        {
            ISharedPreferencesEditor editor = mSharedPref.Edit();
            editor.PutInt(key, value);
            editor.Apply();
            editor.Commit();
        }

        public void setCacheString(string key, string value)
        {
            ISharedPreferencesEditor editor = mSharedPref.Edit();
            editor.PutString(key, value);
            editor.Apply();
            editor.Commit();
        }

        
        public DateTime getLastUpdateLoy()
        {
            string mycache = getCacheString(LastUpdate);
            try
            {
                DateTime result = DateTime.Parse(mycache);
                return result;
            }
            catch
            {
                return FirstUpdate;
            }
        }
        public void setLastUpdateLoyalty(DateTime dtStart)
        {
            string mycache = dtStart.ToString("yyyy-MM-dd HH:mm:ss ");
            //string mycache = (new DateTime(2016,07,01).ToString("yyyy-MM-dd HH:mm:ss"));
            setCacheString(LastUpdate, mycache);
        }

        public override void OnCreate()
        {
            base.OnCreate();
            mSharedPref = this.GetSharedPreferences("expresso", FileCreationMode.MultiProcess);
            notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);


            builderExpresso = new NotificationCompat.Builder(this);
            builderExpresso.SetAutoCancel(false);
            builderExpresso.SetContentTitle(GetString(Resource.String.text_plusexpressways_sync));
            builderExpresso.SetSmallIcon(Resource.Drawable.Icon);

            builderloyalty = new NotificationCompat.Builder(this);
            builderloyalty.SetAutoCancel(false);
            builderloyalty.SetContentTitle(GetString(Resource.String.text_plusmile_sync));
            builderloyalty.SetSmallIcon(Resource.Drawable.Icon);


            //builderExpresso.SetContentText(string.Format(GetString(Resource.String.text_plusexpressways_sync), lstItem.Count)); // The message to display.
            // notificationManager.Notify(notifyID, builderExpresso.Build());

            new System.Threading.Thread(new ThreadStart(async () =>
            {
                try
                {
                    while (string.IsNullOrEmpty(EXPRESSO.Utils.Cons.IdEntity))
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    while (EXPRESSO.Utils.Cons.myEntity == null)
                    {
                        System.Threading.Thread.Sleep(100);
                    }

                    while (isRunning)
                    {
                        System.Threading.Thread.Sleep(1000);
                        // var result = await EXPRESSO.Processing.Connections.UpdateConnections.getUpdateData("");
                        if ((DateTime.Now - LastCheck).TotalMinutes >= 5)
                        {
                            if (EXPRESSO.Utils.Cons.IdEntity != null)
                            {
                                IsOld = false;
                                LastCheck = DateTime.Now;
                                DateTime mDtStartExpresso = DateTime.UtcNow;

                                #region [Setting]
                                EXPRESSO.Models.ServiceResult settingsResult = await EXPRESSO.Processing.Connections.UserAccountConnection.getEntityByDomain(EXPRESSO.Utils.Cons.myEntity.sub_domain);
                                if (settingsResult.intStatus == 1)
                                {
                                    EXPRESSO.Utils.Cons.myEntity.mSettings = settingsResult.Data as EXPRESSO.Models.Settings;
                                }
                                #endregion

                                #region [Expresso]
                                string jsonOldDataExpresso = getCacheString(SYNCEDATA_PLUS);
                                string jsonSyncDataExpresso = "";
                                SynData_Expresso data = JsonConvert.DeserializeObject<SynData_Expresso>(jsonOldDataExpresso);
                                if (data != null)
                                {
                                    if (data.Data != null)
                                    {
                                        mDtStartExpresso = data.DateStart;
                                        //OnGetData_Expresso(data.Data);
                                        jsonSyncDataExpresso = data.Data;
                                        IsOld = true;
                                    }
                                    else
                                    {
                                        IsOld = false;
                                        jsonSyncDataExpresso = await EXPRESSO.Processing.Connections.UpdateConnections.getUpdateData(getLastUpdate().ToString("yyyy-MM-dd HH:mm:ss"));
                                        
                                    }
                                }
                                else
                                {
                                    IsOld = false;
                                    jsonSyncDataExpresso = await EXPRESSO.Processing.Connections.UpdateConnections.getUpdateData(getLastUpdate().ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                                if (!string.IsNullOrEmpty(jsonSyncDataExpresso))
                                {
                                    //SaveCache_Expresso(jsonSyncDataExpresso, mDtStartExpresso);
                                    SynData_Expresso sync = new SynData_Expresso() { DateStart = mDtStartExpresso, Data = jsonSyncDataExpresso };
                                    var saveData = JsonConvert.SerializeObject(sync);
                                    setCacheString(SYNCEDATA_PLUS, saveData);

                                    var dataGened = await EXPRESSO.Processing.Connections.UpdateConnections.GenerateData(jsonSyncDataExpresso);
                                    SyncExpresso(dataGened, mDtStartExpresso);
                                }
                                else
                                {

                                }
                                #endregion

                                #region [Loyalty]
                                DateTime mDtStartLoyalty;
                                string jsonOldataLoyalty = getCacheString(SYNCEDATA_LOYALTY);
                                MBB_GetData syncData = null;
                                if (string.IsNullOrEmpty(jsonOldataLoyalty))
                                {
                                    isOldLoyalty = false;
                                    //threadloyalty.GetData(getLastUpdateLoy(), new M_BBGetDataDeletedID());
                                    syncData = await MasterData.GetData(getLastUpdateLoy(), new M_BBGetDataDeletedID());
                                    mDtStartLoyalty = syncData.LastGet;
                                }
                                else
                                {
                                    syncData = JsonConvert.DeserializeObject<MBB_GetData>(jsonOldataLoyalty);
                                    if (data != null)
                                    {
                                        isOldLoyalty = true;
                                        syncData = await MasterData.GetData(getLastUpdateLoy(), new M_BBGetDataDeletedID());
                                    }
                                    mDtStartLoyalty = syncData.LastGet;
                                }
                                if (syncData != null)
                                {
                                    SyncLoyalty(syncData, mDtStartLoyalty);
                                }

                                #endregion

                                LastCheck = DateTime.Now;
                                //threadloyalty.GetData(getLastUpdateLoy(), new M_BBGetDataDeletedID());
                            }
                        }
                        System.Threading.Thread.Sleep(5000);
                    }
                }
                catch (System.Exception ex)
                {

                }
            })).Start();


            new System.Threading.Thread(new ThreadStart(() =>
            {

                while (true)
                {
                    try
                    {
                        List<EXPRESSO.Models.SettingNotification> lstOr = getSettingNotification();
                        List<EXPRESSO.Models.SettingNotification> lstLiveTraffic = new List<EXPRESSO.Models.SettingNotification>();
                        DateTime dtNow = DateTime.Now;
                        foreach (var item in lstOr)
                        {
                            string datetime = item.dtTime.ToString("HH:mm");
                            string strNow = dtNow.ToString("HH:mm");
                            if (datetime == strNow)
                            {
                                lstLiveTraffic.Add(item);
                            }
                        }





                        var listOK = lstLiveTraffic;// lstLiveTraffic.Where(p => p.dtTime.Hour == dtNow.Hour && p.dtTime.Minute == dtNow.Minute).ToList();
                        if (listOK.Count > 0)
                        {
                            var lstTollPlaza = listOK.Where(p => p.intType == 2).Select(P => P.idParent).ToList();
                            if (lstTollPlaza.Count > 0)
                            {
                                notification_TollPlaza(lstTollPlaza);
                            }

                            var lstLiveTrafficx = listOK.Where(p => p.intType == 1).Select(p => p.idParent).ToList();
                            if (lstLiveTrafficx.Count > 0)
                            {
                                notification_LiveTraffic(lstLiveTrafficx);
                            }

                            var listLiveTraffic13 = listOK.Where(p => p.intType == 3).Select(p => p.idParent).ToList();
                            if (listLiveTraffic13.Count > 0)
                            {
                                notification_LiveTraffic13(listLiveTraffic13);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {

                    }

                    System.Threading.Thread.Sleep(59000);
                }


            })).Start();
        }



        private void notification_LiveTraffic(List<string> ids)
        {
            

                List<EXPRESSO.Models.Database.TrafficUpdate> trafficDetails = new List<EXPRESSO.Models.Database.TrafficUpdate>();

                LiveTrafficThread trafficThread = new LiveTrafficThread();
                var types = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14 };
                MastersThread threadM = new MastersThread();
                threadM.OnLoadListHighway += (List<EXPRESSO.Models.Database.TblHighway> resultx) =>
                {
                    var highways = resultx.Select(p => p.idHighway).ToList();
                    trafficThread.OnLoadLiveTrafficResult += (EXPRESSO.Models.ServiceResult resulttraffic) =>
                    {
                        if (resulttraffic.intStatus == 1)
                        {
                            List<EXPRESSO.Models.Database.TrafficUpdate> lstTraffic = resulttraffic.Data as List<EXPRESSO.Models.Database.TrafficUpdate>;
                            lstTraffic = lstTraffic.Where(p => ids.Contains(p.idTrafficUpdate)).ToList();
                            foreach (var detail in lstTraffic)
                            {
                                int icon = ResourceUtil.GetResourceID(this, "ic_menu_traffic_type" + detail.intType);
                                var builder = new NotificationCompat.Builder(this);
                                builder.SetContentTitle(GetString(Resource.String.app_name));
                                builder.SetAutoCancel(false);
                                builder.SetContentText(detail.strTitle);
                                builder.SetSmallIcon(icon);
                                builder.SetContentText(detail.strDescription); // The message to display.
                                builder.SetSmallIcon(Resource.Drawable.Icon);


                                Intent intent = new Intent(this, typeof(LiveTrafficDetailActivity));
                                intent.PutExtra(LiveTrafficDetailActivity.DATA, JsonConvert.SerializeObject(detail));
                                intent.SetFlags(ActivityFlags.ClearTop);
                                var pendingIntent = PendingIntent.GetActivity(Application.Context, 0, intent, PendingIntentFlags.UpdateCurrent);
                                builder.SetContentIntent(pendingIntent);
                                notificationManager.Notify((new Random()).Next(0, 100000), builder.Build());

                            }
                        }
                    };
                    trafficThread.GetLiveTraffic(types, highways, null);
                };
                threadM.loadListHighway();
           
        }

        private void notification_LiveTraffic13(List<string> ids)
        {
            //var listFavoriteLocation = getFavoriteLocation().Where(p => ids.Contains(p.idFavoriteLocation)).ToList();
            List<EXPRESSO.Models.Database.TrafficUpdate> trafficDetails = new List<EXPRESSO.Models.Database.TrafficUpdate>();

            LiveTrafficThread trafficThread = new LiveTrafficThread();
            var types = new List<int>() { 13 };
            MastersThread threadM = new MastersThread();
            threadM.OnLoadListHighway += (List<EXPRESSO.Models.Database.TblHighway> resultx) =>
            {
                var highways = resultx.Select(p => p.idHighway).ToList();
                trafficThread.OnLoadLiveTrafficResult += (EXPRESSO.Models.ServiceResult resulttraffic) =>
                {
                    if (resulttraffic.intStatus == 1)
                    {
                        List<EXPRESSO.Models.Database.TrafficUpdate> lstTraffic = resulttraffic.Data as List<EXPRESSO.Models.Database.TrafficUpdate>;
                        lstTraffic = lstTraffic.Where(p => ids.Contains(p.idTrafficUpdate)).ToList();
                        foreach (var detail in lstTraffic)
                        {
                            Bitmap imageBitmap = null;

                            using (var webClient = new WebClient())
                            {
                                var imageBytes = webClient.DownloadData(detail.strURL);
                                if (imageBytes != null && imageBytes.Length > 0)
                                {

                                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                                    var builder = new Notification.Builder(this);
                                    builder.SetAutoCancel(false);
                                    builder.SetContentTitle(GetString(Resource.String.app_name));
                                    builder.SetContentText(detail.strDescription);
                                    builder.SetStyle(new Notification.BigPictureStyle().BigPicture(imageBitmap));
                                    builder.SetSmallIcon(Resource.Drawable.Icon);

                                    Intent intent = new Intent(this, typeof(LiveFeedDetailActivity));
                                    intent.SetFlags(ActivityFlags.ClearTop );
                                    intent.PutExtra(LiveFeedDetailActivity.DATA, JsonConvert.SerializeObject(detail));
                                    intent.PutExtra(LiveFeedDetailActivity.MODELISFAVORITE, true);
                                    intent.PutExtra(LiveFeedDetailActivity.MODETYPE, LiveFeedDetailActivity.MODETYPE_HIGHWAY);
                                    intent.SetAction(EXPRESSO.Utils.StringUtils.RandomString(10, true));
                                    var pendingIntent = PendingIntent.GetActivity(Application.Context, (new Random()).Next(0,10000), intent, PendingIntentFlags.UpdateCurrent);

                                    builder.SetContentIntent(pendingIntent);
                                    notificationManager.Notify((new Random()).Next(0, 100000), builder.Build());
                                }
                            }
                        }
                    }
                };
                trafficThread.GetLiveTraffic(types, highways, null);
            };
            threadM.loadListHighway();
        }

        private void notification_TollPlaza(List<string> ids)
        {
            MastersThread threadM = new MastersThread();
            threadM.OnLoadListHighway += (List<EXPRESSO.Models.Database.TblHighway> result) =>
            {
                var highways = result.Select(p => p.idHighway).ToList();
                LiveTrafficThread thread = new LiveTrafficThread();
                thread.OnLoadFacilityCCTV += (EXPRESSO.Models.ServiceResult resultx) =>
                {
                    if (resultx.intStatus == 1)
                    {
                        ImageView img = new ImageView(this);
                        List<EXPRESSO.Models.BaseItem> lstItem = resultx.Data as List<EXPRESSO.Models.BaseItem>;
                        var items = lstItem.Where(p => p.Item is EXPRESSO.Models.TollPlazaCCTV).ToList();//
                        items = items.Where(p => ids.Contains((p.Item as EXPRESSO.Models.TollPlazaCCTV).idTollPlazaCctv)).ToList();
                        foreach (var item in items)
                        {
                            
                            if (item.Item is EXPRESSO.Models.TollPlazaCCTV)
                            {
                                var tollplaza = item.Item as EXPRESSO.Models.TollPlazaCCTV;
                                Bitmap imageBitmap = null;

                                using (var webClient = new WebClient())
                                {
                                    var imageBytes = webClient.DownloadData(tollplaza.strCCTVImage);
                                    if (imageBytes != null && imageBytes.Length > 0)
                                    {
                                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                                        img.BuildDrawingCache(true);
                                        var builder = new Notification.Builder(this);
                                        builder.SetAutoCancel(false);

                                        builder.SetContentTitle(GetString(Resource.String.app_name));
                                        builder.SetContentText(tollplaza.strDescription);
                                        builder.SetStyle(new Notification.BigPictureStyle().BigPicture(imageBitmap));
                                        builder.SetSmallIcon(Resource.Drawable.Icon);

                                        Intent intent = new Intent(this, typeof(LiveFeedDetailActivity));
                                        intent.SetFlags(ActivityFlags.ClearTop );
                                        intent.PutExtra(LiveFeedDetailActivity.DATA, JsonConvert.SerializeObject(tollplaza));
                                        intent.PutExtra(LiveFeedDetailActivity.MODELISFAVORITE, true);
                                        intent.PutExtra(LiveFeedDetailActivity.MODETYPE, LiveFeedDetailActivity.MODETYPE_TOLLPLAZA);
                                        intent.SetAction(EXPRESSO.Utils.StringUtils.RandomString(10, true));
                                        var pendingIntent = PendingIntent.GetActivity(Application.Context, (new Random()).Next(0, 10000), intent, PendingIntentFlags.UpdateCurrent);

                                        builder.SetContentIntent(pendingIntent);

                                        notificationManager.Notify((new Random()).Next(0,100000), builder.Build());
                                    }
                                }
                            }
                            
                        }
                    }
                };
                thread.loadFacilityCCTV(highways, 2);
            };
            threadM.loadListHighway();
         
        }

        public override IBinder OnBind(Intent intent)
        {
            //throw new NotImplementedException();
            return mBinder;
        }

        public class SynData_Expresso
        {
            public DateTime DateStart { get; set; }
            public string Data {get; set;}
        }
    
        private void SyncExpresso(EXPRESSO.Models.ServiceResult result, DateTime dtStart)
        {
            if (result.intStatus == 1)
            {
                List<EXPRESSO.Models.BaseItem> lstItem = result.Data as List<EXPRESSO.Models.BaseItem>;
                if (lstItem != null)
                {
                    if (lstItem.Count > 0)
                    {

                    
                        int start = 0;
                        if (IsOld)
                        {
                            start = getCacheInt(SYNCEDATA_PLUS_INDEX);
                            //thread.UpdateData(lstItem, start);
                        }
                        var lstCache = new List<EXPRESSO.Models.BaseItem>();
                        for (int i = start; i < lstItem.Count; i++)
                        {
                            var baseItem = lstItem[i];
                            lstCache.Add(baseItem);
                            try
                            {
                                if (lstCache.Count > 500 || i == lstItem.Count - 1)
                                {

                                    EXPRESSO.Processing.LocalData.LoadData.updateData(lstCache);
                                    lstCache.Clear();
                                    //Changed

                                    builderExpresso.SetProgress(lstItem.Count, i, false);
                                    notificationManager.Notify(notifyIDExpresso, builderExpresso.Build());
                                }
                            }
                            catch (System.Exception ex)
                            {
                                LogUtils.WriteError("SyncExpresso", baseItem.Item.ToString() + ":" + ex.Message);
                            }
                        }
                        //finish
                        setLastUpdate(dtStart);
                        setCacheString(SYNCEDATA_PLUS, "");

                        builderExpresso.SetContentText(GetString(Resource.String.mess_update_finish));
                        builderExpresso.SetProgress(lstItem.Count, lstItem.Count, false);
                        notificationManager.Notify(notifyIDExpresso, builderExpresso.Build());
                    }
                    else
                    {
                        setLastUpdate(dtStart);
                        setCacheString(SYNCEDATA_PLUS, "");
                    }
                }
                else
                {
                    setLastUpdate(dtStart);
                    setCacheString(SYNCEDATA_PLUS, "");

                }

            }
        }


        private void SyncLoyalty(MBB_GetData data, DateTime mDtStartLoyalty)
        {
       
            var saveData = JsonConvert.SerializeObject(data);
            setCacheString(SYNCEDATA_LOYALTY, saveData);

            mDtStartLoyalty = data.LastGet;
            var lstItem = new List<BaseItem>();
            if (data.MemberTypes != null)
            {
                foreach (var item in data.MemberTypes)
                {
                    BaseItem bitem = new BaseItem();
                    bitem.Item = item;
                    lstItem.Add(bitem);
                }
            }
            if (data.Merchants != null)
            {
                foreach (var item in data.Merchants)
                {
                    BaseItem bitem = new BaseItem();
                    bitem.Item = item;
                    lstItem.Add(bitem);
                }
            }
            if (data.MerchantCategories != null)
            {
                foreach (var item in data.MerchantCategories)
                {
                    BaseItem bitem = new BaseItem();
                    bitem.Item = item;
                    lstItem.Add(bitem);
                }
            }
            if (data.MerchantProducts != null)
            {
                foreach (var item in data.MerchantProducts)
                {
                    BaseItem bitem = new BaseItem();
                    bitem.Item = item;
                    lstItem.Add(bitem);
                }
            }
            if (data.RedemptionCategories != null)
            {
                foreach (var item in data.RedemptionCategories)
                {
                    BaseItem bitem = new BaseItem();
                    bitem.Item = item;
                    lstItem.Add(bitem);
                }
            }
            if (data.RedemptionProducts != null)
            {
                foreach (var item in data.RedemptionProducts)
                {
                    BaseItem bitem = new BaseItem();
                    bitem.Item = item;
                    lstItem.Add(bitem);
                }
            }
            if (data.RedemptionProductDetails != null)
            {
                foreach (var item in data.RedemptionProductDetails)
                {
                    BaseItem bitem = new BaseItem();
                    bitem.Item = item;
                    lstItem.Add(bitem);
                }
            }
            if (data.RedemptionPartners != null)
            {
                foreach (var item in data.RedemptionPartners)
                {
                    BaseItem bitem = new BaseItem();
                    bitem.Item = item;
                    lstItem.Add(bitem);
                }
            }
            if (data.MerchantLocations != null)
            {
                foreach (var item in data.MerchantLocations)
                {
                    BaseItem bitem = new BaseItem();
                    bitem.Item = item;
                    lstItem.Add(bitem);
                }
            }
            if (data.MerchantProductMemberTypes != null)
            {
                foreach (var item in data.MerchantProductMemberTypes)
                {
                    BaseItem bitem = new BaseItem();
                    bitem.Item = item;
                    lstItem.Add(bitem);
                }
            }
            if (data.MemberGroups != null)
            {
                foreach (var item in data.MemberGroups)
                {
                    BaseItem bitem = new BaseItem();
                    bitem.Item = item;
                    lstItem.Add(bitem);
                }
            }
            if (data.MemberGroupDetails != null)
            {
                foreach (var item in data.MemberGroupDetails)
                {
                    BaseItem bitem = new BaseItem();
                    bitem.Item = item;
                    lstItem.Add(bitem);
                }
            }
            if (data.States != null)
            {
                foreach (var item in data.States)
                {
                    BaseItem bitem = new BaseItem();
                    bitem.Item = item;
                    lstItem.Add(bitem);
                }
            }
            if (data.Countries != null)
            {
                foreach (var item in data.Countries)
                {
                    BaseItem bitem = new BaseItem();
                    bitem.Item = item;
                    lstItem.Add(bitem);
                }
            }
            if (lstItem.Count > 0)
            {
                int start = 0;
                if (isOldLoyalty)
                {
                    start = getCacheInt(SYNCEDATA_LOYALTY_INDEX);
                  
                }
                UpdateDBLoyalty(lstItem, start, mDtStartLoyalty);
            }
            else
            {
                setCacheString(SYNCEDATA_LOYALTY, "");
                setLastUpdateLoyalty(mDtStartLoyalty);
            }
        }

        public void UpdateDBLoyalty(List<BaseItem> lstItem,int start, DateTime dtStart)
        {
            try
            {
                List<BaseItem> lstCache = new List<BaseItem>();
                List<MasterThreads.Action> lstActions = new List<MasterThreads.Action>();
                for (int i = start; i < lstItem.Count; i++)
                {

                    MasterThreads.Action action = MasterThreads.Action.Delete;
                    var baseItem = lstItem[i];
                    lstCache.Add(baseItem);


                    string jsonData = JsonConvert.SerializeObject(baseItem.Item);
                    LogUtils.WriteLog("UpdateData", i + ":" + jsonData);
                    if (baseItem.Item is MemberType)
                    {
                        MemberType item = baseItem.Item as MemberType;
                        if (BaseDatabase.getDB().Table<MemberType>().Where(p => p.MemberTypeID == item.MemberTypeID).Count() > 0)
                        {
                         
                            action = MasterThreads.Action.Update;
                        }
                        else
                        {
                            action = MasterThreads.Action.AddNew;
                        }
                    }
                    else if (baseItem.Item is Merchant)
                    {
                        Merchant item = baseItem.Item as Merchant;
                        if (BaseDatabase.getDB().Table<Merchant>().Where(p => p.MerchantID == item.MerchantID).Count() > 0)
                        {
                            
                            action = MasterThreads.Action.Update;
                        }
                        else
                        {
                            
                            action = MasterThreads.Action.AddNew;
                        }
                    }
                    else if (baseItem.Item is MerchantCategory)
                    {
                        MerchantCategory item = baseItem.Item as MerchantCategory;
                        if (BaseDatabase.getDB().Table<MerchantCategory>().Where(p => p.MerchantCategoryID == item.MerchantCategoryID).Count() > 0)
                        {
                            action = MasterThreads.Action.Update;
                        }
                        else
                        {
                            action = MasterThreads.Action.AddNew;
                        }
                    }
                    else if (baseItem.Item is MerchantProduct)
                    {
                        MerchantProduct item = baseItem.Item as MerchantProduct;
                        if (BaseDatabase.getDB().Table<MerchantProduct>().Where(p => p.MerchantProductID == item.MerchantProductID).Count() > 0)
                        {
                            action = MasterThreads.Action.Update;
                        }
                        else
                        {
                            action = MasterThreads.Action.AddNew;
                        }
                    }
                    else if (baseItem.Item is RedemptionCategory)
                    {
                        RedemptionCategory item = baseItem.Item as RedemptionCategory;
                        if (BaseDatabase.getDB().Table<RedemptionCategory>().Where(p => p.RedemptionCategoryID == item.RedemptionCategoryID).Count() > 0)
                        {
                            action = MasterThreads.Action.Update;
                        }
                        else
                        {
                            action = MasterThreads.Action.AddNew;
                        }
                    }
                    else if (baseItem.Item is RedemptionProduct)
                    {
                        RedemptionProduct item = baseItem.Item as RedemptionProduct;
                        if (BaseDatabase.getDB().Table<RedemptionProduct>().Where(p => p.RedemptionProductID == item.RedemptionProductID).Count() > 0)
                        {
                            action = MasterThreads.Action.Update;
                        }
                        else
                        {
                            action = MasterThreads.Action.AddNew;
                        }
                    }
                    else if (baseItem.Item is RedemptionProductDetail)
                    {
                        RedemptionProductDetail item = baseItem.Item as RedemptionProductDetail;
                        if (BaseDatabase.getDB().Table<RedemptionProductDetail>().Where(p => p.RedemptionProductDetailID == item.RedemptionProductDetailID).Count() > 0)
                        {
                            action = MasterThreads.Action.Update;
                        }
                        else
                        {
                            action = MasterThreads.Action.AddNew;
                        }
                    }
                    else if (baseItem.Item is RedemptionPartner)
                    {
                        RedemptionPartner item = baseItem.Item as RedemptionPartner;
                        if (BaseDatabase.getDB().Table<RedemptionPartner>().Where(p => p.PartnerID == item.PartnerID).Count() > 0)
                        {
                            action = MasterThreads.Action.Update;
                        }
                        else
                        {
                            action = MasterThreads.Action.AddNew;
                        }
                    }
                    else if (baseItem.Item is MerchantLocation)
                    {
                        MerchantLocation item = baseItem.Item as MerchantLocation;
                        if (BaseDatabase.getDB().Table<MerchantLocation>().Where(p => p.idMerchantLocation == item.idMerchantLocation).Count() > 0)
                        {
                            action = MasterThreads.Action.Update;
                        }
                        else
                        {
                            action = MasterThreads.Action.AddNew;
                        }
                    }
                    else if (baseItem.Item is MerchantProductMemberType)
                    {
                        MerchantProductMemberType item = baseItem.Item as MerchantProductMemberType;
                        if (BaseDatabase.getDB().Table<MerchantProductMemberType>().Where(p => p.idMerchantProductMemberType == item.idMerchantProductMemberType).Count() > 0)
                        {
                            action = MasterThreads.Action.Update;
                        }
                        else
                        {
                            action = MasterThreads.Action.AddNew;
                        }
                    }
                    else if (baseItem.Item is MemberGroup)
                    {
                        MemberGroup item = baseItem.Item as MemberGroup;
                        if (BaseDatabase.getDB().Table<MemberGroup>().Where(p => p.MemberGroupID == item.MemberGroupID).Count() > 0)
                        {
                            action = MasterThreads.Action.Update;
                        }
                        else
                        {
                            action = MasterThreads.Action.AddNew;
                        }
                    }
                    else if (baseItem.Item is MemberGroupDetail)
                    {
                        MemberGroupDetail item = baseItem.Item as MemberGroupDetail;
                        if (BaseDatabase.getDB().Table<MemberGroupDetail>().Where(p => p.MemberGroupDetailID == item.MemberGroupDetailID).Count() > 0)
                        {
                            action = MasterThreads.Action.Update;
                        }
                        else
                        {
                            action = MasterThreads.Action.AddNew;
                        }
                    }
                    else if (baseItem.Item is State)
                    {
                        State item = baseItem.Item as State;
                        if (BaseDatabase.getDB().Table<State>().Where(p => p.RegionID == item.RegionID).Count() > 0)
                        {
                            action = MasterThreads.Action.Update;
                        }
                        else
                        {
                            action = MasterThreads.Action.AddNew;
                        }
                    }
                    else if (baseItem.Item is Country)
                    {
                        Country item = baseItem.Item as Country;
                        if (BaseDatabase.getDB().Table<Country>().Where(p => p.RegionID == item.RegionID).Count() > 0)
                        {
                            action = MasterThreads.Action.Update;
                        }
                        else
                        {
                            action = MasterThreads.Action.AddNew;
                        }
                    }
                    if (baseItem.Item is MerchantProduct)
                    {
                        var product = baseItem.Item as MerchantProduct;
                        if (product.MainImageID != null)
                        {
                            var imgMain = BaseDatabase.getDB().Table<Document>().Where(p => p.ID == product.MainImageID).FirstOrDefault();
                            if (imgMain == null)
                            {
                                try
                                {
                                    string filename = "";// product.ProductName + "_" + StringUtils.RandomString(3) + ".png";
                                    do
                                    {
                                        filename = EXPRESSO.Utils.StringUtils.RandomString(10);
                                    }
                                    while (BaseDatabase.getDB().Table<Document>().Where(p => p.FileName == filename).Count() > 0);
                                    var url = new Uri(Cons.API_IMG_URL + product.MainImageID);
                              

                                    imgMain = new Document();
                                    imgMain.ID = product.MainImageID.Value;
                                    imgMain.FileName = Cons.API_IMG_URL + product.MainImageID;
                                    BaseDatabase.getDB().Insert(imgMain);
                                }
                                catch (System.Exception ex)
                                {

                                }
                            }

                            if (product.ThumbnailImageID != null)
                            {
                                var imgThum = BaseDatabase.getDB().Table<Document>().Where(p => p.ID == product.ThumbnailImageID).FirstOrDefault();
                                if (imgThum == null)
                                {
                                    try
                                    {
                                        string filename = "";
                                        do
                                        {
                                            filename = StringUtils.RandomString(15);
                                        }
                                        while (BaseDatabase.getDB().Table<Document>().Where(p => p.FileName == filename).Count() > 0);
                                        var url = new Uri(Cons.API_IMG_URL + product.ThumbnailImageID);
                                  
                                        imgThum = new Document();
                                        imgThum.ID = product.ThumbnailImageID.Value;
                                        imgThum.FileName = Cons.API_IMG_URL + product.ThumbnailImageID;
                                        BaseDatabase.getDB().Insert(imgThum);
                                    }
                                    catch (System.Exception ex)
                                    {

                                    }
                                }
                            }
                        }

                    }
                    else if (baseItem.Item is RedemptionProduct)
                    {
                        var product = baseItem.Item as RedemptionProduct;
                        if (product.ImageID != null)
                        {
                            var imgMain = BaseDatabase.getDB().Table<Document>().Where(p => p.ID == product.ImageID).FirstOrDefault();
                            if (imgMain == null)
                            {
                                try
                                {
                                    string filename = "";// product.ProductName + "_" + StringUtils.RandomString(3) + ".png";
                                    do
                                    {
                                        filename = StringUtils.RandomString(10);
                                    }
                                    while (BaseDatabase.getDB().Table<Document>().Where(p => p.FileName == filename).Count() > 0);
                                    var url = new Uri(Cons.API_IMG_URL + product.ImageID);
                                 
                                    imgMain = new Document();
                                    imgMain.ID = product.ImageID.Value;
                                    imgMain.FileName = Cons.API_IMG_URL + product.ImageID;
                                    BaseDatabase.getDB().Insert(imgMain);
                                }
                                catch (System.Exception ex)
                                {

                                }
                            }
                        }
                    }
                    else if (baseItem.Item is MemberGroup)
                    {
                        var membergroup = baseItem.Item as MemberGroup;
                        if (membergroup.idDocument != null)
                        {
                            var imgMain = BaseDatabase.getDB().Table<Document>().Where(p => p.ID == membergroup.idDocument).FirstOrDefault();
                            if (imgMain == null)
                            {
                                try
                                {
                                    string filename = "";// product.ProductName + "_" + StringUtils.RandomString(3) + ".png";
                                    do
                                    {
                                        filename = StringUtils.RandomString(10);
                                    }
                                    while (BaseDatabase.getDB().Table<Document>().Where(p => p.FileName == filename).Count() > 0);
                                    var url = new Uri(Cons.API_IMG_URL + membergroup.idDocument);
                                    imgMain = new Document();
                                    imgMain.ID = membergroup.idDocument.Value;
                                    imgMain.FileName = Cons.API_IMG_URL + membergroup.idDocument;
                                    BaseDatabase.getDB().Insert(imgMain);
                                }
                                catch (System.Exception ex)
                                {

                                }
                            }
                        }
                    }
                    try
                    {
                        lstActions.Add(action);
                        //await Task.Run(() => LocalData.updateData(baseItem,action));
                        if (lstCache.Count > 500 || i == lstItem.Count - 1)
                        {
                            LocalData.updateData(lstCache, lstActions);
                            lstCache.Clear();
                            lstActions.Clear();

                            //Change

                            builderloyalty.SetProgress(lstItem.Count, i, false);
                            notificationManager.Notify(notifyIDLoyalty, builderloyalty.Build());
                        }
                    }
                    catch (System.Exception ex)
                    {
                        LogUtils.WriteError("updateData", baseItem.Item.ToString() + ":" + ex.Message);
                    }

                }

                //Finish
                setCacheString(SYNCEDATA_LOYALTY, "");
                setLastUpdateLoyalty(dtStart);

                builderloyalty.SetProgress(lstItem.Count, lstItem.Count, false);
                builderloyalty.SetContentText(GetString(Resource.String.mess_update_finish));
                notificationManager.Notify(notifyIDLoyalty, builderloyalty.Build());
            }
            catch (System.Exception ex)
            {
                LogUtils.WriteError("Update", ex.Message);
            }
        }

    }
}