using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;

namespace BabyMonitorApp.Helpers
{
    public class MediaStreamHelper
    {
        private static MediaStreamHelper instance;
        private MediaCapture camera;

        private MediaStreamHelper()
        {
        }

        public static MediaStreamHelper GetInstance()
        {
            if (null == instance) instance = new MediaStreamHelper();
            return instance;
        }
    }
}