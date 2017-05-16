using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class DomainSetting
    {
        public Settings setting { get; set; }
        
    }

    public class Settings
    {
        public int maxtitle { get; set; }
        public int maxdescription { get; set; }
        public int rowperpage { get; set; }
        public int thumblargew { get; set; }
        public int thumblargeh { get; set; }
        public int thumbsmallw { get; set; }
        public int thumbsmallh { get; set; }
        public int compressed { get; set; }
        public int maxsizew { get; set; }
        public int maxsizeh { get; set; }
        public int maximage { get; set; }
        public string traffic_alert_label { get; set; }
        public string road_closure_label { get; set; }
        public string poi_closure_label { get; set; }
        public string major_disruption_label { get; set; }
        public string incidents_congestion_label { get; set; }
        public string roadworks_label { get; set; }
        public string adverse_weather_label { get; set; }
        public string alternative_route_label { get; set; }
        public string speed_limit_label { get; set; }
        public string speed_trap_label { get; set; }
        public string future_event_label { get; set; }
        public string future_roadwork_label { get; set; }
        public string future_road_closure_label { get; set; }
        public string future_poi_closure_label { get; set; }
        public string vehicle_class_1_label { get; set; }
        public string vehicle_class_1_icon { get; set; }
        public string vehicle_class_2_icon { get; set; }
        public string vehicle_class_3_icon { get; set; }
        public string vehicle_class_4_icon { get; set; }
        public string vehicle_class_5_icon { get; set; }
        public string vehicle_class_2_label { get; set; }
        public string vehicle_class_3_label { get; set; }
        public string vehicle_class_4_label { get; set; }
        public string vehicle_class_5_label { get; set; }
        public string toll_plaza_label { get; set; }
        public string vista_point_label { get; set; }
        public string csc_label { get; set; }
        public string rsa_label { get; set; }
        public string petrol_station_label { get; set; }
        public string tunnel_label { get; set; }
        public string interchange_label { get; set; }
        public string rsa_icon { get; set; }
        public string toll_plaza_icon { get; set; }
        public string traffic_alert_icon { get; set; }
        public string road_closure_icon { get; set; }
        public string poi_closure_icon { get; set; }
        public string vista_point_icon { get; set; }
        public string csc_icon { get; set; }
        public string petrol_station_icon { get; set; }
        public string tunnel_icon { get; set; }
        public string interchange_icon { get; set; }

    }


}
