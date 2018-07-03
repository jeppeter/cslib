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
        Dictionary<string,Object>.KeyCollection keys;
        string k;
        int i;
        string rets = "";
        keys = this.m_obj.Keys;
        rets += "{\n";
        for (i=0;i<keys.Length;i++) {
        	k = keys[i].Key;
            rets += String.Format("\"{0}\" : {1}\n", k, this.m_obj[k]);
        }
        rets += "}\n";
        return rets;
    }

    static void Main(string[] args)
    {
        JsonUtil util ;
        int i;
        if (args.Length > 0) {
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
