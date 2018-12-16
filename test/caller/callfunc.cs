using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;


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

		private object[] _get_param_args(object[] args,ParameterInfo[] paraminfos)
		{
			object[] newargs;
			object[] lastargs;
			ParameterInfo lastparam;
			int i,j;
			if (args.Length > paraminfos.Length) {
				newargs = new object[paraminfos.Length];
				for (i=0;i < (paraminfos.Length - 1) ; i++) {
					if (!(args[i].GetType().IsSubclassOf(paraminfos[i].ParameterType) || args[i].GetType().Equals(paraminfos[i].ParameterType))) {
						this.__throw_exception(String.Format("[{0}] not subclass of [{1}] [{2}]", i, paraminfos[i].ParameterType.Name, args[i].GetType().Name));
					}
					newargs[i] = args[i];
				}

				lastparam = paraminfos[(paraminfos.Length - 1)];
				if (!lastparam.ParameterType.IsArray){
					this.__throw_exception(String.Format("last param not array"));
				}
				lastargs = new object[(args.Length - paraminfos.Length - 1)];
				newargs[(paraminfos.Length - 1)] = (object)lastargs;
				for (i=(paraminfos.Length - 1),j=0;i<args.Length;i++,j ++) {
					if (!(args[i].GetType().IsSubclassOf(lastparam.ParameterType) || args[i].GetType().Equals(lastparam.ParameterType))) {
						this.__throw_exception(String.Format("[{0}] not subclass of [{1}] [{2}]", i, lastparam.ParameterType.Name,args[i].GetType().Name));
					}
					lastargs[j] = args[i];
				}
			} else if (args.Length < paraminfos.Length) {
				newargs = args;
				for (i=0; i < args.Length; i++) {
					if (!(args[i].GetType().IsSubclassOf(paraminfos[i].ParameterType) || args[i].GetType().Equals(paraminfos[i].ParameterType))) {
						this.__throw_exception(String.Format("[{0}] not subclass of [{1}]", i, paraminfos[i].ParameterType.Name));
					}
				}
				for (i=args.Length;i < paraminfos.Length ;i ++) {
					if (!paraminfos[i].HasDefaultValue){
						this.__throw_exception(String.Format("[{0}] param not default", i));
					}
				}
			} else {
				/*that is equal*/
				newargs = args;
				for (i=0;i< args.Length ;i ++) {
					if (! (args[i].GetType().IsSubclassOf(paraminfos[i].ParameterType) || args[i].GetType().Equals(paraminfos[i].ParameterType) )) {
						this.__throw_exception(String.Format("[{0}] not subclass of [{1}] [{2}]", i, paraminfos[i].ParameterType.Name, args[i].GetType().Name));
					}
				}
			}
			return newargs;
		}

		private MethodBase _check_funcname(Type tinfo,string funcname, params  object[] args)
		{
			MethodInfo[] infos;
			ParameterInfo[] paraminfos;
			List<MethodInfo> okmeths = new List<MethodInfo>();
			infos = tinfo.GetMethods();
			foreach (var curmeth in infos) {
				Console.Out.WriteLine("[{0}]",curmeth.Name);
				if (curmeth.Name == funcname) {
					Console.Out.WriteLine("match [{0}]", funcname);
					paraminfos= curmeth.GetParameters();
					try {
						this._get_param_args(args,paraminfos);
						okmeths.Add(curmeth);
					}
					catch(ParseException e) {
						Console.Error.WriteLine("catch error [{0}]", e);
					}
				}
			}

			if (okmeths.Count == 0) {
				return null;
			}
			return okmeths[0];

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
			object[] newargs;
			ParameterInfo[] paraminfos;
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
			paraminfos = meth.GetParameters();
			newargs = this._get_param_args(args,paraminfos);
			return meth.Invoke(null,newargs);
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