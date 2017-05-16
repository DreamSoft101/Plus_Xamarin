using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;
using System.IO;
using EXPRESSO.Utils;

namespace Dex.Com.Expresso.Utils
{
    public class FtpClient
    {
        //public static void UpdateFTP()
        //{
        //    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://addres.com/file.txt");
        //    request.Method = WebRequestMethods.Ftp.DownloadFile;
        //    request.Credentials = new NetworkCredential("login", "password");
        //    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
        //    Stream responseStream = response.GetResponseStream();
        //    StreamReader reader = new StreamReader(responseStream);
        //    keys_from_server = reader.ReadToEnd();
        //    reader.Close();
        //}

        public static void sendAPicture(string filename, string path)
        {
            try
            {


                string ftpHost = "ftpexp.droidvn.com";// Cons.mFtpSettings.ftpHost;

                string ftpUser = Cons.mFtpSettings.ftpUser;

                string ftpPassword = Cons.mFtpSettings.ftpPassword;

                string ftpPath = Cons.mFtpSettings.ftpRootPath;

                string ftpfullpath = "ftp://" + ftpHost + "/" + ftpPath + "/" + filename;

                FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(ftpfullpath);

                //userid and password for the ftp server  

                ftp.Credentials = new NetworkCredential(ftpUser, ftpPassword);

                //ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                FileStream fs = File.OpenRead(path);

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                ftpstream.Close();
                ftpstream.Flush();
            }
            catch (Exception ex)
            {

            }
            //  fs.Flush();

        }

        public static void sendAPicture(string filename, byte[] data)
        {
            try
            {


                string ftpHost = "ftpexp.droidvn.com";// Cons.mFtpSettings.ftpHost;

                string ftpUser = Cons.mFtpSettings.ftpUser;

                string ftpPassword = Cons.mFtpSettings.ftpPassword;

                string ftpPath = Cons.mFtpSettings.ftpRootPath;

                string ftpfullpath = "ftp://" + ftpHost + "/" + ftpPath + "/" + filename;

                FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(ftpfullpath);

                //userid and password for the ftp server  

                ftp.Credentials = new NetworkCredential(ftpUser, ftpPassword);

                //ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

               
                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(data, 0, data.Length);
                ftpstream.Close();
                ftpstream.Flush();
            }
            catch (Exception ex)
            {

            }
            //  fs.Flush();

        }
    }
}