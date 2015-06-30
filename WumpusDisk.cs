using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointFHelp;

namespace Hunt_the_Wumpus
{
    class WumpusDisk : GameObject
    {
        private static long Count = 0; //This is only used to give each arrow a unique name in the ObjectManager

        const float InitialSpeed = 20.0f;
        const float Friction = 0.009f;

        PointF Velocity; //Stores the Horizontal and Vertical components of its velocity
        Player ToHit;

        public WumpusDisk(Player ToHit, PointF InitialPosition, float Rotation)
            : base("WumpusDisk" + Count, "Disk1.png", 32, 32)
        {
            Count++; //Increase the count of the number of arrows we have made
            ZOrder = 10; //Arrows appear in front of everything else.
            this.Position = new PointF(InitialPosition.X, InitialPosition.Y);

            this.ToHit = ToHit;

            Velocity = new PointF(InitialSpeed * (float)Math.Cos((Math.PI / 180.0f) * this.Rotation),
                InitialSpeed * (float)Math.Sin((Math.PI / 180.0f) * this.Rotation)); //Send the arrow along the correct path

            Collision = CollisionTypes.Rectangular;
            CollisionBox = new PointF(16, 16);

            RemoveFrame(0);
            AddFrame("WDisk\\WDisk1.png", 4);
            AddFrame("WDisk\\WDisk2.png", 4);
            AddFrame("WDisk\\WDisk3.png", 4);
            AddFrame("WDisk\\WDisk4.png", 4);
            AddFrame("WDisk\\WDisk5.png", 4);
            AddFrame("WDisk\\WDisk6.png", 4);
            AddFrame("WDisk\\WDisk7.png", 4);
            AddFrame("WDisk\\WDisk8.png", 4);
            GoToFrame(0);
            AnimateForwards();
        }
        public override void Update()
        {
            base.Update();
            Position.X += Velocity.X; //Change position based on Pixels / Frame
            Position.Y += Velocity.Y;

            Velocity.X *= (1 - Friction); //Decrease speed based on the friction
            Velocity.Y *= (1 - Friction);

            if (Math.Sqrt(Velocity.X * Velocity.X + Velocity.Y * Velocity.Y) <= 0.009f) //If the arrow isn't moving, kill it!
                IsDead = true;
            if (!IsInViewport) //If it goes outside the screen, kill it!
                IsDead = true;

            if (this.IsCollidedWithGameObject(ToHit))
            {
                ToHit.Lives--;
                this.IsDead = true;
            }
        }
    }
}
