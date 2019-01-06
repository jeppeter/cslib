using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;



namespace Constr
{
	public class CC
	{
		private int m_n;
		private string m_s;
		public CC(int n=52)
		{
			this.m_s = "";
			this.m_n = n;
		}

		public CC(string m="e2")
		{
			this.m_s = m;
			this.m_n = 0;
		}

		public CC(string[] m)
		{
			if (m.Length > 0) {
				this.m_s = m[0];
			}
			this.m_n = 0;
		}

		public CC(string cc="ss",int n=30)
		{
			this.m_s = cc;
			this.m_n = n;
		}

		public string Get()
		{
			return String.Format("s [{0}] n [{1}]", this.m_s,this.m_n);
		}
	}

	public interface _NC
	{
		int Count();
		void Add(object o);
		void Clear();
		object[] ToArray();
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

		object[] _NC.ToArray()
		{
			return this.m_list.ToArray() as object[];
		}
	}

	public class Constr
	{
		public Constr()
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

		public static void Main(string[] args)
		{
			Constr c = new Constr();
			_NC arrlist = c.MakeArrays("Constr.CC");
			int i;
			CC[] cclist;
			arrlist.Add(new CC(53));
			arrlist.Add(new CC("w2"));
			cclist = (CC[]) arrlist.ToArray();
			for (i=0; i < cclist.Length ;i ++) {
				Console.Out.WriteLine("[{0}]=[{1}]", i, cclist[i].Get());
			}
			return;
		}
	}
}