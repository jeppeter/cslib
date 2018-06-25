using NUnit.Framework;
using tstcase;


namespace tstcase.Test
{
    [TestFixture]
    class tstcase_Test
    {
        private BaseCall* _mcall = null;
        public tstcase_Test()
        {
            _mcall = new BaseCall();
        }

        [Test]
        public void tstcase_Test_add()
        {
            var result = _mcall.BaseCall_add(1, 2);
            Assert.That(result == 3);
        }
    }
}
