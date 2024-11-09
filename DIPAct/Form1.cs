
using WebCamLib;
using ImageProcess2;

namespace DIPAct
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed;
        Bitmap imageB, imageA; //image A is the background
        Bitmap resultImage;
        Bitmap b;
        Device[] devices;



        Color green = Color.FromArgb(0, 0, 255); //  the target green


        public Form1()
        {
            InitializeComponent();
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


        private void Form1_Load(object sender, EventArgs e)
        {
            devices = DeviceManager.GetAllDevices();
        }

        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {
            devices[0].ShowWindow(pictureBox3);
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            devices[0].Stop();
        }

        private void greyscaleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //get 1 frame
            IDataObject data;
            Image bmap;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            Bitmap b = new Bitmap(bmap);

            //processed = new Bitmap(b.Width, b.Height);
            //Color pixel;
            //int ave;
            //for (int x = 0; x < b.Width; x++)
            //{
            //    for (int y = 0; y < b.Height; y++)
            //    {
            //        pixel = b.GetPixel(x, y);
            //        ave = (pixel.R + pixel.G + pixel.B) / 3;
            //        Color grey = Color.FromArgb(ave, ave, ave);
            //        processed.SetPixel(x, y, grey);

            //    }
            //}

            //pictureBox2.Image = processed;
            BitmapFilter.GrayScale(b);
            pictureBox2.Image = b;
        }

        private void colorInversionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            timer2.Enabled = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            IDataObject data;
            Image bmap;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            Bitmap b = new Bitmap(bmap);
            processed = new Bitmap(b.Width, b.Height);
            Color pixel;

            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    pixel = b.GetPixel(x, y);

                    Color inversion = Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                    processed.SetPixel(x, y, inversion);

                }
            }

            pictureBox2.Image = processed;
        }

        private void sepiaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            timer3.Enabled = true;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {

            IDataObject data;
            Image bmap;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            Bitmap b = new Bitmap(bmap);

            processed = new Bitmap(b.Width, b.Height);
            Color pixel;
            int red, green, blue;
            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    pixel = b.GetPixel(x, y);
                    red = (int)(0.393 * pixel.R + 0.769 * pixel.G + 0.189 * pixel.B);
                    green = (int)(0.349 * pixel.R + 0.686 * pixel.G + 0.168 * pixel.B);
                    blue = (int)(0.272 * pixel.R + 0.534 * pixel.G + 0.131 * pixel.B);

                    Color sepia = Color.FromArgb(Math.Min(255, red), Math.Min(255, green), Math.Min(255, blue));
                    processed.SetPixel(x, y, sepia);

                }
            }

            pictureBox2.Image = processed;
        }

        private void basicCopyToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            timer4.Enabled = true;

        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            IDataObject data;
            Image bmap;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            Bitmap b = new Bitmap(bmap);

            processed = new Bitmap(b.Width, b.Height);
            Color pixel;
            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    pixel = b.GetPixel(x, y);
                    processed.SetPixel(x, y, pixel);
                }
            }

            pictureBox2.Image = processed;
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            IDataObject data;
            Image bmap;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            Bitmap b = new Bitmap(bmap);

            Color mygreen = Color.FromArgb(107, 137, 85);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3; //turn it into greyscale
            int threshold = 5;
            resultImage = new Bitmap(imageA.Width, imageA.Height);

            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    Color pixel = b.GetPixel(x, y);
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
            pictureBox1.Image = resultImage;
        }

        private void subtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer5.Enabled = true;
        }

        private void smoothToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.Smooth(processed, 4);
            pictureBox2.Image = processed;

        }

        private void gaussianBlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.GaussianBlur(processed, 4);
            pictureBox2.Image = processed;
        }

        private void sharpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.Sharpen(processed, 11);
            pictureBox2.Image = processed;
        }

        private void meanRemovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.MeanRemoval(processed, 9);
            pictureBox2.Image = processed;
        }

        private void embossingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.EmbossLaplacian(processed);
            pictureBox2.Image = processed;
        }

        private void embossHorizontalVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.EmbossHorzVert(processed);
            pictureBox2.Image = processed;
        }

        private void embossAllDirectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.EmbossAllDirection(processed);
            pictureBox2.Image = processed;
        }

        private void lossyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.Lossy(processed);
            pictureBox2.Image = processed;
        }

        private void embossVerticalOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.EmbossVertical(processed);
            pictureBox2.Image = processed;
        }

        private void embossHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.EmbossHorizontal(processed);
            pictureBox2.Image = processed;
        }

        private void edgeEnhanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}
