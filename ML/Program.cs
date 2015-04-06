using System;
using System.Collections.Generic;

namespace ML
{
	class MainClass
	{

		public static void Main (string[] args)
		{
            CrossValidate cv = new CrossValidate(0.7, 5);
            cv.ReadData(@"../../test_data/iris1.csv", ',');
            cv.StartValidation();
		}
	}
}
