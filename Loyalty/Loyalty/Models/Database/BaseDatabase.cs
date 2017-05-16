using Loyalty.Models.Database;
using SQLite.Net;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput.Database
{
    public static class BaseDatabase
    {
        private static SQLiteConnection database;
        public static object locker = new object();

        public static void Init(string path, ISQLitePlatform sqlitePlatform)
        {
            if (database == null)
            {
                database = new SQLiteConnection(sqlitePlatform, path);
                database.CreateTable<MemberType>();
                database.CreateTable<Merchant>();
                database.CreateTable<MerchantCategory>();
                database.CreateTable<MerchantProduct>();
                database.CreateTable<RedemptionCategory>();
                database.CreateTable<RedemptionPartner>();
                database.CreateTable<RedemptionProduct>();
                database.CreateTable<RedemptionProductDetail>();
                database.CreateTable<TblLog>();
                database.CreateTable<Document>();
                database.CreateTable<MerchantLocation>();
                database.CreateTable<Favorites>();
                database.CreateTable<MerchantProductMemberType>();
                database.CreateTable<Recent>();
                database.CreateTable<MemberGroup>();
                database.CreateTable<MemberGroupDetail>();
                database.CreateTable<MemberRedeemInfoProduct>();
                database.CreateTable<Country>();
                database.CreateTable<State>();
            }
        }

        public static SQLiteConnection getDB(string path, ISQLitePlatform sqlitePlatform)
        {
            if (database == null)
            {
                database = new SQLiteConnection(sqlitePlatform, path);
            }
            return database;
        }

        public static SQLiteConnection getDB()
        {
            return database;
        }
    }
}