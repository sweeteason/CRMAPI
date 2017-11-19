using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CRMAPI.Core.Entity;
using CRMAPI.Core.Repository;

namespace CRMAPI.Tests
{
    [TestClass]
    public class APITest
    {
        MobileTimeRepository mobileTimeRepository = new MobileTimeRepository("Data Source=crmdbv.database.windows.net;Initial Catalog=CRMDB;User=eason_yu;Pwd=Aa1111111;");

        [TestMethod]
        public void GetMobileTimesTest()
        {
            List<tek_mobiletime> mobiletime = mobileTimeRepository.GetMobileTimeList();
            Assert.AreEqual(true, mobiletime.Count > 0);
        }

        [TestMethod]
        public void GetMobiletimeByNo()
        {
            tek_mobiletime mobiletime = mobileTimeRepository.GetMobiletimeByNo("1234567890");
            Assert.AreNotEqual(null, mobiletime);
        }

        [TestMethod]
        public void GetMobiletimeByStatus()
        {
            List<tek_mobiletime> mobiletime = mobileTimeRepository.GetMobiletimeByStatus("1");
            Assert.AreEqual(true, mobiletime.Count > 0);
        }
    }
}
