using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
namespace Hunt_the_Wumpus
{
    class Game : Form
    {
        public static Label DebugLabel;

        public static Label LifeCounter;
        public static Label DiskCounter;

        public static Label Question;

        public static Label OptionA;
        public static Label OptionB;
        public static Label OptionC;
        public static Label OptionD;

        public static Label RoomMessage;

        public static TextBox NameEnter;

        public static Stopwatch watch;

        public static bool Quit; //If Quit is true, the game will exit

        private System.ComponentModel.IContainer Components = null;

        public static PointF Dimensions; //The dimensions of the game
        public Game(uint Width, uint Height, uint FrameRate, string Name)
        {
            FrameRateController.Initialize(FrameRate); //Controls the Framerate to be at 60 FPS

            watch = new Stopwatch();

            Components = new System.ComponentModel.Container();             //Form Setup
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = Name;
            this.Width = (int)Width;
            this.Height = (int)Height;
            this.Visible = true;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Activate();
            this.BackColor = Color.Black;
            this.DoubleBuffered = true;
            this.Paint += Game_Paint;

            this.KeyDown += new KeyEventHandler(Game_KeyDown); //Keyboard Input Handler setup
            this.KeyUp += new KeyEventHandler(Game_KeyUp);

            Dimensions = new PointF(Width, Height); //Sets the dimensions of the game

            DebugLabel = new Label();
            DebugLabel.AutoSize = true;
            DebugLabel.Location = new System.Drawing.Point(100, 100);
            DebugLabel.Name = "DebugLabel";
            DebugLabel.ForeColor = Color.White;
            DebugLabel.Visible = false;
            Controls.Add(DebugLabel);

            Font CounterFont = new Font(this.Font.FontFamily, 39);

            LifeCounter = new Label();
            LifeCounter.Font = CounterFont;
            LifeCounter.AutoSize = true;
            LifeCounter.Location = new System.Drawing.Point(95, 32);
            LifeCounter.Name = "LifeCounter";
            LifeCounter.ForeColor = Color.FromArgb(220, 0, 0);
            LifeCounter.Visible = true;
            LifeCounter.BackColor = Color.Transparent;
            Controls.Add(LifeCounter);

            DiskCounter = new Label();
            DiskCounter.Font = CounterFont;
            DiskCounter.AutoSize = true;
            DiskCounter.Location = new System.Drawing.Point(95, 91);
            DiskCounter.Name = "DiskCounter";
            DiskCounter.ForeColor = Color.FromArgb(5, 5, 255);
            DiskCounter.Visible = true;
            DiskCounter.BackColor = Color.Transparent;
            Controls.Add(DiskCounter);

            Font QuestionFont = new Font(this.Font.FontFamily, 24);

            Question = new Label();
            Question.Font = QuestionFont;
            Question.AutoSize = true;
            Question.Location = new System.Drawing.Point(100, 100);
            Question.Name = "Question";
            Question.ForeColor = Color.White;
            Question.Visible = false;
            Question.BackColor = Color.Transparent;
            Controls.Add(Question);

            OptionA = new Label();
            OptionA.Font = QuestionFont;
            OptionA.AutoSize = true;
            OptionA.Location = new System.Drawing.Point(120, 365);
            OptionA.Name = "OptionA";
            OptionA.ForeColor = Color.FromArgb(5, 5, 255);
            OptionA.Visible = false;
            OptionA.BackColor = Color.Transparent;
            Controls.Add(OptionA);

            OptionB = new Label();
            OptionB.Font = QuestionFont;
            OptionB.AutoSize = true;
            OptionB.Location = new System.Drawing.Point(120, 425);
            OptionB.Name = "OptionB";
            OptionB.ForeColor = Color.FromArgb(5, 5, 255);
            OptionB.Visible = false;
            OptionB.BackColor = Color.Transparent;
            Controls.Add(OptionB);

            OptionC = new Label();
            OptionC.Font = QuestionFont;
            OptionC.AutoSize = true;
            OptionC.Location = new System.Drawing.Point(120, 485);
            OptionC.Name = "OptionC";
            OptionC.ForeColor = Color.FromArgb(5, 5, 255);
            OptionC.Visible = false;
            OptionC.BackColor = Color.Transparent;
            Controls.Add(OptionC);

            OptionD = new Label();
            OptionD.Font = QuestionFont;
            OptionD.AutoSize = true;
            OptionD.Location = new System.Drawing.Point(120, 545);
            OptionD.Name = "OptionD";
            OptionD.ForeColor = Color.FromArgb(5, 5, 255);
            OptionD.Visible = false;
            OptionD.BackColor = Color.Transparent;
            Controls.Add(OptionD);

            Font RoomMessageFont = new Font(this.Font.FontFamily, 40);

            RoomMessage = new Label();
            RoomMessage.Font = RoomMessageFont;
            RoomMessage.AutoSize = true;
            RoomMessage.Location = new System.Drawing.Point(455, 100);
            RoomMessage.Name = "RoomMessage";
            RoomMessage.ForeColor = Color.FromArgb(255, 140, 0);
            RoomMessage.Visible = false;
            RoomMessage.BackColor = Color.Transparent;
            RoomMessage.TextAlign = ContentAlignment.TopCenter;
            Controls.Add(RoomMessage);

            NameEnter = new TextBox();
            NameEnter.Font = RoomMessageFont;
            NameEnter.Location = new System.Drawing.Point(383, 560);
            NameEnter.TextAlign = HorizontalAlignment.Center;
            NameEnter.Name = "NameEnter";
            NameEnter.Visible = false;
            NameEnter.Width = 600;
            NameEnter.Enabled = true;
            Controls.Add(NameEnter);

            XInputWrapper.XboxController.StartPolling(); //Start checking the XBoxController for input and updating the state
        }

