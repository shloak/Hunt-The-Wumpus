using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointFHelp;

namespace Hunt_the_Wumpus
{
    static class ObjectManager
    {
        private const int LengthOfDictionary = 50;
        public static Dictionary<string, GameObject> Objects = new Dictionary<string, GameObject>(LengthOfDictionary); //We start this at a higher than default value for optimization purposes
        //This decreases the number of hash collisions.

        static List<GameObject> ObjectsToAdd = new List<GameObject>(); //The objects to add at the beginning of the next frame
        static List<GameObject> ObjectsToRemove = new List<GameObject>(); //The objects to remove at the beginning of the next frame
        public static void Update()
        {
            foreach (GameObject obj in ObjectsToAdd) //Add all of the objects that need adding
            {
                if (!Objects.Keys.Contains(obj.Name))
                    Objects.Add(obj.Name, obj);
            }
            foreach (GameObject obj in ObjectsToRemove) //Remove all of the objects that need removing
                Objects.Remove(obj.Name);

            ObjectsToAdd.Clear(); //Clear the lists
            ObjectsToRemove.Clear();
        }
        public static void Clear()
        {
            Objects.Clear();
            ObjectsToAdd.Clear();
            ObjectsToRemove.Clear();
        }
        public static void AddGameObject(GameObject obj) //Queues a GameObject for adding into the Objects dictionary
        {
            if (obj != null)
            {
                ObjectsToAdd.Add(obj);
            }
        }
        public static void RemoveGameObject(GameObject obj) //Queues a GameObject for removal from the Objects dictionary
        {
            if (obj != null)
            {
                ObjectsToRemove.Add(obj);
            }
        }
        public static GameObject GetObjectByName(string name) //Gets an object by name
        {
            try
            {
                return Objects[name];
            }
            catch
            {
                return null;
            }
        }

    }
}
