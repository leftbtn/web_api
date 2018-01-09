using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api.Models.Comment
{
    public class CommentListResult
    {
        public List<Comment> CommentList { get; set; }
        public CommentListResult() {
            CommentList = new List<Comment>();
        }
        public class Comment
        {
            public string id { get; set; }
            public string userName { get; set; }
            public string userAccount { get; set; }
            public string articleId { get; set; }
            public string details { get; set; }
            public string createDateTime { get; set; }
        }
    }
}