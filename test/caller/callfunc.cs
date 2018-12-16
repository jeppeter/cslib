using System;
using System.Diagnostics;
using System.Reflection;


namespace CallAble
{
	public class ParseException : Exception
	{
		public ParseException(string str) : base(str)
		{
		}
	}

	public class CallAble
	{
		public CallAble()
		{

		}

		private void __throw_exception(string s)
		{
			throw new ParseException(s);
		}
		private MethodBase _check_funcname(Type tinfo,string funcname, params  object[] args)
		{
			MethodBase meth;
			MethodInfo[] infos;
			ParameterInfo[] paraminfos;		
			infos = tinfo.GetMethods();
			foreach (var i in infos) {
				Console.Out.WriteLine("{0}", i);
			}
			meth = tinfo.GetMethod(funcname, BindingFlags.Static | BindingFlags.Public);
			if (meth == null) {
				return null;
			}
			Console.Out.WriteLine("find function {0}", funcname);
			paraminfos = meth.GetParameters();
			foreach(var i in paraminfos) {
				Console.Out.WriteLine("param[{0}]",i);
			}
			foreach (var i in args) 
			{
				Console.Out.WriteLine("args[{0}]", i.GetType());
			}
			return meth;
		}

		private MethodBase _call_func_inner(string dllname, string nspc, string clsname,string fname, params object[] args)
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
					curtype = curbase.DeclaringType;
					Console.Out.WriteLine("curbase [{0}] curtype [{1}] ", curbase, curtype);
					curtype = Type.GetType(curtype.FullName);
					Console.Out.WriteLine("curtype [{0}]", curtype);
					meth = this._check_funcname(curtype,fname, args);
					if (meth != null) {
						return meth;
					}
				}
			} else if (dllname.Length <= 0 && 
				nspc.Length <= 0) {
				/*now to get */
				curtype = Type.GetType(clsname);
				if (curtype == null) {
					return null;
				}
				meth = this._check_funcname(curtype, fname, args);
				if (meth != null) {
					return meth;
				}
			} else if (dllname.Length <= 0) {
				bindname = String.Format("{0}.{1}", nspc, clsname);
				curtype = Type.GetType(bindname);
				meth = this._check_funcname(curtype, fname, args);
				if (meth != null) {
					return meth;
				}
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
			}
			return null;
		}

		public object call_func(string funcname, params object[] args)
		{
			string[] sarr;
			string namespc = "";
			MethodBase meth = null;
			int i;
			if (funcname.Length == 0) {
				this.__throw_exception(String.Format("null funcname can not accept"));
			}

			sarr = funcname.Split('.');
			if (sarr.Length == 1) {
				meth = this._call_func_inner("","","",funcname,args);
			} else if (sarr.Length == 2) {
				/*this is the class name and function name*/
				meth = this._call_func_inner("","",sarr[0],sarr[1],args);
			} else if (sarr.Length == 3) {
				/*this is the namespace name and class name and function name*/
				meth = this._call_func_inner("",sarr[0],sarr[1],sarr[2],args);
			} else if (sarr.Length == 4) {
				meth = this._call_func_inner(sarr[0],sarr[1],sarr[2],sarr[3],args);
				if (meth == null) {
					namespc = String.Format("{0}.{1}", sarr[0],sarr[1]);
					meth = this._call_func_inner("",namespc, sarr[2],sarr[3],args);
				}
			} else {
				namespc =  "";
				for (i=1; i < (sarr.Length - 2) ;i ++) {
					if (namespc.Length > 0) {
						namespc += ".";
					}
					namespc += sarr[i];					
				}

				meth = this._call_func_inner(sarr[0], namespc, sarr[sarr.Length - 2], sarr[sarr.Length - 1]);
				if (meth == null) {
					i = 0;
					namespc = "";
					for (i = 0; i < (sarr.Length - 2) ;i ++) {
						if (namespc.Length > 0) {
							namespc += ".";
						}
						namespc += sarr[i];					
					}
					meth = this._call_func_inner("", namespc, sarr[sarr.Length - 2], sarr[sarr.Length - 1]);
				}
			}

			if (meth == null) {
				this.__throw_exception(String.Format("can not find [{0}] method", funcname));
			}
			Console.Out.WriteLine("will invoke function {0}", funcname);
			for(i=0;i<args.Length;i++){
				Console.Out.WriteLine("{0}={1}",i,args[i]);
			}
			return meth.Invoke(null,args);
		}

		public static string string_function(string fmtstr, params object[] args)
		//public static string string_function(string fmtstr)
		{
			Console.Out.WriteLine("call string_function all");
			return String.Format(fmtstr,args);
			//return fmtstr;
		}

		public static void Main(string[] args)
		{
			string s;
			CallAble clb;
			clb = new CallAble();
			s = (string)clb.call_func("string_function", "cc {0}", "www");
			//s = CallAble.string_function("cc {0} {1}", "www","w322");
			Console.Out.WriteLine(s);
			return;
		}
	}
}