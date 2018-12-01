using System;


namespace CallAble
{
	public class CallAble
	{
		private MethodInfo _call_func_inner(string dllname, string nspc, string clsname,string fname)
		{
			int i;
			if (dllname.Length <= 0 && 
				nspc.Length <= 0 && 
				clsname.Length <= 0) {
				/*this Method by call function*/
				StackTrace stk = new StackTrace();

			}
		}

		public object call_func(string funcname, params object[] args)
		{
			string[] sarr;
			string fn = "";
			string cls = "";
			string namespc = "";
			MethodInfo meth = null;
			sarr = String.Split(".", funcname);
			if (sarr.Length == 1) {
				meth = this._call_func_inner("","","",funcname);
			} else if (sarr.Length == 2) {
				/*this is the class name and function name*/
				meth = this._call_func_inner("","",sarr[0],sarr[1]);
			} else if (sarr.Length == 3) {
				/*this is the namespace name and class name and function name*/
				meth = this._call_func_inner("",sarr[0],sarr[1],sarr[2]);
			} else if (sarr.Length > 3) {
				/*this may be */
			}
		}

		private static string string_function(string fmtstr, params object[] args)
		{
			return String.Format(fmtstr,args);
		}

		public static void Main(string[] args)
		{
			string s;
			s = call_func("string_function", "cc {0}", "www");
			Console.Out.WriteLine(s);
			return;
		}
	}
}