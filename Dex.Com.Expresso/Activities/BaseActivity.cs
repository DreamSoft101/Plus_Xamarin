
using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using EXPRESSO.Models.Database;
using System.IO;
using EXPRESSO.Utils;
using EXPRESSO.Models;
using Newtonsoft.Json;

namespace Dex.Com.Expresso.Activities
{
    public abstract class BaseActivity : Loyalty.Droid.Activities.BaseActivity
    {
        public static string DBNAME = "expresso.db";
        public static string MYENTITY = "myentity";
        public static string CURRENT_MYENTITY = "current_myentity";
        public static string ENTITYSETTINGPREFIX = "SETTINGENTITY_";
        public static string ENTITYLASTUPDATEPREFIX = "LASTUPDATE_";
        public static string FAVORITE_TRAFFICTYPE = "TRAFFICTYPE";
        public static string FAVORITE_LOCATION = "LOCATION";

        public static string FAVORITE_FACILITIESTYPE = "FAVORITE_FACILITIESTYPE";
        public static string FAVORITE_POITYPE = "FAVORITE_POITYPE";
        public static string FAVORITE_PLUSRABGERCATEGORY = "FAVORITE_PLUSRANGER_CATEGORY";

        public ISharedPreferences mSharedPref;

        //Loyalty



        public bool ShowMenuSave = false;
        public bool ShowMenuEdit = false;
        public bool ShowMenuDelete = false;
        public bool ShowMenuInfo = false;

        public static string SETTING_NOTIFICATION = "NOTIFICATION";

        public List<SettingNotification> getSettingNotification()
        {
            string mycache = getCacheString(ENTITYSETTINGPREFIX + Cons.IdEntity + "_" + SETTING_NOTIFICATION);
            var items = StringUtils.String2Object<List<SettingNotification>>(mycache);
            if (items == null)
            {
                items = new List<SettingNotification>();
            }
            return items;
        }

        public void setSettingNotification(List<SettingNotification> items)
        {
            string json = StringUtils.Object2String(items);
            setCacheString(ENTITYSETTINGPREFIX + Cons.IdEntity + "_" + SETTING_NOTIFICATION, json);
        }

        public List<MyEntity> getMyEntity()
        {
            string mycache = getCacheString(MYENTITY);
            List<MyEntity> result = StringUtils.String2Object<List<MyEntity>>(mycache);
            if (result == null)
                result = new List<MyEntity>();
            return result;
        }


        public DateTime getLastUpdate()
        {
            string mycache = getCacheString(ENTITYLASTUPDATEPREFIX + Cons.IdEntity);
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
            //string mycache = dtStart.ToString("yyyy-MM-dd HH:mm:ss");
            ////string mycache = (new DateTime(2016,07,01).ToString("yyyy-MM-dd HH:mm:ss"));
            //setCacheString(ENTITYLASTUPDATEPREFIX + Cons.IdEntity, mycache);
            //LogUtils.WriteLog("setLastUpdate", mycache);
        }

        public void saveMyEntity(List<MyEntity> lstEntity)
        {
            string json = StringUtils.Object2String(lstEntity);
            setCacheString(MYENTITY, json);
        }

        public void saveCurrentMyEntity(MyEntity myEntity)
        {
            string json = StringUtils.Object2String(myEntity);
            Cons.IdEntity = myEntity.idEntity;
            Cons.myEntity = myEntity;
            setCacheString(CURRENT_MYENTITY, json);
        }

        public MyEntity getCurrentMyEntity()
        {
            string mycache = getCacheString(CURRENT_MYENTITY);
            return StringUtils.String2Object<MyEntity>(mycache);
        }

        public List<FavoriteLocation> getFavoriteLocation()
        {
            string mycache = getCacheString(ENTITYSETTINGPREFIX + FAVORITE_LOCATION);
            List<FavoriteLocation> result = StringUtils.String2Object<List<FavoriteLocation>>(mycache);
            if (result == null)
                result = new List<FavoriteLocation>();
            return result;
        }

        public void setFavoriteLocation(List<FavoriteLocation> lstItemm)
        {
            string mydata = JsonConvert.SerializeObject(lstItemm);
            setCacheString(ENTITYSETTINGPREFIX + FAVORITE_LOCATION, mydata);
        }

        public void setFavoriteFacilitiesType(List<FacilitiesSetting> lstItemm)
        {
            string mydata = JsonConvert.SerializeObject(lstItemm);
            setCacheString(ENTITYSETTINGPREFIX + FAVORITE_FACILITIESTYPE, mydata);
        }

