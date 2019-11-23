using System.Threading.Tasks;
using Meadow.Hardware;

namespace BenjaminOwen.Meadow.Sensors.Temperature
{
    internal class TMP36
    {
        private const float yIntercept = 500;
        private const float millivoltsPerDegreeC = 10;

        private IAnalogInputPort analogPort;

        public TMP36(IIODevice device, IPin pin)
        {
            analogPort = device.CreateAnalogInputPort(pin);
        }

        public async Task<float> ReadAsync()
        {
            float millivolts = await analogPort.Read(1) * 1000;
            return (millivolts - yIntercept) / millivoltsPerDegreeC;
        }
    }
}
