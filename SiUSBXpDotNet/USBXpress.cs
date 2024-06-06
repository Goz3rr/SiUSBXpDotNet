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
    }
}
