using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointFHelp;

namespace Hunt_the_Wumpus
{
    class Disk : GameObject
    {
        private static long Count = 0; //This is only used to give each arrow a unique name in the ObjectManager

        const float InitialSpeed = 20.0f;
        const float Friction = 0.009f;

        PointF Velocity; //Stores the Horizontal and Vertical components of its velocity

        public Disk(PointF InitialPosition, float Rotation)
            : base("Disk" + Count, "Disk1.png", 32, 32)
        {
            Count++; //Increase the count of the number of arrows we have made
            ZOrder = 10; //Arrows appear in front of everything else.
            this.Position = new PointF(InitialPosition.X, InitialPosition.Y);
            this.Rotation = Rotation + 90; //Since the arm is pointed up in the file, we have to rotate it 90 degrees

            Velocity = new PointF(InitialSpeed * (float)Math.Cos((Math.PI / 180.0f) * this.Rotation),
                InitialSpeed * (float)Math.Sin((Math.PI / 180.0f) * this.Rotation)); //Send the arrow along the correct path

            Collision = CollisionTypes.Rectangular;
            CollisionBox = new PointF(16, 16);

            RemoveFrame(0);
            AddFrame("Disk\\Disk1.png", 4);
            AddFrame("Disk\\Disk2.png", 4);
            AddFrame("Disk\\Disk3.png", 4);
            AddFrame("Disk\\Disk4.png", 4);
            AddFrame("Disk\\Disk5.png", 4);
            AddFrame("Disk\\Disk6.png", 4);
            AddFrame("Disk\\Disk7.png", 4);
            AddFrame("Disk\\Disk8.png", 4);
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

            var Enemies = from e in ObjectManager.Objects.Values where e is Enemy select e; //Get all the enemies and bats
            var Bats = from b in ObjectManager.Objects.Values where b is Bat select b;
            var Wumpi = from w in ObjectManager.Objects.Values where w is Wumpus select w;
            foreach (Enemy e in Enemies) //If it hits an enemy, kill the enemy!!
                if (this.IsCollidedWithGameObject(e))
                {
                    e.IsDead = true;
                    Cave.CurrentRoom.NumberOfEnemies--;
                }
            foreach (Bat b in Bats)
                if (this.IsCollidedWithGameObject(b))
                    b.IsDead = true;
            foreach (Wumpus w in Wumpi)
            {
                if (this.IsCollidedWithGameObject(w))
                {
                    w.Lives--;
                    this.IsDead = true;
                }
            }
        }
    }
}
