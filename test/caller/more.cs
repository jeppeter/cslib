using System;
using System.Reflection;
using System.Diagnostics;


namespace Call5
{
	public class CC
	{
		public static string cc_call(int num)
		{
			StackTrace stk = new StackTrace();
			MethodBase info = stk.GetFrame(num).GetMethod();
			return string.Format(
			                 "{0}.{1}.{2}()",
			                 info.ReflectedType.Namespace,
			                 info.ReflectedType.Name,
			                 info.Name);
		}

	}	
}

namespace Callc2
{
	public class Callcc
	{

		private static string cc_call1(int num)
		{
			return Call5.CC.cc_call(num);
		}

		private static string cc_call2(int num)
		{
			return cc_call1(num);
		}

		private static string cc_call3(int num)
		{
			return cc_call2(num);
		}


		public static void Main(string[] args)
		{
			Console.Out.WriteLine("[0]{0}",cc_call3(0));
			Console.Out.WriteLine("[1]{0}",cc_call3(1));
			Console.Out.WriteLine("[2]{0}",cc_call3(2));
			Console.Out.WriteLine("[3]{0}",cc_call3(3));
			Console.Out.WriteLine("[4]{0}",cc_call3(4));
			return;
		}
	}
}

