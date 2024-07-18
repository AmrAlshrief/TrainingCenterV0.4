using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainingCenterLib.Entities;

namespace TrainingCenterUI.Controllers
{
    public class AvailableCoursesController : Controller
    {
        private TrainingCenterLibDbContext db = new TrainingCenterLibDbContext();

        // GET: AvailableCourses
        public async Task<ActionResult> Index()
        {
            var availableCourses = db.AvailableCourses.Include(a => a.Cours).Include(a => a.InstructorAvailability);
            return View(await availableCourses.ToListAsync());
        }

        // GET: AvailableCourses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailableCours availableCours = await db.AvailableCourses.FindAsync(id);
            if (availableCours == null)
            {
                return HttpNotFound();
            }
            return View(availableCours);
        }

        // GET: AvailableCourses/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.InstructorAvailabilityID = new SelectList(db.InstructorAvailabilities, "InstructorAvailabilityID", "InstructorAvailabilityID");
            return View();
        }

        // POST: AvailableCourses/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AvailableCourseID,CourseID,InstructorAvailabilityID,StartAt")] AvailableCours availableCours)
        {
            if (ModelState.IsValid)
            {
                db.AvailableCourses.Add(availableCours);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", availableCours.CourseID);
            ViewBag.InstructorAvailabilityID = new SelectList(db.InstructorAvailabilities, "InstructorAvailabilityID", "InstructorAvailabilityID", availableCours.InstructorAvailabilityID);
            return View(availableCours);
        }

        // GET: AvailableCourses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailableCours availableCours = await db.AvailableCourses.FindAsync(id);
            if (availableCours == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", availableCours.CourseID);
            ViewBag.InstructorAvailabilityID = new SelectList(db.InstructorAvailabilities, "InstructorAvailabilityID", "InstructorAvailabilityID", availableCours.InstructorAvailabilityID);
            return View(availableCours);
        }

        // POST: AvailableCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AvailableCourseID,CourseID,InstructorAvailabilityID,StartAt")] AvailableCours availableCours)
        {
            if (ModelState.IsValid)
            {
                db.Entry(availableCours).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", availableCours.CourseID);
            ViewBag.InstructorAvailabilityID = new SelectList(db.InstructorAvailabilities, "InstructorAvailabilityID", "InstructorAvailabilityID", availableCours.InstructorAvailabilityID);
            return View(availableCours);
        }

        // GET: AvailableCourses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailableCours availableCours = await db.AvailableCourses.FindAsync(id);
            if (availableCours == null)
            {
                return HttpNotFound();
            }
            return View(availableCours);
        }

        // POST: AvailableCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AvailableCours availableCours = await db.AvailableCourses.FindAsync(id);
            db.AvailableCourses.Remove(availableCours);
            await db.SaveChangesAsync();
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
