using EXPRESSO.Models;
using EXPRESSO.Processing.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Threads
{
    public class CommunicationThreads
    {

        public delegate void onGetFeedback(ServiceResult result);
        public onGetFeedback OnGetFeedback;
        public async void getFeedback(BaseItem item)
        {
            try
            {
                var result = await Task.Run(() => CommunicationConnection.getFeedback(item));
                //return result;
                if (OnGetFeedback != null)
                {
                    OnGetFeedback(result);
                }
            }
            catch (Exception ex)
            {

            }
        }


        public delegate void onPostFeedback(ServiceResult result);
        public onGetFeedback OnPostFeedback;
        public async void postFeedback(BaseItem item,string strContent)
        {
            try
            {
                var result = await Task.Run(() => CommunicationConnection.postFeedback(item, strContent));
                //return result;
                if (OnPostFeedback != null)
                {
                    OnPostFeedback(result);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
