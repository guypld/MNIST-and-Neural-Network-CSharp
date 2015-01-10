using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NN
{
    class Program
    {
        public static DigitImage[] TestImages = new DigitImage[10000];
        public static DigitImage[] TrainingImages = new DigitImage[60000];
        static void Main(string[] args)
        {
            const string filesPath = @"E:\Dropbox\University\M.Sc\Neural Networks\HW1\";

            string testImagesPath =     filesPath + "t10k-images.idx3-ubyte";
            string testLabelsPath =     filesPath + "t10k-labels.idx1-ubyte";
            string trainingImagesPath = filesPath + "train-images.idx3-ubyte";
            string trainingLabelsPath = filesPath + "train-labels.idx1-ubyte";

            ReadMNIST TestUpdater       = new ReadMNIST(testLabelsPath, testImagesPath, DATASET__TYPE.TESTING);
            TestUpdater.Update();
            ReadMNIST TrainingUpdater   = new ReadMNIST(trainingLabelsPath, trainingImagesPath, DATASET__TYPE.TRAINING);
            TrainingUpdater.Update();

            WelcomeScreen();
            


            //Console.WriteLine(

        }

        private static void WelcomeScreen()
        {
            Console.WriteLine("*****************************");
            Console.WriteLine("*                           *");
            Console.WriteLine("* Neural Networking Project *");
            Console.WriteLine("*                           *");
            Console.WriteLine("*****************************");
            Console.WriteLine();
            Console.WriteLine("Select Project to run:");
            Console.WriteLine("1. Project A: Back-propagation");
            Console.WriteLine("2. Porject B: Counter-propagation");
            Console.WriteLine("3. Project C: Counter-propagation with SDM");
            string res = Console.ReadLine();

            while (res != "1" && res != "2" && res != "3")
            {
                Console.WriteLine("invalid input! please select a project to run:");
                res = Console.ReadLine();
            }
            switch (res)
            {
                case "1":
                    backprop bp = new backprop();
                    bp.start();
                    break;
                case "2":
                    Console.WriteLine("TBD");
                    break;
                case "3":
                    Console.WriteLine("TBD");
                    break;
                default:
                    Console.WriteLine("wrong input! how did i get here?!");
                    break;
            }
        }
    }
}
