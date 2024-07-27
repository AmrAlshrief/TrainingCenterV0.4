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
using Antlr.Runtime;
using TrainingCenterLib.Repository.Services;
using WebMatrix.WebData;

namespace TrainingCenterUI.Controllers
{
    public class CourseController : Controller
    {
        private TrainingCenterLibDbContext db = new TrainingCenterLibDbContext();
        private readonly CourseService _CourseService;
        private readonly int _UserId;

        public CourseController() 
        {
            _UserId = WebSecurity.CurrentUserId;
            _CourseService = new CourseService();

        }

        // GET: Course
        public async Task<ActionResult> Index()
        {
            if (Session["login"] != null) 
            {
                //var courses = db.Courses.Include(c => c.Courses1).Where(c=> c.CourseName != "N/A" && c.IsProgramming);
                return View(await _CourseService.GetAllProgCoursesAsync());
            }
            else 
            {
                return RedirectToAction("Login", "Account");
            }

        }

        public async Task<ActionResult> IndexForLanguage()
        {
            if (Session["login"] != null)
            {
                //var courses = db.Courses.Include(c => c.Courses1).Where(c => c.CourseName != "N/A" &&  c.IsProgramming == false);
                return View(await _CourseService.GetAllLanguageCoursesAsync());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        // GET: Course/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (Session["login"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Cours cours = await db.Courses.FindAsync(id);
                if (cours == null)
                {
                    return HttpNotFound();
                }
                return View(cours);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
         
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            if (Session["login"] != null)
            {
                //if (course.IsProgramming == true)
                //{
                //    var ProgrammingCourses = _CourseService.GetAllProgrammingCourses();
                //    ViewBag.Requirement_CourseID = new SelectList(ProgrammingCourses, "CourseID", "CourseName", course.Requirement_CourseID);
                //    
                //}
                //else
                //{
                //    var nonProgrammingCourses = _CourseService.GetAllLanguageCourses();
                //    ViewBag.Requirement_CourseID = new SelectList(nonProgrammingCourses, "CourseID", "CourseName", course.Requirement_CourseID);
                //    return View(course);
                //}
                Cours course = new Cours();
                ViewBag.Requirement_CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
                return View(course);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            

            
        }

        // POST: Course/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CourseID,CourseName,Duration,Requirement_CourseID,IsProgramming,UserID")] Cours cours)
        {
            if (Session["login"] != null)
            {
                if (ModelState.IsValid)
                {
                    cours.CreatedAt = DateTime.Now;
                    db.Courses.Add(cours);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                if (cours.IsProgramming == true)
                {
                    var ProgrammingCourses = _CourseService.GetAllProgrammingCourses();
                    ViewBag.Requirement_CourseID = new SelectList(ProgrammingCourses, "CourseID", "CourseName", cours.Requirement_CourseID);
                }
                else
                {
                    var nonProgrammingCourses = _CourseService.GetAllLanguageCourses();
                    ViewBag.Requirement_CourseID = new SelectList(nonProgrammingCourses, "CourseID", "CourseName", cours.Requirement_CourseID);
                }
                //ViewBag.Requirement_CourseID = new SelectList(db.Courses, "CourseID", "CourseName", cours.Requirement_CourseID);
                return View(cours);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
           
        }

        // GET: Course/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["login"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Cours cours = await db.Courses.FindAsync(id);
                if (cours == null)
                {
                    return HttpNotFound();
                }

                if (cours.IsProgramming == true)
                {
                    var ProgrammingCourses = _CourseService.GetAllProgrammingCourses();
                    ViewBag.Requirement_CourseID = new SelectList(ProgrammingCourses, "CourseID", "CourseName", cours.Requirement_CourseID);
                }
                else
                {
                    var nonProgrammingCourses = _CourseService.GetAllLanguageCourses();
                    ViewBag.Requirement_CourseID = new SelectList(nonProgrammingCourses, "CourseID", "CourseName", cours.Requirement_CourseID);
                }

                //ViewBag.Requirement_CourseID = new SelectList(db.Courses, "CourseID", "CourseName", cours.Requirement_CourseID);
                return View(cours);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        // POST: Course/Edit/5
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CourseID,CourseName,Duration,Requirement_CourseID,IsProgramming,IsDeleted,CreatedAt,UserID")] Cours cours)
        {
            if (Session["login"] != null)
            {
                if (ModelState.IsValid)
                {

                    db.Entry(cours).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                if (cours.IsProgramming == true)
                {
                    var ProgrammingCourses = _CourseService.GetAllProgrammingCourses();
                    ViewBag.Requirement_CourseID = new SelectList(ProgrammingCourses, "CourseID", "CourseName", cours.Requirement_CourseID);
                }
                else
                {
                    var nonProgrammingCourses = _CourseService.GetAllLanguageCourses();
                    ViewBag.Requirement_CourseID = new SelectList(nonProgrammingCourses, "CourseID", "CourseName", cours.Requirement_CourseID);
                }

                return View(cours);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
      
        }

        // GET: Course/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["login"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Cours cours = await db.Courses.FindAsync(id);
                if (cours == null)
                {
                    return HttpNotFound();
                }
                return View(cours);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
           
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (Session["login"] != null)
            {
                Cours cours = await db.Courses.FindAsync(id);
                db.Courses.Remove(cours);
                await db.SaveChangesAsync();
                if (cours.IsProgramming)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("IndexForLanguage");
                }
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
