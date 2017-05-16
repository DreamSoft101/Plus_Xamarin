using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using EXPRESSO.Processing.Connections;
using EXPRESSO.Processing.LocalData;
using EXPRESSO.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Threads
{
    public class PLUSRangerThreads
    {
        public delegate void onGetSetting(ServiceResult result);
        public onGetSetting OnGetSetting;

        public async void GetSetting()
        {
            try
            {
                var result = await Task.Run(() => PLUSRangerConnections.getSettings());
                //return result;
                if (OnGetSetting != null)
                {
                    OnGetSetting(result);
                }
            }
            catch (Exception ex)
            {

            }
        }


        public delegate void onGetCategory(ServiceResult result);
        public onGetCategory OnGetCategory;
        public async void GetCategory()
        {
            try
            {
                var result = await Task.Run(() => PLUSRangerConnections.GetCategory());
                
                //return result;
                if (OnGetCategory != null)
                {
                    OnGetCategory(result);
                }
            }
            catch (Exception ex)
            {

            }
        }


        public delegate void onGetListOpsComm(ServiceResult result);
        public onGetListOpsComm OnGetListOpsComm;
        public async void GetListOpsComm(List<string> id, int page)
        {
            try
            {
                var result = await Task.Run(() => PLUSRangerConnections.GetListOpsComm(id, page));
                //return result;
                if (OnGetListOpsComm != null)
                {
                    OnGetListOpsComm(result);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public delegate void onGetNearestLocation(ServiceResult result);
        public onGetNearestLocation OnGetNearestLocation;
        public async void GetNearestLocation(double lat, double lng)
        {
            try
            {
                var result = await Task.Run(() => PLUSRangerConnections.NearestLocation(lat, lng));
                //return result;
                if (OnGetNearestLocation != null)
                {
                    OnGetNearestLocation(result);
                }
            }
            catch (Exception ex)
            {

            }
        }


        public delegate void onPost(ServiceResult result);
        public onPost OnPost;
        public async void SavePostToLocal(string name, string title, string description, string email, string phone, string idcategory, double lat, double lng, List<TblMedia> lstMedia)
        {
            try
            {
                TblPost post = new TblPost();
                post.decLatitude = lat.ToString();
                post.decLongitude = lng.ToString();
                post.dtCreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                post.dtLastUpdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                post.idCategory = idcategory;
                //post.idOpsComm = Guid.NewGuid().ToString();
                post.intPosted = 0;
                post.intSource = 1;
                post.intStatus = 1;
                //post.strAddress
                post.strAllowAccess = "";
                //post.strCategory
                post.strContactNo = phone;
                post.strCreateBy = Cons.myEntity.User.strUserName;
                post.strCustomerName = email;
                post.strDescription = description;
                post.strContactEmail = email;
                post.strTitle = title;

                foreach (var item in lstMedia)
                {
                    item.idopscomm = post.idOpsComm;

                }


                var result = await Task.Run(() => PLUSRangerConnections.PostData(post, lstMedia));
                if (OnPost != null)
                {
                    OnPost(result);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}