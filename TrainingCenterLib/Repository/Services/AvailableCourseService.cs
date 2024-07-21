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
        public async Task<IEnumerable<AvailableCours>> GetAllCoursesAsync()
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

        Task<AvailableCours> IAvailableCourseService.GetAvailableCourseByIdAsync(int id)
        {
            throw new NotImplementedException();
        }


        public async Task CreateAvailableCourseAsync(AvailableCours availableCours, int UserId)
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
                        UserInfo.CreateAudit(ActionType.Add, Action.AddAvailableCourse, UserId, MasterEntity.AvailableCourse, "Available Course Added");
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

        public async Task UpdateAvailableCourseAsync(AvailableCours availableCours, int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(availableCours).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Update, Action.UpdateAvailableCourse, UserId, MasterEntity.AvailableCourse, "Available Course Info Updated");
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

        public async Task DeleteAvailableCourseAsync(int id, int UserId)
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
                        UserInfo.CreateAudit(ActionType.Delete, Action.DeleteAvailableCourse, UserId, MasterEntity.AvailableCourse, "Available Course Deleted");
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
