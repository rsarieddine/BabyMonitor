using BabyMonitorApp.Model;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text;

using System.Threading.Tasks;

using System.Threading.Tasks;

namespace BabyMonitorApp.Helpers
{
    public class IoTHubConnector
    {
        public string DeviceID = "1";
        private static IoTHubConnector instance;

        private IoTHubConnector()
        {
        }

        public static IoTHubConnector GetInstance()
        {
            if (null == instance) instance = new IoTHubConnector();
            return instance;
        }

        public async Task SendDataToAzure(RealTimeUnit data)
        {
            var deviceClient = DeviceClient.CreateFromConnectionString("HostName=MEAHackIoTHub.azure-devices.net;DeviceId=AmbientDeviceA;SharedAccessKey=kP0Hs+6UT5WGIDFXOHnELXtcVN3PRg7snSle3YXVu94=", TransportType.Http1);

            var json = JsonConvert.SerializeObject(data, Formatting.Indented);

            var msg = new Message(Encoding.UTF8.GetBytes(json));

            try
            {
                await deviceClient.SendEventAsync(msg);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task ReceiveDataFromAzure()
        {
            var deviceClient = DeviceClient.CreateFromConnectionString("HostName=MEAHackIoTHub.azure-devices.net;DeviceId=AmbientDeviceA;SharedAccessKey=kP0Hs+6UT5WGIDFXOHnELXtcVN3PRg7snSle3YXVu94=", TransportType.Http1);

            Message receivedMessage;
            string messageData;

            while (true)
            {
                receivedMessage = await deviceClient.ReceiveAsync();

                if (receivedMessage != null)
                {
                    messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    await deviceClient.CompleteAsync(receivedMessage);
                }
            }
        }
    }
}