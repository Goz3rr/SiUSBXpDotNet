using System.Reflection;
using System.Runtime.InteropServices;

namespace SiUSBXpDotNet
{
    internal enum SiStatus
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

    internal static partial class SiUSBXp
    {
        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_GetNumDevices")]
        internal static partial SiStatus SI_GetNumDevices(out uint lpdwNumDevices);

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
