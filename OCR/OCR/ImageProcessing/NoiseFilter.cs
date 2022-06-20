using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class NoiseFilter
    {
        private Bitmap _bmp;

        public Bitmap bmp
        {
            get {
                return _bmp;
            }
            set {
                _bmp = value;
            }
        }
        public Bitmap Filter(Bitmap bmp)
        {
            Bitmap rezultImage = new Bitmap(bmp);

            int[,] arrR = new int[rezultImage.Width, rezultImage.Height];
            int[,] arrG = new int[rezultImage.Width, rezultImage.Height];
            int[,] arrB = new int[rezultImage.Width, rezultImage.Height];

            for (int i = 0; i < rezultImage.Width; i++)
            {
                for (int j = 0; j < rezultImage.Height; j++)
                {
                    arrR[i, j] = rezultImage.GetPixel(i, j).R;
                    arrG[i, j] = rezultImage.GetPixel(i, j).G;
                    arrB[i, j] = rezultImage.GetPixel(i, j).B;
                }
            }

            for (int i = 1; i < rezultImage.Width - 1; i++)
            {
                for (int j = 1; j < rezultImage.Height - 1; j++)
                {
                    int arrRSum = 0, arrGSum = 0, arrBSum = 0;
                    int arrsrR = 0, arrsrG = 0, arrsrB = 0;
                    for (int x = -1; x < 2; x++)
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            arrRSum = arrRSum + arrR[i + x, j + y];
                            arrGSum = arrGSum + arrG[i + x, j + y];
                            arrBSum = arrBSum + arrB[i + x, j + y];

                        }
                    }
                    arrsrR = arrRSum / 9;
                    arrsrG = arrGSum / 9;
                    arrsrB = arrBSum / 9;
                    rezultImage.SetPixel(i, j, Color.FromArgb(arrsrR, arrsrG, arrsrB));

                }
            }


            return rezultImage;
        }

    }
}
