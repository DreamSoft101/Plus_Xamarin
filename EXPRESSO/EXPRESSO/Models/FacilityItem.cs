using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class FacilityItem
    {
        public string ID { get; set; }
        public string strName { get; set; }
        public string UrlImg { get; set; }
        public int IsNearby { get; set; } //1. is Nearby
        public int Type { get; set; } //RSA Toll CSC Pertrol
        public string IdHighway { get; set; }
        public List<string> SubUrlImg { get; set; }

        public object Data { get; set; }

        public bool IsFavorite { get; set; }

        public string strCategoryNearbyName { get; set; }
        public FacilityItem()
        {
            SubUrlImg = new List<string>();
        }
    }
}
