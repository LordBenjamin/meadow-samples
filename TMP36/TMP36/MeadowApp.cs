using System;
using System.Threading.Tasks;
using BenjaminOwen.Meadow.Sensors.Temperature;
using Meadow;
using Meadow.Devices;
using Meadow.Units;

namespace BenjaminOwen.Meadow.Samples.TMP36Temperature
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        private readonly TMP36 tmp36;

        public MeadowApp()
        {
            tmp36 = new TMP36(Device, Device.Pins.A00);
            Run().GetAwaiter().GetResult();
        }

        private async Task Run()
        {
            while (true)
            {
                Temperature temp = await tmp36.ReadAsync()
                    .ConfigureAwait(false);

                Console.WriteLine("{0:N2} °C", temp.Celsius);
            }
        }
    }
}
