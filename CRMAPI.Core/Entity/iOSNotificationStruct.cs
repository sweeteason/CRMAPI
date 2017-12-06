using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CRMAPI.Core.Entity
{
    public class iOSNotificationStruct
    {
        private string _body;
        private string _title;

        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

    }
}
