using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_api.Models.Other;

namespace web_api.Models.Account
{
    public class UserDetailInfo : ResultData
    {
        public string Account { get; set; }
        public string NikeName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string birthday { get; set; }
    }
}