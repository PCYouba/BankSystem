using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankSystem.Users.Models;
using BankSystem.Users.Servies;
using PetaPoco;
using Core;
using Core.Code;
using System.Text.RegularExpressions;
using BankSystem.Users.Controllers.Filters;
using BankSystem.Core.Model;

namespace BankSystem.Controllers
{
    public class UsersController : Controller
    {

        UserReg ur = new Users.Servies.UserReg();
        #region 验证码
        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckCode()
        {
            //图片的格式并且进行切换图片
            return File(Code.CheckCode.RndCodeImg(), "image/gif");
        }
        #endregion
        #region 登陆
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="uInfo"></param>
        /// <returns></returns>
        public ActionResult Login(LoginInfo lInfo)
        {
            FormCollection FC = new FormCollection();            
            //lInfo.Account = FC["LoginCode"].PadLeft(8, '0');
            //lInfo.pass = FC["LoginPWD"];
            return View(lInfo);
        }
        [HttpPost]
        public ActionResult Login(FormCollection FC)
        {
            try
            {
                ViewBag.error = string.Empty;
                //从界面获取文本框的值
                string a = Convert.ToString(FC["LoginCode"]);
                string b = FC["LoginPWD"];
                LoginInfo lInfo = new LoginInfo();
                lInfo.Account = FC["LoginCode"].PadLeft(8, '0');
                lInfo.pass = b;
                //判断验证码是否正确              
                string UserInput = FC["CheckCode"] ?? string.Empty;
                string SavedCode = (string)Session["rndcode"];
                if (!UserInput.ToLower().Equals(SavedCode))
                {
                    ViewBag.error = "验证码错误!";
                    return View(lInfo);
                }              
                if (string.IsNullOrEmpty(FC["LoginCode"]))
                {
                    return Content("<script>alert('登录名不能为空！');document.location.href='../Users/Login';</script>");
                }
                if (!Regex.IsMatch(a, @"^\d{8}$"))
                {                    
                    return Content("<script>alert('登录名必须为8位！');document.location.href='../Users/Login';</script>");
                }
                if (string.IsNullOrEmpty(b))
                {                   
                    return Content("<script>alert('密码不能为空！');document.location.href='../Users/Login';</script>");
                }
                UserInfo uInfo = new Users.Models.UserInfo();
                uInfo.Account = Convert.ToInt32(a);
                uInfo.CreateDate = DateTime.Now;
                uInfo.LoginPass = b;
                if (a == "00000000")
                {                   
                    return Content("<script>alert('请转到管理员界面登陆！');document.location.href='../Users/Login';</script>");
                }
                //获取用户的信息
                UserReg ur = new Users.Servies.UserReg();
                UserInfo userInfo = ur.Login(uInfo);
                Session["Account"] = a;   
                //判断数据库信息是否与用户输入的信息是否相同            
                if (userInfo.Account != Convert.ToInt32(a) && userInfo.LoginPass != b)
                {
                    ViewBag.error = "账号或密码错误！";
                    return View();                   
                }              
                    return RedirectToAction("Menu");                
            }
            catch (Exception)
            {

                ViewBag.error = "请输入正确格式！";
                return View();
            }
           
                                                                                        
        }
        #endregion
        #region 管理员登陆
        /// <summary>
        /// 管理员登陆
        /// </summary>
        /// <param name="uInfo"></param>
        /// <returns></returns>
        public ActionResult guanliyuan(BankSystem.Core.Model.LoginInfo lInfo )
        {
            FormCollection FC = new FormCollection();
            //lInfo.Account = FC["LoginCode"].PadLeft(8, '0');
            //lInfo.pass = FC["LoginPWD"];
            return View(lInfo);
        }
       
