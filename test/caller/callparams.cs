using System;
using System.Diagnostics;
using System.Reflection;


namespace callparams
{

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


	public interface _NC
	{
		int Count();
		void Add(object o);
		void Clear();
	}

	public class _NList<T> : _NC
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
				throw new Exception(String.Format("not type of  [{0}]", typeof(T).FullName));
			}
		}

		void _NC.Clear()
		{
			this.m_list.Clear();
		}

		T[] _NC.ToArray()
		{
			return this.m_list.ToArray();
		}
	}


	public class callparams
	{
		public callparams()
		{

		}

		public void AddObject(_NC arrlist,params object[] oarr)
		{
			int i;
			if (oarr.Length > 0) {
				for (i=0;i < oarr.Length;i++) {
					arrlist.Add(oarr[i]);
				}
			}
			return;
		}

		public _NC MakeArrays(string typename)
		{
			Type t = Type.GetType(typename);
			Type[] typeargs = {t};
			Type tlist = typeof(_NList<>);
			Type me = tlist.MakeGenericType(typeargs);
			_NC arrlist = Activator.CreateInstance(me) as _NC;
			if (arrlist == null) {
				throw new Exception(String.Format("can not create generic for [{0}]", typename));
			}
			return arrlist;
		}
		public static void Main(string[] objs)
		{
			object[] args;
			CCN[] cc;
			Type tp = Type.GetType("callparams.StaticClass");
			MethodInfo minfo;
			string s;
			callparams cs = new callparams();
			

			args=  new object[2];
			args[0] = (string) "ccs";
			cc = new CCN[3];
			cc[0] = new CCN("222");
			cc[1] = new CCAN("32221");
			cc[2] = new CCN("wweew");
			args[1] = (object) cc;
			minfo = tp.GetMethod("get_ccn");
			s = (string) minfo.Invoke(null,args);
			Console.Out.WriteLine(s);
			return;
		}
	}
}