using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Diagnostics;

namespace CoinsActivity
{
  
    public class CoinsProcess
    {
        // Method to detect coins and draw rectangles
        public Bitmap DetectCoinsAndDrawRectangles(Bitmap inputImage, out double totalValue)
        {    
            var coins = FindCoins(inputImage);

            using (Graphics g = Graphics.FromImage(inputImage))
            {
          
                totalValue = 0;

                foreach (var coin in coins)
                {
                    // Draw rectangle around the detected coin
                    g.DrawRectangle(Pens.Red, coin);

                    // Measure and identify the coin
                    string coinInfo = MeasureCoinSize(coin);

                    // Add coin value to the total
                    totalValue += GetCoinValue(coin);
                   
           
                }
            }

            return inputImage;
        }

        
        public double GetCoinValue(Rectangle coinRectangle)
        {
            // Calculate the area of the rectangle (coin)
            int area = coinRectangle.Width * coinRectangle.Height;
          
            // Identify the coin type and return its value
            string coinType = IdentifyCoin(area);
            return GetCoinValueByType(coinType);
        }

        
        public string IdentifyCoin(int area)
        {
            
            if (area >= 900 && area < 910)
            {
                return "0.05 Peso";
            }
            else if (area >= 1000 && area < 1010)
            {
                return "0.10 Peso";
            }
            else if (area >= 1300 && area < 1310)
            {
                return "1 Peso";
            }
            else if (area >= 1600 && area < 1610)
            {
                return "5 Peso"; 
            }
            else
            {
                return "Unknown Coin"; 
            }
        }

   
        public double GetCoinValueByType(string coinType)
        {
            switch (coinType)
            {
                case "0.05 Peso":
                    return 0.05;
                case "0.10 Peso":
                    return 0.10;
                case "1 Peso":
                    return 1.0;
                case "5 Peso":
                    return 5;
                default:
                    return 0.0; 
            }
        }


   
        public string MeasureCoinSize(Rectangle coinRectangle)
        {
            // Calculate the area of the rectangle (coin)
            int area = coinRectangle.Width * coinRectangle.Height;

            // Identify the coin based on the area (example with typical coin sizes)
            string coinType = IdentifyCoin(area);

            // Return a description with size and coin type
            return $"{coinType} ({area} pixels)";
        }

       



        

      


        public Rectangle[] FindCoins(Bitmap edgeImage)
        {
            // Simple implementation of finding contours or connected components (basic approach)
            // In a real application, you could use more advanced libraries like OpenCV

            var coins = new List<Rectangle>();

            for (int x = 0; x < edgeImage.Width; x++)
            {
                for (int y = 0; y < edgeImage.Height; y++)
                {
                    if (edgeImage.GetPixel(x, y).ToArgb() == Color.White.ToArgb()) // If it's an edge pixel
                    {
                        // Check for the bounding box of the region
                        int minX = x, minY = y, maxX = x, maxY = y;
                        for (int i = x; i < edgeImage.Width && edgeImage.GetPixel(i, y).ToArgb() == Color.White.ToArgb(); i++)
                        {
                            maxX = i;
                        }
                        for (int j = y; j < edgeImage.Height && edgeImage.GetPixel(x, j).ToArgb() == Color.White.ToArgb(); j++)
                        {
                            maxY = j;
                        }

                        coins.Add(new Rectangle(minX, minY, maxX - minX, maxY - minY));
                    }
                }
            }

            return coins.ToArray();
        }

       
    }

}
