using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class NeuralNetwork
    {
        private int numInput;
        private int numHidden;
        private int numOutput;

        private double[] inputs;
        private double[][] ihWeights; // input-to-hidden
        private double[] ihSums;
        private double[] ihBiases;
        private double[] ihOutputs;

        private double[][] hoWeights;  // hidden-to-output
        private double[] hoSums;
        private double[] hoBiases;
        private double[] outputs;

        private double[] oGrads; // output gradients for back-propagation
        private double[] hGrads; // hidden gradients for back-propagation

        private double[][] ihPrevWeightsDelta;  // for momentum with back-propagation
        private double[] ihPrevBiasesDelta;

        private double[][] hoPrevWeightsDelta;
        private double[] hoPrevBiasesDelta;

        public NeuralNetwork(int numInput, int numHidden, int numOutput)
        {
            this.numInput = numInput;
            this.numHidden = numHidden;
            this.numOutput = numOutput;

            inputs = new double[numInput];
            ihWeights = Utils.MakeMatrix(numInput, numHidden);
            ihSums = new double[numHidden];
            ihBiases = new double[numHidden];
            ihOutputs = new double[numHidden];
            hoWeights = Utils.MakeMatrix(numHidden, numOutput);
            hoSums = new double[numOutput];
            hoBiases = new double[numOutput];
            outputs = new double[numOutput];

            oGrads = new double[numOutput];
            hGrads = new double[numHidden];

            ihPrevWeightsDelta = Utils.MakeMatrix(numInput, numHidden);
            ihPrevBiasesDelta = new double[numHidden];
            hoPrevWeightsDelta = Utils.MakeMatrix(numHidden, numOutput);
            hoPrevBiasesDelta = new double[numOutput];
        }

        public void UpdateWeights(double[] tValues, double eta, double alpha) // update the weights and biases using back-propagation, with target values, eta (learning rate), alpha (momentum)
        {
            // assumes that SetWeights and ComputeOutputs have been called and so all the internal arrays and matrices have values (other than 0.0)
            if (tValues.Length != numOutput)
                throw new Exception("target values not same Length as output in UpdateWeights");

            // 1. compute output gradients
            for (int i = 0; i < oGrads.Length; ++i)
            {
                double derivative = (1 - outputs[i]) * (1 + outputs[i]); // derivative of tanh
                oGrads[i] = derivative * (tValues[i] - outputs[i]);
            }

            // 2. compute hidden gradients
            for (int i = 0; i < hGrads.Length; ++i)
            {
                double derivative = (1 - ihOutputs[i]) * ihOutputs[i]; // (1 / 1 + exp(-x))'  -- using output value of neuron
                double sum = 0.0;
                for (int j = 0; j < numOutput; ++j) // each hidden delta is the sum of numOutput terms
                    sum += oGrads[j] * hoWeights[i][j]; // each downstream gradient * outgoing weight
                hGrads[i] = derivative * sum;
            }

            // 3. update input to hidden weights (gradients must be computed right-to-left but weights can be updated in any order
            for (int i = 0; i < ihWeights.Length; ++i) // 0..2 (3)
            {
                for (int j = 0; j < ihWeights[0].Length; ++j) // 0..3 (4)
                {
                    double delta = eta * hGrads[j] * inputs[i]; // compute the new delta
                    ihWeights[i][j] += delta; // update
                    ihWeights[i][j] += alpha * ihPrevWeightsDelta[i][j]; // add momentum using previous delta. on first pass old value will be 0.0 but that's OK.
                }
            }

            // 3b. update input to hidden biases
            for (int i = 0; i < ihBiases.Length; ++i)
            {
                double delta = eta * hGrads[i] * 1.0; // the 1.0 is the constant input for any bias; could leave out
                ihBiases[i] += delta;
                ihBiases[i] += alpha * ihPrevBiasesDelta[i];
            }

            // 4. update hidden to output weights
            for (int i = 0; i < hoWeights.Length; ++i)  // 0..3 (4)
            {
                for (int j = 0; j < hoWeights[0].Length; ++j) // 0..1 (2)
                {
                    double delta = eta * oGrads[j] * ihOutputs[i];  // see above: ihOutputs are inputs to next layer
                    hoWeights[i][j] += delta;
                    hoWeights[i][j] += alpha * hoPrevWeightsDelta[i][j];
                    hoPrevWeightsDelta[i][j] = delta;
                }
            }

            // 4b. update hidden to output biases
            for (int i = 0; i < hoBiases.Length; ++i)
            {
                double delta = eta * oGrads[i] * 1.0;
                hoBiases[i] += delta;
                hoBiases[i] += alpha * hoPrevBiasesDelta[i];
                hoPrevBiasesDelta[i] = delta;
            }
        } // UpdateWeights

        public void SetWeights(double[] weights)
        {
            // copy weights and biases in weights[] array to i-h weights, i-h biases, h-o weights, h-o biases
            int numWeights = (numInput * numHidden) + (numHidden * numOutput) + numHidden + numOutput;
            if (weights.Length != numWeights)
                throw new Exception("The weights array length: " + weights.Length + " does not match the total number of weights and biases: " + numWeights);

            int k = 0; // points into weights param

            for (int i = 0; i < numInput; ++i)
                for (int j = 0; j < numHidden; ++j)
                    ihWeights[i][j] = weights[k++];

            for (int i = 0; i < numHidden; ++i)
                ihBiases[i] = weights[k++];

            for (int i = 0; i < numHidden; ++i)
                for (int j = 0; j < numOutput; ++j)
                    hoWeights[i][j] = weights[k++];

            for (int i = 0; i < numOutput; ++i)
                hoBiases[i] = weights[k++];
        }

        public double[] GetWeights()
        {
            int numWeights = (numInput * numHidden) + (numHidden * numOutput) + numHidden + numOutput;
            double[] result = new double[numWeights];
            int k = 0;
            for (int i = 0; i < ihWeights.Length; ++i)
                for (int j = 0; j < ihWeights[0].Length; ++j)
                    result[k++] = ihWeights[i][j];
            for (int i = 0; i < ihBiases.Length; ++i)
                result[k++] = ihBiases[i];
            for (int i = 0; i < hoWeights.Length; ++i)
                for (int j = 0; j < hoWeights[0].Length; ++j)
                    result[k++] = hoWeights[i][j];
            for (int i = 0; i < hoBiases.Length; ++i)
                result[k++] = hoBiases[i];
            return result;
        }

        public double[] ComputeOutputs(double[] xValues)
        {
            if (xValues.Length != numInput)
                throw new Exception("Inputs array length " + inputs.Length + " does not match NN numInput value " + numInput);

            for (int i = 0; i < numHidden; ++i)
                ihSums[i] = 0.0;
            for (int i = 0; i < numOutput; ++i)
                hoSums[i] = 0.0;

            for (int i = 0; i < xValues.Length; ++i) // copy x-values to inputs
                this.inputs[i] = xValues[i];

            for (int j = 0; j < numHidden; ++j)  // compute input-to-hidden weighted sums
                for (int i = 0; i < numInput; ++i)
                    ihSums[j] += this.inputs[i] * ihWeights[i][j];

            for (int i = 0; i < numHidden; ++i)  // add biases to input-to-hidden sums
                ihSums[i] += ihBiases[i];

            for (int i = 0; i < numHidden; ++i)   // determine input-to-hidden output
                ihOutputs[i] = SigmoidFunction(ihSums[i]);

            for (int j = 0; j < numOutput; ++j)   // compute hidden-to-output weighted sums
                for (int i = 0; i < numHidden; ++i)
                    hoSums[j] += ihOutputs[i] * hoWeights[i][j];

            for (int i = 0; i < numOutput; ++i)  // add biases to input-to-hidden sums
                hoSums[i] += hoBiases[i];

            for (int i = 0; i < numOutput; ++i)   // determine hidden-to-output result
                this.outputs[i] = HyperTanFunction(hoSums[i]);

            double[] result = new double[numOutput]; // could define a GetOutputs method instead
            this.outputs.CopyTo(result, 0);

            return result;
        } // ComputeOutputs

        private static double StepFunction(double x) // an activation function that isn't compatible with back-propagation bcause it isn't differentiable
        {
            if (x > 0.0) return 1.0;
            else return 0.0;
        }

        private static double SigmoidFunction(double x)
        {
            if (x < -45.0) return 0.0;
            else if (x > 45.0) return 1.0;
            else return 1.0 / (1.0 + Math.Exp(-x));
        }

        private static double HyperTanFunction(double x)
        {
            if (x < -10.0) return -1.0;
            else if (x > 10.0) return 1.0;
            else return Math.Tanh(x);
        }
    } // class NeuralNetwork

}
