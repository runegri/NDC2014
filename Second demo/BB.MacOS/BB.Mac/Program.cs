using MonoMac.AppKit;
using MonoMac.Foundation;
using BubbleBobble.MacOS;

namespace BubbleBobble.MacOS
{
	class Program
	{
		static void Main (string [] args)
		{
			NSApplication.Init ();

			using (var p = new NSAutoreleasePool ()) {
				NSApplication.SharedApplication.Delegate = new AppDelegate();
				NSApplication.Main(args);
			}
		}
	}

	class AppDelegate : NSApplicationDelegate
	{
		BubbleBobbleGame game;
		public override void FinishedLaunching (MonoMac.Foundation.NSObject notification)
		{			
			game = new BubbleBobbleGame ();
			game.Run ();
		}

		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
		{
			return true;
		}
	}	
}
