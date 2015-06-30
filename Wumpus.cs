using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunt_the_Wumpus
{
    class Wumpus : GameObject
    {
        protected const float Speed = 3.0f;  // TODO: test speed - might be too fast(?)
        protected Player ToTrack;
        uint InvincibilityTimer = 60;
        Sound Fatality = new Sound("DeathMusic.mp3");
        public int Lives = Cave.Difficulty + 25;

        public Wumpus(Player p)
            : base("Wumpus", "Wumpus.png", 128, 128)
        {
            Position = new PointFHelp.PointF(0, 0);
            ToTrack = p;

            Collision = CollisionTypes.Rectangular; //Enable the collision
            CollisionBox = new PointFHelp.PointF(Width, Height);
            ShootTimer = SecondsPerShoot * 60;
        }

        const float SecondsPerShoot = 0.5f;
        float ShootTimer;
        public override void Update()
        {
            base.Update();

            float DeltaX = (ToTrack.Position.X - Position.X); //Get the difference in X
            float DeltaY = (ToTrack.Position.Y - Position.Y); //Get the difference in Y

            ShootTimer--;
            if (ShootTimer == 0)
            {
                ShootTimer = SecondsPerShoot * 60;
                ObjectManager.AddGameObject(new WumpusDisk(ToTrack, this.Position, (float)Math.Atan2(DeltaY, DeltaX)));
            }


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
            if (Lives <= 0)
            {
                IsDead = true;
            }
        }
        public override void OnDeath()
        {
            base.OnDeath();
            Fatality.Play();
            Game.watch.Stop();
            uint Time = (uint)(Game.watch.ElapsedMilliseconds * (-0.25 * Cave.Difficulty + 1.5));
            GameStateManager.SetCurrentStateNoInitialize(new GameOver(true, Time));
        }
    }
}
