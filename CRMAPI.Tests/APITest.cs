using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CRMAPI.Core.Entity;
using CRMAPI.Core.Repository;
using System.Net;
using System.IO;

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

        [TestMethod]
        public void TestPost()
        {
            // Create a request using a URL that can receive a post.   
            WebRequest request = WebRequest.Create("http://172.21.50.81/repair/CheckLogin/tekr0152");
            request.Method = "POST";
            string postData = "=2wsxcde3";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream(); 
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream); 
            string responseFromServer = reader.ReadToEnd(); 
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
        }


    }
}
