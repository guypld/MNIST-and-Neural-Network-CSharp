using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Neuro;
using AForge.Neuro.Learning;
using NN;

namespace MNIST_Core
{
    public class BackPropagation
    {
        private MNISTCore _MCore = new MNISTCore();
        private ActivationNetwork network;
        private BackPropagationLearning teacher;

        public DataSet TrainSet { get; set; }
        public DataSet TestSet { get; set; }

        public MNISTCore MCore
        {
            get { return _MCore; }
            set { _MCore = value; }
        }


        private void FillSet(DataSet set, List<DigitImage> list)
        {
            set.Input = new double[list.Count()][];
            set.Output = new double[list.Count()][];

            for (int i = 0; i < list.Count(); i++)
            {
                //Input : [28*28] pixels
                set.Input[i] = list[i].RawImage;

                //Output : [10] items [-1,-1,...,1,-1,-1,..] - representing label(0 to 10)
                set.Output[i] = new double[10];
                for (int j = 0; j < 10; j++)
                {
                    set.Output[i][j] = -1;
                }
                set.Output[i][int.Parse(list[i].Label.ToString())] = 1;
            }

        }

        public void Init()
        {
            TrainSet = new DataSet();
            TestSet = new DataSet();
            FillSet(TrainSet, MCore.TrainingImages);
            FillSet(TestSet, MCore.TestImages);
        }

        /// <summary>
        /// Train using Back Propogation
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="outputData">The output data.</param>
        /// <param name="iterations">The iterations.</param>
        /// <param name="neurons">The neurons.</param>
        /// <returns></returns>
        private string[] Train(double[][] inputData, double[][] outputData, int iterations)
        {
            System.Diagnostics.Debug.WriteLine("Training Started...");
            bool needStopping = false;
            int iterateCount = 0;
            double error = 0;
            string[] results = new string[2];

            // initialize input and output values
            double[][] input = inputData;
            double[][] output = outputData;

            var network = new AForge.Neuro.ActivationNetwork(
                new AForge.Neuro.BipolarSigmoidFunction(2),
                784, // 784 inputs (coz each array corresponding to an image consists of 784 elements )
                500,20, //784 neurons in the first layer  (corresponding to input)
                10); //10 neurons in the second layer (corresponding to 10 digits )
            network.Randomize();
            teacher = new AForge.Neuro.Learning.BackPropagationLearning(network);
            teacher.LearningRate = 0.075;//1;
            
            teacher.Momentum = 0;


            while (!needStopping)
            {
                error = teacher.RunEpoch(input, output);
                if (error == 0) //If the error rate is 0
                {
                    break;
                }
                else if (Math.Round(error, 2) == 0) // If the error rate is 0 to the second decimal point
                {
                    break;
                }
                else if (iterateCount < iterations) //If the given iteraions are completed
                {
                    iterateCount++;
                }
                else
                {
                    needStopping = true;
                }
                teacher.LearningRate *= 0.85;
                iterateCount++;
                System.Diagnostics.Debug.WriteLine("Iteration  :\t" + iterateCount + " \tError Rate :\t" + error);
            }

            System.Diagnostics.Debug.WriteLine("Error Rate : " + error);
            results[0] = error.ToString();
            System.Diagnostics.Debug.WriteLine("Iterations : " + iterateCount);
            results[1] = iterateCount.ToString();
            System.Diagnostics.Debug.WriteLine("Training Completed...");
            return results;
        }


        public void start(MNISTCore core, int trainCount, int testCount)
        {
            MCore = core;
            // initialize input and output values
            Init();

            var s = Train(TrainSet.Input, TrainSet.Output, 300);


            // Check The Test Set now:

            //List<double> Test = new List<double>();
            //for (int x = data.Length + 1; x < data.Length * 2; x++)
            //{
            //    double nErg = network.Compute(new double[] { x / Factor })[0];
            //    Test.Add(nErg);
            //}

            
        }


    }




}
