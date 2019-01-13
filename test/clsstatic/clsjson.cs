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
			JToken tok = JToken.Parse(s);
			Dictionary<string,JToken> dtok;
			Dictionary<string, object> retdict = new Dictionary<string,object>();
			List<string> keys;
			string k;
			object v;
			JToken tokv;
			JValue jval;
			string valtype;
			int i;
			dtok = tok.ToObject<Dictionary<string, JToken>>();
			keys = new List<string>(dotk.Keys);
			for (i=0; i < keys.Count; i++) {
				k = keys[i];
				tokv = dtok[k];

				valtype = tokv.GetType().FullName;
				if (valtype == "Newtonsoft.Json.Linq.JValue") {
					jval = (JValue) tokv;
					switch (jval.Type) {
						case JTokenType.Integer:
							v = (object)((System.Int64) jval.Value);
							break;
						case JTokenType.Float:
							v = (object)((System.Double) jval.Value);
							break;
						case JTokenType.String:
							v = (object)((System.String) jval.Value);
							break;
						case JTokenType.Null:
							v = (object)null;
							break;
						case JTokenType.Boolean:
							v = (object)((System.Boolean) jval.Value);
							break;
						default:
							throw new Exception(String.Format("value type [{0}] not supported", jval.Type));
					}
				} else  {
					throw new Exception(String.Format("not supported type [{0}]", valtype));
				}
				retdict[k] = v;
			}
			return retdict;
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