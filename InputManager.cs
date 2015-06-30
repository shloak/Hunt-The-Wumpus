using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Hunt_the_Wumpus
{
    static class InputManager
    {
        const int NumberOfKeys = 194; //That is the number of keys in the Keys enum
        public static bool[] KeysCurrentFrame = new bool[NumberOfKeys]; //All of the keys that are pressed this frame
        public static bool[] KeysLastFrame = new bool[NumberOfKeys]; //All of the keys that were pressed last frame
        public static bool[] KeyBuffer = new bool[NumberOfKeys]; //The asynchronous key buffer that is written into whenever the event fires
        public static void OnKeyDown(object Sender, KeyEventArgs e)
        {
            if (!(e.KeyValue > NumberOfKeys))
                KeyBuffer[e.KeyValue] = true;
        }
        public static void OnKeyReleased(object Sender, KeyEventArgs e)
        {
            if (!(e.KeyValue > NumberOfKeys))
                KeyBuffer[e.KeyValue] = false;
        }
        public static bool IsKeyPressed(Keys Key)
        {
            return KeysCurrentFrame[(int)Key];
        }
        public static bool IsKeyTriggered(Keys Key)
        {
            return KeysCurrentFrame[(int)Key] & !KeysLastFrame[(int)Key];
        }
        public static bool IsKeyReleased(Keys Key)
        {
            return !KeysCurrentFrame[(int)Key] & KeysLastFrame[(int)Key];
        }
    }
}
