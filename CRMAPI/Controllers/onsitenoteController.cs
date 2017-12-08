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
        public string SetPushNotification(string id, string status)
        {
            string boolReturn = "true";
            try
            {
                tek_onsitenote onsite = mobileRepository.GetOnSiteNote(id);
                string user_token = mobileRepository.GetToken(status);

                iOSFcmPushMessage fpmReturn = new iOSFcmPushMessage();
                //組合要傳送的字串
                fpmReturn.APIKey = "AAAAfvUhnv8:APA91bFaChaP_0X0ypjInh63Hj87kqUpDFsTkjg_pZeMSdvpOK77QmPOg5iLOjFKERawonUtVPsY9oUWQ8pKuBceHqB1VBQdwBW16w9JlpSVQ4xurPBX6pL34bFlisUZ_Spx4sNVGHcQ";
                fpmReturn.RegID = user_token;
                fpmReturn.Message = new iOSNotificationStruct
                {
                    Title = "你有一筆新的派工，維修單號：" + onsite.tek_repair_no,
                    Body = (onsite.tek_serviceaccount + onsite.tek_note)
                };

                var result = "-1";//
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
                    if (oJSON["error"].ToString().Length > 0)
                    {
                        boolReturn = "false";
                    }
                    //sReturn = oJSON["error"].ToString();
                }
                //returnStr.Append(responseStr + "\n");
            }
            catch (Exception)
            {
                boolReturn = "false";
            }
            return boolReturn;
        }
    }
}