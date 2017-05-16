
using EXPRESSO.Models.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EXPRESSO.Utils.Cons;

namespace EXPRESSO.Utils
{
    public static class LogUtils
    {
        public static DebugType debugType = DebugType.LogAll;
        public static string mPath;
        public static void WriteLog(string TAG,string Content)
        {
            if (debugType == DebugType.LogCat)
            {
                //Log.Info(TAG, Content);
            }
            else if (debugType == DebugType.LogFile)
            {
                Write2DB(TAG, Content, 1);
            }
            else if (debugType == DebugType.LogAll)
            {
                //Log.Info(TAG, Content);
                Write2DB(TAG, Content, 1);
            }
        }

        public static void Write2DB(string Tag, string Content,int type)
        {
            //lock (BaseDatabase.locker)
            //{
            //    //TblLog log = new TblLog();
            //    //log.strContent = Content;
            //    //log.strDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            //    //log.strTag = Tag;
            //    //log.intType = type;
            //    //BaseDatabase.getDB().Insert(log);
            //}
        }

        public static void WriteError(string TAG, string Content)
        {
            if (debugType == DebugType.LogCat)
            {
                //Log.Error(TAG, Content);
            }
            else if (debugType == DebugType.LogFile)
            {
                Write2DB(TAG, Content, -1);
            }
            else if (debugType == DebugType.LogAll)
            {
                //Log.Info(TAG, Content);
                Write2DB(TAG, Content, -1);
            }
        }
    }
}
