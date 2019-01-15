using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace clsstatic
{
	public class clsstatic
	{
		public static object get_token(string key,JToken tokv)
		{
			string valtype;
			JValue jval;
			object v;
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
			} else if (valtype == "Newtonsoft.Json.Linq.JArray") {
				v = (object)get_array(key, tokv.Value<JArray>());
			} else if (valtype == "Newtonsoft.Json.Linq.JObject") {
				v = (object)get_object(key, tokv.Value<JObject>());
			} else {
				throw new Exception(String.Format("not supported type [{0}]", valtype));
			}
			return v;
		}

		public static object[] get_array(string key,JArray jarr)
		{
			object[] retobj;
			int i;
			JToken tok;

			retobj = new object[jarr.Count];
			for (i=0; i < jarr.Count ;i ++) {
				tok = jarr[i];
				retobj[i] = get_token(String.Format("{0}[{1}]", key, i), tok);
			}
			return retobj;
		}

		public static Dictionary<string, object> get_object(  string key,JObject jobj)
		{
			Dictionary<string,object> retdict = new Dictionary<string,object>();
			Dictionary<string,JToken> nobj;
			List<String> keys;
			string k;
			object v;
			int i;
			nobj = jobj.ToObject<Dictionary<string,JToken>>();
			keys = new List<String>(nobj.Keys);
			for (i=0; i < keys.Count; i++) {
				k = keys[i];
				v = get_token(k, nobj[k]);
				retdict[k] = v;
			}
			return retdict;
		}

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
			keys = new List<string>(dtok.Keys);
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
				} else if (valtype == "Newtonsoft.Json.Linq.JArray") {
					v = get_array(k, tokv.Value<JArray>());
				} else if (valtype == "Newtonsoft.Json.Linq.JObject") {
					v = get_object(k, tokv.Value<JObject>());
				} else {
					throw new Exception(String.Format("not supported type [{0}]", valtype));
				}
				retdict[k] = v;
			}
			return retdict;
		}

		public static void Main(string[] args)
		{			
			return;
		}
	}
}