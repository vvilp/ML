using System;
using System.Collections.Generic;

namespace ML
{
	public class Neuron
	{
		public List <double> weight { get; set; }

		public Neuron (int numInput)
		{
			weight = new List<double> ();
			for (int i = 0; i < numInput; i++) {
				weight.Add (Util.RandomDouble(-0.1, 0.1));
			}
		}
	}

	public class NeuronLayer
	{
		private List <Neuron> neuronList;

		public NeuronLayer (int numNeuron, int numNeuronInput)
		{
			neuronList = new List<Neuron> ();
			for (int i = 0; i < numNeuron; i++) {
				neuronList.Add (new Neuron(numNeuronInput));
			}
		}
	}

	public class NeuralNet
	{

	}
}

