using System.Runtime.InteropServices;
using System.Text;

namespace SiUSBXpDotNet
{
    public enum SiLabsPartNum : byte
    {
        Unknown = 0x0,
        CP2101 = 0x1,
        CP2102 = 0x2,
        CP2103 = 0x3,
        CP2104 = 0x4,
        CP2105 = 0x5,
        CP2108 = 0x8,
        CP2109 = 0x9,
        CP2110 = 0xA,
        CP2112 = 0xC,
        CP2114 = 0xE,
        CP2102N_QFN28 = 0x20,
        CP2102N_QFN24 = 0x21,
        CP2102N_QFN20 = 0x22,
        EFM8 = 0x80,
        EFM32 = 0x81
    }

    public class USBXpressDevice(uint deviceId) : IDisposable
    {
        private SafeUSBXpressDeviceHandle? deviceHandle;
        private bool disposed;

        public uint DeviceID { get; } = deviceId;
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

                _ = SiUSBXp.SI_GetPartNumber(deviceHandle!, out var partNum);
                return (SiLabsPartNum)partNum;
            }
        }

        public byte InterfaceNum
        {
            get
            {
                if (!IsOpen)
                    throw new InvalidOperationException();

                _ = SiUSBXp.SI_GetInterfaceNumber(deviceHandle!, out var interfaceNum);
                return interfaceNum;
            }
        }

        public string ProductString
        {
            get
            {
                if (!IsOpen)
                    throw new InvalidOperationException();

                ReadOnlySpan<byte> buffer = stackalloc byte[SiUSBXp.MaxDeviceStrLen];
                _ = SiUSBXp.SI_GetDeviceProductString(deviceHandle!, ref MemoryMarshal.GetReference(buffer), out var len, true);
                return Encoding.UTF8.GetString(buffer[..len]);
            }
        }

        public USBXpressDevice(int deviceId)
            : this((uint)deviceId)
        {
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
    }
}
