using System;
using Foundation;
using GoodVibrations.Interfaces.Services;

namespace GoodVibrations.iOS.Services
{
    public class ImageService : IImageService
    {

        public string BundlePrefix {
            get {
                return NSBundle.MainBundle.ResourcePath;
            }
        }
    }
}
