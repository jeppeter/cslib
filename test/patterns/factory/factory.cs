using System;

namespace FactorySpace
{
	abstract class FactoryBase
	{
	}

	class FactoryA : FactoryBase
	{
		public FactoryA()
		{
			Console.WriteLine("FactoryA");
		}
	}

	class FactoryB : FactoryBase
	{
		public FactoryB()
		{
			Console.WriteLine("FactoryB");
		}

	}

	abstract class CreateFactory
	{
		public abstract FactoryBase CreateFactory();
	}

	class CreateA : CreateFactory
	{
	  	public override FactoryBase CreateFactory()
	  	{
	  		return new FactoryA();
	  	}
	}

	class CreateB :CreateFactory
	{
		public override FactoryBase CreateFactory()
		{
			return new FactoryB();
		}
	}


	public class MainApp
	{
		static void Main(string[] args)
		{
			FactoryBase base= new FactoryBase[2];
			CreateB baseb = new CreateB();
			CreateA basea = new CreateA();

			base[0] = basea.CreateFactory();
			base[1] = baseb.CreateFactory();
			return;
		}
	}
}