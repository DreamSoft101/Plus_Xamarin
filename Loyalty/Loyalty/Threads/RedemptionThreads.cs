using Loyalty.Models;
using Loyalty.Models.Database;
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
    public class RedemptionThreads
    {
        public delegate void onResult(ServiceResult obj);
        public onResult OnResult;

        public async void RedemptionHomePage(Guid? MemberTypeID = null)
        {
            try
            {
                var document = await Task.Run(() => LocalData.getDocument());
                ServiceResult sDocument = new ServiceResult();
                sDocument.Data = document;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sDocument);
                }

                var redemption = await Task.Run(() => LocalData.getListRedemptionProduct(MemberTypeID, null));
                ServiceResult sRedemption = new ServiceResult();
                sRedemption.Data = redemption;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sRedemption);
                }


                var redemptiondetail = await Task.Run(() => LocalData.getListRedemptionProductDetail(MemberTypeID, null));
                ServiceResult sDetail = new ServiceResult();
                sDetail.Data = redemptiondetail;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sDetail);
                }

                var redemptioncategory = await Task.Run(() => LocalData.getListRedemptionCategory());
                ServiceResult sRdemptioncategory = new ServiceResult();
                sRdemptioncategory.Data = redemptioncategory;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sRdemptioncategory);
                }

            }
            catch (Exception ex)
            {
                LogUtils.WriteError("RedemptionHomePage", ex.Message);
            }
        }

        public async void GetEVoucher(Guid? MemberTypeID = null)
        {
            try
            {
                var document = await Task.Run(() => LocalData.getDocument());
                ServiceResult sDocument = new ServiceResult();
                sDocument.Data = document;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sDocument);
                }

                var redemption = await Task.Run(() => LocalData.GetEvoucher(MemberTypeID));
                ServiceResult sRedemption = new ServiceResult();
                sRedemption.Data = redemption;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sRedemption);
                }


                var redemptiondetail = await Task.Run(() => LocalData.getListRedemptionProductDetail(MemberTypeID, null));
                ServiceResult sDetail = new ServiceResult();
                sDetail.Data = redemptiondetail;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sDetail);
                }

                var redemptioncategory = await Task.Run(() => LocalData.getListRedemptionCategory());
                ServiceResult sRdemptioncategory = new ServiceResult();
                sRdemptioncategory.Data = redemptioncategory;
                //return result;
                if (OnResult != null)
                {
                    OnResult(sRdemptioncategory);
                }

            }
            catch (Exception ex)
            {
                LogUtils.WriteError("RedemptionHomePage", ex.Message);
            }
        }

        public async void AddProductToCart(MemberRedeemInfoProduct item)
        {
            try
            {
                await Task.Run(() => LocalData.AddProductToCart(item));
                if (OnResult != null)
                {
                    OnResult(null);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("AddProductToCart", ex.Message);
            }
        }

        public async void RemoveProductFromCart(MemberRedeemInfoProduct item, bool isDelete  = false)
        {
            try
            {
                await Task.Run(() => LocalData.RemoveProductFromCart(item , isDelete));
                if (OnResult != null)
                {
                    OnResult(null);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("RemoveProductFromCart", ex.Message);
            }
        }

        public async void FragmentCart( )
        {
            try
            {
                if (Cons.mMemberCredentials != null)
                {
                    if (Cons.mMemberCredentials.MemberProfile != null)
                    {
                        var accountsumarry = await Task.Run(() => UserAccount.GetRewardAccountSummary(Cons.mMemberCredentials.MemberProfile.idMasterAccount, Cons.mMemberCredentials.LoginParams.authSessionId, null));
                        ServiceResult saccountsumarry = new ServiceResult();
                        saccountsumarry.Data = accountsumarry;
                        saccountsumarry.Mess = accountsumarry.ResponseMessage;
                        saccountsumarry.StatusCode = accountsumarry.StatusCode;
                        //return result;
                        if (OnResult != null)
                        {
                            OnResult(saccountsumarry);
                        }
                    }
                    
                }
               


                var cart = await Task.Run(() => LocalData.GetCart());
                ServiceResult sCart = new ServiceResult();
                sCart.Data = cart;
                if (OnResult != null)
                {
                    OnResult(sCart);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("RemoveProductFromCart", ex.Message);
            }
        }

        public delegate void onGetCartCount(int count);
        public onGetCartCount OnGetCartCount;
        public async void GetCartCount()
        {
            try
            {
                var cart = await Task.Run(() => LocalData.GetCart().Sum(p => p.Quantity));
               if (OnGetCartCount != null)
                {
                    OnGetCartCount(cart);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Redeem", ex.Message);
            }
        }

        public async void Redeem(MDeliveryInfo address)
        {
            try
            {
                var cart = await Task.Run(() => LocalData.GetCart());
                var result = await Task.Run(() => Redemption.Redeem(cart, address));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.StatusCode = result.StatusCode;
                sResult.Mess = result.ResponseMessage;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Redeem", ex.Message);
            }
        }

        public async void UnLockEVoucher(int  id)
        {
            try
            {
                await Task.Run(() => LocalData.UnLockEVoucher(id));
                if (OnResult != null)
                {
                    OnResult(null);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("Redeem", ex.Message);
            }
        }

        public async void RedemptionDetail(Guid idRedemption)
        {
            try
            {
                var result = await Task.Run(() => Redemption.GetRedemptionDetail(idRedemption));
                ServiceResult sResult = new ServiceResult();
                sResult.Data = result;
                sResult.StatusCode = result.StatusCode;
                sResult.Mess = result.ResponseMessage;
                if (OnResult != null)
                {
                    OnResult(sResult);
                }

                var product = await Task.Run(() => LocalData.getListRedemptionProduct(null,null));
                ServiceResult sResultProduct = new ServiceResult();
                sResultProduct.Data = product;
                sResultProduct.StatusCode = 1;
                if (OnResult != null)
                {
                    OnResult(sResultProduct);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("RedemptionDetail", ex.Message);
            }
        }

        public async void EVoucherDetail(MRedemptionDetailVoucher data)
        {
            try
            {
                var product = await Task.Run(() => LocalData.GetRedemptionProduct(new Guid(data.RedemptionProductId)));
                ServiceResult sPoduct = new ServiceResult();
                sPoduct.Data = product;
                if (OnResult != null)
                {
                    OnResult(sPoduct);
                }

                var document = await Task.Run(() => LocalData.GetDocument(product.ImageID));
                ServiceResult sDocument = new ServiceResult();
                sDocument.Data = document;
                if (OnResult != null)
                {
                    OnResult(sDocument);
                }



                var detail = await Task.Run(() => LocalData.GetRedemptionProductDetail(new Guid(data.RedemptionProductDetailId)));
                ServiceResult sDetail = new ServiceResult();
                sDetail.Data = detail;
                if (OnResult != null)
                {
                    OnResult(sDetail);
                }

                ServiceResult sFinish = new ServiceResult();
                sFinish.Data = "FINISH";
                if (OnResult != null)
                {
                    OnResult(sFinish);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("EVoucherDetail", ex.Message);
            }
        }

        public async void EVoucherDetail(MemberVoucherInfo data)
        {
            try
            {
                var product = await Task.Run(() => LocalData.GetRedemptionProduct(data.RedemptionProductId.Value));
                ServiceResult sPoduct = new ServiceResult();
                sPoduct.Data = product;
                if (OnResult != null)
                {
                    OnResult(sPoduct);
                }

                var document = await Task.Run(() => LocalData.GetDocument(product.ImageID));
                ServiceResult sDocument = new ServiceResult();
                sDocument.Data = document;
                if (OnResult != null)
                {
                    OnResult(sDocument);
                }



                var detail = await Task.Run(() => LocalData.GetRedemptionProductDetail(data.idRedemptionProductDetail.Value));
                ServiceResult sDetail = new ServiceResult();
                sDetail.Data = detail;
                if (OnResult != null)
                {
                    OnResult(sDetail);
                }

                ServiceResult sFinish = new ServiceResult();
                sFinish.Data = "FINISH";
                if (OnResult != null)
                {
                    OnResult(sFinish);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("EVoucherDetail", ex.Message);
            }
        }


        public async void PhysicalDetail(MRedemptionDetailPhysic data)
        {
            try
            {
                var product = await Task.Run(() => LocalData.GetRedemptionProduct(new Guid(data.RedemptionProductId)));
                ServiceResult sPoduct = new ServiceResult();
                sPoduct.Data = product;
                if (OnResult != null)
                {
                    OnResult(sPoduct);
                }

                var document = await Task.Run(() => LocalData.GetDocument(product.ImageID));
                ServiceResult sDocument = new ServiceResult();
                sDocument.Data = document;
                if (OnResult != null)
                {
                    OnResult(sDocument);
                }

                try
                {
                    var detail = await Task.Run(() => LocalData.GetRedemptionProductDetail(new Guid(data.RedemptionProductDetailId)));
                    ServiceResult sDetail = new ServiceResult();
                    sDetail.Data = detail;
                    if (OnResult != null)
                    {
                        OnResult(sDetail);
                    }
                }
                catch (Exception ex)
                {

                }

               

                ServiceResult sFinish = new ServiceResult();
                sFinish.Data = "FINISH";
                if (OnResult != null)
                {
                    OnResult(sFinish);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("PhysicalDetail", ex.Message);
            }
        }

        public async void EVoucherDetail(Guid  id)
        {
            try
            {
                var product = await Task.Run(() => LocalData.GetRedemptionProduct(id));
                ServiceResult sPoduct = new ServiceResult();
                sPoduct.Data = product;
                if (OnResult != null)
                {
                    OnResult(sPoduct);
                }

                var document = await Task.Run(() => LocalData.GetDocument(product.ImageID));
                ServiceResult sDocument = new ServiceResult();
                sDocument.Data = document;
                if (OnResult != null)
                {
                    OnResult(sDocument);
                }


            }
            catch (Exception ex)
            {
                LogUtils.WriteError("EVoucherDetail", ex.Message);
            }
        }

        public async void AuthencationEVoucher(MemberRedeemInfoProduct item)
        {
            try
            {
                var product = await Task.Run(() => LocalData.GetRedemptionProduct(item.RedemptionProductId));
                ServiceResult sPoduct = new ServiceResult();
                sPoduct.Data = product;
                if (OnResult != null)
                {
                    OnResult(sPoduct);
                }

                var document = await Task.Run(() => LocalData.GetDocument(product.ImageID));
                ServiceResult sDocument = new ServiceResult();
                sDocument.Data = document;
                if (OnResult != null)
                {
                    OnResult(sDocument);
                }



                var detail = await Task.Run(() => LocalData.GetRedemptionProductDetail(item.RedemptionProductDetailId));
                ServiceResult sDetail = new ServiceResult();
                sDetail.Data = detail;
                if (OnResult != null)
                {
                    OnResult(sDetail);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("EVoucherDetail", ex.Message);
            }
        }

        public async void UseEVoucher(string voucherno, byte startus)
        {
            try
            {
                var detail = await Task.Run(() => Redemption.UseEvoucher(voucherno, startus, DateTime.Now));
                ServiceResult sDetail = new ServiceResult();
                sDetail.Data = detail;
                if (OnResult != null)
                {
                    OnResult(sDetail);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("UseEVoucher", ex.Message);
            }
        }


        public async void GetListEVoucher(int page, int pagesize)
        {
            try
            {
                var detail = await Task.Run(() => Redemption.GetListEVoucher(page,page));
                ServiceResult sDetail = new ServiceResult();
                sDetail.Data = detail;
                sDetail.StatusCode = detail.StatusCode;
                if (OnResult != null)
                {
                    OnResult(sDetail);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetListEVoucher", ex.Message);
            }
        }


        public async void ClearCart()
        {
            try
            {
                await Task.Run(() => LocalData.ClearCart());
                ServiceResult sDetail = new ServiceResult();
                if (OnResult != null)
                {
                    OnResult(sDetail);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("ClearCart", ex.Message);
            }
        }
    }
}
