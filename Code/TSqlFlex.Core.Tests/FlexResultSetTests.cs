using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TSqlFlex.Core;

namespace TSqlFlex.Core.Tests
{
    [TestFixture()]
    public class FlexResultSetTests
    {

        [Test()]
        public void CanCreateEmptyFlexResultSet()
        {
            FlexResultSet fsr = new FlexResultSet();
            Assert.IsNotNull(fsr);
            
            Assert.IsNotNull(fsr.results);
            Assert.AreEqual(0, fsr.results.Count);

            Assert.IsNotNull(fsr.schemaTables);
            Assert.AreEqual(0, fsr.schemaTables.Count);

            Assert.IsNotNull(fsr.exceptions);
            Assert.AreEqual(0, fsr.exceptions.Count);

        }
    }
}
