using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXPRESSO.Models.Database;
using EXPRESSO.Models;

namespace EXPRESSO
{
    public  class LoadData
    {
        public delegate void onLoadListCSC(List<TblCSC> result);
        public delegate void onLoadListLiveTraffic(List<TrafficUpdate> result);
        public delegate void onLoadTollPlaza(List<TblTollPlaza> result);
        public delegate void onLoadListLiveFeed(List<TrafficUpdate> result);
        public delegate void onLoadListRSA(List<TblRSA> result);
        public delegate void onLoadFacilities(List<TblFacilities> result);

        public event onLoadListCSC OnLoadListCSC;
        public event onLoadListLiveTraffic OnLoadListLiveTraffic;
        public event onLoadTollPlaza OnLoadListTollPlaza;
        public event onLoadListLiveFeed OnLoadListLiveFeed;
        public event onLoadListRSA OnLoadListRSA;
        public event onLoadFacilities OnLoadFacilities;

        private List<TrafficUpdate> getListLiveTraffic(int type)
        {
            return null;
        }
        public  List<TblCSC> getListCSC()
        {
            List<TblCSC> result = new List<TblCSC>();
            if (OnLoadListCSC != null)
            {
                OnLoadListCSC(result);
            }
            return null;
        }

        public List<TblTollPlaza> getListTollPlaza()
        {
            List<TblTollPlaza> result = new List<TblTollPlaza>();
            if (OnLoadListTollPlaza != null)
            {
                OnLoadListTollPlaza(result);
            }
            return null;
        }

        public List<TrafficUpdate> getListLiveFeed()
        {
            return null;
        }

        public List<TblRSA> getListRSA()
        {
            return null;
        }

        public List<TblFacilities> getListFacilities ()
        {
            return null;
        }



    }
}
