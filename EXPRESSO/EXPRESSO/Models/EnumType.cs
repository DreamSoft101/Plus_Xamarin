using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class EnumType
    {
        public enum StatusType
        {
            Active = 1,
            InActive = 0,
            Deleted = -1
        }

        public enum LiveTrafficType
        {
            All = 0,
            Alert = 1,
            RSAClosure = 2,
            MajorDisruption = 3,
            Incident = 4,
            RoadWork = 5,
            AlternativeRoute = 6,
            FutureEvent = 7,
            HeavyVechicleRestriction = 8,
            AESSpeedTrap = 9
        }

        public enum FacilityType
        {

        }

        public enum LiveFeedType
        {

        }

        public enum DeviceType
        {
            iPhone = 0,
            iPad = 1,
            AndroidPhone = 2,
            AndroidTablet = 3,

        }

        
        public enum FavoriteType
        {
            RSA = 1,
            TollPlaza = 2,
            PertrolStation = 3,
            CSC = 4,
            Facilities = 5,
            Nearby = 6,
            Location = 7,
            LiveFeed = 8,
            LiveFeed_TollPlaza = 9,
        }
        public enum FacilitiesType
        {
            RSA = 0,
            RSA_SIGNATURE = 1,
            CSC = 2,
            LAYBY = 3,
            TOLLPLAZA = 4,
            INTERCHANGE = 5,
            PETROLSTATION = 6,
            TNG_RELOAD = 7,
            TUNNEL = 8,
            VISTAPOINT = 9,
            NEARBY = 10,
            POLICE_STATION = 11,
            FIRE_STATION = 12,
            HOSPITAL = 13,
            CAFE_RESTAURENT = 14,
            SPORTS = 15,
            RETAIL = 16,
            FACILITY = 17,
            SSK = 18,
            PLUSSmile = 19,
        }
        /*
        public enum RSAType
        {
            RSA = 1,
            Lay_By = 2
        }
        */

    }
}
