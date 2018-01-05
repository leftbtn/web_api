using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CommonFrameWork;
using web_api.Models.Other;
using apiServices;
using web_api.Models.Account;

namespace web_api.Controllers
{
    public class AccountController : BaseController
    {
        #region 注册一个帐号
        [HttpPost]
        public ResultData Register([FromBody]RegisterPostData data)
        {
            var result = new ResultData();
            result.success = false;
            var msg = string.Empty;
            if (string.IsNullOrEmpty(data.Account) || string.IsNullOrEmpty(data.Password)) { msg = "帐号密码均不能为空"; return result; }
            if (string.IsNullOrEmpty(data.NikeName)) { msg = "昵称不能为空"; return result; }
            if (BLLService.AccountServices.ExistAccount(data.Account)) { msg = "用户名已注册"; return result; }
            var o = new User();
            o.Account = data.Account;
            o.Password = data.Password;
            o.NikeName = data.NikeName;
            result.success = Transaction(new Func<bool>(delegate ()
            {
                result.success = BLLService.AccountServices.Register(o, out msg);
                return result.success;
            }), ref msg);
            result.msg = msg;
            return result;
        }
        #endregion

        #region 登录
        [HttpPost]
        public ResultData Login([FromBody]LoginPostData data)
        {
            var result = new ResultData();
            result.success = false;
            var msg = string.Empty;
            if (string.IsNullOrEmpty(data.Account) || string.IsNullOrEmpty(data.Password))
            {
                msg = "帐号密码均不能为空";
                return result;
            }
            result.success = BLLService.AccountServices.Login(data.Account, data.Password, out msg);
            result.msg = msg;
            return result;
        }
        #endregion

        #region 根据Id获取用户基本信息
        public UserInfo GetUserInfro(string userid)
        {
            var result = new UserInfo();
            result.success = false;
            result.msg = string.Empty;
            var o = db.User.Where(c => c.Id == userid).FirstOrDefault();
            if (o == null)
            {
                result.msg = "找不到该用户";
                return result;
            }
            result.success = true;
            result.Account = o.Account;
            result.NikeName = o.NikeName;
            result.CreateDateTime = Tools.ToDateTimeString(o.CreateDateTime);
            return result;
        }
        #endregion
    }

}