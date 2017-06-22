using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankSystem.Bank.Models;
using BankSystem.Users.Models;
using System.Text.RegularExpressions;
using BankSystem.Bank.Servies;
using BankSystem.Users.Servies;
using PetaPoco;
using Core.Code;

namespace BankSystem.Controllers
{
    public class TradeController : Controller
    {
        /// <summary>
        /// 实例化实体类
        /// </summary>
        Trade ur = new Trade();
        UserReg ur1 = new UserReg();
        #region  存款      
        /// <summary>
        /// 存款
        /// </summary>
        /// <returns></returns>
        public ActionResult InMoney(UserInfo userInfo)
        {
            //根据账号 获取该账号的信息
            int account = Convert.ToInt32(Session["Account"]);
            userInfo = ur1.GetUserDetail(account);
            return View(userInfo);
        }
        [HttpPost]
        // [Login]
        public ActionResult InMoney(FormCollection FC)
        {
            try
            {
                int account = Convert.ToInt32(Session["Account"]);
                UserInfo userInfo = ur1.GetUserDetail(account);
                //判断界面是否输入信息
                if (string.IsNullOrEmpty(FC["balance"]))
                {
                    return Content("<script>alert('必须输入存款金额！');document.location.href='../Trade/InMoney';</script>");
                }
                if (int.Parse(FC["balance"]) < 0)
                {
                    
                    return Content("<script>alert('存款金额必须大于零元！');document.location.href='../Trade/InMoney';</script>");
                }
                if (FC["balance"] != null)
                {
                    decimal ba = Convert.ToDecimal(FC["balance"]);
                    ur.Inmoney(account, ba);
                    //记录存款信息
                    TradeInfo tInfo = new Bank.Models.TradeInfo();
                    tInfo.Account = userInfo.Account;
                    tInfo.TradeType = 1;//附上默认值
                    tInfo.MoneyNum = ba;
                    tInfo.CreateDate = DateTime.Now;
                    //调用方法 实现存款
                    ur.AdminInmoney(tInfo);
                    return RedirectToAction("Menu", "Users");
                }
                else
                {
                    return View(userInfo);
                }
            }
            catch (Exception)
            {
                ViewBag.show = "请输入正确的格式！";
                return View();
            }

        }
        #endregion
        #region 取款
        /// <summary> 
        /// 取款
        /// </summary>
        /// <param name="FC"></param>
        /// <returns></returns>
        public ActionResult OutMoney(UserInfo userInfo)
        {
            int account = Convert.ToInt32(Session["Account"]);
            userInfo = ur1.GetUserDetail(account);
            return View(userInfo);
        }
        [HttpPost]
        // [Login]
        public ActionResult OutMoney(FormCollection FC)
        {
            //根据账号获取账户信息
            int account = Convert.ToInt32(Session["Account"]);
            UserInfo userInfo = ur1.GetUserDetail(account);
            string balance = FC["yue"];
            string tradePWD = FC["tradePWD"];
            //判断界面信息是否输入
            if (string.IsNullOrEmpty(FC["yue"]))
            {
                return Content("<script>alert('必须输入取款金额！');document.location.href='../Trade/OutMoney';</script>");
            }
            if (!Regex.IsMatch(FC["yue"], @"^[0-9]*$"))
            {
                ViewBag.show = "取款金额输入格式不对！";
                return View(userInfo);
            }            
            if (string.IsNullOrEmpty(FC["tradePWD"]))
            {
                return Content("<script>alert('必须输入取款密码！');document.location.href='../Trade/OutMoney';</script>");
            }
            if (int.Parse(FC["yue"]) < 0)
            {
                
                return Content("<script>alert('取款金额必须大于零元！');document.location.href='../Trade/OutMoney';</script>");

            }
            if (Convert.ToDecimal(FC["yue"]) > userInfo.Balance)
            {
                ViewBag.show = "您的余额并不足！";
                return View(userInfo);
            }
            if (tradePWD == userInfo.TradePass)
            {
                ur.Outmoney(account, Convert.ToDecimal(balance));
                //记录取款信息
                TradeInfo tInfo = new Bank.Models.TradeInfo();
                tInfo.Account = userInfo.Account;
                tInfo.TradeType = 0;//附上默认值
                tInfo.MoneyNum = Convert.ToDecimal(balance);
                tInfo.CreateDate = DateTime.Now;//创建时间为当前的时间
                ur.AdminInmoney(tInfo);
                return RedirectToAction("Menu", "Users");
            }
            else
            {
                ViewBag.show = "密码错误！！！";
                return View(userInfo);
            }
        }
        #endregion
        #region 用户之间的转账
        /// <summary>
        /// 用户之间的转账
        /// </summary>
        /// <param name="FC"></param>
        /// <returns></returns>
        public ActionResult Trans(UserInfo userInfo)
        {
            int account = Convert.ToInt32(Session["Account"]);
             userInfo = ur1.GetUserDetail(account);
            return View(userInfo);
        }
        [HttpPost]
        public ActionResult Trans(FormCollection FC)
        {
           //根据账号获取账号的详细信息
                int account = Convert.ToInt32(Session["Account"]);
                UserInfo userInfo = ur1.GetUserDetail(account);
                string balance =FC["recmoney"];
                string recaccount = FC["recaccount"];
                UserInfo uinfo = new UserInfo();
                if (string.IsNullOrEmpty(FC["recaccount"]))
                {
                    return Content("<script>alert('必须输入转账账号！');document.location.href='../Trade/Trans';</script>");
                }
                if (!Regex.IsMatch(FC["recaccount"], @"^\d{8}$"))
                {
                    ViewBag.error = "转账账户必须为8位";
                    return View(userInfo);
                }
                if (ur1.GetUserDetail(Convert.ToInt32(FC["recaccount"])) == null)
                {
                    return Content("<script>alert('用户不存在！');document.location.href='../Trade/Trans';</script>");
                }
                if (string.IsNullOrEmpty(FC["recmoney"]))
                {
                    return Content("<script>alert('必须输入转账金额！');document.location.href='../Trade/Trans';</script>");
                }
                if (int.Parse(FC["recmoney"]) < 0)
                {                   
                    return Content("<script>alert('转账金额必须大于零元！');document.location.href='../Trade/Trans';</script>");
                }
               
                uinfo.Account = Convert.ToInt32(recaccount);
                uinfo.Balance = Convert.ToDecimal(balance);
                string tradePWD = FC["tradePWD"];
            
           
                

            //userInfo.CreateDate = DateTime.Now;
            if (tradePWD != userInfo.TradePass)
                {
                    ViewBag.error = "密码错误！！";
                    return View(userInfo);
                }                                 
                ur.Outmoney(account, Convert.ToDecimal(balance));
                ur.TransInmoney(uinfo);
            //记录交易的信息，为其提供具体信息
                TradeInfo tInfo = new Bank.Models.TradeInfo();
                tInfo.Account = userInfo.Account;
                tInfo.TradeType = 2;
                tInfo.MoneyNum = Convert.ToDecimal(balance);
                tInfo.CreateDate = DateTime.Now;
                ur.AdminInmoney(tInfo);
                return RedirectToAction("Menu", "Users");
                
            }
        #endregion

