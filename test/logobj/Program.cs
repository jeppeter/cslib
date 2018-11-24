using System;
using log4net;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System.Diagnostics;
using System.Reflection;
using System.Threading;


namespace LogObj
{
	public class ParseException : Exception
	{
		public ParseException(string str) : base(str)
		{
		}
	}

	public class _LogObject
	{

		private class Mux
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
					m = new Mutex(true,name,out created);
					if (!created) {
						goto try_again;
					}
				}
			wait_one:
				try {
					m.WaitOne();				
					waited = true;				
				}
				catch(AbandonedMutexException) {
					if (m != null) {
						goto wait_one;
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


		private ILog m_logger;

		private string get_namespace(int stknum)
		{
			StackTrace stk = new StackTrace();
			MethodBase info = stk.GetFrame(stknum).GetMethod();
			return info.ReflectedType.Namespace;
		}

		private bool _create_repository(string name)
		{
			var repos = LogManager.GetAllRepositories();
			foreach (var c in repos) {
				if (c.Name == name) {
					return false;
				}
			}
			LogManager.CreateRepository(name);
			return true;
		}

		public _LogObject(string cmdname="extargsparse")
		{
			string curnamespace,callernamespace;
			Logger l;
			string lvlstr = "ERROR";
			string appname = String.Format("{0}_APPENDER", cmdname).ToUpper();
			string loglvl = String.Format("{0}_LOGLVL",cmdname).ToUpper();
			int ival=0;
			ConsoleAppender app=null;
			Mux mux = new Mux(cmdname);
			string logval  = Environment.GetEnvironmentVariable(loglvl);
			if (logval != "") {
				try {
					ival = Int32.Parse(logval);
				}
				catch(Exception) {
					ival = 0;
				}
			}

			if (ival <= 0) {
				lvlstr = "ERROR";
			} else if (ival <= 1) {
				lvlstr = "WARN";
			} else if (ival <= 2) {
				lvlstr = "INFO";
			} else if (ival <= 3) {
				lvlstr = "DEBUG";
			} else {
				lvlstr = "ALL";
			}


			/*now first to get the class*/
			try {
				curnamespace = get_namespace(1);
				callernamespace = get_namespace(2);
				if (curnamespace != callernamespace) {
					throw new ParseException(String.Format("can not be call directly by outer namespace [{0}]", curnamespace));
				}
				this._create_repository(cmdname);
				if (LogManager.Exists(cmdname,cmdname) == null) {
					PatternLayout patternLayout = new PatternLayout();
					Hierarchy hr;
					patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
					patternLayout.ActivateOptions();
					this.m_logger = LogManager.GetLogger(cmdname,cmdname);
					l = (Logger) this.m_logger.Logger;
					l.Level = l.Hierarchy.LevelMap[lvlstr];
					app = new ConsoleAppender();
					app.Name = appname;
					app.Layout = patternLayout;
					//app.Target = "Console.Error";
					l.AddAppender(app);
					hr = (Hierarchy)LogManager.GetRepository(cmdname);
					hr.Configured = true;
				} else {
					this.m_logger = LogManager.GetLogger(cmdname,cmdname);
				}
			}
			finally {
				mux.Close();
			}
		}

		public void Fatal(params object[] strs)
		{
			return;
		}

		public void Error(params object[] strs)
		{
			return;
		}

		public void Warn(params object[] strs)
		{
			return;
		}

		public void Info(params object[] strs)
		{
			return;
		}

		public void Debug(params object[] strs)
		{
			return;
		}

		public void Trace(params object[] strs)
		{
			return;
		}
	}

	public class ObjCC
	{
		public static void Main(string[] args)
		{
			string cmdname;
			_LogObject log;
			string[] cargs;
			int i;
			if (args.Length < 2) {
				Console.Error.WriteLine("at least 2 args");
				return;
			}
			cmdname = args[0];

			cargs = new string[(args.Length-1)];
			for (i=0;i<(args.Length - 1) ; i++){
				cargs[i] = args[i+1];
			}

			log = new _LogObject(cmdname);
			log.Info(cargs);
			log.Error(cargs);
			log.Debug(cargs);
			log.Trace(cargs);
			log.Warn(cargs);
			log.Fatal(cargs);

			log.Info(1,cargs);
			log.Error(1,cargs);
			log.Debug(1,cargs);
			log.Trace(1,cargs);
			log.Warn(1,cargs);
			log.Fatal(1,cargs);
			return;
		}
	}

}