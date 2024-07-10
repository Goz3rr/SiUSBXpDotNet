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

    internal enum SiRxQueueStatus : uint
    {
        Empty = 0,
        NoOverrun = Empty,
        Overrun = 1,
        Ready = 2,
    }

    internal enum SiPinCharacteristic
    {
        HeldInactive = 0,
        StatusInput = HeldInactive,
        HeldActive = 1,
        HandshakeLine= HeldActive,
        FirmwareControlled = 2,
        ReceiveFlowControl = 3,
    }

    internal enum SiGpio : byte
    {
        Gpio0 = 1 << 0,
        Gpio1 = 1 << 1,
        Gpio2 = 1 << 2,
        Gpio3 = 1 << 3,
        Gpio4 = 1 << 4,
        Gpio5 = 1 << 5,
        Gpio6 = 1 << 6,
        Gpio7 = 1 << 7,
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

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_GetDeviceProductString")]
        internal static partial SiStatus SI_GetDeviceProductString(SafeUSBXpressDeviceHandle cyHandle, ref byte lpProduct, out byte lpbLength, [MarshalAs(UnmanagedType.Bool)] bool bConvertToASCII);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_SetBaudRate")]
        internal static partial SiStatus SI_SetBaudRate(SafeUSBXpressDeviceHandle cyHandle, uint dwBaudRate);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_SetBreak")]
        internal static partial SiStatus SI_SetBreak(SafeUSBXpressDeviceHandle cyHandle, ushort wBreakState);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_SetFlowControl")]
        internal static partial SiStatus SI_SetFlowControl(SafeUSBXpressDeviceHandle cyHandle, byte bCTS_MaskCode, byte bRTS_MaskCode, byte bDTR_MaskCode, byte bDSR_MaskCode, byte bDCD_MaskCode, byte bFlowXonXoff);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_SetLineControl")]
        internal static partial SiStatus SI_SetLineControl(SafeUSBXpressDeviceHandle cyHandle, ushort wLineControl);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_GetModemStatus")]
        internal static partial SiStatus SI_GetModemStatus(SafeUSBXpressDeviceHandle cyHandle, out byte ModemStatus);


        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_ReadLatch")]
        internal static partial SiStatus SI_ReadLatch(SafeUSBXpressDeviceHandle cyHandle, out byte lpbLatch);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_WriteLatch")]
        internal static partial SiStatus SI_WriteLatch(SafeUSBXpressDeviceHandle cyHandle, byte bMask, byte bLatch);


        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_CheckRXQueue")]
        internal static partial SiStatus SI_CheckRXQueue(SafeUSBXpressDeviceHandle cyHandle, out uint lpdwNumBytesInQueue, out SiRxQueueStatus lpdwQueueStatus);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_FlushBuffers")]
        internal static partial SiStatus SI_FlushBuffers(SafeUSBXpressDeviceHandle cyHandle, byte FlushTransmit, byte FlushReceive);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_CancelIo")]
        internal static partial SiStatus SI_CancelIo(SafeUSBXpressDeviceHandle cyHandle);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_CancelIoEx")]
        internal static partial SiStatus SI_CancelIoEx(SafeUSBXpressDeviceHandle cyHandle, IntPtr o);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_Read")]
        internal static partial SiStatus SI_Read(SafeUSBXpressDeviceHandle cyHandle, ref byte lpBuffer, uint dwBytesToRead, ref uint lpdwBytesReturned, IntPtr o = default);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_Write")]
        internal static partial SiStatus SI_Write(SafeUSBXpressDeviceHandle cyHandle, ref byte lpBuffer, uint dwBytesToWrite, ref uint lpdwBytesWritten, IntPtr o = default);

        [LibraryImport(nameof(SiUSBXp), EntryPoint = "SI_DeviceIOControl")]
        internal static partial SiStatus SI_DeviceIOControl(SafeUSBXpressDeviceHandle cyHandle, uint dwIoControlCode, ref byte lpInBuffer, uint dwBytesToRead, ref byte lpOutBuffer, uint dwBytesToWrite, ref uint lpdwBytesSucceeded);

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
