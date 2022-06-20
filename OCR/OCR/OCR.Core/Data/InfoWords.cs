
using System.Collections.Generic;

namespace OCR.Core.Data
{
    public class InfoWords
    {
        private int _num;
        private int _begin;
        private int _end;

        public int num
        {
            get
            {
                return _num;
            }
            set
            {
                _num = value;
            }
        }

        public int begin
        {
            get 
            {
                return _begin;
            }
            set
            {
                _begin = value;
            }
        }

        public int end
        {
            get
            {
                return _end;
            }
            set 
            {
                _end = value;
            }
        }

        public InfoWords( int Num, int Begin, int End)
        {
            num = Num;
            begin = Begin;
            end = End;
        }
    }
}
