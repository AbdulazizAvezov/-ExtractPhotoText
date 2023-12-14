using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using Patagames.Ocr;

namespace WepCameraTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        VideoCaptureDevice VideoCapture;
        FilterInfoCollection FilterInfo;
        void StartCamera()
        {
            try
            {
                FilterInfo = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                VideoCapture = new VideoCaptureDevice(FilterInfo[0].MonikerString);
                VideoCapture.NewFrame += new NewFrameEventHandler(Camera_On);

                // Kamerani ishga tushiramiz
                VideoCapture.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Camera_On(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            VideoCapture.Stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = pictureBox1.Image;
            string filename = @"C:\Users\ABDULAZIZ1308\OneDrive\Рабочий стол\Images\" + textBox1.Text + ".jpg";
            var bitmap = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            pictureBox2.DrawToBitmap(bitmap, pictureBox2.ClientRectangle);
            System.Drawing.Imaging.ImageFormat imageFormat = null;
            imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
            bitmap.Save(filename, imageFormat);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files |*.jpg; *.jpeg; *.png; *.gif; *.bmp";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Bu qisimda faqat birinchi faylni olish kerak
                    string selectedFile = openFileDialog.FileNames[0];
                    pictureBox2.ImageLocation = selectedFile;

                    using (var objOcr = OcrApi.Create())
                    {
                        objOcr.Init(Patagames.Ocr.Enums.Languages.English);

                        string plaintext = objOcr.GetTextFromImage(selectedFile);

                        textBox2.Text = plaintext;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartCamera();
        }
    }
}
