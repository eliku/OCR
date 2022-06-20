using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace OCR.Core.Data
{
    public class Monomap
    {
        private float[,] _brightValue;                               //Яркость для каждого пикселя
        private float[] _averLineValues;                             //средние значения яркости для каждой строки
        private float[] _averColValues;                              //средние значения яркости для каждого столбца найденной строки
        private float _brightImage;                                  //значение яркости всего изображения
        private float _brightCol;                                    //среднее значение яркости в строке
        public Bitmap imageNew;

        public List<InfoLine> _coord = new List<InfoLine>();          //координаты распознаных строк
        public List<InfoWords> _words = new List<InfoWords>();        //координаты распознаных слов

        public Monomap(Bitmap image)
        {
            float kof = 0.99f;

            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            imageNew = new Bitmap(image);
            _brightValue = new float[image.Width, image.Height];
            _averLineValues = new float[image.Height];
            _averColValues = new float[image.Width];

            _coord.Clear();
            //вычисляем яркость
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);
                    _brightValue[x, y] = image.GetPixel(x, y).GetBrightness();
                }
            }

            float sum = 0;
            //вычисляем среднюю яркость каждой строки
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    sum += _brightValue[x, y];
                }
                _averLineValues[y] = sum / image.Width;
                sum = 0;
            }
            //вычисляем среднюю яркость всей картинки
            _brightImage = 0;
            for (int y = 0; y < image.Height; y++)
            {
                _brightImage += _averLineValues[y];
            }
            _brightImage = _brightImage / image.Height;

            //вычисляем координаты верхний и нижней границы строк
            for (int y = 2; y < image.Height - 3; y++)
            {
                if (((_averLineValues[y] > (kof * _brightImage)) && (_averLineValues[y + 1] < _brightImage)) ||
                        ((_averLineValues[y + 1] < _brightImage) && (_averLineValues[y + 2] < _brightImage) && (_averLineValues[y + 3] < _brightImage)))
                {
                    for (int y2 = y; y2 < image.Height - 3; y2++)
                    {
                        if ((_averLineValues[y2 - 2] < (kof * _brightImage)) && (_averLineValues[y2 - 1] < (kof * _brightImage)) &&
                            (_averLineValues[y2] > (kof * _brightImage)) && (_averLineValues[y2 + 1] > _brightImage) &&
                            (_averLineValues[y2 + 2] > _brightImage) && (_averLineValues[y2 + 3] > _brightImage))
                        {
                            _coord.Add(new InfoLine(_coord.Count+1, y, y2));
                            y = y2;
                            break;
                        }
                    }
                }
            }
            //вычисление средней высоты строк
            if (_coord.Count != 0)
            {
                int h = 0;

                for (int i = 0; i < _coord.Count; i++)
                {
                    h += _coord[i].xDown - _coord[i].xUp;
                }

                h = (int)(Math.Ceiling(0.3 * (float)(h / _coord.Count)));

                for (int i = 0; i < _coord.Count; i++)
                {
                    _coord[i].xDown += h;
                    _coord[i].xUp -= h;
                }
            }
            //ищем слова
            _words.Clear();
            //вычисляем среднюю яркость каждой строки
            for (int k = 0; k < _coord.Count; k++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    float sumCol = 0;
                    for (int y = _coord[k].xUp; y < _coord[k].xDown; y++)
                    {
                        sumCol += _brightValue[x, y];
                    }
                    _averColValues[x] = sumCol / (_coord[k].xDown - _coord[k].xUp);
                }
                _brightCol = 0;

                for (int x = 0; x < image.Width; x++)
                {
                    _brightCol += _averColValues[x];
                }
                _brightCol = _brightCol / image.Width;

                for (int x = 2; x < image.Width - 4; x++)
                {
                    if ((_averColValues[x - 2] > kof * _brightCol) && (_averColValues[x - 1] > kof * _brightCol) && (_averColValues[x] < kof * _brightCol) &&
                        (_averColValues[x + 1] < kof * _brightCol) && (_averColValues[x + 2] < kof * _brightCol) &&
                        (_averColValues[x + 3] < kof * _brightCol) && (_averColValues[x + 4] < kof * _brightCol))
                    {
                        for (int x2 = x; x2 < image.Width - 1; x2++)
                        {
                            if ((_averColValues[x2 - 1] < kof * _brightCol) && (_averColValues[x2] > kof * _brightCol) && (_averColValues[x2 + 1] > kof * _brightCol))
                            {
                                _words.Add(new InfoWords(k, x, x2));
                                x = x2;
                                break;
                            }
                        }
                    }
                }
            }

            //прорисовка слов
            for (int k = 0; k < _coord.Count; k++)
            {
                for (int l = 0; l < _words.Count; l++)
                {
                    if (_words[l].num == k) 
                    {
                        for (int y = _coord[k].xUp; y < _coord[k].xDown; y++)
                        {
                            imageNew.SetPixel(_words[l].begin, y, Color.FromArgb(102, 35, 153));
                            imageNew.SetPixel(_words[l].end, y, Color.FromArgb(204, 0, 51));
                        }
                    }
                }            
            }
        }
    }
}


