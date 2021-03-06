﻿using System;
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

            //取得user token
            string token = mobileRepository.GetToken("傳入user id");

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
                        body = fpmReturn.Message
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
            return mobileRepository.GetRepairListByAccount(query).Where(p => p.tek_repairstatus == status);
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
        public bool SetUserToken(string id, string status)
        {
            return mobileRepository.SetUserToken(id, status);
        }

        /// <summary>
        /// 留言推播
        /// </summary>v
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public string SetPushNotification(string id)
        {
            string boolReturn = "true";

            tek_repair onsite = mobileRepository.GetRepairById(id);
            if (onsite == null)
            {
                return "No Record！此單號" + id + "(使用者：" + onsite.tek_m_user + ")，查無維修單";
            }
            try
            {

                string user_token = mobileRepository.GetToken(onsite.tek_m_user); //status



                iOSFcmPushMessage fpmReturn = new iOSFcmPushMessage();
                //組合要傳送的字串
                fpmReturn.APIKey =
                    "AAAAfvUhnv8:APA91bFaChaP_0X0ypjInh63Hj87kqUpDFsTkjg_pZeMSdvpOK77QmPOg5iLOjFKERawonUtVPsY9oUWQ8pKuBceHqB1VBQdwBW16w9JlpSVQ4xurPBX6pL34bFlisUZ_Spx4sNVGHcQ";
                fpmReturn.RegID = user_token;
                fpmReturn.Message = new iOSNotificationStruct
                {
                    Title = "你有一筆新的派工，維修單號：" + onsite.tek_name,
                    Body = (onsite.tek_account + onsite.tek_remark)
                };

                var result = "-1"; //
                var webAddr = "https://fcm.googleapis.com/fcm/send";

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json;charset=utf-8;";
                httpWebRequest.Headers.Add($"Authorization:key={fpmReturn.APIKey}");
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //轉換傳送資料成json格式
                    var json = new
                    {
                        to = fpmReturn.RegID,
                        notification = new
                        {
                            title = fpmReturn.Message.Title,
                            body = fpmReturn.Message.Body
                        }
                    };
                    string p = JsonConvert.SerializeObject(json); //將Linq to json轉為字串
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
                {
                    //有失敗情況就寫Log
                    //EventLog.WriteEntry("發送訊息給" + RegistrationID + "失敗：" + responseStr);

                    oJSON = (JObject)oJSON["results"][0];
                    if (oJSON["error"].ToString() == "InvalidRegistration" ||
                        oJSON["error"].ToString() == "NotRegistered")
                    {
                        //無效的RegistrationID
                    }
                    if (oJSON["error"].ToString().Length > 0)
                    {
                        mobileRepository.UpdateRepairStatus(onsite.tek_name, "error", oJSON["error"].ToString());
                        return "false";
                    }                    
                    //sReturn = oJSON["error"].ToString();
                }
                else
                {
                    mobileRepository.UpdateRepairStatus(onsite.tek_name, "complete", "");
                }
                //returnStr.Append(responseStr + "\n");
                return boolReturn;
            }
            catch (Exception ex)
            {
                //throw ex;
                mobileRepository.UpdateRepairStatus(onsite.tek_name, "error", ex.Message);
                return "false";
            }
            //finally
            //{
            //    mobileRepository.UpdateRepairStatus(onsite.tek_name, onsite.Status, "");
            //}
            

            ///// <summary>
            ///// 維修單變更狀態
            ///// </summary>
            ///// <param name="tek_name">維修單號</param>
            ///// <param name="status">狀態</param>
            ///// <param name="Log">Log</param>
            //public void UpdateRepairStatus(string tek_name, string status, string Log)
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