using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace scannerFood.Services
{
    public interface IImageService
    {
        void ScanImage(string subscriptionKey, string endpoint);
        void CopyImage(Image image);
    }
}
