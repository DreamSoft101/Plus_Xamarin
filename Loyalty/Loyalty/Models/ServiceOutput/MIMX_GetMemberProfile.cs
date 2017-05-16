using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MGetMemberProfile : ResponseBase
    {
        public string PortalEmail;
        public string PortalDisplayName;
        public string PortalFBToken;
        public string PortalGToken;
        public string inAccountReferenceNo;
        public string inCardNumber;
        public int inAccountType;
        public Int64 idMasterAccount;
        public string strMasterAccountReferenceNo;
        public int intMasterAccountReferenceType;
        public string strFirstName;
        public string strLastName;
        public string strDisplayName;
        public string strDateofBirth;
        public byte Gender;
        public string Nationality;
        public MAdress CorrespondenceAddress = new MAdress();
        public MAdress DeliveryAddress = new MAdress();
        public string strIncomeLevel;
        public string strEducationBackground;
        public string strJobTitle;
        public string strBusinessPhone;
        public string strHousePhone;
        public string NotifyMemberViaEmail;
        public string NotifyMemberViaSMS;
        public string strRemarks;
        public string strFax;
        public string PreferredContactMethod;
        public string strMobileNo;
        public string strEmail;
        public int intMemberStatus;
        public int intBillCycle;
        public MIMX_AccountDetails[] AccountDetails;
        public int intMaritalStatus;
        public string strReligion;
        public string strRace;
        public int intStatus;
        public byte bitEStatement;
        public string dtEStatementSubscribe;
        public List<int> Hobbies { get; set; }
        public List<int> Interests { get; set; }
        public MGetMemberProfile(ResponseBase p)
        {
            this.CopyFromBase(p);
        }

        public MGetMemberProfile()
        {

        }
    }
}
