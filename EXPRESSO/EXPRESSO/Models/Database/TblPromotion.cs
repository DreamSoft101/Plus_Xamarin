using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblPromotion
    {
        public TblPromotion()
        {

        }
        [PrimaryKey]
        public int idPromo { get; set; }
        public int idPromoCatg { get; set; }
        public string strTitle { get; set; }
        public string strMerchantName { get; set; }
        public string strAddress { get; set; }
        public string strContactNo { get; set; }
        public string strWebsite { get; set; }
        public string strEmail { get; set; }
        public decimal decLatitude { get; set; }
        public decimal decLongitude { get; set; }
        public string strDescription { get; set; }
        public string strLocationImg { get; set; }
        public long dtValidFromDate { get; set; }
        public long dtValidToDate { get; set; }
        public long dtDisplayFromDate { get; set; }
        public long dtDisplayToDate { get; set; }
        public string strTermsConditions { get; set; }
        public string strSMSMsg { get; set; }
        public string strEmailMsg { get; set; }
        public string strFacebookMsg { get; set; }
        public string strTwitterMsg { get; set; }
        public int intStatus { get; set; }
        public string idEntity { get; set; }
    }
}
