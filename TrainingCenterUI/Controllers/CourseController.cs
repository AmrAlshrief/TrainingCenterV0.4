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
using Antlr.Runtime;
using TrainingCenterLib.Repository.Services;

namespace TrainingCenterUI.Controllers
{
    public class CourseController : Controller
    {
        private TrainingCenterLibDbContext db = new TrainingCenterLibDbContext();
        private readonly CourseService _CourseService;
        private readonly int _UserId;

        public CourseController() 
        {
            _CourseService = new CourseService();
        }

        // GET: Course
        public async Task<ActionResult> Index()
        {

            var courses = db.Courses.Include(c => c.Courses1).Where(c=> c.CourseName != "N/A" && c.IsProgramming == true);
            return View(await courses.ToListAsync());
        }

        public async Task<ActionResult> IndexForLanguage()
        {

            var courses = db.Courses.Include(c => c.Courses1).Where(c => c.CourseName != "N/A" &&  c.IsProgramming == false);
            return View(await courses.ToListAsync());
        }

        // GET: Course/Details/5
        public async Task<ActionResult> Details(int? id)
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

        // GET: Course/Create
        public ActionResult Create()
        {
            Cours course = new Cours();
            ViewBag.Requirement_CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            return View(course);
        }

        // POST: Course/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CourseID,CourseName,Duration,Requirement_CourseID,IsProgramming,UserID")] Cours cours)
        {
            if (ModelState.IsValid)
            {
                //////cours.CreatedAt = DateTime.Now;
                //////db.Courses.Add(cours);
                //////await db.SaveChangesAsync();

                await CreateCourseAsync(cours, UserId);
                return RedirectToAction("Index");
            }

            ViewBag.Requirement_CourseID = new SelectList(db.Courses, "CourseID", "CourseName", cours.Requirement_CourseID);
            return View(cours);
        }

        // GET: Course/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.Requirement_CourseID = new SelectList(db.Courses, "CourseID", "CourseName", cours.Requirement_CourseID);
            return View(cours);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CourseID,CourseName,Duration,Requirement_CourseID,IsProgramming,IsDeleted,CreatedAt,UserID")] Cours cours)
        {
            if (ModelState.IsValid)
            {

                //////db.Entry(cours).State = EntityState.Modified;
                //////await db.SaveChangesAsync();

                await UpdateCourseAsync(cours, UserId);
                return RedirectToAction("Index");
            }
            ViewBag.Requirement_CourseID = new SelectList(db.Courses, "CourseID", "CourseName", cours.Requirement_CourseID);
            return View(cours);
        }

        // GET: Course/Delete/5
        public async Task<ActionResult> Delete(int? id)
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

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ////Cours cours = await db.Courses.FindAsync(id);
            ////db.Courses.Remove(cours);
            ////await db.SaveChangesAsync();
            ///
            await SoftDeleteCourseAsync(id, UserId);
            if (cours.IsProgramming) 
            {
                return RedirectToAction("Index");
            }
            else 
            {
                return RedirectToAction("IndexForLanguage");
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
