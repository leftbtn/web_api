using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonFrameWork;

namespace apiServices.BLLServices
{
    public class AccountServices: BaseService
    {
        #region 注册一个帐号
        public bool Register(User m, out string msg)
        {
            bool result = false;
            msg = string.Empty;
            try
            {
                m.Id = Tools.GetGuid();
                m.CreateDateTime = DateTime.Now;
                db.User.Add(m);
                msg = m.Id;
                result = db.SaveChanges() > 0;
            }
            catch(Exception ex)
            {
                msg = ex.Message;
            }
            return result;
        }
        #endregion

        #region 登录
        public bool Login(string Account,string Password,out string msg) 
        {
            bool result = false;
            msg = string.Empty;
            try
            {
                var o = db.User.Where(c => c.Account == Account).FirstOrDefault();
                if(o == null)
                {
                    msg = "帐号不存在";
                    return result;
                }
                if(o.Password != Password)
                {
                    msg = "密码不正确";
                    return result;
                }
                msg = o.Id;
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return result;
        }
        #endregion

        #region 判断帐号是否存在
        public bool ExistAccount(string Account)
        {
            return db.User.Where(c => c.Account == Account).FirstOrDefault() != null;
        }
        #endregion

        #region 判断用户Id是否存在
        public bool ExistUserId(string userid)
        {
            return db.User.Where(c => c.Id == userid).FirstOrDefault() != null;
        }
        #endregion
    }
}
