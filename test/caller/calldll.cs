using System;


namespace CallDll
{
	public class CallDll
	{
		public static string cc_call(string fmtstr , params object[] args)
		{
			return String.Format(fmtstr,args);
		}
	}
}