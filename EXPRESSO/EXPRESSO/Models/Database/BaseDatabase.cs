using SQLite.Net;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
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
                //database.CreateTable<TblPost>();
                //database.CreateTable<TblCategory>();
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
