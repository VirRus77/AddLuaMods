using System.Linq;
using System.Reflection;
using AddLuaMods.Tests.TestClasses;
using AddLuaMods.Tests.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AddLuaMods.Tests
{
    [TestClass]
    public class UnitTestMakeBag
    {
        private const BindingFlags Everything = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;

        [TestMethod]
        public void TestMethod1()
        {
            var x = MakeBag.Make<global::ResearchStation>(
                makeTypes: MakeBag.Types.Methods | MakeBag.Types.Private | MakeBag.Types.Public,
                makeInstance: false
            );
        }

        [TestMethod]
        public void TestGetMethod()
        {
            var methodInfo = typeof(global::ResearchStation).GetTypeInfo().GetDeclaredMethod("SetState");
        }

        [TestMethod]
        public void TestGetPrivateMethod()
        {
            var type = typeof(TestSetInherit);
            var methodsInfo = type.GetMethods(Everything)
                //.Where(v=>v.DeclaringType != typeof(object))
                .ToArray();
        }
    }
}
