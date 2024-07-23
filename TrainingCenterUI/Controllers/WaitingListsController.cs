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
using TrainingCenterLib.Repository.Services;
using WebMatrix.WebData;

namespace TrainingCenterUI.Controllers
{
    public class WaitingListsController : Controller
    {
        private TrainingCenterLibDbContext db = new TrainingCenterLibDbContext();
        private readonly WaitingListService _waitingListService;
        private readonly int _UserId;

        // GET: WaitingLists

        public WaitingListsController() 
        {
            _UserId = WebSecurity.CurrentUserId;
            _waitingListService = new WaitingListService(_UserId);

        }
        public async Task<ActionResult> Index()
        {
            var waitingLists = db.WaitingLists.Include(w => w.AvailableCours).Include(w => w.Student);
            return View(await waitingLists.ToListAsync());
        }

        // GET: WaitingLists/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WaitingList waitingList = await db.WaitingLists.FindAsync(id);
            if (waitingList == null)
            {
                return HttpNotFound();
            }
            return View(waitingList);
        }

        // GET: WaitingLists/Create
        public ActionResult Create()
        {
            var courses = _waitingListService.GetCourseName();
            ViewBag.AvailableCourseID = new SelectList(courses, "AvailableCourseID", "CourseName");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName");
            WaitingList waitingList = new WaitingList();
            return View(waitingList);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "WaitingListID,StudentID,AvailableCourseID,GroupName,IsPaid,IsCash")] WaitingList waitingList)
        {
            var isAlreadyEnrolled = _waitingListService.IsAlreadyEnrolled(waitingList.StudentID, waitingList.AvailableCourseID);

            if (isAlreadyEnrolled)
            {
                ModelState.AddModelError("", "You cannot enroll twice in the same course.");
                TempData["ErrorMessage"] = "You cannot enroll twice in the same course.";
                
                return RedirectToAction("Create");
            }
            if (ModelState.IsValid)
            {
                await _waitingListService.CreateWaitingListAsync(waitingList);
                return RedirectToAction("Index");
            }

            ViewBag.AvailableCourseID = new SelectList(db.AvailableCourses, "AvailableCourseID", "AvailableCourseID", waitingList.AvailableCourseID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", waitingList.StudentID);
            return View(waitingList);
        }

        // GET: WaitingLists/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WaitingList waitingList = await db.WaitingLists.FindAsync(id);
            if (waitingList == null)
            {
                return HttpNotFound();
            }


            ViewBag.AvailableCourseID = new SelectList(db.AvailableCourses, "AvailableCourseID", "AvailableCourseID", waitingList.AvailableCourseID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", waitingList.StudentID);
            return View(waitingList);
        }

        // POST: WaitingLists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "WaitingListID,StudentID,AvailableCourseID,GroupName,IsPaid,IsCash")] WaitingList waitingList)
        {
            var isAlreadyEnrolled = _waitingListService.IsAlreadyEnrolled(waitingList.StudentID, waitingList.AvailableCourseID);

            if (isAlreadyEnrolled)
            {
                ModelState.AddModelError("", "You cannot enroll twice in the same course.");
                TempData["ErrorMessage"] = "You cannot enroll twice in the same course.";

                return RedirectToAction("Edit");
            }

            if (ModelState.IsValid)
            {
                await _waitingListService.UpdateWaitingListAsync(waitingList);
                return RedirectToAction("Index");
            }
            ViewBag.AvailableCourseID = new SelectList(db.AvailableCourses, "AvailableCourseID", "AvailableCourseID", waitingList.AvailableCourseID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", waitingList.StudentID);
            return View(waitingList);
        }

        // GET: WaitingLists/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WaitingList waitingList = await db.WaitingLists.FindAsync(id);
            if (waitingList == null)
            {
                return HttpNotFound();
            }
            return View(waitingList);
        }

        // POST: WaitingLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _waitingListService.DeleteWaitingListAsync(id);
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
