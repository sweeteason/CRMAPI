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
    public class onsitenoteController : ApiController
    {
        MobileRepository mobileRepository = new MobileRepository(DatabaseName.ConnectionString);

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
            tek_onsitenote onsite = mobileRepository.GetOnSiteNote(id);
            if (onsite == null)
            {
                return "No Record！此單號" + id + "(使用者：" + onsite.tek_m_user + ")，查無留言";
            }
            try
            {
                string user_token = mobileRepository.GetToken(onsite.tek_m_user);

                iOSFcmPushMessage fpmReturn = new iOSFcmPushMessage();
                //組合要傳送的字串
                fpmReturn.APIKey =
                    "AAAAfvUhnv8:APA91bFaChaP_0X0ypjInh63Hj87kqUpDFsTkjg_pZeMSdvpOK77QmPOg5iLOjFKERawonUtVPsY9oUWQ8pKuBceHqB1VBQdwBW16w9JlpSVQ4xurPBX6pL34bFlisUZ_Spx4sNVGHcQ";
                fpmReturn.RegID = user_token;
                fpmReturn.Message = new iOSNotificationStruct
                {
                    Title = "你有一筆新的留言，維修單號：" + onsite.tek_repair_no,
                    Body = (onsite.tek_serviceaccount + onsite.tek_note)
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
                    oJSON = (JObject)oJSON["results"][0];
                    if (oJSON["error"].ToString() == "InvalidRegistration" ||
                        oJSON["error"].ToString() == "NotRegistered")
                    {
                        //無效的RegistrationID
                    }
                    if (oJSON["error"].ToString().Length > 0)
                    {
                        mobileRepository.UpdateOnsitenoteStatus(onsite.tek_repair_no, "error", oJSON["error"].ToString());
                        return "false";
                    }                    
                    //sReturn = oJSON["error"].ToString();
                }
                else
                {
                    mobileRepository.UpdateOnsitenoteStatus(onsite.tek_repair_no, "complete", "");
                }
                //returnStr.Append(responseStr + "\n");
                return boolReturn;
            }
            catch (Exception ex)
            {
                //throw ex;
                mobileRepository.UpdateOnsitenoteStatus(onsite.tek_repair_no, "error", ex.Message);
                return "false";
            }
            //finally
            //{
            //    mobileRepository.UpdateOnsitenoteStatus(onsite.tek_repair_no, onsite.Status, "");
            //}
            //return boolReturn;

            ///// <summary>
            ///// 留言變更狀態
            ///// </summary>
            ///// <param name="tek_name">維修單號</param>
            ///// <param name="status">狀態</param>
            ///// <param name="Log">Log</param>
            //public void UpdateOnsitenoteStatus(string tek_repair_no, string status, string Log)
        }
    }
}