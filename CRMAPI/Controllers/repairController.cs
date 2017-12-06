using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using CRMAPI.Core.Entity;
using CRMAPI.Core.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CRMAPI.Controllers
{
    public class repairController : ApiController
    {
        MobileRepository mobileRepository = new MobileRepository(DatabaseName.ConnectionString);

        /// <summary>
        /// 由外部傳來的驗證資料，組合FireBase需要做推播的功能
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public String PushMessage(iOSFcmPushMessage obj)
        {
            string sReturn = "";
            iOSFcmPushMessage fpmReturn = new iOSFcmPushMessage();

            fpmReturn.APIKey = obj.APIKey;
            fpmReturn.RegID = obj.RegID;
            fpmReturn.Message = obj.Message;

            var result = "-1";
            var webAddr = "https://fcm.googleapis.com/fcm/send";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json;charset=utf-8;";
            httpWebRequest.Headers.Add($"Authorization:key={fpmReturn.APIKey}");
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var json = new
                {
                    to = fpmReturn.RegID,
                    notification = new
                    {
                        body=fpmReturn.Message
                    }
                };
                string p = JsonConvert.SerializeObject(json);//將Linq to json轉為字串
                streamWriter.Write(p);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            JObject oJSON = (JObject)JsonConvert.DeserializeObject(result);
            if (Convert.ToInt32(oJSON["failure"].ToString()) > 0)
            {//有失敗情況就寫Log
                //EventLog.WriteEntry("發送訊息給" + RegistrationID + "失敗：" + responseStr);

                oJSON = (JObject)oJSON["results"][0];
                if (oJSON["error"].ToString() == "InvalidRegistration" || oJSON["error"].ToString() == "NotRegistered")
                { //無效的RegistrationID
                  //從DB移除
                    //SqlParameter[] param = new SqlParameter[] { new SqlParameter() { ParameterName = "@RegistrationID", SqlDbType = SqlDbType.VarChar, Value = RegistrationID } };
                    //SqlHelper.ExecteNonQuery(CommandType.Text, "Delete from tb_MyRegisID Where RegistrationID=@RegistrationID", param);

                }
                sReturn = oJSON["error"].ToString();
            }
            //returnStr.Append(responseStr + "\n");

            return sReturn;
        }


        /// <summary>
        /// 取得全部維修資料
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<tek_repair> GetRepair(int page = 1, int pageSize = 10)
        {
            QueryList query = new QueryList();
            query.Page = page;
            query.PageSize = pageSize;
            return mobileRepository.GetRepairList(query);
        }

        /// <summary>
        /// 依照員工帳號取得維修資料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        // GET api/<controller>/5
        public IEnumerable<tek_repair> GetRepair(string id, int page = 1, int pageSize = 10)
        {
            QueryList query = new QueryList();
            query.Page = page;
            query.PageSize = pageSize;
            query.Keyword = id;
            return mobileRepository.GetRepairListByAccount(query);
        }

        /// <summary>
        /// 依照員工帳號與維修狀態，取得維修資料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<tek_repair> GetRepair(string id, string status, int page = 1, int pageSize = 10)
        {
            QueryList query = new QueryList();
            query.Page = page;
            query.PageSize = pageSize;
            query.Keyword = id;
            return mobileRepository.GetRepairListByAccount(query).Where(p=> p.tek_repairstatus == status);
        }

        /// <summary>
        /// 寫入預約記錄
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="reserve"></param> 
        /// <returns></returns>
        [HttpGet]
        public bool AddReserve(string id, string status, string user, string reserve)
        {
            bool flag = mobileRepository.AddReserve(id, status, user, reserve);
            MService.MobileServiceSoapClient ms = new MService.MobileServiceSoapClient();
            return (ms.SyncMobile("SyncMobiletime", id, "PLUGIN") == "Succeed!") && flag;
        }

        /// <summary>
        /// 寫入預約記錄
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="reserve"></param> 
        /// <returns></returns>
        [HttpGet]
        public bool AddGPS(string id, string status, string user, string gps)
        {
            bool flag = mobileRepository.AddGPS(id, status, user, gps);
            MService.MobileServiceSoapClient ms = new MService.MobileServiceSoapClient();
            return (ms.SyncMobile("SyncMobiletime", id, "PLUGIN") == "Succeed!") && flag;
        }

        /// <summary>
        /// 寫入預約記錄
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public bool AddReserve(string id, string status, string user)
        {
            bool flag = mobileRepository.AddReserve(id, status, user);
            MService.MobileServiceSoapClient ms = new MService.MobileServiceSoapClient();
            return (ms.SyncMobile("SyncMobiletime", id, "PLUGIN") == "Succeed!") && flag;             
        }


        /// <summary>
        /// 留言推播
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public void SetPushNotification(string id)
        {
            tek_onsitenote onsite = mobileRepository.GetOnSiteNote(id);

        }

        /// <summary>
        /// 檢查登入
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public string CheckLogin(string id, string status)
        {
            //return status + "!";
            return mobileRepository.CheckLogin(id, status);
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }


        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}