        #region 注销
        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="FC"></param>
        /// <returns></returns>
        // [Login]
        public ActionResult Delete(UserInfo userInfo)
        {
            int account = Convert.ToInt32(Session["Account"]);
             userInfo = ur1.GetUserDetail(account);
            return View(userInfo);
        }
        [HttpPost]
        public ActionResult Delete(FormCollection FC)
        {
            int account = Convert.ToInt32(Session["Account"]);
            string LoginPWD = FC["LoginPWD"];
            UserInfo userInfo = ur1.GetUserDetail(account);          
            if (string.IsNullOrEmpty(FC["LoginPWD"]))
            {
                return Content("<script>alert('密码必须输入！');document.location.href='../Trade/Delete';</script>");
            }
            if (LoginPWD != userInfo.LoginPass)
            {                
                return Content("<script>alert('密码错误！');document.location.href='../Trade/Delete';</script>");
            }
            ur.DeleteUser(account);          
            return Content("<script>alert('注销成功！');document.location.href='../Trade/Delete';</script>");
        }
        #endregion
        #region 交易查询(默认显示当前用户的交易信息)
        /// <summary>
        /// 交易查询(默认显示当前用户的交易信息)
        /// </summary>
        /// <returns></returns>
        // [Login]
        public ActionResult TradeSearch()
        {
            int CurrPage;
            int.TryParse(Request.QueryString["page"], out CurrPage);
            CurrPage = CurrPage <= 0 ? 1 : CurrPage;
            int account = Convert.ToInt32(Session["Account"]);
            ViewBag.account = account;
            Page<TradeInfo> uList = ur.GetTradeList(CurrPage, 5, account);
            return View(uList);
        }
        ///// <summary>
        /// 交易查询（根据时间查询）
        /// </summary>
        /// <param name="FC"></param>
        /// <returns></returns>
    
