using System;


namespace gettype
{
	public class gettype
	{
		public static void Main(string[] args)
		{
			int ival = 0;
			double fval = 0.0;
			string sval = "";
			System.Int64 lval  = 0;

			Console.WriteLine("ival [{0}]", ival.GetType().FullName);
			Console.WriteLine("fval [{0}]", fval.GetType().FullName);
			Console.WriteLine("sval [{0}]", sval.GetType().FullName);
			Console.WriteLine("lval [{0}]", lval.GetType().FullName);
		}
	}
}