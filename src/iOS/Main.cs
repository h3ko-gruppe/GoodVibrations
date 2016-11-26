using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace GoodVibrations.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main (string [] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
                System.Console.WriteLine($"Unhandled Error: {e}");

            try
            {
                // if you want to use a different Application Delegate class from "AppDelegate"
                // you can specify it here.
                UIApplication.Main(args, null, "AppDelegate");
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"Unhandled Error: {e}");

                throw;
            }
        }
    }
}
