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
    public class Contents
    {
        //GetAnnouncement
        //GetAnnouncement(Guid APIid, string Hash, string TransactionRefNo, string idAnnouncement, byte DetailFlg, string Language, byte? intPreviewMode)
        public static async Task<MGetAnnouncement> GetAnnouncement(string idAnnouncement, byte DetailFlg, string Language, byte? intPreviewMode)
        {
            MGetAnnouncement result = new MGetAnnouncement();
            try
            {
                BaseClient client = new BaseClient();
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + idAnnouncement + DetailFlg + Language + intPreviewMode;
                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "GetAnnouncement";

                client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("idAnnouncement", idAnnouncement);
                client.AddParameter("DetailFlg", DetailFlg);
                Language = string.IsNullOrEmpty(Language) ? "EN" : Language;
                client.AddParameter("Language", Language);
                client.AddParameter("intPreviewMode", intPreviewMode);
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MGetAnnouncement>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetAnnouncement", ex.Message);
            }
            return result;
        }


        //GetPortalMenu(Guid APIid,string Hash, string TransactionRefNo, byte intPreviewMode)
        //listCheckhash.Add("GetPortalMenu".ToUpper(), new string[] { "TransactionRefNo","intPreviewMode" });
        public static async Task<MGetMenuPortal> GetPortalMenu(byte intPreviewMode)
        {
            MGetMenuPortal result = new MGetMenuPortal();
            try
            {
                BaseClient client = new BaseClient();
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + intPreviewMode;
                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "GetPortalMenu";

                client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("intPreviewMode", intPreviewMode);
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MGetMenuPortal>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetAnnouncement", ex.Message);
            }
            return result;
        }


        //GetPortalContent(Guid APIid, string Hash, string TransactionRefNo, Guid? idPortalMenu, int? idPortalContent, string strContentName, int? Flag, byte? intPreviewMode)
        //listCheckhash.Add("GetPortalContent".ToUpper(), new string[] { "TransactionRefNo", "idPortalMenu", "idPortalContent", "strContentName", "Flag", "intPreviewMode" });
        public static async Task<MGetPortalMenu> GetPortalContent(Guid? idPortalMenu, int? idPortalContent, string strContentName, int? Flag, byte? intPreviewMode)
        {
            MGetPortalMenu result = new MGetPortalMenu();
            try
            {
                BaseClient client = new BaseClient();
                string TransactionRefNo = StringUtils.RandomString();
                string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + idPortalMenu + idPortalContent + strContentName + Flag + intPreviewMode;
                hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "GetPortalContent";

                client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("idPortalMenu", idPortalMenu == null ? "" : idPortalMenu.ToString());
                client.AddParameter("idPortalContent", idPortalContent == null ? "" : idPortalContent.ToString());
                client.AddParameter("strContentName", strContentName);
                client.AddParameter("Flag", Flag);
                client.AddParameter("intPreviewMode", intPreviewMode);
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MGetPortalMenu>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetAnnouncement", ex.Message);
            }
            return result;
        }


        public static async Task<MGetFAQ> GetFaq(string idFAQ, int DetailFlg, string Language, byte? intPreviewMode)
        {
            MGetFAQ result = new MGetFAQ();
            try
            {
                BaseClient client = new BaseClient();
                string TransactionRefNo = StringUtils.RandomString();
               // string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + idPortalMenu + idPortalContent + strContentName + Flag + intPreviewMode;
                //hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "GetFAQ";

                client.AddParameter("Hash", "");
                client.AddParameter("TransactionRefNo", TransactionRefNo);

                if (idFAQ != null)
                {
                    client.AddParameter("idFAQ", idFAQ);
                }
                
                client.AddParameter("DetailFlg", DetailFlg);
                client.AddParameter("Language", Language);
                if (intPreviewMode != null)
                {
                    client.AddParameter("intPreviewMode", intPreviewMode);
                }
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MGetFAQ>(strResult);
                if (data != null)
                {
                    data.FAQ = data.FAQ.OrderBy(p => p.SequenceOfQuestion).ToArray();
                }
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetFaq", ex.Message);
            }
            return result;
        }
    }
}
