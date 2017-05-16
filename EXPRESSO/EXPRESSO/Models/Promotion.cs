using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class Promotion
    {
        public string IdPromotion;
        public string IdCategory;
        public string strTitle;
        public string strmerchantName;
        public string strAddress;
        public string strContactNo;
        public string strWebsite;
        public string strEmail;
        public string strDescription;
        public string strLocationImg;
        public long dtValidFromDate;
        public long dtValidToDate;
        public string strTerms;
        public string strSmsMsg;
        public string strEmailMsg;
        public string strFacebookMsg;
        public decimal decLat;
        public decimal decLng;
        public int intStatus;
    }
}