        public List<POITypeSetting> getAllPOIType()
        {
            if (Cons.myEntity != null)
            {
                if (Cons.myEntity.mSettings != null)
                {
                    Settings set = Cons.myEntity.mSettings;
                    POITypeSetting rsa = new POITypeSetting() { strName = string.IsNullOrEmpty(set.rsa_label) ? GetString(Resource.String.facilities_type_rsa) : set.rsa_label, intType = 0, intDataPOIType = 1 , strURL = set.rsa_icon } ;
                    POITypeSetting tollplaza = new POITypeSetting() { strName = string.IsNullOrEmpty(set.toll_plaza_label) ? GetString(Resource.String.facilities_type_tollplaza) : set.toll_plaza_label, intType = 3, intDataPOIType = 1, strURL = set.toll_plaza_icon };
                    POITypeSetting pertrol = new POITypeSetting() { strName = string.IsNullOrEmpty(set.petrol_station_label) ? GetString(Resource.String.facilities_type_pertrol) : set.petrol_station_label, intType = 1, intDataPOIType = 1, strURL = set.petrol_station_icon };
                    POITypeSetting csc = new POITypeSetting() { strName = string.IsNullOrEmpty(set.csc_label) ? GetString(Resource.String.facilities_type_csc) : set.csc_label, intType = 2, intDataPOIType = 1 , strURL = set.csc_icon };
                    POITypeSetting tunnel = new POITypeSetting() { strName = string.IsNullOrEmpty(set.csc_label) ? GetString(Resource.String.facilities_type_tunnel) : set.tunnel_label, intType = 0, intDataPOIType = 1, intRSAType = 5 , strURL= set.tunnel_icon};
                    POITypeSetting vistapoint = new POITypeSetting() { strName = string.IsNullOrEmpty(set.csc_label) ? GetString(Resource.String.facilities_type_visapoint) : set.vista_point_label, intType = 0, intDataPOIType = 1, intRSAType = 6, strURL = set.vista_point_icon };
                    POITypeSetting interchange = new POITypeSetting() { strName = string.IsNullOrEmpty(set.interchange_label) ? GetString(Resource.String.facilities_type_interchange) : set.interchange_label, intType = 0, intDataPOIType = 1, intRSAType = 4, strURL = set.interchange_icon };

                    //POITypeSetting police = new POITypeSetting() { strName = string.IsNullOrEmpty(set.poli) ? GetString(Resource.String.appbar_scrolling_view_behavior) : set.rsa_label, intType = 0, intDataPOIType = 1, intRSAType = 6 };


                    List<POITypeSetting> lstResult = new List<POITypeSetting>();



                    lstResult.Add(rsa);
                    lstResult.Add(tollplaza);
                    lstResult.Add(pertrol);
                    lstResult.Add(csc);
                    lstResult.Add(tunnel);
                    lstResult.Add(vistapoint);
                    lstResult.Add(interchange);

                    foreach (var item in lstResult)
                    {
                        item.intID = item.intType.ToString() + item.intRSAType.ToString() + item.intType.ToString() + item.intDataPOIType.ToString();
                    }

                    return lstResult;
                }
            }
            return null;
        }

        public List<FacilitiesSetting> getFacilitiesType()
        {
            string mycache = getCacheString(ENTITYSETTINGPREFIX + FAVORITE_FACILITIESTYPE);
            List<FacilitiesSetting> result = StringUtils.String2Object<List<FacilitiesSetting>>(mycache);
            if (result == null)
                result = new List<FacilitiesSetting>();
            return result;
        }

        public List<POITypeSetting> getFavoritePOIType()
        {
            string mycache = getCacheString(ENTITYSETTINGPREFIX + FAVORITE_POITYPE);
            List<POITypeSetting> result = StringUtils.String2Object<List<POITypeSetting>>(mycache);
            if (result == null)
                result = new List<POITypeSetting>();
            return result;
        }
        public void setFavoritePOIType(List<POITypeSetting> lstItemm)
        {
            string mydata = JsonConvert.SerializeObject(lstItemm);
            setCacheString(ENTITYSETTINGPREFIX + FAVORITE_POITYPE, mydata);
        }

        public void savePlusRangerCategory(List<TblCategory> lstItem)
        {
            string mydata = JsonConvert.SerializeObject(lstItem);
            setCacheString(ENTITYSETTINGPREFIX + FAVORITE_PLUSRABGERCATEGORY, mydata);
        }
        public List<TblCategory> getPlusRangerCategory()
        {
            string mycache = getCacheString(ENTITYSETTINGPREFIX + FAVORITE_PLUSRABGERCATEGORY);
            List<TblCategory> result = StringUtils.String2Object<List<TblCategory>>(mycache);
            if (result == null)
                result = new List<TblCategory>();
            return result;
        }


