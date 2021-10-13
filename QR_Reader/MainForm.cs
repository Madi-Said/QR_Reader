using System;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;

namespace QR_Reader
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Button startBtn;
        private Label camLbl;
        private PictureBox captureBox;
        private ComboBox cboDevice;
        private TextBox QR_Text;
        private FilterInfoCollection filterInfoCollection;
        private Timer watch;
        private VideoCaptureDevice captureDevice;
        public MainForm()
        {
            InitializeComponent();
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.startBtn = new System.Windows.Forms.Button();
            this.camLbl = new System.Windows.Forms.Label();
            this.captureBox = new System.Windows.Forms.PictureBox();
            this.cboDevice = new System.Windows.Forms.ComboBox();
            this.QR_Text = new System.Windows.Forms.TextBox();
            this.watch = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.captureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(343, 56);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(75, 23);
            this.startBtn.TabIndex = 0;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // camLbl
            // 
            this.camLbl.AutoSize = true;
            this.camLbl.Location = new System.Drawing.Point(65, 28);
            this.camLbl.Name = "camLbl";
            this.camLbl.Size = new System.Drawing.Size(49, 13);
            this.camLbl.TabIndex = 1;
            this.camLbl.Text = "Caméra :";
            // 
            // captureBox
            // 
            this.captureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.captureBox.Location = new System.Drawing.Point(12, 57);
            this.captureBox.Name = "captureBox";
            this.captureBox.Size = new System.Drawing.Size(325, 285);
            this.captureBox.TabIndex = 2;
            this.captureBox.TabStop = false;
            // 
            // cboDevice
            // 
            this.cboDevice.FormattingEnabled = true;
            this.cboDevice.Location = new System.Drawing.Point(113, 25);
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(224, 21);
            this.cboDevice.TabIndex = 3;
            // 
            // QR_Text
            // 
            this.QR_Text.BackColor = System.Drawing.Color.White;
            this.QR_Text.Location = new System.Drawing.Point(343, 86);
            this.QR_Text.Multiline = true;
            this.QR_Text.Name = "QR_Text";
            this.QR_Text.Size = new System.Drawing.Size(273, 256);
            this.QR_Text.TabIndex = 4;
            // 
            // watch
            // 
            this.watch.Interval = 1000;
            this.watch.Tick += new System.EventHandler(this.Watch_Tick);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(623, 349);
            this.Controls.Add(this.QR_Text);
            this.Controls.Add(this.cboDevice);
            this.Controls.Add(this.captureBox);
            this.Controls.Add(this.camLbl);
            this.Controls.Add(this.startBtn);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Lire un code QR via la Webcam";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.captureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
                cboDevice.Items.Add(filterInfo.Name);
            cboDevice.SelectedIndex = 0;
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            captureDevice = new VideoCaptureDevice(filterInfoCollection[cboDevice.SelectedIndex].MonikerString);
            captureDevice.NewFrame += CaptureDevice_NewFrame;
            captureDevice.Start();
            watch.Start();
        }

        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            captureBox.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (captureDevice.IsRunning)
                captureDevice.Stop();
        }

        private void Watch_Tick(object sender, EventArgs e)
        {
            if (captureBox.Image != null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode((Bitmap)captureBox.Image);
                if (result != null)
                {
                    QR_Text.Text = result.ToString();
                    watch.Stop();
                    if (captureDevice.IsRunning)
                        captureDevice.Stop();
                }
            }
        }
    }
}