using System;

namespace list
{
	public class list
	{
		//private static readonly string[] m_listonly = new string[]{"hello","world"};
		private string m_file;
		private string m_attr;

		private object get_value(string name)
		{
			string memname = String.Format("m_{0}", name);
			Console.WriteLine("get {0}",memname);
			return this.GetType().GetProperty(memname);
		}

		private list()
		{
			this.m_file = "cc";
			this.m_attr = "sx";
			this.m_file = this.m_file;
			this.m_attr = this.m_attr;
		}



		public static void Main(string[] args)
		{
			int i;
			list lm = new list();
			for (i=0;i<args.Length;i++) {
				Console.WriteLine("{0}={1}", args[i], lm.get_value(args[i]));
			}
			return;
		}
	}
}