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
    class MainMenu : Menu
    {
        public override void Initialize()
        {
            base.Initialize();
            Buttons = new Button[4];

            GameObject Background = new GameObject("Background", "TitleScreen.png", 1366, 768);
            Background.ZOrder = -10;
            ObjectManager.AddGameObject(Background);

            Buttons[0] = new Button("PlayButton", "PlayText1.png", "PlayText2.png", 154, 39);
            Buttons[0].Position = new PointF(0, 0);

            Buttons[1] = new Button("TutorialButton", "TutorialText1.png", "TutorialText2.png", 154, 39);
            Buttons[1].Position = new PointF(0, -60);

            Buttons[2] = new Button("CreditsButton", "CreditsText1.png", "CreditsText2.png", 157, 39);
            Buttons[2].Position = new PointF(0, -120);

            Buttons[3] = new Button("QuitButton", "QuitText1.png", "QuitText2.png", 156, 39);
            Buttons[3].Position = new PointF(0, -180);

            GameObject Credits = new GameObject("Credits", "Credits.jpg", 1366, 768);
            ObjectManager.AddGameObject(Credits);
            Credits.ZOrder = 10;
            Credits.IsVisible = false;

            GameObject Tutorial = new GameObject("Tutorial", "Tutorial.png", 1366, 768);
            ObjectManager.AddGameObject(Tutorial);
            Tutorial.ZOrder = 10;
            Tutorial.IsVisible = false;

            Game.NameEnter.Visible = false;

            foreach (Button obj in Buttons)
                ObjectManager.AddGameObject(obj);

            InCredits = false;
            InTutorial = false;
        }
        private bool InCredits;
        private bool InTutorial;
        public override void Update()
        {
            base.Update();
            if (InputManager.IsKeyTriggered(Keys.Enter) || (Player1.IsAPressed && CanPressA))
            {
                switch (CurrentSelected)
                {
                    case 0:
                        GameStateManager.CurrentState = new DifficultySelector();
                        break;
                    case 1:
                        ObjectManager.GetObjectByName("Tutorial").IsVisible = true;
                        InTutorial = true;
                        break;
                    case 2:
                        ObjectManager.GetObjectByName("Credits").IsVisible = true;
                        InCredits = true;
                        break;
                    case 3:
                        Game.Quit = true;
                        break;
                    default:
                        break;
                }
            }
            if (InputManager.IsKeyTriggered(Keys.Escape) || (Player1.IsBPressed && CanPressB))
            {
                if (InCredits)
                {
                    ObjectManager.GetObjectByName("Credits").IsVisible = false;
                    InCredits = false;
                }
                else if (InTutorial)
                {
                    ObjectManager.GetObjectByName("Tutorial").IsVisible = false;
                    InCredits = false;
                }
                else
                    Game.Quit = true;
            }

        }
    }
}
