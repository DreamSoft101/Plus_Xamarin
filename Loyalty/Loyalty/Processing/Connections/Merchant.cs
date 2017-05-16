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
    public class MerchantConnection
    {
        public static async Task<MBB_Rating> Rating(Guid idProduct, int Rating, string Comment)
        {
            MBB_Rating result = new MBB_Rating();
            BaseClient client = new BaseClient();
            try
            {
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + Cons.mMemberCredentials.LoginParams.strLoginId + Cons.mMemberCredentials.LoginParams.authSessionId + idProduct + Rating + Comment;

                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "BB_Rating";
                 client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("authSessionId", Cons.mMemberCredentials.LoginParams.authSessionId);
                client.AddParameter("LoginId", Cons.mMemberCredentials.LoginParams.strLoginId);
                client.AddParameter("Rating", Rating);
                client.AddParameter("MerchantProductID", idProduct);
                client.AddParameter("Comment", Comment);
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MBB_Rating>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetData", ex.Message);
            }
            return result;
        }


        public static async Task<MCommon_GetMerchantProductRatings> GetCommentsAndRating(Guid MerchantProductID, int pageSize = 9999, int pageIndex = 0)
        {
            MCommon_GetMerchantProductRatings result = new MCommon_GetMerchantProductRatings();
            BaseClient client = new BaseClient();
            try
            {
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + MerchantProductID + pageSize + pageIndex + (Cons.mMemberCredentials != null ? Cons.mMemberCredentials.LoginParams.strLoginId : "") + (Cons.mMemberCredentials != null ? Cons.mMemberCredentials.LoginParams.authSessionId : "");

                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "Common_GetMerchantProductRatings";
                client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("MerchantProductID", MerchantProductID);
                client.AddParameter("pageSize", pageSize);
                client.AddParameter("pageIndex", pageIndex);

                if (Cons.mMemberCredentials != null)
                {
                    client.AddParameter("LoginId", Cons.mMemberCredentials.LoginParams.strLoginId);
                    client.AddParameter("authSessionId", Cons.mMemberCredentials.LoginParams.authSessionId);
                }

                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MCommon_GetMerchantProductRatings>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetComments", ex.Message);
            }
            return result;
        }
    }
}
