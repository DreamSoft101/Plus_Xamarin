using Loyalty.Models.ServiceOutput;
using Loyalty.Processing.Connections;
using Loyalty.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Threads
{
    public class ContentThreads
    {
        public delegate void onResult(ServiceResult obj);
        public onResult OnResult;

        public async void GetPortalMenu(byte intPreviewMode = 0)
        {
            try
            {
                var result = await Task.Run(() => Contents.GetPortalMenu(intPreviewMode));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.StatusCode = result.StatusCode;
                result.ResponseMessage = result.ResponseMessage;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("RedemptionHomePage", ex.Message);
            }
        }

        public async void GetPortalContent(Guid? idMenu, int? idPortalContent, int Flag = 1, byte intPreviewMode = 0)
        {
            try
            {
                var result = await Task.Run(() => Contents.GetPortalContent(idMenu, idPortalContent,null,Flag,intPreviewMode));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.StatusCode = result.StatusCode;
                result.ResponseMessage = result.ResponseMessage;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("RedemptionHomePage", ex.Message);
            }
        }

        public async void GetFaq(string idFAQ, int DetailFlg, string Language, byte? intPreviewMode)
        {
            try
            {
                var result = await Task.Run(() => Contents.GetFaq( idFAQ,  DetailFlg,  Language, intPreviewMode));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.StatusCode = result.StatusCode;
                result.ResponseMessage = result.ResponseMessage;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetFaq", ex.Message);
            }
        }
    }
}
