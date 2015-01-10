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

            for (int i = 0; i < list.Count() ; i++)
            {
                set.Input[i] = list[i].RawImage.Select(b => double.Parse(b.ToString())).ToArray();
                set.Output[i] = new double[] { double.Parse(list[i].Label.ToString()) };
            }

        }

        public void Init()
        {
            TrainSet = new DataSet();
            TestSet = new DataSet();
            FillSet(TrainSet, MCore.TrainingImages);
            FillSet(TestSet, MCore.TestImages);
        }



        public void start(MNISTCore core, int trainCount, int testCount)
        {
            MCore = core;
            // initialize input and output values
            Init();
            // create neural network
            ActivationNetwork network = new ActivationNetwork(
                new SigmoidFunction(2),
                2, // two inputs in the network
                2, // two neurons in the first layer
                1); // one neuron in the second layer
            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);

            bool needToStop = false;

            // loop
            while (!needToStop)
            {
                // run epoch of learning procedure
                double error = teacher.RunEpoch(TrainSet.Input, TrainSet.Output);
                // check error value to see if we need to stop
                // ...

                Console.WriteLine("Error = " + error);
            }
        }
       
            
    }



    
}
