using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointFHelp;

namespace Hunt_the_Wumpus
{
    class Enemy : GameObject
    {
        private static long Counter = 0; //This is used to give each enemy a unique name in the ObjectManager
        protected const float Speed = 4.0f; //The default speed of the enemy
        protected Player ToTrack; //A reference to the player to chase
        uint InvincibilityTimer = 60; //This is used to ensure that there is a 1 second time period in which this enemy cannot attack again

        static Random Rand = new Random();

        public Enemy(Player p)
            : base("Enemy" + Counter, "Enemy.png", 64, 64)
        {
            Position = new PointF(Rand.Next((int)(-Game.Dimensions.X / 2 + 128), (int)(Game.Dimensions.X / 2 - 128)),
                Rand.Next((int)(-Game.Dimensions.Y / 2 + 128), (int)(Game.Dimensions.Y / 2 - 128))); //Spawn the enemy in a random location

            ToTrack = p; //Assign the player to be tracked

            Collision = CollisionTypes.Rectangular; //Enable the collision
            CollisionBox = new PointF(Width, Height);

            Counter++;

            SetFrameDuration(0, 60); //Set the default picture to be 60 frames long
            AddFrame("Enemy2.png", 60); //Add another picture and set that to be 60 frames long
            GoToFrame(0); //Go to the first frame
            AnimateForwards(); //Begin animation
        }
        public override void OnDeath()
        {
            //ObjectManager.AddGameObject(new Enemy(ToTrack));
        }
        public override void Update()
        {
            base.Update();

            float DeltaX = (ToTrack.Position.X - Position.X); //Get the difference in X
            float DeltaY = (ToTrack.Position.Y - Position.Y); //Get the difference in Y
            float Distance = (float)Math.Sqrt(DeltaX * DeltaX
                + DeltaY * DeltaY); //Get the distance between the two

            Position.X += (DeltaX / Distance) * Speed; //Get a vector, normalize it, and then get its X Component. Multiply this by the desired speed
            Position.Y += (DeltaY / Distance) * Speed; //Get a vector, normalize it, and then get its Y Component. Multiply this by the desired speed

            if (InvincibilityTimer != 0) //Decrement the timer if the enemy cannot attack yet
                InvincibilityTimer--;

            if (this.IsCollidedWithGameObject(ToTrack) && InvincibilityTimer == 0) //If it hits the player, the decrement the number of lives the player has
            {
                ToTrack.Lives--;
                InvincibilityTimer = 60;
            }
        }
    }
}
