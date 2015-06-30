﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace XInputWrapper
{
    //Marshalling a C# Struct to match the XINPUT_KEY_STROKE Struct
    [StructLayout(LayoutKind.Explicit)]
    public struct XInputKeystroke
    {
        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(0)]
        public short VirtualKey;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(2)]
        public char Unicode;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(4)]
        public short Flags;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(5)]
        public byte UserIndex;

        [MarshalAs(UnmanagedType.I1)]
        [FieldOffset(6)]
        public byte HidCode;
    }
}
