using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunt_the_Wumpus
{
    class Wall : GameObject
    {
        public bool Collidable //If it is set to true, it is visible and you can run into it. If not, then it is invisible and you can't run into it
        {
            set
            {
                if (value)
                {
                    IsVisible = true;
                    Collision = CollisionTypes.Rectangular;
                }
                else
                {
                    IsVisible = false;
                    Collision = CollisionTypes.None;
                }
            }
        }
        public Wall(string Name)
            : base(Name, "Wall.png", 128, 128)
        {
            Collidable = false;
            CollisionBox = new PointFHelp.PointF(128, 128);

            RemoveFrame(0);
            AddFrame("Wall.png", 15);
            AddFrame("Wall2.png", 15);
            AddFrame("Wall3.png", 15);
            //AnimateForwards();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
