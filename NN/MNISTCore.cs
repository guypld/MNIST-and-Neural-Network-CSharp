using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MNIST_Core
{
    public class MNISTCore
    {
        private ReadMNIST _TrainingDB;
        private ReadMNIST _TestDB;

        public MNISTCore()
        {
            
        }

        public List<DigitImage> TrainingImages
        {
            get { return _TrainingDB.Images; }
        }
        public List<DigitImage> TestImages
        {
            get { return _TestDB.Images; }
        }

        public Boolean LoadDB(string filesPath, int trainSize, int testSize)
        {
            try
            {
                string testImagesPath = filesPath + "t10k-images.idx3-ubyte";
                string testLabelsPath = filesPath + "t10k-labels.idx1-ubyte";
                string trainingImagesPath = filesPath + "train-images.idx3-ubyte";
                string trainingLabelsPath = filesPath + "train-labels.idx1-ubyte";

                _TrainingDB = new ReadMNIST(trainingLabelsPath, trainingImagesPath, trainSize);
                _TestDB = new ReadMNIST(testLabelsPath, testImagesPath, testSize);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
