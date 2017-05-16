using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using EXPRESSO.Processing.Connections;
using EXPRESSO.Processing.LocalData;
using EXPRESSO.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static EXPRESSO.Models.EnumType;

namespace EXPRESSO.Threads
{
    public class MastersThread
    {
        public delegate void onLoadListAnnoucement(ServiceResult result);
        public onLoadListAnnoucement OnLoadListAnnoucement;

        public delegate void onLoadNewDataResult(Object obj);
        public onLoadNewDataResult OnLoadNewDataResult;


        public delegate void onLoadEntitiesResult(List<TblEntities> result);
        public onLoadEntitiesResult OnLoadEntitiesResult;

        public delegate void onLoadHighway(TblHighway result);
        public onLoadHighway OnLoadHighway;

        public delegate void onLoadListHighway(List<TblHighway> result);
        public onLoadListHighway OnLoadListHighway;

        public  async void loadAnnoucement(int page)
        {
            try
            {
                var result = await Task.Run(() => MasterDataConnections.getAnnouncement(page));
                if (OnLoadListAnnoucement != null)
                {
                    OnLoadListAnnoucement(result);

                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadListAnnoucementScroller", ex.Message);
            }
            
        }


        public delegate void onLoadAnnoucement(ServiceResult result);
        public onLoadAnnoucement OnLoadAnnoucement;
        public async void loadAnnoucementDetail(string id)
        {
            try
            {
                var result = await Task.Run(() => MasterDataConnections.getAnnouncementDetail(id));
                if (OnLoadAnnoucement != null)
                {
                    OnLoadAnnoucement(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadListAnnoucementScroller", ex.Message);
            }

        }

        public delegate void onLoadLog(List<TblLog> lst);
        public onLoadLog OnLoadLog;
        public async void loadAllLog()
        {
            try
            {
                var result = LoadData.getAllLog();

               if (OnLoadLog != null)
                {
                    OnLoadLog(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadAllLog", ex.Message);
            }
        }

        public async void clearAllLog()
        {
            try
            {
                DeleteData.clearAllLog();
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("clearAllLog", ex.Message);
            }
        }

    


        public async void loadListHighway()
        {
            try
            {
                var result = await Task.Run(() => Processing.LocalData.LoadData.getListHighway());
                if (OnLoadListHighway != null)
                {
                    OnLoadListHighway(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadListHighway", ex.Message);
            }
           
        }
        public async void loadHighway(string id)
        {
            try
            {
                var result = await Task.Run(() => Processing.LocalData.LoadData.getHighway(id));
                if (OnLoadHighway != null)
                {
                    OnLoadHighway(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadHighway", ex.Message);
            }

        }
    }
}
