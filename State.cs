using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Hunt_the_Wumpus
{
    abstract class State
    {
        public virtual void Initialize() //What to do on startup of the state
        {
            ObjectManager.Clear();
        }
        public virtual void Update() //Updates all of the objects in the state
        {
            foreach (GameObject obj in ObjectManager.Objects.Values)
                obj.Update();
        }
    }
}