using System;
using System.Security.Principal;
using log4net;

namespace MyApp
{
    internal class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string[] args)
        {
            Logger.InfoFormat("Running as {0}", WindowsIdentity.GetCurrent().Name);
            Logger.Error("This will appear in red in the console and still be written to file!");
            Console.WriteLine("please enter return to exit");
            Console.ReadLine();
        }
    }
}