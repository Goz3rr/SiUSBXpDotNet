using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace SiUSBXpDotNet
{
    public class USBXpressDevice(uint deviceId) : IDisposable
    {
        private SafeUSBXpressDeviceHandle? deviceHandle;
        private bool disposed;

        public uint DeviceID { get; } = deviceId;

        [MemberNotNullWhen(true, nameof(deviceHandle))]
        public bool IsOpen => deviceHandle != null && !deviceHandle.IsInvalid && !deviceHandle.IsClosed;

        public string SerialNumber => GetProductString(SiProductString.SerialNumber);
        public string Description => GetProductString(SiProductString.Description);
        public string LinkName => GetProductString(SiProductString.LinkName);
        public string Pid => GetProductString(SiProductString.Pid);
        public string Vid => GetProductString(SiProductString.Vid);

        public SiLabsPartNum PartNum
        {
            get
            {
                if (!IsOpen)
                    throw new InvalidOperationException();

                _ = SiUSBXp.SI_GetPartNumber(deviceHandle, out var partNum);
                return (SiLabsPartNum)partNum;
            }
        }

        public string ProductString
        {
            get
            {
                if (!IsOpen)
                    throw new InvalidOperationException();

                ReadOnlySpan<byte> buffer = stackalloc byte[SiUSBXp.MaxDeviceStrLen];
                _ = SiUSBXp.SI_GetDeviceProductString(deviceHandle, ref MemoryMarshal.GetReference(buffer), out var len, true);
                return Encoding.UTF8.GetString(buffer[..len]);
            }
        }

        public USBXpressDevice(int deviceId)
            : this((uint)deviceId)
        {
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Close();
            }

            disposed = true;
        }

        private string GetProductString(SiProductString productString)
        {
            ReadOnlySpan<byte> buffer = stackalloc byte[SiUSBXp.MaxDeviceStrLen];
            var status = SiUSBXp.SI_GetProductStringSafe(DeviceID, ref MemoryMarshal.GetReference(buffer), SiUSBXp.MaxDeviceStrLen, productString);
            return Encoding.UTF8.GetString(buffer[..buffer.IndexOf((byte)0)]);
        }

        public void Open()
        {
            if (IsOpen)
                throw new InvalidOperationException();

            deviceHandle?.Dispose();
            deviceHandle = new();

            var status = SiUSBXp.SI_Open(DeviceID, ref deviceHandle);

            if (status != SiStatus.Success)
                Close();
        }

        public void Close()
        {
            deviceHandle?.Close();
            deviceHandle = null;
        }

        public void SetBaudrate(uint baudrate)
        {
            if (!IsOpen)
                throw new InvalidOperationException();

            var status = SiUSBXp.SI_SetBaudRate(deviceHandle, baudrate);

            if (status == SiStatus.InvalidBaudrate)
                throw new ArgumentOutOfRangeException(nameof(baudrate), "Invalid baud rate");
        }

        public void SetBreak(bool breakState)
        {
            if (!IsOpen)
                throw new InvalidOperationException();

            _ = SiUSBXp.SI_SetBreak(deviceHandle, (ushort)(breakState ? 1 : 0));

        }

        public void FlushBuffers(bool flushTransmit = true, bool flushReceive = true)
        {
            if (!IsOpen)
                throw new InvalidOperationException();

            _ = SiUSBXp.SI_FlushBuffers(deviceHandle, (byte)(flushTransmit ? 1 : 0), (byte)(flushReceive ? 1 : 0));
        }

        // https://usermanual.wiki/Document/AN1692020USBXpress20Programmers20Guide.1429687653/html#pfc
        // https://www.silabs.com/documents/public/application-notes/AN169.pdf
    }
}
