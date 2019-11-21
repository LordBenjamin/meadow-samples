using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Temperature;

namespace HelloMeadow
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        public MeadowApp()
        {
            Debug.Print("Read TMP36");

            //
            //  Create a new TMP36 object to check the temperature every 1s and
            //  to report any changes greater than +/- 0.1C.
            //
            var _tmp36 = new AnalogTemperature(Device,
                Device.Pins.A00,
                AnalogTemperature.KnownSensorType.TMP36,
                updateInterval: 1000,
                temperatureChangeNotificationThreshold: 0.1F);

            //
            //  Connect an interrupt handler.
            //
            _tmp36.TemperatureChanged += (s, e) =>
            {
                Debug.Print("Temperature: " + e.CurrentValue.ToString("f2"));
            };
        }
    }
}