        public void saveTrafficType(List<int> lstItem)
        {
            string mydata = JsonConvert.SerializeObject(lstItem);
            setCacheString(ENTITYSETTINGPREFIX + FAVORITE_TRAFFICTYPE, mydata);
        }
        public List<int> getTrafficType()
        {
            string mycache = getCacheString(ENTITYSETTINGPREFIX + FAVORITE_TRAFFICTYPE);
            List<int> result = StringUtils.String2Object<List<int>>(mycache);
            if (result == null)
                result = new List<int>();
            return result;
        }

        public List<SettingHighway> getMySetting()
        {
            string mycache = getCacheString(ENTITYSETTINGPREFIX + Cons.IdEntity);
            List<SettingHighway> result = StringUtils.String2Object<List<SettingHighway>>(mycache);
            if (result == null)
                result = new List<SettingHighway>();
            return result;
        }

        public void saveMySetting(string strData)
        {
            setCacheString(ENTITYSETTINGPREFIX + Cons.IdEntity, strData);
        }

        #region [Cache]
        public string getCacheString(string key)
        {
            return mSharedPref.GetString(key, "");
        }

        public void setCacheString(string key, string value)
        {
            ISharedPreferencesEditor editor = mSharedPref.Edit();
            editor.PutString(key, value);
            editor.Apply();
            editor.Commit();
        }

        public int getCacheInt(string key)
        {
            return mSharedPref.GetInt(key, int.MinValue);
        }

        public void setCacheInt(string key, int value)
        {
            ISharedPreferencesEditor editor = mSharedPref.Edit();
            editor.PutInt(key, value);
            editor.Apply();
            editor.Commit();
        }

        public long getCacheLong(string key)
        {
            return mSharedPref.GetLong(key, long.MinValue);
        }

        public void setCacheLong(string key, long value)
        {
            ISharedPreferencesEditor editor = mSharedPref.Edit();
            editor.PutLong(key, value);
            editor.Apply();
            editor.Commit();
        }
        #endregion

        private void copyDataBase()
        {
            string path = "";
            using (var assets = Assets.Open(DBNAME))
            {
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
                path = Path.Combine(documentsPath, DBNAME);
                if (!File.Exists(path))
                {
                    using (var dest = File.Create(path))
                    {
                        assets.CopyTo(dest);
                    }
                }
                BaseDatabase.Init(path, new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid());
            }

        }

        /*
        private void MockupData()
        {
            List<MyEntity> result = new List<MyEntity>();
            MyEntity entity1 = new MyEntity();
            entity1.idEntity = "1";
            entity1.User = new UserInfos();
            entity1.User.strAvatar = "http://www.iconsfind.com/wp-content/uploads/2015/08/20150831_55e46ad551392.png";
            entity1.User.strUserName = "nthoa";
            entity1.Entity = new TblEntities();
            entity1.Entity.idEntity = "1";
            entity1.Entity.strName = "Plussmile 1";
            result.Add(entity1);

            MyEntity entity2 = new MyEntity();
            entity2.idEntity = "2";
            entity2.User = new UserInfos();
            entity2.User.strAvatar = "http://www.iconsfind.com/wp-content/uploads/2015/08/20150831_55e46ad551392.png";
            entity2.User.strUserName = "nthoa";
            entity2.Entity = new TblEntities();
            entity2.Entity.idEntity = "2";
            entity2.Entity.strName = "Plussmile 2";
            result.Add(entity2);

            string data = StringUtils.Object2String(result);


            setCacheString(MYENTITY, data);
        }*/

        public Toolbar Toolbar
        {
            get;
            set;
        }

        public bool isHomePage = false;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            mSharedPref = this.GetSharedPreferences("expresso", FileCreationMode.MultiProcess);
            copyDataBase();

            Cons.IdEntity = Cons.PLUSEntity.ToString();
            SetContentView(LayoutResource);
            Toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            if (Toolbar != null)
            {
                SetSupportActionBar(Toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                if (!isHomePage)
                {
                    this.Toolbar.NavigationClick += Toolbar_NavigationClick;
                }

            }


             //Cons.mMemberCredentials = 
        }



        private void Toolbar_NavigationClick(object sender, Toolbar.NavigationClickEventArgs e)
        {
            OnBackPressed();
        }

       

        protected int ActionBarIcon
        {
            set { Toolbar.SetNavigationIcon(value); }
        }


    }
}

