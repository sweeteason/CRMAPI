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
        MobileRepository mobileRepository = new MobileRepository("Data Source=crmdbv.database.windows.net;Initial Catalog=CRMDB;User=eason_yu;Pwd=Aa1111111;");

        [TestMethod]
        public void GetMobileTimesTest()
        {
            QueryList query = new QueryList();
            query.Page = 1;
            query.PageSize = 10;
            List<tek_repair> mobile = mobileRepository.GetRepairList(query);
            Assert.AreEqual(true, mobile.Count > 0);
        }

        [TestMethod]
        public void GetRepairListByAccount()
        {
            QueryList query = new QueryList();
            query.Page = 1;
            query.PageSize = 10;
            query.Keyword = "aaa";
            List<tek_repair> mobile = mobileRepository.GetRepairListByAccount(query);
            Assert.AreEqual(true, mobile.Count > 0);
        }

    }
}
