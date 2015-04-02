using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Codeflow.Models;
using Codeflow.Dal;

namespace Codeflow.Controllers
{
    public class AnswerController : Controller
    {
        private CodeFlowContext db = new CodeFlowContext();

        // GET: /Answer/
        public ActionResult Index()
        {
            return View(db.Answers.ToList());
        }

        // GET: /Answer/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // GET: /Answer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Answer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Answerstring")] Guid id, Answer answer)
        {
            if (ModelState.IsValid)
            {
                answer.Id = Guid.NewGuid();
                answer.QuestionID = id;
                answer.OwnerName = System.Web.HttpContext.Current.User.Identity.Name;
                //if (User.Identity.IsAuthenticated)
                //{
                    //answer.AccountID = Answer answer = db.Answers.Find(id);
                    Account owner = db.Accounts.FirstOrDefault(a => a.Email.Equals(answer.OwnerName)); 
                    answer.AccountID = owner.ID;
                    db.Answers.Add(answer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                //}
            }

            return View(answer);
        }

        // GET: /Answer/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: /Answer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Answerstring,Votes,OwnerName,OwnerID")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(answer);
        }

        // GET: /Answer/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: /Answer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Answer answer = db.Answers.Find(id);
            db.Answers.Remove(answer);
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
    }
}
