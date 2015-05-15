using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using System.IO;

namespace TSqlFlex.Core.Tests
{
    [TestFixture()]
    class CSVReaderTests
    {

        [Test()]
        public void can_get_stream()
        {
            Stream stream = CSVExampleUTF8CRLF();
            Assert.IsNotNull(stream);
            Assert.AreEqual(true,stream.CanRead, "Expected stream to be readable");
        }

        [Test()]
        public void can_read_stream()
        {
            Stream stream = CSVExampleUTF8CRLF();
            CSVReader reader = new CSVReader(stream, Encoding.UTF8, true, 10);
            Assert.AreEqual(true, stream.CanRead, "Expected stream to be readable");
        }

        private Stream CSVExampleUTF8CRLF()
        {
            return Utils.GetResourceAsStream(@"TSqlFlex.Core.Tests.Resources.CSVExampleUTF8CRLF.txt");
        }
    
    }
}
