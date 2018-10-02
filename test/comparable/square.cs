using System;
using System.Collections;

namespace Square
{
	public class Square1
	{
		public static void Main(string[] args)
		{
			ArrayList cs = new ArrayList();
			//Square cur;
			cs.Add(new Square(1,1));
			cs.Add(new Square(2,2));
			cs.Add(new Square(4,10));
			cs.Add(new Square(3,12));
			cs.Sort();
			foreach(Square cur in cs)  {
				Console.WriteLine("{0}", cur);
			}
			return;
		}
	}

	class Square : IComparable
	{
		private int m_width;
		private int m_height;


		public override string ToString()
		{
			return String.Format("w[{0}]*h[{1}]", this.m_height, this.m_width);
		}

		public Square(int w,int h)
		{
			this.m_width = w;
			this.m_height = h;
		}
		public int CompareTo(object obj)
		{
			Square other = obj as Square;
			if (obj != null) {
				return CompareTo(other);
			}
			throw new ArgumentException("not null comparable");
		}

		private int CompareTo(Square obj)
		{
			int a1 = this.m_width * this.m_height;
			int a2 = obj.m_height * obj.m_width;
			if (a1 == a2 ){
				return 0;
			} else if (a1  > a2) {
				return 1;
			} else {
				return -1;
			}
		}

		public override int GetHashCode()
		{
			return this.m_width.GetHashCode() | this.m_height.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			Square square = obj as Square;
			if (obj == null) {
				return false;
			}
			if (this.m_width  * this.m_height ==  square.m_height * square.m_width)  {
				return  true;
			}
			return false;
		}
	}
}