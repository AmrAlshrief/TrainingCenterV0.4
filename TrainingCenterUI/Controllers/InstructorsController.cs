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
using System.Data.Entity.Validation;

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
            _UserID = WebSecurity.CurrentUserId;
            _InstructorService = new InstructorService(_UserID);

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
                    await _InstructorService.AddInstructorAsync(instructor);
                    if (!_InstructorService.IsValid || instructor == null)
                    {
                        ModelState.AddModelError("", "Unable to Update Instructor.");
                        TempData["ErrorMessage"] = "Unable to Update Instructor";
                        RedirectToAction("Index");
                    }
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
                    await _InstructorService.UpdateInstructorAsync(instructor);
                    if (!_InstructorService.IsValid || instructor == null)
                    {
                        ModelState.AddModelError("", "Unable to Update Instructor.");
                        TempData["ErrorMessage"] = "Unable to Update Instructor";
                        RedirectToAction("Index");
                    }
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
        public ActionResult DeleteConfirmed(int id)
        {
           
            if (Session["login"] != null)
            {
               
                
                    try
                    {
                        var instructor = _db.Instructors.FindAsync(id);
                        if (instructor != null)
                        {
                            _InstructorService.SoftDeleteInstructor(id);

                        }
                        if (!_InstructorService.IsValid || instructor == null)
                        {
                            ModelState.AddModelError("", "You cannot enroll twice in the same course.");
                            TempData["ErrorMessage"] = "Unable to Delete Instructor";
                            RedirectToAction("Index");
                        }
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                        // Join the error messages into a single string
                        var fullErrorMessage = string.Join("; ", errorMessages);

                        // Combine the original exception message with the new one
                        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                        TempData["ErrorMessage"] = exceptionMessage;

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
