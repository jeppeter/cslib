using System;
using log4net;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System.Diagnostics;


namespace logcode
{

	class _LogObject
	{
		private ILog m_logger;

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

		public _LogObject(string name)
		{
			Logger l;
			//Level lvl;
			//int i;
			string lvlstr="DEBUG";
			string appname = String.Format("{0}_APPENDER", name).ToUpper();
			ConsoleAppender app=null;
			this._create_repository(name);
			if (LogManager.Exists(name,name) != null) {
				this.m_logger = LogManager.GetLogger(name,name);
				l = (Logger) this.m_logger.Logger;
				l.Level = l.Hierarchy.LevelMap[lvlstr];
			} else {
				PatternLayout patternLayout = new PatternLayout();
				Hierarchy hr;
				patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
				patternLayout.ActivateOptions();
				this.m_logger = LogManager.GetLogger(name,name);
				l = (Logger) this.m_logger.Logger;
				l.Level = l.Hierarchy.LevelMap[lvlstr];
				app = new ConsoleAppender();
				app.Name = appname;
				app.Layout = patternLayout;
				//app.Target = "Console.Error";
				l.AddAppender(app);
				hr = (Hierarchy)LogManager.GetRepository(name);
				hr.Configured = true;
			}
		}

		private string __get_caller(int callstack)
		{
			StackTrace stk = new StackTrace(true);
			string retstr = "";
			if (callstack < stk.FrameCount) {
				StackFrame frm = stk.GetFrame(callstack);
				retstr += String.Format("[{0}:{1}] ", frm.GetFileName(), frm.GetFileLineNumber());
			}
			return retstr;
		}

		public void Debug(string msg, int callstack=1)
		{
			string callmsg;
			callmsg = this.__get_caller(callstack + 1);
			callmsg += " ";
			callmsg += msg;

			this.m_logger.Debug(callmsg);
		}

		public void Error(string msg, int callstack=1)
		{
			string callmsg;
			callmsg = this.__get_caller(callstack + 1);
			callmsg += " ";
			callmsg += msg;

			this.m_logger.Error(callmsg);
		}

		public void Warn(string msg, int callstack=1)
		{
			string callmsg;
			callmsg = this.__get_caller(callstack + 1);
			callmsg += " ";
			callmsg += msg;

			this.m_logger.Warn(callmsg);
		}

		public void Info(string msg, int callstack=1)
		{
			string callmsg;
			callmsg = this.__get_caller(callstack + 1);
			callmsg += " ";
			callmsg += msg;

			this.m_logger.Info(callmsg);
		}

		public void Fatal(string msg, int callstack=1)
		{
			string callmsg;
			callmsg = this.__get_caller(callstack + 1);
			callmsg += " ";
			callmsg += msg;

			this.m_logger.Fatal(callmsg);
		}
	}

    class Program
    {
        static void Main(string[] args)
        {
            _LogObject logobj = new _LogObject("func");
            _LogObject secobj = new _LogObject("func");
            logobj.Info("hello world");
            logobj.Debug("hello world");
            logobj.Warn("hello world");
            logobj.Error("hello world");
            logobj.Fatal("hello world");

            secobj.Info("hello world");
            secobj.Debug("hello world");
            secobj.Warn("hello world");
            secobj.Error("hello world");
            secobj.Fatal("hello world");
            return;
        }
    }
}
