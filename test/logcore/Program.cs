using System;
using System.IO;
using System.Configuration;
using log4net;
using log4net.Config;
using System.Reflection;

//[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace logcore
{
    class Program
    {
    private static string get_app_config(string basedir)
    {
        string abspath = Path.GetFullPath(basedir);
        string parentpath=basedir, retstr = "";

        while (true) {
            try {
                if (File.Exists(Path.Combine(abspath, "App.Config"))) {
                    retstr = Path.Combine(abspath,"App.Config");
                    break;
                }
                parentpath = abspath;
                abspath = Path.GetFullPath(Path.Combine(parentpath,".."));
                if (parentpath == abspath) {
                    break;
                }
            } catch  {
                break;
            }
        }
        return retstr;
    }
        static void Main(string[] args)
        {
            ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string configpath = "";
        if (args.Length > 0) {
            configpath = args[0];
        } else {
            configpath = get_app_config(".");
        }
        if (configpath.Length > 0) {
            Console.WriteLine("get {0}", configpath);
            XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()),new System.IO.FileInfo(configpath));
        }

        //Logger.InfoFormat("Running as {0}", WindowsIdentity.GetCurrent().Name);
        Logger.Error("This will appear in red in the console and still be written to file!");
        Console.WriteLine("please enter return to exit");
        Console.ReadLine();
        }
    }
}
