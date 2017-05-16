using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using EXPRESSO.Processing.Connections;
using EXPRESSO.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Threads
{
    public class UsersThreads
    {
        public delegate void onLoginResult(UserInfos user);
        public onLoginResult OnLoginResult;

        public delegate void onFogetResult(bool result);
        public onFogetResult OnForgetResult;

        public delegate void onChangePasswordResult(UserInfos userinfo);
        public onChangePasswordResult OnChangePasswordResult;

       
        /// <summary>
        /// ChangePassword and result return to event OnChangePasswordResult
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="strSession"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task doChangePassword(string idUser, string strSession, string oldPassword, string newPassword)
        {
            try
            {
                var result = await Task.Run(() => UserAccountConnection.ChangePassword(idUser, strSession, oldPassword, newPassword));
                //return result;
                if (OnChangePasswordResult != null)
                {
                    OnChangePasswordResult(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("doChangePassword", ex.Message);
            }
            
        }


        public delegate void onGetListEntitiesResult(ServiceResult result);
        public onGetListEntitiesResult OnGetListEntitiesResult;
        public async void loadGetListEntities()
        {
            try
            {
                var result = await Task.Run(() => UserAccountConnection.GetListEntities());
                //return result;
                if (OnGetListEntitiesResult != null)
                {
                    OnGetListEntitiesResult(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadGetListEntities", ex.Message);
            }
           
        }


        public delegate void onLoadAPIKey(ServiceResult entity);
        public onLoadAPIKey OnLoadAPIKey;
        public delegate void onLoginSusscess(ServiceResult entity, ServiceResult user);
        public onLoginSusscess OnLoginSusscess;
        public async Task doLogin(string idEntity, string strUserName, string strPassword)
        {
            try
            {
                var result = await Task.Run(() => UserAccountConnection.getAPIKey(idEntity));
                if (OnLoadAPIKey != null)
                {
                    OnLoadAPIKey(result);
                }

                if (result.intStatus == 1)
                {
                    var resultLogin = await Task.Run(() => UserAccountConnection.Login(strUserName, strPassword, result.Data as MyEntity));
                    if (OnLoginSusscess != null)
                    {
                        OnLoginSusscess(result, resultLogin);
                    }
                }
                else
                {
                    if (OnLoginSusscess != null)
                    {
                        OnLoginSusscess(result, null);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("doLogin", ex.Message);
            }
            
        }

        public async void loadEntityInfo(string idEntity)
        {
            try
            {
                var result = await Task.Run(() => UserAccountConnection.getAPIKey(idEntity));
                if (OnLoadAPIKey != null)
                {
                    OnLoadAPIKey(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("loadEntityInfo", ex.Message);
            }

            
        }

        public delegate void onRegisterSusscess(ServiceResult entity, ServiceResult user);
        public onRegisterSusscess OnRegisterSusscess;
        public async Task doRegister(string idEntity, string strUserName, string strPassword,string strMobile,string strFirstName, string strLastName)
        {
            try
            {

                var result = await Task.Run(() => UserAccountConnection.getAPIKey(idEntity));
                if (OnLoadAPIKey != null)
                {
                    OnLoadAPIKey(result);
                }
                if (result.intStatus == 1)
                {
                    var resultRegister = await Task.Run(() => UserAccountConnection.Register(strUserName, strPassword, strMobile, strFirstName, strLastName, result.Data as MyEntity));
                    if (OnRegisterSusscess != null)
                    {
                        OnRegisterSusscess(result, resultRegister);
                    }
                }
                else
                {
                    if (OnRegisterSusscess != null)
                    {
                        OnRegisterSusscess(result, null);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("doRegister", ex.Message);
            }

        }

        public delegate void onForgetpassword(ServiceResult result);
        public onForgetpassword OnForgetpassword;
        public async Task doForgetpassword(string strEmail, string idEntity)
        {
            try
            {
                var result = await Task.Run(() => UserAccountConnection.getAPIKey(idEntity));

                if (result.intStatus == 1)
                {
                    MyEntity entity = result.Data as MyEntity;
                    var resultForget = await Task.Run(() => UserAccountConnection.MemberForgetpassword(strEmail, entity));
                    if (OnForgetpassword != null)
                    {
                        OnForgetpassword(resultForget);
                    }
                }
                else
                {
                    if (OnForgetpassword != null)
                    {
                        OnForgetpassword(result);
                    }
                }

               
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("doForgetpassword", ex.Message);
            }

        }


        public delegate void onLoginSocial(ServiceResult result);
        public onLoginSocial OnLoginSocial;

        public async Task doLoginSocial(string idEntity , string provider, string token)
        {
            try
            {
                var resultAPKKey = await Task.Run(() => UserAccountConnection.getAPIKey(idEntity));
                if (OnLoadAPIKey != null)
                {
                    OnLoadAPIKey(resultAPKKey);
                }

                var result = await Task.Run(() => UserAccountConnection.LoginSocial(provider, token, resultAPKKey));
                if (OnLoginSusscess != null)
                {
                    OnLoginSusscess(resultAPKKey,result);
                }

            }
            catch (Exception ex)
            {
                LogUtils.WriteError("doLoginSocial", ex.Message);
            }

        }

        public delegate void onGetDomainSetting(ServiceResult result);
        public onGetDomainSetting OnGetDomainSetting;
        public async Task getDomainSetting(string domain)
        {
            try
            {
                var resultAPKKey = await Task.Run(() => UserAccountConnection.getEntityByDomain(domain));
                if (OnGetDomainSetting != null)
                {
                    OnGetDomainSetting(resultAPKKey);
                }

            }
            catch (Exception ex)
            {
                LogUtils.WriteError("doLoginSocial", ex.Message);
            }
           
        }
    }
}
