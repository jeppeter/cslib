using System;

namespace CallFuncDll
{
	public class CallFuncDll
	{
		public static void CallFuncOut(string name)
		{
			Console.Out.WriteLine("name [{0}]", name);
			return;
		}

		public static string CallFuncForm(string name)
		{
			Console.Out.WriteLine("name form [{0}]", name);
			return String.Format("m_{0}", name);
		}
	}
}