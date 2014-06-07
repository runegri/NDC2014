using MonoMac.AppKit;
using MonoMac.Foundation;
using BubbleBobble.MacOS;
using Chopper;

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
		ChopperGame game;
		public override void FinishedLaunching (MonoMac.Foundation.NSObject notification)
		{			
			game = new ChopperGame ();
			game.Run ();
		}

		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
		{
			return true;
		}
	}	
}
