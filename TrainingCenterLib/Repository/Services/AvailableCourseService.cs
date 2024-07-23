using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;

namespace TrainingCenterLib.Repository.Interfaces
{
    public class AvailableCourseService : IAvailableCourseService
    {
        private readonly int _UserId;

        public AvailableCourseService(int userId) 
        {
            _UserId = userId;
        }
        public async Task<IEnumerable<AvailableCours>> GetAllAvailableCoursesAsync()
        {
            try
            {
                using (var context = new TrainingCenterLibDbContext())
                {

                    return await context.AvailableCourses.ToListAsync();
                    
                }

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        Task<AvailableCours> GetAvailableCourseByIdAsync(int id)
        {
            throw new NotImplementedException();
        }


        public async Task CreateAvailableCourseAsync(AvailableCours availableCours)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        //availableCours.CreatedAt = DateTime.Now;
                        context.AvailableCourses.Add(availableCours);
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Add, Action.AddAvailableCourse, _UserId, MasterEntity.AvailableCourse, "Available Course Added");
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

        public async Task UpdateAvailableCourseAsync(AvailableCours availableCours)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(availableCours).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Update, Action.UpdateAvailableCourse, _UserId, MasterEntity.AvailableCourse, "Available Course Info Updated");
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

        public async Task DeleteAvailableCourseAsync(int id)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var AvailableCourse = await context.AvailableCourses.FindAsync(id);
                        if (AvailableCourse == null)
                        {
                            throw new KeyNotFoundException("Available Course not found.");
                        }

                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Delete, Action.DeleteAvailableCourse, _UserId, MasterEntity.AvailableCourse, "Available Course Deleted");
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

        Task<AvailableCours> IAvailableCourseService.GetAvailableCourseByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
