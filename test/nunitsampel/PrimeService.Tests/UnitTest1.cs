using NUnit.Framework;
using Prime.Services;

namespace Prime.UnitTests.Services
{
    [TestFixture]
    public class PrimeService_IsPrimeShould
    {

        public PrimeService_IsPrimeShould()
        {
        }

        [Test]
        public void ReturnFalseGivenValueOf1()
        {
            PrimeService _test;
            _test = new PrimeService();
            var result = _test.IsPrime(1);

            Assert.IsFalse(result, "1 should not be prime");
        }
    }
}