using DoublePendulum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.IO;
using System.Drawing.Drawing2D; 

namespace DoublePendulum
{
    public partial class OutputWindow : Form
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel SimulationPanel;
        private System.Windows.Forms.Panel MenuPanel;
        private System.Windows.Forms.Panel jsonPanel;
        private System.Windows.Forms.TrackBar Length1Scroll;
        private System.Windows.Forms.Label length1Label;
        private System.Windows.Forms.TrackBar Length2Scroll;
        private System.Windows.Forms.Label length2Label;
        private System.Windows.Forms.TrackBar Angle1Scroll;
        private System.Windows.Forms.Label angle1Label;
        private System.Windows.Forms.TrackBar Angle2Scroll;
        private System.Windows.Forms.Label angle2Label;
        private System.Windows.Forms.TextBox gTextBox;
        private System.Windows.Forms.TextBox Mass1TextBox;
        private System.Windows.Forms.TextBox Mass2TextBox;
        private System.Windows.Forms.Button StartStopButton;
        private System.Windows.Forms.Timer pendulumTimer;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button randomiseButton;
        private System.Windows.Forms.TextBox randomiseSeed;
        private System.Windows.Forms.Button menuButton;
        private System.Windows.Forms.PictureBox pendulumBox;
        private System.Windows.Forms.CheckBox showTrail;
        private System.Windows.Forms.CheckBox trailFade;
        private System.Windows.Forms.Button reRunButton;
        private System.Windows.Forms.Button multiPendulum;
        private System.Windows.Forms.TextBox noPendulums;
        private System.Windows.Forms.TextBox offsetAngle;
        private System.Windows.Forms.CheckBox hidePendulums;
        private System.Windows.Forms.TextBox customiseColour;
        private System.Windows.Forms.TextBox CustomiseBackgroundColor;
        private System.Windows.Forms.Button optionsButton;
        private System.Windows.Forms.Button confirmFilePath;
        private System.Windows.Forms.TextBox filePathBox;
        private System.Windows.Forms.Button saveJSON;
        private System.Windows.Forms.TextBox saveFilePath;
        private System.Windows.Forms.Button backFromOptionsButton;
        private System.Windows.Forms.Button enterPendulum;
        private System.Windows.Forms.Label gLabel;
        private System.Windows.Forms.Label m1Label;
        private System.Windows.Forms.Label m2Label;
        private System.Windows.Forms.Label l1StaticLabel;
        private System.Windows.Forms.Label l2StaticLabel;
        private System.Windows.Forms.Label a1StaticLabel;
        private System.Windows.Forms.Label a2StaticLabel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label penNumLabel;
        private System.Windows.Forms.Label offsetAngleLabel;
        private System.Windows.Forms.Label seedLabel;
        private System.Windows.Forms.Label trailColorLabel;
        private System.Windows.Forms.Label bgColorLabel;
        private System.Windows.Forms.Label jsonSavePathLabel;
        private System.Windows.Forms.Label jsonLoadPathLabel;
        private System.Windows.Forms.Label jsonLoadTitle;
        private System.Windows.Forms.Label jsonSaveTitle;

        private System.Windows.Forms.PictureBox gCheck;
        private System.Windows.Forms.PictureBox m1Check;
        private System.Windows.Forms.PictureBox m2Check;
        private System.Windows.Forms.PictureBox penNumCheck;
        private System.Windows.Forms.PictureBox offsetAngleCheck;
        private System.Windows.Forms.PictureBox trailColorCheck;
        private System.Windows.Forms.PictureBox bgColorCheck;
        private System.Windows.Forms.PictureBox rootColorCheck;
        private System.Windows.Forms.PictureBox middleColorCheck;
        private System.Windows.Forms.PictureBox tailColorCheck;
        private System.Windows.Forms.PictureBox lineColorCheck;
        private System.Windows.Forms.PictureBox jsonLoadCheck;
        private System.Windows.Forms.PictureBox jsonSaveCheck;

        private System.Windows.Forms.Label rootColorLabel;
        private System.Windows.Forms.TextBox rootColorBox;
        private System.Windows.Forms.Label middleColorLabel;
        private System.Windows.Forms.TextBox middleColorBox;
        private System.Windows.Forms.Label tailColorLabel;
        private System.Windows.Forms.TextBox tailColorBox;
        private System.Windows.Forms.Label lineColorLabel;
        private System.Windows.Forms.TextBox lineColorBox;


        List<Pendulum> pendulums = null;
        bool multi = false;
        bool showTrailChecked = false;
        bool showTrailFade = false;
        private string isPointDragged = "N";
        double offset = 0.5;
        int pendulumNumber = 1; 
        bool showPendulums = true;
        Color trailColor = Color.FromArgb(255, 0, 0, 255);
        Color rootColor = Color.Black;
        Color middleColor = Color.Navy;
        Color tailColor = Color.Navy;
        Color lineColor = Color.Navy;

        private Bitmap greenTick;
        private Bitmap redCross;

        public OutputWindow()
        {
            InitializeComponent();
            CreateValidationBitmaps(); 
            SetInitialValidationStatus(); 
            updateConstants(true); 
            pendulumBox.MouseDown += MainForm_MouseDown;
            pendulumBox.MouseMove += MainForm_MouseMove;
            pendulumBox.MouseUp += MainForm_MouseUp;
        }

