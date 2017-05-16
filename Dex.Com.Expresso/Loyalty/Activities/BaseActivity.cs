
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Loyalty.Utils;
using System.IO;
using Loyalty.Models.ServiceOutput.Database;

using Newtonsoft.Json;
using Loyalty.Models;
using Loyalty.Models.ServiceOutput;
using Dex.Com.Expresso;
using Dex.Com.Expresso.Loyalty.Droid.Dialogs;
using Loyalty.Threads;

namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    public abstract class BaseActivity : AppCompatActivity
    {
        public static string DBNAME_Loyalty = "loyalty.db";
        public static string IsFistTime = "FistTime";
        public static string MyAuth = "MyAuth";
        public static string LastUpdate = "LASTUPDATE";
        public static string SettingMemberType = "SettingMemberType";
        public bool IsHomePage = false;
        public ISharedPreferences mSharedPref;

        public static DateTime FirstUpdate = new DateTime(1990, 11, 2);
        #region [Cache]
        public string getCacheString(string key)
        {
            return mSharedPref.GetString(key, "");
        }

        public List<ItemPreferences> getSettingMemberType()
        {
            try
            {
                string data = getCacheString(SettingMemberType);
                var result = JsonConvert.DeserializeObject<List<ItemPreferences>>(data);
                result = result == null ? new List<ItemPreferences>() : result;
                return result;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getSettingMemberType", ex.Message);
                return new List<ItemPreferences>();
            }
        }

        public void setSettingMemberType(List<ItemPreferences> data)
        {
            try
            {
                string strData = JsonConvert.SerializeObject(data);
                setCacheString(SettingMemberType, strData);
                Cons.Preferences = data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("setSettingMemberType", ex.Message);
            }
        }

        public DateTime getLastUpdate()
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

        public void setLastUpdate(DateTime dtStart)
        {
            //string mycache = dtStart.ToString("yyyy-MM-dd HH:mm:ss ");
            ////string mycache = (new DateTime(2016,07,01).ToString("yyyy-MM-dd HH:mm:ss"));
            //setCacheString(LastUpdate, mycache);
            //LogUtils.WriteLog("setLastUpdate", mycache);
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

        public Toolbar Toolbar
        {
            get;
            set;
        }

        public void LogOut()
        {
            setCacheString(MyAuth, "");
            Cons.mMemberCredentials = null;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            copyDataBase();
            SetContentView(LayoutResource);
            Toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            if (Toolbar != null)
            {
                SetSupportActionBar(Toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);

                if (!IsHomePage)
                {
                    this.Toolbar.NavigationClick += Toolbar_NavigationClick;
                }
            }

            mSharedPref = this.GetSharedPreferences("loyalty", FileCreationMode.MultiProcess);

            string jsonData = getCacheString(MyAuth);
            if (!string.IsNullOrEmpty(jsonData))
            {
                Cons.mMemberCredentials = JsonConvert.DeserializeObject<MValidateMemberCredentials>(jsonData);
            }

            Cons.Preferences = getSettingMemberType();
        }

        private void Toolbar_NavigationClick(object sender, Toolbar.NavigationClickEventArgs e)
        {
            OnBackPressed();
        }

        protected abstract int LayoutResource
        {
            get;
        }

        protected int ActionBarIcon
        {
            set { Toolbar.SetNavigationIcon(value); }
        }

        private void copyDataBase()
        {
            string path = "";
            using (var assets = Assets.Open(DBNAME_Loyalty))
            {
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
                path = Path.Combine(documentsPath, DBNAME_Loyalty);
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

        protected override void OnResume()
        {
            base.OnResume();
            bool isNoticeUpdate = false;
            bool isFirst = getCacheInt(IsFistTime) == int.MinValue;
            setCacheInt(IsFistTime, int.MaxValue);
            //return;
            /*
            if (getLastUpdate() == FirstUpdate && !isFirst)
            {

                //Not init before
                FragmentTransaction ft = FragmentManager.BeginTransaction();
                Fragment prev = FragmentManager.FindFragmentByTag("update");
                if (prev != null)
                {
                    // UpdateDialog newFragment = (UpdateDialog)prev;
                    //newFragment.Show(ft, "update");
                    //ft.Remove(prev);
                }
                else
                {
                    ft.AddToBackStack(null);
                    UpdateDialog newFragment = UpdateDialog.NewInstance(null);
                    newFragment.EventOnDismiss += () =>
                    {
                        //LoadSearchData();
                        //ListItemClicked(0, true);
                    };
                    newFragment.Show(ft, "update");

                }
            }
            else
            {
                if (isNoticeUpdate)
                    return;
                isNoticeUpdate = true;
                //Check update data
                MasterThreads thread = new MasterThreads();
                thread.OnResult += (ServiceResult result) =>
                {
                    if (result.StatusCode == 1)
                    {
                        var data = result.Data as MBB_GetData;
                        bool isHaveData = false;
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
                            FragmentTransaction ft = FragmentManager.BeginTransaction();
                            Fragment prev = FragmentManager.FindFragmentByTag("update");
                            if (prev != null)
                            {
                                ft.Remove(prev);
                            }
                            ft.AddToBackStack(null);
                            UpdateDialog newFragment = UpdateDialog.NewInstance(null, lstItem, data.LastGet);
                            newFragment.EventOnDismiss += () =>
                            {

                                // ListItemClicked(Resource.Id.nav_pointsofinterest);
                                //LoadSearchData();
                                //ListItemClicked(0, true);
                            };
                            newFragment.Show(ft, "update");
                        }
                    }
                };
                //mdtStartUpdate = DateTime.Now;
                thread.GetData(getLastUpdate(), new M_BBGetDataDeletedID());*/

        }
    }

}

