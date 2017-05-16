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
    public static class DeleteData
    {
        public static TblFavLoc DeleteFavorite(int idFavorite)
        {
            lock (BaseDatabase.locker)
            {
                var item = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idMyFav == idFavorite).FirstOrDefault();
                BaseDatabase.getDB().Table<TblFavLoc>().Delete(p => p.idMyFav == idFavorite);
                return item;
            }
        }

        public static void DeleteAllFavorite()
        {
            lock (BaseDatabase.locker)
            {
                LogUtils.WriteLog("DeleteAllFavorite", Cons.IdEntity);
                BaseDatabase.getDB().Table<TblFavLoc>().Delete(p => p.idEntity == Cons.IdEntity);
            }
        }
        public static TblFavLoc DeleteFavorite(string idObject, FavoriteType type)
        {
            lock (BaseDatabase.locker)
            {
                var item = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idObject == idObject && p.intObjectType == (int)type && p.idEntity == Cons.IdEntity).FirstOrDefault();
                LogUtils.WriteLog("DeleteFavorite", JsonConvert.SerializeObject(item));
                BaseDatabase.getDB().Table<TblFavLoc>().Delete(p => p.idObject == idObject && p.intObjectType == (int)type && p.idEntity == Cons.IdEntity);
                return item;
            }
        }

        public static TblFavLoc DeleteFavorite(TblRSA tblrsa)
        {
            lock (BaseDatabase.locker)
            {
                int idType = (int)FavoriteType.RSA;
                var item = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idObject == tblrsa.idRSA && p.intObjectType == idType && p.idEntity == Cons.IdEntity).FirstOrDefault();
                item.intStatus = 2;
                BaseDatabase.getDB().Table<TblFavLoc>().Delete(p => p.idObject == tblrsa.idRSA && p.intObjectType == idType && p.idEntity == Cons.IdEntity);
                return item;
            }
        }

        public static TblFavLoc DeleteFavorite(TrafficUpdate tblUpdate)
        {
            lock (BaseDatabase.locker)
            {
                int idType = (int)FavoriteType.LiveFeed;
                var item = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idObject == tblUpdate.idTrafficUpdate && p.intObjectType == idType && p.idEntity == Cons.IdEntity).FirstOrDefault();
                item.intStatus = 2;
                BaseDatabase.getDB().Table<TblFavLoc>().Delete(p => p.idObject == tblUpdate.idTrafficUpdate && p.intObjectType == idType && p.idEntity == Cons.IdEntity);
                return item;
            }
        }

        public static TblFavLoc DeleteFavorite(TollPlazaCCTV tollPlazaCCTV)
        {
            lock (BaseDatabase.locker)
            {
                int idType = (int)FavoriteType.LiveFeed_TollPlaza;
                var item = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idObject == tollPlazaCCTV.idTollPlazaCctv && p.intObjectType == idType && p.idEntity == Cons.IdEntity).FirstOrDefault();
                item.intStatus = 2;
                BaseDatabase.getDB().Table<TblFavLoc>().Delete(p => p.idObject == tollPlazaCCTV.idTollPlazaCctv && p.intObjectType == idType && p.idEntity == Cons.IdEntity);
                return item;
            }
        }

        public static TblFavLoc DeleteFavorite(TblPetrolStation tblPertrol)
        {
            lock (BaseDatabase.locker)
            {
                int idType = (int)FavoriteType.PertrolStation;
                var item = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idObject == tblPertrol.idPetrol && p.intObjectType == idType && p.idEntity == Cons.IdEntity).FirstOrDefault();
                item.intStatus = 2;
                BaseDatabase.getDB().Table<TblFavLoc>().Delete(p => p.idObject == tblPertrol.idPetrol && p.intObjectType == idType && p.idEntity == Cons.IdEntity);
                return item;
            }
        }

        public static TblFavLoc DeleteFavorite(TblCSC tblCSC)
        {
            lock (BaseDatabase.locker)
            {
                int idType = (int)FavoriteType.CSC;
                var item = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idObject == tblCSC.idCSC && p.intObjectType == idType && p.idEntity == Cons.IdEntity).FirstOrDefault();
                item.intStatus = 2;
                BaseDatabase.getDB().Table<TblFavLoc>().Delete(p => p.idObject == tblCSC.idCSC && p.intObjectType == idType && p.idEntity == Cons.IdEntity);
                return item;
            }
        }

        public static TblFavLoc DeleteFavorite(TblTollPlaza tblTollPlaza)
        {
            lock (BaseDatabase.locker)
            {
                int idType = (int)FavoriteType.TollPlaza;
                var item = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idObject == tblTollPlaza.idTollPlaza && p.intObjectType == idType && p.idEntity == Cons.IdEntity).FirstOrDefault();
                item.intStatus = 2;
                BaseDatabase.getDB().Table<TblFavLoc>().Delete(p => p.idObject == tblTollPlaza.idTollPlaza && p.intObjectType == idType && p.idEntity == Cons.IdEntity);
                return item;
            }
        }

        public static TblFavLoc DeleteFavorite(TblNearby tblNearby)
        {
            lock (BaseDatabase.locker)
            {
                int idType = (int)FavoriteType.Nearby;
                var item = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idObject == tblNearby.idNearby && p.intObjectType == idType && p.idEntity == Cons.IdEntity).FirstOrDefault();
                item.intStatus = 2;
                BaseDatabase.getDB().Table<TblFavLoc>().Delete(p => p.idObject == tblNearby.idNearby && p.intObjectType == idType && p.idEntity == Cons.IdEntity);
                return item;
            }
        }

        public static TblFavLoc DeleteFavorite(TblFacilities tblFacility)
        {
            lock (BaseDatabase.locker)
            {
                int idType = (int)FavoriteType.Facilities;
                var item = BaseDatabase.getDB().Table<TblFavLoc>().Where(p => p.idObject == tblFacility.idFacilities && p.intObjectType == idType && p.idEntity == Cons.IdEntity).FirstOrDefault();
                item.intStatus = 2;
                BaseDatabase.getDB().Table<TblFavLoc>().Delete(p => p.idObject == tblFacility.idFacilities && p.intObjectType == idType && p.idEntity == Cons.IdEntity);
                return item;
            }
        }


        public static void clearAllLog()
        {
            lock (BaseDatabase.locker)
            {
                BaseDatabase.getDB().Table<TblLog>().Delete(p => 1 == 1);
            }
        }

        public static void clearCategory()
        {
            lock (BaseDatabase.locker)
            {
                BaseDatabase.getDB().Table<TblCategory>().Delete(p => 1 == 1);
            }
        }

    }
}
