using Microsoft.Win32;
using System;
using ImageProcessing;
using System.Drawing;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OCR.Core.Data;
using System.Windows.Controls;

namespace OCR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Bitmap> _bitmaps = new List<Bitmap>();
        public MainWindow()
        {
            InitializeComponent();
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "(*.jpg)|*.jpg|(*.png)|*.png";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                _bitmaps.Clear();
                cbNoise.IsChecked = false;
                cbConvertGray.IsChecked = false;
                cbConvertWB.IsChecked = false;
            }
            Bitmap bmpFilter = new Bitmap(openFileDialog.FileName);
            _bitmaps.Add(bmpFilter);
            //-------------------------Подавление шума---------------
            _bitmaps.Add(new NoiseFilter().Filter(bmpFilter));
            cbNoise.IsChecked = true;

            //-------------------------Преобразование изображения в серо - белое изображение-
            _bitmaps.Add(OutPicture(_bitmaps[_bitmaps.Count - 1]));
            cbConvertGray.IsChecked = true;

            //-------------------------Преобразование изображение в черно-белое-
            Monomap mp = new Monomap( _bitmaps[_bitmaps.Count - 1]);
            _bitmaps.Add(mp.imageNew);
            cbConvertWB.IsChecked = true;

            //-------------------------Вывод изображения-------------
            IntPtr hBitmap = _bitmaps[_bitmaps.Count - 1].GetHbitmap();
            BitmapSource result = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(hBitmap);
            image.Source = result;




        }

        private Bitmap OutPicture(Bitmap bmp)
        {
            Bitmap output = new Bitmap(bmp.Width, bmp.Height);

            for (int j = 0; j < bmp.Height; j++)
            {
                for (int i = 0; i < bmp.Width; i++)
                {
                    UInt32 pixel = (UInt32)(bmp.GetPixel(i,j).ToArgb());

                    float R = (float)((pixel & 0x00FF0000) >> 16);
                    float G = (float)((pixel & 0x0000FF00) >> 8);
                    float B = (float)((pixel & 0x000000FF));

                    R = G = B = (R + G + B) / 3.0f;

                    UInt32 newPixel =(UInt32)0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | ((UInt32)B);
                    output.SetPixel(i, j, System.Drawing.Color.FromArgb((int)newPixel));

                }
            }

            return output;
        }

    }
}
