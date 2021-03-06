using System;
using System.IO;
using System.Collections;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[Serializable]
public class SerializeObject
{
    public int ID
    {
        get;
        set;
    }

    public string UserName
    {
        get;
        set;
    }

    public string Password
    {
        get;
        set;
    }

    private string CC
    {
        get;
        set;
    }

    /// this is not serializable
    public SerializeObject()
    {
        this.CC = "cc";
    }

    [XmlIgnoreAttribute]
    public string notSerialize;
}

public class App
{
    [STAThread]
    public static void Main(string[] args)
    {
        if (args.Length > 0) {
        Serialize(args[0]);
        Deserialize(args[0]);
        }
    }

    static void Serialize(string fname)
    {
        SerializeObject serializeObject = new SerializeObject();
        serializeObject.ID = 1;
        serializeObject.UserName = "csdbfans";
        serializeObject.Password = "csdbfans";
        serializeObject.notSerialize = "博客园";

        FileStream fs = new FileStream(fname, FileMode.Create);
        XmlSerializer formatter = new XmlSerializer(typeof(SerializeObject));
        try
        {
            formatter.Serialize(fs, serializeObject);
        }
        catch (SerializationException e)
        {
            Console.WriteLine("Failed to serialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }
    }


    static void Deserialize(string fname)
    {
        SerializeObject serializeObject = null;

        FileStream fs = new FileStream(fname, FileMode.Open);
        try
        {
            XmlSerializer formatter = new XmlSerializer(typeof(SerializeObject));
            serializeObject = (SerializeObject)formatter.Deserialize(fs);
        }
        catch (SerializationException e)
        {
            Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }

        Console.WriteLine("反序列Serializable的结果：ID->" + serializeObject.ID + ", UserName->"                           
+ serializeObject.UserName + ", Password->" + serializeObject.Password);
        Console.WriteLine("反序列化NonSerialized结果：" + serializeObject.notSerialize);
    }
}