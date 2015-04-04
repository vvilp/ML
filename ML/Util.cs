using System;

namespace ML
{
	public static class Util
	{
		static Random random;
		public static double RandomDouble(double min, double max)
		{
			if (random == null) {
				random = new Random();
			}
			return random.NextDouble() * (max - min) + min;
		}
	}
}

