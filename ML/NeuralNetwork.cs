using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ML
{
	public class Neuron
	{
		public double[] Weight { get; set; }
		public double[] Input { get; set; }
		public double Output { get; set; }

		public Neuron(int numInput)
		{
			Weight = new double[numInput + 1];
			for (int i = 0; i < numInput + 1; i++)
			{
				Weight[i] = (Util.RandomDouble(-1, 1));
			}

		}

		public double UpdateInOutPut(double[] Input)
		{
            if (Input == null)
            {
                return Double.MinValue;
            }
                
			this.Input = Input;
			double res = 0;
			for (int i = 0; i < Input.Length; i++)
			{
				res += Weight[i] * Input[i];
			}
			res += Weight[Input.Length];
			res = Util.Sigmoid(res);
			Output = res;
			return res;
		}

		//for output layer, thetaO = (o - t) * o * (1 - o)
		//for hidden layer, thetaH = TODO
		public double[] UpdateWeight(double delta, double learningRate)
		{
			for (int i = 0; i < Weight.Length - 1; i++)
			{
				Weight[i] += (-learningRate) * delta * Input[i];
			}
			Weight[Weight.Length - 1] += (-learningRate) * delta;
			return Weight;
		}
	}

	public class NeuronLayer
	{
		public List<Neuron> NeuronList { get; set; }

		public NeuronLayer(int numNeuron, int numNeuronInput)
		{
			NeuronList = new List<Neuron>();
			for (int i = 0; i < numNeuron; i++)
			{
				NeuronList.Add(new Neuron(numNeuronInput));
			}
		}
	}

	public class NeuralNet
	{
		private readonly double[][] trainingData;
        private readonly double[][] targetValue; // {0,1,2} -> {{1,0,0},{0,1,0},{0,0,1}}
		private double learningRate;
		private int hiddenLayerNum;
		private int hiddenLayerNeuronNum;
		private int outputNeuronNum;

		private NeuronLayer hiddenLayer;
		private NeuronLayer outputLayer;


		public NeuralNet(double[][] trainingInput, int[] trainingTarget, double learningRate, int hiddenLayerNum = 1, int hiddenLayerNeuronNum = 2)
		{
			this.trainingData = trainingInput;
            this.targetValue = new double[trainingInput.Length][];
			this.learningRate = learningRate;
			this.hiddenLayerNum = hiddenLayerNum;
			this.hiddenLayerNeuronNum = hiddenLayerNeuronNum;

            HashSet<int> targetSet= new HashSet<int>(trainingTarget);
            for (int i = 0; i < trainingTarget.Length; i++)
            {
                double [] eachTarget = new double[targetSet.Count];
                eachTarget[trainingTarget[i]] = 1;
                targetValue[i] = eachTarget;
            }

            this.outputNeuronNum = targetSet.Count;

			hiddenLayer = new NeuronLayer(this.hiddenLayerNeuronNum, trainingInput[0].Length);
			outputLayer = new NeuronLayer(outputNeuronNum, this.hiddenLayerNeuronNum); // single output
		}

		public void TrainingProcess(bool isDebug = false)
		{
			int iterationNum = 0;
			double MSE = 0;
			while (true)
			{
                iterationNum++;
				for (int indexData = 0; indexData < trainingData.Length; indexData++)
				{
					string debugStr = "";
					if (isDebug)
					{
						debugStr = String.Format("Input data : {0,-13} | target : {1,-10} | output: ", string.Join(",", trainingData[indexData]), string.Join(",", targetValue[indexData]));
					}

					// forward
					double[] HlayerOutputs = new double[hiddenLayer.NeuronList.Count];
					for (int i = 0; i < hiddenLayer.NeuronList.Count; i++)
					{
						double output = hiddenLayer.NeuronList[i].UpdateInOutPut(trainingData[indexData]);
						HlayerOutputs[i] = output;
					}

					double[] OlayerOutputs = new double[outputLayer.NeuronList.Count];
					for (int i = 0; i < outputLayer.NeuronList.Count; i++)
					{
						double output = outputLayer.NeuronList[i].UpdateInOutPut(HlayerOutputs);
						OlayerOutputs[i] = output;
						if (isDebug)
						{
							debugStr += String.Format("{0:0.00} ", output);
						}
					}   

					// backpropagation
					double[] deltaO = new double[outputLayer.NeuronList.Count];
					for (int i = 0; i < outputLayer.NeuronList.Count; i++)
					{
						double o = OlayerOutputs[i];
						double t = targetValue[indexData][i];
						deltaO[i] = o * (1 - o) * (o - t);
					}

					double[] deltaH = new double[hiddenLayer.NeuronList.Count];
					for (int i = 0; i < hiddenLayer.NeuronList.Count; i++)
					{
						double deltaTemp = 0;
						for (int j = 0; j < outputLayer.NeuronList.Count; j++)
						{
							deltaTemp += deltaO[j] * outputLayer.NeuronList[j].Weight[i];
						}
						double o = HlayerOutputs[i];
						deltaH[i] = o * (1 - o) * deltaTemp;
					}

					// update w
					for (int i = 0; i < outputLayer.NeuronList.Count; i++)
					{
						outputLayer.NeuronList[i].UpdateWeight(deltaO[i], this.learningRate);
					}

					for (int i = 0; i < hiddenLayer.NeuronList.Count; i++)
					{
						hiddenLayer.NeuronList[i].UpdateWeight(deltaH[i], this.learningRate);
					}

					// Mean squared error
					double MSEI = 0;
					for (int i = 0; i < OlayerOutputs.Length; i++)
					{
						double delta = (OlayerOutputs[i] - targetValue[indexData][i]);
						MSEI += delta * delta;
					}
					MSEI = MSEI / OlayerOutputs.Length;
					MSE += MSEI;
					if (isDebug)
					{
						debugStr += String.Format("MSEI : {0:0.0000} ", MSEI);
                        Console.WriteLine(debugStr);
					}
                    

				}
				MSE = MSE / trainingData.Length;

				if (isDebug)
				{
                    Console.WriteLine("MSE : " + MSE);
				}

                if (iterationNum % 500 == 0)
                {
                    Console.WriteLine("iterationNum : {0}, MSE : {1:0.0000}", iterationNum, MSE);
                    //Console.Read();
                }

                if (MSE < 0.02 || iterationNum == 50000)
                {
                    Console.WriteLine("iterationNum : {0}, MSE : {1:0.0000}, Training  Process Finish", iterationNum, MSE);
                    break;
                }

				MSE = 0;
				
			}
		}

        public int Classify(double[] input)
        {
            double[] HlayerOutputs = new double[hiddenLayer.NeuronList.Count];
            for (int i = 0; i < hiddenLayer.NeuronList.Count; i++)
            {
                double output = hiddenLayer.NeuronList[i].UpdateInOutPut(input);
                HlayerOutputs[i] = output;
            }

            double[] OlayerOutputs = new double[outputLayer.NeuronList.Count];
            for (int i = 0; i < outputLayer.NeuronList.Count; i++)
            {
                double output = outputLayer.NeuronList[i].UpdateInOutPut(HlayerOutputs);
                OlayerOutputs[i] = output;
            }

            int maxIndex = 0;
            double max = Double.MinValue;
            for (int i = 0; i < OlayerOutputs.Length; i++)
            {
                if (max < OlayerOutputs[i])
                {
                    maxIndex = i;
                    max = OlayerOutputs[i];
                }
            }

            return maxIndex;
        }
	}
}

