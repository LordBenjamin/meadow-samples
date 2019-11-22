using System;
using System.Diagnostics;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Hardware;

namespace HelloMeadow
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        private readonly MAX7219 display;

        private IDigitalOutputPort ledPort;

        public MeadowApp()
        {
            Console.WriteLine("Creating Outputs...");

            ledPort = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedGreen);

            display = new MAX7219(
                Device,
                din: Device.Pins.D00,
                cs: Device.Pins.D01,
                clk: Device.Pins.D02,
                displayCount: 1);

            Console.WriteLine("Run...");
            Run();
        }

        private void Run()
        {
            ledPort.State = true;

            Console.WriteLine("Display test...");
            display.StartDisplayTest();
            Thread.Sleep(1000);
            display.StopDisplayTest();

            Console.WriteLine("Intensity...");
            display.SetIntensity(0xF);
            Console.WriteLine("Scan Limit...");
            display.SetScanLimit(0x7);
            Console.WriteLine("Decode mode = on...");
            display.SetDecodeModeOn();

            Console.WriteLine("Clear...");
            display.ClearDisplay();

            Console.WriteLine("Wake from shutdown...");
            display.Wake();

            Console.WriteLine("Set digits...");

            for (int val = 0; val < 99999999; val++)
            {
                display.SetValue(val);
            }

            Thread.Sleep(5000);

            Console.WriteLine("Shutdown...");
            display.Shutdown();
            ledPort.State = false;
        }
    }
}

