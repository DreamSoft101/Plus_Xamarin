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
    public static class MasterData
    {
        public static async Task<MBB_GetData> GetData(DateTime lastGet, M_BBGetDataDeletedID delete)
        {
            MBB_GetData result = new MBB_GetData();
            BaseClient client = new BaseClient();
            try
            {
                string TransactionRefNo = StringUtils.RandomString();
                // string hash = Cons.APIid + Cons.APIKey + TransactionRefNo + Type + UserName + Password + FirstName + LastName + MobileNo + Email;
                // hash = EncryptUtils.getHash(hash);
                //Normal
                client.StrMethod = "BB_GetData";
                // client.AddParameter("Hash", hash);
                client.AddParameter("TransactionRefNo", TransactionRefNo);
                client.AddParameter("lastGet", lastGet.ToString("yyyy-MM-dd HH:mm:ss"));
                client.AddParameter("oldID", delete);
                //oldID
                string strResult = await client.getData();
                var data = JsonConvert.DeserializeObject<MBB_GetData>(strResult);
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetData", ex.Message);
            }
            return result;
        }

    }
}
