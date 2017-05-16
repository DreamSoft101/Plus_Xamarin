using Loyalty.Models.ServiceOutput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class UserInfos
    {
        public string strUserName;
        public string strAvatar;
        public string strToken;
        public string strMobileNo;
        public string strFirstName;
        public string strLastName;
        public bool isActive = false;
        public int intStatus;
        public DateTime dtCreateDate;

        public MValidateMemberCredentials LoyaltyAccount { get; set; }
    }
}
