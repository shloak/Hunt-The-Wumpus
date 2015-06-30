using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Hunt_the_Wumpus
{
    public static class FrameRateController
    {
        public static uint FrameRate; //The framerate
        public static volatile bool ProceedToNextFrame; //Tells Game whether or not it can proceed to the next frame
        public static Thread FrameRateControlThread; //The timer for each frame runs on a separate thread
        private static Stopwatch watch; //The stopwatch to be used
        public static void Initialize(uint Framerate)
        {
            FrameRate = Framerate;
            watch = new Stopwatch();
            watch.Start(); //Start the watch
            FrameRateControlThread = new Thread(new ThreadStart(() => //Run the stopwatch until the Framerate Controller gets deinitialized.
            {
            while (true)
            {
                if (watch.ElapsedMilliseconds >= 1000 / FrameRate)
                {
                    watch.Restart();
                    ProceedToNextFrame = true;
                }
            }
            }));
            ProceedToNextFrame = true;
            FrameRateControlThread.Start();
        }
        public static void Deinitialize()
        {
            FrameRateControlThread.Abort();
        }
    }
}
