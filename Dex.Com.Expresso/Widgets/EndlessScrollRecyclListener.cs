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
using Android.Support.V7.Widget;

namespace Dex.Com.Expresso.Widgets
{
    public  class EndlessScrollRecyclListener : RecyclerView.OnScrollListener
    {
        // The total number of items in the dataset after the last load
        private int previousTotalItemCount = 0;
        private bool loading = true;
        private int visibleThreshold = 5;
        int firstVisibleItem, visibleItemCount, totalItemCount;
        private int startingPageIndex = 0;
        private int currentPage = 0;
        public delegate void OnLoadMore(int page, int totalItemsCount);
        public OnLoadMore onLoadMore;

        public override void OnScrolled(RecyclerView mRecyclerView, int dx, int dy)
        {
            base.OnScrolled(mRecyclerView, dx, dy);
            LinearLayoutManager mLayoutManager = (LinearLayoutManager)mRecyclerView.GetLayoutManager();
            visibleItemCount = mRecyclerView.ChildCount;
            totalItemCount = mLayoutManager.ItemCount;
            firstVisibleItem = mLayoutManager.FindFirstVisibleItemPosition();
            onScroll(firstVisibleItem, visibleItemCount, totalItemCount);
        }

        public void onScroll(int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            // If the total item count is zero and the previous isn't, assume the
            // list is invalidated and should be reset back to initial state
            if (totalItemCount < previousTotalItemCount)
            {
                this.currentPage = this.startingPageIndex;
                this.previousTotalItemCount = totalItemCount;
                if (totalItemCount == 0)
                {
                    this.loading = true;
                }
            }
            // If it’s still loading, we check to see if the dataset count has
            // changed, if so we conclude it has finished loading and update the current page
            // number and total item count.
            if (loading && (totalItemCount > previousTotalItemCount))
            {
                loading = false;
                previousTotalItemCount = totalItemCount;
                currentPage++;
            }

            // If it isn’t currently loading, we check to see if we have breached
            // the visibleThreshold and need to reload more data.
            // If we do need to reload some more data, we execute onLoadMore to fetch the data.
            if (!loading && (totalItemCount - visibleItemCount) <= (firstVisibleItem +
                    visibleThreshold))
            {
                onLoadMore(currentPage + 1, totalItemCount);
                loading = true;
            }
        }
        
        public void Clear()
        {
            currentPage = 0;
        }
    }
}