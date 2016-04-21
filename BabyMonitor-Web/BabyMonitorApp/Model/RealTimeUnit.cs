using BabyMonitorApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApp.Model
{
    public class RealTimeUnit : NotifyChangeBase
    {
        private string deviceID;

        public string DeviceID
        {
            get { return deviceID; }
            set { SetProperty(ref deviceID, value); }
        }

        private string sensorname;

        public string SensorName
        {
            get { return sensorname; }
            set { SetProperty(ref sensorname, value); }
        }

        private float sensorvalue;

        public float SensorValue
        {
            get { return sensorvalue; }
            set { SetProperty(ref sensorvalue, value); }
        }

        private DateTime timestamp;

        public DateTime Timestamp
        {
            get { return timestamp; }
            set { SetProperty(ref timestamp, value); }
        }
    }
}