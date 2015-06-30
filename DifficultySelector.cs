using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PointFHelp;
using XInputWrapper;
namespace Hunt_the_Wumpus
{
    class DifficultySelector : Menu
    {
        public override void Initialize()
        {
            base.Initialize();

            GameObject Background = new GameObject("Background", "TitleScreen.png", 1366, 768);
            Background.ZOrder = -10;
            ObjectManager.AddGameObject(Background);

            Buttons = new Button[3];

            Buttons[0] = new Button("EasyButton", "EasyText1.png", "EasyText2.png", 132 , 33);
            Buttons[0].Position = new PointF(0, 0);

            Buttons[1] = new Button("MediumButton", "MediumText1.png", "MediumText2.png", 132, 33);
            Buttons[1].Position = new PointF(0, -60);

            Buttons[2] = new Button("HardButton", "HardText1.png", "HardText2.png", 130, 33);
            Buttons[2].Position = new PointF(0, -120);

            foreach (Button b in Buttons)
                ObjectManager.AddGameObject(b);
        }

        public override void Update()
        {
            base.Update();

            if (InputManager.IsKeyTriggered(Keys.Enter) || (Player1.IsAPressed && CanPressA))
            {
                Cave.Difficulty = CurrentSelected + 1;
                GameStateManager.CurrentState = new TheLevel();
                Game.watch.Start();
            }

            if (InputManager.IsKeyTriggered(Keys.Escape) || (Player1.IsBPressed && CanPressB))
            {
                GameStateManager.CurrentState = new MainMenu();
            }
        }
    }
}
