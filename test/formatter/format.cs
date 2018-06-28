using System;

namespace Formatter
{
public class Formatter
{
    public static void Main(string[] args)
    {
    	string fmtstr;
    	string[] paramsc = new string[(args.Length - 1)];

    	Array.Copy(args,1,paramsc,0,args.Length-1);

    	fmtstr = String.Format(args[0], paramsc);
    	Console.WriteLine("{0}", fmtstr);
    	return;
    }
}
}