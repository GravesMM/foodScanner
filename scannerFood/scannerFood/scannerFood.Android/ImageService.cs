using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using scannerFood.Droid;
using scannerFood.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(ImageService))]
namespace scannerFood.Droid
{

    public class ImageService : IImageService
    {
        public void ScanImage(string subscriptionKey, string endpoint)
        {

            Console.WriteLine(GetRootPath());
            Console.WriteLine("Azure Cognitive Services Computer Vision - .NET quickstart example");
            Console.WriteLine();


            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

            // Extract text (OCR) from a local image using the Read API
            ReadFileLocal(client, "/storage/emulated/0/Android/data/com.companyname.scannerfood/files/easter-angel-cake-ingredients-list-by-bakethiscake.jpg").Wait();

            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Computer Vision quickstart is complete.");
            Console.WriteLine();
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();





            //var filename = "testfile.txt";
            //var destination = Path.Combine(GetRootPath(), filename);

            //File.WriteAllText(destination, localFile);

            //string text = File.ReadAllText(destination);
            //Console.WriteLine(text);
        } 

        public string GetRootPath()
        {
            return Application.Context.GetExternalFilesDir(null).ToString();
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

        public static async Task ReadFileLocal(ComputerVisionClient client, string localFile)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("READ FILE FROM LOCAL");
            Console.WriteLine();

            // Read text from URL
            var textHeaders = await client.ReadInStreamAsync(File.OpenRead(localFile));
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

                    //foreach (string word in subWords)
                    //{
                    //    foreach (string allergen in eggList)
                    //    {
                    //        if (word == allergen)
                    //        {

                    //            //Console.WriteLine(page.Lines[0].Text);


                    //            Console.WriteLine("\n Allergen detected \n");

                    //        }
                    //    }
                    //    //Console.WriteLine(word);
                    //}
                }
            }
            Console.WriteLine();
        }
        /*
         * END - READ FILE - LOCAL
         */
        // </snippet_read_local>

        public void CopyImage(Xamarin.Forms.Image image)
        {
            throw new NotImplementedException();
        }

    }
}