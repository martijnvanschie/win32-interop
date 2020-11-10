using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32.Interop.Advapi32
{
    public static partial class SecurityBaseApi
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool AllocateLocallyUniqueId([Out] out UInt64 Luid);
    }
}
