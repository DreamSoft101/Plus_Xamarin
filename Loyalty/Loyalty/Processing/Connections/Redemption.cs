using Loyalty.Models;
using Loyalty.Models.Database;
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
    public static class Redemption
    {
        public static async Task<MMemberRedeem> Redeem(List<MemberRedeemInfoProduct> items, MDeliveryInfo address)
        {
            MMemberRedeem result = new MMemberRedeem();
            try
            {
                BaseClient client = new BaseClient();
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo;// + authSessionId + idMasterAccount + MemberID.ToString();// + Cons.mMemberCredentials.LoginParams.authSessionId + Cons.mMemberCredentials.LoginParams.strLoginId;
                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "MemberRedeem";
                client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("TotalPointsRedeem", items.Sum(p => p.Quantity * p.Points));
                client.AddParameter("TotalCashRedeem", 0);
                client.AddParameter("idMasterAccount", Cons.mMemberCredentials.MemberProfile.idMasterAccount);
                client.AddParameter("authSessionId", Cons.mMemberCredentials.LoginParams.authSessionId);
                client.AddParameter("RedemptionProducts", items);
                client.AddParameter("DeliveryInfomation", address);
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MMemberRedeem>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Redeem", ex.Message);
            }
            return result;
        }

        public static async Task<MGetRedemptionDetails> GetRedemptionDetail(Guid idRedemption)
        {
            MGetRedemptionDetails result = new MGetRedemptionDetails();
            try
            {
                BaseClient client = new BaseClient();
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + idRedemption.ToString() + Cons.mMemberCredentials.LoginParams.authSessionId;// + authSessionId + idMasterAccount + MemberID.ToString();// + Cons.mMemberCredentials.LoginParams.authSessionId + Cons.mMemberCredentials.LoginParams.strLoginId;
                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "GetRedemptionDetails";
                client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("idRedemption", idRedemption);
                client.AddParameter("authSessionId", Cons.mMemberCredentials.LoginParams.authSessionId);
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MGetRedemptionDetails>(strResult);
                data.RedeemProduct.Clear();
                dynamic jsondata = JsonConvert.DeserializeObject(strResult);
                foreach (var product in jsondata.RedeemProduct)
                {
                    if (product.TrackingNumber != null)
                    {
                        MRedemptionDetailPhysic item = JsonConvert.DeserializeObject<MRedemptionDetailPhysic>(product.ToString());
                        data.RedeemProduct.Add(item);
                    }
                    else if (product.strVoucherNo != null)
                    {
                        MRedemptionDetailVoucher item = JsonConvert.DeserializeObject<MRedemptionDetailVoucher>(product.ToString());
                        data.RedeemProduct.Add(item);
                    }
                    else if (product.FeeAmount != null)
                    {
                        MRedemptionDetailFee item = JsonConvert.DeserializeObject<MRedemptionDetailFee>(product.ToString());
                        data.RedeemProduct.Add(item);
                    }
                    else if (product.xmlInfo != null)
                    {
                        MRedemptionDetailExchange item = JsonConvert.DeserializeObject<MRedemptionDetailExchange>(product.ToString());
                        data.RedeemProduct.Add(item);
                    }
                    else 
                    {
                        MRedemptionInstant item = JsonConvert.DeserializeObject<MRedemptionInstant>(product.ToString());
                        data.RedeemProduct.Add(item);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Redeem", ex.Message);
            }
            return result;
        }

        public static async Task<BB_MUseVoucher> UseEvoucher(string voucherno, byte status, DateTime date)
        {
            BB_MUseVoucher result = new BB_MUseVoucher();
            try
            {
                BaseClient client = new BaseClient();
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + Cons.mMemberCredentials.LoginParams.strLoginId + Cons.mMemberCredentials.LoginParams.authSessionId + voucherno + status ;// + authSessionId + idMasterAccount + MemberID.ToString();// + Cons.mMemberCredentials.LoginParams.authSessionId + Cons.mMemberCredentials.LoginParams.strLoginId;
                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "BB_UseVoucherNo";
                client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("LoginId", Cons.mMemberCredentials.LoginParams.strLoginId);
                client.AddParameter("authSessionId", Cons.mMemberCredentials.LoginParams.authSessionId);
                client.AddParameter("voucherNo", voucherno);
                client.AddParameter("status", status);
                client.AddParameter("date", date);
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<BB_MUseVoucher>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Redeem", ex.Message);
            }
            return result;
        }

        public static async Task<MBB_GetListEVoucher> GetListEVoucher(int page, int pagesize)
        {
            MBB_GetListEVoucher result = new MBB_GetListEVoucher();
            try
            {
                BaseClient client = new BaseClient();
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + Cons.mMemberCredentials.LoginParams.strLoginId + Cons.mMemberCredentials.LoginParams.authSessionId + page + pagesize;// + authSessionId + idMasterAccount + MemberID.ToString();// + Cons.mMemberCredentials.LoginParams.authSessionId + Cons.mMemberCredentials.LoginParams.strLoginId;
                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "BB_GetListEVoucher";
                client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("LoginId", Cons.mMemberCredentials.LoginParams.strLoginId);
                client.AddParameter("authSessionId", Cons.mMemberCredentials.LoginParams.authSessionId);
                client.AddParameter("page", page);
                client.AddParameter("pageSize", pagesize);
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MBB_GetListEVoucher>(strResult);

                if (data.StatusCode == 1)
                {
                    var document = LocalData.LocalData.getDocument();
                   // data.ListVoucher = data.ListVoucher.Where(p => p.intStatus.Value != 4);
                    //var product = LocalData.LocalData.getListRedemptionProduct(null, null);
                    foreach (var item in data.ListVoucher)
                    {
                        var redemptionproduct = LocalData.LocalData.GetRedemptionProduct(item.RedemptionProductId.Value);
                        if (redemptionproduct.ImageID != null)
                        {
                            var doc = document.Where(p => p.ID == redemptionproduct.ImageID.Value).FirstOrDefault();
                            if (doc != null)
                            {
                                item.strImgPicture = doc.FileName;
                            }
                           
                        }
                        item.intStatus = item.intStatus == null ? (byte)0 : item.intStatus.Value;
                        item.strName = redemptionproduct.ProductName;
                    }


                }
                data.ListVoucher = data.ListVoucher.Where(p => p.intStatus.Value != 4).ToList();
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("BB_GetListEVoucher", ex.Message);
            }
            return result;
        }
    }
}
