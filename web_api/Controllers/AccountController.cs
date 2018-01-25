using System;

using System.Linq;

using System.Web.Http;
using CommonFrameWork;
using web_api.Models.Other;
using apiServices;
using web_api.Models.Account;
using System.IO;
using System.Web;

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
            result.msg = string.Empty;
            var msg = string.Empty;
            if (string.IsNullOrEmpty(data.Account) || string.IsNullOrEmpty(data.Password)) { result.msg = "帐号密码均不能为空"; return result; }
            if (string.IsNullOrEmpty(data.NikeName)) { result.msg = "昵称不能为空"; return result; }
            if (BLLService.AccountServices.ExistAccount(data.Account)) { result.msg = "用户名已注册"; return result; }
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
                result.msg = "帐号密码均不能为空";
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
            result.CreateDateTime =  Tools.ToDateString(o.CreateDateTime);
            return result;
        }
        #endregion


        #region 保存用户的详细信息
        [HttpPost]
        public ResultData SaveUserDeatailInfo([FromBody]SaveUserPostData data)
        {
            var result = new ResultData();
            result.success = false;
            result.msg = string.Empty;
            var msg = string.Empty;
            var o = new User();
            o.Id = data.UserId;
            o.Address = data.Addres;
            if (!string.IsNullOrEmpty(data.birthday)) { o.birthday = Convert.ToDateTime(data.birthday); }
            o.Email = data.Email;
            o.HeadImg = data.HeadImg;
            o.NikeName = data.NikeName;
            o.Phone = data.Phone;
            o.HeadImg = data.HeadImg;
            result.success = Transaction(new Func<bool>(delegate ()
            {
                result.success = BLLService.AccountServices.SaveUserINfoDetail(o, out msg);
                return result.success;
            }), ref msg);
            result.msg = msg;
            return result;
        }
        #endregion

        #region 获取一个用户的详细信息
        [HttpGet]
        public UserDetailInfo GetUserDeatilInfo(string userid)
        {
            var result = new UserDetailInfo();
            result.success = false;
            result.msg = string.Empty;
            var o = db.User.Where(c => c.Id == userid).FirstOrDefault();
            if (o == null)
            {
                result.msg = "找不到该用户";
                return result;
            }
            result.success = true;
            result.Email = o.Email;
            result.birthday =  o.birthday == null? "" : Tools.ToDateString(o.birthday);
            result.Address = o.Address;
            result.Phone = o.Phone;
            result.NikeName = o.NikeName;
            result.Account = o.Account;
            result.HeadImg = o.HeadImg;
            return result;
        }

        #endregion



        #region 保存用户的头像
        [HttpPost]
        public string UploadImg()
        {
            var File = HttpContext.Current.Request.Files["file"];
            if (File == null)
            {
                return JsonHelper.ToJson(new { status = false, msg = "图片上传失败！" });
            }

            bool IsImg = false;
            string fileExtension = System.IO.Path.GetExtension(File.FileName).ToLower();
            string[] allowedExtensions = { ".gif", ".png", ".bmp", ".jpg" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    IsImg = true;
                }
            }

            if (!IsImg)
            {
                return JsonHelper.ToJson(new { status = false, msg = "上传的文件必须是图片！" });
            }

            string fileName = SystemHelper.CreateGuid() + fileExtension;

            var UpLoadImagePath = Tools.GetWebPhysicalPath() + "UploadImages/";

            if (!Directory.Exists(UpLoadImagePath))
            {
                Directory.CreateDirectory(UpLoadImagePath);
            }
            File.SaveAs(UpLoadImagePath + fileName);
            //FileUpload.SaveToRemoteServer(File, ConfigurationManager.AppSettings["UpLoadImagePath"], fileName);
            string ShowImage = Tools.GetWebPath() + "/UploadImages/" + fileName;

            return JsonHelper.ToJson(new { status = true, msg = ShowImage });
        }
        #endregion
    }

}