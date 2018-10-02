using System;
using System.Collections;


namespace tuplecls
{
	public class TupleClass 
	{
		public static void Main(string[] args)
		{
			//int h,w,c;
			Tuple<int,int,int> b;
			//(h,w,c) = GetTuple(3);
			b = GetTuple(3);
			//(h,w,c) = GetTuple(3);
			//Console.WriteLine("{0},{1},{2}", h,w,c);
			Console.WriteLine("{0},{1},{2}",b.Item1,b.Item2,b.Item3);
			return;
		}

		public static Tuple<int,int,int> GetTuple(int v)
		{
			return new Tuple<int,int,int>(v*1,v*2,v*3);
		}
	}
}