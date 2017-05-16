using Loyalty.Models;
using Loyalty.Models.ServiceOutput;
using Loyalty.Processing.Connections;
using Loyalty.Processing.LocalData;
using Loyalty.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Threads
{
    public class UserThreads
    {
        public delegate void onResult(ServiceResult result);
        public onResult OnResult;
        public async void Register(int Type, string UserName, string Token, string Password, string FirstName, string LastName, string MobileNo, string Email)
        {
            try
            {
                var result = await Task.Run(() => UserAccount.Register(Type,UserName,Token, Password, FirstName,LastName,MobileNo,Email));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.Mess = result.ResponseMessage;
                sResult.StatusCode = result.StatusCode;
                //return result;
                if (OnResult != null)
                {
                  OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Register", ex.Message);
            }
        }

        public async void Login(string UserName, string Token, string Password)
        {
            try
            {
                var result = await Task.Run(() => UserAccount.Login( UserName, Token, Password));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.Mess = result.ResponseMessage;
                sResult.StatusCode = result.StatusCode;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Login", ex.Message);
            }
        }

        public async void Preference()
        {
            try
            {
                var membergroups = await Task.Run(() => LocalData.GetMemberGroup());
                //ServiceResult sMemberGroup = new ServiceResult();
                //sMemberGroup.Data = membergroup;

                //if (OnResult != null)
                //{
                //    OnResult(sMemberGroup);
                //}


                var documents = await Task.Run(() => LocalData.getDocument());
                //ServiceResult sDocument = new ServiceResult();
                //sDocument.Data = document;
                //if (OnResult != null)
                //{
                //    OnResult(sDocument);
                //}

                var result = await Task.Run(() => LocalData.GetMemberType());
                //ServiceResult sResult = new ServiceResult();
                //sResult.Data = result;

                //if (OnResult != null)
                //{
                //    OnResult(sResult);
                //}

                var membergroupdetail = await Task.Run(() => LocalData.GetListMemberGroupDetail());

                List<ItemPreferences> lstResult = new List<ItemPreferences>();
                foreach(var item in result)
                {
                    var lstgroup = membergroupdetail.Where(p => p.MemberTypeID == item.MemberTypeID).ToList();
                    if (lstgroup.Count > 0)
                    {
                        foreach(var groupid in lstgroup)
                        {
                            var membergroup = membergroups.Where(p => p.MemberGroupID == groupid.MemberGroupID).FirstOrDefault();
                            ItemPreferences ip = new ItemPreferences();
                            ip.mMemberType = item;
                            ip.mMemberGroup = membergroup;
                            if (membergroup.idDocument != null)
                            {
                                var document = documents.Where(p => p.ID == membergroup.idDocument).FirstOrDefault();
                                if (document != null)
                                {
                                    ip.mDocument = document;
                                }
                            }
                            lstResult.Add(ip);
                        }
                    }
                    else
                    {
                        ItemPreferences ip = new ItemPreferences();
                        ip.mMemberType = item;
                        lstResult.Add(ip);
                    }
                }
                ServiceResult sResult = new ServiceResult();
                sResult.Data = lstResult;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Preference", ex.Message);
            }
        }


        public async void LinkToMasterAccount(string refNo, int refType)
        {
            try
            {
                var result = await Task.Run(() => UserAccount.LinkSSOAccountToMasterAccount(refNo,refType));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.Mess = result.ResponseMessage;
                sResult.StatusCode = result.StatusCode;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }


            }
            catch (Exception ex)
            {
                LogUtils.WriteError("LinkToMasterAccount", ex.Message);
            }
        }


        public async void GetMemberProfile(string auth, string loginid, bool loadSummary)
        {
            try
            {
                var result = await Task.Run(() => UserAccount.GetMemberProfile(auth, loginid));
                if (loadSummary)
                {
                    foreach (var item in result.AccountDetails)
                    {
                        item.Summary = await Task.Run(() => UserAccount.GetRewardAccountSummary(result.idMasterAccount, Cons.mMemberCredentials.LoginParams.authSessionId, item.idMember));
                    }
                }
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.Mess = result.ResponseMessage;
                sResult.StatusCode = result.StatusCode;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetMemberProfile", ex.Message);
            }
        }
        public async void GetMemberRdemptionHistory()
        {
            try
            {
                var result = await Task.Run(() => UserAccount.GetRedemptionHistory(Cons.mMemberCredentials.MemberProfile.idMasterAccount, Cons.mMemberCredentials.LoginParams.authSessionId));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.Mess = result.ResponseMessage;
                sResult.StatusCode = result.StatusCode;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetMemberRdemptionHistory", ex.Message);
            }
        }

        public async void GetRewardAccountSummary(Guid? idMember)
        {
            try
            {
                var result = await Task.Run(() => UserAccount.GetRewardAccountSummary(Cons.mMemberCredentials.MemberProfile.idMasterAccount, Cons.mMemberCredentials.LoginParams.authSessionId, idMember));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.Mess = result.ResponseMessage;
                sResult.StatusCode = result.StatusCode;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetRewardAccountSummary", ex.Message);
            }
        }

        public async void GetStatementFileList(Guid idMember)
        {
            try
            {
                var result = await Task.Run(() => UserAccount.GetStatementFileList(Cons.mMemberCredentials.MemberProfile.idMasterAccount, Cons.mMemberCredentials.LoginParams.authSessionId, idMember));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.Mess = result.ResponseMessage;
                sResult.StatusCode = result.StatusCode;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetStatementFileList", ex.Message);
            }
        }
    }
}
