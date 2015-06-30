using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunt_the_Wumpus
{
    class HighScoreLevel : State
    {
        public override void Initialize()
        {
            base.Initialize();
            bool AreScores = false;
            uint[] scores = HighScore.HighScores.GetTopScores(10);
            string[] names = HighScore.HighScores.GetTopNames(10);
            Game.OptionA.Location = new System.Drawing.Point(400, 50);
            Game.OptionB.Location = new System.Drawing.Point(966, 50);
            Game.OptionA.Text = "SCORES";
            Game.OptionB.Text = "NAMES";
            String StringScores = "";
            String StringNames = "";
            for (int i = 0; i < scores.Length; i++)
            {
                if (scores[i] != 0)
                {
                    AreScores = true;
                    StringScores += scores[i] + "\n\n";
                    if (names[i] != null)
                        StringNames += names[i] + "\n\n";
                }
            }
            if (!AreScores)
            {
                Game.OptionC.Location = new System.Drawing.Point(683, 300);
                Game.OptionC.Text = "No Scores!";
            }
            else
            {
                Game.OptionC.Location = new System.Drawing.Point(400, 100);
                Game.OptionD.Location = new System.Drawing.Point(966, 100);
                Game.OptionC.Text = StringScores;
                Game.OptionD.Text = StringNames;
            }
        }
        public override void Update()
        {
            base.Update();
        }
    }
}
