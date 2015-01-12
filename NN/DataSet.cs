using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NN
{
    public class DataSet
    {
        double[][] _Input;
        double[][] _Output;

        public double[][] Output
        {
            get { return _Output; }
            set { _Output = value; }
        }
        public double[][] Input
        {
            get { return _Input; }
            set { _Input = value; }
        }
        
    }
}