        [HttpPost]
        public ActionResult guanliyuan(FormCollection FC)
        {
            try
            {
                ViewBag.error = string.Empty;
                //获取界面的text的值
                string a = FC["LoginCode"];
                string b = FC["LoginPWD"];
                BankSystem.Core.Model.LoginInfo lInfo = new BankSystem.Core.Model.LoginInfo();
                lInfo.Account = FC["LoginCode"].PadLeft(8, '0');
                lInfo.pass = FC["LoginPWD"];
                UserInfo uInfo = new Users.Models.UserInfo();
                uInfo.Account = Convert.ToInt32(a);
                uInfo.LoginPass = b;
                //验证码的判断
                string UserInput = FC["CheckCode"] ?? string.Empty;
                string SavedCode = (string)Session["rndcode"];
                if (!UserInput.ToLower().Equals(SavedCode))
                {                  
                    ViewBag.error = "验证码错误!";
                    return View(lInfo);
                }
                if (!Regex.IsMatch(a, @"^\d{8}$"))
                {                    
                    return Content("<script>alert('登录名必须为8位！');document.location.href='../Users/Login';</script>");
                }
                if (string.IsNullOrEmpty(a) && string.IsNullOrEmpty(b))
                {                    
                    ViewBag.error = "登录名或者密码不能为空!";
                    return View(lInfo);
                }                
                UserReg ur = new Users.Servies.UserReg();
                UserInfo userInfo = ur.Login(uInfo);
                if (userInfo.Account !=Convert.ToInt32(a) && userInfo.LoginPass != b)
                {
                      ViewBag.error = "账号或密码错误！";
                      return View(lInfo);
                }
                return RedirectToAction("AdminSet", "Trade");                                 
            }
            catch (Exception)
            {

                ViewBag.error = "请输入正确格式错误！";
                return View();
            }
           
            
        }
        #endregion
        #region 新用户注册
        /// <summary>
        /// 新用户注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Reg()
        {
            UserInfo uInfo = new Users.Models.UserInfo();       
            return View(uInfo);
        }
       
       // [Login]
        [HttpPost]
        public ActionResult Reg(UserInfo uInfo,FormCollection FC)
        {
            string LoginPass = FC["LoginPWD"];
            string TradePass = FC["qrTradePass"];

            if (!ModelState.IsValid)
            {
                return View(uInfo);
            }
            //判断两次密码输入的是否一致
            if (LoginPass != uInfo.LoginPass || TradePass != uInfo.TradePass)
            {
                ViewBag.LoginPass="请输入一致的密码";
                return View(uInfo);
            }             
            if(uInfo.Balance < 10)
            {
                ViewBag.show = "开户余额不能为小于10元！";
                return View(uInfo);
            }                             
            uInfo.CreateDate = DateTime.Now;           
            ur.UsersReg(uInfo);
            return RedirectToAction("Login");
        }
        #endregion
        #region 主菜单
        // GET: Users      
        /// <summary>
        /// 主菜单
        /// </summary>
        /// <returns></returns>

        public ActionResult Menu()
        {
            if(Session["Account"] == null)
            {
                return RedirectToAction("Login");
            }                                       
                UserInfo userInfo = ur.GetUserDetail(Convert.ToInt32(Session["Account"])) ;                              
            //根据int字段给性别赋值

                if (userInfo.Sex == 0)
                {
                    ViewBag.show = "男";
                }
                else
                {
                    ViewBag.show = "女";
                }
                return View(userInfo);            
        }
        #endregion
        /// <summary>
        /// 用户修改信息
        /// </summary>
        /// <param name="FC"></param>
        /// <returns></returns>
        //[Login]
        public ActionResult Update()
        {           
            //根据账号获取全部信息，在进行修改信息
            UserInfo uInfo = ur.GetUserDetail(Convert.ToInt32(Session["Account"]));            
            return View("Update", uInfo);
        }
        [HttpPost]
        public ActionResult Update(UserInfo uInfo)
        {         
           //绑定实体类 用实体类进行修改资料
            if(uInfo.Account > 0)
            {
                uInfo.CreateDate = DateTime.Now;
                ur.UserUpdate(uInfo);
            }

            return Content("<script>alert('修改成功！');document.location.href='../Users/Menu';</script>");
        }


    
    }
}