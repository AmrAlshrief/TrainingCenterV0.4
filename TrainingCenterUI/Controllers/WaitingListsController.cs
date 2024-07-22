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
    public class WaitingListsController : Controller
    {
        private TrainingCenterLibDbContext db = new TrainingCenterLibDbContext();

        // GET: WaitingLists
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
            ViewBag.AvailableCourseID = new SelectList(db.AvailableCourses, "AvailableCourseID", "AvailableCourseID");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName");
            return View();
        }

        // POST: WaitingLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "WaitingListID,StudentID,AvailableCourseID,GroupName,IsPaid,IsCash")] WaitingList waitingList)
        {
            if (ModelState.IsValid)
            {
                db.WaitingLists.Add(waitingList);
                await db.SaveChangesAsync();
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "WaitingListID,StudentID,AvailableCourseID,GroupName,IsPaid,IsCash")] WaitingList waitingList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(waitingList).State = EntityState.Modified;
                await db.SaveChangesAsync();
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
            WaitingList waitingList = await db.WaitingLists.FindAsync(id);
            db.WaitingLists.Remove(waitingList);
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
