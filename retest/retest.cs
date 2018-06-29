using System;
using System.IO;
using System.Text.RegularExpressions;

namespace retest
{
	public class NotArgument : Exception
	{
		public NotArgument(string str) : base(str)
		{

		}
	}

	public class retest
	{
		private string m_cmd;
		private string[] m_args;

		private void Usage(int ec, string fmtstr, params object[] fmtobj)
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

			writer.WriteLine("retest [cmd]...");
			writer.WriteLine("\tmatch restr instr...");

			Environment.Exit(ec);
		}

		public retest(string[] args)
		{
			if (args.Length < 1) {
				throw new NotArgument(String.Format("{0} < 2 arguemtns", args.Length));
			}
			this.m_cmd = args[0];
			this.m_args = args;
		}

		private int match_handler()
		{
			int i;
			if (this.m_args.Length < 3) {
				throw new NotArgument(String.Format("match restr instr..."));
			}
			Regex regex = new Regex(this.m_args[1]);
			for (i=2;i<this.m_args.Length;i++) {
				if (regex.IsMatch(this.m_args[i])) {
					Console.Out.WriteLine("[{0}] matches [{1}]", this.m_args[i], this.m_args[1]);
				} else {
					Console.Out.WriteLine("[{0}] matches [{1}]", this.m_args[i], this.m_args[1]);
				}
			}
			return 0;
		}

		private int process()
		{
			switch(this.m_args[0]) {
				case "match":
				return this.match_handler();
				case "-h":
					this.Usage(0,"");
					return 0;
				case "--help":
					this.Usage(0,"");
					return 0;
				default:
				throw new NotArgument(String.Format("not supported cmd [{0}]", this.m_args[1]));
			}
		}


		public static void Main(string[] args)
		{
			retest r = new retest(args);
			r.process();
			return;
		}
	}
}