using Loyalty.Models.ServiceOutput;
using Loyalty.Processing.LocalData;
using Loyalty.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Threads
{
    public class FavoriteThreads
    {
        public delegate void onResult(ServiceResult result);
        public onResult OnResult;

        public async void GetListByType(int type)
        {
            try
            {
                var result = await Task.Run(() => LocalData.getListFavoriteByType(type));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetListByType", ex.Message);
            }
        }

        public async void insertFavorite(int type,Guid idObject)
        {
            try
            {
                var result = await Task.Run(() => LocalData.insertFavorite(type,idObject));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("insertFavorite", ex.Message);
            }
        }

        public async void deleteFavorite(Guid id)
        {
            try
            {
                var result = await Task.Run(() => LocalData.deleteFavorite(id));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("insertFavorite", ex.Message);
            }
        }

        public async void AddRecent(Guid ID, int Type)
        {
            try
            {
                await Task.Run(() => LocalData.AddRecent(ID, Type));
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("AddRecent", ex.Message);
            }
        }
    }
}