        void Game_Paint(object sender, PaintEventArgs e)
        {
            foreach (KeyValuePair<string, GameObject> Obj in ObjectManager.Objects.OrderBy(Key => Key.Value.ZOrder)) //Order the gameobjects by ZOrder, drawing the lowest first
            {
                if (Obj.Value.IsVisible)
                {
                    Obj.Value.Draw(e.Graphics, ref Dimensions);
                }
            }
        }

        void Game_KeyDown(object sender, KeyEventArgs e)
        {
            InputManager.OnKeyDown(sender, e);
        }

        void Game_KeyUp(object sender, KeyEventArgs e)
        {
            InputManager.OnKeyReleased(sender, e);
        }

        new public void Update()
        {
            Application.DoEvents();
            base.Update();
            if (FrameRateController.ProceedToNextFrame) //If we can move onto the next frame
            {
                for (int i = 0; i < InputManager.KeyBuffer.Length; i++) //Copy the asynchronous buffer into the synchronous one
                {
                    InputManager.KeysCurrentFrame[i] = InputManager.KeyBuffer[i];
                }
                this.Invalidate(); //Invalidate the current image. This calls the Game_Paint event
                ObjectManager.Update(); //Update everything in the ObjectManager
                GameStateManager.CurrentState.Update(); //Update the current GameState
                FrameRateController.ProceedToNextFrame = false; //Notify the FramerateController that the frame has been executed

                for (int i = 0; i < InputManager.KeysLastFrame.Length; i++) //Copy the synchronous buffer into the previous frame buffer
                {
                    InputManager.KeysLastFrame[i] = InputManager.KeysCurrentFrame[i];
                }
            }

        }
        new public void Dispose()
        {
            if (Components != null) //Dispose of the components
            {
                Components.Dispose();
            }
            FrameRateController.Deinitialize(); //Close the FramerateController thread
            XInputWrapper.XboxController.StopPolling(); //Close the Polling thread
        }


        private void InitializeComponent()
        {
            //DebugLabel.Size = new System.Drawing.Size(35, 13);
            //Game Setup
            this.ClientSize = new System.Drawing.Size(490, 301);
            this.DoubleBuffered = true;
            this.Name = "Game";
            this.Load += new System.EventHandler(this.Game_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown)
                return;
            Quit = true;
        }

        private void Game_Load(object sender, EventArgs e)
        {

        }
    }
}