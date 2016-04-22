using BabyMonitorApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace BabyMonitorApp.ViewModel
{
    public class MainView : NotifyChangeBase
    {
        private DispatcherTimer timer;

        public MainView()
        {
            Init();
        }

        public async Task Init()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += Timer_Tick;

            timer.Start();

            CurrentHumidity = 40;
            CurrentTemperature = 24;
        }

        private async void Timer_Tick(object sender, object e)
        {
            //CurrentTemperature = IoTPiHelper.GetInstance().GetTemperature();

            await IoTHubConnector.GetInstance().SendDataToAzure(new Model.RealTimeUnit() { DeviceID = "1", SensorName = "AmbientTemperature", SensorValue = CurrentTemperature, Timestamp = DateTime.Now });

            //CurrentHumidity = IoTPiHelper.GetInstance().GetHumidity();
            await IoTHubConnector.GetInstance().SendDataToAzure(new Model.RealTimeUnit() { DeviceID = "1", SensorName = "AmbientHumidity", SensorValue = CurrentHumidity, Timestamp = DateTime.Now });
        }

        private BaseCommand testSend;

        public BaseCommand TestSend
        {
            get { return testSend; }
            set { testSend = value; }
        }

        private float currentTemperature;

        public float CurrentTemperature
        {
            get { return currentTemperature; }
            set { SetProperty(ref currentTemperature, value); }
        }

        private float currentHumidity;

        public float CurrentHumidity
        {
            get { return currentHumidity; }
            set { SetProperty(ref currentHumidity, value); }
        }
    }
}