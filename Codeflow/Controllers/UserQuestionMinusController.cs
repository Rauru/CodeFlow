﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Codeflow.Models;
using Codeflow.Dal;

namespace Codeflow.Controllers
{
    public class UserQuestionMinusController : Controller
    {
        private CodeFlowContext db = new CodeFlowContext();

        // GET: /UserQuestionMinus/
        public ActionResult Index()
        {
            return View(db.Qvotesminus.ToList());
        }

        // GET: /UserQuestionMinus/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserQuestionMinus userquestionminus = db.Qvotesminus.Find(id);
            if (userquestionminus == null)
            {
                return HttpNotFound();
            }
            return View(userquestionminus);
        }

        // GET: /UserQuestionMinus/Create
        public ActionResult Create(Guid? id)
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
            return Create(question);
        }

        // POST: /UserQuestionMinus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="QvoteId,QuestionId,AccountId")] Question question)
        {
            UserQuestionMinus userquestionvotes = new UserQuestionMinus(); ;
            if (ModelState.IsValid)
            {
                bool logged = (System.Web.HttpContext.Current.User != null) &&
                            System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
                if (logged)
                {
                    String OwnerName = System.Web.HttpContext.Current.User.Identity.Name;
                    Account owner = db.Accounts.FirstOrDefault(a => a.Email.Equals(OwnerName));
                    UserQuestionVotes upvotes =
                        db.Qvotes.FirstOrDefault(
                            a => a.QuestionId.Equals(question.QuestionID) && a.AccountId.Equals(owner.ID));
                    UserQuestionMinus minusvotes =
                        db.Qvotesminus.FirstOrDefault(
                            a => a.QuestionId.Equals(question.QuestionID) && a.AccountId.Equals(owner.ID));
                    if (upvotes == null && minusvotes == null)
                    {
                        userquestionvotes.QvoteId = Guid.NewGuid();
                        userquestionvotes.QuestionId = question.QuestionID;
                        userquestionvotes.AccountId = owner.ID;
                        db.Qvotesminus.Add(userquestionvotes);
                        db.SaveChanges();
                        return RedirectToAction("Details", "Question", new { id = question.QuestionID });
                    }
                    HttpCookie Voted = Request.Cookies["Voted"];
                    Voted = new HttpCookie("Voted");
                    Voted.Values.Add("Avoted", "Already voted on this question");
                    Voted.Expires = DateTime.Now.AddDays(1);
                    Response.AppendCookie(Voted);
                    ModelState.AddModelError("", "Already voted on this question");
                }
                ModelState.AddModelError("", "You need to log in to vote");
            }

            return RedirectToAction("Details", "Question", new { id = question.QuestionID });
        }

        // GET: /UserQuestionMinus/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserQuestionMinus userquestionminus = db.Qvotesminus.Find(id);
            if (userquestionminus == null)
            {
                return HttpNotFound();
            }
            return View(userquestionminus);
        }

        // POST: /UserQuestionMinus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="QvoteId,QuestionId,AccountId")] UserQuestionMinus userquestionminus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userquestionminus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userquestionminus);
        }

        // GET: /UserQuestionMinus/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserQuestionMinus userquestionminus = db.Qvotesminus.Find(id);
            if (userquestionminus == null)
            {
                return HttpNotFound();
            }
            return View(userquestionminus);
        }

        // POST: /UserQuestionMinus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UserQuestionMinus userquestionminus = db.Qvotesminus.Find(id);
            db.Qvotesminus.Remove(userquestionminus);
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
