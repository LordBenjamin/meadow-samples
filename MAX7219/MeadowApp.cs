using System;
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
            Console.WriteLine("Intensity...");
            display.SetIntensity(1);
            Console.WriteLine("Scan Limit...");
            display.SetScanLimit(0x7);

            Console.WriteLine("Display test...");
            display.StartDisplayTest();
            Thread.Sleep(1000);
            display.StopDisplayTest();

            Console.WriteLine("Wake from shutdown...");
            display.Wake();

            Thread.Sleep(1000);

            Console.WriteLine("Shutdown...");
            display.Shutdown();
            ledPort.State = false;
        }
    }
}

