using System.Runtime.InteropServices;

namespace SiUSBXpDotNet
{
    internal class SafeUSBXpressDeviceHandle : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeUSBXpressDeviceHandle() : base(invalidHandleValue: IntPtr.Zero, ownsHandle: true)
        {
        }

        protected override bool ReleaseHandle()
        {
            return SiUSBXp.SI_Close(handle) == SiStatus.Success;
        }
    }
}
