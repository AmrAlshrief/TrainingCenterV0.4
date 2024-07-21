using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;
using TrainingCenterLib.Repository.Interfaces;

namespace TrainingCenterLib.Repository.Services
{
    public class RunningCourseService : IRunningCourseService
    {
        public async Task<IEnumerable<RunningCours>> GetAllRunningCoursesAsync()
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                try
                {
                    return await context.RunningCourses.ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        Task<RunningCours> GetRunningCourseByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task CreateRunningCourseAsync(RunningCours runningCourse, int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        runningCourse.CreatedAt = DateTime.Now;
                        context.RunningCourses.Add(runningCourse);
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Add, Action.AddRunningCourse, UserId, MasterEntity.RunningCourse, "Running Course Added");
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

        public async Task UpdateRunningCourseAsync(RunningCours runningCourse, int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(runningCourse).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Update, Action.UpdateRunningCourse, UserId, MasterEntity.RunningCourse, "Running Course Info Updated");
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

        public async Task DeleteRunningCourseAsync(int id, int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var RunningCourse = await context.RunningCourses.FindAsync(id);
                        if (RunningCourse == null)
                        {
                            throw new KeyNotFoundException("Running Course not found.");
                        }
                        context.RunningCourses.Remove(RunningCourse);
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Delete, Action.DeleteRunningCourse, UserId, MasterEntity.RunningCourse, "Running Course Deleted");
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

        

        
    }
}
