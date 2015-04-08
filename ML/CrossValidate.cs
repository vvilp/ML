using System;
using System.Collections.Generic;

namespace ML
{
    public class CrossValidate
    {
        private Data data;
        private double trainingSize;
        private int validationRounds;
        private int foldsNum;
        public CrossValidate(double trainingSize, int validationRounds, int foldsNum)
        {
            data = new Data();
            this.trainingSize = trainingSize;
            this.validationRounds = validationRounds;
            this.foldsNum = foldsNum;
        }

        public void ReadData(string path, char splitChar)
        {
            data.ReadFile(path, splitChar);
        }

        public void AddTraningModel()
        {

        }

        public void StartValidation()
        {
            //double avgCorrectRate = 0;
            for (int ri = 0; ri < validationRounds; ri++)
            {
                //data.SplitData(trainingSize);
                double[][] trainingInput = data.Input;
                int[] trainingTarget = data.Target;

                NeuralNet nn = new NeuralNet(trainingInput, trainingTarget, 0.005, 20);
                nn.TrainingProcess(false);


                List<List<int>> indexList = data.GetIndexInFolds(foldsNum);

                foreach (List<int> indexs in indexList)
                {
                    int correctNum = 0;
                    foreach (int index in indexs)
                    {
                        int result = nn.Classify(trainingInput[index]);
                        if (result == trainingTarget[index])
                        {
                            correctNum++;
                        }
                        //Console.WriteLine("Test input:{0} | Target class:{1} | OutPut class:{2}", string.Join(",", trainingInput[index]),trainingTarget[index], result);
                    }
                    double CorrectRate = (double)correctNum / (double)indexs.Count;
                    Console.WriteLine("CorrectRate:" + CorrectRate);
                }

//                double[][] testInput = data.GetTestInput();
//                int[] testTarget = data.GetTestTarget();
//                int correctNum = 0;
//                for (int i = 0; i < testInput.Length; i++)
//                {
//                    int result = nn.Classify(testInput[i]);
//                    //Console.WriteLine("Test input:{0} | Target class:{1} | OutPut class:{2}", string.Join(",", testInput[i]),testTarget[i], result);
//                    if (result == testTarget[i])
//                    {
//                        correctNum++;
//                    }
//                }
//
//                double CorrectRate = (double)correctNum / (double)testInput.Length;
//                Console.WriteLine("CorrectRate:" + CorrectRate);
//                avgCorrectRate += CorrectRate;
            }
//            avgCorrectRate = avgCorrectRate / validationRounds;
//            Console.WriteLine("Average Correct Rate:" + avgCorrectRate);
        }
    }
}

