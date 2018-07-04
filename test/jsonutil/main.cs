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
            this.m_obj = JObject.Parse(File.ReadAllText(instr));
        } else {
            this.m_obj = JObject.Parse(instr);
        }
    }

    private string format_jarray_string(int tab, string name, JArray arr)
    {
    	string rets;
    	int i;
    	Object obj;
    	rets = "";
    	for (i=0;i<tab;i++) {
    		rets += "    ";
    	}
    	rets += String.Format("\"%s\" : ", name);
    	for (i=0;i< arr.Count;i++) {
    		obj = arr[i];
    		Console.Out.WriteLine("[{0}] = [{1}]", i, obj.GetType().FullName);
    	}
    	rets += "\n";
    	return rets;

    }

    private string format_jobject_string(int tab,string name,JObject obj)
    {
    	string rets;
    	int i;
    	rets = "";
    	for (i=0;i<tab;i++) {
    		rets += "    ";
    	}

    	if (name.Length > 0) {
    		rets += String.Format("\"%s\" : ", name);
    	}

    	rets += "{\n";
    	return rets;
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
