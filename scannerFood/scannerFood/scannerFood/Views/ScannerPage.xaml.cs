using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using scannerFood.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace scannerFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class ScannerPage : ContentPage
    {
        protected string PhotoPath { get; set; }
        protected Stream stream { get; set; }

        // <snippet_vars>
        // Add your Computer Vision subscription key and endpoint   
        static readonly string subscriptionKey = "c1f515eaa1ef4950b0eaee273ff256c4";
        static readonly string endpoint = "https://allergycomputervision.cognitiveservices.azure.com/";
        // </snippet_vars>
        // </snippet_using_and_vars>
        private const string READ_TEXT_LOCAL_IMAGE = "easter-angel-cake-ingredients-list-by-bakethiscake.jpg";

        private const string READ_TEXT_URL_IMAGE = "https://intelligentkioskstore.blob.core.windows.net/visionapi/suggestedphotos/3.png";


        static readonly string[] eggNames = { "egg", "eggs", "egg powder", "dried eggs", "egg solids", "albumin" };
        static readonly List<string> eggList = new List<string>(eggNames);

        public static Image currentPhoto;
        public static string currentPath = "/data/user/0/com.companyname.scannerfood/cache/image.jpg";

        public ScannerPage()
        {
            InitializeComponent();
        }

        async Task TakePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                
                await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage

            stream = await photo.OpenReadAsync();

            PhotoImage.Source = ImageSource.FromStream(() => stream);
             

            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
             
            //using (var stream = await photo.OpenReadAsync())
            //using (var newStream = File.OpenWrite(newFile))
            //    await stream.CopyToAsync(newStream);

            PhotoPath = newFile;
            
            


        }

        async void OnPickPhotoButtonClicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream != null)
            {
                image.Source = ImageSource.FromStream(() => stream);
            }

            (sender as Button).IsEnabled = true;
        }

        private void btnOpenCamera_Clicked(object sender, EventArgs e) 
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await TakePhotoAsync();

                //PhotoImage.Source = ImageSource.FromFile(PhotoPath);
            });
        }


        static void Main(string localFile, Stream stream)
        {
            Console.WriteLine("Azure Cognitive Services Computer Vision - .NET quickstart example");
            Console.WriteLine();

            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

            // Extract text (OCR) from a local image using the Read API
            ReadFileLocal(client, localFile, stream).Wait();

            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Computer Vision quickstart is complete.");
            Console.WriteLine();
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }

        /*
         * AUTHENTICATE
         * Creates a Computer Vision client used by each example.
         */
        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }
        /*
         * END - Authenticate
         */
        /*
             * READ FILE - LOCAL
             */

        public static async Task ReadFileLocal(ComputerVisionClient client, string localFile, Stream stream)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("READ FILE FROM LOCAL");
            string path = "easter-angel-cake-ingredients-list-by-bakethiscake.jpg";

            // Read text from URL
            var textHeaders = await client.ReadInStreamAsync(stream);
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);

            // <snippet_extract_response>
            // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            Console.WriteLine($"Reading text from local file {Path.GetFileName(localFile)}...");
            Console.WriteLine();
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));
            // </snippet_extract_response>

            // <snippet_extract_display>
            // Display the found text.
            Console.WriteLine();

            Console.WriteLine(results.Status);

            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {

                    foreach (int number in line.BoundingBox)
                    {
                        Console.Write(number + " ");
                    }
                    Console.WriteLine();

                    string[] subWords = line.Text.ToString().Split(' ');

                    foreach (string word in subWords)
                    {
                        foreach (string allergen in eggList)
                        {
                            if (word == allergen)
                            {

                                //Console.WriteLine(page.Lines[0].Text);


                                Console.WriteLine("\n Allergen detected \n");

                            }
                        }
                        //Console.WriteLine(word);
                    }
                }
            }
            Console.WriteLine();
        }

        public void btScanImage_Clicked(object sender, EventArgs e)
        {
            try
            {
                //Console.WriteLine(File.Exists(PhotoPath));
                
                //Main(PhotoPath, stream);

                //DependencyService.Get<IImageService>().ScanImage(subscriptionKey, endpoint);

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Console.WriteLine("Error with Scan");
            }
        }
    }
}