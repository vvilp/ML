using System;

namespace ML
{
    public class CrossValidate
    {
        private Data data;
        private double trainingSize;
        private int validationRounds;

        public CrossValidate(double trainingSize, int validationRounds)
        {
            data = new Data();
            this.trainingSize = trainingSize;
            this.validationRounds = validationRounds;
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
            double avgCorrectRate = 0;
            for (int ri = 0; ri < validationRounds; ri++)
            {
                data.SplitData(trainingSize);
                double[][] trainingInput = data.GetTrainingInput();
                int[] trainingTarget = data.GetTrainingTarget();

                NeuralNet nn = new NeuralNet(trainingInput, trainingTarget, 0.005, 20);
                nn.TrainingProcess(false);

                double[][] testInput = data.GetTestInput();
                int[] testTarget = data.GetTestTarget();
                int correctNum = 0;
                for (int i = 0; i < testInput.Length; i++)
                {
                    int result = nn.Classify(testInput[i]);
                    //Console.WriteLine("Test input:{0} | Target class:{1} | OutPut class:{2}", string.Join(",", testInput[i]),testTarget[i], result);
                    if (result == testTarget[i])
                    {
                        correctNum++;
                    }
                }

                double CorrectRate = (double)correctNum / (double)testInput.Length;
                Console.WriteLine("CorrectRate:" + CorrectRate);
                avgCorrectRate += CorrectRate;
            }
            avgCorrectRate = avgCorrectRate / validationRounds;
            Console.WriteLine("Average Correct Rate:" + avgCorrectRate);
        }
    }
}

