﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Thalili.Models;

namespace Thalili.Controllers
{
    public class RegisterController : Controller
    {

        thaliliEntities1 Context = new thaliliEntities1();
        
        public ActionResult Index()
        {
            if (Session["userName"] != null)
            {
                return RedirectToAction("Index", "Logout");
            }
                return View();
        }
        [HttpPost]
        public ActionResult RegisterConfirm(user newuser)
        {

            var cheak = Context.users.Where(d => d.email == newuser.email).FirstOrDefault();
            if (cheak != null)
            {
                ViewData["ERoor"] = "البريد الالكترونى مستخدم";
                ViewData["Name"] = newuser.name;
                ViewData["Email"] = newuser.email;
                ViewData["Password"] = newuser.pass;
                return View("Index");
            }
            user user1 = new user();
            user1.email = newuser.email;
            user1.name = newuser.name;
            user1.activation_code = Guid.NewGuid().ToString();
            user1.pass = Crypto.Hash(newuser.pass);
            Context.users.Add(user1);
            Context.SaveChanges();
            Session["UserID"] = user1.user_id;
            Session["userName"] = user1.name;
            SendVerificationLinkEmail(user1.email, user1.activation_code);
            return View();
        }
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            //int x =(int) Session["UserID"];

            var v = Context.users.Where(a => a.activation_code==id).FirstOrDefault();
            if(v == null)
            {
                return RedirectToAction("Index", "Login");
            }
               Session["UserID"] = v.user_id;
               Session["userName"] = v.name;
                v.Email_confirmed = true;
                Context.SaveChanges();
            return View();
        }
        [NonAction]
        public void SendVerificationLinkEmail(string email, string Activation_code)
        {
            var verifyUrl = "/Register/VerifyAccount/" + Activation_code;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("tthalyly@gmail.com", "Thalili");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "mama2468"; // Replace with actual password
            string subject = "Your account is successfully created!";

            string body = "<br/><br/>We are excited to tell you that your Thalili account is" +
                " successfully created. Please click on the below link to verify your account" +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }
}