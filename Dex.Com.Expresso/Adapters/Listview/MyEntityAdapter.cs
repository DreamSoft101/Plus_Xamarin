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

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class MyEntityAdapter : BaseAdapter
    {
        private Context mContext;
        private List<MyEntity> mLstItem;

        public MyEntityAdapter(Context conext, List<MyEntity> lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
        }

        public override int Count
        {
            get
            {
                return  mLstItem.Count;
            }
        }


        public override Java.Lang.Object GetItem(int position)
        {
            //return (mLstItem[position]).Cast<VehicleClasses>();
            return null;
        }

        public MyEntity GetMyEntity (int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public ImageView mImgAvatar;
            public TextView mTxtEntityName;
            public TextView mTxtUserName;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder viewHoder = null;
            var item = GetMyEntity(position);
            if (convertView == null)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.item_my_entity, null);
                viewHoder = new ViewHolder();
                viewHoder.mImgAvatar = convertView.FindViewById<ImageView>(Resource.Id.imgAvatar);
                viewHoder.mTxtUserName = convertView.FindViewById<TextView>(Resource.Id.txtName);
                viewHoder.mTxtEntityName = convertView.FindViewById<TextView>(Resource.Id.txtEntityName);
                convertView.Tag = viewHoder;
            }
            else
            {
                viewHoder = convertView.Tag as ViewHolder;
            }

            viewHoder.mImgAvatar.SetImageResource(Resource.Drawable.img_default_avatar);
            if (!string.IsNullOrEmpty(item.User.strAvatar))
            {
                Picasso.With(mContext)
                .Load(item.User.strAvatar)
                .Into(viewHoder.mImgAvatar);
            }
            viewHoder.mTxtEntityName.Text = item.Entity.strName;
            viewHoder.mTxtUserName.Text = item.User.strUserName;
            return convertView;
        }
    }
}