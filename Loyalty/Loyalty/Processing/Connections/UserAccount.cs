using Loyalty.Models.ServiceOutput;
using Loyalty.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Loyalty.Utils.EncryptUtils;

namespace Loyalty.Processing.Connections
{
    public static class UserAccount
    {
        //public delegate void onResult(ServiceResult result);
        //public onResult OnResult;

        public  static async Task<MRegisterPortalCredentials> Register(int Type, string UserName, string Token, string Password, string FirstName, string LastName, string MobileNo, string Email)
        {
            MRegisterPortalCredentials result = new MRegisterPortalCredentials();
            try
            {
                BaseClient client = new BaseClient();
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + Type + UserName + Password + Email;
                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "RegisterPortalCredentials";
                client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("RefType", Type);
                //client.AddParameter("RefNo", Type);
                client.AddParameter("LoginId", UserName);
                client.AddParameter("Password", Password);
                client.AddParameter("FirstName", FirstName);
                client.AddParameter("LastName", LastName);
                client.AddParameter("Email", Email);
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MRegisterPortalCredentials>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Register", ex.Message);
            }
            return result;
        }

        public static async Task<MValidateMemberCredentials> Login(string UserName, string Token, string Password)
        {
            MValidateMemberCredentials result = new MValidateMemberCredentials();
            try
            {
                BaseClient client = new BaseClient();
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + UserName + Password ;
                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "ValidateMemberCredentials";
                client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
             
                client.AddParameter("LoginId", UserName);
                client.AddParameter("Password", "a5309d3c348d152582424506a142a685");
              
                string strResult = await client.getData();

                var data = JsonConvert.DeserializeObject<MValidateMemberCredentials>(strResult);

                data.MemberProfile = await GetMemberProfile(data.LoginParams.authSessionId, data.LoginParams.strLoginId);

                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Register", ex.Message);
            }
            return result;
        }

        public static async Task<MValidateMemberCredentials> LinkSSOAccountToMasterAccount(string refNo, int refType)
        {
            MValidateMemberCredentials result = new MValidateMemberCredentials();
            try
            {
                BaseClient client = new BaseClient();
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + refNo + refType + Cons.mMemberCredentials.LoginParams.authSessionId + Cons.mMemberCredentials.LoginParams.strLoginId;
                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "LinkSSOAccountToMasterAccount";
                client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("refNo", refNo);
                client.AddParameter("refType", refType);
                client.AddParameter("authSessionId", Cons.mMemberCredentials.LoginParams.authSessionId);
                client.AddParameter("LoginId", Cons.mMemberCredentials.LoginParams.strLoginId);
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MValidateMemberCredentials>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Register", ex.Message);
            }
            return result;
        }

        public static async Task<MGetMemberProfile> GetMemberProfile(string authSessionId, string LoginID)
        {
            MGetMemberProfile result = new MGetMemberProfile();
            try
            {
                BaseClient client = new BaseClient();
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + authSessionId + LoginID ;// + Cons.mMemberCredentials.LoginParams.authSessionId + Cons.mMemberCredentials.LoginParams.strLoginId;
                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "GetMemberProfile";
                client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("authSessionId", authSessionId);
                client.AddParameter("LoginID", LoginID);
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MGetMemberProfile>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("IMX_GetMemberProfile", ex.Message);
            }
            return result;
        }



        public static async Task<MGetRedemptionHistory> GetRedemptionHistory(long idMasterAccount, string authSessionId)
        {
            MGetRedemptionHistory result = new MGetRedemptionHistory();
            try
            {
                BaseClient client = new BaseClient();
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + idMasterAccount + authSessionId ;// + Cons.mMemberCredentials.LoginParams.authSessionId + Cons.mMemberCredentials.LoginParams.strLoginId;
                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "GetRedemptionHistory";
                client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("idMasterAccount", idMasterAccount);
                client.AddParameter("authSessionId", authSessionId);
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MGetRedemptionHistory>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetRedemptionHistory", ex.Message);
            }
            return result;
        }

        public static async Task<MGetRewardAccountSummary> GetRewardAccountSummary(long idMasterAccount, string authSessionId, Guid? MemberID)
        {
            MGetRewardAccountSummary result = new MGetRewardAccountSummary();
            try
            {
                BaseClient client = new BaseClient();
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + authSessionId+ idMasterAccount  + (MemberID == null ? "" : MemberID.ToString());// + Cons.mMemberCredentials.LoginParams.authSessionId + Cons.mMemberCredentials.LoginParams.strLoginId;
                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "GetRewardAccountSummary";
                client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("idMasterAccount", idMasterAccount);
                client.AddParameter("authSessionId", authSessionId);
                client.AddParameter("MemberID", (MemberID == null ? "" : MemberID.ToString()));
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MGetRewardAccountSummary>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetRedemptionHistory", ex.Message);
            }
            return result;
        }

        public static async Task<MGetStatementFileList> GetStatementFileList(long idMasterAccount, string authSessionId, Guid MemberID)
        {
            MGetStatementFileList result = new MGetStatementFileList();
            try
            {
                BaseClient client = new BaseClient();
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + authSessionId + idMasterAccount +  MemberID.ToString();// + Cons.mMemberCredentials.LoginParams.authSessionId + Cons.mMemberCredentials.LoginParams.strLoginId;
                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "GetStatementFileList";
                client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("idMasterAccount", idMasterAccount);
                client.AddParameter("authSessionId", authSessionId);
                client.AddParameter("memberID", MemberID);
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MGetStatementFileList>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetRedemptionHistory", ex.Message);
            }
            return result;
        }
        /*
        public static async Task<MGetStatementFileList> GetPointBalance()
        {
            MGetStatementFileList result = new MGetStatementFileList();
            try
            {
                BaseClient client = new BaseClient();
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + Cons.mMemberCredentials.LoginParams.authSessionId + Cons.mMemberCredentials.MemberProfile.idMasterAccount + MemberID.ToString();// + Cons.mMemberCredentials.LoginParams.authSessionId + Cons.mMemberCredentials.LoginParams.strLoginId;
                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "IMX_GetPointBalance";
                client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("idMasterAccount", idMasterAccount);
                client.AddParameter("authSessionId", authSessionId);
                client.AddParameter("memberID", MemberID);
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MGetStatementFileList>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetRedemptionHistory", ex.Message);
            }
            return result;
        }*/
    }
}
