using System;

namespace ML
{
	public static class Util
	{
//		public Util ()
//		{
//		}

		public static double RandomDouble(double min, double max)
		{
			Random random = new Random();
			return random.NextDouble() * (max - min) + min;
		}
	}
}

