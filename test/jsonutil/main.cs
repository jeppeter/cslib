using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;


/*****************************************
* to build just call
* dotnet build -c Release -r win7-x64
   it will give you the win7
*****************************************/

namespace jsonutil
{
class JsonUtil
{
    private JObject m_obj;
    public JsonUtil(string instr, bool filed = false)
    {
        if (filed) {
            this.m_obj = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(instr));
        } else {
            this.m_obj = JsonConvert.DeserializeObject<JObject>(instr);
        }
    }

    private string format_tab(int tab)
    {
        var rets = "";
        int i;
        for (i = 0; i < tab; i++) {
            rets += "    ";
        }
        return rets;
    }

    private string format_tab_noline(int tab, string fmtstr, params object[] args)
    {
        var rets = "";
        rets += format_tab(tab);
        if (args.Length > 0) {
            rets += String.Format(fmtstr, args);
        } else {
            rets += String.Format("{0}", fmtstr);
        }

        return rets;
    }

    private string format_tab_line(int tab, string fmtstr, params object[] args)
    {
        var rets = this.format_tab_noline(tab, fmtstr, args);
        rets += "\n";
        return rets;
    }

    private string format_array(int tab, string key, JArray arr)
    {
        var rets = "";
        int i = 0;
        JToken tok;
        JValue val;
        string valtype;

        if (key.Length > 0) {
            rets += format_tab_noline(tab , "\"{0}\" :", key);
        }

        rets += "[";
        for (i = 0; i < arr.Count; i++) {
            if (i > 0) {
                rets += ",";
            }
            tok = arr[i];
            valtype = tok.GetType().FullName;
            if (valtype == "Newtonsoft.Json.Linq.JValue") {
                val = (JValue) tok;
                switch (val.Type) {
                case JTokenType.Integer:
                    rets += String.Format("{0}", (System.Int64) val.Value);
                    break;
                case JTokenType.Float:
                    rets += String.Format("{0}", (System.Double) val.Value);
                    break;
                case JTokenType.String:
                    rets += String.Format("\"{0}\"", (System.String) val.Value);
                    break;
                default:
                    throw new Exception(String.Format("can not find type {0}", val.Type));
                }
            } else if (valtype == "Newtonsoft.Json.Linq.JArray") {
                rets += this.format_array(tab + 1 , "", tok.Value<JArray>());
            } else if (valtype == "Newtonsoft.Json.Linq.JObject") {
                rets += this.format_object(tab + 1 , "", tok.Value<JObject>());
            } else {
                throw new Exception(String.Format("unknown type [{0}]", valtype));
            }
        }
        rets += "]";
        return rets;
    }


    private string format_object(int tab, string key, JObject obj)
    {
        var rets = "";
        List<String> keys;
        Dictionary<string, object> nobj;
        int i;
        if (key.Length > 0) {
            rets += format_tab(tab);
            rets += String.Format("\"{0}\" : ", key);
        }
        nobj = obj.ToObject<Dictionary<string, object>>();
        keys = new List<String>(nobj.Keys);
        rets += "{\n";

        for (i = 0; i < keys.Count; i++) {
            if (i > 0) {
                rets += ",\n";
            }
            switch (nobj[keys[i]].GetType().FullName) {
            case "System.String":
                rets += format_tab_noline(tab + 1, "\"{0}\": \"{1}\"" , keys[i], (System.String) nobj[keys[i]]);
                break;
            case "System.Int64":
                rets += format_tab_noline(tab + 1 , "\"{0}\" : {1}", keys[i], (System.Int64) nobj[keys[i]]);
                break;
            case "System.Double":
                rets += format_tab_noline(tab + 1 , "\"{0}\" : {1}", keys[i], (System.Double) nobj[keys[i]]);
                break;
            case "Newtonsoft.Json.Linq.JArray":
                rets += this.format_array(tab + 1 , keys[i], (Newtonsoft.Json.Linq.JArray) nobj[keys[i]]);
                break;
            case "Newtonsoft.Json.Linq.JObject":
                rets += this.format_object(tab + 1 , keys[i], (Newtonsoft.Json.Linq.JObject) nobj[keys[i]]);
                break;
            }
        }

        if (keys.Count > 0) {
            rets += "\n";
        }
        rets += format_tab_noline(tab, "}");
        return rets;
    }


    public override string ToString()
    {
        var rets = this.format_object(0, "", this.m_obj);
        return rets;
    }

    public static void Main(string[] args)
    {
        JsonUtil util ;
        int i;
        if (args.Length > 0) {
            i = 0;
            if (args[i] == "parse") {
                for (i = 1; i < args.Length; i++) {
                    util = new JsonUtil(args[i], true);
                    Console.Out.WriteLine("{0}", util.ToString());
                }
            } else {
                throw new Exception(String.Format("unknown [{0}] command", args[0]));
            }
        }
        return;
    }
}
}
