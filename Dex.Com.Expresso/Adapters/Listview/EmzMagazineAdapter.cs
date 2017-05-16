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
using Square.Picasso;
using EXPRESSO.Utils;
using Android.Database;
using Dex.Com.Expresso.Activities;
using Newtonsoft.Json;
using System.IO;

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class EmzMagazineAdapter : BaseAdapter
    {
        private Context mContext;
        private List<Emagazine> mLstItem;
        private DownloadManager dm;
        private BaseActivity mBaseActivity;
        public EmzMagazineAdapter(Context conext, List<Emagazine> lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
            mBaseActivity = conext as BaseActivity;
            dm = (DownloadManager)mContext.GetSystemService(Context.DownloadService);
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

        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder = null;
            var item = getBaseItem(position);
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


            if (convertView == null)
            {
                holder = new ViewHolder();
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.emz_item_magazine, null);
                holder.imgPicture = convertView.FindViewById<ImageView>(Resource.Id.imgPicture);
                holder.txtTitle = convertView.FindViewById<TextView>(Resource.Id.txtTitle);
                holder.txtSubTitle = convertView.FindViewById<TextView>(Resource.Id.txtSubTitle);
                holder.mLnlDownload = convertView.FindViewById<LinearLayout>(Resource.Id.lnlDownload);
                holder.mBtnButton = convertView.FindViewById<Button> (Resource.Id.btnButton);
                holder.mBtnButton.Click += MBtnButton_Click;
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }


            Picasso.With(mContext).Load(Cons.Emz_URL + item.FileName.Replace(".pdf",".png")).Error(Resource.Drawable.img_error).Into(holder.imgPicture);
            holder.txtTitle.Text = item.Title;
            holder.txtSubTitle.Text = item.Subtitle;
            holder.mLnlDownload.Visibility = ViewStates.Invisible;
            holder.mBtnButton.Tag = position;

            var down = downs.Where(p => p.FileName == item.FileName).FirstOrDefault();
            string path = System.IO.Path.Combine((string)Android.OS.Environment.ExternalStorageDirectory, "PLUS");
            string fileName = path + "/" + item.FileName;
            var exist = File.Exists(fileName);
            var downStatus = down != null ? validDownload(down.DownloaID) : DownloadStatus.Failed;
            if (exist)
            {
                if (downStatus != DownloadStatus.Running && downStatus != DownloadStatus.Pending && downStatus != DownloadStatus.Paused)
                {
                    holder.mLnlDownload.Visibility = ViewStates.Invisible;
                    holder.mBtnButton.Text = "Read";
                }
                else
                {
                    //item.DownloaID = down.DownloaID;
                    holder.mLnlDownload.Visibility = ViewStates.Visible;
                    holder.mBtnButton.Text = "Download";
                }
                
            }
            else
            {
                if (down != null)
                {
                    if (downStatus == DownloadStatus.Running || downStatus == DownloadStatus.Pending || downStatus == DownloadStatus.Paused)
                    {
                        //item.DownloaID = down.DownloaID;
                        holder.mLnlDownload.Visibility = ViewStates.Visible;
                        holder.mBtnButton.Text = "Download";
                    }
                    else
                    {
                        holder.mLnlDownload.Visibility = ViewStates.Invisible;
                        holder.mBtnButton.Text = "Download";
                    }
                }
                else
                {
                    holder.mLnlDownload.Visibility = ViewStates.Invisible;
                    holder.mBtnButton.Text = "Download";
                }
            }

            return convertView;
        }

        private void MBtnButton_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var intPosition = Convert.ToInt16(btn.Tag.ToString());
            var item = mLstItem[intPosition];
            if (btn.Text == "Download")
            {
                var downStatus = validDownload(item.DownloaID);
                if (downStatus == DownloadStatus.Pending || downStatus == DownloadStatus.Running || downStatus == DownloadStatus.Paused)
                {
                    return;
                }
                //throw new NotImplementedException();


                DownloadManager.Request request = new DownloadManager.Request(Android.Net.Uri.Parse(Cons.Emz_URL + item.FileName));
                //request.SetVisibleInDownloadsUi(false).SetNotificationVisibility(DownloadManager.Request.VisibilityVisible);
                request.SetNotificationVisibility(DownloadVisibility.Visible | DownloadVisibility.VisibleNotifyCompleted);
                // request.SetNotificationVisibility();
                request.SetDescription(item.Subtitle).SetTitle(item.Title);

                //string path = System.IO.Path.Combine((string)Android.OS.Environment.ExternalStorageDirectory, "PLUS");
                request.SetDestinationInExternalPublicDir("/PLUS", item.FileName);
                //magazine.setDownloadManagerId(dm.enqueue(request));
                long downloadID = dm.Enqueue(request);
                item.DownloaID = downloadID;


                var baseActivity = mBaseActivity;
                string jsonCache = baseActivity.getCacheString("DMCache");
                List<Emagazine> downs = new List<Emagazine>();
                if (!string.IsNullOrEmpty(jsonCache))
                {
                    downs = JsonConvert.DeserializeObject<List<Emagazine>>(jsonCache);
                    if (downs == null)
                    {
                        downs = new List<Emagazine>();
                    }
                }

                var itemE = downs.Where(p => p.FileName == item.FileName).FirstOrDefault();
                if (itemE != null)
                {
                    downs.Remove(itemE);
                }
                item.LastDown = DateTime.Now;
                downs.Add(item);
                baseActivity.setCacheString("DMCache", JsonConvert.SerializeObject(downs));
                this.NotifyDataSetChanged();
            }
            else
            {
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
                        Toast.MakeText(mContext,"No Application Available to View PDF", ToastLength.Short).Show();
                    }
                 
                }
               
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