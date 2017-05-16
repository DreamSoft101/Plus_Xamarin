using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TrafficUpdate
    {
        public TrafficUpdate()
        {

        }
        public string idHwayLocation { get; set; }
        public string idTrafficUpdate { get; set; }
        public int intType { get; set; }
        public string idHighway { get; set; }
        public decimal decLocation { get; set; }
        public double decLng { get; set; }
        public double decLat { get; set; }
        public string strTitle { get; set; }
        public int intSpeedLimit { get; set; }
        public int intWaterLevel { get; set; }
        public string strDescription { get; set; }
        public string strURL { get; set; }
        public DateTime dtStartDateTime { get; set; }
        public DateTime dtEndDateTime { get; set; }
        public int intVisible { get; set; }
        public int intStatus { get; set; }
        public long dtLastUpdate { get; set; }
        public string idEntity { get; set; }
        public string strWaypoint { get; set; }

        public List<Waypoint> lstWaypoint { get; set; }
        public string overview_polyline { get; set; }

        private int SortLatLng { get; set; }

        public void SetSortLatLng (int value)
        {
            SortLatLng = value;
        }

        public int GetSortLatLng()
        {
            return SortLatLng;
        }

    }

}
