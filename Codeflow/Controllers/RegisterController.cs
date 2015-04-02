using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Codeflow.Models;
using Codeflow.Dal;
using RestSharp;

namespace Codeflow.Controllers
{
    public class RegisterController : Controller
    {
        private CodeFlowContext db = new CodeFlowContext();

        // GET: /Register/
        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
            //return RedirectToAction("Details", "Account");
            // RedirectToAction("action name", "controller name"); 
            // meogheController => "meoghe" is controller name
        }

        // GET: /Register/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: /Register/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Register/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,CreationdDateTime,Email,Verifiedmail,Password,ConfirmPassword")] Account account)
        {
            if (ModelState.IsValid)
            {
                account.ID = Guid.NewGuid();
                account.Verifiedmail = false;
                account.CreationdDateTime = DateTime.Now;
                
                if (account.Password.Any(char.IsUpper))
                {
                    if (account.Password.Any(char.IsLower))
                    {
                        if (account.Password.Any(char.IsNumber))
                        {
                            if (account.Password.Any(char.IsLetterOrDigit))
                            {
                                
                                bool hasRepeatingCharsInARow = false;
                                for (int index = 2; index < account.Password.Length; index++)
                                {
                                    if (account.Password[index] == account.Password[index - 1])
                                    {
                                        hasRepeatingCharsInARow = true;
                                    }
                                }
                                if (!hasRepeatingCharsInARow)
                                {
                                    if (System.Text.RegularExpressions.Regex.IsMatch(account.Password,
                                        "^[a-zA-Z0-9\x20]+$"))
                                    {
                                        var v = db.Accounts.FirstOrDefault(a => a.Email.Equals(account.Email));
                                        if (v==null)
                                        {
                                            db.Accounts.Add(account);
                                            db.SaveChanges();
                                            //return RedirectToAction("Index");
                                            SendSimpleMessage(account.Email);
                                            return RedirectToAction("LogInWithoucaptcha", "AccountLogIn");
                                        }
                                        else
                                        {
                                            ModelState.AddModelError("", "User already registered with that email");
                                        }
                                    }
                                    else
                                    {
                                    ModelState.AddModelError("", "Only letters and numbers admited in the password field");
                                        
                                    }


                                }
                                else
                                {
                                    ModelState.AddModelError("", "Repeated characters arent accepted in the password field");
                                }
                                
                            }
                            else
                            {
                                ModelState.AddModelError("", "Only letters and numbers admited in the password field");
                            }
                            
                        }
                        else
                        {
                             ModelState.AddModelError("", "Password must contain Numbers");
                        }
                    }else
                    {
                        ModelState.AddModelError("", "Password must contain lower case letters");
                    }

                }else
                {
                    ModelState.AddModelError("", "Password must contain uper case letters");
                }
                
                

            }

            return View(account);
        }

        // GET: /Register/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: /Register/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,FirstName,LastName,CreationdDateTime,Email,Password")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: /Register/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: /Register/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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

        public IRestResponse SendSimpleMessage(String Email)
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

            request.AddParameter("text", Url.Action("ValidateAccount", "Account", new { owner.ID }, Request.Url.Scheme));
            //"@Html.ActionLink("Edit", "Edit", new { id=item.ID })");
            request.Method = Method.POST;
            return client.Execute(request);
        }
    }
}