        [HttpPost]
        public ActionResult TradeSearch(FormCollection FC)
        {
            //界面上的开始结束时间
            string StartTime = Convert.ToString(FC["StatrTime"]);
            string FinishTime = Convert.ToString(FC["FinishTime"]);            
            if (string.IsNullOrEmpty(FC["StatrTime"]))
            {
                return Content("<script>alert('开始时间未选！');document.location.href='../Trade/TradeSearch';</script>");
            }
            if (string.IsNullOrEmpty(FC["FinishTime"]))
            {
                return Content("<script>alert('结束时间未选！');document.location.href='../Trade/TradeSearch';</script>");
            }            
                int CurrPage;
                int.TryParse(Request.QueryString["page"], out CurrPage);
                CurrPage = CurrPage <= 0 ? 1 : CurrPage;
                int account = Convert.ToInt32(Session["Account"]);
                ViewBag.account = account;
            //分页显示所有的账户信息
                Page<TradeInfo> uList = ur.GetTradeLists(CurrPage, 10, account, Convert.ToDateTime(StartTime), Convert.ToDateTime(FinishTime));
                return View(uList);            

        }
        #endregion
        #region 管理员功能（冻结/解除冻结账户）
        /// <summary>
        /// 管理员功能（冻结/解除冻结账户）
        /// </summary>
        /// <returns></returns>
        /// 
        // [Login]
        public ActionResult AdminSet()
        {
            int CurrPage;
            int.TryParse(Request.QueryString["page"], out CurrPage);
            CurrPage = CurrPage <= 0 ? 1 : CurrPage;
            Page<UserInfo> uList = ur.AdminSet(CurrPage, 5);
            //ur.SumMoney();             
            return View(uList);
        }
        //[Login]
        [HttpPost]
        [MultiButton("action1")]//根据MultiButtonAttribute来区别那个提交按钮进行的操作
        public ActionResult AdminSet(FormCollection FC)
        {
            try
            {
                string Account = FC["account"];
                if (string.IsNullOrEmpty(FC["account"]))
                {
                    return Content("<script>alert('请输入账号！');document.location.href='../Trade/AdminSet';</script>");
                }
                UserInfo uInfo = ur.GetAll(Convert.ToInt32(Account));
                if (!Regex.IsMatch(Account, @"^\d{8}$"))
                {
                    ViewBag.error = "账户名必须为8位";
                    return View(uInfo);
                }
                int CurrPage;
                int.TryParse(Request.QueryString["page"], out CurrPage);
                CurrPage = CurrPage <= 0 ? 1 : CurrPage;
                Page<UserInfo> uList = ur.AdminSet(CurrPage, 5);
                if (Convert.ToInt32(Account) == uInfo.Account)
                {
                    int status = uInfo.Status;
                    if (status == 0 || status == 2)
                    {
                        ViewBag.error = "此账户已经被冻结或注销！";
                    }
                    else if (status == 1)
                    {

                        ur.UpdateStatus(Convert.ToInt32(Account));
                        return RedirectToAction("AdminSet", "Trade");
                        //return Content("冻结成功！");                   
                    }
                }
                else
                {
                    ViewBag.error = "没有该用户";
                }
                return View(uList);
                //return Content("冻结成功！");
            }
            catch (Exception)
            {

                return Content("<script>alert('请检查输入规范！');document.location.href='../Trade/AdminSet';</script>");
            }
           
        }
        // [Login]
        [HttpPost]
        [MultiButton("action2")]//根据MultiButtonAttribute来区别那个提交按钮进行的操作
        public ActionResult AdminSet(FormCollection FC, string Account)
        {
            try
            {
                int CurrPage;
                Account = FC["account"];
                if (string.IsNullOrEmpty(FC["account"]))
                {
                    return Content("<script>alert('请输入账号！');document.location.href='../Trade/AdminSet';</script>");
                }
                if (!Regex.IsMatch(FC["account"], @"^[0-9]*$"))
                {
                    return Content("<script>alert('只能输入数字！');document.location.href='../Trade/AdminSet';</script>");
                }
                UserInfo uInfo = ur.GetAll(Convert.ToInt32(Account));
                if (!Regex.IsMatch(FC["account"], @"^\d{8}$"))
                {
                    ViewBag.error = "账户名必须为8位";
                    return View(uInfo);
                }
                int.TryParse(Request.QueryString["page"], out CurrPage);
                CurrPage = CurrPage <= 0 ? 1 : CurrPage;
                Page<UserInfo> uList = ur.AdminSet(CurrPage, 5);


                if (Convert.ToInt32(Account) == uInfo.Account)
                {
                    int status = uInfo.Status;
                    if (status == 0)
                    {
                        ur.UpdateStatusfin(Convert.ToInt32(Account));
                        ViewBag.error = "解冻成功！！！";
                        return RedirectToAction("AdminSet", "Trade");
                    }
                    else if (status == 1)
                    {
                        ViewBag.error = "此账户为正常状态！";
                    }
                    else if (status == 2)
                    {
                        ViewBag.error = "此账户已经被注销！";
                    }
                }
                else
                {
                    ViewBag.error = "没有该用户";
                }
                return View(uList);
                //return Content("冻结成功！");
            }
            catch (Exception)
            {

                return Content("<script>alert('请检查输入规范！');document.location.href='../Trade/AdminSet';</script>");
            }
           
        }
        #endregion
    }
}