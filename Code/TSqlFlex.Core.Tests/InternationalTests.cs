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
        public void DATETIME2_WhenTimeSeparatorIsDot_ScriptsCorrectly()
        {

            setToItalianUsingDotTimeSeparator();

            DateTime baseData = new DateTime(2000, 10, 31, 2, 33, 44);
            object data = baseData;
            
            Assert.AreEqual("'2000-10-31T02:33:44'", FieldScripting.formatDateTime(data, true), "we expect to have : as the time separator despite the set culture.");

        }

        [Test()]
        public void DECIMAL_WhenDecimalSeparatorIsPeriod_ScriptsCorrectly()
        {
            setToItalianUsingCommaDecimalSeparator();

            Decimal baseData = 1.5M;
            object data = baseData;

            Assert.AreEqual("1.5", FieldScripting.formatDecimal(data), "we expect to have . as the decimal separator despite the set culture.");
        }

        [Test()]
        public void DOUBLE_WhenDecimalSeparatorIsPeriod_ScriptsCorrectly()
        {
            setToItalianUsingCommaDecimalSeparator();

            double baseData = 1.5;
            object data = baseData;

            Assert.AreEqual("1.5", FieldScripting.formatDouble(data), "we expect to have . as the decimal separator despite the set culture.");
        }

        [Test()]
        public void SINGLE_WhenDecimalSeparatorIsPeriod_ScriptsCorrectly()
        {
            setToItalianUsingCommaDecimalSeparator();

            float baseData = 1.5F;
            object data = baseData;

            Assert.AreEqual("1.5", FieldScripting.formatSingle(data), "we expect to have . as the decimal separator despite the set culture.");
        }

        private static void setToItalianUsingDotTimeSeparator()
        {
            var usePeriodsForTime = new DateTimeFormatInfo();
            usePeriodsForTime.TimeSeparator = ".";

            var italyWithPeriods = new CultureInfo("it-IT");
            italyWithPeriods.DateTimeFormat = usePeriodsForTime;

            Thread.CurrentThread.CurrentCulture = italyWithPeriods;
            Thread.CurrentThread.CurrentUICulture = italyWithPeriods;
        }


        private static void setToItalianUsingCommaDecimalSeparator()
        {
            var useCommaForDecimalSeparator = new NumberFormatInfo();
            useCommaForDecimalSeparator.NumberDecimalSeparator = ",";

            var italyWithCommas = new CultureInfo("it-IT");
            italyWithCommas.NumberFormat = useCommaForDecimalSeparator;

            Thread.CurrentThread.CurrentCulture = italyWithCommas;
            Thread.CurrentThread.CurrentUICulture = italyWithCommas;
        }
    }
}
