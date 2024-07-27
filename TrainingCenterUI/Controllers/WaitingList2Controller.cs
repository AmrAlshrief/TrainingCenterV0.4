using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrainingCenterLib.Entities;
using TrainingCenterLib.Repository.Interfaces;
using TrainingCenterLib.Repository.Services;
using TrainingCenterUI.Models;
using WebMatrix.WebData;
using System.Net;

namespace TrainingCenterUI.Controllers
{
    public class WaitingList2Controller : Controller
    {
        private readonly NewWaitingListService _waitingListcourseService;
        private readonly TrainingCenterLibDbContext _Db;
        private readonly WaitingListService _WaitingListService;
        private readonly int _UserID;
        public WaitingList2Controller()
        {
            _Db = new TrainingCenterLibDbContext();
            _UserID = WebSecurity.CurrentUserId;
            _waitingListcourseService = new NewWaitingListService();
            _WaitingListService = new WaitingListService(_UserID);

        }
        // GET: WaitingList2
        public ActionResult Index()
        {
            if (Session["login"] != null)
            {
                var waitingLists = _Db.WaitingLists.Include(w => w.AvailableCours).Include(w => w.Student).Include(w => w.ActiveCourseByGroup).ToList();


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
            else
            {
                return RedirectToAction("Login", "Account");
            }
           
        }

        


        [HttpGet]
        public ActionResult Create()
        {
            if (Session["login"] != null)
            {
                var model = new WaitingListViewModel
                {
                    Students = new SelectList(_waitingListcourseService.GetStudents().Where(i => !i.IsDeleted), "StudentID", "FirstName"),
                    Courses = new SelectList(_WaitingListService.GetCourseName(), "AvailableCourseID", "CourseName")
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WaitingListViewModel model)
        {
            if (Session["login"] != null)
            {
                var isAlreadyEnrolled = _WaitingListService.IsAlreadyEnrolled(model.StudentID, model.AvailableCourseID);

                if (isAlreadyEnrolled)
                {
                    ModelState.AddModelError("", "You cannot enroll twice in the same course.");
                    TempData["ErrorMessage"] = "You cannot enroll twice in the same course.";

                    return RedirectToAction("Create");
                }
                if (ModelState.IsValid)
                {
                    string resultMessage = _waitingListcourseService.AddStudentToWaitingList(
                        model.StudentID,
                        model.AvailableCourseID,
                        model.IsPaid,
                        model.GroupName,
                        model.IsCash);

                    TempData["Message"] = resultMessage;
                    return RedirectToAction("Index");
                }

                model.Students = new SelectList(_waitingListcourseService.GetStudents().Where(i => !i.IsDeleted), "StudentID", "FirstName", model.StudentID);
                model.Courses = new SelectList(_waitingListcourseService.GetAvailableCourses(), "AvailableCourseID", "CourseID", model.AvailableCourseID);
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

           
        }

        // GET: WaitingLists/Delete/5
        public ActionResult Delete(int? id)
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
            WaitingList waitingList =  _Db.WaitingLists.Find(id);
            if (waitingList == null)
            {
                return HttpNotFound();
            }
            return View(waitingList);
        }

        // POST: WaitingLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login"] != null)
            {
                WaitingList waitingList = _Db.WaitingLists.Find(id);
                _Db.WaitingLists.Remove(waitingList);
                _Db.SaveChangesAsync();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (Session["login"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                WaitingList waitingList = _Db.WaitingLists.Find(id);
                if (waitingList == null)
                {
                    return HttpNotFound();
                }

                return View(waitingList);
            }
            else 
            {
                return RedirectToAction("Login", "Account");

            }


           
        }

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
            WaitingList waitingList = await _Db.WaitingLists.FindAsync(id);
            if (waitingList == null)
            {
                return HttpNotFound();
            }


            ViewBag.AvailableCourseID = new SelectList(_Db.AvailableCourses, "AvailableCourseID", "AvailableCourseID", waitingList.AvailableCourseID);
            ViewBag.StudentID = new SelectList(_Db.Students, "StudentID", "FirstName", waitingList.StudentID);
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
            var isAlreadyEnrolled = _WaitingListService.IsAlreadyEnrolled(waitingList.StudentID, waitingList.AvailableCourseID);

            if (isAlreadyEnrolled)
            {
                ModelState.AddModelError("", "You cannot enroll twice in the same course.");
                TempData["ErrorMessage"] = "You cannot enroll twice in the same course.";

                return RedirectToAction("Edit");
            }

            if (ModelState.IsValid)
            {
                await _WaitingListService.UpdateWaitingListAsync(waitingList);
                return RedirectToAction("Index");
            }
            ViewBag.AvailableCourseID = new SelectList(_Db.AvailableCourses, "AvailableCourseID", "AvailableCourseID", waitingList.AvailableCourseID);
            ViewBag.StudentID = new SelectList(_Db.Students, "StudentID", "FirstName", waitingList.StudentID);
            return View(waitingList);
        }


    }
}
