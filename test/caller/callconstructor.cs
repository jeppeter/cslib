using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;


namespace Constr
{
public class CC
{
    private int m_n;
    private string m_s;
    public CC(int n = 52)
    {
        this.m_s = "";
        this.m_n = n;
    }

    public CC(string m = "e2")
    {
        this.m_s = m;
        this.m_n = 0;
    }

    public CC(string[] m)
    {
        if (m.Length > 0) {
            this.m_s = m[0];
        }
        this.m_n = 0;
    }

    public CC(string cc = "ss", int n = 30)
    {
        this.m_s = cc;
        this.m_n = n;
    }

    public string Get()
    {
        return String.Format("s [{0}] n [{1}]", this.m_s, this.m_n);
    }
}

public class Constr
{
    public Constr()
    {

    }

    public object CreateObject(string name)
    {
        Type ct = Type.GetType(name);
        ConstructorInfo finfo = null;
        object[] paramcc;
        int paramnum = 0;
        int idx, jdx, i;
        foreach (ConstructorInfo item in ct.GetConstructors()) {
            idx = 0;
            jdx = 0;
            foreach (ParameterInfo pinfo in item.GetParameters()) {
                if (! pinfo.HasDefaultValue) {
                    jdx ++;
                }
                idx ++;
            }
            if (jdx == 0) {
                finfo = item;
                paramnum = idx;
                break;
            }
        }

        if (finfo == null) {
            throw new Exception(String.Format("can not find null or default value constructor [{0}]" , name));
        }

        paramcc = new object[paramnum];
        i = 0;
        foreach (ParameterInfo pinfo in finfo.GetParameters()) {
            paramcc[i] = (object)pinfo.DefaultValue;
            i ++;
        }
        return finfo.Invoke(paramcc);
    }

    public static void Main(string[] args)
    {
    	Constr c = new Constr();
    	CC cc =  c.CreateObject("Constr.CC") as CC;
    	Console.Out.WriteLine("{0}", cc.Get());
        return ;
    }
}
}