using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UIKit;

namespace PLUS.iOS.Sources.PageViews
{
    public class MyAccountSource : UIPageViewControllerDataSource
    {
        private UIPageViewController _parentViewController;
        private List<string> _pageTitles;
        private List<UIViewController> _controller;
        private int Index = 0;
        

        public MyAccountSource(UIPageViewController parentViewController, List<string> pageTitles, List<UIViewController> controller)
        {
            _parentViewController = parentViewController;
            _pageTitles = pageTitles;
            _controller = controller;
        }

        public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            //  var vc = referenceViewController as ListAccountController;
            if (Index == _pageTitles.Count-1)
            {
                return null;
            }
            Index++;
            return _controller[Index];
        }

        public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            if (Index == 0)
            {
                return null;
            }
            Index--;

            return _controller[Index];
        }
        public override nint GetPresentationCount(UIPageViewController pageViewController)
        {
            return _pageTitles.Count;
        }

        public override nint GetPresentationIndex(UIPageViewController pageViewController)
        {
            return 0;
        }
    }
}