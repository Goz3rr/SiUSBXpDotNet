using System.Runtime.InteropServices;
using System.Text;

namespace SiUSBXpDotNet
{
    public class USBXpressDevice(uint deviceId)
    {
        public uint DeviceID { get; } = deviceId;

        private string? _serialNum;
        public string SerialNumber => _serialNum ??= GetProductString(SiProductString.SerialNumber);

        private string? _description;
        public string Description => _description ??= GetProductString(SiProductString.Description);

        private string? _linkName;
        public string LinkName => _linkName ??= GetProductString(SiProductString.LinkName);

        private string? _pid;
        public string Pid => _pid ??= GetProductString(SiProductString.Pid);

        private string? _vid;
        public string Vid => _vid ??= GetProductString(SiProductString.Vid);

        private string GetProductString(SiProductString productString)
        {
            ReadOnlySpan<byte> buffer = stackalloc byte[128];
            var status = SiUSBXp.SI_GetProductStringSafe(DeviceID, out MemoryMarshal.GetReference(buffer), 128, productString);
            return Encoding.UTF8.GetString(buffer[..buffer.IndexOf((byte)0)]);
        }

        public void Open()
        {
            
        }
    }
}
