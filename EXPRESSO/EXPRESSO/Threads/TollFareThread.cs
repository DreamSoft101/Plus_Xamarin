using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXPRESSO.Processing.LocalData;
using EXPRESSO.Utils;

namespace EXPRESSO.Threads
{
    public class TollFareThread
    {
        public delegate void onLoadClassesResult(List<VehicleClasses> lstClasses, List<TblTollPlaza> lstHighway);
        public onLoadClassesResult OnLoadClassesResult;

        public delegate void onGetInfoResult(string idRound, int intStartSeq, int intEngSeq, decimal decRate, int vclass);
        public onGetInfoResult OnGetInfoResult;

        public delegate void onSearch(List<Journey> mJourneyList);
        public onSearch OnSearch;

        public delegate void onLoadVehicleClasses(List<VehicleClasses> lstClasse);
        public onLoadVehicleClasses OnLoadVehicleClasses;
        public async void loadTollFareHomePage()
        {
            try
            {
                var lstClasses = EXPRESSO.Processing.LocalData.LoadData.getListVehicleClasses();
                var lstTollPlaza = Processing.LocalData.LoadData.getListTollPlaza();
                if (OnLoadClassesResult != null)
                {
                    OnLoadClassesResult(lstClasses, lstTollPlaza);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadTollFareHomePage", ex.Message);
            }
        }

        public delegate void onGetTollPlaza(List<TblTollPlaza> toll);
        public onGetTollPlaza OnGetTollPlaza;
        public async void loadTollPlaza()
        {
            try
            {
                var lstTollPlaza = EXPRESSO.Processing.LocalData.LoadData.getListTollPlaza();
             
                if (OnGetTollPlaza != null)
                {
                    OnGetTollPlaza(lstTollPlaza);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadTollFareHomePage", ex.Message);
            }
        }

        public async void loadVehicleClasses()
        {
            try
            {
                var lstClasses = EXPRESSO.Processing.LocalData.LoadData.getListVehicleClasses();
                if (OnLoadVehicleClasses != null)
                {
                    OnLoadVehicleClasses(lstClasses);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadVehicleClasses", ex.Message);
            }
            
        }
        public async void loadInfo(TblTollPlaza from, TblTollPlaza to, VehicleClasses vclass)
        {
            try
            {
                List<TblRouteDetails> lstFrom = EXPRESSO.Processing.LocalData.LoadData.getListByRoute(from, 3);
                List<TblRouteDetails> lstTo = EXPRESSO.Processing.LocalData.LoadData.getListByRoute(to, 3);
                string idRound = null;
                int startSeq = 0;
                int endSeq = 0;
                decimal rate = 0;
                foreach (var f in lstFrom)
                {
                    foreach (var t in lstTo)
                    {
                        if (f.idRoute == t.idRoute)
                        {
                            idRound = f.idRoute;
                            startSeq = f.intSeq;
                            endSeq = t.intSeq;
                            break;
                        }
                    }
                }

                rate = EXPRESSO.Processing.LocalData.LoadData.getRate(from, to, vclass);
                if (OnGetInfoResult != null)
                {
                    OnGetInfoResult(idRound, startSeq, endSeq, rate, vclass.intValue);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadInfo", ex.Message);
            }
            
        }


        public async void doSearch(string idRound, int intStart,int intEnd)
        {
            try
            {
                List<Journey> mJourneyList = new List<Journey>();
                var listRouteDetail = LoadData.getListByRoute(idRound, intStart, intEnd);
                foreach (var route in listRouteDetail)
                {
                    Journey journey = new Journey();
                    journey.mTblRouteDetail = route;
                    if (route.intType == 0
                        || route.intType == 4
                        || route.intType == 5
                        || route.intType == 6
                        || route.intType == 7
                        || route.intType == 8
                        )
                    {
                        //RSA
                        var rsa = LoadData.getRSA(route.idRouteItem);
                        if (rsa == null)
                            continue;

                        journey.mStrParentTypeName = "RSA";
                        journey.mParentType = EnumType.FacilitiesType.RSA;
                        journey.mStrParentName = rsa.strName;
                        switch (route.intType)
                        {
                            case 0:
                                {
                                    if (rsa.strType == "3")
                                    {
                                        journey.mStrParentTypeName = "RSA Signature";
                                    }
                                    break;
                                }
                            case 4:
                                {
                                    journey.mStrParentTypeName = "OBR";
                                    break;
                                }
                            case 5:
                                {
                                    if (rsa.strType == "1")
                                    {
                                        journey.mStrParentTypeName = "RSA Signature";
                                    }
                                    else
                                    {
                                        journey.mStrParentTypeName = "Lay-By";
                                    }
                                    journey.mParentType = EnumType.FacilitiesType.LAYBY;
                                    break;
                                }
                            case 6:
                                {
                                    journey.mStrParentTypeName = "Interchange";
                                    journey.mParentType = EnumType.FacilitiesType.INTERCHANGE;
                                    break;
                                }
                            case 7:
                                {
                                    journey.mStrParentTypeName = "Tunnel";
                                    journey.mParentType = EnumType.FacilitiesType.TUNNEL;
                                    break;
                                }
                            case 8:
                                {
                                    journey.mStrParentTypeName = "Vista Point";
                                    journey.mParentType = EnumType.FacilitiesType.TUNNEL;
                                    break;
                                }
                        }
                    }
                    else if (route.intType == 1)
                    {
                        //Petrol Station
                        var petrol = LoadData.getPertrolStation(route.idRouteItem);
                        if (petrol == null)
                        {
                            continue;
                        }
                        journey.mStrParentName = petrol.strName;
                        journey.mParentType = EnumType.FacilitiesType.PETROLSTATION;
                        journey.mStrParentTypeName = "Petrol Station";
                    }
                    else if (route.intType == 2)
                    {
                        //CSC
                        var csc = LoadData.getCSC(route.idRouteItem);
                        if (csc == null)
                        {
                            continue;
                        }
                        journey.mStrParentName = csc.strName;
                        journey.mParentType = EnumType.FacilitiesType.CSC;
                        journey.mStrParentTypeName = "CSC";
                    }
                    else if (route.intType == 3)
                    {
                        //Toll Plaza
                        var tollPlaza = LoadData.getTollPlaza(route.idRouteItem);
                        if (tollPlaza == null)
                        {
                            continue;
                        }
                        journey.mStrParentName = tollPlaza.strName;
                        journey.mParentType = EnumType.FacilitiesType.TOLLPLAZA;
                        journey.mStrParentTypeName = "Toll Plaza";

                    }
                    mJourneyList.Add(journey);
                }
                if (OnSearch != null)
                {
                    OnSearch(mJourneyList);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("doSearch", ex.Message);
            }
            
        }

        public delegate void onGetTollFare(TblTollFare tollfare);
        public onGetTollFare OnGetTollFare;
        public async void getTollFare(string  id,string idto)
        {
            TblTollFare item = null;
            if (string.IsNullOrEmpty(idto))
            {
                item = LoadData.getTollFare(id);
            }
            else
            {
                 item = LoadData.getTollFare(id, idto);
            }
            if (OnGetTollFare != null)
            {
                OnGetTollFare(item);
            }
        }

        public delegate void onGetAmount(decimal tollAmt);
        public onGetAmount OnGetAmount;
        public async void GetAmountTollFare(TblTollPlaza from, TblTollPlaza to, VehicleClasses vclass)
        {
            decimal TollAmt = 0;
            if (to == null)
            {
                var item = await  Task.Run(() => LoadData.getTollFare(from.idTollPlaza));
                switch (vclass.intValue)
                {
                    case 1:
                        {
                            TollAmt = item.decTollAmt1;
                            break;
                        }
                    case 2:
                        {
                            TollAmt = item.decTollAmt2;
                            break;
                        }
                    case 3:
                        {
                            TollAmt = item.decTollAmt3;
                            break;
                        }
                    case 4:
                        {
                            TollAmt = item.decTollAmt4;
                            break;
                        }
                    case 5:
                        {
                            TollAmt = item.decTollAmt5;
                            break;
                        }
                    default:
                        {
                            TollAmt = -1;
                            break;
                        }
                }
            }
            else
            {
                var item = await Task.Run(() => LoadData.getTollFare(from.idTollPlaza,to.idTollPlaza));
                if (item != null)
                {
                    switch (vclass.intValue)
                    {
                        case 1:
                            {
                                TollAmt = item.decTollAmt1;
                                break;
                            }
                        case 2:
                            {
                                TollAmt = item.decTollAmt2;
                                break;
                            }
                        case 3:
                            {
                                TollAmt = item.decTollAmt3;
                                break;
                            }
                        case 4:
                            {
                                TollAmt = item.decTollAmt4;
                                break;
                            }
                        case 5:
                            {
                                TollAmt = item.decTollAmt5;
                                break;
                            }
                        default:
                            {
                                TollAmt = -1;
                                break;
                            }
                    }
                }
                else
                {
                    TollAmt = -1;
                }
            }
            if (OnGetAmount != null)
            {
                OnGetAmount(TollAmt);
            }
        }
    }
}
