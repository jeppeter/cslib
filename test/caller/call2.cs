using System;

namespace Call2
{
	public class Call2
	{
		public static string cc_call(string fmtstr , params object[] args)
		{
			return String.Format(fmtstr,args);
		}
	}
}