using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace TSqlFlex.Core.Tests
{
    [TestFixture()]
    class CSVRendererTests
    {
        [Test()]
        public void string_with_no_commas_is_not_enclosed()
        {
            string testit = "This is a test";
            Assert.AreEqual(testit, TSqlFlex.Core.CSVRenderer.escapeForCSV(testit));
        }

        [Test()]
        public void string_with_commas_is_enclosed()
        {
            string testit = "Developer, Jane Q.";
            Assert.AreEqual("\"" + testit + "\"", TSqlFlex.Core.CSVRenderer.escapeForCSV(testit));
        }

        [Test()]
        public void string_starting_with_zero_is_enclosed()
        {
            string testit = "00000000";
            Assert.AreEqual("\"" + testit + "\"", TSqlFlex.Core.CSVRenderer.escapeForCSV(testit));
        }

        [Test()]
        public void string_starting_with_numeric_nonzero_is_not_enclosed()
        {
            string testit = "10000000";
            Assert.AreEqual(testit, TSqlFlex.Core.CSVRenderer.escapeForCSV(testit));
        }

        [Test()]
        public void string_that_is_lone_zero_is_not_enclosed()
        {
            string testit = "0";
            Assert.AreEqual(testit, TSqlFlex.Core.CSVRenderer.escapeForCSV(testit));
        }
    }
}
