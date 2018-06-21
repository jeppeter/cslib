using System;
using System.Security.Principal;
using System.IO;
using log4net;
using log4net.Config;

namespace MyApp
{
internal class Program
{
    private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

    private static void Main(string[] args)
    {
        string configpath = "";
        if (args.Length > 0) {
            configpath = args[0];
        } else {
            configpath = get_app_config(".");
        }
        if (configpath.Length > 0) {
            Console.WriteLine("get {0}", configpath);
            XmlConfigurator.Configure(new System.IO.FileInfo(configpath));
        }
        Logger.InfoFormat("Running as {0}", WindowsIdentity.GetCurrent().Name);
        Logger.Error("This will appear in red in the console and still be written to file!");
        Console.WriteLine("please enter return to exit");
        Console.ReadLine();
    }
}
}