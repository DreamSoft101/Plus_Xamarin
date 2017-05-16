using EXPRESSO.Models;
using EXPRESSO.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EXPRESSO.Processing.Connections
{
    public class EmzConnection
    {
        public static async Task<List<EmzAlbum>> GetAlbum()
        {
            try
            {
                List<EmzAlbum> result = new List<EmzAlbum>();
                BaseClient client = new BaseClient(Cons.Emz_URL + "Albums.plist");
                string strXML = await client.getData();

                var xml = XDocument.Parse(strXML);
                string lstKey = "";
                foreach (XElement plistnode in xml.Elements())
                {
                    if (plistnode.Name == "plist")
                    {
                        XElement node = plistnode.Elements().FirstOrDefault();
                        if (node != null)
                        {
                            foreach (XElement pdist in node.Elements())
                            {
                                var album = new EmzAlbum();
                                foreach (XElement item in pdist.Elements())
                                {
                                    if (lstKey == "FileName")
                                    {
                                        album.FileName = item.Value.ToString();
                                    }
                                    else if (lstKey == "Picture")
                                    {
                                        album.Picture = item.Value.ToString();
                                    }
                                    else if (lstKey == "Title")
                                    {
                                        album.Title = item.Value.ToString();
                                    }
                                    else if (lstKey == "Quantity")
                                    {
                                        album.Quantity = item.Value.ToString();
                                    }
                                    lstKey = item.Value.ToString();
                                }
                                result.Add(album);
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetAlbum", ex.Message);
            }
            return null;
        }


        public static async Task<List<Emagazine>> GetEmagazine(string albumn)
        {
            try
            {
                List<Emagazine> result = new List<Emagazine>();
                BaseClient client = new BaseClient(Cons.Emz_URL + albumn);
                string strXML = await client.getData();

                var xml = XDocument.Parse(strXML);
                string lstKey = "";
                foreach (XElement plistnode in xml.Elements())
                {
                    if (plistnode.Name == "plist")
                    {
                        XElement node = plistnode.Elements().FirstOrDefault();
                        if (node != null)
                        {
                            foreach (XElement pdist in node.Elements())
                            {
                                var album = new Emagazine();
                                foreach (XElement item in pdist.Elements())
                                {
                                    if (lstKey == "FileName")
                                    {
                                        album.FileName = item.Value.ToString();
                                    }
                                    else if (lstKey == "Subtitle")
                                    {
                                        album.Subtitle = item.Value.ToString();
                                    }
                                    else if (lstKey == "Title")
                                    {
                                        album.Title = item.Value.ToString();
                                    }
                                    lstKey = item.Value.ToString();
                                }
                                result.Add(album);
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("GetAlbum", ex.Message);
            }
            return null;
        }


    }
}
