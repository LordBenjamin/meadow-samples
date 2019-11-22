using System;
using Meadow.Hardware;

namespace HelloMeadow
{
    class MAX7219
    {
        private const byte IntensityOpCode = 0xA;
        private const byte ScanLimitOpCode = 0xB;
        private const byte ShutdownOpCode = 0xC;
        private const byte DisplayTestOpCode = 0xF;

        private IDigitalOutputPort dinPort;
        private IDigitalOutputPort csPort;
        private IDigitalOutputPort clkPort;

        // 2 bytes (opcode + data) * 8 segments
        private byte[] buffer;

        public MAX7219(IIODevice device, IPin din, IPin cs, IPin clk, int displayCount = 1)
        {
            Device = device;
            DisplayCount = displayCount < 0 || displayCount > 8 ? 8 : displayCount;

            buffer = new byte[DisplayCount * 2];

            dinPort = device.CreateDigitalOutputPort(din);
            csPort = device.CreateDigitalOutputPort(cs);
            clkPort = device.CreateDigitalOutputPort(clk);

            csPort.State = true;
        }

        public IIODevice Device { get; }
        public int DisplayCount { get; }

        public void Wake()
        {
            SendCommand(ShutdownOpCode, 1); // Essentially enable normal operation by telling it not to be shut down
        }

        internal void SetScanLimit(byte value)
        {
            SendCommand(ScanLimitOpCode, 0);
        }

        internal void StartDisplayTest()
        {
            SendCommand(DisplayTestOpCode, 1);
        }

        internal void StopDisplayTest()
        {
            SendCommand(DisplayTestOpCode, 0);
        }

        internal void Shutdown()
        {
            SendCommand(ShutdownOpCode, 0);
        }

        internal void SetIntensity(byte value)
        {
            SendCommand(IntensityOpCode, value);
        }

        private void SendCommand(byte opCode, byte data)
        {
            for (int i = 0; i < DisplayCount; i++)
            {
                SendCommand(i, opCode, data);
            }
        }

        private void SendCommand(int cell, byte opCode, byte data)
        {
            int offset = cell * 2;

            Array.Clear(buffer, 0, buffer.Length);

            buffer[offset] = data;
            buffer[offset + 1] = opCode;

            csPort.State = false;

            for (int i = buffer.Length; i > 0; i--)
            {
                ShiftOut(buffer[i - 1]);
            }

            csPort.State = true;
            clkPort.State = false;
        }

        private void ShiftOut(byte value)
        {
            for (int i = 0; i < 8; i++)
            {
                clkPort.State = false;

                dinPort.State = GetBit(value, i);

                clkPort.State = true;
            }
        }

        // Assume 0 is the MSB andd 7 is the LSB.
        public static bool GetBit(byte value, int index)
        {
            if (index < 0 || index > 7)
                throw new ArgumentOutOfRangeException();

            int shift = 7 - index;

            // Get a single bit in the proper position.
            byte bitMask = (byte)(1 << shift);

            // Mask out the appropriate bit.
            byte masked = (byte)(value & bitMask);

            // If masked != 0, then the masked out bit is 1.
            // Otherwise, masked will be 0.
            return masked != 0;
        }
    }
}
