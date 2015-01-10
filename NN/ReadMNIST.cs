using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MNIST_Core
{

    public class ReadMNIST
    {
        private byte[][] pixles;
        private byte label;
        private string m_labelsPath;
        private string m_imagesPath;

        private int m_DBSize;

        public int DBSize
        {
            get { return m_DBSize; }
            set { m_DBSize = value; }
        }


        private List<DigitImage> m_Images = new List<DigitImage>();

        public List<DigitImage> Images
        {
            get { return m_Images; }
            set { m_Images = value; }
        }

        public ReadMNIST(string labelsPath, string imagesPath, int size)
        {
            m_labelsPath = labelsPath;
            m_imagesPath = imagesPath;
            DBSize = size;

            Update();
        }

        public void Update()
        {
            try
            {
                FileStream fsLabels    = new FileStream(m_labelsPath, FileMode.Open);
                FileStream fsImages    = new FileStream(m_imagesPath, FileMode.Open);
                BinaryReader brLabels = new BinaryReader(fsLabels);
                BinaryReader brImages = new BinaryReader(fsImages);

                //parse images
                int magic1 = brImages.ReadInt32();
                int numImages = brImages.ReadInt32();
                int numRows = brImages.ReadInt32();
                int nubCols = brImages.ReadInt32();

                //parse labels
                int magic2 = brLabels.ReadInt32();
                int numLabels = brLabels.ReadInt32();

                Images.Clear();

                pixles = new byte[28][];
                for (int i = 0; i < pixles.Length; i++)
                    pixles[i] = new byte[28];

                //for imgaes
                for (int di = 0; di < DBSize; di++)
                {
                    for (int i = 0; i < 28; i++)
                    {
                        for (int j = 0; j < 28; j++)
                        {
                            pixles[i][j] = (byte)brImages.ReadByte();
                        }

                    }
                    label = brLabels.ReadByte();
                    DigitImage dImage = new DigitImage(pixles, label);
                    //Console.WriteLine(dImage.ToString());
                    //Console.ReadLine();

                    Images.Add(dImage);
                }

                fsImages.Close();
                fsLabels.Close();
                brImages.Close();
                brLabels.Close();

            }
            catch(Exception ex)
            {
                Console.WriteLine("problem parsing MNIST DB");
            }
        }

        //private int GetDatasetSize(DATASET__TYPE type)
        //{
        //    int res = 0;
        //    if (type == DATASET__TYPE.TESTING)
        //        res = 10000;
        //    else
        //        res = 60000;
        //    return res;
        //}


        

    }
}
