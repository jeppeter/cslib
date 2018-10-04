using System;
using System.Collections.Generic;
using System.Linq;


namespace LinqTst
{
	class LQ
	{
		public string Name {get;set;}
		public int Age{get;set;}
		public string PhoneNumber{get;set;}
		public LQ(string name, int age,string pn)
		{
			this.Name = name;
			this.Age = age;
			this.PhoneNumber = pn;
		}
		public override  string ToString()
		{
			return String.Format("Name:{0},Age:{1},PhoneNumber:{2}", this.Name,this.Age,this.PhoneNumber);
		}
	}

	public class LinqTst
	{
		public static void Main(string[] args)
		{
			List<LQ> strlst  = new List<LQ>();
			strlst.Add(new LQ("cc",32,"3022"));
			strlst.Add(new LQ("bb",30,"220322"));
			strlst.Add(new LQ("e2", 50,"2332s"));
			var find = from data in strlst  where  data.Age  < 35  select data;
			foreach (LQ curq in find)
			{
				Console.WriteLine("{0}", curq);
			}	
			return;
		}
	}
}