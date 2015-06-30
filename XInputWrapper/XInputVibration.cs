using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace XInputWrapper
{
    //Marshalling a C# Struct to match the XINPUT_VIBRATION Struct
    [StructLayout(LayoutKind.Sequential)]
    public struct XInputVibration
    {
        [MarshalAs(UnmanagedType.I2)]
        public ushort LeftMotorSpeed;

        [MarshalAs(UnmanagedType.I2)]
        public ushort RightMotorSpeed;
    }
}
