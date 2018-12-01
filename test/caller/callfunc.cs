using System;
using System.Diagnostics;
using System.Reflection;


namespace CallAble
{
	public class CallAble
	{
		private MethodBase _check_funcname(Type tinfo,string funcname, params  object[] args)
		{
			MethodBase meth;
			meth = tinfo.GetMethod(funcname, BindingFlags.Static);
			if (meth == null) {
				return null;
			}
			return meth;
		}

		private MethodInfo _call_func_inner(string dllname, string nspc, string clsname,string fname, params object[] args)
		{
			int i;
			StackFrame frm;
			StackTrace stk;
			MethodBase curbase;
			MethodBase meth;
			Type curtype;
			string bindname;
			Assembly asbl;
			string dl;
			if (dllname.Length <= 0 && 
				nspc.Length <= 0 && 
				clsname.Length <= 0) {
				/*this Method by call function*/
				stk = new StackTrace();
				for (i=0;i<stk.FrameCount;i++) {
					frm = stk.GetFrame(i);
					curbase = frm.GetMethod();
					curtype = curbase.GetType();
					meth = this._check_funcname(fname, args);
					if (meth != null) {
						return meth;
					}
				}
				return null;
			} else if (dllname.Length <= 0 && 
				nspc.Length <= 0) {
				/*now to get */
				curtype = typeof(clsname);
				if (curtype == null) {
					return null;
				}
				meth = this._check_funcname(curtype, fname, args);
				if (meth != null) {
					return meth;
				}
				return null;
			} else if (dllname.Length <= 0) {
				bindname = String.Format("{0}.{1}", nspc, clsname);
				curtype = typeof(bindname);
				meth = this._check_funcname(curtype, fname, args);
				if (meth != null) {
					return meth;
				}
				return null;
			} else {
				bindname = String.Format("{0}.{1}", nspc, clsname);
				dl = String.Format("{0}.dll",dllname);
				asbl = Assembly.LoadFrom(dl);
				if (asbl == null) {
					return null;
				}
				curtype = asbl.GetType(bindname,false,true);
				if (curtype == null) {
					return null;
				}
				meth = this._check_funcname(curtype,fname,args);
				if (meth != null){
					return meth;
				}
				return null;
			}
			return null;
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