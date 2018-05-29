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
    public class ClassesController : Controller
    {
        private AdvWebDevProjectEntities db = new AdvWebDevProjectEntities();

        // GET: Classes
        public ActionResult Index()
        {
            
            List<Class> filteredClasses = db.Classes.ToList();
            return View(db.Classes.ToList());

        }

        public ActionResult MyClasses()
        {
            if(Session["StudentID"] != null)
            {
                int sID = Convert.ToInt32(Session["StudentID"]);
                List<pSelClassesByStudentID_Result> assignedClasses = db.pSelClassesByStudentID(sID).ToList<pSelClassesByStudentID_Result>();

                return View(assignedClasses);
            }
            return Redirect("/Login/Index");
        }

        [HttpPost]
        public ActionResult Drop(FormCollection form)
        {
            int sID = Convert.ToInt32(Session["StudentID"]);
            int cID = Convert.ToInt32(form["ClassID"]);
            db.pDelClassStudents(cID, sID);
            return Redirect("/MyClasses");
        }


        [HttpPost]
        public ActionResult Register(FormCollection form)
        {
            if(Session["StudentID"] != null)
            {
                int studentID = Convert.ToInt32(Session["StudentID"]);
                List<CollegeWebsite.Models.pSelClassesByStudentID_Result> assignedClasses = db.pSelClassesByStudentID(studentID).ToList<pSelClassesByStudentID_Result>();
                int classId = Convert.ToInt32(form["ClassId"]);

                for(int i = 0; i < assignedClasses.Count; i++)
                {
                    if (classId == assignedClasses[0].ClassId)
                    {
                        return Redirect("Index");
                    }
                }

                db.pInsClassStudents(classId, studentID);
                return Redirect("Index");
            }
            else
            {
                return Redirect("Index");
            }
            

            
        }

        // GET: Classes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // GET: Classes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClassId,ClassName,ClassDate,ClassDescription")] Class @class)
        {
            if (ModelState.IsValid)
            {
                db.Classes.Add(@class);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@class);
        }

        // GET: Classes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClassId,ClassName,ClassDate,ClassDescription")] Class @class)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@class).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@class);
        }

        // GET: Classes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Class @class = db.Classes.Find(id);
            db.Classes.Remove(@class);
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
