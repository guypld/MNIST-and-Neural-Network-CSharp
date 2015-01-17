using System;

namespace BackPropagation
{
  class BackPropagationProgram
  {
    static void Main(string[] args)
    {
      try
      {
        Console.WriteLine("\nBegin Neural Network Back-Propagation demo\n");

        Console.WriteLine("Creating a 3-input, 4-hidden, 2-output neural network");
        Console.WriteLine("Using sigmoid function for input-to-hidden activation");
        Console.WriteLine("Using tanh function for hidden-to-output activation");
        NeuralNetwork nn = new NeuralNetwork(3, 4, 2);

        // arbitrary weights and biases
        double[] weights = new double[] {
          0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0, 1.1, 1.2,
          -2.0, -6.0, -1.0, -7.0,
          1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2.0,
          -2.5, -5.0 };

        Console.WriteLine("\nInitial 26 random weights and biases are:");
        Utils.ShowVector(weights, 2, true);

        Console.WriteLine("Loading neural network weights and biases");
        nn.SetWeights(weights);

        Console.WriteLine("\nSetting inputs:");
        double[] xValues = new double[] { 1.0, 2.0, 3.0 };
        Utils.ShowVector(xValues, 2, true);

        double[] initialOutputs = nn.ComputeOutputs(xValues);
        Console.WriteLine("Initial outputs:");
        Utils.ShowVector(initialOutputs, 4, true);

        double[] tValues = new double[] { -0.8500, 0.7500 }; // target (desired) values. note these only make sense for tanh output activation
        Console.WriteLine("Target outputs to learn are:");
        Utils.ShowVector(tValues, 4, true);

        double eta = 0.90;  // learning rate - controls the maginitude of the increase in the change in weights. found by trial and error.
        double alpha = 0.04; // momentum - to discourage oscillation. found by trial and error.
        Console.WriteLine("Setting learning rate (eta) = " + eta.ToString("F2") + " and momentum (alpha) = " + alpha.ToString("F2"));
        
        Console.WriteLine("\nEntering main back-propagation compute-update cycle");
        Console.WriteLine("Stopping when sum absolute error <= 0.01 or 1,000 iterations\n");
        int ctr = 0;
        double[] yValues = nn.ComputeOutputs(xValues); // prime the back-propagation loop
        double error = Error(tValues, yValues);
        while (ctr < 1000 && error > 0.01)
        {
          Console.WriteLine("===================================================");
          Console.WriteLine("iteration = " + ctr);
          Console.WriteLine("Updating weights and biases using back-propagation");
          nn.UpdateWeights(tValues, eta, alpha);
          Console.WriteLine("Computing new outputs:");
          yValues = nn.ComputeOutputs(xValues);
          Utils.ShowVector(yValues, 4, false);
          Console.WriteLine("\nComputing new error");
          error = Error(tValues, yValues);
          Console.WriteLine("Error = " + error.ToString("F4"));
          ++ctr;
        }
        Console.WriteLine("===================================================");
        Console.WriteLine("\nBest weights and biases found:");
        double[] bestWeights = nn.GetWeights();
        Utils.ShowVector(bestWeights, 2, true);
        
        Console.WriteLine("End Neural Network Back-Propagation demo\n");
        Console.ReadLine();
      }
      catch (Exception ex)
      {
        Console.WriteLine("Fatal: " + ex.Message);
        Console.ReadLine();
      }
    } // Main

    static double Error(double[] target, double[] output) // sum absolute error. could put into NeuralNetwork class.
    {
      double sum = 0.0;
      for (int i = 0; i < target.Length; ++i)
        sum += Math.Abs(target[i] - output[i]);
      return sum;
    }

  } // class BackPropagation

} // ns
