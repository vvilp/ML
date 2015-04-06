
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    class Data
    {
        private double[][] input;
        private int[] target;
        private string[] classLabel;

        private double[][] trainingInput;
        private int[] trainingTarget;
        private string[] trainingClassLabel;

        private double[][] testInput;
        private int[] testTarget;
        private string[] testClassLabel;


        // the default class label is the last one
        public void ReadFile(string path, char splitChar)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            input = new double[lines.Length][];
            target = new int[lines.Length];
            classLabel = new string[lines.Length];
            List<string> labelList = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                string[] strArr = lines[i].Split(new char[] { splitChar });
                double[] eachData = new double[strArr.Length - 1];
                for (int k = 0; k < strArr.Length - 1; k++)
                {
                    eachData[k] = Double.Parse(strArr[k]);
                }
                input[i] = eachData;

                classLabel[i] = strArr[strArr.Length - 1];
                int labelIndex = labelList.IndexOf(strArr[strArr.Length - 1]);
                if (labelIndex == -1)
                {
                    target[i] = labelList.Count;
                    labelList.Add(strArr[strArr.Length - 1]);
                }
                else
                {
                    target[i] = labelIndex;
                }
            }

            Console.WriteLine("Total data num : " + lines.Length);
        }

        private void TestDuplication(List<int> index)
        {
            List<int> test = new List<int>();
            foreach (int d in index)
            {
                if (test.IndexOf(d) != -1)
                {
                    Console.WriteLine("{0} Has Duplication at {1}", d, test.IndexOf(d));
                }
                test.Add(d);
            }
        }

        //trainingPercentage : 0.8 -> test : 0.2
        public void SplitData(double trainingPercentage)
        {
            if (input == null || target == null || classLabel == null)
            {
                return;
            }

            List<int> indexList = new List<int>(Enumerable.Range(0, input.Length).ToList());
            List<int> trainingIndexList = new List<int>();

            int traningCount = (int)((double)indexList.Count * trainingPercentage);
            int testCount = indexList.Count - traningCount;

            for (int i = 0; i < traningCount; i++)
            {
                int randIndex = Util.RandomInt(0, indexList.Count);
                trainingIndexList.Add(indexList[randIndex]);
                indexList.Remove(indexList[randIndex]);
            }
            //TestDuplication(trainingIndexList);
            Console.WriteLine("Train data Num : " + trainingIndexList.Count);

            List<int> testIndexList = indexList;
            Console.WriteLine("Test data Num : " + testIndexList.Count);

            trainingInput = new double[traningCount][];
            trainingTarget = new int[traningCount];
            trainingClassLabel = new string[traningCount];

            testInput = new double[testCount][];
            testTarget = new int[testCount];
            testClassLabel = new string[testCount];

            for (int i = 0; i < trainingIndexList.Count; i++)
            {
                trainingInput[i] = input[trainingIndexList[i]];
                trainingTarget[i] = target[trainingIndexList[i]];
                trainingClassLabel[i] = classLabel[trainingIndexList[i]];
            }

            for (int i = 0; i < testIndexList.Count; i++)
            {
                testInput[i] = input[testIndexList[i]];
                testTarget[i] = target[testIndexList[i]];
                testClassLabel[i] = classLabel[testIndexList[i]];
            }

            //Console.WriteLine(string.Join(",", trainingIndexList.ToArray()));

        }

        public double[][] GetTrainingInput()
        {
            return trainingInput;
        }

        public int[] GetTrainingTarget()
        {
            return trainingTarget;
        }

        public string[] GetTrainingClassLabel()
        {
            return trainingClassLabel;
        }

        public double[][] GetTestInput()
        {
            return testInput;
        }

        public int[] GetTestTarget()
        {
            return testTarget;
        }

        public string[] GetTestClassLabel()
        {
            return testClassLabel;
        }

    }
}
