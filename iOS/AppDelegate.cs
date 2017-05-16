using Foundation;
using UIKit;
using Facebook.CoreKit;
using Google.Core;
using Google.SignIn;

namespace PLUS.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
        // class-level declarations
        string appId = "318581368494042";
        string appName = "Expresso";

        //Google Config
        string clientId = "419784069151-u0b4glmm3u6dh0l5j964i8o0e5dbg8tj.apps.googleusercontent.com";

        public override UIWindow Window
		{
			get;
			set;
		}

        public RootViewController RootViewController { get { return UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers[0] as RootViewController; } }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
            //Window = new UIWindow(UIScreen.MainScreen.Bounds);

            // If you have defined a root view controller, set it here:
            /*var storyboard = UIStoryboard.FromName("Main", NSBundle.MainBundle);
            UIViewController rootViewController = (UIViewController)storyboard.InstantiateViewController("RootViewController");
            Window.RootViewController = new UINavigationController(rootViewController);

            // make the window visible
            Window.MakeKeyAndVisible();*/

            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            NSError configureError;
            Context.SharedInstance.Configure(out configureError);
            if (configureError != null)
            {
                // If something went wrong, assign the clientID manually
                //Console.WriteLine("Error configuring the Google context: {0}", configureError);
                SignIn.SharedInstance.ClientID = clientId;
            }

            //return true;
            Settings.AppID = appId;
            Settings.DisplayName = appName;
            // ...

            // This method verifies if you have been logged into the app before, and keep you logged in after you reopen or kill your app.
            return ApplicationDelegate.SharedInstance.FinishedLaunching(application, launchOptions);
        }

		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            // We need to handle URLs by passing them to their own OpenUrl in order to make the SSO authentication works.
            if (url.Scheme.StartsWith("fb"))
            {
                return ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApplication, annotation);
            }
            else
            {
                return SignIn.SharedInstance.HandleUrl(url, sourceApplication, annotation);
            }
            
            
        }
    }
}

