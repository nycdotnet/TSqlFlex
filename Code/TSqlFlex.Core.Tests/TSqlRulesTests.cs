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

        [Test()]
        public void Space_IsConsideredWhitespace()
        {
            Assert.AreEqual(true, TSqlRules.ContainsWhitespace("a b"));
        }

        [Test()]
        public void Tab_IsConsideredWhitespace()
        {
            Assert.AreEqual(true, TSqlRules.ContainsWhitespace("a\tb"));
        }

        [Test()]
        public void LineFeed_IsConsideredWhitespace()
        {
            Assert.AreEqual(true, TSqlRules.ContainsWhitespace("a\nb"));
        }

        [Test()]
        public void CarriageReturn_IsConsideredWhitespace()
        {
            Assert.AreEqual(true, TSqlRules.ContainsWhitespace("a\rb"));
        }

        [Test()]
        public void StringsWithSquareBrackets_AreConsideredAsHavingOne()
        {
            Assert.AreEqual(true, TSqlRules.ContainsSquareBracket("a[b"));
            Assert.AreEqual(true, TSqlRules.ContainsSquareBracket("a]b"));
        }

        [Test()]
        public void StringWithoutSquareBracket_IsNotConsideredAsHavingOne()
        {
            Assert.AreEqual(false, TSqlRules.ContainsSquareBracket("ab"));
        }
    }
}
