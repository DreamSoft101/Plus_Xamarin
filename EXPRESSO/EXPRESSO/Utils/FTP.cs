using Java.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Utils
{
    public class FTP
    {
        public void sendAPicture(string picture)
        {

            string ftpHost = "xxxx";

            string ftpUser = "yyyy";

            string ftpPassword = "zzzzz";

            string ftpfullpath = "ftp://myserver.com/testme123.jpg";

            FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);

            //userid and password for the ftp server  

            ftp.Credentials = new NetworkCredential(ftpUser, ftpPassword);

            ftp.KeepAlive = true;
            ftp.UseBinary = true;
            ftp.Method = WebRequestMethods.Ftp.UploadFile;

            FileStream fs = File.OpenRead(picture);

            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);

            fs.Close();

            Stream ftpstream = ftp.GetRequestStream();
            ftpstream.Write(buffer, 0, buffer.Length);
            ftpstream.Close();
            ftpstream.Flush();

            //  fs.Flush();

        }
    }
}
