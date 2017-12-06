using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMAPI.Core.Entity
{
    public class iOSFcmPushMessage
    {
        private string _regID;
        private string _APIKey;
        private string _Message;

        /// <summary>
        /// 訊息內容
        /// </summary>
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
        /// <summary>
        /// firebase的application驗證key
        /// </summary>
        public string APIKey
        {
            get { return _APIKey; }
            set { _APIKey = value; }
        }

        /// <summary>
        /// 手機app上的token字串
        /// </summary>
        public string RegID
        {
            get { return _regID; }
            set { _regID = value; }
        }
    }
}