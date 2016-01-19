using System;
using System.Collections.Generic;

namespace Core
{
	public class Stopwatch
	{
		private static Dictionary<int,DateTime> start = new Dictionary<int,DateTime>();
	
		public static int Start()
		{
			int id = 0;
			while (start.ContainsKey(id))
			{
				id++;
			}
			
			start [id] = System.DateTime.Now;

			return id;
		}

		public static double End(int id)
		{
			if (start.ContainsKey (id))
			{
				TimeSpan time = System.DateTime.Now.Subtract(start[id]);
				return time.TotalMilliseconds;
			}
			return 0;
		}
	}
}