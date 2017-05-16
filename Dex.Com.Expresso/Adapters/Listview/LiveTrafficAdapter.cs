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
using EXPRESSO.Models.Database;
using Android.Support.V7.Widget;
using Square.Picasso;
using EXPRESSO.Models;
using Dex.Com.Expresso.Utils;
using EXPRESSO.Threads;
using static EXPRESSO.Models.EnumType;
using EXPRESSO.Utils;
using Dex.Com.Expresso.Activities;
using Android.Gms.Maps;
using Dex.Com.Expresso.Loyalty.Droid.Utils;

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class LiveTrafficAdapter : RecyclerView.Adapter, IFilterable
    {
        public event EventHandler<BaseItem> ItemClick;
        private Context mContext;
        private List<BaseItem> mLstItem;
        private ItemFilter mFilter;

        public LiveTrafficAdapter(Context conext, List<BaseItem> lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
            mFilter = new ItemFilter(mLstItem);
            mFilter.OnFiltered += (List<BaseItem> items) =>
            {
                this.mLstItem = items;
                this.NotifyDataSetChanged();
            };
        }

        public override int ItemCount
        {
            get
            {
                return mLstItem.Count;
            }
        }
        public Filter Filter
        {
            get
            {
                return mFilter;
            }
        }


        public BaseItem GetBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        private class BaseViewHolder : RecyclerView.ViewHolder
        {
            public CardView Root;
            public TextView txtTitle;
            public TextView txtDescription;
            public TextView txtLevel;
            public TextView txtDate;
            public ImageView imgType;
            public BaseItem mBaseItem;
            public LinearLayout lnlHeader;
            public LinearLayout lnlContent;
            public TextView txtHeaderName;
            public RecyclerView.Adapter mAdapter;
            public ImageView imgFeedback;
            public FrameLayout mFrameLayout;
            private LinearLayout mLnlShare;
            public Context mContext;
            public ImageView mImgFavoriteLocation;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                // Locate and cache view references:
                this.Root = itemView.FindViewById<CardView>(Resource.Id.cvRoot);
                this.txtTitle = itemView.FindViewById<TextView>(Resource.Id.txtTitle);
                this.txtDescription = itemView.FindViewById<TextView>(Resource.Id.txtDescription);
                this.txtLevel = itemView.FindViewById<TextView>(Resource.Id.txtLevel);
                this.imgType = itemView.FindViewById<ImageView>(Resource.Id.imgType);

                this.lnlHeader = itemView.FindViewById<LinearLayout>(Resource.Id.lnlHeader);
                this.lnlContent = itemView.FindViewById<LinearLayout>(Resource.Id.lnlContent);
                this.txtDate = itemView.FindViewById<TextView>(Resource.Id.txtDateTime);
                this.imgFeedback = itemView.FindViewById<ImageView>(Resource.Id.imgFeedback);
                this.mLnlShare = ItemView.FindViewById<LinearLayout>(Resource.Id.lnlShare);
                this.txtHeaderName = itemView.FindViewById<TextView>(Resource.Id.txtHeaderName);

                this.mImgFavoriteLocation = ItemView.FindViewById<ImageView>(Resource.Id.imgFavoriteLocation);

                this.imgFeedback.Click += ImgFeedback_Click;
                this.mLnlShare.Click += Share_Click;
                itemView.Click += (sender, e) =>
                {
                    itemClick(base.Position);
                    
                };
            }

            private void Share_Click(object sender, EventArgs e)
            {
                var item = mBaseItem.Item as TrafficUpdate;
                //throw new NotImplementedException();
                Intent sendIntent = new Intent();
                sendIntent.SetAction(Intent.ActionSend);
                //sendIntent.PutExtra(Intent.ExtraTitle, item.strTitle);
                sendIntent.PutExtra(Intent.ExtraText, item.strTitle);
                sendIntent.SetType("text/plain");
                mContext.StartActivity(sendIntent);
            }

            private void ImgFeedback_Click(object sender, EventArgs e)
            {
                Intent intent = new Intent(mContext, typeof(ActivityFeedback));
                intent.PutExtra(ActivityFeedback.MODEL_DATA, StringUtils.Object2String(mBaseItem.Item as TrafficUpdate));
                mContext.StartActivity(intent);
            }

            
        }
    


        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);
            item.setTag(BaseItem.TagName.Position, position);
            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.mContext = mContext;
            baseHolder.mAdapter = this;
            baseHolder.mBaseItem = item;
            baseHolder.lnlHeader.Visibility = ViewStates.Gone;
            baseHolder.lnlContent.Visibility = ViewStates.Gone;
            baseHolder.mImgFavoriteLocation.Visibility = ViewStates.Gone;
            if (item.Item is TblHighway)
            {
                //Header
                baseHolder.lnlHeader.Visibility = ViewStates.Visible;
                var itemHighway = item.Item as TblHighway;
                baseHolder.txtHeaderName.Text = itemHighway.strName;
           
            }
            else if (item.Item is FavoriteLocation)
            {
                var fitem = item.Item as FavoriteLocation;
                baseHolder.lnlHeader.Visibility = ViewStates.Visible;
                baseHolder.txtHeaderName.Text = fitem.strFavoriteLocationName;
            }
            else if (item.Item is TrafficUpdate)
            {
                baseHolder.lnlContent.Visibility = ViewStates.Visible;
                baseHolder.Root.SetCardBackgroundColor(mContext.getColor(Resource.Color.White));
                baseHolder.lnlContent.Visibility = ViewStates.Visible;
                var itemTrafficUpdate = item.Item as TrafficUpdate;
                baseHolder.txtTitle.Text = itemTrafficUpdate.strTitle;
                baseHolder.txtDescription.Text = string.Format(mContext.GetString(Resource.String.facility_detail_location_format), itemTrafficUpdate.decLocation);
                //Picasso.With(mContext).Load(itemTrafficUpdate.).Into(baseHolder.mImgLogo);

                //LiveTrafficType type = (LiveTrafficType)itemTrafficUpdate.intType;
                int icon = ResourceUtil.GetResourceID(mContext, "ic_menu_traffic_type" + itemTrafficUpdate.intType);
                if (icon != 0)
                {
                    baseHolder.imgType.SetImageResource(icon);
                }
                else
                {
                    LogUtils.WriteError("LiveTrafficAdapter", "Type: " + itemTrafficUpdate.intType);
                }

                switch (itemTrafficUpdate.intType)
                {
                    case 1:
                        {
                            baseHolder.txtLevel.Text = mContext.GetString(Resource.String.live_traffic_speed_level_1);
                            break;
                        }
                    case 2:
                        {
                            baseHolder.txtLevel.Text = mContext.GetString(Resource.String.live_traffic_speed_level_2);
                            break;
                        }
                    case 3:
                        {
                            baseHolder.txtLevel.Text = mContext.GetString(Resource.String.live_traffic_speed_level_3);
                            break;
                        }
                }

                baseHolder.txtDate.Text = itemTrafficUpdate.dtStartDateTime.ToString("dd/MM/yyyy HH:mm");

               
                /*

                switch (type)
                {
                    case LiveTrafficType.Alert:
                        {

                            break;
                        }
                    case LiveTrafficType.AlternativeRoute:
                        {
                            break;
                        }
                    case LiveTrafficType.FutureEvent:
                        {
                            break;
                        }
                    case LiveTrafficType.HeavyVechicleRestriction:
                        {
                            break;
                        }
                    case LiveTrafficType.Incident:
                        {
                            break;
                        }
                    case LiveTrafficType.MajorDisruption:
                        {
                            break;
                        }
                    case LiveTrafficType.RoadWork:
                        {
                            break;
                        }
                    case LiveTrafficType.RSAClosure:
                        {
                            break;
                        }
                    case LiveTrafficType.AESSpeedTrap:
                        {
                            break;
                        }
                }*/

            }
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_livetraffic, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
            return holder;
        }
        void OnClick(int position)
        {
            if (ItemClick != null)
            {
                if (GetBaseItem(position).Item is TblHighway)
                {

                }
                else
                {
                    ItemClick(this, GetBaseItem(position));
                }
            }
        }

        private class ItemFilter : Filter
        {
            private List<BaseItem> mOriginalData;
            private List<BaseItem> mLstItem;

            public delegate void onFiltered(List<BaseItem> data);
            public onFiltered OnFiltered;

            public ItemFilter(List<BaseItem> originalData)
            {
                this.mOriginalData = originalData;
            }
            protected override FilterResults PerformFiltering(Java.Lang.ICharSequence constraint)
            {
                FilterResults results = new FilterResults();
                if (constraint == null)
                {
                    results.Values = null;
                    results.Count = 0;
                    return results;
                }
                else
                {
                    try
                    {
                        if (constraint.ToString() == null)
                        {
                            if (mLstItem == null)
                            {
                                mLstItem = mOriginalData;
                                results.Count = mLstItem.Count;
                                results.Values = mLstItem.ToJavaObject();
                                return results;
                            }
                            else
                            {
                                results.Count = mLstItem.Count;
                                results.Values = mLstItem.ToJavaObject();
                                return results;
                            }
                        }
                        string strQuery = constraint.ToString().ToLower();
                        if (string.IsNullOrEmpty(strQuery))
                        {
                            results.Count = mOriginalData.Count;
                            results.Values = mOriginalData.ToJavaObject();
                            return results;
                        }
                        if (mLstItem != null)
                        {
                            mLstItem.Clear();
                        }
                        else
                        {
                            mLstItem = new List<BaseItem>();
                        }


                        foreach (var item in mOriginalData)
                        {
                            if (item.Item is TrafficUpdate)
                            {
                                var itemf = item.Item as TrafficUpdate;
                                if (itemf.strTitle.ToLower().StartsWith(strQuery) || itemf.strDescription.ToLower().Contains(strQuery))
                                {
                                    mLstItem.Add(item);
                                }
                            }
                        }

                        results.Count = mLstItem.Count;
                        results.Values = mLstItem.ToJavaObject();
                        return results;
                    }
                    catch (System.Exception ex)
                    {
                        results.Count = mOriginalData.Count;
                        results.Values = mOriginalData.ToJavaObject();
                        return results;
                    }

                }

            }

            protected override void PublishResults(Java.Lang.ICharSequence constraint, FilterResults results)
            {
                if (results.Count > 0)
                {
                    List<BaseItem> data = results.Values.ToNetObject<List<BaseItem>>();
                    if (OnFiltered != null)
                    {
                        OnFiltered(data);
                    }
                }
            }
        }

    }
}