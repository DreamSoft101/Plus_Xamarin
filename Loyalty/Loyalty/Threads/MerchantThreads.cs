using Loyalty.Models.Database;
using Loyalty.Models.ServiceOutput;
using Loyalty.Processing.Connections;
using Loyalty.Processing.LocalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Threads
{
    public class MerchantThreads
    {
        public delegate void onResult(ServiceResult result);
        public onResult OnResult;
        public async void Raiting(Guid idProduct, int Rating, string Comment)
        {
            try
            {
                var result = await Task.Run(() => Processing.Connections.MerchantConnection.Rating(idProduct, Rating, Comment));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.Mess = result.ResponseMessage;
                sResult.StatusCode = result.StatusCode;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async void GetCommentAndRating(Guid idProduct)
        {
            try
            {
                var result = await Task.Run(() => Processing.Connections.MerchantConnection.GetCommentsAndRating(idProduct));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.Mess = result.ResponseMessage;
                sResult.StatusCode = result.StatusCode;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async void HomMerchantOffer()
        {
            try
            {
                var offers = await Task.Run(() => LocalData.getMerchantProductMemberType());
                ServiceResult soffers = new ServiceResult();
                soffers.Data = offers;
                if (OnResult != null)
                {
                    OnResult(soffers);
                }

                var merchant = await Task.Run(() => LocalData.getMerchant());
                ServiceResult smerchant = new ServiceResult();
                smerchant.Data = merchant;
                if (OnResult != null)
                {
                    OnResult(smerchant);
                }

                var favorite = await Task.Run(() => LocalData.getListFavoriteByType( Favorites.intMerchantProduct));
                ServiceResult sfavorite = new ServiceResult();
                sfavorite.Data = favorite;
                if (OnResult != null)
                {
                    OnResult(sfavorite);
                }

                var document = await Task.Run(() => LocalData.getDocument());
                ServiceResult sdocument = new ServiceResult();
                sdocument.Data = document;
                if (OnResult != null)
                {
                    OnResult(sdocument);
                }

                var location = await Task.Run(() => LocalData.getMerchantLocation());
                ServiceResult slocation = new ServiceResult();
                slocation.Data = location;
                if (OnResult != null)
                {
                    OnResult(slocation);
                }

                var membergroup = await Task.Run(() => LocalData.GetMemberGroup());
                ServiceResult smembergroup = new ServiceResult();
                smembergroup.Data = membergroup;
                if (OnResult != null)
                {
                    OnResult(smembergroup);
                }

                var membergroupdetail = await Task.Run(() => LocalData.GetListMemberGroupDetail());
                ServiceResult smembergroupdetail = new ServiceResult();
                smembergroupdetail.Data = membergroupdetail;
                if (OnResult != null)
                {
                    OnResult(smembergroupdetail);
                }


                var result = await Task.Run(() => LocalData.getListMerchantProduct());
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
                
            }
            catch (Exception ex)
            {

            }
        }

        public async void FavoriteMerchantOffer()
        {
            try
            {
                var offers = await Task.Run(() => LocalData.getMerchantProductMemberType());
                ServiceResult soffers = new ServiceResult();
                soffers.Data = offers;
                if (OnResult != null)
                {
                    OnResult(soffers);
                }

                var merchant = await Task.Run(() => LocalData.getMerchant());
                ServiceResult smerchant = new ServiceResult();
                smerchant.Data = merchant;
                if (OnResult != null)
                {
                    OnResult(smerchant);
                }

                var favorite = await Task.Run(() => LocalData.getListFavoriteByType(Favorites.intMerchantProduct));
                ServiceResult sfavorite = new ServiceResult();
                sfavorite.Data = favorite;
                if (OnResult != null)
                {
                    OnResult(sfavorite);
                }

                var document = await Task.Run(() => LocalData.getDocument());
                ServiceResult sdocument = new ServiceResult();
                sdocument.Data = document;
                if (OnResult != null)
                {
                    OnResult(sdocument);
                }

                var location = await Task.Run(() => LocalData.getMerchantLocation());
                ServiceResult slocation = new ServiceResult();
                slocation.Data = location;
                if (OnResult != null)
                {
                    OnResult(slocation);
                }

                var membergroup = await Task.Run(() => LocalData.GetMemberGroup());
                ServiceResult smembergroup = new ServiceResult();
                smembergroup.Data = membergroup;
                if (OnResult != null)
                {
                    OnResult(smembergroup);
                }

                var membergroupdetail = await Task.Run(() => LocalData.GetListMemberGroupDetail());
                ServiceResult smembergroupdetail = new ServiceResult();
                smembergroupdetail.Data = membergroupdetail;
                if (OnResult != null)
                {
                    OnResult(smembergroupdetail);
                }

                var result = await Task.Run(() => LocalData.getListMerchantProductFavorite());
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async void RecentMerchantOffer()
        {
            try
            {
                var offers = await Task.Run(() => LocalData.getMerchantProductMemberType());
                ServiceResult soffers = new ServiceResult();
                soffers.Data = offers;
                if (OnResult != null)
                {
                    OnResult(soffers);
                }

                var merchant = await Task.Run(() => LocalData.getMerchant());
                ServiceResult smerchant = new ServiceResult();
                smerchant.Data = merchant;
                if (OnResult != null)
                {
                    OnResult(smerchant);
                }

                var favorite = await Task.Run(() => LocalData.getListFavoriteByType(Favorites.intMerchantProduct));
                ServiceResult sfavorite = new ServiceResult();
                sfavorite.Data = favorite;
                if (OnResult != null)
                {
                    OnResult(sfavorite);
                }

                var document = await Task.Run(() => LocalData.getDocument());
                ServiceResult sdocument = new ServiceResult();
                sdocument.Data = document;
                if (OnResult != null)
                {
                    OnResult(sdocument);
                }

                var location = await Task.Run(() => LocalData.getMerchantLocation());
                ServiceResult slocation = new ServiceResult();
                slocation.Data = location;
                if (OnResult != null)
                {
                    OnResult(slocation);
                }
                var membergroup = await Task.Run(() => LocalData.GetMemberGroup());
                ServiceResult smembergroup = new ServiceResult();
                smembergroup.Data = membergroup;
                if (OnResult != null)
                {
                    OnResult(smembergroup);
                }

                var membergroupdetail = await Task.Run(() => LocalData.GetListMemberGroupDetail());
                ServiceResult smembergroupdetail = new ServiceResult();
                smembergroupdetail.Data = membergroupdetail;
                if (OnResult != null)
                {
                    OnResult(smembergroupdetail);
                }

                var result = await Task.Run(() => LocalData.getListMerchantProductRecent());
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}

//getListMerchantProductRecent