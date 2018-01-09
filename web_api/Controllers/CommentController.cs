using apiServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using web_api.Models.Blog;
using web_api.Models.Comment;
using web_api.Models.Other;

namespace web_api.Controllers
{
    public class CommentController : BaseController
    {
        #region 保存用户的评论
        [HttpPost]
        public ResultData SaveComment([FromBody]SaveCommentDate m)
        {
            var result = new ResultData();
            var o = new Comment();
            var msg = string.Empty;
            o.ArticleId = m.ArticleId;
            o.UserId = m.UserId;
            o.Details = m.Details; 
            result.success = Transaction(new Func<bool>(delegate ()
            {
                result.success = BLLService.CommentServices.SaveCommentForId(o, out msg);
                return result.success;
            }), ref msg);
            result.msg = msg;
            return result;
        }
        #endregion


        #region 根据获取评论id删除内容
        [HttpPost]
        public ResultData DeleteBlogForId([FromBody]DeletePostData data)
        {
            var result = new ResultData();
            result.success = false;
            var msg = string.Empty;
            result.success = BLLService.CommentServices.Delete(data.ArticleId, out msg);
            result.msg = msg;
            return result;
        }
        #endregion
    }
}
