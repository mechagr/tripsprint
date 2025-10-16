using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Media;
using System.Windows.Forms;
using NAudio.Wave;
using WinTimer = System.Windows.Forms.Timer;

namespace Tripsprint
{
    public partial class Form1 : Form
    {
        private const int JumpHeight = 150;
        private const int GroundLevel = 313;
        private const int BackgroundWidth = 900;
        private const int BackgroundHeight = 450;
        private const int BaseSpeed = 10;
        private const int WormSpeedDefault = 12;

        private Image backgroundImage;
        private Image backgroundImage2;

        private int bg1X;
        private int bg2X;
        private int bgY;
        private int wormSpeed = WormSpeedDefault;
        private int speed = BaseSpeed;
        private int score;
        private int attackTimer;
        private int attackRate;
        private int formWidth;

        private readonly int[] flyingYPositions = { 276, 350, 276, 250 };

        private bool jumping;
        private bool crouching;
        private bool flyingAttack;
        private bool gameOver;
        private bool gameStarted;

        private readonly Random random = new();
        private readonly List<PictureBox> obstacles = new();
        private readonly PictureBox hitBox = new();
        private readonly PrivateFontCollection fontCollection = new();

        private SoundPlayer backgroundMusic;
        private readonly WinTimer musicTimer = new();

        public Form1()
        {
            InitializeComponent();
            SetupGame();
            ShowLoadingScreen();
        }

        private void SetupGame()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = Color.Black;

            backgroundImage = Properties.Resources.bg;
            backgroundImage2 = Properties.Resources.bg;
            bg2X = BackgroundWidth;
            formWidth = ClientSize.Width;

            obstacles.AddRange(new[] { obstacle1, obstacle2, obstacle3 });

            backgroundMusic = new SoundPlayer(Properties.Resources.bgmusic);
            musicTimer.Interval = 51500;
            musicTimer.Tick += (_, __) =>
            {
                if (!gameOver)
                {
                    backgroundMusic.Stop();
                    backgroundMusic.Play();
                }
            };

            try
            {
                fontCollection.AddFontFile("font/pixel.ttf");
                lblScore.Font = new Font(fontCollection.Families[0], 30, FontStyle.Bold);
            }
            catch
            {
                lblScore.Font = new Font("Consolas", 30, FontStyle.Bold);
            }

            lblScore.ForeColor = Color.White;
            lblScore.BackColor = Color.Transparent;
            lblScore.Text = "Score: 0";

            hitBox.BackColor = Color.Transparent;
            hitBox.Size = new Size(eyeball.Width, eyeball.Height - 1);
            Controls.Add(hitBox);

            gameOver = false;
            gameStarted = false;
            attackRate = random.Next(12, 20);
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (!gameStarted && e.KeyCode == Keys.Space)
            {
                StartGame();
                return;
            }

            if (gameOver || !gameStarted) return;

            if (e.KeyCode == Keys.Up && !jumping)
            {
                jumping = true;
                PlaySoundEffect("jumpfx");
            }
            else if (e.KeyCode == Keys.Down && !jumping && !crouching)
            {
                eyeball.Image = Properties.Resources.low;
                eyeball.Top = 352;
                crouching = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (!gameStarted) return;

            if (e.KeyCode == Keys.Down && !gameOver)
            {
                eyeball.Image = Properties.Resources.glide;
                eyeball.Top = GroundLevel;
                crouching = false;
            }

            if (e.KeyCode == Keys.Enter && gameOver)
                ResetGame();
        }

        private void FormPaintEvent(object sender, PaintEventArgs e)
        {
            var canvas = e.Graphics;
            canvas.SmoothingMode = SmoothingMode.AntiAlias;
            canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;

            canvas.DrawImage(backgroundImage, bg1X, bgY, BackgroundWidth, BackgroundHeight);
            canvas.DrawImage(backgroundImage2, bg2X, bgY, BackgroundWidth, BackgroundHeight);

            if (!gameStarted)
            {
                canvas.TextRenderingHint = TextRenderingHint.AntiAlias;

                using var titleFont = new Font(fontCollection.Families[0], 60, FontStyle.Bold);
                using var instructionFont = new Font(fontCollection.Families[0], 24, FontStyle.Regular);

                string title = "TRIPSPRINT";
                SizeF titleSize = canvas.MeasureString(title, titleFont);
                float titleX = (ClientSize.Width - titleSize.Width) / 2;
                float titleY = (ClientSize.Height - titleSize.Height) / 2 - 50;
                canvas.DrawString(title, titleFont, Brushes.White, titleX, titleY);

                string instruction = "Press [SPACE] to Start";
                SizeF instructionSize = canvas.MeasureString(instruction, instructionFont);
                float instructionX = (ClientSize.Width - instructionSize.Width) / 2;
                float instructionY = titleY + titleSize.Height + 30;
                canvas.DrawString(instruction, instructionFont, Brushes.Gray, instructionX, instructionY);
            }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            MoveBackgrounds();
            Invalidate();

            if (!gameStarted) return;

            MoveObstacles();
            lblScore.Text = $"Score: {score}";

            hitBox.Left = eyeball.Right - (hitBox.Width + 20);
            hitBox.Top = eyeball.Top + 5;

            HandleJumping();
            CheckCollisions();
        }

