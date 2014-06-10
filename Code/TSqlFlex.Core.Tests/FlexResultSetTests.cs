using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TSqlFlex.Core.Tests
{
    [TestFixture()]
    public class FlexResultSetTests
    {
        [Test()]
        public void CreatingEmptyFlexResultSet_ResultsInEmptyCollections()
        {
            FlexResultSet fsr = new FlexResultSet();
            Assert.IsNotNull(fsr);

            Assert.IsNotNull(fsr.results);
            Assert.AreEqual(0, fsr.results.Count);

        }
    }
}
