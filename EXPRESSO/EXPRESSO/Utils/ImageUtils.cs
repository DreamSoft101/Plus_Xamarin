using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Utils
{
    public class ImageUtils
    {
        /*
        public string GetCCTV(TblCCTVImagePath cctv, MyEntity entity, bool encryption)
        {
            string nameimage = "";
            string eximage = "";
            string url = "";
            bool hasnumber = false;
            if (encryption)
            {
                if (!string.IsNullOrEmpty(cctv.strURL))
                {
                    if (cctv.strURL.ToLower().Contains("http"))
                    {
                        string[] split = cctv.strURL.Split('/');
                        for (int i = 0; i < split.Length - 1; i++)
                        {
                            url = url + split[i] + "/";
                        }
                        if (!split[split.Length - 1].Contains("."))
                        {
                            hasnumber = false;
                        }
                        else
                        {
                            string[] imagename = split[split.Length - 1].Split('.');
                            nameimage = imagename[0];
                            eximage = imagename[1];
                        }

                    }
                    else
                    {
                        string[] split = cctv.strURL.Split('.');
                        if (split.Length > 1)
                        {
                            nameimage = split[0];
                            eximage = split[1];
                            url = string.Format("http://{0}/cctv/entity_{1}/", entity.BACKEND_DOMAIN, entity.idEntity);
                        }
                        else
                        {
                            hasnumber = false;
                        }
                    }
                }
            }
            else
            {
                if (!cctv.strURL.ToLower().Contains("http"))
                {
                    url = string.Format("http://{0}/cctv/entity_{1}/{2}", entity.BACKEND_DOMAIN, entity.idEntity, cctv.strURL);
                }
                hasnumber = false;
            }
            string encrypt = "";
            if (!string.IsNullOrEmpty(url))
            {
                encrypt = entity.Secretkey + DateTime.UtcNow.ToString("yyyyMMdd") + nameimage;

            }

        }*/

        
    }
}
