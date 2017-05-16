using Loyalty.Models;
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
    public class AddCardThreads
    {
        public delegate void onResult(ServiceResult obj);
        public onResult OnResult;

        public async void Common_CheckMasterAccountExist( string RefNo, int RefType)
        {
            try
            {
                var result = await Task.Run(() => AddCardConnections.Common_CheckMasterAccountExist(RefNo, RefType));
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
                LogUtils.WriteError("Common_CheckMasterAccountExist", ex.Message);
            }
        }

        public async void Common_ValidatePlusMileCard(string RefNo, int RefType)
        {
            try
            {
                var result = await Task.Run(() => AddCardConnections.Common_ValidatePlusMileCard(RefNo, RefType));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.StatusCode = result.StatusCode;
                sResult.Mess = result.ResponseMessage;
                result.ResponseMessage = result.ResponseMessage;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Common_CheckMasterAccountExist", ex.Message);
            }
        }

        public async void SendOTP(long idMasterAccount, string mobile)
        {
            try
            {
                var result = await Task.Run(() => AddCardConnections.SendOTP(idMasterAccount, mobile));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.StatusCode = result.StatusCode;
                sResult.Mess = result.ResponseMessage;
                result.ResponseMessage = result.ResponseMessage;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("SendOTP", ex.Message);
            }
        }

        public async void ValidateOTP(MValidatePlusMileCard cardInfo, SentOTPResult otp, string strOTP)
        {
            try
            {
                var result = await Task.Run(() => AddCardConnections.ValidateOTP(cardInfo, otp, strOTP));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.StatusCode = result.StatusCode;
                sResult.Mess = result.ResponseMessage;
                result.ResponseMessage = result.ResponseMessage;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("SendOTP", ex.Message);
            }
        }

        public async void ValidateOTP(MasterAccountInfo info, SentOTPResult otp, string strOTP)
        {
            try
            {
                var result = await Task.Run(() => AddCardConnections.ValidateOTP(info, otp, strOTP));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.StatusCode = result.StatusCode;
                sResult.Mess = result.ResponseMessage;
                result.ResponseMessage = result.ResponseMessage;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("SendOTP", ex.Message);
            }
        }

        public async void CheckMasterAccount(string RefNo, int RefType)
        {
            try
            {
                var result = await Task.Run(() => AddCardConnections.CheckMasterAccount(RefNo, RefType));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.StatusCode = result.StatusCode;
                sResult.Mess = result.ResponseMessage;
                result.ResponseMessage = result.ResponseMessage;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("CheckMasterAccount", ex.Message);
            }
        }
    }
}
