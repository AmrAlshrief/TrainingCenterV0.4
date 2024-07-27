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
            if (Session["login"] != null)
            {

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            //var waitingLists = db.WaitingLists.Include(w => w.AvailableCours).Include(w => w.Student);
            //var waitingLists = db.WaitingLists.Include(w => w.AvailableCours).Include(w => w.Student).ToList();
            var waitingLists = db.WaitingLists.Include(w => w.AvailableCours).Include(w => w.Student).Include(w => w.ActiveCourseByGroup).ToList();
            var groupCounts = waitingLists
                .GroupBy(w => w.AvailableCours.AvailableCourseID)
                .Select(g => new
                {
                    AvailableCourseID = g.Key,
                    Count = g.Count()
                })
                .ToDictionary(g => g.AvailableCourseID, g => g.Count);

            ViewBag.GroupCounts = groupCounts;

            return View(waitingLists);
        }

        // GET: WaitingLists/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (Session["login"] != null)
            {

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (Session["login"] != null)
            {

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            var courses = _waitingListService.GetCourseName();
            ViewBag.AvailableCourseID = new SelectList(courses, "AvailableCourseID", "CourseName");
            ViewBag.StudentID = new SelectList(db.Students.Where(i => !i.IsDeleted), "StudentID", "FirstName");
            WaitingList waitingList = new WaitingList();
            return View(waitingList);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "WaitingListID,StudentID,AvailableCourseID,GroupName,IsPaid,IsCash")] WaitingList waitingList)
        {
            if (Session["login"] != null)
            {

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (Session["login"] != null)
            {

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (Session["login"] != null)
            {

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (Session["login"] != null)
            {

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (Session["login"] != null)
            {

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            await _waitingListService.DeleteWaitingListAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        //public JsonResult GetAvailableCourse(int waitingListID)
        //{
        //    // Assuming you have a DbContext instance called _context
        //    var availableCourse = db.AvailableCourses
        //        .Include(ac => ac.InstructorAvailability) // Assuming there's a navigation property
        //        .FirstOrDefault(ac => ac.WaitingListID == waitingListID);

        //    if (availableCourse == null)
        //    {
        //        return Json(new { success = false, message = "No available course found." });
        //    }

        //    // Extract necessary information
        //    var availableCourseID = availableCourse.AvailableCourseID;
        //    var isGroupDay = availableCourse.InstructorAvailability.IsGroupDay; // Adjust based on your model
        //    var timeSlot = availableCourse.InstructorAvailability.TimeSlot; // Adjust based on your model

        //    // Prepare the data to return
        //    var data = new
        //    {
        //        AvailableCourseID = availableCourseID,
        //        IsGroupDay = isGroupDay,
        //        TimeSlot = timeSlot
        //    };

        //    return Json(new { success = true, data = data });
        //}

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
