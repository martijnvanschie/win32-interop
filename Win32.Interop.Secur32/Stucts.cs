using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32.Interop.Secur32
{
    public static partial class NtSecApi
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct LSA_STRING
        {
            public UInt16 Length;
            public UInt16 MaximumLength;
            public /*PCHAR*/ IntPtr Buffer;

            public LSA_STRING(string value)
            {
                Length = (ushort)value.Length;
                MaximumLength = (ushort)value.Length;
                Buffer = Marshal.StringToHGlobalAnsi(value);
            }
        }
    }
}
