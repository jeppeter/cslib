using System;
using System.IO;
using System.Text.RegularExpressions;

namespace retest
{
	public class retest
	{
		private string m_cmd;

		private void Usage(int ec, string fmtstr, object[] fmtobj)
		{
			TextWriter writer = Console.Error;
			string outstr;
			if (ec == 0) {
				writer = Console.Out;
			}
			if (fmtstr.Length > 0) {
				outstr = String.Format(fmtstr,fmtobj);
				writer.WriteLine("{0}", outstr);
			}

			writer.WriteLine("")


			Environment.Exit(ec);
		}

		public retest(string cmd)
		{

		}


		public static void Main(string[] args)
		{
			if (args.Length < 2) {

			}
		}
	}
}