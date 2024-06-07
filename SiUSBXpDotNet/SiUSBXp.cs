using System.Reflection;
using System.Runtime.InteropServices;

namespace SiUSBXpDotNet
{
    internal enum SiStatus : byte
    {
        Success = 0x00,
        InvalidHandle = 0x01,
        ReadError = 0x02,
        RxQueueNotReady = 0x03,
        WriteError = 0x04,
        ResetError = 0x05,
        InvalidParameter = 0x06,
        InvalidRequestLength = 0x07,
        DeviceIoFailed = 0x08,
        InvalidBaudrate = 0x09,
        FunctionNotSupported = 0x0A,
        GlobalDataError = 0x0B,
        SystemErrorCode = 0x0C,
        ReadTimedOut = 0x0D,
        WriteTimedOut = 0x0E,
        IoPending = 0x0F,
        NothingToCancel = 0xA0,
        DeviceNotFound = 0xFF,
    }

    internal enum SiProductString : uint
    {
        SerialNumber = 0,
        Description = 1,
        LinkName = 2,
        Vid = 3,
        Pid = 4,
    }

    internal static partial class SiUSBXp
    {
        internal const int MaxDeviceStrLen = 256;
        internal const int MaxReadSize = 4096 * 16;
        internal const int MaxWriteSize = 4096;

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_GetNumDevices")]
        internal static partial SiStatus SI_GetNumDevices(out uint lpdwNumDevices);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_GetDLLVersion")]
        internal static partial SiStatus SI_GetDLLVersion(out uint HighVersion, out uint LowVersion);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_GetProductStringSafe")]
        internal static unsafe partial SiStatus SI_GetProductStringSafe(uint dwDeviceNum, ref byte lpvDeviceString, IntPtr DeviceStringLenInBytes, SiProductString dwFlags);


        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_GetTimeouts")]
        internal static partial SiStatus SI_GetTimeouts(out uint lpdwReadTimeout, out uint lpdwWriteTimeout);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_SetTimeouts")]
        internal static partial SiStatus SI_SetTimeouts(uint dwReadTimeoutInMilliseconds, uint dwWriteTimeoutInMilliseconds);


        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_Open")]
        internal static partial SiStatus SI_Open(uint dwDevice, ref SafeUSBXpressDeviceHandle cyHandle);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_Close")]
        internal static partial SiStatus SI_Close(IntPtr cyHandle);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_GetPartNumber")]
        internal static partial SiStatus SI_GetPartNumber(SafeUSBXpressDeviceHandle cyHandle, out byte lpbPartNum);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_GetInterfaceNumber")]
        internal static partial SiStatus SI_GetInterfaceNumber(SafeUSBXpressDeviceHandle cyHandle, out byte lpbInterfaceNum);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_GetDeviceProductString")]
        internal static partial SiStatus SI_GetDeviceProductString(SafeUSBXpressDeviceHandle cyHandle, ref byte lpProduct, out byte lpbLength, [MarshalAs(UnmanagedType.Bool)] bool bConvertToASCII);

        static SiUSBXp()
        {
            NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);
        }

        private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName != nameof(SiUSBXp))
                return IntPtr.Zero;

            var handle = IntPtr.Zero;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (Environment.Is64BitProcess)
                    NativeLibrary.TryLoad("./runtimes/win-x64/native/SiUSBXp.dll", out handle);
                else
                    NativeLibrary.TryLoad("./runtimes/win-x86/native/SiUSBXp.dll", out handle);
            }

            return handle;
        }
    }
}
