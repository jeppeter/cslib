using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;


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

	public class Constr
	{
		public static void Main(string[] args)
		{
			Type ct = Type.GetType("Constr.CC");
			string s;
			int idx,jdx,i;
			ConstructorInfo finfo = null;
			CC c;
			object [] paramcc;
	     	foreach (ConstructorInfo item in ct.GetConstructors())
	        {	
	        	s = String.Format("name [{0}](", item.Name);
	        	idx = 0;
	        	jdx = 0;
	            foreach(ParameterInfo pinfo in item.GetParameters()) 
	            {
	            	if (idx >0 ) {
	            		s += ",";
	            	}
	            	s += pinfo.ParameterType.FullName;
	            	s += " ";
	            	s += pinfo.Name;

	            	if (pinfo.HasDefaultValue) {
	            		s += " = ";
	            		s += pinfo.DefaultValue.ToString();
	            	} else {
	            		jdx ++;
	            	}
	            	idx ++;
	            }
	            s += ")";
	            Console.Out.WriteLine(s);

	            if (jdx == 0) {
	            	finfo = item;
	            	paramcc = new object[idx];
	            	i = 0;
	            	foreach(ParameterInfo pinfo in item.GetParameters()) 
	            	{
	            		paramcc[i] = (object)pinfo.DefaultValue;
	            		i ++;
	            	}
	            	c = (CC) finfo.Invoke(paramcc);
	            	Console.Out.WriteLine("{0}",c.Get());
	            }
	        }
	        return ;
		}
	}
}