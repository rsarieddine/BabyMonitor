using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Band;


namespace BabyMonitorApp.Helpers
{
    public class MicrosoftBandHelper
    {
        private MicrosoftBandHelper()
        {
           


        }
        private static MicrosoftBandHelper instance;
        private IBandClient bandClient;
        private bool running;
        private int samplesReceived = 0;
        private IBandInfo[] pairedBands = null;

        private string responseString = String.Empty;

        public int CurrentAmbientLight { get; private set; }
        public int CurrentHeartRate { get; private set; }
        public int CurrentGsr { get; private set; }
        public double CurrentSkinTemp { get; private set; }

        public static MicrosoftBandHelper  GetInstance()
        {
            if (null == instance) instance = new MicrosoftBandHelper();
                instance.Initialize();
                return instance;
        }

        private async void Initialize()
        {
            try
            {
                pairedBands = await BandClientManager.Instance.GetBandsAsync();
                // Connect to Microsoft Band.
                bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]);

                if (pairedBands.Length < 1)
                {
                    this.responseString  = "This application requires a Microsoft Band paired to your device.";
                    return;
                }

                await CheckSensorPermissions();

                {


                    if (await CheckSensorSupport()) return;


                    running = true;




                    while (running)
                    {
                        // Receive AmbientLight data for a while, then stop the subscription.
                        await Task.Delay(TimeSpan.FromSeconds(5));
                        this.responseString = String.Format("Heart Rate : {0} \n Ambient Light : {1} \n Skin Temp : {2} \n GSR ; {3}",
                                                                    CurrentHeartRate.ToString(),
                                                                    CurrentAmbientLight.ToString(),
                                                                    CurrentSkinTemp.ToString(),
                                                                    CurrentGsr.ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                this.responseString = ex.ToString();
            }
        

        }

        private async void Terminate()
        {
            try
            {
                await bandClient.SensorManager.HeartRate.StopReadingsAsync();
                await bandClient.SensorManager.AmbientLight.StopReadingsAsync();
                await bandClient.SensorManager.SkinTemperature.StopReadingsAsync();
                await bandClient.SensorManager.Gsr.StopReadingsAsync();

                this.responseString = "Not Monitoring Anymore";
                running = false;
            }
            catch (Exception ex)
            {

                this.responseString = ex.ToString();
            }

        }

        private async Task<bool> CheckSensorSupport()
        {
            if (await CheckAmbientLightSupport()) return true;
            if (await CheckHeartRateSupport()) return true;
            if (await CheckSkinTempSupport()) return true;
            if (await CheckGsrSupport()) return true;
            return false;
        }

        private async Task CheckSensorPermissions()
        {
            await CheckHeartRatePermissions();
            await CheckGsrPermissions();
            await CheckSkinTempPermissions();
            await CheckAmbientLightPermissions();
        }

        private async Task<bool> CheckGsrSupport()
        {
            if (!bandClient.SensorManager.Gsr.IsSupported)
            {
                this.responseString =
                    "Gsr sensor is not supported with your Band version. Microsoft Band 2 is required.";
                return true;
            }
            else
            {
                // Subscribe to GSR data.
                bandClient.SensorManager.Gsr.ReadingChanged += (s, args) =>
                {
                    samplesReceived++;
                    CurrentGsr = args.SensorReading.Resistance;
                };
                await bandClient.SensorManager.Gsr.StartReadingsAsync();
            }
            return false;
        }

        private async Task<bool> CheckSkinTempSupport()
        {
            if (!bandClient.SensorManager.SkinTemperature.IsSupported)
            {
                this.responseString =
                    "Skin Temperature sensor is not supported with your Band version. Microsoft Band 2 is required.";
                return true;
            }
            else
            {
                // Subscribe to SkinTemp data.
                bandClient.SensorManager.SkinTemperature.ReadingChanged += (s, args) =>
                {
                    samplesReceived++;
                    CurrentSkinTemp = args.SensorReading.Temperature;
                };
                await bandClient.SensorManager.SkinTemperature.StartReadingsAsync();
            }
            return false;
        }

        private async Task<bool> CheckHeartRateSupport()
        {
            if (!bandClient.SensorManager.HeartRate.IsSupported)
            {
                this.responseString =
                    "HeartRate sensor is not supported with your Band version. Microsoft Band 2 is required.";
                return true;
            }
            else
            {
                // Subscribe to HeartRate data.
                bandClient.SensorManager.HeartRate.ReadingChanged += (s, args) =>
                {
                    samplesReceived++;
                    CurrentHeartRate = args.SensorReading.HeartRate;
                };
                await bandClient.SensorManager.HeartRate.StartReadingsAsync();
            }
            return false;
        }

        private async Task<bool> CheckAmbientLightSupport()
        {
            if (!bandClient.SensorManager.AmbientLight.IsSupported)
            {
                this.responseString =
                    "AmbientLight sensor is not supported with your Band version. Microsoft Band 2 is required.";
                return true;
            }
            else
            {
                // Subscribe to AmbientLight data.
                bandClient.SensorManager.AmbientLight.ReadingChanged += (s, args) =>
                {
                    samplesReceived++;
                    CurrentAmbientLight = args.SensorReading.Brightness;
                };
                await bandClient.SensorManager.AmbientLight.StartReadingsAsync();
            }
            return false;
        }

        private async Task CheckSkinTempPermissions()
        {
            bool SkinTempConsentGranted;


            if (bandClient.SensorManager.SkinTemperature.GetCurrentUserConsent() == UserConsent.Granted)
            {
                SkinTempConsentGranted = true;
            }
            else
            {
                SkinTempConsentGranted = await bandClient.SensorManager.SkinTemperature.RequestUserConsentAsync();
            }
        }

        private async Task CheckGsrPermissions()
        {
            bool GSRConsentGranted;


            if (bandClient.SensorManager.Gsr.GetCurrentUserConsent() == UserConsent.Granted)
            {
                GSRConsentGranted = true;
            }
            else
            {
                GSRConsentGranted = await bandClient.SensorManager.Gsr.RequestUserConsentAsync();
            }
        }

        private async Task CheckHeartRatePermissions()
        {
            bool heartRateConsentGranted;


            if (bandClient.SensorManager.HeartRate.GetCurrentUserConsent() == UserConsent.Granted)
            {
                heartRateConsentGranted = true;
            }
            else
            {
                heartRateConsentGranted = await bandClient.SensorManager.HeartRate.RequestUserConsentAsync();
            }
        }

        private async Task CheckAmbientLightPermissions()
        {
            bool AmbientLightConsentGranted;


            if (bandClient.SensorManager.AmbientLight.GetCurrentUserConsent() == UserConsent.Granted)
            {
                AmbientLightConsentGranted = true;
            }
            else
            {
                AmbientLightConsentGranted = await bandClient.SensorManager.AmbientLight.RequestUserConsentAsync();
            }
        }


    }


}
