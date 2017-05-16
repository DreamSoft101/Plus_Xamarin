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
using static EXPRESSO.Models.EnumType;

namespace EXPRESSO.Threads
{
    public class LiveTrafficThread
    {
        public delegate void onLoadHighwayResult(List<TblHighway> lstHighway);
        public onLoadHighwayResult OnLoadHighwayResult;

        public async void loadHighway()
        {
            try
            {
                var resultHighway = await Task.Run(() => LoadData.getListHighway());
                if (OnLoadHighwayResult != null)
                {
                    OnLoadHighwayResult(resultHighway);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadHighway", ex.Message);
            }
           
        }

        public delegate void onLoadLiveTrafficResult(ServiceResult result);
        public onLoadLiveTrafficResult OnLoadLiveTrafficResult;

        public async void loadLiveTraffic(List<int> types,List<string> idHighway, string idParent)
        {
            try
            {
                var resultHighway = await Task.Run(() => LiveTrafficConnections.getListLiveTrafficUpdate(types, idHighway, idParent));

                

                if (OnLoadLiveTrafficResult != null)
                {
                    OnLoadLiveTrafficResult(resultHighway);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadLiveTraffic", ex.Message);
            }
            
        }
        public async void GetLiveTraffic(List<int> types, List<string> idHighway, string idParent)
        {
            try
            {
                var resultHighway = await Task.Run(() => LiveTrafficConnections.GetLiveTraffic(types, idHighway, idParent));



                if (OnLoadLiveTrafficResult != null)
                {
                    OnLoadLiveTrafficResult(resultHighway);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadLiveTraffic", ex.Message);
            }

        }

        //GetLiveTraffic

        public delegate void onLoadLiveTrafficDetail(ServiceResult result);
        public onLoadLiveTrafficDetail OnLoadLiveTrafficDetail;

        public async void loadLiveTrafficDetail(string id)
        {
            try
            {
                var resultHighway = await Task.Run(() => LiveTrafficConnections.getLiveTrafficDetail(id));

                if (resultHighway.intStatus == 1)
                {
                    var item = resultHighway.Data as TrafficUpdate;
                    if (item.intType == 6 && item.lstWaypoint != null)
                    {
                        item.overview_polyline = await Task.Run(() => LiveTrafficConnections.getDirection(item.lstWaypoint));
                    }
                }
                


                if (OnLoadLiveTrafficDetail != null)
                {
                    OnLoadLiveTrafficDetail(resultHighway);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadLiveTraffic", ex.Message);
            }

        }

        public delegate void onLoadDirection(string polyline);
        public onLoadDirection OnLoadDirection;
        public async void loadDirection(List<Waypoint> waypoints)
        {
            try
            {
                var resultHighway = await Task.Run(() => LiveTrafficConnections.getDirection(waypoints));
                if (OnLoadDirection != null)
                {
                    OnLoadDirection(resultHighway);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadLiveTraffic", ex.Message);
            }

        }

        public delegate void onLoadFacilityCCTV(ServiceResult result);
        public onLoadFacilityCCTV OnLoadFacilityCCTV;

        public async void loadFacilityCCTV(List<string> idHighway, int type)
        {
            try
            {
                var resultHighway = await Task.Run(() => LiveTrafficConnections.getFacilityCCTV(type, idHighway));



                if (OnLoadFacilityCCTV != null)
                {
                    OnLoadFacilityCCTV(resultHighway);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadLiveTraffic", ex.Message);
            }

        }
    }
}
