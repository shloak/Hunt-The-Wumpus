using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunt_the_Wumpus.Helpers
{
    class Map
    { 
		private int wumpusRoom;
		private int bat1Room;
		private int bat2Room;
		private int pit1Room;
		private int playerRoom;
		private Cave currentCave;  

		// initialize the cave later
        public Map(){ 
			wumpusRoom = 0;
			bat1Room = 0;
			bat2Room = 0;
			pit1Room = 0;
			playerRoom = 0;
        } 

        public int getWumpusRoom()
        {
            return wumpusRoom;
        }

		public int getBat1Room()
		{
			return bat1Room;
		}
		
		public int getBat2Room()
		{
			return bat2Room;
		}
		
		public int getPit1Room()
		{
			return pit1Room;
		}
		
		public int getPlayerRoom()
		{
			return playerRoom;
		}
		
		public bool wumpusHit(int a)
		{
			if(a == wumpusRoom){
				return true;
			}
			return false
		} 
		
		public void setMap(int WR, int B1, int B2, int P1, int PR)
		{
			wumpusRoom = WR;
			bat1Room = B1;
			bat2Room = B2;
			pit1Room = P1;
			playerRoom = PR;
		}		
    }
}



