using System;
using NUnit.Framework;

namespace TSqlFlex.Core.Tests
{

    [TestFixture()]
    class TSqlRulesTests
    {
        [Test()]
        public void ReservedWord_IsConsideredReserved()
        {
            Assert.AreEqual(true, TSqlRules.IsReservedWord("select"));
        }

        [Test()]
        public void NonReservedWord_IsNotConsideredReserved()
        {
            Assert.AreEqual(false, TSqlRules.IsReservedWord("SomeSillyThing"));
        }
    }
}
