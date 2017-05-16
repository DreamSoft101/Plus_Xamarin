using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using EXPRESSO.Processing.Connections;
using EXPRESSO.Processing.LocalData;
using EXPRESSO.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Threads
{
    public class UpdateThread
    {
        public delegate void onGetNewData(string data);
        public onGetNewData OnGetNewData;



        public delegate void onDataChange(string strTable, Action action, int intIndex, int intCount);
        public onDataChange OnDataChange;

        public delegate void onUpdateComplate();
        public onUpdateComplate OnUpdateComplate;

        public delegate void onGenerateData(ServiceResult result);
        public onGenerateData OnGenerateData;
        public enum Action
        {
            AddNew = 1,
            Update = 2,
            Delete = 3,
            Unknown = 4,
        }

        public async void generateData(string result)
        {
            try
            {
                var data = await Task.Run(() => UpdateConnections.GenerateData(result));

                if (OnGenerateData != null)
                {
                    try
                    {
                        OnGenerateData(data);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        public async void getNewData(DateTime lastUpdate)
        {
            try
            {

                var result = await Task.Run(() => UpdateConnections.getUpdateData( lastUpdate.ToString("yyyy-MM-dd HH:mm:ss")));


                if (OnGetNewData != null)
                {
                    try
                    {
                        OnGetNewData(result);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                

            }
            catch (Exception ex)
            {
                LogUtils.WriteError("getNewData", ex.Message);
            }
        }

        private bool isCancel = false;
        public void Cancel()
        {
            isCancel = true;
        }
        public async void UpdateData(List<BaseItem> lstItem, int start = 0)
        {
            try
            {
                List<BaseItem> lstCache = new List<BaseItem>();
                for (int i = start; i < lstItem.Count; i++)
                {
                    if (isCancel)
                    {
                        return;
                    }
                    var baseItem = lstItem[i];

                    lstCache.Add(baseItem);


                    try
                    {
                        if (lstCache.Count > 500 || i == lstItem.Count - 1)
                        {
                           
                            await Task.Run(() => LoadData.updateData(lstCache));
                            lstCache.Clear();
                            if (OnDataChange != null)
                            {
                                OnDataChange("", Action.Unknown, i, lstItem.Count);
                            }
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
    }
}
