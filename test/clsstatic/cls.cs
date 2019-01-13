using System;
using System.Collections.Generic;


namespace clsstatic
{
	public class clsstatic
	{
		static Dictionary<string,object> m_dict = new Dictionary<string, object>
		{
			{"prog", "cls"},
			{"usage", ""},
			{"helphandler" , null}
		};

		public static void Main(string[] args)
		{
			int i;
			string c;
			object val;
			for (i=0;i<args.Length;i++) {
				c = args[i];
				if (m_dict.TryGetValue(c,out val)) {
					Console.Out.WriteLine("[{0}]={1}", c, val);
				} else {
					Console.Out.WriteLine("[{0}] no value", c);
				}
			}
			return;
		}
	}
}