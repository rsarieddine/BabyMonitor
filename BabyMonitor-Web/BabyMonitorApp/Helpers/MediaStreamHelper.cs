using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Microsoft.WindowsAzure.MediaServices.Client;
using System.Configuration;
using System.IO;
using System.Threading;

namespace BabyMonitorApp.Helpers
{
    public class MediaStreamHelper
    {
        private static MediaStreamHelper instance;
        private MediaCapture camera;

        // Read values from the App.config file.
        private static readonly string _mediaServicesAccountName =
            ConfigurationManager.AppSettings["MediaServicesAccountName"];
        private static readonly string _mediaServicesAccountKey =
            ConfigurationManager.AppSettings["MediaServicesAccountKey"];

        // Field for service context.
        private static CloudMediaContext _context = null;
        private static MediaServicesCredentials _cachedCredentials = null;


        private MediaStreamHelper()
        {
        }

        public static MediaStreamHelper GetInstance()
        {
            if (null == instance) instance = new MediaStreamHelper();
            return instance;
        }

        private static void Initalize()
        {
            try
            {
                // Create and cache the Media Services credentials in a static class variable.
                _cachedCredentials = new MediaServicesCredentials(
                                _mediaServicesAccountName,
                                _mediaServicesAccountKey);
                // Used the chached credentials to create CloudMediaContext.
                _context = new CloudMediaContext(_cachedCredentials);



                // If you want to secure your high quality input media files with strong encryption at rest on disk,
                // use AssetCreationOptions.StorageEncrypted instead of AssetCreationOptions.None.


                IAsset inputAsset =
                    UploadFile("SET FILE NAME", AssetCreationOptions.None);


                //No config
                var JobState = RunIndexingJob(inputAsset);


            }
            catch (Exception exception)
            {
                // Parse the XML error message in the Media Services response and create a new
                // exception with its content.
                exception = MediaServicesExceptionParser.Parse(exception);

                //Add Output Options Here
                //Response.Write(....);
            }
            finally
            {
                //Do Some Cleanup

            }



        }


        static public IAsset UploadFile(string fileName, AssetCreationOptions options)
        {
            IAsset inputAsset = _context.Assets.CreateFromFile(
                fileName,
                options,
                (af, p) =>
                {
                    Console.WriteLine("Uploading '{0}' - Progress: {1:0.##}%", af.Name, p.Progress);
                });

            Console.WriteLine("Asset {0} created.", inputAsset.Id);

            return inputAsset;
        }


        static bool RunIndexingJob(IAsset inputAsset, string configurationFile = "")
        {
            // Declare a new job.
            IJob job = _context.Jobs.Create(String.Format("{0} File Indexing Job", inputAsset.Name));

            // Get a reference to the Azure Media Indexer.
            string MediaProcessorName = "Azure Media Indexer";
            IMediaProcessor processor = GetLatestMediaProcessorByName(MediaProcessorName);

            // Read configuration from file if specified.
            string configuration = string.IsNullOrEmpty(configurationFile) ? "" : File.ReadAllText(configurationFile);

            // Create a task with the encoding details, using a string preset.
            ITask task = job.Tasks.AddNew("File Indexing Task",
                processor,
                configuration,
                TaskOptions.None);

            // Specify the input asset to be indexed.
            task.InputAssets.Add(inputAsset);

            // Add an output asset to contain the results of the job.
            task.OutputAssets.AddNew(String.Format("{0} Output Asset", inputAsset.Name), AssetCreationOptions.None);

            // Use the following event handler to check job progress.  
            job.StateChanged += new EventHandler<JobStateChangedEventArgs>(StateChanged);

            // Launch the job.
            job.Submit();

            // Check job execution and wait for job to finish.
            Task progressJobTask = job.GetExecutionProgressTask(CancellationToken.None);

            progressJobTask.Wait();

            // If job state is Error, the event handling
            // method for job progress should log errors.  Here we check
            // for error state and exit if needed.
            if (job.State == JobState.Error)
            {
                //Add Output Options Here
                //Response.Write(....);
                return false;
            }

            return true;
        }

        static IMediaProcessor GetLatestMediaProcessorByName(string mediaProcessorName)
        {
            var processor = _context.MediaProcessors
            .Where(p => p.Name == mediaProcessorName)
            .ToList()
            .OrderBy(p => new Version(p.Version))
            .LastOrDefault();

            if (processor == null)
                throw new ArgumentException(string.Format("Unknown media processor",
                                                           mediaProcessorName));

            return processor;
        }

        static void StateChanged(object sender, JobStateChangedEventArgs e)
        {
            switch (e.CurrentState)
            {
                case JobState.Finished:
                    //Add Output Options Here
                    //Response.Write(....);
                    break;
                case JobState.Canceling:
                case JobState.Queued:
                case JobState.Scheduled:
                case JobState.Processing:
                    //Add Output Options Here
                    //Response.Write(....);
                    break;
                case JobState.Canceled:
                    //Add Output Options Here
                    //Response.Write(....);
                    break;
                case JobState.Error:
                    //Add Output Options Here
                    //Response.Write(....);
                    break;
                default:
                    break;
            }
        }

    }
}