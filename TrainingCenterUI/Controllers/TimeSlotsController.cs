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
using System.Security.Cryptography.X509Certificates;

namespace TrainingCenterUI.Controllers
{
    public class TimeSlotsController : Controller
    {
        private TrainingCenterLibDbContext db = new TrainingCenterLibDbContext();
        private readonly TimeSlotService _timeSlotService;

        public TimeSlotsController() 
        {
            _UserId = WebSecurity.CurrentUserId;
            _timeSlotService = new TimeSlotService();
        }

        // GET: TimeSlots
        public async Task<ActionResult> Index()
        {
            return View(await db.TimeSlots.ToListAsync());
        }

        // GET: TimeSlots/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeSlot timeSlot = await db.TimeSlots.FindAsync(id);
            if (timeSlot == null)
            {
                return HttpNotFound();
            }
            return View(timeSlot);
        }

        // GET: TimeSlots/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TimeSlots/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TimeSlotID,StartTime,EndTime")] TimeSlot timeSlot)
        {
            if (ModelState.IsValid)
            {
                //////db.TimeSlots.Add(timeSlot);
                //////await db.SaveChangesAsync();
                ///
                await AddTimeAsync(timeSlot, UserId);
                return RedirectToAction("Index");
            }

            return View(timeSlot);
        }

        // GET: TimeSlots/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeSlot timeSlot = await db.TimeSlots.FindAsync(id);
            if (timeSlot == null)
            {
                return HttpNotFound();
            }
            return View(timeSlot);
        }

        // POST: TimeSlots/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TimeSlotID,StartTime,EndTime")] TimeSlot timeSlot)
        {
            if (ModelState.IsValid)
            {
                //////db.Entry(timeSlot).State = EntityState.Modified;
                //////await db.SaveChangesAsync();
                ///
                await UpdateTimeAsync(timeSlot, UserId);
                return RedirectToAction("Index");
            }
            return View(timeSlot);
        }

        // GET: TimeSlots/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeSlot timeSlot = await db.TimeSlots.FindAsync(id);
            if (timeSlot == null)
            {
                return HttpNotFound();
            }
            return View(timeSlot);
        }

        // POST: TimeSlots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            //////TimeSlot timeSlot = await db.TimeSlots.FindAsync(id);
            //////db.TimeSlots.Remove(timeSlot);
            //////await db.SaveChangesAsync();
            ///
            await DeleteTimeAsync(id, UserId);
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
