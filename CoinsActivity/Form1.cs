using ImageProcess2;
using OpenCvSharp;
using System;



namespace CoinsActivity
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed;


        public Form1()
        {
            InitializeComponent();

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog(this);
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }



        private void button1_Click(object sender, EventArgs e)
        {

            CoinsProcess c = new CoinsProcess();

            Bitmap processed = (Bitmap)loaded.Clone();
            ImageProcess2.BitmapFilter.GrayScale(processed);
            ImageProcess2.BitmapFilter.GaussianBlur(processed, 9);
            ImageProcess2.BitmapFilter.EdgeDetectConvolution(processed, 3, 100);
          

            double totalValue;
            Bitmap resultImage = c.DetectCoinsAndDrawRectangles(processed, out totalValue);

          
            pictureBox2.Image = resultImage;




            label1.Text = $"Total value of coins: {totalValue:F2} pesos";
        }





       

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
