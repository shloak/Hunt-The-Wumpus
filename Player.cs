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
    class Player : GameObject
    {
        //References to the walls that it can collide with
        public Wall[] NWalls;
        public Wall[] SEWalls;
        public Wall[] NEWalls;
        public Wall[] SWalls;
        public Wall[] SWWalls;
        public Wall[] NWWalls;
        public Wall[] EdgeWalls;

        XboxController Pad; //The XBoxController to receive input from

        public int Lives = 500; //The number of lives

        public Player()
            : base("Player", "Player\\Standing.png", 64, 128)
        {
            ScreenWrapping = true;
            Collision = CollisionTypes.Rectangular;
            CollisionBox = new PointFHelp.PointF(128, 128);
            Pad = XboxController.RetrieveController(0);

            RemoveFrame(0);
            AddFrame("Player\\Run1.png", 4);
            AddFrame("Player\\Run2.png", 4);
            AddFrame("Player\\Standing.png", 4);
            AnimateForwards();
        }
        private static void RemoveOtherObjects() //Removes all of the players' arrows
        {
            foreach (GameObject obj in ObjectManager.Objects.Values)
            {
                if (obj is Disk)
                    ObjectManager.RemoveGameObject(obj);
                if (obj is Enemy)
                    ObjectManager.RemoveGameObject(obj);
                if (obj is Bat)
                    ObjectManager.RemoveGameObject(obj);
            }
                
        }
        private void SpawnEnemies()
        {
            for (int i = 0; i < Cave.CurrentRoom.NumberOfEnemies; i++)
            {
                ObjectManager.AddGameObject(new Enemy(this));
            }
        }
        public override void ScreenWrap() //Screenwrap the player. This changes the room in the cave
        {
            if (ScreenWrapping)
            {
                if (Position.X > 256 && Position.Y > Game.Dimensions.Y / 2)
                {
                    Position.Y = -Game.Dimensions.Y / 2;
                    Position.X *= -1;
                    Cave.MoveRoom(Directions.NorthEast);
                    RemoveOtherObjects();
                    SpawnEnemies();
                }
                else if (Position.X < -256 && Position.Y > Game.Dimensions.Y / 2)
                {
                    Position.Y = -Game.Dimensions.Y / 2;
                    Position.X *= -1;
                    Cave.MoveRoom(Directions.NorthWest);
                    RemoveOtherObjects();
                    SpawnEnemies();
                }
                else if (Position.X > 256 && Position.Y < -Game.Dimensions.Y / 2)
                {
                    Position.Y = Game.Dimensions.Y / 2;
                    Position.X *= -1;
                    Cave.MoveRoom(Directions.SouthEast);
                    RemoveOtherObjects();
                    SpawnEnemies();
                }
                else if (Position.X < -256 && Position.Y < -Game.Dimensions.Y / 2)
                {
                    Position.Y = Game.Dimensions.Y / 2;
                    Position.X *= -1;
                    Cave.MoveRoom(Directions.SouthWest);
                    RemoveOtherObjects();
                    SpawnEnemies();
                }
                else if (Position.Y > Game.Dimensions.Y / 2)
                {
                    Position.Y = -Game.Dimensions.Y / 2;
                    Cave.MoveRoom(Directions.North);
                    RemoveOtherObjects();
                    SpawnEnemies();
                }
                else if (Position.Y < -Game.Dimensions.Y / 2)
                {
                    Position.Y = Game.Dimensions.Y / 2;
                    Cave.MoveRoom(Directions.South);
                    RemoveOtherObjects();
                    SpawnEnemies();
                }
                else if (Position.X < -Game.Dimensions.X / 2 && Position.Y > 0)
                {
                    Position.X = Game.Dimensions.X / 2;
                    Position.Y *= -1;
                    Cave.MoveRoom(Directions.NorthWest);
                    RemoveOtherObjects();
                    SpawnEnemies();
                }
                else if (Position.X < -Game.Dimensions.X / 2 && Position.Y < 0)
                {
                    Position.X = Game.Dimensions.X / 2;
                    Position.Y *= -1;
                    Cave.MoveRoom(Directions.SouthWest);
                    RemoveOtherObjects();
                    SpawnEnemies();
                }
                else if (Position.X > Game.Dimensions.X / 2 && Position.Y > 0)
                {
                    Position.X = -Game.Dimensions.X / 2;
                    Position.Y *= -1;
                    Cave.MoveRoom(Directions.NorthEast);
                    RemoveOtherObjects();
                    SpawnEnemies();
                }
                else if (Position.X > Game.Dimensions.X / 2 && Position.Y < 0)
                {
                    Position.X = -Game.Dimensions.X / 2;
                    Position.Y *= -1;
                    Cave.MoveRoom(Directions.SouthEast);
                    RemoveOtherObjects();
                    SpawnEnemies();
                }

            }
            Game.DebugLabel.Text = "Current Room: Room " + Cave.CurrentRoom.Number + "\nLives: " + Lives + "\nWumpus Room: " + Cave.WumpusRoom +
               "\nPit Room: " + Cave.PitRoom + "\nBat Rooms: " + Cave.BatRoom1 + ", " + Cave.BatRoom2;
        }
        public override void Update()
        {
            base.Update();

            //Check for collision with the walls
            bool CanMoveUp = true;
            bool CanMoveRight = true;
            bool CanMoveDown = true;
            bool CanMoveLeft = true;
            foreach (Wall w in NWalls)
            {
                if (w.IsCollidedWithGameObject(this))
                {
                    if (this.Position.Y + this.CollisionBox.Y / 2 >= w.Position.Y - w.CollisionBox.Y / 2)
                        CanMoveUp = false;
                }
            }
            foreach (Wall w in NEWalls)
            {
                if (w.IsCollidedWithGameObject(this))
                {
                    if (this.Position.Y + this.CollisionBox.Y / 2 >= w.Position.Y - w.CollisionBox.Y / 2)
                        CanMoveUp = false;
                    if (this.Position.X + this.CollisionBox.X / 2 >= w.Position.X - w.CollisionBox.X / 2)
                        CanMoveRight = false;
                }
            }
            foreach (Wall w in SEWalls)
            {
                if (w.IsCollidedWithGameObject(this))
                {
                    if (this.Position.Y - this.CollisionBox.Y / 2 <= w.Position.Y + w.CollisionBox.Y / 2)
                        CanMoveDown = false;
                    if (this.Position.X + this.CollisionBox.X / 2 >= w.Position.X - w.CollisionBox.X / 2)
                        CanMoveRight = false;
                }
            }
            foreach (Wall w in SWalls)
            {
                if (w.IsCollidedWithGameObject(this))
                {
                    if (this.Position.Y - this.CollisionBox.Y / 2 <= w.Position.Y + w.CollisionBox.Y / 2)
                        CanMoveDown = false;
                }
            }
            foreach (Wall w in SWWalls)
            {
                if (w.IsCollidedWithGameObject(this))
                {

                    if (this.Position.Y - this.CollisionBox.Y / 2 <= w.Position.Y + w.CollisionBox.Y / 2)
                        CanMoveDown = false;
                    if (this.Position.X - this.CollisionBox.X / 2 <= w.Position.X + w.CollisionBox.X / 2)
                        CanMoveLeft = false;
                }
            }
            foreach (Wall w in NWWalls)
            {
                if (w.IsCollidedWithGameObject(this))
                {

                    if (this.Position.Y + this.CollisionBox.Y / 2 >= w.Position.Y - w.CollisionBox.Y / 2)
                        CanMoveUp = false;
                    if (this.Position.X - this.CollisionBox.X / 2 <= w.Position.X + w.CollisionBox.X / 2)
                        CanMoveLeft = false;
                }
            }
            if (EdgeWalls[0].IsCollidedWithGameObject(this))
            {
                if (this.Position.X + this.CollisionBox.X / 2 >= EdgeWalls[0].Position.X - EdgeWalls[0].CollisionBox.X / 2)
                    CanMoveRight = false;
                if (this.Position.Y - this.CollisionBox.Y / 2 <= EdgeWalls[0].Position.Y + EdgeWalls[0].CollisionBox.Y / 2)
                    CanMoveDown = false;
            }
            if (EdgeWalls[1].IsCollidedWithGameObject(this))
            {
                if (this.Position.X + this.CollisionBox.X / 2 >= EdgeWalls[1].Position.X - EdgeWalls[1].CollisionBox.X / 2)
                    CanMoveRight = false;
                if (this.Position.Y + this.CollisionBox.Y / 2 >= EdgeWalls[1].Position.Y - EdgeWalls[1].CollisionBox.Y / 2)
                    CanMoveUp = false;
            }
            if (EdgeWalls[2].IsCollidedWithGameObject(this))
            {
                if (this.Position.X - this.CollisionBox.X / 2 <= EdgeWalls[2].Position.X + EdgeWalls[2].CollisionBox.X / 2)
                    CanMoveLeft = false;
                if (this.Position.Y - this.CollisionBox.Y / 2 <= EdgeWalls[2].Position.Y + EdgeWalls[2].CollisionBox.Y / 2)
                    CanMoveDown = false;
            }
            if (EdgeWalls[3].IsCollidedWithGameObject(this))
            {
                if (this.Position.X - this.CollisionBox.X / 2 <= EdgeWalls[3].Position.X + EdgeWalls[3].CollisionBox.X / 2)
                    CanMoveLeft = false;
                if (this.Position.Y + this.CollisionBox.Y / 2 >= EdgeWalls[3].Position.Y - EdgeWalls[3].CollisionBox.Y / 2)
                    CanMoveUp = false;
            }

            Game.LifeCounter.Text = "  " + Lives;

            PointF XBoxMovement = new PointF((Pad.LeftThumbStick.X / 8192) * 4, (Pad.LeftThumbStick.Y / 8192) * 4); //Get the vector to the point that we want to move to


            bool Moving = false;

            if (((XBoxMovement.X > 0 && CanMoveRight) && (XBoxMovement.Y > 0 && CanMoveUp)) ||
                ((XBoxMovement.X > 0 && CanMoveRight) && (XBoxMovement.Y < 0 && CanMoveDown)) ||
                ((XBoxMovement.X < 0 && CanMoveLeft) && (XBoxMovement.Y > 0 && CanMoveUp)) ||
                ((XBoxMovement.X < 0 && CanMoveLeft) && (XBoxMovement.Y < 0 && CanMoveDown) ||
                ((XBoxMovement.X == 0) && (XBoxMovement.Y > 0 && CanMoveUp)) ||
                ((XBoxMovement.X == 0) && (XBoxMovement.Y < 0 && CanMoveDown)) ||
                ((XBoxMovement.X > 0 && CanMoveRight) && (XBoxMovement.Y == 0)) ||
                ((XBoxMovement.X < 0 && CanMoveLeft) && (XBoxMovement.Y == 0))))
            {
                Position += XBoxMovement; //If we can move in the direction that we need, then we should move!
                Moving = true;
            }

            if (InputManager.IsKeyPressed(Keys.W) && CanMoveUp)
            {
                this.Position.Y += 5;
                Moving = true;
            }
            if (InputManager.IsKeyPressed(Keys.S) && CanMoveDown)
            {
                this.Position.Y -= 5;
                Moving = true;
            }
            if (InputManager.IsKeyPressed(Keys.D) && CanMoveRight)
            {
                this.Position.X += 5;
                Moving = true;
            }
            if (InputManager.IsKeyPressed(Keys.A) && CanMoveLeft)
            {
                this.Position.X -= 5;
                Moving = true;
            }

            if (Moving == false)
                GoToFrame(2);
            if (Lives <= 0)
            {
                Game.watch.Stop();
                uint Time = (uint)(Game.watch.ElapsedMilliseconds * (-0.25 * Cave.Difficulty + 1.5));
                GameStateManager.SetCurrentStateNoInitialize(new GameOver(false, Time));
            }
        }
    }
}
