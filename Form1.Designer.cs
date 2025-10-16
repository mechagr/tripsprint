namespace Tripsprint
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblScore = new Label();
            pictureBox1 = new PictureBox();
            eyeball = new PictureBox();
            obstacle1 = new PictureBox();
            obstacle2 = new PictureBox();
            obstacle3 = new PictureBox();
            GameTimer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)eyeball).BeginInit();
            ((System.ComponentModel.ISupportInitialize)obstacle1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)obstacle2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)obstacle3).BeginInit();
            SuspendLayout();

            lblScore.AutoSize = true;
            lblScore.Location = new Point(12, 9);
            lblScore.Name = "lblScore";
            lblScore.Size = new Size(50, 20);
            lblScore.TabIndex = 0;
            lblScore.Text = "label1";

            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Location = new Point(2, 309);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(862, 125);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;

            eyeball.BackColor = Color.Transparent;
            eyeball.Image = Properties.Resources.glide;
            eyeball.Location = new Point(12, 287);
            eyeball.Name = "eyeball";
            eyeball.Size = new Size(22, 16);
            eyeball.SizeMode = PictureBoxSizeMode.AutoSize;
            eyeball.TabIndex = 2;
            eyeball.TabStop = false;

            obstacle1.BackColor = Color.Transparent;
            obstacle1.Image = Properties.Resources.worm1;
            obstacle1.Location = new Point(167, 287);
            obstacle1.Name = "obstacle1";
            obstacle1.Size = new Size(24, 27);
            obstacle1.SizeMode = PictureBoxSizeMode.AutoSize;
            obstacle1.TabIndex = 3;
            obstacle1.TabStop = false;
            obstacle1.Tag = "obstacle";

            obstacle2.BackColor = Color.Transparent;
            obstacle2.Image = Properties.Resources.worm2;
            obstacle2.Location = new Point(245, 284);
            obstacle2.Name = "obstacle2";
            obstacle2.Size = new Size(24, 30);
            obstacle2.TabIndex = 4;
            obstacle2.TabStop = false;
            obstacle2.Tag = "obstacle";

            obstacle3.BackColor = Color.Transparent;
            obstacle3.Image = Properties.Resources.butterfly;
            obstacle3.Location = new Point(388, 282);
            obstacle3.Name = "obstacle3";
            obstacle3.Size = new Size(27, 21);
            obstacle3.SizeMode = PictureBoxSizeMode.AutoSize;
            obstacle3.TabIndex = 5;
            obstacle3.TabStop = false;
            obstacle3.Tag = "obstacle";

            GameTimer.Enabled = true;
            GameTimer.Interval = 15;
            GameTimer.Tick += GameTimerEvent;

            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(862, 433);
            Controls.Add(obstacle3);
            Controls.Add(obstacle2);
            Controls.Add(obstacle1);
            Controls.Add(eyeball);
            Controls.Add(pictureBox1);
            Controls.Add(lblScore);
            Name = "Form1";
            Text = "Tripsprint";
            Paint += FormPaintEvent;
            KeyDown += KeyIsDown;
            KeyUp += KeyIsUp;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)eyeball).EndInit();
            ((System.ComponentModel.ISupportInitialize)obstacle1).EndInit();
            ((System.ComponentModel.ISupportInitialize)obstacle2).EndInit();
            ((System.ComponentModel.ISupportInitialize)obstacle3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblScore;
        private PictureBox pictureBox1;
        private PictureBox eyeball;
        private PictureBox obstacle1;
        private PictureBox obstacle2;
        private PictureBox obstacle3;
        private System.Windows.Forms.Timer GameTimer;
    }
}
