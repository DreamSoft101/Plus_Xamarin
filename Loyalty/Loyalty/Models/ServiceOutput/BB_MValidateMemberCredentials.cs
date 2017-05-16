using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MValidateMemberCredentials : ResponseBase
    {
        public MGetMemberProfile MemberProfile;
        private MLoginParams mLoginParams;
        public MLoginParams LoginParams
        {
            get { return this.mLoginParams; }
            set { this.mLoginParams = value; }
        }
        public MValidateMemberCredentials()
        {

        }
        public MValidateMemberCredentials(ResponseBase p)
        {
            this.CopyFromBase(p);
            mLoginParams = new MLoginParams();
        }

    }

    public class MLoginParams
    {
        private long mIntPortalCredentials;
        public long idPortalCredentials
        {
            get { return this.mIntPortalCredentials; }
            set { this.mIntPortalCredentials = value; }
        }
        private string mStrAuthSessionId;
        public string authSessionId
        {
            get { return this.mStrAuthSessionId; }
            set { this.mStrAuthSessionId = value; }
        }
        private string mStrLoginId;
        public string strLoginId
        {
            get { return this.mStrLoginId; }
            set { this.mStrLoginId = value; }
        }
        private byte mBLoginIdStatus;
        public byte LoginIdStatus
        {
            get { return this.mBLoginIdStatus; }
            set { this.mBLoginIdStatus = value; }
        }
        private long mIntIdMasterAccount;
        public long idMasterAccount
        {
            get { return this.mIntIdMasterAccount; }
            set { this.mIntIdMasterAccount = value; }
        }
        private DateTime mDtePwdExpiryDate;
        public DateTime PwdExpiryDate
        {
            get { return this.mDtePwdExpiryDate; }
            set { this.mDtePwdExpiryDate = value; }
        }
        private byte mBChangePwdFlag;
        public byte ChangePwdFlag
        {
            get { return this.mBChangePwdFlag; }
            set { this.mBChangePwdFlag = value; }
        }

    }
}
