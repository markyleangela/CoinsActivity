using AForge.Video;
using AForge.Video.DirectShow;

namespace DIPAct
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed;
        Bitmap imageB, imageA; //image A is the background
        Bitmap resultImage;


        VideoCaptureDevice webcam;
    
        Color green = Color.FromArgb(0, 0, 255); // Define the target green
      

        public Form1()
        {
            InitializeComponent();
        }

        private void StartWebcam()
        {
            // Initialize the webcam and start capturing
            var devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            webcam = new VideoCaptureDevice(devices[0].MonikerString);
            webcam.NewFrame += new NewFrameEventHandler(ProcessFrame);
            webcam.Start();
        }

        private void ProcessFrame(object sender, NewFrameEventArgs eventArgs)
        {
           
            Bitmap imageB = (Bitmap)eventArgs.Frame.Clone();
            Bitmap resultImage = new Bitmap(imageB.Width, imageB.Height);
            int threshold = 5;
           
            for (int x = 0; x < imageB.Width; x++)
            {
                for (int y = 0; y < imageB.Height; y++)
                {
                    Color pixel = imageB.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int greenGrey = (green.R + green.G + green.B) / 3;
                
                    Color backpixel = imageA.GetPixel(x, y);
                    int subtractvalue = Math.Abs(grey - greenGrey);
                    
                    if (subtractvalue  < threshold)
                        resultImage.SetPixel(x, y,backpixel);
                    else
                        resultImage.SetPixel(x, y, pixel);
                }
            }

         
            pictureBox3.Image = resultImage;
        }

        private void StopWebcam()
        {
            if (webcam != null && webcam.IsRunning)
            {
                webcam.SignalToStop();
                webcam.WaitForStop();
            }
        }


        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }

        private void basicCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, y, pixel);
                }
            }

            pictureBox2.Image = processed;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }




        private void saveFileDialog1_FileOk_2(object sender, System.ComponentModel.CancelEventArgs e)
        {
            processed.Save(saveFileDialog1.FileName);
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            int ave;
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    ave = (pixel.R + pixel.G + pixel.B) / 3;
                    Color grey = Color.FromArgb(ave, ave, ave);
                    processed.SetPixel(x, y, grey);

                }
            }

            pictureBox2.Image = processed;
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;

            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);

                    Color inversion = Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                    processed.SetPixel(x, y, inversion);

                }
            }

            pictureBox2.Image = processed;
        }


        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicDIP.Hist(ref loaded, ref processed);
            pictureBox2.Image = processed;

        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            int red, green, blue;
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    red = (int)(0.393 * pixel.R + 0.769 * pixel.G + 0.189 * pixel.B);
                    green = (int)(0.349 * pixel.R + 0.686 * pixel.G + 0.168 * pixel.B);
                    blue = (int)(0.272 * pixel.R + 0.534 * pixel.G + 0.131 * pixel.B);

                    Color sepia = Color.FromArgb(Math.Min(255, red), Math.Min(255, green), Math.Min(255, blue));
                    processed.SetPixel(x, y, sepia);

                }
            }

            pictureBox2.Image = processed;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            imageB = new Bitmap(openFileDialog2.FileName);
            pictureBox1.Image = imageB;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
        }

        private void openFileDialog3_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            imageA = new Bitmap(openFileDialog3.FileName);
            pictureBox2.Image = imageA;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Color mygreen = Color.FromArgb(0, 0, 255);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3; //turn it into greyscale
            int threshold = 5;
            resultImage = new Bitmap(imageA.Width, imageA.Height);
      
            for (int x = 0; x < imageB.Width; x++)
            {
                for (int y = 0; y < imageB.Height; y++)
                {
                    Color pixel = imageB.GetPixel(x, y);
                    Color backpixel = imageA.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int subtractvalue = Math.Abs(grey - greygreen);
                    if (subtractvalue < threshold)
                    {
                        resultImage.SetPixel(x, y, backpixel);
                    }
                    else
                    {
                        resultImage.SetPixel(x, y, pixel);
                    }
                }
            }
            pictureBox3.Image = resultImage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            StartWebcam();
        }
    }
}
