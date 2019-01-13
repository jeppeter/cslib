using System;
using System.Collections.Generic;


namespace clsstatic
{
	public class clsstatic
	{
		public static Dictionary<string,object> m_defdict = new Dictionary<string,object>{
			{"prog", Environment.GetCommandLineArgs().Length > 0 ? Environment.GetCommandLineArgs()[0] : ""},
			{"usage", ""},
			{"description", ""},
			{"epilog", ""},
			{"version","0.0.1"},
			{"errorhandler" , "exit"},
			{"helphandler", null},
			{"longprefix", "--"},
			{"shortprefix", "-"},
			{"nohelpoption", false},
			{"nojsonoption", false},
			{"helplong", "help"},
			{"helpshort", "h"},
			{"jsonlong", "json"},
			{"cmdprefixadded", true},
			{"parseall", true},
			{"screenwidth", 80},
			{"flagnochange", false}
		};

		public static void Main(string[] args)
		{
			int i;
			string c;
			object val;
			for (i=0;i<args.Length;i++) {
				c = args[i];
				if (clsstatic.m_defdict.TryGetValue(c,out val)) {
					Console.Out.WriteLine("[{0}]={1}", c, val);
				} else {
					Console.Out.WriteLine("[{0}] no value", c);
				}
			}

			foreach(KeyValuePair<string,object> kv in clsstatic.m_defdict) {
				Console.Out.WriteLine("[{0}]=[{1}]", kv.Key, kv.Value);
			}
			return;
		}
	}
}