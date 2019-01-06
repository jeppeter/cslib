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

	public class CCN
	{
		private string m_s;
		public CCN()
		{
			this.m_s = "";
		}

		public CCN(string c)
		{
			this.m_s = c;
		}

		public string Get()
		{
			return this.m_s;
		}
	}

	public class CCAN : CCN
	{
		public CCAN() : base()
		{

		}

		public CCAN(string c) : base(c)
		{

		}
	}

	public class StaticClass
	{
		public static string get_ccn(string fmtstr,params CCN[] args)
		{
			string s;
			CCN c;
			int i;
			s = "";
			s += fmtstr;
			for(i=0;i< args.Length; i++) {
				c = args[i];
				s += " [";
				s += c.Get();
				s += "]";
			}		
			return s;
		}
	}

	public class CC
	{
		public static string format_string(string fmtstr, params object[] args)
		{
			return String.Format(fmtstr,args);
		}
	}

	public class CallAble
	{


		private interface _NC
		{
			int Count();
			void Add(object o);
			void Clear();
			object[] ToArray();
		}

		private class _NList<T> : _NC
		{
			private List<T> m_list = new List<T>();

			int _NC.Count()
			{
				return this.m_list.Count;
			}

			void _NC.Add(object o)
			{
				if (o is T) {
					this.m_list.Add((T)o);
				} else {
					throw new ParseException(String.Format("not type of  [{0}]", typeof(T).FullName));
				}
			}

			void _NC.Clear()
			{
				this.m_list.Clear();
			}

			object[] _NC.ToArray()
			{
				return this.m_list.ToArray() as object[];
			}
		}

		public CallAble()
		{

		}

		private void __add_object(_NC arrlist,params object[] oarr)
		{
			int i;
			if (oarr.Length > 0) {
				for (i=0;i < oarr.Length;i++) {
					arrlist.Add(oarr[i]);
				}
			}
			return;
		}

		private _NC __make_arrays(string typename)
		{
			Type t = Type.GetType(typename);
			Type[] typeargs = {t};
			Type tlist = typeof(_NList<>);
			Type me = tlist.MakeGenericType(typeargs);
			_NC arrlist = Activator.CreateInstance(me) as _NC;
			if (arrlist == null) {
				throw new ParseException(String.Format("can not create generic for [{0}]", typename));
			}
			return arrlist;
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
			Type lasttype;
			int i,j;
			bool bsucc;
			_NC cc;
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
				lasttype = lastparam.ParameterType.GetElementType();
				lastargs = new object[(args.Length - paraminfos.Length+1)];
				newargs[(paraminfos.Length - 1)] = (object)lastargs;
				for (i=(paraminfos.Length - 1),j=0;i<args.Length;i++,j ++) {
					if (!(args[i].GetType().IsSubclassOf(lasttype) || args[i].GetType().Equals(lasttype))) {
						this.__throw_exception(String.Format("[{0}] not subclass of [{1}] [{2}]", i, lastparam.ParameterType.Name,args[i].GetType().Name));
					}
					lastargs[j] = (object)args[i];
				}
			} else if (args.Length < paraminfos.Length) {
				newargs = new object[paraminfos.Length];
				for (i=0; i < args.Length; i++) {
					if (!(args[i].GetType().IsSubclassOf(paraminfos[i].ParameterType) || args[i].GetType().Equals(paraminfos[i].ParameterType))) {
						this.__throw_exception(String.Format("[{0}] not subclass of [{1}]", i, paraminfos[i].ParameterType.Name));
					}
					newargs[i] = args[i];
				}
				for (i=args.Length;i < paraminfos.Length ;i ++) {
					bsucc = false;
					if (!paraminfos[i].HasDefaultValue){
						if (i==(paraminfos.Length - 1)) {
							if (paraminfos[i].ParameterType.IsArray) {
								bsucc = true;
								cc = this.__make_arrays(paraminfos[i].ParameterType.GetElementType().FullName);
								newargs[i] = (object) Convert.ChangeType(cc.ToArray(),paraminfos[i].ParameterType);
							}
						}
					} else {
						bsucc = true;
						newargs[i] = paraminfos[i].DefaultValue;
					}
					if (!bsucc) {
						this.__throw_exception(String.Format("[{0}] param not default", i));
					}
					
				}
			} else {
				/*that is equal*/
				newargs = args;
				for (i=0;i< args.Length ;i ++) {
					if (! (args[i].GetType().IsSubclassOf(paraminfos[i].ParameterType) || args[i].GetType().Equals(paraminfos[i].ParameterType) )) {
						bsucc = false;
						if (i == (args.Length - 1) && paraminfos[i].ParameterType.IsArray) {
							if (args[i].GetType().IsSubclassOf(paraminfos[i].ParameterType.GetElementType()) || 
								args[i].GetType().Equals(paraminfos[i].ParameterType.GetElementType())) {
								bsucc = true;
								cc = this.__make_arrays(paraminfos[i].ParameterType.GetElementType().FullName);
								this.__add_object(cc,args[i]);
								newargs[i] = (object) Convert.ChangeType(cc.ToArray(),paraminfos[i].ParameterType);
							}
						}
						if (! bsucc) {
							this.__throw_exception(String.Format("[{0}] not subclass of [{1}] [{2}]", i, paraminfos[i].ParameterType.Name, args[i].GetType().Name));	
						}
						
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
				if (curmeth.Name == funcname) {
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
			int i,j;
			StackFrame frm;
			StackTrace stk;
			MethodBase curbase;
			MethodBase meth;
			Type curtype;
			string bindname;
			Assembly asbl;
			string dl;
			string[] sarr;
			if (dllname.Length <= 0 && 
				nspc.Length <= 0 && 
				clsname.Length <= 0) {
				/*this Method by call function*/
				stk = new StackTrace();
				for (i=0;i<stk.FrameCount;i++) {
					frm = stk.GetFrame(i);
					curbase = frm.GetMethod();
					curtype = curbase.DeclaringType;
					curtype = Type.GetType(curtype.FullName);
					meth = this._check_funcname(curtype,fname, args);
					if (meth != null) {
						return meth;
					}
				}
			} else if (dllname.Length <= 0 && 
				nspc.Length <= 0) {
				stk = new StackTrace();
				for (i=0;i < stk.FrameCount;i++) {
					frm = stk.GetFrame(i);
					curbase = frm.GetMethod();
					curtype = curbase.DeclaringType;
					sarr = curtype.FullName.Split('.');
					nspc = "";
					for (j=0;j < (sarr.Length - 1); j++) {
						if (nspc.Length > 0) {
							nspc += ".";
						}
						nspc += sarr[j];
					}
					curtype = Type.GetType(String.Format("{0}.{1}", nspc,clsname));
					if (curtype == null) {
						continue;
					}
					meth = this._check_funcname(curtype, fname, args);
					if (meth != null) {
						return meth;
					}
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
			int i,j;
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
			for (i=0; i < newargs.Length ;i ++) {

				Console.Out.WriteLine("[{0}] type [{1}]", i, newargs[i].GetType().FullName);
				if (newargs[i].GetType().IsArray) {
					object[] c = (object[]) newargs[i];
					for (j=0; j < c.Length; j++) {
						Console.Out.WriteLine("    [{0}] type [{1}]", j, c[j].GetType().FullName);
					}
				}
			}
			return meth.Invoke(null,newargs);
		}

		public static string string_function(string fmtstr, params object[] args)
		{
			Console.Out.WriteLine("call string_function all");
			return String.Format(fmtstr,args);
		}

		public static string string_default_function(string fmtstr,string defname = "cc")
		{
			return fmtstr + " name " + defname;
		}

		public static void Main(string[] args)
		{
			string s;
			CallAble clb;
			clb = new CallAble();
			s = (string)clb.call_func("string_function", "cc {0} {1}", "www", "w322");
			Console.Out.WriteLine(s);
			s = (string)clb.call_func("StaticClass.get_ccn", "sswww", new CCN("www"), new CCAN("ww222"));
			//s = StaticClass.get_ccn("sswww", new CCN("www"), new CCAN("ww222"));
			Console.Out.WriteLine(s);
			//s = (string)clb.call_func("string_function", "cc WW");
			//s = (string)clb.call_func("string_function", "cc {0}", "www");
			//s = (string)clb.call_func("CC.format_string", "cc {0}", "www");
			//s = (string)clb.call_func("CC.format_string", "cc {0}", "www");
			//s = (string) clb.call_func("Call2.Call2.cc_call","cc {0}", "cca2");
			//s = (string) clb.call_func("calldll.CallDll.CallDll.cc_call","cc {0}", "cca2");
			//Console.Out.WriteLine(s);
			s = (string)clb.call_func("string_default_function", "bbs");
			//s = CallAble.string_default_function("bbs");
			Console.Out.WriteLine(s);
			return;
		}
	}
}