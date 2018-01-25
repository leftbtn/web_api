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
using CommonFrameWork;

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

        #region 保存用户的评论的回复
        [HttpPost]
        public ResultData SaveReply([FromBody]SaveCommentDate m)
        {
            var result = new ResultData();
            var o = new Reply();
            var msg = string.Empty;
            o.CommentId = m.CommentId;
            o.FromId = m.FromId;
            o.ToId = m.ToId;
            o.Details = m.Details;
            result.success = Transaction(new Func<bool>(delegate ()
            {
                result.success = BLLService.CommentServices.SaveReply(o, out msg);
                return result.success;
            }), ref msg);
            result.msg = msg;
            return result;
        }
        #endregion



        #region 根据文章id获取评论列表
        [HttpGet]
        public CommentListResult CommentList(string id)
        {
            var Result = new CommentListResult();
        
            var o = db.Comment.Where(c=>c.ArticleId == id).OrderByDescending(c=>c.CreateDateTime).ToList();
            foreach (var item in o)
            {
                var ReplyList = new List<CommentListResult.Reply>();
                var user = db.User.Where(q => q.Id == item.UserId).FirstOrDefault();
                if (BLLService.CommentServices.ExistReply(item.Id))
                {
                    var l = db.Reply.Where(c => c.CommentId == item.Id).OrderBy(c=>c.CreateDateTime).ToList();
                    foreach(var r in l)
                    {
                        var fromAccont = BLLService.AccountServices.GetUserNikeNameForId(r.FromId);
                        var toAccount = BLLService.AccountServices.GetUserNikeNameForId(r.ToId);
                        ReplyList.Add(new CommentListResult.Reply
                        {
                            Id = r.Id,
                            CommentId = r.CommentId,
                            details = r.Details,
                            FromAccount = fromAccont,
                            FromId = r.FromId,
                            ToAccount = toAccount,
                            createDateTime = Tools.ToDateTimeString(r.CreateDateTime)
                        });
                    }
                }
                Result.CommentList.Add(new CommentListResult.Comment
                {
                    id = item.Id,
                    articleId = item.ArticleId,
                    userAccount = user.Account,
                    userName = user.NikeName,
                    userImg = user.HeadImg??"",
                    userId = item.UserId,
                    details = item.Details,
                    createDateTime = Tools.ToDateString(item.CreateDateTime),
                    ReplyList = ReplyList
                });
            }
            return Result;
        }

        #endregion

        #region 根据评论id删除内容
        [HttpPost]
        public ResultData DeleteCommentForId([FromBody]DeletePostData data)
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
