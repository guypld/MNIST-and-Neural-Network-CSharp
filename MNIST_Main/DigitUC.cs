using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MNIST_Main
{
    public partial class DigitUC : UserControl
    {

        private byte[][] m_Pixels;

        public byte[][] Pixels
        {
            get { return m_Pixels; }
            set 
            { 
                m_Pixels = value;
                if (value != null)
                {
                    UpdateDigit();
                }
            }
        }

        private void UpdateDigit()
        {
            var g = panel1.CreateGraphics();

            for (int i = 0; i < Pixels.Length; i++)
            {
                for (int j = 0; j < Pixels[i].Length; j++)
                {
                    if (Pixels[i][j] == 0)
                    {
                        g.DrawLine(new Pen(Brushes.White),j,i,j+1,i);
                    }
                    else
	                {
                        g.DrawLine(new Pen(Brushes.Black), j, i, j+1, i);
	                }
                }
            }
        }

        public DigitUC()
        {
            InitializeComponent();
        }

        private void DigitUC_Load(object sender, EventArgs e)
        {

        }

        private void DigitUC_Resize(object sender, EventArgs e)
        {
            this.Width = 28;
            this.Height = 28;
        }
    }
}
