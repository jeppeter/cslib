using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace CallFuncExe
{
	public class CallFuncExe
	{

		public static object CallFunc(string dllname, string spacename, string clsname, string methodname, object[] inputs)
		{
			Assembly dll=null;
			Type tp;
			MethodInfo mi;
			if (dllname == "") {
				dll = Assembly.GetExecutingAssembly();
			} else {
				try {
					dll = Assembly.LoadFrom(dllname);	
				}
				catch(FileNotFoundException ec) {
					/*we throw output */
					throw ec;
				}				
			}

			tp = dll.GetType(String.Format("{0}.{1}", spacename, clsname));
			if (tp == null) {
				throw new Exception(String.Format("can not found [{0}.{1}]", spacename, clsname));
			}
			mi = tp.GetMethod(methodname);
			if (mi == null) {
				throw new Exception(String.Format("can not found method [{0}]", methodname));
			}
			return mi.Invoke(null, inputs);
		}

		public static void Main(string[] args)
		{
			object retobj;
			object[] inputs;
			int i;
			if (args.Length < 4) {
				Console.Error.WriteLine("dllname namespacename classname methodname args...");
			}


			inputs = new object[args.Length - 4];
			for (i=0;i<inputs.Length;i++) {
				inputs[i] = args[4+i];
			}
			retobj = CallFunc(args[0], args[1], args[2],args[3],inputs);
			Console.Out.WriteLine("ret [{0}]", retobj);

			return;
		}		
	}
}