using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;


namespace TSqlFlex.Core.Tests
{
    [TestFixture()]
    class ExcelLauncherTests
    {
        [Test()]
        public void quoteThePath_GivenAnUnquotedString_QuotesIt()
        {
            Assert.AreEqual("\"C:\\My Folder\\Something.xml\"", TSqlFlex.Core.ExcelLauncher.quoteThePath("C:\\My Folder\\Something.xml"));
        }

        [Test()]
        public void quoteThePath_GivenAQuotedString_DoesNotQuoteIt()
        {
            Assert.AreEqual("\"C:\\My Folder\\Something.xml\"", TSqlFlex.Core.ExcelLauncher.quoteThePath("\"C:\\My Folder\\Something.xml\""));
        }
    }
}
