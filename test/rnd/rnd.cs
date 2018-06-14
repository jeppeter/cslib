using System;

namespace consoleapp
{
	public class consoleapp 
	{
		public static void Main(string[] args)
		{
			TimeSpan t = (DateTime.UtcNow - new DateTime(1970,1,1));
			Random rnd = new Random((int)t.TotalSeconds);
			int i;
			int ita=3;

			if (args.Length > 0) {
				ita = int.Parse(args[0]);
			}
			for (i=0;i<ita;i++) {
				Console.WriteLine("[{0}]={1}", i, rnd.Next(1024));
			}
		}
	}
}