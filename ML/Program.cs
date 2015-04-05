using System;
using System.Collections.Generic;

namespace ML
{
	class MainClass
	{
		static void NeuralnetworkTest1()
		{
			double[][] inputArr = new double[][]{new double[]{0,0}, new double[]{0,1}, new double[]{1, 0}, new double[]{1,1}};
			double[][] targetArr = new double[][]{new double[]{1,0},new double[]{0,1},new double[]{0,1},new double[]{1,0}};
			NeuralNet nn = new NeuralNet (inputArr, targetArr, 0.5, 1);
			nn.TrainingProcess ();
		}

		static void NeuralnetworkTest2()
		{
			string[] lines = System.IO.File.ReadAllLines(@"../../test_data/test5.txt");

			double[][] inputArr = new double[lines.Length][];
			double[][] targetArr = new double[lines.Length][];

			List<string> labelList = new List<string> ();

			for (int i = 0; i < lines.Length; i++) {
				string [] strArr = lines [i].Split (new char[]{'\t'});
				double [] eachData = new double[strArr.Length - 1];
				for (int k= 0; k < strArr.Length - 1; k++) {
					eachData [k] = Double.Parse(strArr [k]);
				}
				inputArr [i] = eachData;

				int labelIndex = labelList.IndexOf(strArr[strArr.Length - 1]);
				if (labelIndex == -1) {
					labelList.Add(strArr[strArr.Length - 1]);
				}
			}

			for (int i = 0; i < lines.Length; i++) {
				string [] strArr = lines [i].Split (new char[]{'\t'});
				int labelIndex = labelList.IndexOf(strArr[strArr.Length - 1]);
				double [] eachTarget = new double[labelList.Count];
				eachTarget[labelIndex] = 1;
				targetArr [i] = eachTarget;
			}

			NeuralNet nn = new NeuralNet (inputArr, targetArr, 0.05, 20);
			nn.TrainingProcess (true);
		}

		public static void Main (string[] args)
		{

			NeuralnetworkTest2 ();
			//Util.WriteArray (new double[]{0.1,0.12312321321});
		}
	}
}
