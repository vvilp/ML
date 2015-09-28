using System;
using System.Collections.Generic;
using System.IO;


namespace ML
{
	class MainClass
	{
        static void TestPlot()
        {

        }

		public static void Main (string[] args)
		{
            CrossValidate cv = new CrossValidate(0.7, 5, 5);
            cv.ReadData(@"../../test_data/test5.txt", '\t');
            cv.StartValidation();
		}
	}
}
