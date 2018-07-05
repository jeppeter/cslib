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
    private JToken m_obj;
    public JsonUtil(string instr, bool filed = false)
    {
        if (filed) {
            //this.m_obj = JObject.Parse(File.ReadAllText(instr));
            this.m_obj = JToken.Parse(File.ReadAllText(instr));
        } else {
            //this.m_obj = JObject.Parse(instr);
            this.m_obj = JToken.Parse(instr);
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

    private string format_token(int tab, string key, JToken tok)
    {
        var rets = "";
        JValue val;
        var valtype = "";
        string curs ;

        if (key.Length > 0) {
            rets += format_tab_noline(tab, "\"{0}\" : ", key);
        }

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
            case JTokenType.Null:
                rets += String.Format("null");
                break;
            case JTokenType.Boolean:
                curs = String.Format("{0}",(System.Boolean) val.Value);
                rets += curs.ToLower();
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
        return rets;
    }

    private string format_array(int tab, string key, JArray arr)
    {
        var rets = "";
        int i = 0;
        JToken tok;

        if (key.Length > 0) {
            rets += format_tab_noline(tab , "\"{0}\" : ", key);
        }

        rets += "[";
        for (i = 0; i < arr.Count; i++) {
            if (i > 0) {
                rets += ",";
            }
            tok = arr[i];
            rets += this.format_token(tab, "", tok);
        }
        rets += "]";
        return rets;
    }


    private string format_object(int tab, string key, JObject obj)
    {
        var rets = "";
        List<String> keys;
        Dictionary<string, JToken> nobj;
        JToken tok;
        int i;
        if (key.Length > 0) {
            rets += format_tab(tab);
            rets += String.Format("\"{0}\" : ", key);
        }
        nobj = obj.ToObject<Dictionary<string, JToken>>();
        keys = new List<String>(nobj.Keys);
        rets += "{\n";

        for (i = 0; i < keys.Count; i++) {
            if (i > 0) {
                rets += ",\n";
            }
            tok = nobj[keys[i]];
            rets += this.format_token(tab + 1 , keys[i], tok);
        }

        if (keys.Count > 0) {
            rets += "\n";
        }
        rets += format_tab_noline(tab, "}");
        return rets;
    }


    public override string ToString()
    {
        var rets = this.format_token(0, "", this.m_obj);
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
            }  else if (args[i] == "array") {
                for (i = 1 ; i < args.Length ; i ++) {
                    JToken arr = JToken.Parse(File.ReadAllText(args[i]));

                }

            } else {
                throw new Exception(String.Format("unknown [{0}] command", args[0]));
            }
        }
        return;
    }
}
}
