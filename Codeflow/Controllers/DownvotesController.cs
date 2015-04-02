using System;
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
    public class DownvotesController : Controller
    {
        private CodeFlowContext db = new CodeFlowContext();

        // GET: /Downvotes/
        public ActionResult Index()
        {
            return View(db.ADownvotes.ToList());
        }

        // GET: /Downvotes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Downvotes downvotes = db.ADownvotes.Find(id);
            if (downvotes == null)
            {
                return HttpNotFound();
            }
            return View(downvotes);
        }

        // GET: /Downvotes/Create
        public ActionResult Create(Guid? id)
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
            return Create(answer);
        }

        // POST: /Downvotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,AnswerId,AccountId")] Answer answer)
        {
            Downvotes userquestionvotes = new Downvotes(); ;
            if (ModelState.IsValid)
            {
                bool logged = (System.Web.HttpContext.Current.User != null) &&
                            System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
                if (logged)
                {
                    String OwnerName = System.Web.HttpContext.Current.User.Identity.Name;
                    Account owner = db.Accounts.FirstOrDefault(a => a.Email.Equals(OwnerName));
                    Upvotes upvotes =
                        db.AUpvotes.FirstOrDefault(
                            a => a.AnswerId.Equals(answer.Id) && a.AccountId.Equals(owner.ID));
                    Downvotes downvotes =
                        db.ADownvotes.FirstOrDefault(
                            a => a.AnswerId.Equals(answer.Id) && a.AccountId.Equals(owner.ID));

                    if (upvotes == null&& downvotes == null)
                    {
                        userquestionvotes.Id = Guid.NewGuid();
                        userquestionvotes.AnswerId = answer.Id;
                        userquestionvotes.AccountId = owner.ID;
                        db.ADownvotes.Add(userquestionvotes);
                        db.SaveChanges();
                        return RedirectToAction("Details", "Question", new { id = answer.QuestionID });
                    }
                    HttpCookie Voted = Request.Cookies["Voted"];
                    Voted = new HttpCookie("Voted");
                    Voted.Values.Add("Avoted", "Already voted on this Answer");
                    Voted.Expires = DateTime.Now.AddDays(1);
                    Response.AppendCookie(Voted);

                    ModelState.AddModelError("", "Already voted on this question");
                }
                ModelState.AddModelError("", "You need to log in to vote");
            }

            return RedirectToAction("Details", "Question", new { id = answer.QuestionID });
        }

        // GET: /Downvotes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Downvotes downvotes = db.ADownvotes.Find(id);
            if (downvotes == null)
            {
                return HttpNotFound();
            }
            return View(downvotes);
        }

        // POST: /Downvotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,AnswerId,AccountId")] Downvotes downvotes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(downvotes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(downvotes);
        }

        // GET: /Downvotes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Downvotes downvotes = db.ADownvotes.Find(id);
            if (downvotes == null)
            {
                return HttpNotFound();
            }
            return View(downvotes);
        }

        // POST: /Downvotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Downvotes downvotes = db.ADownvotes.Find(id);
            db.ADownvotes.Remove(downvotes);
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
