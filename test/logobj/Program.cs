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
						m.ReleaseMutex();
					}
					waited = true;
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

		private string __format_message(params object[] strs)
		{
			string fmtstr = "";
			StackTrace stk = new StackTrace(true);
			string s = strs[0].GetType().Name;
			object[] nparams;
			StackFrame frm;
			MethodBase meth;
			string basefmt = "";
			int mc=0;
			int i;
			int addi = 1;
			int stkidx=2;
			mc = strs.Length - 1;
			if (s == "Int32") {
				nparams = new object[strs.Length-1];
				stkidx = (int)strs[0];
				stkidx += 1;
				addi = 2;
				mc = strs.Length - 2;
				s = strs[1].GetType().Name;
				basefmt = (string)strs[1];
			} else {
				basefmt = (string) strs[0];
			}

			nparams = new object[mc];
			for (i=0;i<mc ; i++) {
				nparams[i] =strs[(i+addi)];
			}

			frm = stk.GetFrame(stkidx);
			if (frm != null) {
				meth = frm.GetMethod();
				fmtstr += String.Format("[{0}:{1}:{2}] ", meth.Name, frm.GetFileName(), frm.GetFileLineNumber());
			}

			fmtstr += String.Format(basefmt,nparams);

			//Console.Out.WriteLine("{0}", fmtstr);
			return fmtstr;
		}

		public void Fatal(params object[] strs)
		{
			string s = this.__format_message(strs);
			this.m_logger.Fatal(s);
			return;
		}

		public void Error(params object[] strs)
		{
			string s = this.__format_message(strs);
			this.m_logger.Error(s);
			return;
		}

		public void Warn(params object[] strs)
		{
			string s = this.__format_message(strs);
			this.m_logger.Warn(s);
			return;
		}

		public void Info(params object[] strs)
		{
			string s = this.__format_message(strs);
			this.m_logger.Info(s);
			return;
		}

		public void Debug(params object[] strs)
		{
			string s = this.__format_message(strs);
			this.m_logger.Debug(s);
			return;
		}

		public object call_func(string funcname, params object[] args)
		{
			
			return null;
		}
	}

	public class ObjCC
	{
		public static void Main(string[] args)
		{
			string cmdname;
			_LogObject log;
			if (args.Length < 1) {
				Console.Error.WriteLine("at least 2 args");
				return;
			}
			cmdname = args[0];

			log = new _LogObject(cmdname);
			log.Info("Hello cc {0} {1}","bb",320);
			log.Error("Hello cc {0} {1}","bb",320);
			log.Debug("Hello cc {0} {1}","bb",320);
			log.Warn("Hello cc {0} {1}","bb",320);
			log.Fatal("Hello cc {0} {1}","bb",320);

			log.Info(1,"Hello cc {0} {1}","bb",320);
			log.Error(1,"Hello cc {0} {1}","bb",320);
			log.Debug(1,"Hello cc {0} {1}","bb",320);
			log.Warn(1,"Hello cc {0} {1}","bb",320);
			log.Fatal(1,"Hello cc {0} {1}","bb",320);
			log.Fatal(50,"Hello cc {0} {1}","bb",320);
			return;
		}
	}

}