using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XInputWrapper;

namespace Hunt_the_Wumpus
{
    class Arm : GameObject
    {
        Player P; //Gets a reference to the player to follow
        XboxController Controller; //The controller to receive input from

        public int Ammo = 2000; //The number of disks to throw

        Sound GunMusic = new Sound("GunMusic.mp3");

        bool ReadyToFire;
        public Arm(Player p, int ControllerIndex)
            : base("Arm", "Arm1.png", 64, 128)
        {
            P = p;
            Controller = XboxController.RetrieveController(ControllerIndex);
            ReadyToFire = true;

            RemoveFrame(0);
            AddFrame("Arm\\Arm1.png", 4);
            AddFrame("Arm\\Arm2.png", 4);
            AddFrame("Arm\\Arm3.png", 4);
            AddFrame("Arm\\Arm4.png", 4);
            AddFrame("Arm\\Arm5.png", 4);
            AddFrame("Arm\\Arm6.png", 4);
            AddFrame("Arm\\Arm7.png", 4);
            AddFrame("Arm\\Arm8.png", 4);
            GoToFrame(0);
            AnimateForwards();
        }
        public override void Update()
        {
            base.Update();

            this.Position.X = P.Position.X; //Follow the player
            this.Position.Y = P.Position.Y;

            Rotation =
                Controller.RightThumbStick.X < XInputConstants.XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE && //If the stick is in the deadzone, keep Rotation the same
                Controller.RightThumbStick.X > -XInputConstants.XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE &&
                Controller.RightThumbStick.Y < XInputConstants.XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE &&
                Controller.RightThumbStick.Y > -XInputConstants.XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE ?
                Rotation
                : ((float)(Math.Atan2(Controller.RightThumbStick.Y, Controller.RightThumbStick.X) * 180 / Math.PI) - 90); //Otherwise update it

            if (InputManager.IsKeyPressed(Keys.Right))
                Rotation -= 5;
            if (InputManager.IsKeyPressed(Keys.Left))
                Rotation += 5;

            if (!(Controller.RightTrigger > 0)) //If we let go of the trigger, we can shoot!
                ReadyToFire = true;

            if(Ammo >= 10)
                Game.DiskCounter.Text = "" + Ammo;
            else
                Game.DiskCounter.Text = "  " + Ammo;
            if (Ammo >  0 && (InputManager.IsKeyTriggered(Keys.Space) || (Controller.RightTrigger > 0 && ReadyToFire)))
            {
                ObjectManager.AddGameObject(new Disk(Position, Rotation));
                GunMusic.Play();
                ReadyToFire = false;
                Ammo--;
            }
        }
    }
}
