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

        public static TimeSpan ReadTimeout
        {
            get
            {
                _ = SiUSBXp.SI_GetTimeouts(out var readTimeout, out var _);

                if (readTimeout == 0xFFFFFFFF)
                    return Timeout.InfiniteTimeSpan;

                return TimeSpan.FromMilliseconds(readTimeout);
            }

            set
            {
                var newValue = (uint)value.TotalMilliseconds;
                if (value == Timeout.InfiniteTimeSpan)
                    newValue = 0xFFFFFFFF;

                _ = SiUSBXp.SI_SetTimeouts(newValue, (uint)WriteTimeout.TotalMilliseconds);
            }
        }

        public static TimeSpan WriteTimeout
        {
            get
            {
                _ = SiUSBXp.SI_GetTimeouts(out var _, out var writeTimeout);

                if (writeTimeout == 0xFFFFFFFF)
                    return Timeout.InfiniteTimeSpan;

                return TimeSpan.FromMilliseconds(writeTimeout);
            }

            set
            {
                var newValue = (uint)value.TotalMilliseconds;
                if (value == Timeout.InfiniteTimeSpan)
                    newValue = 0xFFFFFFFF;

                _ = SiUSBXp.SI_SetTimeouts((uint)ReadTimeout.TotalMilliseconds, newValue);
            }
        }

        public static IEnumerable<USBXpressDevice> GetDevices() => Enumerable.Range(0, NumDevices).Select(i => new USBXpressDevice(i));

        public static Version GetDLLVersion()
        {
            _ = SiUSBXp.SI_GetDLLVersion(out var high, out var low);

            return new Version((int)high, (int)low);
        }
    }
}
