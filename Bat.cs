using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointFHelp;

namespace Hunt_the_Wumpus
{
    class Bat : GameObject
    {
        private const float Speed = 3.0f;
        static int Count = 0;

        static Random Rand = new Random();

        Player ToTrack;
        public Bat(Player p)
            : base("Bat" + Count, "Bat 1.png", 128, 64)
        {
            Position = new PointF(Rand.Next((int)(-Game.Dimensions.X / 2 + 128), (int)(Game.Dimensions.X / 2 - 128)),
                Rand.Next((int)(-Game.Dimensions.Y / 2 + 128), (int)(Game.Dimensions.Y / 2 - 128))); //Spawn the enemy in a random location

            Count++;

            ToTrack = p; //Assign the player to be tracked

            Collision = CollisionTypes.Rectangular; //Enable the collision
            CollisionBox = new PointF(Width, Height);

            RemoveFrame(0);
            AddFrame("Bat 1.png", 6);
            AddFrame("Bat 2.png", 6);
            GoToFrame(0);
            AnimateForwards();
        }
        public override void Update()
        {
            base.Update();
            float DeltaX = (ToTrack.Position.X - Position.X); //Get the difference in X
            float DeltaY = (ToTrack.Position.Y - Position.Y); //Get the difference in Y
            float Distance = (float)Math.Sqrt(DeltaX * DeltaX
                + DeltaY * DeltaY); //Get the distance between the two

            Rotation = (float)(Math.Atan2(DeltaY, DeltaX) * 180 / Math.PI) - 90; //Set the rotation to the angle between the player and the enemy

            Position.X += (DeltaX / Distance) * Speed; //Get a vector, normalize it, and then get its X Component. Multiply this by the desired speed
            Position.Y += (DeltaY / Distance) * Speed; //Get a vector, normalize it, and then get its Y Component. Multiply this by the desired speed
            if (this.IsCollidedWithGameObject(ToTrack))
            {
                Random r = new Random();
                Cave.CurrentRoom = Cave.Rooms[r.Next(0, 30)];
                ToTrack.Position = new PointFHelp.PointF(0, 0);
                this.IsDead = true;
            }
        }
        public override void OnDeath()
        {
            base.OnDeath();
            Cave.CurrentRoom.NeedToSpawnBat = false;
        }
    }
}
