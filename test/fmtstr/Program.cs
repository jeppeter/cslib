using System;

namespace fmtstr
{
    class Program
    {
    	private static string format_string(string fmtstr, params object[] args)
    	{
    		return String.Format(fmtstr,args);
    	}
        static void Main(string[] args)
        {
            string rets;
            string[] slicearr = {};

            rets = format_string("hello {0}", "world");
            Console.Out.WriteLine(rets);
            if (args.Length > 1) {
            	slicearr = new string[(args.Length-1)];
            }
            Array.Copy(args,1,slicearr,0,args.Length - 1);
            rets = format_string(args[0], slicearr);
            Console.Out.Write(rets);
            return;
        }
    }
}
