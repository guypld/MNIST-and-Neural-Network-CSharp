using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NN
{
    class DigitImage
    {
        public byte[][] pixels;
        public byte label;

        public DigitImage(byte[][] _pixels, byte _label)
        {
            pixels = new byte[28][];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = new byte[28];

            for (int i = 0; i < 28; i++)
                for (int j = 0; j < 28; j++)
                    pixels[i][j] = _pixels[i][j];

            label = _label;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    if (pixels[i][j] == 0)
                        s += " "; //white
                    else if (pixels[i][j] == 255)
                        s += "0"; //black
                    else
                        s += "."; //gray
                }
                s += "\n";
            }
            s += label.ToString();
            return s;
        }
    }
}
