using System;

namespace Dex.Com.Expresso
{
    public interface IExpandCollapseListener
    {
        void OnRecyclerViewItemExpanded(int position);

        void OnRecyclerViewItemCollapsed(int position);
    }
}

