using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrainingCenterLib.Repository.Services;
using TrainingCenterLib.Repository.Interfaces;
using TrainingCenterLib.Entities;
using TrainingCenterLib.Repository;
using WebMatrix.WebData;

namespace TrainingCenterUI.Controllers
{
    public class InstructorsController : Controller
    {
        private TrainingCenterLibDbContext _db;
        private readonly IInstructorService _InstructorService;
        private readonly int _UserID;

        public InstructorsController() 
        {
            _db = new TrainingCenterLibDbContext();
            _InstructorService = new InstructorService();
            _UserID = WebSecurity.CurrentUserId;

        }

        public async Task<ActionResult> Index()
        {
            if(Session["login"] != null) 
            {
                return View(await _InstructorService.GetAllAsync());
            }
            else 
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        // GET: Instructors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if(Session["login"] != null) 
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var instructor = await _db.Instructors.FindAsync(id);
                if (instructor == null)
                {
                    return HttpNotFound();
                }
                return View(instructor);
            }
            else 
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        // GET: Instructors/Create
        public ActionResult Create()
        {
            if(Session["login"] != null) 
            {
                Instructor instructor = new Instructor();
                return View(instructor);
            }
            else 
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FirstName,SecondName,LastName,Email,Phone")] Instructor instructor)
        {
            if(Session["login"] != null) 
            {
                if (ModelState.IsValid)
                {
                    await _InstructorService.AddInstructorAsync(instructor, _UserID);

                    return RedirectToAction("Index");
                }
                return View(instructor);
            }
            else 
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        // GET: Instructors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["login"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var instructor = await _db.Instructors.FindAsync(id);
                if (instructor == null)
                {
                    return HttpNotFound();
                }
                return View(instructor);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
           

            
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "InstructorID,FirstName,SecondName,LastName,Email,Phone,UserID")] Instructor instructor)
        {
            if (Session["login"] != null)
            {
                if (ModelState.IsValid)
                {
                    await _InstructorService.UpdateInstructorAsync(instructor, _UserID);
                    //db.Entry(instructor).State = EntityState.Modified;
                    //await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(instructor);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        // GET: Instructors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["login"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var instructor = await _db.Instructors.FindAsync(id);
                if (instructor == null)
                {
                    return HttpNotFound();
                }
                return View(instructor);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (Session["login"] != null)
            {
                var instructor = await _db.Instructors.FindAsync(id);
                if (instructor != null)
                {
                    await _InstructorService.SoftDeleteInstructorAsync(id, _UserID);
           
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}