        private void CreateValidationBitmaps()
        {
            greenTick = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(greenTick))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen greenPen = new Pen(Color.Green, 2))
                {
                    g.DrawLine(greenPen, 3, 8, 7, 12);
                    g.DrawLine(greenPen, 7, 12, 13, 4);
                }
            }

            redCross = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(redCross))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen redPen = new Pen(Color.Red, 2))
                {
                    g.DrawLine(redPen, 3, 3, 13, 13);
                    g.DrawLine(redPen, 3, 13, 13, 3);
                }
            }
        }

        private void SetInitialValidationStatus()
        {
            gCheck.Image = greenTick;
            m1Check.Image = greenTick;
            m2Check.Image = greenTick;
            penNumCheck.Image = greenTick;
            penNumCheck.Visible = false; 
            offsetAngleCheck.Image = greenTick;
            offsetAngleCheck.Visible = false;
            trailColorCheck.Image = greenTick;
            bgColorCheck.Image = greenTick;
            rootColorCheck.Image = greenTick;
            middleColorCheck.Image = greenTick;
            tailColorCheck.Image = greenTick;
            lineColorCheck.Image = greenTick;

            jsonLoadCheck.Image = null;
            jsonSaveCheck.Image = null;
        }


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SimulationPanel = new System.Windows.Forms.Panel();
            this.bgColorLabel = new System.Windows.Forms.Label();
            this.trailColorLabel = new System.Windows.Forms.Label();
            this.seedLabel = new System.Windows.Forms.Label();
            this.a2StaticLabel = new System.Windows.Forms.Label();
            this.a1StaticLabel = new System.Windows.Forms.Label();
            this.l2StaticLabel = new System.Windows.Forms.Label();
            this.l1StaticLabel = new System.Windows.Forms.Label();
            this.m2Label = new System.Windows.Forms.Label();
            this.m1Label = new System.Windows.Forms.Label();
            this.gLabel = new System.Windows.Forms.Label();

            this.pendulumBox = new System.Windows.Forms.PictureBox();
            this.Length1Scroll = new System.Windows.Forms.TrackBar();
            this.length1Label = new System.Windows.Forms.Label();
            this.Length2Scroll = new System.Windows.Forms.TrackBar();
            this.length2Label = new System.Windows.Forms.Label();
            this.Angle1Scroll = new System.Windows.Forms.TrackBar();
            this.angle1Label = new System.Windows.Forms.Label();
            this.Angle2Scroll = new System.Windows.Forms.TrackBar();
            this.angle2Label = new System.Windows.Forms.Label();
            this.gTextBox = new System.Windows.Forms.TextBox();
            this.Mass1TextBox = new System.Windows.Forms.TextBox();
            this.Mass2TextBox = new System.Windows.Forms.TextBox();
            this.StartStopButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.randomiseButton = new System.Windows.Forms.Button();
            this.randomiseSeed = new System.Windows.Forms.TextBox();
            this.menuButton = new System.Windows.Forms.Button();
            this.showTrail = new System.Windows.Forms.CheckBox();
            this.trailFade = new System.Windows.Forms.CheckBox();
            this.reRunButton = new System.Windows.Forms.Button();
            this.multiPendulum = new System.Windows.Forms.Button();
            this.noPendulums = new System.Windows.Forms.TextBox();
            this.offsetAngle = new System.Windows.Forms.TextBox();
            this.hidePendulums = new System.Windows.Forms.CheckBox();
            this.customiseColour = new System.Windows.Forms.TextBox();
            this.CustomiseBackgroundColor = new System.Windows.Forms.TextBox();
            this.gCheck = new System.Windows.Forms.PictureBox();
            this.m1Check = new System.Windows.Forms.PictureBox();
            this.m2Check = new System.Windows.Forms.PictureBox();
            this.penNumCheck = new System.Windows.Forms.PictureBox();
            this.offsetAngleCheck = new System.Windows.Forms.PictureBox();
            this.trailColorCheck = new System.Windows.Forms.PictureBox();
            this.bgColorCheck = new System.Windows.Forms.PictureBox();
            this.rootColorLabel = new System.Windows.Forms.Label();
            this.rootColorBox = new System.Windows.Forms.TextBox();
            this.middleColorLabel = new System.Windows.Forms.Label();
            this.middleColorBox = new System.Windows.Forms.TextBox();
            this.tailColorLabel = new System.Windows.Forms.Label();
            this.tailColorBox = new System.Windows.Forms.TextBox();
            this.lineColorLabel = new System.Windows.Forms.Label();
            this.lineColorBox = new System.Windows.Forms.TextBox();
            this.rootColorCheck = new System.Windows.Forms.PictureBox();
            this.middleColorCheck = new System.Windows.Forms.PictureBox();
            this.tailColorCheck = new System.Windows.Forms.PictureBox();
            this.lineColorCheck = new System.Windows.Forms.PictureBox();
            this.MenuPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.penNumLabel = new System.Windows.Forms.Label();
            this.offsetAngleLabel = new System.Windows.Forms.Label();
            this.optionsButton = new System.Windows.Forms.Button();
            this.enterPendulum = new System.Windows.Forms.Button();
            this.jsonPanel = new System.Windows.Forms.Panel();
            this.jsonSaveTitle = new System.Windows.Forms.Label();
            this.jsonLoadTitle = new System.Windows.Forms.Label();
            this.jsonLoadPathLabel = new System.Windows.Forms.Label();
            this.jsonSavePathLabel = new System.Windows.Forms.Label();
            this.backFromOptionsButton = new System.Windows.Forms.Button();
            this.saveFilePath = new System.Windows.Forms.TextBox();
            this.saveJSON = new System.Windows.Forms.Button();
            this.filePathBox = new System.Windows.Forms.TextBox();
            this.confirmFilePath = new System.Windows.Forms.Button();
            this.jsonLoadCheck = new System.Windows.Forms.PictureBox();
            this.jsonSaveCheck = new System.Windows.Forms.PictureBox();
            this.pendulumTimer = new System.Windows.Forms.Timer(this.components);
            this.SimulationPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pendulumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Length1Scroll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Length2Scroll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Angle1Scroll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Angle2Scroll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m1Check)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m2Check)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.penNumCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetAngleCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trailColorCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgColorCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rootColorCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.middleColorCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tailColorCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineColorCheck)).BeginInit();
            this.MenuPanel.SuspendLayout();
            this.jsonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jsonLoadCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.jsonSaveCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // SimulationPanel
            // 
            this.SimulationPanel.Controls.Add(this.bgColorLabel);
            this.SimulationPanel.Controls.Add(this.trailColorLabel);
            this.SimulationPanel.Controls.Add(this.seedLabel);
            this.SimulationPanel.Controls.Add(this.a2StaticLabel);
            this.SimulationPanel.Controls.Add(this.a1StaticLabel);
            this.SimulationPanel.Controls.Add(this.l2StaticLabel);
            this.SimulationPanel.Controls.Add(this.l1StaticLabel);
            this.SimulationPanel.Controls.Add(this.m2Label);
            this.SimulationPanel.Controls.Add(this.m1Label);
            this.SimulationPanel.Controls.Add(this.gLabel);
            this.SimulationPanel.Controls.Add(this.penNumLabel);
            this.SimulationPanel.Controls.Add(this.offsetAngleLabel);
            this.SimulationPanel.Controls.Add(this.pendulumBox);
            this.SimulationPanel.Controls.Add(this.Length1Scroll);
            this.SimulationPanel.Controls.Add(this.length1Label);
            this.SimulationPanel.Controls.Add(this.Length2Scroll);
            this.SimulationPanel.Controls.Add(this.length2Label);
            this.SimulationPanel.Controls.Add(this.Angle1Scroll);
            this.SimulationPanel.Controls.Add(this.angle1Label);
            this.SimulationPanel.Controls.Add(this.Angle2Scroll);
            this.SimulationPanel.Controls.Add(this.angle2Label);
            this.SimulationPanel.Controls.Add(this.gTextBox);
            this.SimulationPanel.Controls.Add(this.Mass1TextBox);
            this.SimulationPanel.Controls.Add(this.Mass2TextBox);
            this.SimulationPanel.Controls.Add(this.StartStopButton);
            this.SimulationPanel.Controls.Add(this.resetButton);
            this.SimulationPanel.Controls.Add(this.randomiseButton);
            this.SimulationPanel.Controls.Add(this.randomiseSeed);
            this.SimulationPanel.Controls.Add(this.menuButton);
            this.SimulationPanel.Controls.Add(this.showTrail);
            this.SimulationPanel.Controls.Add(this.trailFade);
            this.SimulationPanel.Controls.Add(this.reRunButton);
            this.SimulationPanel.Controls.Add(this.multiPendulum);
            this.SimulationPanel.Controls.Add(this.noPendulums);
            this.SimulationPanel.Controls.Add(this.offsetAngle);
            this.SimulationPanel.Controls.Add(this.hidePendulums);
            this.SimulationPanel.Controls.Add(this.customiseColour);
            this.SimulationPanel.Controls.Add(this.CustomiseBackgroundColor);
            this.SimulationPanel.Controls.Add(this.gCheck);
            this.SimulationPanel.Controls.Add(this.m1Check);
            this.SimulationPanel.Controls.Add(this.m2Check);
            this.SimulationPanel.Controls.Add(this.penNumCheck);
            this.SimulationPanel.Controls.Add(this.offsetAngleCheck);
            this.SimulationPanel.Controls.Add(this.trailColorCheck);
            this.SimulationPanel.Controls.Add(this.bgColorCheck);
            this.SimulationPanel.Controls.Add(this.rootColorLabel);
            this.SimulationPanel.Controls.Add(this.rootColorBox);
            this.SimulationPanel.Controls.Add(this.middleColorLabel);
            this.SimulationPanel.Controls.Add(this.middleColorBox);
            this.SimulationPanel.Controls.Add(this.tailColorLabel);
            this.SimulationPanel.Controls.Add(this.tailColorBox);
            this.SimulationPanel.Controls.Add(this.lineColorLabel);
            this.SimulationPanel.Controls.Add(this.lineColorBox);
            this.SimulationPanel.Controls.Add(this.rootColorCheck);
            this.SimulationPanel.Controls.Add(this.middleColorCheck);
            this.SimulationPanel.Controls.Add(this.tailColorCheck);
            this.SimulationPanel.Controls.Add(this.lineColorCheck);
            this.SimulationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SimulationPanel.Location = new System.Drawing.Point(0, 0);
            this.SimulationPanel.Name = "SimulationPanel";
            this.SimulationPanel.Size = new System.Drawing.Size(1260,801);
            this.SimulationPanel.TabIndex = 0;
            this.SimulationPanel.Visible = false;
            // 
            // bgColorLabel
            // 
            this.bgColorLabel.AutoSize = true;
            this.bgColorLabel.Location = new System.Drawing.Point(1060, 497);
            this.bgColorLabel.Name = "bgColorLabel";
            this.bgColorLabel.Size = new System.Drawing.Size(95, 13);
            this.bgColorLabel.TabIndex = 35;
            this.bgColorLabel.Text = "Background Color:";
            // 
            // trailColorLabel
            // 
            this.trailColorLabel.AutoSize = true;
            this.trailColorLabel.Location = new System.Drawing.Point(1060, 448);
            this.trailColorLabel.Name = "trailColorLabel";
            this.trailColorLabel.Size = new System.Drawing.Size(57, 13);
            this.trailColorLabel.TabIndex = 34;
            this.trailColorLabel.Text = "Trail Color:";
            // 
            // seedLabel
            // 
            this.seedLabel.AutoSize = true;
            this.seedLabel.Location = new System.Drawing.Point(1060, 273);
            this.seedLabel.Name = "seedLabel";
            this.seedLabel.Size = new System.Drawing.Size(78, 13);
            this.seedLabel.TabIndex = 33;
            this.seedLabel.Text = "Random Seed:";
            // 
            // a2StaticLabel
            // 
            this.a2StaticLabel.AutoSize = true;
            this.a2StaticLabel.Location = new System.Drawing.Point(1060, 198);
            this.a2StaticLabel.Name = "a2StaticLabel";
            this.a2StaticLabel.Size = new System.Drawing.Size(46, 13);
            this.a2StaticLabel.TabIndex = 32;
            this.a2StaticLabel.Text = "Angle 2:";
            // 
            // a1StaticLabel
            // 
            this.a1StaticLabel.AutoSize = true;
            this.a1StaticLabel.Location = new System.Drawing.Point(1060, 134);
            this.a1StaticLabel.Name = "a1StaticLabel";
            this.a1StaticLabel.Size = new System.Drawing.Size(46, 13);
            this.a1StaticLabel.TabIndex = 31;
            this.a1StaticLabel.Text = "Angle 1:";
            // 
            // l2StaticLabel
            // 
            this.l2StaticLabel.AutoSize = true;
            this.l2StaticLabel.Location = new System.Drawing.Point(1060, 70);
            this.l2StaticLabel.Name = "l2StaticLabel";
            this.l2StaticLabel.Size = new System.Drawing.Size(52, 13);
            this.l2StaticLabel.TabIndex = 30;
            this.l2StaticLabel.Text = "Length 2:";
            // 
            // l1StaticLabel
            // 
            this.l1StaticLabel.AutoSize = true;
            this.l1StaticLabel.Location = new System.Drawing.Point(1060, 9);
            this.l1StaticLabel.Name = "l1StaticLabel";
            this.l1StaticLabel.Size = new System.Drawing.Size(52, 13);
            this.l1StaticLabel.TabIndex = 29;
            this.l1StaticLabel.Text = "Length 1:";
            // 
            // m2Label
            // 
            this.m2Label.AutoSize = true;
            this.m2Label.Location = new System.Drawing.Point(908, 88);
            this.m2Label.Name = "m2Label";
            this.m2Label.Size = new System.Drawing.Size(44, 13);
            this.m2Label.TabIndex = 28;
            this.m2Label.Text = "Mass 2:";
            // 
            // m1Label
            // 
            this.m1Label.AutoSize = true;
            this.m1Label.Location = new System.Drawing.Point(908, 49);
            this.m1Label.Name = "m1Label";
            this.m1Label.Size = new System.Drawing.Size(44, 13);
            this.m1Label.TabIndex = 27;
            this.m1Label.Text = "Mass 1:";
            // 
            // gLabel
            // 
            this.gLabel.AutoSize = true;
            this.gLabel.Location = new System.Drawing.Point(908, 10);
            this.gLabel.Name = "gLabel";
            this.gLabel.Size = new System.Drawing.Size(16, 13);
            this.gLabel.TabIndex = 26;
            this.gLabel.Text = "g:";
            // 
            // pendulumBox
            // 
            this.pendulumBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pendulumBox.Location = new System.Drawing.Point(12, 12);
            this.pendulumBox.Name = "pendulumBox";
            this.pendulumBox.Size = new System.Drawing.Size(886, 777);
            this.pendulumBox.TabIndex = 0;
            this.pendulumBox.TabStop = false;
            // 
            // Length1Scroll
            // 
            this.Length1Scroll.Location = new System.Drawing.Point(1063, 25);
            this.Length1Scroll.Maximum = 100;
            this.Length1Scroll.Minimum = 10;
            this.Length1Scroll.Name = "Length1Scroll";
            this.Length1Scroll.Size = new System.Drawing.Size(170, 45);
            this.Length1Scroll.TabIndex = 1;
            this.Length1Scroll.Value = 50;
            this.Length1Scroll.Scroll += new System.EventHandler(this.Length1Scroll_Scroll);
            // 
            // length1Label
            // 
            this.length1Label.AutoSize = true;
            this.length1Label.Location = new System.Drawing.Point(1118, 9);
            this.length1Label.Name = "length1Label";
            this.length1Label.Size = new System.Drawing.Size(19, 13);
            this.length1Label.TabIndex = 2;
            this.length1Label.Text = "50";
            // 
            // Length2Scroll
            // 
            this.Length2Scroll.Location = new System.Drawing.Point(1063, 88);
            this.Length2Scroll.Maximum = 100;
            this.Length2Scroll.Minimum = 10;
            this.Length2Scroll.Name = "Length2Scroll";
            this.Length2Scroll.Size = new System.Drawing.Size(170, 45);
            this.Length2Scroll.TabIndex = 3;
            this.Length2Scroll.Value = 50;
            this.Length2Scroll.Scroll += new System.EventHandler(this.Length2Scroll_Scroll);
            // 
            // length2Label
            // 
            this.length2Label.AutoSize = true;
            this.length2Label.Location = new System.Drawing.Point(1118, 70);
            this.length2Label.Name = "length2Label";
            this.length2Label.Size = new System.Drawing.Size(19, 13);
            this.length2Label.TabIndex = 4;
            this.length2Label.Text = "50";
            // 
            // Angle1Scroll
            // 
            this.Angle1Scroll.Location = new System.Drawing.Point(1063, 151);
            this.Angle1Scroll.Maximum = 180;
            this.Angle1Scroll.Minimum = -180;
            this.Angle1Scroll.Name = "Angle1Scroll";
            this.Angle1Scroll.Size = new System.Drawing.Size(170, 45);
            this.Angle1Scroll.TabIndex = 5;
            this.Angle1Scroll.Scroll += new System.EventHandler(this.Angle1Scroll_Scroll);
            // 
            // angle1Label
            // 
            this.angle1Label.AutoSize = true;
            this.angle1Label.Location = new System.Drawing.Point(1118, 134);
            this.angle1Label.Name = "angle1Label";
            this.angle1Label.Size = new System.Drawing.Size(13, 13);
            this.angle1Label.TabIndex = 6;
            this.angle1Label.Text = "0";
            // 
            // Angle2Scroll
            // 
            this.Angle2Scroll.Location = new System.Drawing.Point(1063, 215);
            this.Angle2Scroll.Maximum = 180;
            this.Angle2Scroll.Minimum = -180;
            this.Angle2Scroll.Name = "Angle2Scroll";
            this.Angle2Scroll.Size = new System.Drawing.Size(170, 45);
            this.Angle2Scroll.TabIndex = 7;
            this.Angle2Scroll.Scroll += new System.EventHandler(this.Angle2Scroll_Scroll);
            // 
            // angle2Label
            // 
            this.angle2Label.AutoSize = true;
            this.angle2Label.Location = new System.Drawing.Point(1118, 198);
            this.angle2Label.Name = "angle2Label";
            this.angle2Label.Size = new System.Drawing.Size(13, 13);
            this.angle2Label.TabIndex = 8;
            this.angle2Label.Text = "0";
            // 
            // gTextBox
            // 
            this.gTextBox.Location = new System.Drawing.Point(911, 26);
            this.gTextBox.Name = "gTextBox";
            this.gTextBox.Size = new System.Drawing.Size(100, 20);
            this.gTextBox.TabIndex = 9;
            this.gTextBox.Text = "9.81";
            this.gTextBox.TextChanged += new System.EventHandler(this.gTextBox_TextChanged);
            // 
            // Mass1TextBox
            // 
            this.Mass1TextBox.Location = new System.Drawing.Point(911, 65);
            this.Mass1TextBox.Name = "Mass1TextBox";
            this.Mass1TextBox.Size = new System.Drawing.Size(100, 20);
            this.Mass1TextBox.TabIndex = 11;
            this.Mass1TextBox.Text = "10";
            this.Mass1TextBox.TextChanged += new System.EventHandler(this.Mass1TextBox_TextChanged);
            // 
            // Mass2TextBox
            // 
            this.Mass2TextBox.Location = new System.Drawing.Point(911, 104);
            this.Mass2TextBox.Name = "Mass2TextBox";
            this.Mass2TextBox.Size = new System.Drawing.Size(100, 20);
            this.Mass2TextBox.TabIndex = 13;
            this.Mass2TextBox.Text = "10";
            this.Mass2TextBox.TextChanged += new System.EventHandler(this.Mass2TextBox_TextChanged);
            // 
            // StartStopButton
            // 
            this.StartStopButton.Location = new System.Drawing.Point(911, 198);
            this.StartStopButton.Name = "StartStopButton";
            this.StartStopButton.Size = new System.Drawing.Size(100, 23);
            this.StartStopButton.TabIndex = 15;
            this.StartStopButton.Text = "START";
            this.StartStopButton.UseVisualStyleBackColor = true;
            this.StartStopButton.Click += new System.EventHandler(this.StartStopButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(911, 227);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(100, 23);
            this.resetButton.TabIndex = 16;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // randomiseButton
            // 
            this.randomiseButton.Location = new System.Drawing.Point(911, 289);
            this.randomiseButton.Name = "randomiseButton";
            this.randomiseButton.Size = new System.Drawing.Size(100, 23);
            this.randomiseButton.TabIndex = 17;
            this.randomiseButton.Text = "Randomise";
            this.randomiseButton.UseVisualStyleBackColor = true;
            this.randomiseButton.Click += new System.EventHandler(this.randomiseButton_Click);
            // 
            // randomiseSeed
            // 
            this.randomiseSeed.Location = new System.Drawing.Point(1063, 289);
            this.randomiseSeed.Name = "randomiseSeed";
            this.randomiseSeed.Size = new System.Drawing.Size(170, 20);
            this.randomiseSeed.TabIndex = 18;
            // 
            // menuButton
            // 
            this.menuButton.Location = new System.Drawing.Point(1121, 766);
            this.menuButton.Name = "menuButton";
            this.menuButton.Size = new System.Drawing.Size(75, 23);
            this.menuButton.TabIndex = 19;
            this.menuButton.Text = "Menu";
            this.menuButton.UseVisualStyleBackColor = true;
            this.menuButton.Click += new System.EventHandler(this.menuButton_Click);
            // 
            // showTrail
            // 
            this.showTrail.AutoSize = true;
            this.showTrail.Location = new System.Drawing.Point(911, 362);
            this.showTrail.Name = "showTrail";
            this.showTrail.Size = new System.Drawing.Size(76, 17);
            this.showTrail.TabIndex = 20;
            this.showTrail.Text = "Show Trail";
            this.showTrail.UseVisualStyleBackColor = true;
            this.showTrail.CheckedChanged += new System.EventHandler(this.showTrail_CheckedChanged);
            // 
            // trailFade
            // 
            this.trailFade.AutoSize = true;
            this.trailFade.Location = new System.Drawing.Point(911, 385);
            this.trailFade.Name = "trailFade";
            this.trailFade.Size = new System.Drawing.Size(73, 17);
            this.trailFade.TabIndex = 21;
            this.trailFade.Text = "Trail Fade";
            this.trailFade.UseVisualStyleBackColor = true;
            this.trailFade.Visible = false;
            this.trailFade.CheckedChanged += new System.EventHandler(this.trailFade_CheckedChanged);
            // 
            // reRunButton
            // 
            this.reRunButton.Location = new System.Drawing.Point(911, 256);
            this.reRunButton.Name = "reRunButton";
            this.reRunButton.Size = new System.Drawing.Size(100, 23);
            this.reRunButton.TabIndex = 22;
            this.reRunButton.Text = "Re-run";
            this.reRunButton.UseVisualStyleBackColor = true;
            this.reRunButton.Click += new System.EventHandler(this.reRunButton_Click);
            // 
            // multiPendulum
            // 
            this.multiPendulum.Location = new System.Drawing.Point(1063, 333);
            this.multiPendulum.Name = "multiPendulum";
            this.multiPendulum.Size = new System.Drawing.Size(170, 23);
            this.multiPendulum.TabIndex = 23;
            this.multiPendulum.Text = "Enable Multiple Pendulums";
            this.multiPendulum.UseVisualStyleBackColor = true;
            this.multiPendulum.Click += new System.EventHandler(this.multiPendulum_Click);
            // 
            // noPendulums
            // 
            this.noPendulums.Location = new System.Drawing.Point(1183, 362);
            this.noPendulums.Name = "noPendulums";
            this.noPendulums.Size = new System.Drawing.Size(50, 20);
            this.noPendulums.TabIndex = 24;
            this.noPendulums.Text = "1";
            this.noPendulums.Visible = false;
            this.noPendulums.TextChanged += new System.EventHandler(this.noPendulums_TextChanged);
            //
            // penNumLabel
            //
            this.penNumLabel.AutoSize = true;
            this.penNumLabel.Location = new System.Drawing.Point(1060, 362);
            this.penNumLabel.Name = "penNumLabel";
            this.penNumLabel.Size = new System.Drawing.Size(101, 13);
            this.penNumLabel.TabIndex = 35;
            this.penNumLabel.Text = "Number of Pendulums:";
            this.penNumLabel.Visible = false;
            // 
            // offsetAngle
            // 
            this.offsetAngle.Location = new System.Drawing.Point(1183, 390);
            this.offsetAngle.Name = "offsetAngle";
            this.offsetAngle.Size = new System.Drawing.Size(30, 20);
            this.offsetAngle.TabIndex = 36;
            this.offsetAngle.Text = "0.5";
            this.offsetAngle.Visible = false;
            this.offsetAngle.TextChanged += new System.EventHandler(this.offsetAngle_TextChanged);
            //
            // offsetAngleLabel
            //
            this.offsetAngleLabel.AutoSize = true;
            this.offsetAngleLabel.Location = new System.Drawing.Point(1060, 390);
            this.offsetAngleLabel.Name = "offsetAngleLabel";
            this.offsetAngleLabel.Size = new System.Drawing.Size(72, 13);
            this.offsetAngleLabel.TabIndex = 37;
            this.offsetAngleLabel.Text = "Offset Angle:";
            this.offsetAngleLabel.Visible = false;
            // 
            // hidePendulums
            // 
            this.hidePendulums.AutoSize = true;
            this.hidePendulums.Location = new System.Drawing.Point(911, 408);
            this.hidePendulums.Name = "hidePendulums";
            this.hidePendulums.Size = new System.Drawing.Size(103, 17);
            this.hidePendulums.TabIndex = 26;
            this.hidePendulums.Text = "Hide Pendulums";
            this.hidePendulums.UseVisualStyleBackColor = true;
            this.hidePendulums.CheckedChanged += new System.EventHandler(this.hidePendulums_CheckedChanged);
            // 
            // customiseColour
            // 
            this.customiseColour.Location = new System.Drawing.Point(1063, 464);
            this.customiseColour.Name = "customiseColour";
            this.customiseColour.Size = new System.Drawing.Size(170, 20);
            this.customiseColour.TabIndex = 27;
            this.customiseColour.Text = "Blue";
            this.customiseColour.TextChanged += new System.EventHandler(this.customiseColour_TextChanged);
            // 
            // CustomiseBackgroundColor
            // 
            this.CustomiseBackgroundColor.Location = new System.Drawing.Point(1063, 513);
            this.CustomiseBackgroundColor.Name = "CustomiseBackgroundColor";
            this.CustomiseBackgroundColor.Size = new System.Drawing.Size(170, 20);
            this.CustomiseBackgroundColor.TabIndex = 29;
            this.CustomiseBackgroundColor.Text = "WhiteSmoke";
            this.CustomiseBackgroundColor.TextChanged += new System.EventHandler(this.customiseColour_TextChanged);
            // 
            // gCheck
            // 
            this.gCheck.Location = new System.Drawing.Point(1013, 26);
            this.gCheck.Name = "gCheck";
            this.gCheck.Size = new System.Drawing.Size(16, 16);
            this.gCheck.TabIndex = 48;
            this.gCheck.TabStop = false;
            // 
            // m1Check
            // 
            this.m1Check.Location = new System.Drawing.Point(1013, 65);
            this.m1Check.Name = "m1Check";
            this.m1Check.Size = new System.Drawing.Size(16, 16);
            this.m1Check.TabIndex = 49;
            this.m1Check.TabStop = false;
            // 
            // m2Check
            // 
            this.m2Check.Location = new System.Drawing.Point(1013, 104);
            this.m2Check.Name = "m2Check";
            this.m2Check.Size = new System.Drawing.Size(16, 16);
            this.m2Check.TabIndex = 50;
            this.m2Check.TabStop = false;
            // 
            // penNumCheck
            // 
            this.penNumCheck.Location = new System.Drawing.Point(1235, 362);
            this.penNumCheck.Name = "penNumCheck";
            this.penNumCheck.Size = new System.Drawing.Size(16, 16);
            this.penNumCheck.TabIndex = 51;
            this.penNumCheck.TabStop = false;
            // 
            // offsetAngleCheck
            // 
            this.offsetAngleCheck.Location = new System.Drawing.Point(1235, 390);
            this.offsetAngleCheck.Name = "offsetAngleCheck";
            this.offsetAngleCheck.Size = new System.Drawing.Size(16, 16);
            this.offsetAngleCheck.TabIndex = 58;
            this.offsetAngleCheck.TabStop = false;
            // 
            // trailColorCheck
            // 
            this.trailColorCheck.Location = new System.Drawing.Point(1235, 464);
            this.trailColorCheck.Name = "trailColorCheck";
            this.trailColorCheck.Size = new System.Drawing.Size(16, 16);
            this.trailColorCheck.TabIndex = 52;
            this.trailColorCheck.TabStop = false;
            // 
            // bgColorCheck
            // 
            this.bgColorCheck.Location = new System.Drawing.Point(1235, 513);
            this.bgColorCheck.Name = "bgColorCheck";
            this.bgColorCheck.Size = new System.Drawing.Size(16, 16);
            this.bgColorCheck.TabIndex = 53;
            this.bgColorCheck.TabStop = false;
            // 
            // rootColorLabel
            // 
            this.rootColorLabel.AutoSize = true;
            this.rootColorLabel.Location = new System.Drawing.Point(1060, 546);
            this.rootColorLabel.Name = "rootColorLabel";
            this.rootColorLabel.Size = new System.Drawing.Size(60, 13);
            this.rootColorLabel.TabIndex = 36;
            this.rootColorLabel.Text = "Root Color:";
            // 
            // rootColorBox
            // 
            this.rootColorBox.Location = new System.Drawing.Point(1063, 562);
            this.rootColorBox.Name = "rootColorBox";
            this.rootColorBox.Size = new System.Drawing.Size(170, 20);
            this.rootColorBox.TabIndex = 37;
            this.rootColorBox.Text = "Black";
            this.rootColorBox.TextChanged += new System.EventHandler(this.customiseColour_TextChanged);
            // 
            // middleColorLabel
            // 
            this.middleColorLabel.AutoSize = true;
            this.middleColorLabel.Location = new System.Drawing.Point(1060, 594);
            this.middleColorLabel.Name = "middleColorLabel";
            this.middleColorLabel.Size = new System.Drawing.Size(68, 13);
            this.middleColorLabel.TabIndex = 39;
            this.middleColorLabel.Text = "Middle Color:";
            // 
            // middleColorBox
            // 
            this.middleColorBox.Location = new System.Drawing.Point(1063, 610);
            this.middleColorBox.Name = "middleColorBox";
            this.middleColorBox.Size = new System.Drawing.Size(170, 20);
            this.middleColorBox.TabIndex = 40;
            this.middleColorBox.Text = "Navy";
            this.middleColorBox.TextChanged += new System.EventHandler(this.customiseColour_TextChanged);
            // 
            // tailColorLabel
            // 
            this.tailColorLabel.AutoSize = true;
            this.tailColorLabel.Location = new System.Drawing.Point(1060, 642);
            this.tailColorLabel.Name = "tailColorLabel";
            this.tailColorLabel.Size = new System.Drawing.Size(54, 13);
            this.tailColorLabel.TabIndex = 42;
            this.tailColorLabel.Text = "Tail Color:";
            // 
            // tailColorBox
            // 
            this.tailColorBox.Location = new System.Drawing.Point(1063, 658);
            this.tailColorBox.Name = "tailColorBox";
            this.tailColorBox.Size = new System.Drawing.Size(170, 20);
            this.tailColorBox.TabIndex = 43;
            this.tailColorBox.Text = "Navy";
            this.tailColorBox.TextChanged += new System.EventHandler(this.customiseColour_TextChanged);
            // 
            // lineColorLabel
            // 
            this.lineColorLabel.AutoSize = true;
            this.lineColorLabel.Location = new System.Drawing.Point(1060, 690);
            this.lineColorLabel.Name = "lineColorLabel";
            this.lineColorLabel.Size = new System.Drawing.Size(57, 13);
            this.lineColorLabel.TabIndex = 45;
            this.lineColorLabel.Text = "Line Color:";
            // 
            // lineColorBox
            // 
            this.lineColorBox.Location = new System.Drawing.Point(1063, 706);
            this.lineColorBox.Name = "lineColorBox";
            this.lineColorBox.Size = new System.Drawing.Size(170, 20);
            this.lineColorBox.TabIndex = 46;
            this.lineColorBox.Text = "Navy";
            this.lineColorBox.TextChanged += new System.EventHandler(this.customiseColour_TextChanged);
            // 
            // rootColorCheck
            // 
            this.rootColorCheck.Location = new System.Drawing.Point(1235, 562);
            this.rootColorCheck.Name = "rootColorCheck";
            this.rootColorCheck.Size = new System.Drawing.Size(16, 16);
            this.rootColorCheck.TabIndex = 54;
            this.rootColorCheck.TabStop = false;
            // 
            // middleColorCheck
            // 
            this.middleColorCheck.Location = new System.Drawing.Point(1235, 610);
            this.middleColorCheck.Name = "middleColorCheck";
            this.middleColorCheck.Size = new System.Drawing.Size(16, 16);
            this.middleColorCheck.TabIndex = 55;
            this.middleColorCheck.TabStop = false;
            // 
            // tailColorCheck
            // 
            this.tailColorCheck.Location = new System.Drawing.Point(1235, 658);
            this.tailColorCheck.Name = "tailColorCheck";
            this.tailColorCheck.Size = new System.Drawing.Size(16, 16);
            this.tailColorCheck.TabIndex = 56;
            this.tailColorCheck.TabStop = false;
            // 
            // lineColorCheck
            // 
            this.lineColorCheck.Location = new System.Drawing.Point(1235, 706);
            this.lineColorCheck.Name = "lineColorCheck";
            this.lineColorCheck.Size = new System.Drawing.Size(16, 16);
            this.lineColorCheck.TabIndex = 57;
            this.lineColorCheck.TabStop = false;
            // 
            // MenuPanel
            // 
            this.MenuPanel.Controls.Add(this.titleLabel);
            this.MenuPanel.Controls.Add(this.optionsButton);
            this.MenuPanel.Controls.Add(this.enterPendulum);
            this.MenuPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MenuPanel.Location = new System.Drawing.Point(0, 0);
            this.MenuPanel.Name = "MenuPanel";
            this.MenuPanel.Size = new System.Drawing.Size(1260,801);
            this.MenuPanel.TabIndex = 20;
            this.MenuPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.MenuPanel_Paint);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(421, 251);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(379, 37);
            this.titleLabel.TabIndex = 2;
            this.titleLabel.Text = "Double Pendulum Project";
            // 
            // optionsButton
            // 
            this.optionsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsButton.Location = new System.Drawing.Point(503, 418);
            this.optionsButton.Name = "optionsButton";
            this.optionsButton.Size = new System.Drawing.Size(202, 60);
            this.optionsButton.TabIndex = 1;
            this.optionsButton.Text = "Options";
            this.optionsButton.UseVisualStyleBackColor = true;
            this.optionsButton.Click += new System.EventHandler(this.optionsButton_Click);
            // 
            // enterPendulum
            // 
            this.enterPendulum.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enterPendulum.Location = new System.Drawing.Point(503, 330);
            this.enterPendulum.Name = "enterPendulum";
            this.enterPendulum.Size = new System.Drawing.Size(202, 60);
            this.enterPendulum.TabIndex = 0;
            this.enterPendulum.Text = "Enter Simulation";
            this.enterPendulum.UseVisualStyleBackColor = true;
            this.enterPendulum.Click += new System.EventHandler(this.enterPendulum_Click);
            // 
            // jsonPanel
            // 
            this.jsonPanel.Controls.Add(this.jsonSaveTitle);
            this.jsonPanel.Controls.Add(this.jsonLoadTitle);
            this.jsonPanel.Controls.Add(this.jsonLoadPathLabel);
            this.jsonPanel.Controls.Add(this.jsonSavePathLabel);
            this.jsonPanel.Controls.Add(this.backFromOptionsButton);
            this.jsonPanel.Controls.Add(this.saveFilePath);
            this.jsonPanel.Controls.Add(this.saveJSON);
            this.jsonPanel.Controls.Add(this.filePathBox);
            this.jsonPanel.Controls.Add(this.confirmFilePath);
            this.jsonPanel.Controls.Add(this.jsonLoadCheck);
            this.jsonPanel.Controls.Add(this.jsonSaveCheck);
            this.jsonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jsonPanel.Location = new System.Drawing.Point(0, 0);
            this.jsonPanel.Name = "jsonPanel";
            this.jsonPanel.Size = new System.Drawing.Size(1260,801);
            this.jsonPanel.TabIndex = 21;
            this.jsonPanel.Visible = false;
            // 
            // jsonSaveTitle
            // 
            this.jsonSaveTitle.AutoSize = true;
            this.jsonSaveTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jsonSaveTitle.Location = new System.Drawing.Point(527, 387);
            this.jsonSaveTitle.Name = "jsonSaveTitle";
            this.jsonSaveTitle.Size = new System.Drawing.Size(145, 25);
            this.jsonSaveTitle.TabIndex = 10;
            this.jsonSaveTitle.Text = "Save Settings";
            // 
            // jsonLoadTitle
            // 
            this.jsonLoadTitle.AutoSize = true;
            this.jsonLoadTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jsonLoadTitle.Location = new System.Drawing.Point(527, 234);
            this.jsonLoadTitle.Name = "jsonLoadTitle";
            this.jsonLoadTitle.Size = new System.Drawing.Size(144, 25);
            this.jsonLoadTitle.TabIndex = 9;
            this.jsonLoadTitle.Text = "Load Settings";
            // 
            // jsonLoadPathLabel
            // 
            this.jsonLoadPathLabel.AutoSize = true;
            this.jsonLoadPathLabel.Location = new System.Drawing.Point(444, 275);
            this.jsonLoadPathLabel.Name = "jsonLoadPathLabel";
            this.jsonLoadPathLabel.Size = new System.Drawing.Size(51, 13);
            this.jsonLoadPathLabel.TabIndex = 8;
            this.jsonLoadPathLabel.Text = "File Path:";
            // 
            // jsonSavePathLabel
            // 
            this.jsonSavePathLabel.AutoSize = true;
            this.jsonSavePathLabel.Location = new System.Drawing.Point(444, 429);
            this.jsonSavePathLabel.Name = "jsonSavePathLabel";
            this.jsonSavePathLabel.Size = new System.Drawing.Size(51, 13);
            this.jsonSavePathLabel.TabIndex = 7;
            this.jsonSavePathLabel.Text = "File Path:";
            // 
            // backFromOptionsButton
            // 
            this.backFromOptionsButton.Location = new System.Drawing.Point(1121, 766);
            this.backFromOptionsButton.Name = "backFromOptionsButton";
            this.backFromOptionsButton.Size = new System.Drawing.Size(75, 23);
            this.backFromOptionsButton.TabIndex = 6;
            this.backFromOptionsButton.Text = "Back";
            this.backFromOptionsButton.UseVisualStyleBackColor = true;
            this.backFromOptionsButton.Click += new System.EventHandler(this.backFromOptionsButton_Click);
            // 
            // saveFilePath
            // 
            this.saveFilePath.Location = new System.Drawing.Point(503, 426);
            this.saveFilePath.Name = "saveFilePath";
            this.saveFilePath.Size = new System.Drawing.Size(202, 20);
            this.saveFilePath.TabIndex = 4;
            // 
            // saveJSON
            // 
            this.saveJSON.Location = new System.Drawing.Point(565, 452);
            this.saveJSON.Name = "saveJSON";
            this.saveJSON.Size = new System.Drawing.Size(75, 23);
            this.saveJSON.TabIndex = 3;
            this.saveJSON.Text = "Save";
            this.saveJSON.UseVisualStyleBackColor = true;
            this.saveJSON.Click += new System.EventHandler(this.saveJSON_Click);
            // 
            // filePathBox
            // 
            this.filePathBox.Location = new System.Drawing.Point(503, 272);
            this.filePathBox.Name = "filePathBox";
            this.filePathBox.Size = new System.Drawing.Size(202, 20);
            this.filePathBox.TabIndex = 1;
            // 
            // confirmFilePath
            // 
            this.confirmFilePath.Location = new System.Drawing.Point(565, 298);
            this.confirmFilePath.Name = "confirmFilePath";
            this.confirmFilePath.Size = new System.Drawing.Size(75, 23);
            this.confirmFilePath.TabIndex = 0;
            this.confirmFilePath.Text = "Load";
            this.confirmFilePath.UseVisualStyleBackColor = true;
            this.confirmFilePath.Click += new System.EventHandler(this.confirmFilePath_Click);
            // 
            // jsonLoadCheck
            // 
            this.jsonLoadCheck.Location = new System.Drawing.Point(707, 272);
            this.jsonLoadCheck.Name = "jsonLoadCheck";
            this.jsonLoadCheck.Size = new System.Drawing.Size(16, 16);
            this.jsonLoadCheck.TabIndex = 11;
            this.jsonLoadCheck.TabStop = false;
            // 
            // jsonSaveCheck
            // 
            this.jsonSaveCheck.Location = new System.Drawing.Point(707, 426);
            this.jsonSaveCheck.Name = "jsonSaveCheck";
            this.jsonSaveCheck.Size = new System.Drawing.Size(16, 16);
            this.jsonSaveCheck.TabIndex = 12;
            this.jsonSaveCheck.TabStop = false;
            // 
            // pendulumTimer
            // 
            this.pendulumTimer.Interval = 10;
            this.pendulumTimer.Tick += new System.EventHandler(this.pendulumTimer_Tick);
            // 
            // OutputWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260,801);
            this.Controls.Add(this.MenuPanel);
            this.Controls.Add(this.SimulationPanel);
            this.Controls.Add(this.jsonPanel);
            this.Name = "OutputWindow";
            this.Text = "Double Pendulum Simulator";
            this.SimulationPanel.ResumeLayout(false);
            this.SimulationPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pendulumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Length1Scroll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Length2Scroll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Angle1Scroll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Angle2Scroll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m1Check)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m2Check)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.penNumCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetAngleCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trailColorCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgColorCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rootColorCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.middleColorCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tailColorCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineColorCheck)).EndInit();
            this.MenuPanel.ResumeLayout(false);
            this.MenuPanel.PerformLayout();
            this.jsonPanel.ResumeLayout(false);
            this.jsonPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jsonLoadCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.jsonSaveCheck)).EndInit();
            this.ResumeLayout(false);

        }


        
        private void enterPendulum_Click(object sender, EventArgs e)
        {
            SimulationPanel.Visible = true;
            MenuPanel.Visible = false;
        }

        private void Length1Scroll_Scroll(object sender, EventArgs e)
        {
            length1Label.Text = Convert.ToString(Length1Scroll.Value);
            updateConstants(true);
        }

        private void Length2Scroll_Scroll(object sender, EventArgs e)
        {
            length2Label.Text = Convert.ToString(Length2Scroll.Value);
            updateConstants(true);
        }

        private void Angle1Scroll_Scroll(object sender, EventArgs e)
        {
            angle1Label.Text = Convert.ToString(Angle1Scroll.Value);
            updateConstants(true);
        }

        private void Angle2Scroll_Scroll(object sender, EventArgs e)
        {
            angle2Label.Text = Convert.ToString(Angle2Scroll.Value);
            updateConstants(true);
        }
        private void gTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {

                if (Convert.ToDouble(gTextBox.Text) > 100 || Convert.ToDouble(gTextBox.Text) < 0)
                {
                    gCheck.Image = redCross; 
                }
                else
                {
                    updateConstants(true);
                    gCheck.Image = greenTick; 

                }
            }
            catch
            {
                gCheck.Image = redCross; 
            }
            ;
        }

        private void Mass1TextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDouble(Mass1TextBox.Text) > 100 || Convert.ToDouble(Mass1TextBox.Text) < 0)
                {
                    m1Check.Image = redCross;

                }
                else
                {
                    updateConstants(true);
                    m1Check.Image = greenTick; 

                }
            }
            catch
            {
                m1Check.Image = redCross; 

            }
            ;

        }

        private void Mass2TextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {

                if (Convert.ToDouble(Mass2TextBox.Text) > 100 || Convert.ToDouble(Mass2TextBox.Text) < 0)
                {
                    m2Check.Image = redCross;
                }
                else
                {
                    updateConstants(true);
                    m2Check.Image = greenTick; 

                }
            }
            catch
            {
                m2Check.Image = redCross; 

            }
            ;

        }

        private void updateConstants(bool check)
        {

            ResetPendulums(check);
            UpdateImage();
        } 

        private void StartStopButton_Click(object sender, EventArgs e)
        {
            if (StartStopButton.Text == "START")
            {


                pendulumTimer.Enabled = true;
                StartStopButton.Text = "STOP";
            }
            else
            {
                StartStopButton.Text = "START";
                pendulumTimer.Enabled = false;
            }

        }

        private int Parse(string number)
        {
            if (int.TryParse(number, out int result))
            {
                return result;
            }
            return 0; 
        }
        private void resetButton_Click(object sender, EventArgs e)
        {

            updateLength1(50);
            updateLength2(50);
            updateAngle1(0);
            updateAngle2(0);
            gTextBox.Text = "9.81";
            Mass1TextBox.Text = "10";
            Mass2TextBox.Text = "10";
            updateConstants(true);
            showPendulums = true;
            showTrailChecked = false;
            showTrail.Checked = false;
            trailFade.Visible = false;
            SetInitialValidationStatus(); 

        }

        private void randomiseButton_Click(object sender, EventArgs e)
        {
            if (randomiseSeed.Text == "")
            {
                Random rnd = new Random();
                updateLength1(rnd.Next(30, 100));
                updateLength2(rnd.Next(30, 100));
                updateAngle1(rnd.Next(-180, 180));
                updateAngle2(rnd.Next(-180, 180));
                updateConstants(true);
            }
            else
            {
                int[] numbers = applyHashing(randomiseSeed.Text);

                updateLength1(Math.Max(Math.Min(numbers[0],Length1Scroll.Maximum), Length1Scroll.Minimum));
                updateLength2(Math.Max(Math.Min(numbers[1], Length2Scroll.Maximum), Length2Scroll.Minimum));
                updateAngle1(Math.Max(Math.Min(numbers[2], Angle1Scroll.Maximum), Angle1Scroll.Minimum));
                updateAngle2(Math.Max(Math.Min(numbers[3], Angle2Scroll.Maximum), Angle2Scroll.Minimum));

                updateConstants(true);
            }
        }

        private int[] applyHashing(string word)
        {
            int[] numbers = { 0, 0, 0, 0 };
            int index = 0;
            foreach (char c in word)
            {
                numbers[0] += c * c * index;
                numbers[1] += c * index * index;
                numbers[2] += (int)Math.Ceiling((double)c * c / (index + 1));
                numbers[3] += c * c * c;
                index++;
            }
            numbers[0] = (numbers[0] % 100) + 1;
            numbers[1] = (numbers[1] % 100) + 1;
            numbers[2] = numbers[2] % 361;
            numbers[3] = numbers[3] % 361;
            numbers[2] -= 180;
            numbers[3] -= 180;
            return numbers;
        }

        private void updateLength1(int number)
        {
            Length1Scroll.Value = number;
            length1Label.Text = Convert.ToString(number);
        }
        private void updateLength2(int number)
        {
            Length2Scroll.Value = number;
            length2Label.Text = Convert.ToString(number);
        }
        private void updateAngle1(int number)
        {
            Angle1Scroll.Value = number;
            angle1Label.Text = Convert.ToString(number);
        }
        private void updateAngle2(int number)
        {
            Angle2Scroll.Value = number;
            angle2Label.Text = Convert.ToString(number);
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            SimulationPanel.Visible = false;
            MenuPanel.Visible = true;
            jsonPanel.Visible = false; 
            pendulumTimer.Enabled = false; 
            StartStopButton.Text = "START";
        }

        private void pendulumTimer_Tick(object sender, EventArgs e)
        {
            if (pendulums != null)
            {
                foreach (Pendulum newpendulum in pendulums)
                {
                    newpendulum.calculateNewAngle();
                    newpendulum.SetNewCoords();
                }
            }
            UpdateImage();

        }

        private void showTrail_CheckedChanged(object sender, EventArgs e)
        {
            showTrailChecked = !showTrailChecked;
            trailFade.Visible = showTrailChecked;
            if (showTrailChecked == false)
            {
                trailFade.Checked = false;
            }

        }
        private void trailFade_CheckedChanged(object sender, EventArgs e)
        {
            showTrailFade = !showTrailFade;
        }

        private void reRunButton_Click(object sender, EventArgs e)
        {
            bool wasRunning = pendulumTimer.Enabled;
            pendulums = null;
            StartStopButton.Text = "START";
            pendulumTimer.Enabled = false; 
            ResetPendulums(true); 
            if (wasRunning)
            {
                StartStopButton_Click(sender, e);
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (!multi && pendulums != null && pendulums.Count > 0)
            {
                int clickX = e.X;
                int clickY = e.Y;

                double tailDistance = CalculateDistance(clickX, clickY, pendulums[0].GetTail().GetX(), pendulums[0].GetTail().GetY());
                double middleDistance = CalculateDistance(clickX, clickY, pendulums[0].GetMiddle().GetX(), pendulums[0].GetMiddle().GetY());
                int threshold = 20;

                if (tailDistance <= threshold)
                {
                    isPointDragged = "T";
                    pendulumTimer.Enabled = false; 
                    StartStopButton.Text = "START";
                }
                if (middleDistance <= threshold)
                {
                    isPointDragged = "M";
                    pendulumTimer.Enabled = false; 
                    StartStopButton.Text = "START";
                }
            }
        }
        
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!multi && pendulums != null && pendulums.Count > 0)
            {
                if (isPointDragged == "T")
                {
                    Node newTail = pendulums[0].GetTail();
                    double changeX = e.X - pendulums[0].GetMiddle().GetX();
                    double changeY = e.Y - pendulums[0].GetMiddle().GetY();

                    if ((Math.Pow(changeX, 2) + Math.Pow(changeY, 2)) > Math.Pow(pendulums[0].GetL2(), 2))
                    {
                        while ((Math.Pow(changeX, 2) + Math.Pow(changeY, 2)) > Math.Pow(pendulums[0].GetL2(), 2))
                        {
                            changeX *= 0.99;
                            changeY *= 0.99;
                        }
                    }
                    else
                    {
                        if (changeX != 0 || changeY != 0) 
                        {
                            while ((Math.Pow(changeX, 2) + Math.Pow(changeY, 2)) < Math.Pow(pendulums[0].GetL2(), 2))
                            {
                                changeX /= 0.99;
                                changeY /= 0.99;
                            }
                        }
                    }

                    int newX = (int)(pendulums[0].GetMiddle().GetX() + changeX);
                    int newY = (int)(pendulums[0].GetMiddle().GetY() + changeY);

                    newTail.SetX(newX);
                    newTail.SetY(newY);
                    pendulums[0].SetTail(newTail);
                    UpdateImage();
                    UpdateAngles();
                }

                if (isPointDragged == "M")
                {

                    Node newMiddle = pendulums[0].GetMiddle();
                    Node newTail = pendulums[0].GetTail();
                    int tailMiddleX = newTail.GetX() - newMiddle.GetX();
                    int tailMiddleY = newTail.GetY() - newMiddle.GetY();
                    double changeX = e.X - pendulums[0].GetRoot().GetX();
                    double changeY = e.Y - pendulums[0].GetRoot().GetY();
                    if ((Math.Pow(changeX, 2) + Math.Pow(changeY, 2)) > Math.Pow(pendulums[0].GetL1(), 2))
                    {
                        while ((Math.Pow(changeX, 2) + Math.Pow(changeY, 2)) > Math.Pow(pendulums[0].GetL1(), 2))
                        {
                            changeX *= 0.99;
                            changeY *= 0.99;
                        }
                    }
                    else
                    {
                        if (changeX != 0 || changeY != 0) 
                        {
                            while ((Math.Pow(changeX, 2) + Math.Pow(changeY, 2)) < Math.Pow(pendulums[0].GetL1(), 2))
                            {
                                changeX /= 0.99;
                                changeY /= 0.99;
                            }
                        }
                    }

                    int newX = (int)(pendulums[0].GetRoot().GetX() + changeX);
                    int newY = (int)(pendulums[0].GetRoot().GetY() + changeY);
                    newMiddle.SetX(newX);
                    newMiddle.SetY(newY);
                    newTail.SetX(newX + tailMiddleX);
                    newTail.SetY(newY + tailMiddleY);
                    pendulums[0].SetMiddle(newMiddle);
                    pendulums[0].SetTail(newTail);
                    UpdateImage();
                    UpdateAngles();
                }
            }

        }
        private void UpdateImage()
        {

            bool first = true;
            if (multi)
            {
                if (pendulums != null)
                {
                    if (pendulums.Count > 0)
                    {
                        pendulumBox.Image = pendulums[0].GetImage(showTrailChecked, showTrailFade, first, showPendulums, pendulumBox.Image);
                        first = false;
                    }
                    for (int i = 1; i < pendulums.Count; i++)
                    {
                        pendulumBox.Image = pendulums[i].GetImage(showTrailChecked, showTrailFade, first, showPendulums, pendulumBox.Image);
                    }
                }
                if (pendulums == null)
                {
                    ResetPendulums(true);
                }
            }
            else
            {
                try
                {
                    if (pendulums != null && pendulums.Count > 0)
                    {
                        first = true;
                        pendulumBox.Image = pendulums[0].GetImage(showTrailChecked, showTrailFade, first, showPendulums, pendulumBox.Image);
                    }
                }
                catch { }
                ;


            }
        }
        private void UpdateAngles()
        {
            if (!multi && pendulums != null && pendulums.Count > 0)
            {
                double xChange = pendulums[0].GetMiddle().GetX() - pendulums[0].GetRoot().GetX();
                double yChange = pendulums[0].GetMiddle().GetY() - pendulums[0].GetRoot().GetY();


                double xChange2 = pendulums[0].GetTail().GetX() - pendulums[0].GetMiddle().GetX();
                double yChange2 = pendulums[0].GetTail().GetY() - pendulums[0].GetMiddle().GetY();


                int angle1 = (int)(Math.Atan2(xChange, yChange) * 180 / Math.PI);
                int angle2 = (int)(Math.Atan2(xChange2, yChange2) * 180 / Math.PI);

                angle1 = Math.Max(-180, Math.Min(180, angle1));
                angle2 = Math.Max(-180, Math.Min(180, angle2));

                updateAngle1(angle1);
                updateAngle2(angle2);
                updateConstants(true);
            }
        }

        
        
        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            isPointDragged = "N";
        }
        private double CalculateDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        private void multiPendulum_Click(object sender, EventArgs e)
        {
            multi = !multi;

            penNumCheck.Visible = !penNumCheck.Visible;
            noPendulums.Visible = !noPendulums.Visible;
            penNumLabel.Visible = !penNumLabel.Visible;

            offsetAngleCheck.Visible = !offsetAngleCheck.Visible;
            offsetAngle.Visible = !offsetAngle.Visible;
            offsetAngleLabel.Visible = !offsetAngleLabel.Visible;

            if (multi)
            {
                ResetPendulums(true);
                UpdateImage();
                noPendulums.Visible = true;
                multiPendulum.Text = "Disable Multiple Pendulums";
            }
            else
            {
                ResetPendulums(true);
                UpdateImage();
                noPendulums.Visible = false;

                multiPendulum.Text = "Enable Multiple Pendulums";

            }
        }
        private void ResetPendulums(bool check)
        {
            double g = 9.81;
            int m1 = 10;
            int m2 = 10;
            if (double.TryParse(gTextBox.Text, out double g_parsed)) g = g_parsed;
            if (int.TryParse(Mass1TextBox.Text, out int m1_parsed)) m1 = m1_parsed;
            if (int.TryParse(Mass2TextBox.Text, out int m2_parsed)) m2 = m2_parsed;
            if (check)
            {
                checkColour(customiseColour.Text, customiseColour.Name);
                checkColour(CustomiseBackgroundColor.Text, CustomiseBackgroundColor.Name);
                checkColour(rootColorBox.Text, rootColorBox.Name);
                checkColour(middleColorBox.Text, middleColorBox.Name);
                checkColour(tailColorBox.Text, tailColorBox.Name);
                checkColour(lineColorBox.Text, lineColorBox.Name);
            }

            PendulumConstants p = new PendulumConstants(g * 100, m1, m2, (int)(Parse(length1Label.Text) * 1.3), (int)(Parse(length2Label.Text) * 1.3), this.rootColor, this.middleColor, this.tailColor, this.lineColor, trailColor, pendulumTimer.Interval);

            pendulums = new List<Pendulum>();
            if (multi)
            {
                for (int i = 0; i < pendulumNumber; i++)
                {
                    Pendulum first = new Pendulum(pendulumBox.Width, pendulumBox.Height, 10, p, ((Parse(angle1Label.Text) + i * offset) * Math.PI) / 180, ((Parse(angle2Label.Text) + i * offset) * Math.PI) / 180, pendulumBox.BackColor);
                    pendulums.Add(first);

                }

            }
            else
            {
                Pendulum first = new Pendulum(pendulumBox.Width, pendulumBox.Height, 10, p, ((Parse(angle1Label.Text)) * Math.PI) / 180, ((Parse(angle2Label.Text)) * Math.PI) / 180, pendulumBox.BackColor);
                pendulums.Add(first);


            }
        }


        private void noPendulums_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (noPendulums.Text == "")
                {
                    pendulumNumber = 1;
                    penNumCheck.Image = greenTick;

                }
                else if (Convert.ToInt64(noPendulums.Text) <= 360 && Convert.ToInt64(noPendulums.Text) > 0)
                {
                    pendulumNumber = Convert.ToInt32(noPendulums.Text);
                    penNumCheck.Image = greenTick; 


                }
                else
                {
                    penNumCheck.Image = redCross; 
                }
            }
            catch
            {
                penNumCheck.Image = redCross; 
            }

            ResetPendulums(true);
            UpdateImage();
        }
        private void offsetAngle_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (offsetAngle.Text == "")
                {
                    offset = 0.5;
                    offsetAngleCheck.Image = redCross;
                }
                else if (Convert.ToDouble(offsetAngle.Text) <= 1 && Convert.ToDouble(offsetAngle.Text) >= 0)
                {
                    offset = Math.Round(Convert.ToDouble(offsetAngle.Text),1);
                    if(offset == 0)
                    {
                        offset = 0.1;
                    }
                    
                    offsetAngleCheck.Image = greenTick;
                }
                else
                {
                    offset = 0.5;
                    offsetAngleCheck.Image = redCross;
                }
            }
            catch
            {
                offsetAngleCheck.Image = redCross;
            }
            ResetPendulums(true);
            UpdateImage();
        }
        private void hidePendulums_CheckedChanged(object sender, EventArgs e)
        {
            showPendulums = !showPendulums;
        }
        private void checkColour(string Text, string Name) 
        {
            string[] colorNames = typeof(Color).GetProperties()
                                                      .Where(p => p.PropertyType == typeof(Color)) 
                                                      .Select(p => p.Name)
                                                      .ToArray();
            string pattern = $"^({string.Join("|", colorNames)})$";
            Regex colorRegex = new Regex(pattern, RegexOptions.IgnoreCase);

            if (string.IsNullOrWhiteSpace(Text))
            {
                switch (Name)
                {
                    case "customiseColour": trailColorCheck.Image = redCross; break;
                    case "CustomiseBackgroundColor": bgColorCheck.Image = redCross; break;
                    case "rootColorBox": rootColorCheck.Image = redCross; break;
                    case "middleColorBox": middleColorCheck.Image = redCross; break;
                    case "tailColorBox": tailColorCheck.Image = redCross; break;
                    case "lineColorBox": lineColorCheck.Image = redCross; break;
                }
                return;
            }

            if (colorRegex.IsMatch(Text))
            {
                Color newColour = Color.FromName(Text);
                if (Name == "customiseColour")
                {
                    trailColorCheck.Image = greenTick; 
                    trailColor = newColour;
                    updateConstants(false);
                    if (pendulums == null) return;
                    foreach (Pendulum p in pendulums)
                    {
                        p.setTrailColor(newColour);
                    }
                }
                else if (Name == "CustomiseBackgroundColor")
                {
                    bgColorCheck.Image = greenTick; 
                    pendulumBox.BackColor = newColour;
                    updateConstants(false);
                    if (pendulums == null) return;
                    foreach (Pendulum p in pendulums)
                    {
                        p.setBackColour(newColour);
                    }
                }
                else if (Name == "rootColorBox")
                {
                    rootColorCheck.Image = greenTick; 
                    this.rootColor = newColour;
                    updateConstants(false);
                }
                else if (Name == "middleColorBox")
                {
                    middleColorCheck.Image = greenTick; 
                    this.middleColor = newColour;
                    updateConstants(false);
                }
                else if (Name == "tailColorBox")
                {
                    tailColorCheck.Image = greenTick; 
                    this.tailColor = newColour;
                    updateConstants(false);
                }
                else if (Name == "lineColorBox")
                {
                    lineColorCheck.Image = greenTick; 
                    this.lineColor = newColour;
                    updateConstants(false);
                }
                

            }
            else
            {

                if (Name == "customiseColour")
                {
                    trailColorCheck.Image = redCross; 
                }
                else if (Name == "CustomiseBackgroundColor")
                {
                    bgColorCheck.Image = redCross; 
                }
                else if (Name == "rootColorBox")
                {
                    rootColorCheck.Image = redCross; 
                }
                else if (Name == "middleColorBox")
                {
                    middleColorCheck.Image = redCross; 
                }
                else if (Name == "tailColorBox")
                {
                    tailColorCheck.Image = redCross; 
                }
                else if (Name == "lineColorBox")
                {
                    lineColorCheck.Image = redCross; 
                }
            }
        }
        private void customiseColour_TextChanged(object sender, EventArgs e)
        {

            System.Windows.Forms.TextBox customiseColor = (System.Windows.Forms.TextBox)sender;
            checkColour(customiseColor.Text, customiseColor.Name); 

        }

        private void optionsButton_Click(object sender, EventArgs e)
        {
            jsonPanel.Visible = true;
            MenuPanel.Visible = false;
        }

        private void confirmFilePath_Click(object sender, EventArgs e)
        {

            if (File.Exists(filePathBox.Text + ".json"))
            {
                try
                {

                    SettingsManager settingsManager = new SettingsManager(filePathBox.Text);

                    Settings loadedSettings = settingsManager.LoadSettings();
                    updateLength1(Convert.ToInt32(loadedSettings.GetLength1()));
                    updateLength2(Convert.ToInt32(loadedSettings.GetLength2()));
                    updateAngle1(Convert.ToInt32(loadedSettings.GetAngle1()));
                    updateAngle2(Convert.ToInt32(loadedSettings.GetAngle2()));
                    multi = loadedSettings.GetMulti();
                    pendulumNumber = loadedSettings.getPenNum();
                    Mass1TextBox.Text = Convert.ToString(loadedSettings.GetMass1());
                    Mass2TextBox.Text = Convert.ToString(loadedSettings.GetMass2());
                    gTextBox.Text = Convert.ToString(loadedSettings.GetGravitationalConstant());
                    if (multi)
                    {
                        multiPendulum.Text = "Disable Multiple Pendulums";
                        noPendulums.Visible = true;
                        noPendulums.Text = Convert.ToString(pendulumNumber);
                    }
                    else
                    {
                        multiPendulum.Text = "Enable Multiple Pendulums";
                        noPendulums.Visible = false;
                        noPendulums.Text = "1";
                    }

                    updateConstants(true); 
                    jsonPanel.Visible = false;
                    SimulationPanel.Visible = true;
                    jsonLoadCheck.Image = greenTick; 
                }
                catch
                {
                    jsonLoadCheck.Image = redCross; 
                }

            }
            else
            {
                jsonLoadCheck.Image = redCross; 
            }
        }
        private void saveJSON_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = saveFilePath.Text;
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    jsonSaveCheck.Image = redCross; 
                    return;
                }
                SettingsManager settingsManager = new SettingsManager(filePath);

                Settings settingsToSave = new Settings(Convert.ToDouble(length1Label.Text), Convert.ToDouble(length2Label.Text), Convert.ToDouble(angle1Label.Text), Convert.ToDouble(angle2Label.Text), multi, Parse(noPendulums.Text), Convert.ToDouble(Mass1TextBox.Text), Convert.ToDouble(Mass2TextBox.Text), Convert.ToDouble(gTextBox.Text));
                settingsManager.SaveSettings(settingsToSave);
                jsonSaveCheck.Image = greenTick; 
            }
            catch (Exception exception)
            {
                jsonSaveCheck.Image = redCross; 
            }
        }
        private void backFromOptionsButton_Click(object sender, EventArgs e)
        {
            SimulationPanel.Visible = false;
            MenuPanel.Visible = true;
            jsonPanel.Visible = false;
        }

        private void MenuPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}