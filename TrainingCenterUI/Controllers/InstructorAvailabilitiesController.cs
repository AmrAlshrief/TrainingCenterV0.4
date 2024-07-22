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
using TrainingCenterUI.Entities;

namespace TrainingCenterUI.Controllers
{
    public class InstructorAvailabilitiesController : Controller
    {
        private TrainingCenterLibDbContext db = new TrainingCenterLibDbContext();
        private readonly InstructorsAvailabilityService _instructorsAvailabilityService;
        private readonly int _UserId;

        public InstructorAvailabilitiesController()
        {
            _UserId = WebSecurity.CurrentUserId;
            _instructorsAvailabilityService = new InstructorsAvailabilityService();
        }
        // GET: InstructorAvailabilities
        public async Task<ActionResult> Index()
        {
            var instructorAvailabilities = db.InstructorAvailabilities.Include(i => i.Instructor).Include(i => i.TimeSlot);
            return View(await instructorAvailabilities.ToListAsync());
        }

        // GET: InstructorAvailabilities/Details/5
        public async Task<ActionResult> Details(int? id)
        {
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
            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FirstName");
            ViewBag.timeSlotID = new SelectList(db.TimeSlots, "TimeSlotID", "TimeSlotID");
            return View();
        }

        // POST: InstructorAvailabilities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "InstructorAvailabilityID,InstructorID,timeSlotID,IsGroupDays1")] InstructorAvailability instructorAvailability)
        {
            if (ModelState.IsValid)
            {
                //////db.InstructorAvailabilities.Add(instructorAvailability);
                //////await db.SaveChangesAsync();
                ///
                await _instructorsAvailabilityService.CreateInstructorAvailabilityAsync(instructorAvailability);
                return RedirectToAction("Index");
            }

            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FirstName", instructorAvailability.InstructorID);
            ViewBag.timeSlotID = new SelectList(db.TimeSlots, "TimeSlotID", "TimeSlotID", instructorAvailability.timeSlotID);
            return View(instructorAvailability);
        }

        // GET: InstructorAvailabilities/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorAvailability instructorAvailability = await db.InstructorAvailabilities.FindAsync(id);
            if (instructorAvailability == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FirstName", instructorAvailability.InstructorID);
            ViewBag.timeSlotID = new SelectList(db.TimeSlots, "TimeSlotID", "TimeSlotID", instructorAvailability.timeSlotID);
            return View(instructorAvailability);
        }

        // POST: InstructorAvailabilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "InstructorAvailabilityID,InstructorID,timeSlotID,IsGroupDays1")] InstructorAvailability instructorAvailability)
        {
            if (ModelState.IsValid)
            {
                //////db.Entry(instructorAvailability).State = EntityState.Modified;
                //////await db.SaveChangesAsync();
                ///
                await _instructorsAvailabilityService.UpdateInstructorAvailabilityAsync(instructorAvailability); 
                return RedirectToAction("Index");
            }
            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FirstName", instructorAvailability.InstructorID);
            ViewBag.timeSlotID = new SelectList(db.TimeSlots, "TimeSlotID", "TimeSlotID", instructorAvailability.timeSlotID);
            return View(instructorAvailability);
        }

        // GET: InstructorAvailabilities/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
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

        // POST: InstructorAvailabilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            //////InstructorAvailability instructorAvailability = await db.InstructorAvailabilities.FindAsync(id);
            //////db.InstructorAvailabilities.Remove(instructorAvailability);
            //////await db.SaveChangesAsync();
            ///
            await _instructorsAvailabilityService.DeleteInstructorAvailabilityAsync(id);
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
