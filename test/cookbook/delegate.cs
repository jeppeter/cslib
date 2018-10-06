using System;

class Program
{
	public delegate void PrintNumber(int v);

	public static void Main(string[] args)
	{
		PrintNumber pn = PNv;
		pn(30);
		pn = CC;
		pn(290);
		return;
	}

	public static void PNv(int v)
	{
		Console.WriteLine("PNv {0}", v);
	}

	public static void CC(int v)
	{
		Console.WriteLine("CC {0}", v);
	}
}