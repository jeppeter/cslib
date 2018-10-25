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
            rets += this.format_object(tab  , "", tok.Value<JObject>());
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

    public string FormatObject(int tab, string key, Object obj)
    {
        return this.format_token(tab,key,obj as JToken);
    }


    public override string ToString()
    {
        var rets = this.format_token(0, "", this.m_obj);
        return rets;
    }

    private JToken __get_value(JToken root, string[] pathparts)
    {
        JToken obj = root;
        JObject objroot;
        string[] newpaths ;
        int idx;
        string valtype;
        if (pathparts.Length == 0 ||
            (pathparts.Length == 1 && pathparts[0] == "")) {
            return root;
        }
        newpaths = new string[(pathparts.Length - 1)];
        for (idx = 1; idx < pathparts.Length;idx ++) {
            newpaths[(idx - 1)] = pathparts[idx];
        }
        if (pathparts[0] == "") {
            return this.__get_value(root,newpaths);
        }

        valtype = root.GetType().FullName;
        if (valtype != "Newtonsoft.Json.Linq.JObject") {
            throw new JsonReaderException(String.Format("[{0}] not get for", pathparts[0]));
        }

        objroot = root as JObject;
        if (objroot[pathparts[0]] == null) {
            throw new JsonReaderException(String.Format("not get [{0}]", pathparts[0]));
        }
        obj = objroot[pathparts[0]] as JToken;
        return this.__get_value(obj, newpaths);
    }

    public JToken get_value(string path)
    {
        string[] pathparts;
        if (this.m_obj == null) {
            throw new JsonReaderException(String.Format("not found [{0}]",path));
        }
        pathparts = path.Split('.');
        return this.__get_value(this.m_obj, pathparts);
    }

    public JToken __set_value(JToken root,string[] pathparts, string type, string valstr)
    {
        if (pathparts.Length == 0 || 
            (pathparts.Length == 1 && pathparts[0] == "")) {
            return root;
        }

        return root;
    }

    public JToken set_value(string path, string type, string valstr)
    {
        string[] pathparts;
        pathparts = path.Split('.');
        this.m_obj = this.__set_value(this.m_obj, pathparts, type,valstr);
        return this.m_obj;
    }


    private static void Usage(int ec, string fmt)
    {
        var writer = Console.Error;
        if (ec == 0) {
            writer = Console.Out;
        }
        if (fmt.Length > 0) {
            writer.WriteLine("{0}", fmt);
        }

        writer.WriteLine("jsonutil [COMMAND] [OPTIONS]");
        writer.WriteLine(" ");
        writer.WriteLine("[COMMAND]");
        writer.WriteLine("\tparse  files...                to parse json file and debug print");
        writer.WriteLine("\tget    file path               to get the value from path");
        writer.WriteLine("\tset    file path type value    to set the value to path on file");

        writer.WriteLine(" ");
        writer.WriteLine("[OPTIONS]");
        writer.WriteLine("\t--help|-h                      to display this help information");

        System.Environment.Exit(ec);
    }

    public static void Main(string[] args)
    {
        JsonUtil util ;
        Object obj;
        string fname;
        string path;
        string type;
        string valstr;
        int i;
        if (args.Length > 0) {
            i = 0;
            if (args[i] == "parse") {
                for (i = 1; i < args.Length; i++) {
                    util = new JsonUtil(args[i], true);
                    Console.Out.WriteLine("{0}", util.ToString());
                }
            }  else if (args[i] == "get") {
                if (args.Length <= (i+2)) {
                    Usage(4,String.Format("[{0}] need path [{1}]", args[i], args.Length));
                }
                fname = args[i+1];
                path = args[(i+2)];
                //Console.Out.WriteLine("fname [{0}] path [{1}]", fname,path);

                util = new JsonUtil(fname, true);
                try{
                    obj = util.get_value(path); 
                    Console.Out.WriteLine("get [{0}] from [{1}]", path,fname);
                    Console.Out.WriteLine("{0}", util.FormatObject(0,"",obj as JToken));
                }
                catch(JsonReaderException ec) {
                    Console.Error.WriteLine("can not find {0} [{1}]", fname, ec.ToString());
                    System.Environment.Exit(4);
                }
            }  else if (args[i] == "set") {
                if (args.Length <= (i+4)) {
                    Usage(4,String.Format("[{0}] need file path type value", args[i]));
                }
                fname = args[(i+1)];
                path = args[(i+2)];
                type = args[(i+3)];
                valstr = args[(i+4)];
                util = new JsonUtil(fname, true);
                try {
                    util.set_value(path,type,valstr);
                    Console.Out.WriteLine("set [{0}] path[{1}] type[{2}] val[{3}]", fname, path, type,valstr);
                    Console.Out.WriteLine("{0}",util.ToString());
                }
                catch(JsonWriterException ec) {
                    Console.Error.WriteLine("can not set [{0}] type [{1}] val[{2}]", path, type, valstr);
                    Console.Error.WriteLine("{0}", ec);
                    System.Environment.Exit(4);
                }

            }else if (args[i] == "--help" || args[i] == "-h") {
                Usage(0,"");
            }else {
                throw new Exception(String.Format("unknown [{0}] command", args[0]));
            }
        } else {
            Usage(3,"need at least one arg");
        }
        return;
    }
}
}
