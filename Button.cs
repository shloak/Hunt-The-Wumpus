using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunt_the_Wumpus
{
    class Button : GameObject
    {
        private bool _Selected;
        public bool Selected
        {
            get
            { 
                return _Selected;
            }
            set
            {
                _Selected = value;
                if (_Selected)
                    GoToFrame(1);
                else
                    GoToFrame(0);
            }
        }
        public Button(string Name, string Unselected, string Selected, uint Width, uint Height)
            : base(Name, "Buttons\\" + Unselected, Width, Height)
        {
            AddFrame("Buttons\\" + Selected, 0);
        }
    }
}
