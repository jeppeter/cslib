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

    private string format_tab_noline(int tab, string fmtstr, params object[]args)
    {
        var rets = "";
        rets += format_tab(tab);
        rets += String.Format(fmtstr, args);
        return rets;
    }

    private string format_tab_line(int tab, string fmtstr, params object[]args)
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

        rets += format_tab_noline(tab , """{0}"" : [", key);
        for (i=0;i<arr.Count;i++) {
            tok = arr[i];
            Console.Err.WriteLine("[{0}]=[{1}]", i, tok.GetType().FullName);
        }
        foreach( var item in arr.Children()) {
            if (i > 0) {
                rets += ",";
            }
            switch()

            i ++;
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
            rets += String.Format("""%s"" : ", key);
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
                rets += format_tab_noline(tab + 1, """{0}"": {1}" , keys[i], (System.String) nobj[keys[i]]);
                break;
            case "System.Int64":
                rets += format_tab_noline(tab + 1 , """{0}"" : {1}", keys[i], (System.Int64) nobj[keys[i]]);
                break;
            case "System.Double":
                rets += format_tab_noline(tab + 1 , """{0}"" : {1}", keys[i], (System.Int64) nobj[keys[i]]);
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
        var rets = this.format_object(0,"",this.m_obj);
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
