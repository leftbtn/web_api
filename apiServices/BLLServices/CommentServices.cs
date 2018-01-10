using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonFrameWork;

namespace apiServices.BLLServices
{
    public class CommentServices: BaseService
    {
        #region 根据文章id保存一条评论
        public bool SaveCommentForId(Comment m, out string msg)
        {
            bool result = false;
            msg = string.Empty;
            try
            {
                result = db.User.Where(c=>c.Id == m.UserId).FirstOrDefault() != null;
                if (result) { msg = "找不到该用户"; return result; }
                result = db.Blog.Where(c=>c.Id == m.ArticleId).FirstOrDefault() != null;
                if (result) { msg = "找不到该篇文章"; return result; }
                m.Id = Tools.GetGuid();
                m.CreateDateTime = DateTime.Now;
                db.Comment.Add(m);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return result;
        }

        #endregion

        #region 保存评论的回复
        public bool SaveReply(Reply m, out string msg)
        {
            bool result = false;
            msg = string.Empty;
            try
            {
                m.Id = Tools.GetGuid();
                m.CreateDateTime = DateTime.Now;
                db.Reply.Add(m);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return result;
        }
        #endregion

        #region 判断该条是否含有回复
        public bool ExistReply(string id)
        {
            return db.Reply.Where(c => c.CommentId == id).FirstOrDefault() != null;
        }

        #endregion


        #region 根据文章id删除评论
        public bool Delete(string id, out string msg)
        {
            bool result = false;
            msg = string.Empty;
            try
            {
                var o = db.Comment.Where(c => c.Id == id).FirstOrDefault();
                if (o == null)
                {
                    msg = "找不到该记录";
                    return result;
                }
                db.Comment.Remove(o);
                result = db.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message;

            }
            return result;
        }
        #endregion


    }
}
