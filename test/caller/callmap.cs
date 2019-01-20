using System;
using System.Collections.Generic;

namespace calldelegate
{
	public class DeleCall
	{
		private delegate string CallFunc(string val);
		private Dictionary<string,CallFunc> m_calldict;

		private string Call1(string n)
		{
			string rets = String.Format("Call1 {0}", n);
			Console.Out.WriteLine(rets);
			return rets;
		}

		private string Call2(string n)
		{
			string rets = String.Format("Call2 {0}", n);
			Console.Out.WriteLine(rets);
			return rets;
		}

		private string Call3(string n)
		{
			string rets = String.Format("Call3 {0}", n);
			Console.Out.WriteLine(rets);
			return rets;
		}

		public DeleCall()
		{
			this.m_calldict = new Dictionary<string,CallFunc>();
			this.m_calldict.Add("Call1", new CallFunc(Call1));
			this.m_calldict.Add("Call2", new CallFunc(Call2));
			this.m_calldict.Add("Call3", new CallFunc(Call3));
		}

		public string CallFunction(string n, string v)
		{
			return (string) this.m_calldict[n].DynamicInvoke(v);
		}
	}

	public class calldelegate
	{
		public static void Main(string[] args)
		{
			DeleCall dc = new DeleCall();
			string s;
			s = dc.CallFunction(args[0], args[1]);
			Console.Out.WriteLine("return [{0}]", s);
			return;
		}
	}
}