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
			int i;
			string lvlstr="ERROR";
			this._create_repository(name);
			this.m_logger = LogManager.GetLogger(name,name);
			l = (Logger) this.m_logger.Logger;
			l.Level = l.Hierarchy.LevelMap[lvlstr];
		}
	}

    class Program
    {
        static void Main(string[] args)
        {
            _LogObject logobj = new _LogObject("func");
            return;
        }
    }
}
