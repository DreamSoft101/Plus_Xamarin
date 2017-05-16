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
using Android.Database;
using Android.Provider;
using Java.IO;
using System.IO;
using Android.Graphics;
using EXPRESSO.Utils;
using System.Net.Http;
using System.Threading.Tasks;
using static Android.Provider.MediaStore;

namespace Dex.Com.Expresso.Utils
{
    public static class ImageUtils
    {
        public static byte[] UriToByteArray(Context context,  Android.Net.Uri uri)
        {
            BitmapFactory.Options options = new BitmapFactory.Options();// Create object of bitmapfactory's option method for further option use
            options.InPurgeable = true; // inPurgeable is used to free up memory while required
          
            Stream iStream = context.ContentResolver.OpenInputStream(uri);
            Bitmap bitmap = BitmapFactory.DecodeStream(iStream);
            MemoryStream baos = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, baos);
            //var data = baos.to
            var data = baos.ToArray();
            return data;
            /*

            ByteArrayOutputStream byteBuffer = new ByteArrayOutputStream();
            int bufferSize = 4096;
            byte[] buffer = new byte[iStream.Length];
            int len = 0;
            iStream.Read(buffer, 0, buffer.Length);
            byteBuffer.Write(buffer, 0, len);
         
            return byteBuffer.ToByteArray();*/
        }

        public static byte[] BitmapToByteArray(Context context, Bitmap bitmap)
        {
            MemoryStream baos = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, baos);
            //var data = baos.to
            var data = baos.ToArray();
            return data;
        }

        public static string ExportBitmapAsPNG(Bitmap bitmap)
        {
            string rootfolder = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "PLUS");
            if (!Directory.Exists(rootfolder))
            {
                Directory.CreateDirectory(rootfolder);
            }
            


            string filename = DateTime.UtcNow.Ticks + "_" + StringUtils.RandomString(5) + ".png";
            //var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var filePath = System.IO.Path.Combine(rootfolder, filename);
            var stream = new FileStream(filePath, FileMode.Create);
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            stream.Close();
            return filePath;
        }

        public async static Task<Bitmap> BitmapFromURL(string strUrl)
        {
            var url = new Uri(strUrl);
            var httpClient = new HttpClient();
            var data = await httpClient.GetByteArrayAsync(url).ConfigureAwait(true);
            Bitmap bmp = BitmapFactory.DecodeByteArray(data, 0, data.Length);
            return bmp;

        }


        public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            float height = options.OutHeight;
            float width = options.OutWidth;
            double inSampleSize = 1D;

            if (height > reqHeight || width > reqWidth)
            {
                int halfHeight = (int)(height / 2);
                int halfWidth = (int)(width / 2);

                // Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
                while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return (int)inSampleSize;
        }

        public static string GetFilePathFromContentUri(Android.Net.Uri selectedVideoUri, Context mContext)
        {
            string strURI = selectedVideoUri.ToString();
            if (strURI.Contains("com.android.providers.media.documents"))
            {
                strURI = strURI.Replace("%3A", ":");
                string[] strSplit1 = strURI.Split('/');
                string lastData = strSplit1[strSplit1.Length - 1];
                string[] strSplit2 = lastData.Split(':');

                string id = strSplit2[1];
                string type = strSplit2[0];

                Android.Net.Uri newURI = null;
                if (type == "image")
                {
                    newURI = MediaStore.Images.Media.ExternalContentUri;
                }
                else if (type == "video")
                {
                    newURI = MediaStore.Video.Media.ExternalContentUri;
                } else if (type == "audio")
                {
                    newURI = MediaStore.Audio.Media.ExternalContentUri;
                }


                string strSelection = "_id=?";
                string[] values = new string[] { id };

                String[] filePathColumn = { MediaColumns.Data };

                ICursor cursor = mContext.ContentResolver.Query(newURI, filePathColumn, strSelection, values, null);
                cursor.MoveToFirst();

                int columnIndex = cursor.GetColumnIndex(filePathColumn[0]);
                string filePath = cursor.GetString(columnIndex);
                cursor.Close();
                return filePath;
            }
            else
            {
                String filePath;
                String[] filePathColumn = { MediaColumns.Data };

                ICursor cursor = mContext.ContentResolver.Query(selectedVideoUri, filePathColumn, null, null, null);
                cursor.MoveToFirst();

                int columnIndex = cursor.GetColumnIndex(filePathColumn[0]);
                filePath = cursor.GetString(columnIndex);
                cursor.Close();
                return filePath;
            }
            
        }

    }
}