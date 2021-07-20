using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using BenjaminOwen.Meadow.Displays;
using BenjaminOwen.Meadow.Sensors.Temperature;
using System.Threading.Tasks;
using Meadow.Units;

namespace BenjaminOwen.Meadow.Samples.DigitalThermometer
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        private readonly MAX7219 display;
        private readonly TMP36 tmp36;

        public MeadowApp()
        {
            Console.WriteLine("Creating devices...");

            display = new MAX7219(
                Device,
                din: Device.Pins.D00,
                cs: Device.Pins.D01,
                clk: Device.Pins.D02,
                displayCount: 1);

            tmp36 = new TMP36(Device, Device.Pins.A00);

            Console.WriteLine("Run...");
            RunAsync().GetAwaiter().GetResult();
        }

        private async Task RunAsync()
        {
            // Init display
            display.StopDisplayTest();
            display.SetIntensity(0xF);
            display.SetScanLimit(0x7);
            display.SetDecodeModeOn();
            display.ClearDisplay();
            display.Wake();

            int lastTemperature = 0;

            // Poll TMP36 and update display when temperature changes
            while (true)
            {
                Temperature temperature = await tmp36.ReadAsync()
                    .ConfigureAwait(false);

                int roundedTemperature = (int)Math.Round(temperature.Celsius, 0);

                if (roundedTemperature != lastTemperature)
                {
                    display.SetValue(roundedTemperature);
                    lastTemperature = roundedTemperature;
                    Thread.Sleep(500);
                }
            }
        }
    }
}

