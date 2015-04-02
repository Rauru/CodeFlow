using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Codeflow.Dal;
using Codeflow.Models;
using RestSharp;
using System.Security.Policy;
using BotDetect.Web.UI.Mvc;
using HttpCookie = System.Web.HttpCookie;

namespace Codeflow.Controllers
{
    public class AccountLogInController : Controller
    {
        private CodeFlowContext db = new CodeFlowContext();
        // GET: /AccountLogin/
        public ActionResult LogIn()
        {

            //FormsAuthentication.SetAuthCookie(Model.Email, false);
            //return RedirectToAction("Details", "Account", new { id = item.ID });



            if (TempData["EdittoLogin"] != null)
            {
                
                @ViewBag.Message = TempData["EdittoLogin"].ToString();
                // here pass the Flag to partial view with value = true
                return View();
            }
            return View();
        }

        public ActionResult LogInWithoucaptcha()
        {

            //FormsAuthentication.SetAuthCookie(Model.Email, false);
            //return RedirectToAction("Details", "Account", new { id = item.ID });



            if (TempData["EdittoLogin"] != null)
            {

                @ViewBag.Message = TempData["EdittoLogin"].ToString();
                // here pass the Flag to partial view with value = true
                return View();
            }
            return View();
        }

        [HttpPost]
        public ActionResult LogInWithoucaptcha(AccountLogInModel account)
        {
            if (ModelState.IsValid)
            {
                if (IsValidEmail(account.Email))
                {
                    
                    using (CodeFlowContext db = new CodeFlowContext())
                    {
                        //var v = db.Accounts.FirstOrDefault(model => model.Email.Equals(account.Email) && model.Password.Equals(account.Password));
                        var v = db.Accounts.FirstOrDefault(model => model.Email.Equals(account.Email));
                        if (v != null)
                        {
                            if (v.Verifiedmail)
                            {
                                if (v.Password == account.Password)
                                {
                                    FormsAuthentication.SetAuthCookie(account.Email, false);
                                    return RedirectToAction("Details", "Account", new { id = v.ID });
                                    
                                    //TempData["EdittoLogin"] = "Your password has been updated";
                                }
                                else
                                {
                                    HttpCookie loginInfo = Request.Cookies["Login"];
                                    if (loginInfo != null)
                                    {
                                        //Response.Cookies["Login"].Expires = DateTime.Now.AddDays(-1);
                                        int times =  int.Parse(Request.Cookies["Login"].Values["LoginCount"]);
                                        if (times == 3)
                                        {
                                            Response.Write("Login fail!");
                                            Response.Cookies["Login"].Expires = DateTime.Now.AddDays(-1);
                                            return RedirectToAction("LogIn", "AccountLogIn");
                                        }
                                        else if (times == 2)
                                        {
                                            Response.Cookies["Login"].Expires = DateTime.Now.AddDays(-1);
                                            loginInfo = new HttpCookie("Login");
                                            loginInfo.Values.Add("LoginCount", 2.ToString());
                                            loginInfo.Expires = DateTime.Now.AddDays(1);
                                            Response.AppendCookie(loginInfo);
                                        }
                                        else
                                        {
                                            Response.Cookies["Login"].Expires = DateTime.Now.AddDays(-1);
                                            loginInfo = new HttpCookie("Login");
                                            loginInfo.Values.Add("LoginCount", 3.ToString());
                                            loginInfo.Expires = DateTime.Now.AddDays(1);
                                            Response.AppendCookie(loginInfo);
                                        }
                                    }
                                    else
                                    {
                                        loginInfo = new HttpCookie("Login");
                                        loginInfo.Values.Add("LoginCount", 1.ToString());
                                        loginInfo.Expires = DateTime.Now.AddDays(1);
                                        Response.AppendCookie(loginInfo);
                                        FailedLogin(v.Email);
                                    }

                                    ModelState.AddModelError("", Request.Cookies["Login"].Values["LoginCount"]);
                                }


                            }
                            else
                            {
                                ModelState.AddModelError("", "Check your email to verify your account");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Email or Password");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email must be in valid format");
                }
            }
            else
            {
                ModelState.AddModelError("", "Login data is incorrect!");
            }
            return View(account);
        }


        /*public ActionResult LogIn(AccountLogInModel Model)
        {
            FormsAuthentication.SetAuthCookie(Model.Email, false);
            return RedirectToAction("Index", "Account");
        }*/

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            //return RedirectToAction("Index", "Question");
            return RedirectToAction("Index", "Account");
        }

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(AccountLogInModel account)
        {
            // this action is for handle post (login)
            if (ModelState.IsValid) // this is check validity
            {
                using (CodeFlowContext db = new CodeFlowContext())
                {
                    
                    //var v = dc.Users.Where(a => a.Username.Equals(u.Username) && a.Password.Equals(u.Password)).FirstOrDefault();
                    var v =
                        db.Accounts.FirstOrDefault(model => model.Email.Equals(account.Email) && model.Password.Equals(account.Password));
                    if (v != null)
                    {
                        //Session["LogedUserID"] = v.UserID.ToString();
                        Session["LogedUserID"] = v.ID.ToString();
                        //Session["LogedUserFullname"] = v.FullName.ToString();
                        Session["LogedUserFullname"] = v.FirstName;
                        return RedirectToAction("Details", "Account", new { id = v.ID});
                    }
                }
            }
            return View(account);
        }*/

        [HttpPost]
        [CaptchaValidation("CaptchaCode", "RegistrationCaptcha",
          "Your input doesn't match displayed characters")]
        public ActionResult LogIn(AccountLogInModel account)
        {
            if (ModelState.IsValid)
            {
                if (IsValidEmail(account.Email))
                {
                    HttpCookie loginInfo = Request.Cookies["LoginAttemps"];
                    using (CodeFlowContext db = new CodeFlowContext())
                    {
                        //var v = db.Accounts.FirstOrDefault(model => model.Email.Equals(account.Email) && model.Password.Equals(account.Password));
                        var v = db.Accounts.FirstOrDefault(model => model.Email.Equals(account.Email));
                        if (v != null)
                        {
                            if (v.Verifiedmail)
                            {
                                if (v.Password == account.Password)
                                {
                                    FormsAuthentication.SetAuthCookie(account.Email, false);
                                    MvcCaptcha.ResetCaptcha("RegistrationCaptcha");
                                    return RedirectToAction("Details", "Account", new {id = v.ID});
                                    
                                    //TempData["EdittoLogin"] = "Your password has been updated";
                                }
                                ModelState.AddModelError("", Request.Cookies["LoginAttemps"].Values["LoginCount"]);
                            }
                            else
                            {
                                ModelState.AddModelError("", "Check your email to verify your account");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Email or Password");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email must be in valid format");
                }
            }
            else
            {
                ModelState.AddModelError("", "Login data is incorrect!");
            }
            return View(account);
        }

        public ActionResult PasswordRecovery()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PasswordRecovery(PasswordRecoveryModel model)
        {
            if (ModelState.IsValid)
            {
                if (IsValidEmail(model.Email)) {
                    using (CodeFlowContext db = new CodeFlowContext())
                    {
                    
                        var v =
                            db.Accounts.FirstOrDefault(a => a.Email.Equals(model.Email));   

                        if (v != null)
                        {
                            if (IsValidEmail(v.Email))
                            {
                                SendSimpleMessage(v.Email);
                                
                                ModelState.AddModelError("", "We have sent an email with further instructions to reset your password");
                                //return RedirectToAction("Edit", "Account", new { id = v.ID });
                            }
                        
                        }
                        else
                        {
                            ModelState.AddModelError("", "No user found with that email");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid mail format");
                }
            }
            else
            {
                ModelState.AddModelError("", "Login data is incorrect!");
            }
            return View(model);
        }

        public static IRestResponse GetValidate(string model)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v2");
            client.Authenticator = new HttpBasicAuthenticator("api",
                                               "key-d8f99fa14ffc7dacdbcac8020db3813d");
            RestRequest request = new RestRequest();
            //request.Resource = "/address/validate";
            //request.AddParameter("address", "foo@mailgun.net");
            request.AddParameter("adress", model);
            return client.Execute(request);
        }

        public  IRestResponse SendSimpleMessage(String Email)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v2");
            client.Authenticator =
                    new HttpBasicAuthenticator("api",
                                               "key-bec6333828729a03e7ee976236a4fe78");
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                "sandbox1eb84ea92ac54c1484333a90d9efc52f.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Mailgun Sandbox <postmaster@sandbox1eb84ea92ac54c1484333a90d9efc52f.mailgun.org>");
            
            request.AddParameter("to", Email);
            request.AddParameter("subject", "Password Change");
            request.AddParameter("text", " : Password Change");
            Account owner = db.Accounts.FirstOrDefault(a => a.Email.Equals(Email));
            
            request.AddParameter("text",  Url.Action("Edit", "Account", new {owner.ID}, Request.Url.Scheme));
                //"@Html.ActionLink("Edit", "Edit", new { id=item.ID })");
            request.Method = Method.POST;
            return client.Execute(request);
        }
        public IRestResponse FailedLogin(String Email)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v2");
            client.Authenticator =
                    new HttpBasicAuthenticator("api",
                                               "key-bec6333828729a03e7ee976236a4fe78");
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                "sandbox1eb84ea92ac54c1484333a90d9efc52f.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Mailgun Sandbox <postmaster@sandbox1eb84ea92ac54c1484333a90d9efc52f.mailgun.org>");

            request.AddParameter("to", Email);
            request.AddParameter("subject", "Login Attemp");
            request.AddParameter("text", " : There was a failed Login Attemp on your account");
            //"@Html.ActionLink("Edit", "Edit", new { id=item.ID })");
            request.Method = Method.POST;
            return client.Execute(request);
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static MvcCaptcha GetRegistrationCaptcha()
        {
            // create the control instance
            MvcCaptcha registrationCaptcha = new MvcCaptcha(
              "RegistrationCaptcha");

            // set up client-side processing of the Captcha code input textbox
            registrationCaptcha.UserInputClientID = "CaptchaCode";

            // Captcha settings
            registrationCaptcha.ImageSize = new System.Drawing.Size(200, 50);
            registrationCaptcha.CodeLength = 4;

            return registrationCaptcha;
        }

        

        
	}
}