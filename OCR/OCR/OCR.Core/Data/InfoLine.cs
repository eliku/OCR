using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.Core.Data
{
    public class InfoLine
    {
        private int _num;
        private int _xUp;
        private int _xDown;

        public int Num
        {
            get {
                return _num;
            }
        }

        public int xUp
        {
            get {
                return _xUp;
            }
            set {
                _xUp = value;
            }
        }

        public int xDown
        {
            get
            {
                return _xDown;
            }
            set
            {
                _xDown = value;
            }
        }
        public InfoLine (int num, int xUp, int xDown)
        {
            _num = num;
            _xUp = xUp;
            _xDown = xDown;
        }
    }
}
