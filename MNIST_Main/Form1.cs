using MNIST_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MNIST_Main
{
    public partial class Form1 : Form
    {
        private int _CurrentIndex = 0;


        private MNISTCore _DB;

        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateDigitImage(int index)
        {
            try
            {
                var img = _DB.TrainingImages[index].Pixels;
                digitUC1.Pixels = img;
            }
            catch 
            {
                
                
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int trainSize = int.Parse(txtTrainSize.Text);
                int testSize = int.Parse(txtTestSize.Text);

                _DB = new MNISTCore();

                var path = txtFilesPath.Text;

                if (!path.EndsWith(@"\"))
                {
                    path += @"\";
                }

                if (_DB.LoadDB(txtFilesPath.Text,trainSize,testSize) )
                {
                    //MessageBox.Show("DB Load succefully!");
                    (sender as Button).Enabled = false;



                }
                else
                {
                    MessageBox.Show("Error while loading DB!","MNIST",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var bp = new BackPropagation();

            bp.start(_DB, _DB.TrainingImages.Count(), _DB.TestImages.Count());
        }

        private void btnNextDigit_Click(object sender, EventArgs e)
        {
            UpdateDigitImage(++_CurrentIndex);
        }

        private void btnPrevDigit_Click(object sender, EventArgs e)
        {
            UpdateDigitImage(--_CurrentIndex);
        }
    }
}
