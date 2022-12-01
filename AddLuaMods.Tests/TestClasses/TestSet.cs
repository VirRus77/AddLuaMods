namespace AddLuaMods.Tests.TestClasses
{
    internal class TestSet
    {
        private void TestPrivateMethod()
        {

        }

        protected void TestProtectedMethod()
        {

        }
    }

    internal class TestSetInherit:TestSet
    {
        private void TestPrivateMethod()
        {

        }

        protected new void TestProtectedMethod()
        {

        }
    }
}
