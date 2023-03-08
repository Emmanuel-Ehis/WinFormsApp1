
namespace IRM_HorseRace
{
    using System;
    using System.Threading;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using System.Diagnostics;

    ///using static System.Windows.Forms.VisualStyles.VisualStyleElement;

    /// <summary>
    /// Uses threads to run an animation independently of controls
    /// </summary>
    public class Animation : Form
    {
        // INSERT UI AND VARIABLES HERE
        private Panel controlPanel, drawingPanel;
        private Button pauseButton, clearButton, quitButton;
        private Thread DesertOrchid_t, RedRum_t;
        private Color backColor = Color.White;
        //variables
        private int topX = 0, bottomX = 0;
        int won = -1;
        public Animation()
        {
            // INSERT CONSTRUCTOR CODE HERE;

            ClientSize = new Size(500, 400);
            SetupControlPanel();
            drawingPanel = new Panel();
            drawingPanel.Bounds = new Rectangle(controlPanel.Width, 0, ClientSize.Width - controlPanel.Width, ClientSize.Height);
            drawingPanel.BackColor = backColor;
            Controls.Add(drawingPanel);
            StartPosition = FormStartPosition.CenterScreen;

            // THREAD CONTROL CODE GOES HERE

            ThreadStart drawStart_top = new System.Threading.ThreadStart(DesertOrchid);
            DesertOrchid_t = new Thread(drawStart_top);
            ThreadStart drawStart_bottom = new System.Threading.ThreadStart(RedRum);
            RedRum_t = new Thread(drawStart_bottom);
        }

        private void SetupControlPanel()
        {
            controlPanel = new Panel();
            controlPanel.Bounds = new Rectangle(0, 0, ClientSize.Width / 5, ClientSize.Height);
            controlPanel.BackColor = Color.White;
            this.Controls.Add(controlPanel);
            // Buttons
            pauseButton = new Button();
            pauseButton.Text = "Start";
            pauseButton.Bounds = new Rectangle(8, 8, 85, 25);
            pauseButton.Click += new EventHandler(pauseButton_Click);
            controlPanel.Controls.Add(pauseButton);
            clearButton = new Button();
            clearButton.Text = "Clear";
            clearButton.Bounds = new Rectangle(8, pauseButton.Bounds.Y + pauseButton.Height + 5, 85, 25);
            clearButton.Click += new EventHandler(clearButton_Click);
            controlPanel.Controls.Add(clearButton);
            quitButton = new Button();
            quitButton.Text = "Quit";
            quitButton.Bounds = new Rectangle(8, clearButton.Bounds.Y + clearButton.Height + 5, 85, 25);
            quitButton.Click += new EventHandler(quitButton_Click);
            controlPanel.Controls.Add(quitButton);
        }
        private void clearButton_Click(object sender, EventArgs args)
        {
            Graphics g = drawingPanel.CreateGraphics();
            Brush brush = new SolidBrush(backColor);
            g.FillRectangle(brush, 0, 0, drawingPanel.Width, drawingPanel.Height);
            this.bottomX = 0; this.topX = 0;
            this.won = -1;
            clearButton.BackColor = Color.White;
            Pen pen = new Pen(Color.Black, 6);
            g.DrawLine(pen, ClientSize.Width - 200, 1, ClientSize.Width - 200, ClientSize.Height);
        }

        /// <summary>
        /// Code for first horse
        /// </summary>

        /// Code for second horse

        private void DesertOrchid()
        {
            Random random = new Random();

            Graphics g = drawingPanel.CreateGraphics();
            Pen pen = new Pen(Color.Black, 6);
            // Draw finish line
            g.DrawLine(pen, ClientSize.Width - 200, ClientSize.Height / 2, ClientSize.Width - 200, ClientSize.Height);
            while (this.won < 1)
            {
                pen = new Pen(Color.FromArgb(100, random.Next(100) + 155, 100), 4);
                if (this.topX > ClientSize.Width - 200 && this.won == -1)
                {
                    this.won = 1;
                    Debug.WriteLine("Desert Orchid won");
                    //clearButton.BackColor = Color.FromArgb(100, random.Next(100) + 155, 100);
                    clearButton.BackColor = Color.FromArgb(255, 0, 255, 0);
                }
                g.DrawLine(pen, this.topX, 0, this.topX += 4, ClientSize.Height / 2);
                Thread.Sleep(random.Next(100) + 40);
            }
        }


        private void RedRum()
        {
            Random random = new Random();
            Graphics g = drawingPanel.CreateGraphics();
            Pen pen = new Pen(Color.Black, 6);
            // Draw finish line
            g.DrawLine(pen, ClientSize.Width - 200, ClientSize.Height / 2, ClientSize.Width - 200, ClientSize.Height);
            while (this.won < 1)
            {
                pen = new Pen(Color.FromArgb(random.Next(100) + 155, 100, 100), 4);
                if (this.topX > ClientSize.Width - 200 && this.won == -1)
                {
                    this.won = 1;
                    Debug.WriteLine("RedRum won");
                   // clearButton.BackColor = Color.FromArgb(random.Next(100) + 155, 100, 100);
                   clearButton.BackColor = Color.FromArgb(255,0,255,0);
                }
                g.DrawLine(pen, this.bottomX, ClientSize.Height / 2, this.bottomX += 4, ClientSize.Height);
                Thread.Sleep(random.Next(100) + 40);
            }
        }

      

        private void quitButton_Click(object sender, EventArgs args)
        {
            if ((DesertOrchid_t.ThreadState & System.Threading.ThreadState.Suspended) != 0)
                DesertOrchid_t.Resume();
            DesertOrchid_t.Abort();
            Application.Exit();
        }
        private void pauseButton_Click(object sender, EventArgs args)
        {
            if (sender == pauseButton)
            {
                if ((DesertOrchid_t.ThreadState & System.Threading.ThreadState.Suspended) != 0)
                {
                    DesertOrchid_t.Resume();
                    pauseButton.Text = "Pause";
                }
                else if ((DesertOrchid_t.ThreadState & (System.Threading.ThreadState.Running | System.Threading.ThreadState.WaitSleepJoin)) != 0)
                {
                  DesertOrchid_t.Suspend();
                    pauseButton.Text = "Resume";
               }
                else if (DesertOrchid_t.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    DesertOrchid_t.Start();
                    RedRum_t.Start();
                    pauseButton.Text = "Pause";
                }
            }
        }


        // Main - this code always at the end
        public static void Main()
        {
            Application.Run(new Animation());
        }
    }

}

