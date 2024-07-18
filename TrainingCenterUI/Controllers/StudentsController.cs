using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainingCenterLib.Repository;
using TrainingCenterLib.Repository.Interfaces;
using TrainingCenterLib.Entities;
using WebMatrix.WebData;

namespace TrainingCenterUI.Controllers
{
    public class StudentsController : Controller
    {
        private readonly TrainingCenterLibDbContext _Db;
        private readonly IStudentService _StudentService;
        private readonly int _UserId;
        // GET: Students

        public StudentsController() 
        {
            _Db = new TrainingCenterLibDbContext();
            _StudentService = new StudentService();
            _UserId = WebSecurity.CurrentUserId;
        }
        public async Task<ActionResult> Index()
        {
            if (Session["login"] != null)
            {
                return View(await _StudentService.GetAllAsync());

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Students/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (Session["login"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Student student = await _Db.Students.FindAsync(id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                return View(student);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
            
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            if (Session["login"] != null)
            {
                Student student = new Student();
                return View(student);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        // POST: Students/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FirstName,SecondName,LastName,Email,Phone,BirthDate,UserID")] Student student)
        {
            if (Session["login"] != null)
            {
                if (ModelState.IsValid)
                {
                    await _StudentService.AddStudentAsync(student, _UserId);
                    return RedirectToAction("Index");
                }

                return View(student);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
           
        }

        // GET: Students/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["login"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Student student = await _Db.Students.FindAsync(id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                return View(student);

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "StudentID,FirstName,SecondName,LastName,Email,Phone,BirthDate,CreatedAt,UserID")] Student student)
        {
            if (Session["login"] != null)
            {
                if (ModelState.IsValid)
                {
                    await _StudentService.UpdateStudentAsync(student, _UserId);
                    return RedirectToAction("Index");
                }
                return View(student);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        // GET: Students/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["login"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Student student = await _Db.Students.FindAsync(id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                return View(student);

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (Session["login"] != null) 
            {
                await _StudentService.SoftDeleteStudentAsync(id, _UserId);
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
                _Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
