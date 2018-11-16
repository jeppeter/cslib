using System;


namespace VarArg
{
	public class VarArg
	{
		private static void PrintTypes(params dynamic[] args)
		{
		    foreach (var arg in args)
		    {
		        Console.WriteLine("[{0}][{1}]",arg.GetType(), arg);
		    }
		}

		static void Main(string[] args)
		{
		    PrintTypes(1,1.0,"hello");
		    //Console.ReadKey();
		}
	}
}