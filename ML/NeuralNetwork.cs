using System;
using System.Collections.Generic;

namespace ML
{
	public class Neuron
	{
		public double[] Weight { get; set; }
		public double[] Input { get; set; }
		public double Output { get; set; }

		private double Sigmoid(double x)
		{
			return 1.0 / (1.0 + Math.Pow (Math.E, -x));
		}

		public Neuron (int numInput)
		{
			Weight = new double[numInput + 1];
			for (int i = 0; i < numInput + 1; i++) {
				Weight[i] = (Util.RandomDouble(- 1, 1));
			}

		}

		public double UpdateInOutPut(double[] Input)
		{
			this.Input = Input;
			double res = 0;
			for (int i = 0; i < Input.Length; i++) {
				res += Weight [i] * Input [i];
			}
			res += Weight [Input.Length];
			res = Sigmoid (res);
			Output = res;
			return  res;
		}

		//for output layer, thetaO = (o - t) * o * (1 - o)
		//for hidden layer, thetaH = TODO
		public double[] UpdateWeight(double delta, double learningRate)
		{
			for (int i = 0; i < Weight.Length - 1; i++) {
				Weight [i] += (-learningRate) * delta * Input [i];
			}
			Weight [Weight.Length - 1] += (-learningRate) * delta;
			return Weight;
		}
	}

	public class NeuronLayer
	{
		public List<Neuron> NeuronList { get; set; }

		public NeuronLayer (int numNeuron, int numNeuronInput)
		{
			NeuronList = new List<Neuron> ();
			for (int i = 0; i < numNeuron; i++) {
				NeuronList.Add (new Neuron(numNeuronInput));
			}
		}
	}

	public class NeuralNet
	{
		private readonly double[][] trainingData;
		private readonly double[][] targetValue;
		private double learningRate;
		private int hiddenLayerNum;
		private int hiddenLayerNeuronNum; // for each layer
		private int outputNeuronNum;

		private NeuronLayer hiddenLayer;
		private NeuronLayer outputLayer;

		private double Sigmoid(double x)
		{
			return 1.0 / (1.0 + Math.Pow (Math.E, -x));
		}

		public NeuralNet(double[][] trainingData, double[][] targetValue, double learningRate, int hiddenLayerNum = 1, int hiddenLayerNeuronNum = 2)
		{
			this.trainingData = trainingData;
			this.targetValue = targetValue;
			this.learningRate = learningRate;
			this.hiddenLayerNum = hiddenLayerNum;
			this.hiddenLayerNeuronNum = hiddenLayerNeuronNum;
			this.outputNeuronNum = targetValue[0].Length;

			hiddenLayer = new NeuronLayer(this.hiddenLayerNeuronNum, trainingData[0].Length);
			outputLayer = new NeuronLayer(outputNeuronNum, this.hiddenLayerNeuronNum); // single output
		}

		public void TrainingProcess()
		{
			int iterationNum = 0;
			double MSE = 0;
			bool isWriteDetails = false;
			while(true){
				for (int indexData = 0; indexData < trainingData.Length; indexData++) {

					if(isWriteDetails)Console.Write("Input data : {0,-13} | target : {1,-10} | output: ", string.Join(",", trainingData[indexData]), string.Join(",", targetValue[indexData]));


					double[] HlayerOutputs = new double[hiddenLayer.NeuronList.Count];
					for (int i = 0; i < hiddenLayer.NeuronList.Count; i++) {
						double output = hiddenLayer.NeuronList [i].UpdateInOutPut (trainingData[indexData]);
						HlayerOutputs [i] = output;
						//Console.WriteLine ("Hlayer neuron {0} output : {1}",i,output);
					}

					double[] OlayerOutputs = new double[outputLayer.NeuronList.Count];
					for (int i = 0; i < outputLayer.NeuronList.Count; i++) {
						double output = outputLayer.NeuronList [i].UpdateInOutPut (HlayerOutputs);
						OlayerOutputs [i] = output;
						if(isWriteDetails)Console.Write("{0" +
							":0.00} ", output);
					}

					double []deltaO = new double[outputLayer.NeuronList.Count];
					for (int i = 0; i < outputLayer.NeuronList.Count; i++) {
						double o = OlayerOutputs [i];
						double t = targetValue [indexData] [i];
						deltaO [i] = o * (1 - o) * (o - t);
					}

					double []deltaH = new double[hiddenLayer.NeuronList.Count];
					for (int i = 0; i < hiddenLayer.NeuronList.Count; i++) {
						double deltaTemp = 0;
						for (int j = 0; j < outputLayer.NeuronList.Count; j++) {
							deltaTemp += deltaO [j] * outputLayer.NeuronList [j].Weight [i];
						}
						double o = HlayerOutputs [i];
						deltaH [i] = o * (1 - o) * deltaTemp;
					}

					// update w
					for (int i = 0; i < outputLayer.NeuronList.Count; i++) {
						outputLayer.NeuronList [i].UpdateWeight (deltaO [i], this.learningRate);
					}

					for (int i = 0; i < hiddenLayer.NeuronList.Count; i++) {
						hiddenLayer.NeuronList [i].UpdateWeight (deltaH [i], this.learningRate);
					}

					double MSEI = 0;
					for (int i = 0; i < OlayerOutputs.Length; i++) {
						double delta = (OlayerOutputs [i] - targetValue [indexData] [i]);
						MSEI += delta * delta;
					}
					MSEI = MSEI / OlayerOutputs.Length;
					MSE += MSEI;
					if(isWriteDetails)Console.WriteLine ("MSEI : {0:0.0000} ", MSEI);
				}
				isWriteDetails = false;
				MSE = MSE / trainingData.Length;

				if(iterationNum % 1000 == 0) {
					Console.WriteLine ();
					Console.WriteLine ("iterationNum : {0}, MSE : {1:0.0000}", iterationNum, MSE);

					isWriteDetails = true;
					Console.Read ();
				}

				MSE = 0;
				iterationNum++;
			}
		}
	}
}

