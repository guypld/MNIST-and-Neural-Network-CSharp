using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Neuro;
using AForge.Neuro.Learning;

namespace NN
{

    class backprop
    {
        public static DigitImage[] _TestImages;
        public static DigitImage[] _TrainingImages;

        double[][] input,output;

        public void Init(int trainCount, int testCount)
        {
            for (int i = 0; i < trainCount; i++)
            {
                input[i] = _TrainingImages[i].pixels;
            }

            double[][] input = new double[4][] {
                new double[] {0, 0}, new double[] {0, 1},
                new double[] {1, 0}, new double[] {1, 1}
            };
            double[][] output = new double[4][] {
                new double[] {0}, new double[] {1},
                new double[] {1}, new double[] {0}
            };
        }



        public void start(DigitImage[] TrainingImages, DigitImage[] TestImages, int trainCount, int testCount)
        {
            _TestImages = TestImages;
            _TrainingImages = TrainingImages;

            Init(trainCount, testCount);

            // initialize input and output values

            // create neural network
            ActivationNetwork network = new ActivationNetwork(
                new SigmoidFunction(2),
                2, // two inputs in the network
                2, // two neurons in the first layer
                1); // one neuron in the second layer
            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            // loop
            while (false)//!needToStop)
            {
                // run epoch of learning procedure
                double error = teacher.RunEpoch(input, output);
                // check error value to see if we need to stop
                // ...
            }
        }
       
            
    }



    
}
