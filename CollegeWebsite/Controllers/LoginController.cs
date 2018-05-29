using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CollegeWebsite.Models;

namespace CollegeWebsite.Controllers
{
    public class LoginController : Controller
    {
        private AdvWebDevProjectEntities db = new AdvWebDevProjectEntities();

        // GET: Login
        public ActionResult Index()
        {
            if(Session["StudentName"] == null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home");
            }
            
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            System.Data.Entity.Core.Objects.ObjectParameter StudentId = new System.Data.Entity.Core.Objects.ObjectParameter("studentID", typeof(int));
            //int StudentId = -1;
            string studentLogin = form["usr"];
            string studentPassword = form["pwd"];
            db.pSelLoginIdByLoginAndPassword(form["usr"], form["pwd"], StudentId);
            Student s = db.Students.Find(StudentId);
            
            if (s.StudentPassword == form["pwd"])
            {
                Session["StudentName"] = s.StudentName;
                Session["StudentLogin"] = s.StudentLogin;
                Session["StudentEmail"] = s.StudentEmail;
                Session["StudentId"] = s.StudentId;

                return Redirect("/Home");
            }
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection form)
        {
            DateTime userDate = DateTime.Parse(form["userDate"]);

            int execute = db.pInsLoginRequest(form["userName"], form["userEmail"], form["userLogin"], form["userActivate"], form["userReason"], userDate);
            return Redirect("/Home");
        }

        // GET: Login/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,StudentName,StudentEmail,StudentLogin,StudentPassword")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,StudentName,StudentEmail,StudentLogin,StudentPassword")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
