using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Codeflow.Models;
using Codeflow.Dal;

namespace Codeflow.Controllers
{
    public class QuestionController : Controller
    {
        private CodeFlowContext db = new CodeFlowContext();

        // GET: /Question/
        public ActionResult Index()
        {
            return View(db.Questions.ToList());
        }

        // GET: /Question/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            if (Request.Cookies["Voted"] != null)
            {
                ModelState.AddModelError("CustomE", Request.Cookies["Voted"].Values["Avoted"]);
                Response.Cookies["Voted"].Expires = DateTime.Now.AddDays(-1);
            }
            return View(question);
        }

        // GET: /Question/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Question/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuestionTittle,QuestionString,AccountID")] Guid id, Question question)
        {
            //Question question = new Question();
            if (ModelState.IsValid)
            {
                bool logged = (System.Web.HttpContext.Current.User != null) &&
                            System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
                if (logged)
                {
                    String OwnerName = System.Web.HttpContext.Current.User.Identity.Name;
                    Account owner = db.Accounts.FirstOrDefault(a => a.Email.Equals(OwnerName));
                    MatchCollection collection = Regex.Matches(question.QuestionTittle, @"[\S]+");
                    if (collection.Count >= 3)
                    {
                        MatchCollection collectiontitle = Regex.Matches(question.QuestionString, @"[\S]+");
                        if (collectiontitle.Count >= 5)
                        {
                            question.QuestionID = Guid.NewGuid();
                            question.Votes = 0;
                            question.AccountID = owner.ID;
                            db.Questions.Add(question);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        ModelState.AddModelError("", "A minium of 5 words required for the Description");
                    }
                    else
                    {
                        ModelState.AddModelError("", "A minium of 3 words required for the title");
                    }
                }
            }

            return View(question);
        }

        // GET: /Question/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: /Question/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details sDefault1ee http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Votes")] Question question)
        {
            if (ModelState.IsValid)
            {
                question.Votes = question.Votes + 1;
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(question);
        }

        // GET: /Question/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: /Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Upvote(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            //question.Votes = question.Votes + 1;
            return View(question);
        }


        
        [HttpPost]
        public ActionResult Upvote(Question question)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(question); 
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
