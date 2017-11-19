using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CRMAPI.Core.Repository
{
    public static class DatabaseName
    {
        public static string ConnectionString { get { return ConfigurationManager.AppSettings["ConnectionString"]; } }

    }
}
