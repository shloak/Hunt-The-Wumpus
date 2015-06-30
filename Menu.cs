using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XInputWrapper;

namespace Hunt_the_Wumpus
{
    class Menu : State
    {
        protected XboxController Player1;
        protected Button[] Buttons;
        protected int CurrentSelected = 0;

        protected bool CanPressA;
        protected bool CanPressB;

        private int MoveTimer;
        private const int FramesForButtonMovement = 12;

        public override void Initialize()
        {
            base.Initialize();
            MoveTimer--;

            Player1 = XboxController.RetrieveController(0);

            MoveTimer = FramesForButtonMovement;

            CanPressA = false;
            CanPressB = false;
        }

        public override void Update()
        {
            base.Update();

            MoveTimer--;

            if (Player1 != null)
            {
                if (!Player1.IsAPressed)
                    CanPressA = true;

                if (!Player1.IsBPressed)
                    CanPressB = true;
            }
            if (Buttons != null)
            {
                foreach (Button b in Buttons)
                    b.Selected = false;

                if (MoveTimer <= 0 && (InputManager.IsKeyPressed(Keys.Up) || Player1.LeftThumbStick.Y > XInputConstants.XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE))
                {
                    if (CurrentSelected > 0)
                    {
                        CurrentSelected--;
                        MoveTimer = FramesForButtonMovement;
                    }
                }
                if (MoveTimer <= 0 && (InputManager.IsKeyPressed(Keys.Down) || Player1.LeftThumbStick.Y < -XInputConstants.XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE))
                {
                    if (CurrentSelected < Buttons.Length - 1)
                    {
                        CurrentSelected++;
                        MoveTimer = FramesForButtonMovement;
                    }
                }
                Buttons[CurrentSelected].Selected = true;
            }
        }
    }
}
