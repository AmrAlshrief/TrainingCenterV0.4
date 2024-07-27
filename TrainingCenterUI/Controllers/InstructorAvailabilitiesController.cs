﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainingCenterLib.Entities;
using WebMatrix.WebData;
using TrainingCenterLib.Repository.Services;
using TrainingCenterLib.Repository.Interfaces;

namespace TrainingCenterUI.Controllers
{
    public class InstructorAvailabilitiesController : Controller
    {
        private TrainingCenterLibDbContext db = new TrainingCenterLibDbContext();
        private readonly int _UserId;
        private  IInstructorAvailabilityService _instructorAvailabilityService;
        public InstructorAvailabilitiesController() 
        {
            _UserId = WebSecurity.CurrentUserId;
            _instructorAvailabilityService = new InstructorAvailabilityService(_UserId);
        }

        // GET: InstructorAvailabilities
        public async Task<ActionResult> Index()
        {
            if (Session["login"] != null)
            {

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View(await _instructorAvailabilityService.GetAllInstructorAvailabilitiesAsync());
        }

        // GET: InstructorAvailabilities/Details/5
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
            InstructorAvailability instructorAvailability = await db.InstructorAvailabilities.FindAsync(id);
            if (instructorAvailability == null)
            {
                return HttpNotFound();
            }
            return View(instructorAvailability);
        }

        // GET: InstructorAvailabilities/Create
        public ActionResult Create()
        {
            if (Session["login"] != null)
            {

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            var activeInstructors = db.Instructors.Where(i => i.IsDeleted == false).ToList();
            ViewBag.InstructorID = new SelectList(activeInstructors, "InstructorID", "FirstName");
            ViewBag.timeSlotID = new SelectList(db.TimeSlots, "TimeSlotID", "TimeSlotID");
            InstructorAvailability instructorAvailability = new InstructorAvailability();
            return View(instructorAvailability);
        }

        // POST: InstructorAvailabilities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "InstructorAvailabilityID,InstructorID,timeSlotID,IsGroupDays1")] InstructorAvailability instructorAvailability)
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
                //db.InstructorAvailabilities.Add(instructorAvailability);
                //await db.SaveChangesAsync();
                await _instructorAvailabilityService.CreateInstructorAvailabilityAsync(instructorAvailability);
                return RedirectToAction("Index");
            }

            var activeInstructors = db.Instructors.Where(i => i.IsDeleted == false).ToList();
            ViewBag.InstructorID = new SelectList(activeInstructors, "InstructorID", "FirstName", instructorAvailability.InstructorID);
            ViewBag.timeSlotID = new SelectList(db.TimeSlots, "TimeSlotID", "TimeSlotID", instructorAvailability.timeSlotID);
            return View(instructorAvailability);
        }

        // GET: InstructorAvailabilities/Edit/5
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
            InstructorAvailability instructorAvailability = await db.InstructorAvailabilities.FindAsync(id);
            if (instructorAvailability == null)
            {
                return HttpNotFound();
            }
            var activeInstructors = db.Instructors.Where(i => i.IsDeleted == false).ToList();
            ViewBag.InstructorID = new SelectList(activeInstructors, "InstructorID", "FirstName", instructorAvailability.InstructorID);
            ViewBag.timeSlotID = new SelectList(db.TimeSlots, "TimeSlotID", "TimeSlotID", instructorAvailability.timeSlotID);
            return View(instructorAvailability);
        }

        // POST: InstructorAvailabilities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "InstructorAvailabilityID,InstructorID,timeSlotID,IsGroupDays1")] InstructorAvailability instructorAvailability)
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
                //db.Entry(instructorAvailability).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                await _instructorAvailabilityService.UpdateInstructorAvailabilityAsync(instructorAvailability);
                return RedirectToAction("Index");
            }
            var activeInstructors = db.Instructors.Where(i => i.IsDeleted == false).ToList();
            ViewBag.InstructorID = new SelectList(activeInstructors, "InstructorID", "FirstName", instructorAvailability.InstructorID);
            ViewBag.timeSlotID = new SelectList(db.TimeSlots, "TimeSlotID", "TimeSlotID", instructorAvailability.timeSlotID);
            return View(instructorAvailability);
        }

        // GET: InstructorAvailabilities/Delete/5
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

            
            int Id = (int)id;
            bool isReferenced = _instructorAvailabilityService.IsAttachedToAnother(Id);

            if (isReferenced)
            {
                ModelState.AddModelError("", "Cannot delete this Instructor Availability as it is referenced by other records.");
                TempData["ErrorMessage"] = "Cannot delete this Instructor Availability as it is referenced by other records.";
                return RedirectToAction("Index");
            }


            InstructorAvailability instructorAvailability = await db.InstructorAvailabilities.FindAsync(id);
            if (instructorAvailability == null)
            {
                return HttpNotFound();
            }
            return View(instructorAvailability);
        }

        // POST: InstructorAvailabilities/Delete/5
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
            InstructorAvailability instructorAvailability = await db.InstructorAvailabilities.FindAsync(id);
            //db.InstructorAvailabilities.Remove(instructorAvailability);
            //await db.SaveChangesAsync();

            
            await _instructorAvailabilityService.DeleteInstructorAvailabilityAsync(id);
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
