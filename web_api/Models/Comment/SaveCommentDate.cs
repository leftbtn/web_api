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
        //保存回复需要
        public string FromId { get; set; }
        public string ToId { get; set; }
        public string CommentId { get; set; }
       
    }
}