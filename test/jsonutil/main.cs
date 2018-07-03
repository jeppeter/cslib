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

    public Object 
    static void Main(string[] args)
    {
    }
}
}
