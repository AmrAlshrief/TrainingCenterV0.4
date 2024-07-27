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
        private readonly int _UserId;

        public RunningCourseService(int userId) 
        {
            _UserId = userId;
        }

        public async Task<IEnumerable<RunningCours>> GetAllRunningCoursesAsync()
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                try
                {
                    var runningCourses = context.RunningCourses.Include(r => r.WaitingList);
                    return await runningCourses.ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task CreateRunningCourseAsync(RunningCours runningCourse)
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
                        UserInfo.CreateAudit(ActionType.Add, Action.AddRunningCourse, _UserId, MasterEntity.RunningCourse, "Running Course Added");
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

        public async Task UpdateRunningCourseAsync(RunningCours runningCourse)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(runningCourse).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Update, Action.UpdateRunningCourse, _UserId, MasterEntity.RunningCourse, "Running Course Info Updated");
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

        public async Task DeleteRunningCourseAsync(int id)
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
                        UserInfo.CreateAudit(ActionType.Delete, Action.DeleteRunningCourse, _UserId, MasterEntity.RunningCourse, "Running Course Deleted");
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

        Task<RunningCours> IRunningCourseService.GetRunningCourseByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
