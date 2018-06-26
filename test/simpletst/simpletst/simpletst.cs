using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NUnitTesting
{
    /*class Program
    {
        static void Main(string[] args)
        {
        }
    }*/

    public class Maths
    {
        public int Add(int a, int b)
        {
            int x = a + b;
            return x;
        }
    }

    [TestFixture]
    public class TestLogging
    {
        [Test]
        public void Add()
        {
            Maths add = new Maths();
            int expectedResult = add.Add(1, 2);
            Console.WriteLine("Add");
            Assert.That(expectedResult, Is.EqualTo(3));
        }

        [Test]
        public void FalseAdd()
        {
            Maths add = new Maths();
            int exp = add.Add(3, 5);
            Console.WriteLine("FalseAdd");
            Assert.That(exp, Is.EqualTo(5));
        }
    }
}