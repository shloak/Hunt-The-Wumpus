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
	static class GameStateManager
	{
		private static State CurrSt; //The reference to the current state
		public static State CurrentState //Gets or sets the value of the current state. If it is set, we initialize the state
		{
			get
			{
				return CurrSt;
			}
			set
			{
				value.Initialize();
				CurrSt = value;
			}
		}
        public static void SetCurrentStateNoInitialize(State St)
        {
            CurrSt = St;
        }
	}
}