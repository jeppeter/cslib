using tstcase;
using NUnit.Framework;


namespace tstcase_Test
{
    [TestFixture]
    public class tstcase_Test
    {
        private BaseCall _mcall ;
        public tstcase_Test()
        {
            _mcall = new BaseCall();
        }

        [Test]
        public void tstcase_Test_add()
        {
            var result = _mcall.BaseCall_add(1, 2);
            Assert.IsTrue(result == 3, "should 3");
        }
    }
}
