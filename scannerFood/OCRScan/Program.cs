using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Linq;

namespace OCRScan
{
    class Program
    {
        // <snippet_vars>
        // Add your Computer Vision subscription key and endpoint   
        static readonly string subscriptionKey = "c1f515eaa1ef4950b0eaee273ff256c4";
        static readonly string endpoint = "https://allergycomputervision.cognitiveservices.azure.com/";
        // </snippet_vars>
        // </snippet_using_and_vars>

        static readonly string[] eggNames = { "egg", "eggs", "egg powder", "dried eggs", "egg solids", "albumin" };
        static readonly List<string> eggList = new List<string>(eggNames);

        // Download these images (link in prerequisites), or you can use any appropriate image on your local machine.
        private const string READ_TEXT_LOCAL_IMAGE = "DavidDisappoint.jpg";

        // <snippet_readtext_url>
        private const string READ_TEXT_URL_IMAGE = "https://intelligentkioskstore.blob.core.windows.net/visionapi/suggestedphotos/3.png";
        // </snippet_readtext_url>

        static void Main(string[] args)
        {
            Console.WriteLine("Azure Cognitive Services Computer Vision - .NET quickstart example");
            Console.WriteLine();

            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

            // Extract text (OCR) from a local image using the Read API
            ReadFileLocal(client, READ_TEXT_LOCAL_IMAGE).Wait();

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
        /*
         * END - READ FILE - LOCAL
         */
        // </snippet_read_local>
    }
}
