﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMAPI.Core.Entity
{
    public class AppRequestModels
    {
        private string _RegistrationID;

        public string RegistrationID
        {
            get { return _RegistrationID; }
            set { _RegistrationID = value; }
        }

    }
}