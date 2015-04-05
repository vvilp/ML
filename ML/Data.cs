using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    class Data
    {
        private double[][] inputArr;
        private double[] targetArr;
        private string[] classLabel;

        // the default class label is the last one
        public void ReadData(string path, char splitChar)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            inputArr = new double[lines.Length][];
            targetArr = new double[lines.Length];
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
                inputArr[i] = eachData;

                classLabel[i] = strArr[strArr.Length - 1];
                int labelIndex = labelList.IndexOf(strArr[strArr.Length - 1]);
                if (labelIndex == -1)
                {
                    targetArr[i] = labelList.Count;
                    labelList.Add(strArr[strArr.Length - 1]);
                }
                else
                {
                    targetArr[i] = labelIndex;
                }
            }
        }

        //trainingPercentage : 0.8 -> test : 0.2
        public void SplitData(double trainingPercentage)
        {
            
        }

    }
}
