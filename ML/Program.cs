using System;
using System.Collections.Generic;

namespace ML
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//Console.WriteLine ("Hello World!");
			Neuron neuron = new Neuron (10);

			List<double> testweight = neuron.weight;
			testweight [0] = 1;
			Console.WriteLine (testweight[0]);
		}
	}
}
