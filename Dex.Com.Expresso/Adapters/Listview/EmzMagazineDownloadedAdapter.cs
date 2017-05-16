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
using EXPRESSO.Models;
using Dex.Com.Expresso.Activities;
using Square.Picasso;
using EXPRESSO.Utils;
using Newtonsoft.Json;
using System.IO;
using Android.Database;

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class EmzMagazineDownloadedAdapter : BaseAdapter
    {
        private Context mContext;
        private List<Emagazine> mLstItem;
        private DownloadManager dm;
        private BaseActivity mBaseActivity;
        public EmzMagazineDownloadedAdapter(Context conext)
        {
            this.mContext = conext;
            dm = (DownloadManager)mContext.GetSystemService(Context.DownloadService);
            mBaseActivity = conext as BaseActivity;

            string jsonCache = mBaseActivity.getCacheString("DMCache");
            List<Emagazine> downs = new List<Emagazine>();
            if (!string.IsNullOrEmpty(jsonCache))
            {
                downs = JsonConvert.DeserializeObject<List<Emagazine>>(jsonCache);
                if (downs == null)
                {
                    downs = new List<Emagazine>();
                }
            }
            this.mLstItem = downs;
            for (int i = 0; i < mLstItem.Count; i++)
            {
                var item = mLstItem[i];
                var down = mLstItem.Where(p => p.FileName == item.FileName).FirstOrDefault();
                string path = System.IO.Path.Combine((string)Android.OS.Environment.ExternalStorageDirectory, "PLUS");
                string fileName = path + "/" + item.FileName;
                var exist = File.Exists(fileName);
                var downStatus = down != null ? validDownload(down.DownloaID) : DownloadStatus.Failed;
                if (exist)
                {
                    if (downStatus != DownloadStatus.Running && downStatus != DownloadStatus.Pending && downStatus != DownloadStatus.Paused)
                    {

                    }
                    else
                    {
                        mLstItem.RemoveAt(i);
                        i--;
                    }

                }
                else
                {
                    mLstItem.RemoveAt(i);
                    i--;
                }
            }
        }

        public override int Count
        {
            get
            {
                return mLstItem.Count;
            }
        }

        public List<Emagazine> getListItems()
        {
            return mLstItem;
        }



        public override Java.Lang.Object GetItem(int position)
        {
            //return (mLstItem[position]).Cast<VehicleClasses>();
            return null;
        }



        public Emagazine getBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public ImageView imgPicture;
            public TextView txtTitle;
            public TextView txtSubTitle;
            public LinearLayout mLnlDownload;
            public Button mBtnButton;
            public View mRoot;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder = null;
            var item = getBaseItem(position);
          

            if (convertView == null)
            {
                holder = new ViewHolder();
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.emz_item_magazine, null);
                convertView.Click += ConvertView_Click;
                holder.mRoot = convertView;
                holder.imgPicture = convertView.FindViewById<ImageView>(Resource.Id.imgPicture);
                holder.txtTitle = convertView.FindViewById<TextView>(Resource.Id.txtTitle);
                holder.txtSubTitle = convertView.FindViewById<TextView>(Resource.Id.txtSubTitle);
                holder.mLnlDownload = convertView.FindViewById<LinearLayout>(Resource.Id.lnlDownload);
                holder.mBtnButton = convertView.FindViewById<Button>(Resource.Id.btnButton);
                holder.mBtnButton.Click += MBtnButton_Click;
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }
            holder.mLnlDownload.Visibility = ViewStates.Invisible;

            Picasso.With(mContext).Load(Cons.Emz_URL + item.FileName.Replace(".pdf", ".png")).Error(Resource.Drawable.img_error).Into(holder.imgPicture);
            holder.txtTitle.Text = item.Title;
            holder.txtSubTitle.Text = item.Subtitle;
            holder.mLnlDownload.Visibility = ViewStates.Invisible;
            holder.mBtnButton.Tag = position;
            //convertView.Tag = position;
            var down = mLstItem.Where(p => p.FileName == item.FileName).FirstOrDefault();
            string path = System.IO.Path.Combine((string)Android.OS.Environment.ExternalStorageDirectory, "PLUS");
            string fileName = path + "/" + item.FileName;
            var exist = File.Exists(fileName);
            var downStatus = down != null ? validDownload(down.DownloaID) : DownloadStatus.Failed;
            if (exist)
            {
                if (downStatus != DownloadStatus.Running && downStatus != DownloadStatus.Pending && downStatus != DownloadStatus.Paused)
                {
                    holder.mRoot.Visibility = ViewStates.Visible;
                    //holder.mLnlDownload.Visibility = ViewStates.Invisible;
                    holder.mBtnButton.Text = "Delete";
                }
                else
                {
                    holder.mRoot.Visibility = ViewStates.Gone;
                }

            }
            else
            {
                holder.mRoot.Visibility = ViewStates.Gone;
            }

            return convertView;
        }

        private void ConvertView_Click(object sender, EventArgs e)
        {
            var btn = sender as View;
            var intPosition = Convert.ToInt16((btn.Tag as ViewHolder).mBtnButton.Tag.ToString());
            var item = mLstItem[intPosition];
            string path = System.IO.Path.Combine((string)Android.OS.Environment.ExternalStorageDirectory, "PLUS");
            string fileName = path + "/" + item.FileName;
            Java.IO.File file = new Java.IO.File(fileName);
            if (file.Exists())
            {
                var uri = Android.Net.Uri.FromFile(file);
                Intent intent = new Intent(Intent.ActionView);
                intent.SetDataAndType(uri, "application/pdf");
                intent.SetFlags(ActivityFlags.ClearTop);
                //intent.setFlags(Intent.FLAG_ACTIVITY_NO_HISTORY);
                try
                {
                    mContext.StartActivity(intent);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(mContext, "No Application Available to View PDF", ToastLength.Short).Show();
                }

            }
        }

        private void MBtnButton_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var intPosition = Convert.ToInt16(btn.Tag.ToString());
            var item = mLstItem[intPosition];
            if (btn.Text == "Delete")
            {
                string path = System.IO.Path.Combine((string)Android.OS.Environment.ExternalStorageDirectory, "PLUS");
                string fileName = path + "/" + item.FileName;
                var exist = File.Exists(fileName);
                if (exist)
                {
                    File.Delete(fileName);
                }
                mLstItem.Remove(item);
                mBaseActivity.setCacheString("DMCache", JsonConvert.SerializeObject(mLstItem));
                this.NotifyDataSetChanged();
            }
        }
        private DownloadStatus validDownload(long downloadId)
        {
            ICursor c = dm.InvokeQuery(new DownloadManager.Query().SetFilterById(downloadId));
            if (c.MoveToFirst())
            {
                int intStatus = c.GetInt(c.GetColumnIndex(DownloadManager.ColumnStatus));
                DownloadStatus status = (DownloadStatus)intStatus;
                return status;
            }
            return DownloadStatus.Failed;
        }
    }
}