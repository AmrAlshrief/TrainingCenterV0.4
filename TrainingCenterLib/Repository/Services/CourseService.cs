using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;
using TrainingCenterLib.Repository.Interfaces;

namespace TrainingCenterLib.Repository.Services
{
    public class CourseService : ICourseService
    {
        public async Task<IEnumerable<Cours>> GetAllProgCoursesAsync()
        {
            try
            {
                using (var context = new TrainingCenterLibDbContext())
                {
                    var courses = context.Courses.Include(c => c.Courses1);

                    return await courses
                        .Where(i => !i.IsDeleted)
                        .ToListAsync();
                }

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<IEnumerable<Cours>> GetAllLanguageCoursesAsync()
        {
            try 
            {
                using (var context = new TrainingCenterLibDbContext())
                {
                    var courses = context.Courses.Include(c => c.Courses1);
                    return await courses
                        .Where(i => !i.IsDeleted && !i.IsProgramming)
                        .ToListAsync();
                }
              
            }
            
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public Task<Cours> GetCourseByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task CreateCourseAsync(Cours course, int UserId)
        {
            try
            {
                using (var context = new TrainingCenterLibDbContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {

                        try
                        {
                            course.CreatedAt = DateTime.Now;
                            context.Courses.Add(course);
                            UserInfo.CreateAudit(ActionType.Add, Action.AddStudent, UserId, MasterEntity.Student, "Add Student");
                            await context.SaveChangesAsync();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }

        }

        public async Task SoftDeleteCourseAsync(int id, int UserId)
        {
            try 
            {

                using(var context = new TrainingCenterLibDbContext()) 
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var course = await context.Courses.FindAsync(id) ?? throw new KeyNotFoundException("Student not found.");
                            course.IsDeleted = true;

                            UserInfo.CreateAudit(ActionType.Delete, Action.DeleteCourse, UserId, MasterEntity.Course, "Delete Course");
                            await context.SaveChangesAsync();

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        
        public async Task UpdateCourseAsync(Cours course, int UserId)
        {
            try
            {
                using (var context = new TrainingCenterLibDbContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            context.Entry(course).State = EntityState.Modified;
                            UserInfo.CreateAudit(ActionType.Update, Action.UpdateCourse, UserId, MasterEntity.Course, "Update Course");
                            await context.SaveChangesAsync();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetNumberOfCoursesAsync()
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                try
                {
                    return await context.Courses.Where(s => !s.IsDeleted).CountAsync();
                }
                catch (Exception ex) 
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public List<Cours> GetCourses(Predicate<Cours> filter)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                try
                {
                    var courses = context.Courses.ToList(); 
                    var filteredCourses = courses.Where(c => filter(c)).ToList();
                   
                    return courses;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public List<Cours> GetAllProgrammingCourses()
        {
            //return GetCourses(c => c.IsProgramming);

            using (var context = new TrainingCenterLibDbContext())
            {
                try
                {
                    var filteredCourses = context.Courses.Where(c => c.IsProgramming).ToList();

                    return filteredCourses;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public List<Cours> GetAllLanguageCourses()
        {
            //return GetCourses(c => c.IsProgramming != true);

            using (var context = new TrainingCenterLibDbContext())
            {
                try
                {
                    
                    var filteredCourses = context.Courses.Where(c => !c.IsProgramming).ToList();

                    return filteredCourses;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
