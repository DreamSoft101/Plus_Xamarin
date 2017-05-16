using EXPRESSO.Models;
using EXPRESSO.Processing.Connections;
using EXPRESSO.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Threads
{
    public class EmzThread
    {
        public delegate void onGetAlbum(List<EmzAlbum> result);
        public onGetAlbum OnGetAlbum;
        public async void GetAlbum()
        {
            try
            {
                var result = await Task.Run(() => EmzConnection.GetAlbum());
                //return result;
                if (OnGetAlbum != null)
                {
                    OnGetAlbum(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetAlbum", ex.Message);
            }

        }

        public delegate void onGetEmagazine(List<Emagazine> result);
        public onGetEmagazine OnGetEmagazine;
        public async void GetEmagazine(string albumn)
        {
            try
            {
                var result = await Task.Run(() => EmzConnection.GetEmagazine(albumn));
                //return result;
                if (OnGetEmagazine != null)
                {
                    OnGetEmagazine(result);
                }
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetEmagazine", ex.Message);
            }

        }
    }
}
