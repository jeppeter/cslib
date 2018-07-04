using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;


namespace jsonutil
{
class JsonUtil
{
    private Dictionary<string, Object> m_obj;
    public JsonUtil(string instr, bool filed = false)
    {
        if (filed) {
            this.m_obj = JsonConvert.DeserializeObject<Dictionary<string, Object>>(File.ReadAllText(instr));
        } else {
            this.m_obj = JsonConvert.DeserializeObject<Dictionary<string, Object>>(instr);
        }
    }


    public override string ToString()
    {
        List<String> keys;
        string k;
        int i;
        string rets = "";
        keys = new List<String>(this.m_obj.Keys);
        rets += "{\n";
        for (i=0;i<keys.Count;i++) {
        	k = keys[i];
            rets += String.Format("\t\"{0}\" = type [{1}]\n", k, this.m_obj[k].GetType().FullName);
        }
        rets += "}\n";
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
