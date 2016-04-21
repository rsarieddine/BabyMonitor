using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace BabyMonitorApp.Helpers
{
    public class IoTPiHelper
    {
        private static IoTPiHelper instance;
        private GpioController controller;
        private GpioPin greenledpin;
        private GpioPin yellowledpin;
        private GpioPin redledpin;
        private GpioPin temphumidtypin;

        private IoTPiHelper()
        {
        }

        public static IoTPiHelper GetInstance()
        {
            if (null == instance) instance = new IoTPiHelper();

            return instance;
        }

        public float GetTemperature()
        {
            return 0;
        }

        public float GetHumidity()
        {
            return 0;
        }

        public void SetGreen(bool onoff)
        {
        }

        public void SetYellow(bool onoff)
        {
        }

        public void SetRed(bool onoff)
        {
        }
    }
}