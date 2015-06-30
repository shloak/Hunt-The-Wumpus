#define WINDOWS7
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace XInputWrapper
{
    public static class XInput
    {

        //NOTE: dwUserIndex always refers to the index of the player
        //NOTE: Anything that is passed with the "ref" keyword is always the output (it is the equivalent of passing a pointer in)
#if WINDOWS7
        //We will be using xinput 9.1.0 because that is currently the xinput that is supported the most
        [DllImport("xinput9_1_0.dll")]
        public static extern int XInputGetState
        (
            int dwUserIndex,  
            ref XInputState pState
        );

        [DllImport("xinput9_1_0.dll")]
        public static extern int XInputSetState
        (
            int dwUserIndex, 
            ref XInputVibration pVibration 
        );

        [DllImport("xinput9_1_0.dll")]
        public static extern int XInputGetCapabilities
        (
            int dwUserIndex, 
            int dwFlags,
            ref XInputCapabilities pCapabilities
        );


        //Since the following two functions are only available on Windows 8, we just return 0

        public static int XInputGetBatteryInformation(int dwUserIndex, byte devType, ref XInputBatteryInformation pBatteryInformation)
        {
           return 0;
        }
        public static int XInputGetKeystroke(int dwUserIndex, int dwReserved, ref XInputKeystroke pKeystroke)
        {
            return 0;
        }
#else


        [DllImport("xinput1_4.dll")]
        public static extern int XInputGetState
        (
            int dwUserIndex,  // [in] Index of the gamer associated with the device
            ref XInputState pState        // [out] Receives the current state
        );

        [DllImport("xinput1_4.dll")]
        public static extern int XInputSetState
        (
            int dwUserIndex,  // [in] Index of the gamer associated with the device
            ref XInputVibration pVibration    // [in, out] The vibration information to send to the controller
        );

        [DllImport("xinput1_4.dll")]
        public static extern int XInputGetCapabilities
        (
            int dwUserIndex,   // [in] Index of the gamer associated with the device
            int dwFlags,       // [in] Input flags that identify the device type
            ref XInputCapabilities pCapabilities  // [out] Receives the capabilities
        );


        [DllImport("xinput1_4.dll")]
        public static extern int XInputGetBatteryInformation
        (
            int dwUserIndex,        // Index of the gamer associated with the device
            byte devType,            // Which device on this user index
            ref XInputBatteryInformation pBatteryInformation // Contains the level and types of batteries
        );

        [DllImport("xinput1_4.dll")]
        public static extern int XInputGetKeystroke
        (
            int dwUserIndex,              // Index of the gamer associated with the device
            int dwReserved,               // Reserved for future use
            ref XInputKeystroke pKeystroke    // Pointer to an XINPUT_KEYSTROKE structure that receives an input event.
        );
#endif
    }


}
