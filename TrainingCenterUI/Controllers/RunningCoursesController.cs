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
using TrainingCenterLib.Repository.Interfaces;
using WebMatrix.WebData;
using TrainingCenterLib.Repository.Services;

namespace TrainingCenterUI.Controllers
{
    public class RunningCoursesController : Controller
    {
        private TrainingCenterLibDbContext db = new TrainingCenterLibDbContext();
        private readonly int _UserId;
        private readonly IRunningCourseService _runningCourseService;

        public RunningCoursesController() 
        {
            _UserId = WebSecurity.CurrentUserId;
            _runningCourseService = new RunningCourseService(_UserId);
        }


        // GET: RunningCourses
        public async Task<ActionResult> Index()
        {
            //var runningCourses = db.RunningCourses.Include(r => r.Room).Include(r => r.WaitingList);
            return View(await _runningCourseService.GetAllRunningCoursesAsync());
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
            RunningCours runningCourse = new RunningCours();
            return View(runningCourse);
        }

        // POST: RunningCourses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RunningCourseID,WaitingListID,StartAt,EndAt,RoomID,CreatedAt,UserID")] RunningCours runningCourse)
        {
            if (ModelState.IsValid)
            {
                //db.RunningCourses.Add(runningCourse);
                //await db.SaveChangesAsync();
                await _runningCourseService.CreateRunningCourseAsync(runningCourse);
                return RedirectToAction("Index");
            }

            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "Name", runningCourse.RoomID);
            ViewBag.WaitingListID = new SelectList(db.WaitingLists, "WaitingListID", "GroupName", runningCourse.WaitingListID);
            return View(runningCourse);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RunningCourseID,WaitingListID,StartAt,EndAt,RoomID,CreatedAt,UserID")] RunningCours runningCours)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(runningCours).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                await _runningCourseService.UpdateRunningCourseAsync(runningCours);
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
            //RunningCours runningCours = await db.RunningCourses.FindAsync(id);
            //db.RunningCourses.Remove(runningCours);
            //await db.SaveChangesAsync();
            await _runningCourseService.DeleteRunningCourseAsync(id);
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
