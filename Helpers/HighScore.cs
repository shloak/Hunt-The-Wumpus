using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace HighScore
{
    public static class HighScores
    {
        public static List<uint> HighScore;
        public static List<string> Names;

        public static int Position(uint score)
        {
            if (HighScore != null)
            {
                if (score <= HighScore[0])
                    return 0;
                for (int i = 1; i < HighScore.Count; i++)
                {
                    if (score <= HighScore[i] && score >= HighScore[i - 1])
                        return i;
                }
                return HighScore.Count;
            }
            return -1;
        }
        public static void AddEntry(uint score, string name)
        {
            if (HighScore != null)
            {
                int position = Position(score);
                HighScore.Insert(position, score);
                Names.Insert(position, name);
                Serialize();
            }
        }
        public static uint[] GetTopScores(uint length)
        {
            uint[] scores = new uint[length];
            for (int i = 0; i < length; i++)
                scores[i] = HighScore[i];
            return scores;
        }
        public static string[] GetTopNames(uint length)
        {
            string[] allNames = new string[length];
            for (int i = 0; i < length; i++)
                allNames[i] = Names[i];
            return allNames;
        }
        public static uint GetHighScore()
        {
            return HighScore[0];
        }
        public static void Serialize()
        {
            XmlTextWriter writer = new XmlTextWriter("WumpusHighScore.sasha", null);
            XmlTextWriter writerNames = new XmlTextWriter("WumpusNames.swag", null);
            XmlSerializer serializer = new XmlSerializer(typeof(List<uint>));
            XmlSerializer serializerNames = new XmlSerializer(typeof(List<string>));
            serializer.Serialize(writer, HighScore);
            serializerNames.Serialize(writerNames, Names);
            writer.Close();
            writerNames.Close();
        }
        public static void DeSerialize()
        {
            HighScore = new List<uint>();
            Names = new List<string>();
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<uint>));
                StreamReader reader = new StreamReader("WumpusHighScore.sasha");
                HighScore = (List<uint>)serializer.Deserialize(reader);
                XmlSerializer serializerNames = new XmlSerializer(typeof(List<string>));
                StreamReader readerNames = new StreamReader("WumpusNames.swag");
                Names = (List<string>)serializer.Deserialize(reader);
                reader.Close();
                readerNames.Close();
            }
            catch
            {
                AddEntry(0, "");
                DeSerialize();
            }
        }
    }
}
