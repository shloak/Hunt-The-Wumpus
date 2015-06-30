using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PointFHelp;

namespace Hunt_the_Wumpus
{
    class TheLevel : State
    {
        //The walls
        Wall[] NWalls;
        Wall[] SEWalls;
        Wall[] NEWalls;
        Wall[] SWalls;
        Wall[] SWWalls;
        Wall[] NWWalls;
        Wall[] EdgeWalls;

        Player ThePlayer; //The player

        int MusicTimer;
        Sound RegularMusic = new Sound("GameMusic.mp3");
        const int RegularMusicDuration = 8; //This is in seconds
        Sound BossMusic = new Sound("BossMusic.mp3");
        const int BossMusicDuration = 26; //This is also in seconds
        bool Music; //If true, it is the BossMusic, if false it is the regular

        public override void Initialize()
        {
            base.Initialize();

            MusicTimer = RegularMusicDuration * (int)FrameRateController.FrameRate;
            RegularMusic.Play();
            Music = false;

            Game.RoomMessage.Visible = true;
            Game.LifeCounter.Visible = true;
            Game.DiskCounter.Visible = true;

            GameObject Background = new GameObject("Background", "GameBackground2.jpg", 1366, 768);
            Background.AddFrame("PitBackground.jpg", 0);
            Background.ZOrder = -10;
            ObjectManager.AddGameObject(Background);

            GameObject LifeIcon = new GameObject("LifeIcon", "LifeIcon.png", 64, 64);
            LifeIcon.Position = new PointFHelp.PointF(-619, 320);
            LifeIcon.ZOrder = 10;
            ObjectManager.AddGameObject(LifeIcon);

            GameObject DiskIcon = new GameObject("DiskIcon", "DiskIcon.png", 64, 64);
            DiskIcon.Position = new PointFHelp.PointF(-619, 256);
            DiskIcon.ZOrder = 10;
            ObjectManager.AddGameObject(DiskIcon);

            ThePlayer = new Player();
            ObjectManager.AddGameObject(ThePlayer); //Create the player
            ObjectManager.AddGameObject(new Arm(ThePlayer, 0)); //Add the arm that follows the player

            //Create the walls
            NWalls = new Wall[4];
            NEWalls = new Wall[3];
            SEWalls = new Wall[3];
            SWalls = new Wall[4];
            SWWalls = new Wall[3];
            NWWalls = new Wall[3];

            for (int i = 0; i < NWalls.Length; i++)
            {
                NWalls[i] = new Wall("NWall" + i);
                ObjectManager.AddGameObject(NWalls[i]);
                NWalls[i].Position.Y = 320;
                NWalls[i].Position.X = (128 * i) - 256;
                NWalls[i].Collidable = true;
            }
            for (int i = 0; i < NEWalls.Length; i++)
            {
                NEWalls[i] = new Wall("NEWall" + i);
                ObjectManager.AddGameObject(NEWalls[i]);
                NEWalls[i].Position.Y = 320 - (i * 128);
                NEWalls[i].Position.X = (128 * i) + 256;
                NEWalls[i].Collidable = true;
            }
            for (int i = 0; i < SEWalls.Length; i++)
            {
                SEWalls[i] = new Wall("SEWall" + i);
                ObjectManager.AddGameObject(SEWalls[i]);
                SEWalls[i].Position.Y = -320 + (i * 128);
                SEWalls[i].Position.X = (128 * i) + 256;
                SEWalls[i].Collidable = true;
            }
            for (int i = 0; i < SWalls.Length; i++)
            {
                SWalls[i] = new Wall("SWall" + i);
                ObjectManager.AddGameObject(SWalls[i]);
                SWalls[i].Position.Y = -320;
                SWalls[i].Position.X = (128 * i) - 256;
                SWalls[i].Collidable = true;
            }
            for (int i = 0; i < SWWalls.Length; i++)
            {
                SWWalls[i] = new Wall("SWWall" + i);
                ObjectManager.AddGameObject(SWWalls[i]);
                SWWalls[i].Position.Y = -320 + (i * 128);
                SWWalls[i].Position.X = (-128 * i) - 256;
                SWWalls[i].Collidable = true;
            }
            for (int i = 0; i < NWWalls.Length; i++)
            {
                NWWalls[i] = new Wall("NWWall" + i);
                ObjectManager.AddGameObject(NWWalls[i]);
                NWWalls[i].Position.Y = 320 - (i * 128);
                NWWalls[i].Position.X = (-128 * i) - 256;
                NWWalls[i].Collidable = true;
            }

            EdgeWalls = new Wall[4];
            for (int i = 0; i < EdgeWalls.Length; i++)
            {
                EdgeWalls[i] = new Wall("EdgeWall" + i);
                EdgeWalls[i].Collidable = true;
                EdgeWalls[i].CollisionBox = new PointF(128, 128);
                ObjectManager.AddGameObject(EdgeWalls[i]);
            }
            EdgeWalls[0].Position = new PointF(619, 64);
            EdgeWalls[1].Position = new PointF(619, -64);
            EdgeWalls[2].Position = new PointF(-619, 64);
            EdgeWalls[3].Position = new PointF(-619, -64);


            ThePlayer.NWalls = NWalls;
            ThePlayer.NEWalls = NEWalls;
            ThePlayer.SEWalls = SEWalls;
            ThePlayer.SWalls = SWalls;
            ThePlayer.SWWalls = SWWalls;
            ThePlayer.NWWalls = NWWalls;
            ThePlayer.EdgeWalls = EdgeWalls;
        }
        bool WumpusSpawned = false;
        bool LivesLost = false;
        public override void Update()
        {
            base.Update();

            MusicTimer--;

            if (MusicTimer == 0)
            {
                MusicTimer = (Music ? BossMusicDuration : RegularMusicDuration) * (int)FrameRateController.FrameRate;
                if (Music)
                    BossMusic.Play();
                else
                    RegularMusic.Play();
            }

            Game.RoomMessage.Visible = true;
            Game.RoomMessage.Text = Cave.GetRoomMessage(Cave.CurrentRoom);

            //This is probably not the most efficient way, but it gets vectorized so it's good enough :)
            foreach (Wall w in NWalls)
                w.Collidable = true;
            foreach (Wall w in NEWalls)
                w.Collidable = true;
            foreach (Wall w in SEWalls)
                w.Collidable = true;
            foreach (Wall w in SWalls)
                w.Collidable = true;
            foreach (Wall w in SWWalls)
                w.Collidable = true;
            foreach (Wall w in NWWalls)
                w.Collidable = true;


            //Remove all of the walls where there is a connection to another room
            if (Cave.CurrentRoom.Locations[0] != -1)
                foreach (Wall w in NWalls)
                    w.Collidable = false;
            if (Cave.CurrentRoom.Locations[1] != -1)
                foreach (Wall w in NEWalls)
                    w.Collidable = false;
            if (Cave.CurrentRoom.Locations[2] != -1)
                foreach (Wall w in SEWalls)
                    w.Collidable = false;
            if (Cave.CurrentRoom.Locations[3] != -1)
                foreach (Wall w in SWalls)
                    w.Collidable = false;
            if (Cave.CurrentRoom.Locations[4] != -1)
                foreach (Wall w in SWWalls)
                    w.Collidable = false;
            if (Cave.CurrentRoom.Locations[5] != -1)
                foreach (Wall w in NWWalls)
                    w.Collidable = false;




            if (Cave.CurrentRoom.NeedToSpawnBat)
            {
                ObjectManager.AddGameObject(new Bat(ThePlayer));
                Cave.CurrentRoom.NeedToSpawnBat = false;
            }
            if (Cave.CurrentRoom.SpecialEffect == SpecialEffects.Pit)
            {
                ObjectManager.GetObjectByName("Background").GoToFrame(1);
            }
            else
            {
                ObjectManager.GetObjectByName("Background").GoToFrame(0);
            }

            Arm a = (Arm)ObjectManager.GetObjectByName("Arm");
            if (a.Ammo <= 0)
            {
                GameObject[] Objects = ObjectManager.Objects.Values.ToArray();
                GameStateManager.CurrentState = new TriviaLevel(this, a, Objects);
            }

            if (Cave.CurrentRoom.SpecialEffect == SpecialEffects.Pit)
            {
                if (!LivesLost)
                {
                    ThePlayer.Lives -= 2;
                    LivesLost = true;
                    Game.RoomMessage.Text = "You fell into the pit!!";
                }
            }
            else
                LivesLost = false;

            if (Cave.CurrentRoom.SpecialEffect == SpecialEffects.Wumpus && !WumpusSpawned)
            {
                ObjectManager.AddGameObject(new Wumpus(ThePlayer));
                WumpusSpawned = true;
            }


            if (InputManager.IsKeyPressed(Keys.Escape))
            {
                Game.Quit = true;
            }
        }

    }
}