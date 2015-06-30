using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PointFHelp;
using XInputWrapper;
using HighScore;

namespace Hunt_the_Wumpus
{
    class GameOver : Menu
    {
        bool Won;
        uint Time;
        public GameOver(bool Won, uint Time)
        {
            this.Won = Won;
            this.Time = Time;
            FirstUpdate = true;
        }
        public override void Initialize()
        {
            base.Initialize();
            GameObject Background;
            Game.LifeCounter.Visible = false;  // clearing background
            Game.DiskCounter.Visible = false;  // clearing background

            if(Won) // if you win: prompt the win screen and input name for highscore
            {
                Background = new GameObject("Win", "Win.png", 1366, 768);
                Game.RoomMessage.Text = "You had a time of " + (float)(Time / 1000) + " seconds\nEnter your name!";
                Game.NameEnter.Visible = true; // allow name entry

                string playerName = "";
                
                if (Player1.IsBPressed && CanPressB)  // save the name when B is clicked
                {
                    playerName = Game.NameEnter.Text;
                }

                HighScores.AddEntry(Time, playerName); // add the name and score to the high scores
            }
            else  // if loss: prompt the loss screen
            {
                Game.RoomMessage.Text = "You had a time of " + (float)(Time / 1000) + " seconds";
                Background = new GameObject("Loss", "Loss.png", 1366, 768);
            }
            ObjectManager.AddGameObject(Background);
        }
        bool FirstUpdate;
        public override void Update()
        {
            base.Update();
            if(FirstUpdate)
            {
                FirstUpdate = false;
                this.Initialize();
            }
            if(InputManager.IsKeyPressed(Keys.Escape) || (Player1.IsBPressed && CanPressB))
            {
                Game.RoomMessage.Visible = false;
                GameStateManager.CurrentState = new MainMenu();
            }
        }
    }
}
