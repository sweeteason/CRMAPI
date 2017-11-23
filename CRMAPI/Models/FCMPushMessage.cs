using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FCMPostPush.Models
{
    public class FCMPushMessage
    {
        private string _regID;
        private string _APIKey;
        private string _Message;

        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public string APIKey
        {
            get { return _APIKey; }
            set { _APIKey = value; }
        }

        public string RegID
        {
            get { return _regID; }
            set { _regID = value; }
        }
    }
}