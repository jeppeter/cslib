using System;
using log4net;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;


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
			string lvlstr="ERROR";
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
				l.Level = l.Hierarchy.LevelMap["DEBUG"];
				app = new ConsoleAppender();
				app.Name = appname;
				app.Layout = patternLayout;
				//app.Target = "Console.Error";
				l.AddAppender(app);
				hr = (Hierarchy)LogManager.GetRepository(name);
				hr.Configured = true;
			}
		}

		public void Debug(object msg)
		{
			this.m_logger.Debug(msg);
		}

		public void Error(object msg)
		{
			this.m_logger.Error(msg);
		}

		public void Warn(object msg)
		{
			this.m_logger.Warn(msg);
		}

		public void Info(object msg)
		{
			this.m_logger.Info(msg);
		}

		public void Fatal(object msg)
		{
			this.m_logger.Fatal(msg);
		}
	}

    class Program
    {
        static void Main(string[] args)
        {
            _LogObject logobj = new _LogObject("func");
            logobj.Info("hello world");
            logobj.Debug("hello world");
            logobj.Warn("hello world");
            logobj.Error("hello world");
            logobj.Fatal("hello world");
            return;
        }
    }
}
