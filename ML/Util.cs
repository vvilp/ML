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

		public static void WriteArray<T>(T[] array) 
		{
			for (int i = 0; i < array.Length; i++) {
				if (array [i].GetType () == new Double ().GetType ()) {
					Console.Write ("{0:0.00} ", array [i]);
				} else {
					Console.Write (array [i] + " ");
				}
			}
		}
	}
}

