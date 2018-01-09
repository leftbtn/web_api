using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api.Models.Comment
{
    public class SaveCommentDate
    {
        public string UserId { get; set; }
        public string ArticleId { get; set; }
        public string Details { get; set; }
    }
}