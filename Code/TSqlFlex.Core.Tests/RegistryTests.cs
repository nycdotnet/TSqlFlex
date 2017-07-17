using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TSqlFlex.Core.Tests
{
    [TestFixture()]
    public class RegistryTests
    {

        [Test()]
        public void GettingRegistryDWordFromKeyThatDoesNotExist_ReturnsNull()
        {
            var dword = Config.getLocalMachineDWORD("THIS\\Key\\DOES\\NOT\\Exist\\", "askldfhjaslkdfjhasdklfjhasldkf");
            Assert.IsFalse(dword.HasValue);
        }

        /// <summary>
        /// This test assumes there is a registry key "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Setup"
        /// </summary>
        [Test()]
        public void GettingRegistryDWordFromKeyThatExistsButValueThatDoesNot_ReturnsNull()
        {
            var dword = Config.getLocalMachineDWORD("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Setup", "askldfhjaslkdfjhasdklfjhasldkf");
            Assert.IsFalse(dword.HasValue);
        }

        /// <summary>
        /// This test assumes there is a registry key "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Setup" with DWORD LogLevel and a number > 0
        /// </summary>
        [Test()]
        public void GettingRegistryDWordFromKeyAndValueThatExistsWorks()
        {
            var dword = Config.getLocalMachineDWORD("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Setup", "LogLevel");
            Assert.IsTrue(dword.HasValue);
        }

        [Test()]
        public void GettingRegistrySzFromKeyThatDoesNotExist_ReturnsNull()
        {
            var regsz = Config.getLocalMachineREGSZ("THIS\\Key\\DOES\\NOT\\Exist\\", "askldfhjaslkdfjhasdklfjhasldkf");
            Assert.IsNull(regsz);
        }

        /// <summary>
        /// This test assumes there is a registry key "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Setup"
        /// </summary>
        [Test()]
        public void GettingRegistrySzFromKeyThatExistsButValueThatDoesNot_ReturnsNull()
        {
            var regsz = Config.getLocalMachineREGSZ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Setup", "askldfhjaslkdfjhasdklfjhasldkf");
            Assert.IsNull(regsz);
        }

        /// <summary>
        /// This test assumes there is a registry key "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Setup" with REG_SZ BootDir with content
        /// </summary>
        [Test()]
        public void GettingRegistrySzFromKeyAndValueThatExistsWorks()
        {
            var regsz = Config.getLocalMachineREGSZ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Setup", "BootDir");
            Assert.IsNotNull(regsz);
            Assert.IsTrue(regsz.Length > 0);
        }

    }
}
