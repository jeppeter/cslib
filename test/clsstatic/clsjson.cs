using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace clsstatic
{
	public class clsstatic
	{
		public static Dictionary<string,object> parse_json(string s)
		{
			JToken tok = 
		}

		public static void Main(string[] args)
		{

			foreach(KeyValuePair<string,object> kv in clsstatic.m_defdict) {
				Console.Out.WriteLine("[{0}]=[{1}]", kv.Key, kv.Value);
			}
			return;
		}
	}
}