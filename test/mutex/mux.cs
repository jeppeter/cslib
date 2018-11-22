using System;
using System.Threading;

namespace Mux
{

	public class Mux
	{
		private Mutex m;
		private bool waited = false;
		public Mux(string name)
		{
			bool created = false;
		try_again:
			m = null;
			try {
				m = Mutex.OpenExisting(name);
			}
			catch(WaitHandleCannotBeOpenedException) {
				m = new Mutex(true,name,out created,null);
				if (!created) {
					goto try_again;
				}
			}
			try {
				m.WaitOne();				
				waited = true;				
			}
			catch(AbandonedMutexException) {
				if (m != null) {
					waited = true;	
				}
			}
		}

		public void Close()
		{
			if (m != null) {
				if (waited) {
					Console.Out.WriteLine("release");
					m.ReleaseMutex();
				}
				waited = true;
				Console.Out.WriteLine("Dispose");
				m.Dispose();
			}
			m = null;
		}

		~Mux()
		{
			this.Close();
		}
	}

	public class MuxThread
	{
		int m_num;
		public void CallAThread(string muxname,int num, string fmt)
		{
			Console.Out.WriteLine("CallA");
			Mux m = null;
			try {
				m = new Mux(muxname);
				int i;
				for (i=0;i<num;i++) {
					Console.Out.WriteLine("calla[{0}]=[{1}][{2}]",i,this.m_num, fmt);
					this.m_num ++;
				}
			}
			finally {
				if (m != null) {
					m.Close();	
				}
				m = null;				
			}
			return;
		}

		public void CallBThread(string muxname, int num, string fmt)
		{
			Mux m = null;
			Console.Out.WriteLine("CallB");
			try{
				m = new Mux(muxname);
				int i;
				for (i=0;i<num;i++) {
					Console.Out.WriteLine("callb[{0}]=[{1}][{2}]",i, this.m_num, fmt);
					this.m_num ++;
				}				
			}
			finally {
				if (m != null) {
					m.Close();	
				}
				m = null;				
			}
			return;
		}

		public MuxThread(int num=0)
		{
			m_num = num;
		}

		~MuxThread()
		{

		}

		public static void Main(string[] args)
		{
			MuxThread c = new MuxThread(20);
			Thread t1 = new Thread(() => c.CallAThread("CALLER",100,"t1") );
			Thread t2 = new Thread(delegate() {
				c.CallBThread("CALLER",200,"t2");
				});
			Thread t3 = new Thread(() => c.CallBThread("CALLER",300,"t3"));
			t2.Start();
			t1.Start();
			t3.Start();

			Console.Out.WriteLine("wait t1");
			t1.Join();
			Console.Out.WriteLine("wait t2");
			t2.Join();
			Console.Out.WriteLine("wait t3");
			t3.Join();
			Console.Out.WriteLine("wait all");
			return;
		}
	}
}