using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api.Models.Account
{
    public class SaveUserPostData
    {
        public string UserId { get; set; }
        public string NikeName { get; set; }
        public string birthday { get; set; }
        public string Phone { get; set; }
        public string Addres { get; set; }
        public string Email { get; set; }
        public string HeadImg { get; set; }
    }
}