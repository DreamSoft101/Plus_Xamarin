using Loyalty.Models;
using Loyalty.Models.ServiceOutput;
using Loyalty.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Processing.Connections
{
    public class AddCardConnections
    {
        public static async Task<CheckMasterAccountExistResult> Common_CheckMasterAccountExist(string RefNo, int RefType)
        {
            CheckMasterAccountExistResult result = new CheckMasterAccountExistResult();
            BaseClient client = new BaseClient();
            try
            {
                string TransactionRefNo = StringUtils.RandomString();
                // string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + Type + UserName + Password + FirstName + LastName + MobileNo + Email;
                // hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "Common_CheckMasterAccountExist";
                // client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("authSessionId", Cons.mMemberCredentials.LoginParams.authSessionId);
                client.AddParameter("LoginId", Cons.mMemberCredentials.LoginParams.strLoginId);
                client.AddParameter("RefNo", RefNo);
                client.AddParameter("RefType", RefType);
                //oldID
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<CheckMasterAccountExistResult>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Common_CheckMasterAccountExist", ex.Message);
            }
            return result;
        }

        public static async Task<MValidatePlusMileCard> Common_ValidatePlusMileCard(string RefNo, int RefType)
        {
            MValidatePlusMileCard result = new MValidatePlusMileCard();
            BaseClient client = new BaseClient();
            try
            {
                string TransactionRefNo = StringUtils.RandomString();
                // string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + Type + UserName + Password + FirstName + LastName + MobileNo + Email;
                // hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "Common_ValidatePlusMileCard";
                // client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("authSessionId", Cons.mMemberCredentials.LoginParams.authSessionId);
                client.AddParameter("LoginId", Cons.mMemberCredentials.LoginParams.strLoginId);
                client.AddParameter("MemberRefNo", RefNo);
                client.AddParameter("MemberType", RefType == 1 ? "e9a424ba-4a69-47c5-9a2f-3d9a69716d23" : "04b77922-ec9f-4364-a9d5-3fdf5ab37021");
                //oldID
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MValidatePlusMileCard>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Common_ValidatePlusMileCard", ex.Message);
            }
            return result;
        }

        public static async Task<SentOTPResult> SendOTP(long idMasterAccount, string mobile)
        {
            SentOTPResult result = new SentOTPResult();
            BaseClient client = new BaseClient();
            try
            {
                string TransactionRefNo = StringUtils.RandomString();
                // string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + Type + UserName + Password + FirstName + LastName + MobileNo + Email;
                // hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "Common_SendOTP";
                // client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("authSessionId", Cons.mMemberCredentials.LoginParams.authSessionId);
                client.AddParameter("LoginId", Cons.mMemberCredentials.LoginParams.strLoginId);
                client.AddParameter("idMasterAccount", idMasterAccount);
                client.AddParameter("Mobile", mobile);
                //oldID
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<SentOTPResult>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("SendOTP", ex.Message);
            }
            return result;
        }


        public static async Task<SentOTPResult> ValidateOTP(MValidatePlusMileCard cardInfo, SentOTPResult otp,string strOTP)
        {
            SentOTPResult result = new SentOTPResult();
            BaseClient client = new BaseClient();
            try
            {
                string TransactionRefNo = StringUtils.RandomString();
                // string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + Type + UserName + Password + FirstName + LastName + MobileNo + Email;
                // hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "Common_ValidateOTP";
                // client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("authSessionId", Cons.mMemberCredentials.LoginParams.authSessionId);
                client.AddParameter("LoginId", Cons.mMemberCredentials.LoginParams.strLoginId);
                client.AddParameter("idMasterAccount", otp.idMasterAccount);
                client.AddParameter("MemberID", cardInfo.MemberID);
                client.AddParameter("RefType", cardInfo.RefType);
                client.AddParameter("RefNo", cardInfo.RefNo);
                client.AddParameter("MobileNo", cardInfo.Mobile);
                client.AddParameter("Name", cardInfo.Name);
                client.AddParameter("idSecurityOTPSession", otp.ID);
                client.AddParameter("strSecurityKey", otp.SecurityKey);
                client.AddParameter("strOTP", strOTP);

                //oldID
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<SentOTPResult>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Common_ValidateOTP", ex.Message);
            }
            return result;
        }

        public static async Task<SentOTPResult> ValidateOTP(MasterAccountInfo info, SentOTPResult otp, string strOTP)
        {
            SentOTPResult result = new SentOTPResult();
            BaseClient client = new BaseClient();
            try
            {
                string TransactionRefNo = StringUtils.RandomString();
                // string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + Type + UserName + Password + FirstName + LastName + MobileNo + Email;
                // hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "Common_ValidateOTP";
                // client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("authSessionId", Cons.mMemberCredentials.LoginParams.authSessionId);
                client.AddParameter("LoginId", Cons.mMemberCredentials.LoginParams.strLoginId);
                client.AddParameter("idMasterAccount", otp.idMasterAccount);
                client.AddParameter("MemberID", info.MemberID);
                client.AddParameter("RefType", info.RefType);
                client.AddParameter("RefNo", info.RefNo);
                client.AddParameter("MobileNo", info.Mobile);
                client.AddParameter("Name", info.Name);
                client.AddParameter("idSecurityOTPSession", otp.ID);
                client.AddParameter("strSecurityKey", otp.SecurityKey);
                client.AddParameter("strOTP", strOTP);

                //oldID
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<SentOTPResult>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Common_ValidateOTP", ex.Message);
            }
            return result;
        }

        public static async Task<CheckMasterAccountExistResult> CheckMasterAccount(string refNo,int refType)
        {
            CheckMasterAccountExistResult result = new CheckMasterAccountExistResult();
            BaseClient client = new BaseClient();
            try
            {
                string TransactionRefNo = StringUtils.RandomString();
                // string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + Type + UserName + Password + FirstName + LastName + MobileNo + Email;
                // hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "Common_CheckMasterAccountExist";
                // client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("authSessionId", Cons.mMemberCredentials.LoginParams.authSessionId);
                client.AddParameter("LoginId", Cons.mMemberCredentials.LoginParams.strLoginId);
                client.AddParameter("RefType", refType);
                client.AddParameter("RefNo", refNo);
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<CheckMasterAccountExistResult>(strResult);

               

                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("CheckMasterAccount", ex.Message);
            }
            return result;
        }
    }
}
