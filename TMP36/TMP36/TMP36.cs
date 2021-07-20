using System.Threading.Tasks;
using Meadow.Hardware;

namespace BenjaminOwen.Meadow.Sensors.Temperature
{
    internal class TMP36
    {
        private const float yIntercept = 500;
        private const float millivoltsPerDegreeC = 10;

        private IAnalogInputPort analogPort;

        public TMP36(IAnalogInputController device, IPin pin)
        {
            analogPort = device.CreateAnalogInputPort(pin, voltageReference: 3.27f);
        }

        public async Task<double> ReadAsync()
        {
            double millivolts = (await analogPort.Read()).Millivolts;
            return (millivolts - yIntercept) / millivoltsPerDegreeC;
        }
    }
}
