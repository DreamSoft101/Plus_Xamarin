using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using EXPRESSO.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EXPRESSO.Models.EnumType;

namespace EXPRESSO.Processing.LocalData
{
    public static class AddData
    {
        
        public static TblFavLoc addFavorite(TblRSA tblRSA)
        {
            lock (BaseDatabase.locker)
            {
                string jsonData = JsonConvert.SerializeObject(tblRSA);
                TblFavLoc fav = new TblFavLoc();
                fav.idEntity = Cons.IdEntity;
                fav.idObject = tblRSA.idRSA;
                fav.strDescription = jsonData;
                
                fav.intObjectType = (int)FavoriteType.RSA;
                
                BaseDatabase.getDB().Insert(fav);
                LogUtils.WriteLog("addFavorite", jsonData);
                return fav;
            }
        }


        public static TblFavLoc addFavorite(TollPlazaCCTV tollPlazaCCTV)
        {
            lock (BaseDatabase.locker)
            {
                string jsonData = JsonConvert.SerializeObject(tollPlazaCCTV);
                TblFavLoc fav = new TblFavLoc();
                fav.idEntity = Cons.IdEntity;
                fav.idObject = tollPlazaCCTV.idTollPlazaCctv;
                fav.strDescription = jsonData;

                fav.intObjectType = (int)FavoriteType.LiveFeed_TollPlaza;

                BaseDatabase.getDB().Insert(fav);
                LogUtils.WriteLog("addFavorite", jsonData);
                return fav;
            }
        }

        public static TblFavLoc addFavorite(TrafficUpdate item)
        {
            lock (BaseDatabase.locker)
            {
                string jsonData = JsonConvert.SerializeObject(item);
                TblFavLoc fav = new TblFavLoc();
                fav.idEntity = Cons.IdEntity;
                fav.idObject = item.idTrafficUpdate;
                fav.strDescription = jsonData;
                fav.intObjectType = (int)FavoriteType.LiveFeed;
                BaseDatabase.getDB().Insert(fav);
                LogUtils.WriteLog("addFavorite", jsonData);
                return fav;
            }
        }

        public static TblFavLoc addFavorite(TblTollPlaza tblTollPlaza)
        {
            lock (BaseDatabase.locker)
            {
                string jsonData = JsonConvert.SerializeObject(tblTollPlaza);
                TblFavLoc fav = new TblFavLoc();
                fav.idEntity = Cons.IdEntity;
                fav.idObject = tblTollPlaza.idTollPlaza;
                fav.strDescription = jsonData;
                fav.intObjectType = (int)FavoriteType.TollPlaza;
                BaseDatabase.getDB().Insert(fav);
                LogUtils.WriteLog("addFavorite", jsonData);
                return fav;
            }
        }

        public static TblFavLoc addFavorite(TblCSC tblCSC)
        {
            lock (BaseDatabase.locker)
            {
                string jsonData = JsonConvert.SerializeObject(tblCSC);
                TblFavLoc fav = new TblFavLoc();
                fav.idEntity = Cons.IdEntity;
                fav.idObject = tblCSC.idCSC;
                fav.strDescription = jsonData;
                fav.intObjectType = (int)FavoriteType.CSC;
                BaseDatabase.getDB().Insert(fav);
                LogUtils.WriteLog("addFavorite", jsonData);
                return fav;
            }
        }

        public static TblFavLoc addFavorite(TblNearby tblNearby)
        {
            lock (BaseDatabase.locker)
            {
                string jsonData = JsonConvert.SerializeObject(tblNearby);
                TblFavLoc fav = new TblFavLoc();
                fav.idEntity = Cons.IdEntity;
                fav.idObject = tblNearby.idNearby;
                fav.strDescription = jsonData;
                fav.intObjectType = (int)FavoriteType.Nearby;
                BaseDatabase.getDB().Insert(fav);
                LogUtils.WriteLog("addFavorite", jsonData);
                return fav;
            }
        }

        public static TblFavLoc addFavorite(TblPetrolStation tblPertrolStation)
        {
            lock (BaseDatabase.locker)
            {
                string jsonData = JsonConvert.SerializeObject(tblPertrolStation);
                TblFavLoc fav = new TblFavLoc();
                fav.idEntity = Cons.IdEntity;
                fav.idObject = tblPertrolStation.idPetrol;
                fav.strDescription = jsonData;
                fav.intObjectType = (int)FavoriteType.PertrolStation;
                BaseDatabase.getDB().Insert(fav);
                LogUtils.WriteLog("addFavorite", jsonData);
                return fav;
            }
        }

        public static TblFavLoc addFavorite(TblFacilities tblFacility)
        {
            lock (BaseDatabase.locker)
            {
                string jsonData = JsonConvert.SerializeObject(tblFacility);
                TblFavLoc fav = new TblFavLoc();
                fav.idEntity = Cons.IdEntity;
                fav.idObject = tblFacility.idFacilities;
                fav.strDescription = jsonData;

                if (tblFacility.intFacilityType == 8 || tblFacility.intFacilityType == 9 || tblFacility.intFacilityType == 10)
                {
                    fav.intObjectType = (int)FacilitiesType.PLUSSmile;
                }
                if (tblFacility.intFacilityType == 12)
                {
                    fav.intObjectType = (int)FacilitiesType.SSK;
                }
                BaseDatabase.getDB().Insert(fav);
                LogUtils.WriteLog("addFavorite", jsonData);
                return fav;
            }
        }

        public static TblPost addPlusRangerPost(TblPost post, List<TblMedia> medias)
        {
            lock (BaseDatabase.locker)
            {
                BaseDatabase.getDB().Insert(post);
                BaseDatabase.getDB().InsertAll(medias);
                return post;
            }
        }

        public static TblCategory addTblCategory(TblCategory category)
        {
            lock (BaseDatabase.locker)
            {
                BaseDatabase.getDB().Insert(category);
                return category;
            }
        }
    }
}
