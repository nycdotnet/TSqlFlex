using System;
using System.Globalization;
using System.Threading;
using NUnit.Framework;


namespace TSqlFlex.Core.Tests
{
    [TestFixture()]
    class InternationalTests
    {
        private CultureInfo originalCulture;
        private CultureInfo originalUICulture;

        [TestFixtureSetUp()]
        public void StartCultureTesting()
        {
            originalCulture = Thread.CurrentThread.CurrentCulture;
            originalUICulture = Thread.CurrentThread.CurrentUICulture;
        }

        [TestFixtureTearDown()]
        public void EndCultureTesting()
        {
            Thread.CurrentThread.CurrentCulture = originalCulture;
            Thread.CurrentThread.CurrentUICulture = originalUICulture;
        }

        [Test()]
        public void DATETIME2_Data_ScriptsCorrectlyIfTimeSeparatorIsDot()
        {

            var usePeriodsForTime = new DateTimeFormatInfo();
            usePeriodsForTime.TimeSeparator = ".";

            var italyWithPeriods = new CultureInfo("it-IT");
            italyWithPeriods.DateTimeFormat = usePeriodsForTime;

            Thread.CurrentThread.CurrentCulture = italyWithPeriods;
            Thread.CurrentThread.CurrentUICulture = italyWithPeriods;

            DateTime baseData = new DateTime(2000, 10, 31, 2, 33, 44);
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "datetime2", false, 0, 0);
            Assert.AreEqual("'2000-10-31T02:33:44'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "datetime2 no fractional seconds");

            baseData = baseData.AddTicks(1234567);
            data = baseData;
            Assert.AreEqual("'2000-10-31T02:33:44.1234567'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "datetime2 fractional seconds");

            baseData = baseData = new DateTime(2000, 10, 31, 2, 33, 44).AddMilliseconds(100);
            data = baseData;
            Assert.AreEqual("'2000-10-31T02:33:44.1'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "partial fractional seconds");

            baseData = new DateTime(2000, 10, 31, 0, 0, 0);
            data = baseData;
            Assert.AreEqual("'2000-10-31'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "midnight omits time altogether");
        }
    }
}
