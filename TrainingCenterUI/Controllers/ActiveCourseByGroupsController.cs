using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainingCenterLib.Entities;
using TrainingCenterLib.Repository;

namespace TrainingCenterUI.Controllers
{
    public class ActiveCourseByGroupsController : Controller
    {
        private TrainingCenterLibDbContext db = new TrainingCenterLibDbContext();

        // GET: ActiveCourseByGroups
        public ActionResult Index()
        {
            if (Session["login"] != null)
            {
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            var activeCourses = db.ActiveCourseByGroups.ToList();

            #region MyRegion
            //var timeSlots = db.TimeSlots.ToList();

            //var viewModel = from ac in activeCourses
            //                join ts in timeSlots on ac.TimeSlotID equals ts.TimeSlotID
            //                select new ActiveCourseByGroupViewModel
            //                {
            //                    ActiveCourseID = ac.ActiveCourseID,
            //                    AvailableCourseID = ac.AvailableCourseID,
            //                    GroupName = ac.GroupName,
            //                    GroupDay = ac.GroupDay.ToString(),
            //                    StartAt = (DateTime)ac.StartAt,
            //                    EndAt = (DateTime)ac.EndAt,
            //                    TimeSlotID = (int)ac.TimeSlotID,
            //                    StartTime = ts.StartTime,
            //                    EndTime = ts.EndTime
            //                }; 
            #endregion

            return View(activeCourses);
        }

        // GET: ActiveCourseByGroups/Details/5
        public ActionResult Details(int? id)
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
            ActiveCourseByGroup activeCourseByGroup = db.ActiveCourseByGroups.Find(id);
            if (activeCourseByGroup == null)
            {
                return HttpNotFound();
            }
            return View(activeCourseByGroup);
        }

        // GET: ActiveCourseByGroups/Create
        public ActionResult Create()
        {
            if (Session["login"] != null)
            {
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // POST: ActiveCourseByGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActiveCourseID,AvailableCourseID,GroupName,GroupDay,TimeSlot,StartAt,EndAt")] ActiveCourseByGroup activeCourseByGroup)
        {
            if (Session["login"] != null)
            {
                
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                db.ActiveCourseByGroups.Add(activeCourseByGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(activeCourseByGroup);
        }

        // GET: ActiveCourseByGroups/Edit/5
        public ActionResult Edit(int? id)
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
            ActiveCourseByGroup activeCourseByGroup = db.ActiveCourseByGroups.Find(id);
            if (activeCourseByGroup == null)
            {
                return HttpNotFound();
            }

            ViewBag.TimeSlotID = new SelectList(db.TimeSlots, "TimeSlotID", "TimeSlotID", activeCourseByGroup.TimeSlotID);

            return View(activeCourseByGroup);
        }

        // POST: ActiveCourseByGroups/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActiveCourseID,AvailableCourseID,GroupName,GroupDay,TimeSlot,StartAt,EndAt")] ActiveCourseByGroup activeCourseByGroup)
        {
            if (Session["login"] != null)
            {
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                db.Entry(activeCourseByGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeSlotID = new SelectList(db.TimeSlots, "TimeSlotID", "TimeSlotID", activeCourseByGroup.TimeSlotID);

            return View(activeCourseByGroup);
        }

        // GET: ActiveCourseByGroups/Delete/5
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
            ActiveCourseByGroup activeCourseByGroup = db.ActiveCourseByGroups.Find(id);
            if (activeCourseByGroup == null)
            {
                return HttpNotFound();
            }

            return View(activeCourseByGroup);
        }

        // POST: ActiveCourseByGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActiveCourseByGroup activeCourseByGroup = db.ActiveCourseByGroups.Find(id);
            db.ActiveCourseByGroups.Remove(activeCourseByGroup);
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
