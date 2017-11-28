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

        ///// <summary>
        ///// 取得外部傳來的資料
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public AppRequestModels PostAppData(AppRequestModels obj)
        //{
        //    AppRequestModels armReturn = new AppRequestModels();

        //    armReturn.RegistrationID = obj.RegistrationID;

        //    return armReturn;
        //}

        ///// <summary>
        ///// 由外部傳來的驗證資料，組合FireBase需要做推播的功能
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public FCMPushMessage PushMessage(FCMPushMessage obj)
        //{
        //    FCMPushMessage fpmReturn = new FCMPushMessage();

        //    fpmReturn.APIKey = obj.APIKey;
        //    fpmReturn.RegID = obj.RegID;
        //    fpmReturn.Message = obj.Message;

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
        //    request.Method = "POST";
        //    request.ContentType = "application/json;charset=utf-8;";
        //    request.Headers.Add($"Authorization: key={fpmReturn.APIKey}");

        //    string RegistrationID = fpmReturn.RegID;
        //    var postData =
        //    new
        //    {
        //        data = new
        //        {
        //            message = fpmReturn.Message //message這個tag要讓前端開發人員知道
        //        },
        //        registration_ids = new string[] { RegistrationID }
        //    };
        //    string p = JsonConvert.SerializeObject(postData);//將Linq to json轉為字串
        //    byte[] byteArray = Encoding.UTF8.GetBytes(p);//要發送的字串轉為byte[]
        //    request.ContentLength = byteArray.Length;

        //    Stream dataStream = request.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();

        //    //發出Request
        //    WebResponse response = request.GetResponse();
        //    Stream responseStream = response.GetResponseStream();
        //    StreamReader reader = new StreamReader(responseStream);
        //    string responseStr = reader.ReadToEnd();
        //    reader.Close();
        //    responseStream.Close();
        //    response.Close();

        //    JObject oJSON = (JObject)JsonConvert.DeserializeObject(responseStr);
        //    if (Convert.ToInt32(oJSON["failure"].ToString()) > 0)
        //    {//有失敗情況就寫Log
        //        //EventLog.WriteEntry("發送訊息給" + RegistrationID + "失敗：" + responseStr);

        //        oJSON = (JObject)oJSON["results"][0];
        //        if (oJSON["error"].ToString() == "InvalidRegistration" || oJSON["error"].ToString() == "NotRegistered")
        //        { //無效的RegistrationID
        //          //從DB移除
        //            SqlParameter[] param = new SqlParameter[] { new SqlParameter() { ParameterName = "@RegistrationID", SqlDbType = SqlDbType.VarChar, Value = RegistrationID } };
        //            //SqlHelper.ExecteNonQuery(CommandType.Text, "Delete from tb_MyRegisID Where RegistrationID=@RegistrationID", param);

        //        }
        //    }
        //    //returnStr.Append(responseStr + "\n");

        //    return fpmReturn;
        //}
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
        /// 更新維修單狀態
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public bool UpdateStatus(string id, string status)
        {
            return mobileRepository.UpdateMobileTime(id, status);
        }

        /// <summary>
        /// 檢查登入
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public string CheckLogin(string id, string status)
        {
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