namespace SiUSBXpDotNet
{
    public class USBXpress
    {
        public static int NumDevices
        {
            get
            {
                _ = SiUSBXp.SI_GetNumDevices(out var numDevices);
                return (int)numDevices;
            }
        }

        public static IEnumerable<USBXpressDevice> GetDevices()
        {
            var devices = new List<USBXpressDevice>();

            for (uint i = 0; i < NumDevices; i++)
                devices.Add(new USBXpressDevice(i));

            return devices;
        }
    }
}
