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
    public class RunningCoursesController : Controller
    {
        private TrainingCenterLibDbContext db = new TrainingCenterLibDbContext();

        // GET: RunningCourses
        public async Task<ActionResult> Index()
        {
            var runningCourses = db.RunningCourses.Include(r => r.Room).Include(r => r.WaitingList);
            return View(await runningCourses.ToListAsync());
        }

        // GET: RunningCourses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RunningCours runningCours = await db.RunningCourses.FindAsync(id);
            if (runningCours == null)
            {
                return HttpNotFound();
            }
            return View(runningCours);
        }

        // GET: RunningCourses/Create
        public ActionResult Create()
        {
            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "Name");
            ViewBag.WaitingListID = new SelectList(db.WaitingLists, "WaitingListID", "GroupName");
            return View();
        }

        // POST: RunningCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RunningCourseID,WaitingListID,StartAt,EndAt,RoomID,CreatedAt,UserID")] RunningCours runningCours)
        {
            if (ModelState.IsValid)
            {
                db.RunningCourses.Add(runningCours);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "Name", runningCours.RoomID);
            ViewBag.WaitingListID = new SelectList(db.WaitingLists, "WaitingListID", "GroupName", runningCours.WaitingListID);
            return View(runningCours);
        }

        // GET: RunningCourses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RunningCours runningCours = await db.RunningCourses.FindAsync(id);
            if (runningCours == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "Name", runningCours.RoomID);
            ViewBag.WaitingListID = new SelectList(db.WaitingLists, "WaitingListID", "GroupName", runningCours.WaitingListID);
            return View(runningCours);
        }

        // POST: RunningCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RunningCourseID,WaitingListID,StartAt,EndAt,RoomID,CreatedAt,UserID")] RunningCours runningCours)
        {
            if (ModelState.IsValid)
            {
                db.Entry(runningCours).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "Name", runningCours.RoomID);
            ViewBag.WaitingListID = new SelectList(db.WaitingLists, "WaitingListID", "GroupName", runningCours.WaitingListID);
            return View(runningCours);
        }

        // GET: RunningCourses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RunningCours runningCours = await db.RunningCourses.FindAsync(id);
            if (runningCours == null)
            {
                return HttpNotFound();
            }
            return View(runningCours);
        }

        // POST: RunningCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RunningCours runningCours = await db.RunningCourses.FindAsync(id);
            db.RunningCourses.Remove(runningCours);
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
