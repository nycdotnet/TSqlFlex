using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml;

namespace TSqlFlex.Core.Tests
{
    [TestFixture()]
    public class XmlSpreadsheetRenderTests
    {
        [Test()]
        public void RenderingTimeField_RendersCorrectly()
        {
            var frs = new FlexResultSet();

            var dt = new List<SQLColumn>() {
                SchemaScriptingTests.FakeColumn("ColIntNotNull", "MyStuff", 32, "int", false, 255, 255),
                SchemaScriptingTests.FakeColumn("ColIntNull", "MyStuff", 32, "int", true, 255, 255),
                SchemaScriptingTests.FakeColumn("ColTimeOfDay", "MyStuff", 32, "time", true, 255, 255)
            };

            var result = new FlexResult() {
                schema = dt,
                data = new List<object[]>() {
                    new object[] {99, 111, new TimeSpan(0, 00, 00, 00, 001)},
                    new object[] {42, null, new TimeSpan(23,59,30)},
                }
            };

            frs.results.Add(result);

            var tempFileName = Guid.NewGuid().ToString() + ".txt";

            var srp = new SqlRunParameters(new SqlConnectionStringBuilder(), "", SqlRunParameters.TO_XML_SPREADSHEET, tempFileName);
            
            XmlSpreadsheetRenderer.renderAsXMLSpreadsheet(frs, srp);

            var xmlSpreadsheetContent = srp.getOutputStreamAsString(tempFileName);

            Assert.IsTrue(xmlSpreadsheetContent.Length > 1000, "expected more than 1000 characters of output");

            Assert.IsTrue(xmlSpreadsheetContent.Contains(">99<"));
            Assert.IsTrue(xmlSpreadsheetContent.Contains(">111<"));
            Assert.IsTrue(xmlSpreadsheetContent.Contains(">1899-12-31T00:00:00.001<"));
            Assert.IsTrue(xmlSpreadsheetContent.Contains(">42<"));
            Assert.IsTrue(xmlSpreadsheetContent.Contains(">1899-12-31T23:59:30.000<"));

            XmlDocument doc = null;

            Assert.DoesNotThrow(() => {
                doc = new XmlDocument();
                doc.LoadXml(xmlSpreadsheetContent);
            }, "expected no exception");

            Assert.IsNotNull(doc, "expected valid XML");
        }
    }
}
