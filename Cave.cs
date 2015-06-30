using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Hunt_the_Wumpus
{
    public enum SpecialEffects  //Holds all Special Effect possibilities
    {
        Bat, Wumpus, Pit, Nothing
    }
    public enum Directions  //Holds the possible directions 
    {
        North, NorthEast, SouthEast, South, SouthWest, NorthWest
    }

    public static class Cave
    {
        public static int MoveCounter = 0;
        public static Random rand = new Random();
        public static int WumpusRoom;
        public static int PitRoom;
        public static int BatRoom1;
        public static int BatRoom2;

        private static int _Difficulty = 2;
        public static int Difficulty    //Easy is 1, Medium is 2, Hard is 3
        {                               //Easy is high chance of rooms connecting, Wumpus does not move
            get                         //Medium is moderate chance of rooms connecting, Wumpus does not move
            {                           //Hard is a low chance of rooms connecting, and the Wumpus moves around in the cave
                return _Difficulty;
            }
            set
            {
                _Difficulty = value;
                Reset();
            }
        }

        public static int ConnectionProbability
        {
            get
            {
                return (60 / Difficulty) + 15;
            }
        }
        public class Room
        {
            public int[] Locations;  //This array holds the room numbers of the adjacent rooms (-1 if it doesn't exist)
            public int Number;  //The index of this room in the Rooms Array
            public SpecialEffects SpecialEffect;  //The room's special effect

            public int NumberOfEnemies = 3;

            private bool BatHasBeenSpawned = false;
            public bool NeedToSpawnBat
            {
                get
                {
                    return SpecialEffect == SpecialEffects.Bat && !BatHasBeenSpawned;
                }
                set
                {
                    BatHasBeenSpawned = !value;
                }
            }

            public Room(int North, int NorthE, int SouthE, int South, int SouthW, int NorthW, SpecialEffects SpecialEffect)
            {
                Locations = new int[6];
                Locations[0] = North;
                Locations[1] = NorthE;
                Locations[2] = SouthE;
                Locations[3] = South;
                Locations[4] = SouthW;
                Locations[5] = NorthW;
                this.SpecialEffect = SpecialEffect;
            }

        }
        public static Room[] Rooms = new Room[30];
        public static Room CurrentRoom;

        static Cave()
        {
            GenerateCave();
        }
        public static void Reset()
        {
            GenerateCave();
        }
        public static void GenerateCave()
        {
            bool[] connections = new bool[30];  //Holds whether or not each room is connected to the system
            connections[0] = true;
            int[,] roomLocations = new int[,]  //All of the possible connections of a room
                {
                {1, 2, 3, 4, 5, 6},
                {8, 9, 2, 0, 6, 7},
                {9, 10, 11, 3, 0, 1},
                {2, 11, 12, 13, 4, 0},
                {0, 3, 13, 14, 15, 5},
                {6, 0, 4, 15, 16, 17},
                {7, 1, 0, 5, 17, 18},
                {20, 8, 1, 6, 18, 19},
                {21, 22, 9, 1, 7, 20},
                {22, 23, 10, 2, 1, 8},
                {23, 24, 25, 11, 2, 9},
                {10, 25, 26, 12, 3, 2},
                {11, 26, 27, 28, 13, 3},
                {3, 12, 28, 29, 14, 4},
                {4, 13, 29, -1, -1, 15},
                {5, 4, 14, -1, -1, 16},
                {17, 5, 15, -1, -1, -1},
                {18, 6, 5, 16, -1, -1},
                {19, 7, 6, 17, -1, -1},
                {-1, 20, 7, 18, -1, -1},
                {-1, 21, 8, 7, 19, -1},
                {-1, -1, 22, 8, 20, -1},
                {-1, 23, 9, 8, 21, -1},
                {-1, -1, 24, 10, 9, 22},
                {-1, -1, -1, 25, 10, 23},
                {24, -1, -1, 26, 11, 10},
                {25, -1, -1, 27, 12, 11},
                {26, -1, -1, -1, 28, 12},
                {12, 27, -1, -1, 29, 13},
                {13, 28, -1, -1, -1, 14}
                };
            while (!AllTrue(connections)) //While the rooms are not all connected
            {
                for (int i = 0; i < Rooms.Length; i++)
                {
                    Rooms[i] = new Room(-1, -1, -1, -1, -1, -1, SpecialEffects.Nothing);  //Make a new cave with no connections
                }
                for (int i = 1; i < connections.Length; i++)  //Makes sure that none of the connections are true
                {
                    connections[i] = false;
                }
                for (int i = 0; i < Rooms.Length; i++)
                {

                    for (int k = 0; k < 6; k++)
                    {
                        if (roomLocations[i, k] != -1 && !AreThree(Rooms[i].Locations) && !AreThree(Rooms[roomLocations[i, k]].Locations)
                            && Rooms[roomLocations[i, k]].Locations[(k + 3) % 6] == -1) //Checks if there is a possible connection, 
                        //not three connected already in either room, and that there is 
                        //no connection already between rooms
                        {
                            int ab = rand.Next(100);
                            if (ab < ConnectionProbability) // 75%, 45%, or 35%
                            {
                                Rooms[i].Locations[k] = roomLocations[i, k]; //Assign the connection between current to corresponding room
                                Rooms[roomLocations[i, k]].Locations[(k + 3) % 6] = i; //Assigning connection between corresponding to current room
                                if (connections[i] || connections[roomLocations[i, k]]) //If either of the rooms is alreay accessible, then both rooms are accessible
                                {
                                    connections[roomLocations[i, k]] = true;
                                    connections[i] = true;
                                }
                            }
                        }
                    }
                }
            }
            //Randomly assign a special effect (Wumpus, Pit, Bat, Bat) to 4 random rooms.
            int RandNum = rand.Next(30);
            while (!Rooms[RandNum].SpecialEffect.Equals(SpecialEffects.Nothing) || RandNum == 0)
                RandNum = rand.Next(30);

            Rooms[RandNum].SpecialEffect = SpecialEffects.Wumpus;
            WumpusRoom = RandNum;

            RandNum = rand.Next(30);
            while (!Rooms[RandNum].SpecialEffect.Equals(SpecialEffects.Nothing) || RandNum == 0)
                RandNum = rand.Next(30);

            Rooms[RandNum].SpecialEffect = SpecialEffects.Pit;
            PitRoom = RandNum;

            RandNum = rand.Next(30);
            while (!Rooms[RandNum].SpecialEffect.Equals(SpecialEffects.Nothing) || RandNum == 0)
                RandNum = rand.Next(30);

            Rooms[RandNum].SpecialEffect = SpecialEffects.Pit;
            PitRoom = RandNum;

            while (!Rooms[RandNum].SpecialEffect.Equals(SpecialEffects.Nothing) || RandNum == 0)
                RandNum = rand.Next(30);

            Rooms[RandNum].SpecialEffect = SpecialEffects.Bat;
            BatRoom1 = RandNum;

            while (!Rooms[RandNum].SpecialEffect.Equals(SpecialEffects.Nothing) || RandNum == 0)
                RandNum = rand.Next(30);

            Rooms[RandNum].SpecialEffect = SpecialEffects.Bat;
            BatRoom2 = RandNum;

            CurrentRoom = Rooms[0];

            for (int i = 0; i < Rooms.Length; i++)  //Assigning the corresponding number to each room
            {
                Rooms[i].Number = i;
            }
        }
        public static bool AreThree(int[] array)  //Returns whether there are less than 3 non-negative values in the integer array
        {
            int counter = 0;
            for (int a = 0; a < array.Length; a++)
            {
                if (array[a] != -1)
                    counter++;
            }
            return counter >= 3;
        }
        public static bool AllTrue(bool[] arr) //Returns that all values of a boolean array are true -> used in validating a random generation
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (!arr[i])
                    return false;
            }
            return true;
        }
        public static void MoveRoom(Directions Direction)  //Assigns the value of the current room to the room in the desired direction
        {
            switch (Direction)
            {
                case Directions.North:
                    CurrentRoom = Rooms[CurrentRoom.Locations[0]];
                    break;
                case Directions.NorthEast:
                    CurrentRoom = Rooms[CurrentRoom.Locations[1]];
                    break;
                case Directions.SouthEast:
                    CurrentRoom = Rooms[CurrentRoom.Locations[2]];
                    break;
                case Directions.South:
                    CurrentRoom = Rooms[CurrentRoom.Locations[3]];
                    break;
                case Directions.SouthWest:
                    CurrentRoom = Rooms[CurrentRoom.Locations[4]];
                    break;
                case Directions.NorthWest:
                    CurrentRoom = Rooms[CurrentRoom.Locations[5]];
                    break;
                default:
                    return;
            }

            MoveCounter++;
            MoveWumpus();
        }
        public static void MoveWumpus()
        {
            if (MoveCounter % 3 == 0 && Difficulty == 3)
            {
                int WumpusRoom = 0;
                for (int i = 0; i < Rooms.Length; i++)
                {
                    if (Rooms[i].SpecialEffect.Equals(SpecialEffects.Wumpus))
                    {
                        WumpusRoom = i;
                        break;
                    }
                }
                int random = rand.Next(30);
                while (random == CurrentRoom.Number || !Rooms[random].SpecialEffect.Equals(SpecialEffects.Nothing))
                    random = rand.Next(30);
                Rooms[WumpusRoom].SpecialEffect = SpecialEffects.Nothing;
                Rooms[random].SpecialEffect = SpecialEffects.Wumpus;
            }
        }
        public static bool IsSpecialRoom(int room, SpecialEffects SpecialEffect)  //Returns whether a room has a particular special effect
        {
            return (Rooms[room].SpecialEffect == SpecialEffect);
        }
        public static string GetRoomMessage(int room)  //Returns the message associated with a particular room (argument is the room number)
        {
            string str = "";
            if ((Rooms[Rooms[room].Locations[0]].SpecialEffect == SpecialEffects.Wumpus) ||
               (Rooms[Rooms[room].Locations[1]].SpecialEffect == SpecialEffects.Wumpus) ||
               (Rooms[Rooms[room].Locations[2]].SpecialEffect == SpecialEffects.Wumpus) ||
               (Rooms[Rooms[room].Locations[3]].SpecialEffect == SpecialEffects.Wumpus) ||
               (Rooms[Rooms[room].Locations[4]].SpecialEffect == SpecialEffects.Wumpus) ||
               (Rooms[Rooms[room].Locations[5]].SpecialEffect == SpecialEffects.Wumpus))
            {
                str += "I smell a wumpoos!\n";
            }
            if ((Rooms[Rooms[room].Locations[0]].SpecialEffect == SpecialEffects.Pit) ||
               (Rooms[Rooms[room].Locations[1]].SpecialEffect == SpecialEffects.Pit) ||
               (Rooms[Rooms[room].Locations[2]].SpecialEffect == SpecialEffects.Pit) ||
               (Rooms[Rooms[room].Locations[3]].SpecialEffect == SpecialEffects.Pit) ||
               (Rooms[Rooms[room].Locations[4]].SpecialEffect == SpecialEffects.Pit) ||
               (Rooms[Rooms[room].Locations[5]].SpecialEffect == SpecialEffects.Pit))
            {
                str += "I feel a draft...\n";
            }
            if ((Rooms[Rooms[room].Locations[0]].SpecialEffect == SpecialEffects.Bat) ||
               (Rooms[Rooms[room].Locations[1]].SpecialEffect == SpecialEffects.Bat) ||
               (Rooms[Rooms[room].Locations[2]].SpecialEffect == SpecialEffects.Bat) ||
               (Rooms[Rooms[room].Locations[3]].SpecialEffect == SpecialEffects.Bat) ||
               (Rooms[Rooms[room].Locations[4]].SpecialEffect == SpecialEffects.Bat) ||
               (Rooms[Rooms[room].Locations[5]].SpecialEffect == SpecialEffects.Bat))
            {
                str += "I hear a bat!\n";
            }
            return str;
        }
        public static string GetRoomMessage(Room room)  //Returns the message associated with a particular room (argument is the room itself)
        {
            if (room.SpecialEffect == SpecialEffects.Wumpus)
            {
                return "I have found the WumpTron! Kill it!!";
            }

            string str = "";

            for (int i = 0; i < 6; i++)
            {
                if (room.Locations[i] > -1)
                    if (Rooms[room.Locations[i]].SpecialEffect == SpecialEffects.Wumpus)
                        str += "I smell a WumpTron!";

            }
            for (int i = 0; i < 6; i++)
            {
                if (room.Locations[i] > -1)
                    if (Rooms[room.Locations[i]].SpecialEffect == SpecialEffects.Pit)
                        str += "\nI feel a draft!";
            }
            for (int i = 0; i < 6; i++)
            {
                if (room.Locations[i] > -1)
                    if (Rooms[room.Locations[i]].SpecialEffect == SpecialEffects.Bat)
                        str += "\nI hear a bat!";

            }
            return str;
        }
    }

}