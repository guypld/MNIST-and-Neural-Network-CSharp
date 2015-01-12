using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MNIST_Core
{
    public class DigitImage
    {
        private const int DIM_SIZE = 28;
        public static int SIZE = DIM_SIZE * DIM_SIZE;

        private byte[][] pixels;
        private byte label;

        public byte Label
        {
            get { return label; }
            set { label = value; }
        }

        public byte[][] Pixels
        {
            get { return pixels; }
            set { pixels = value; }
        }
        
        public double[] RawImage
        {
            get 
            {
                double[] res = new double[SIZE];

                for(int i = 0; i < DIM_SIZE; i++)
                {
                    for(int j = 0; j < DIM_SIZE; j++)
                    {
                        res[i * DIM_SIZE + j] = Pixels[j][i]/127.0 - 1.0;
                    }
                }

                return res;
            }
        }

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
