using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace XInputWrapper
{
    //Marshalling a C# Struct to match the XINPUT_BATTERY_INFORMATION Struct
    [StructLayout(LayoutKind.Explicit)]
    public struct XInputBatteryInformation
    {
        [MarshalAs(UnmanagedType.I1)]
        [FieldOffset(0)]
        public byte BatteryType;

        [MarshalAs(UnmanagedType.I1)]
        [FieldOffset(1)]
        public byte BatteryLevel;
    }
}
