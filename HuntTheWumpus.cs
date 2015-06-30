using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Hunt_the_Wumpus
{
    static class HuntTheWumpus
    {
        [STAThread]
        public static void Main()
        {
            Game Wumpus = new Game(1366, 768, 60, "Hunt the Wumpus"); //Create a new Game
            GameStateManager.CurrentState = new MainMenu(); //Starting the actual game up :D
            while(!Game.Quit) //The game loop!
            {
                Wumpus.Update(); //Update the game
            }
            Wumpus.Dispose(); //Denitialize everything when the program ends
        }
    }
}