        private void HandleJumping()
        {
            if (!jumping) return;

            eyeball.Top -= speed;

            if (eyeball.Top < JumpHeight)
                speed = -BaseSpeed;

            if (eyeball.Top > GroundLevel)
            {
                jumping = false;
                eyeball.Top = GroundLevel;
                speed = BaseSpeed;
            }
        }

        private void CheckCollisions()
        {
            foreach (var obstacle in obstacles)
            {
                if (!obstacle.Bounds.IntersectsWith(hitBox.Bounds)) continue;

                GameTimer.Stop();
                musicTimer.Stop();
                backgroundMusic.Stop();
                eyeball.Image = Properties.Resources.death;
                gameOver = true;

                PlaySoundEffect("hitfx");
                eyeball.Top = GroundLevel;
                break;
            }
        }

        private void ShowLoadingScreen()
        {
            lblScore.Visible = false;
            eyeball.Visible = false;
            obstacle1.Visible = false;
            obstacle2.Visible = false;
            obstacle3.Visible = false;
            hitBox.Visible = false;

            backgroundMusic.Play();
            musicTimer.Start();
            GameTimer.Start();
            Invalidate();
        }

        private void StartGame()
        {
            gameStarted = true;

            lblScore.Visible = true;
            eyeball.Visible = true;
            obstacle1.Visible = true;
            obstacle2.Visible = true;
            obstacle3.Visible = true;
            hitBox.Visible = true;

            ResetGame();
        }

        private void ResetGame()
        {
            if (!gameStarted) return;

            backgroundMusic.Stop();
            backgroundMusic.Play();
            musicTimer.Start();

            eyeball.Image = Properties.Resources.glide;
            eyeball.Top = GroundLevel;

            obstacle1.Left = formWidth + random.Next(100, 200);
            obstacle2.Left = obstacle1.Left + random.Next(600, 800);
            obstacle3.Left = obstacle1.Left + random.Next(200, 400);

            GameTimer.Start();
            score = 0;
            attackTimer = 0;
            speed = BaseSpeed;
            gameOver = false;
            crouching = false;
            wormSpeed = WormSpeedDefault;
        }

        private void MoveBackgrounds()
        {
            bg1X -= 1;
            bg2X -= 1;

            if (bg1X < -BackgroundWidth)
                bg1X = bg2X + BackgroundWidth;

            if (bg2X < -BackgroundWidth)
                bg2X = bg1X + BackgroundWidth;
        }

        private void MoveObstacles()
        {
            if (!flyingAttack)
            {
                obstacle1.Left -= wormSpeed;
                obstacle2.Left -= wormSpeed;
            }
            else
            {
                obstacle3.Left -= wormSpeed;
            }

            if (attackTimer == attackRate)
            {
                flyingAttack = true;
                attackRate = random.Next(12, 20);
            }

            if (attackTimer == 0)
                flyingAttack = false;

            if (obstacle1.Left < -100)
            {
                obstacle1.Left = obstacle2.Left + obstacle2.Width + formWidth + random.Next(100, 300);
                attackTimer++;
                score++;
            }

            if (obstacle2.Left < -100)
            {
                obstacle2.Left = obstacle1.Left + obstacle1.Width + formWidth + random.Next(100, 300);
                attackTimer++;
                score++;
            }

            if (obstacle3.Left < -100)
            {
                obstacle3.Left = formWidth + random.Next(300, 400);
                obstacle3.Top = flyingYPositions[random.Next(flyingYPositions.Length)];
                attackTimer--;
                score++;
            }
        }

        private void PlaySoundEffect(string soundName)
        {
            try
            {
                System.IO.UnmanagedMemoryStream ums = soundName switch
                {
                    "hitfx" => Properties.Resources.hitfx,
                    "jumpfx" => Properties.Resources.jumpfx,
                    _ => null
                };

                if (ums == null) return;

                ums.Position = 0;
                var reader = new WaveFileReader(ums);
                var output = new WaveOutEvent();
                output.Init(reader);
                output.Play();

                output.PlaybackStopped += (_, __) =>
                {
                    output.Dispose();
                    reader.Dispose();
                    ums.Dispose();
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Sound playback error: {ex.Message}");
            }
        }
    }
}
