using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NN
{
    enum DATASET__TYPE
    {
        TRAINING,
        TESTING
    }

    class ReadMNIST
    {
        byte[][] pixles;
        byte label;
        string m_labelsPath;
        string m_imagesPath;
        DATASET__TYPE m_type;

        public ReadMNIST(string labelsPath, string imagesPath, DATASET__TYPE type)
        {
            m_labelsPath = labelsPath;
            m_imagesPath = imagesPath;
            m_type = type;
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

                pixles = new byte[28][];
                for (int i = 0; i < pixles.Length; i++)
                    pixles[i] = new byte[28];

                int size = GetDatasetSize(m_type);
                //for imgaes
                for (int di = 0; di < size; di++)
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

                    if (m_type == DATASET__TYPE.TESTING)
                        Program.TestImages[di] = dImage;
                    else
                        Program.TrainingImages[di] = dImage;
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

        private int GetDatasetSize(DATASET__TYPE type)
        {
            int res = 0;
            if (type == DATASET__TYPE.TESTING)
                res = 10000;
            else
                res = 60000;
            return res;
        }


        

    }
}
