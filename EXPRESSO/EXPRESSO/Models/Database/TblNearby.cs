using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblNearby
    {
        public TblNearby()
        {

        }
        [PrimaryKey]
        public string idNearby { get; set; }
        public string idNearbyCatg { get; set; }
        public string strTitle { get; set; }
        public string strAddress { get; set; }
        public string strContactNo { get; set; }
        public string strWebsite { get; set; }
        public string strEmail { get; set; }
        public double floLatitude { get; set; }
        public double floLongitude { get; set; }
        public string strDescription { get; set; }
        public string strLocationImg { get; set; }
        public DateTime? dtValidFromDate { get; set; }
        public DateTime? dtValidToDate { get; set; }
        public DateTime? dtDisplayFromDate { get; set; }
        public DateTime? dtDisplayToDate { get; set; }
        public string strTermsConditions { get; set; }
        public string strSMSMsg { get; set; }
        public string strEmailMsg { get; set; }
        public string strFacebookMsg { get; set; }
        public string strTwitterMsg { get; set; }
        public int intStatus { get; set; }
        public string idEntity { get; set; }
    }
}
