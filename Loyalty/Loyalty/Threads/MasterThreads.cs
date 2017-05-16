using Loyalty.Models;
using Loyalty.Models.Database;
using Loyalty.Models.ServiceOutput;
using Loyalty.Models.ServiceOutput.Database;
using Loyalty.Processing.Connections;
using Loyalty.Processing.LocalData;
using Loyalty.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Threads
{
    public class MasterThreads
    {
        public delegate void onResult(ServiceResult result);
        public onResult OnResult;

        public delegate void onDataChange(string strTable, Action action, int intIndex, int count);
        public onDataChange OnDataChange;

        public delegate void onUpdateComplate();
        public onUpdateComplate OnUpdateComplate;
        private bool isCancel = false;


        public void Cancel()
        {
            isCancel = true;
        }

        public enum Action
        {
            AddNew = 1,
            Update = 2,
            Delete = 3
        }

        public async void GetData(DateTime lastGet, M_BBGetDataDeletedID delete)
        {
            try
            {
                var result = await Task.Run(() => MasterData.GetData(lastGet, delete));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.Mess = result.ResponseMessage;
                sResult.StatusCode = result.StatusCode;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }

            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetData", ex.Message);
            }
        }


        public async void UpdateData(List<BaseItem> lstItem,int start = 0)
        {
            try
            {
                List<BaseItem> lstCache = new List<BaseItem>();
                List<Action> lstActions = new List<Action>();
                for (int i = start; i < lstItem.Count; i++)
                {
                    if (isCancel)
                    {
                        return;
                    }
                    
                    Action action = Action.Delete;
                    var baseItem = lstItem[i];
                    lstCache.Add(baseItem);

                  

                    
                   string jsonData = JsonConvert.SerializeObject(baseItem.Item);
                   LogUtils.WriteLog("UpdateData", i + ":" + jsonData);
                   if (baseItem.Item is MemberType)
                   {
                       MemberType item = baseItem.Item as MemberType;
                       if (BaseDatabase.getDB().Table<MemberType>().Where(p => p.MemberTypeID == item.MemberTypeID).Count() > 0)
                       {
                           OnDataChange("MemberType", Action.Update, i, lstItem.Count);
                           action = Action.Update;
                       }
                       else
                       {
                           OnDataChange("MemberType", Action.AddNew, i, lstItem.Count);
                           action = Action.AddNew;
                       }
                   }
                   else if (baseItem.Item is Models.Database.Merchant)
                   {
                       Models.Database.Merchant item = baseItem.Item as Models.Database.Merchant;
                       if (BaseDatabase.getDB().Table<Models.Database.Merchant>().Where(p => p.MerchantID == item.MerchantID).Count() > 0)
                       {
                           OnDataChange("Merchant", Action.Update, i, lstItem.Count);
                           action = Action.Update;
                       }
                       else
                       {
                           OnDataChange("Merchant", Action.AddNew, i, lstItem.Count);
                           action = Action.AddNew;
                       }
                   }
                   else if (baseItem.Item is Models.Database.MerchantCategory)
                   {
                       Models.Database.MerchantCategory item = baseItem.Item as Models.Database.MerchantCategory;
                       if (BaseDatabase.getDB().Table<Models.Database.MerchantCategory>().Where(p => p.MerchantCategoryID == item.MerchantCategoryID).Count() > 0)
                       {
                           OnDataChange("MerchantCategory", Action.Update, i, lstItem.Count);
                           action = Action.Update;
                       }
                       else
                       {
                           OnDataChange("MerchantCategory", Action.AddNew, i, lstItem.Count);
                           action = Action.AddNew;
                       }
                   }
                   else if (baseItem.Item is Models.Database.MerchantProduct)
                   {
                       Models.Database.MerchantProduct item = baseItem.Item as Models.Database.MerchantProduct;
                       if (BaseDatabase.getDB().Table<Models.Database.MerchantProduct>().Where(p => p.MerchantProductID == item.MerchantProductID).Count() > 0)
                       {
                           OnDataChange("MerchantProduct", Action.Update, i, lstItem.Count);
                           action = Action.Update;
                       }
                       else
                       {
                           OnDataChange("MerchantProduct", Action.AddNew, i, lstItem.Count);
                           action = Action.AddNew;
                       }
                   }
                   else if (baseItem.Item is Models.Database.RedemptionCategory)
                   {
                       Models.Database.RedemptionCategory item = baseItem.Item as Models.Database.RedemptionCategory;
                       if (BaseDatabase.getDB().Table<Models.Database.RedemptionCategory>().Where(p => p.RedemptionCategoryID == item.RedemptionCategoryID).Count() > 0)
                       {
                           OnDataChange("RedemptionCategory", Action.Update, i, lstItem.Count);
                           action = Action.Update;
                       }
                       else
                       {
                           OnDataChange("RedemptionCategory", Action.AddNew, i, lstItem.Count);
                           action = Action.AddNew;
                       }
                   }
                   else if (baseItem.Item is Models.Database.RedemptionProduct)
                   {
                       Models.Database.RedemptionProduct item = baseItem.Item as Models.Database.RedemptionProduct;
                       if (BaseDatabase.getDB().Table<Models.Database.RedemptionProduct>().Where(p => p.RedemptionProductID == item.RedemptionProductID).Count() > 0)
                       {
                           OnDataChange("RedemptionProduct", Action.Update, i, lstItem.Count);
                           action = Action.Update;
                       }
                       else
                       {
                           OnDataChange("RedemptionProduct", Action.AddNew, i, lstItem.Count);
                           action = Action.AddNew;
                       }
                   }
                   else if (baseItem.Item is Models.Database.RedemptionProductDetail)
                   {
                       Models.Database.RedemptionProductDetail item = baseItem.Item as Models.Database.RedemptionProductDetail;
                       if (BaseDatabase.getDB().Table<Models.Database.RedemptionProductDetail>().Where(p => p.RedemptionProductDetailID == item.RedemptionProductDetailID).Count() > 0)
                       {
                           OnDataChange("RedemptionProductDetail", Action.Update, i, lstItem.Count);
                           action = Action.Update;
                       }
                       else
                       {
                           OnDataChange("RedemptionProductDetail", Action.AddNew, i, lstItem.Count);
                           action = Action.AddNew;
                       }
                   }
                   else if (baseItem.Item is Models.Database.RedemptionPartner)
                   {
                       Models.Database.RedemptionPartner item = baseItem.Item as Models.Database.RedemptionPartner;
                       if (BaseDatabase.getDB().Table<Models.Database.RedemptionPartner>().Where(p => p.PartnerID == item.PartnerID).Count() > 0)
                       {
                           OnDataChange("RedemptionPartner", Action.Update, i, lstItem.Count);
                           action = Action.Update;
                       }
                       else
                       {
                           OnDataChange("RedemptionPartner", Action.AddNew, i, lstItem.Count);
                           action = Action.AddNew;
                       }
                   }
                   else if (baseItem.Item is Models.Database.MerchantLocation)
                   {
                       Models.Database.MerchantLocation item = baseItem.Item as Models.Database.MerchantLocation;
                       if (BaseDatabase.getDB().Table<Models.Database.MerchantLocation>().Where(p => p.idMerchantLocation == item.idMerchantLocation).Count() > 0)
                       {
                           OnDataChange("MerchantLocation", Action.Update, i, lstItem.Count);
                           action = Action.Update;
                       }
                       else
                       {
                           OnDataChange("MerchantLocation", Action.AddNew, i, lstItem.Count);
                           action = Action.AddNew;
                       }
                   }
                   else if (baseItem.Item is Models.Database.MerchantProductMemberType)
                   {
                       Models.Database.MerchantProductMemberType item = baseItem.Item as Models.Database.MerchantProductMemberType;
                       if (BaseDatabase.getDB().Table<Models.Database.MerchantProductMemberType>().Where(p => p.idMerchantProductMemberType == item.idMerchantProductMemberType).Count() > 0)
                       {
                           OnDataChange("MerchantProductMemberType", Action.Update, i, lstItem.Count);
                           action = Action.Update;
                       }
                       else
                       {
                           OnDataChange("MerchantProductMemberType", Action.AddNew, i, lstItem.Count);
                           action = Action.AddNew;
                       }
                   }
                   else if (baseItem.Item is Models.Database.MemberGroup)
                   {
                       Models.Database.MemberGroup item = baseItem.Item as Models.Database.MemberGroup;
                       if (BaseDatabase.getDB().Table<Models.Database.MemberGroup>().Where(p => p.MemberGroupID == item.MemberGroupID).Count() > 0)
                       {
                           OnDataChange("MemberGroup", Action.Update, i, lstItem.Count);
                           action = Action.Update;
                       }
                       else
                       {
                           OnDataChange("MemberGroup", Action.AddNew, i, lstItem.Count);
                           action = Action.AddNew;
                       }
                   }
                   else if (baseItem.Item is Models.Database.MemberGroupDetail)
                   {
                       Models.Database.MemberGroupDetail item = baseItem.Item as Models.Database.MemberGroupDetail;
                       if (BaseDatabase.getDB().Table<Models.Database.MemberGroupDetail>().Where(p => p.MemberGroupDetailID == item.MemberGroupDetailID).Count() > 0)
                       {
                           OnDataChange("MemberGroupDetail", Action.Update, i, lstItem.Count);
                           action = Action.Update;
                       }
                       else
                       {
                           OnDataChange("MemberGroupDetail", Action.AddNew, i, lstItem.Count);
                           action = Action.AddNew;
                       }
                   }
                   else if (baseItem.Item is Models.Database.State)
                   {
                       Models.Database.State item = baseItem.Item as Models.Database.State;
                       if (BaseDatabase.getDB().Table<Models.Database.State>().Where(p => p.RegionID == item.RegionID).Count() > 0)
                       {
                           OnDataChange("State", Action.Update, i, lstItem.Count);
                           action = Action.Update;
                       }
                       else
                       {
                           OnDataChange("State", Action.AddNew, i, lstItem.Count);
                           action = Action.AddNew;
                       }
                   }
                   else if (baseItem.Item is Models.Database.Country)
                   {
                       Models.Database.Country item = baseItem.Item as Models.Database.Country;
                       if (BaseDatabase.getDB().Table<Models.Database.Country>().Where(p => p.RegionID == item.RegionID).Count() > 0)
                       {
                           OnDataChange("Country", Action.Update, i, lstItem.Count);
                           action = Action.Update;
                       }
                       else
                       {
                           OnDataChange("Country", Action.AddNew, i, lstItem.Count);
                           action = Action.AddNew;
                       }
                   }
                   try
                   {
                       #region [Download image]

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
                                           filename = StringUtils.RandomString(10);
                                       }
                                       while (BaseDatabase.getDB().Table<Document>().Where(p => p.FileName == filename).Count() > 0);
                                       var url = new Uri(Cons.API_IMG_URL + product.MainImageID);
                                       //var httpClient = new HttpClient();
                                       //var data = await httpClient.GetByteArrayAsync(url).ConfigureAwait(true);

                                       //var file = await FileSystem.Current.LocalStorage.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                                       //using (System.IO.Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
                                       //{
                                       //    stream.Write(data, 0, data.Length);
                                       //}

                                       imgMain = new Document();
                                       imgMain.ID = product.MainImageID.Value;
                                       imgMain.FileName = Cons.API_IMG_URL + product.MainImageID;
                                       BaseDatabase.getDB().Insert(imgMain);
                                   }
                                   catch (Exception ex)
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
                                           //var httpClient = new HttpClient();
                                           //var data = await httpClient.GetByteArrayAsync(url).ConfigureAwait(true);

                                           //var file = await FileSystem.Current.LocalStorage.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                                           //using (System.IO.Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
                                           //{
                                           //    stream.Write(data, 0, data.Length);
                                           //}

                                           imgThum = new Document();
                                           imgThum.ID = product.ThumbnailImageID.Value;
                                           imgThum.FileName = Cons.API_IMG_URL + product.ThumbnailImageID;
                                           BaseDatabase.getDB().Insert(imgThum);
                                       }
                                       catch (Exception ex)
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
                                       //var httpClient = new HttpClient();
                                       //var data = await httpClient.GetByteArrayAsync(url).ConfigureAwait(true);
                                       //var file = await FileSystem.Current.LocalStorage.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                                       //using (System.IO.Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
                                       //{
                                       //    stream.Write(data, 0, data.Length);
                                       //}

                                       imgMain = new Document();
                                       imgMain.ID = product.ImageID.Value;
                                       imgMain.FileName = Cons.API_IMG_URL + product.ImageID;
                                       BaseDatabase.getDB().Insert(imgMain);
                                   }
                                   catch (Exception ex)
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
                                       //var httpClient = new HttpClient();
                                       //var data = await httpClient.GetByteArrayAsync(url).ConfigureAwait(true);
                                       //var file = await FileSystem.Current.LocalStorage.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                                       //using (System.IO.Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
                                       //{
                                       //    stream.Write(data, 0, data.Length);
                                       //}

                                       imgMain = new Document();
                                       imgMain.ID = membergroup.idDocument.Value;
                                       imgMain.FileName = Cons.API_IMG_URL + membergroup.idDocument;
                                       BaseDatabase.getDB().Insert(imgMain);
                                   }
                                   catch (Exception ex)
                                   {

                                   }
                               }
                           }
                       }
                        #endregion
                        lstActions.Add(action);
                        //await Task.Run(() => LocalData.updateData(baseItem,action));
                        if (lstCache.Count > 500 || i == lstItem.Count - 1)
                        {
                            await Task.Run(() => LocalData.updateData(lstCache, lstActions));
                            lstCache.Clear();
                            lstActions.Clear();
                        }
                    }
                   catch (Exception ex)
                   {
                       LogUtils.WriteError("updateData", baseItem.Item.ToString() + ":" + ex.Message);
                   }

                }
                if (OnUpdateComplate != null)
                {
                    OnUpdateComplate();
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Update", ex.Message);
            }


        }


        public async void ChoosAddressCart()
        {
            try
            {
                var result = await Task.Run(() => LocalData.GetState());
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }

                var result1 = await Task.Run(() => LocalData.GetCountry());
                ServiceResult sResult1 = new ServiceResult();
                sResult1.Data = result1;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sResult1);
                }

                var result2 = await Task.Run(() => UserAccount.GetMemberProfile(Cons.mMemberCredentials.LoginParams.authSessionId, Cons.mMemberCredentials.LoginParams.strLoginId));
                ServiceResult sResult2 = new ServiceResult();
                sResult2.Data = result2;
                if (OnResult != null)
                {
                    OnResult(sResult2);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetCountryAndState", ex.Message);
            }
        }
    }
}
