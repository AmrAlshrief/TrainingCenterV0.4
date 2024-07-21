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
    public class InstructorAvailabilityService : IInstructorAvailabilityService
    {
        

        public async Task<IEnumerable<InstructorAvailability>> GetAllInstructorAvailabilitiesAsync()
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                try
                {
                    return await context.InstructorAvailabilities.ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        Task<InstructorAvailability> GetInstructorAvailabilityByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task CreateInstructorAvailabilityAsync(InstructorAvailability instructorAvailability, int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        context.InstructorAvailabilities.Add(instructorAvailability);
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Add, Action.AddInstructorTimeForCourse, UserId, MasterEntity.InstructorAvailability, "Instructor Availability Added");
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

        public async Task UpdateInstructorAvailabilityAsync(InstructorAvailability instructorAvailability, int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(instructorAvailability).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Update, Action.UpdateInstructorTimeForCourse, UserId, MasterEntity.InstructorAvailability, "Instructor Availability Info Updated");
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

        

        public async Task DeleteInstructorAvailabilityAsync(int id,int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var InstructorAvailability = await context.InstructorAvailabilities.FindAsync(id);
                        if (InstructorAvailability == null)
                        {
                            throw new KeyNotFoundException("Instructor Availability  not found.");
                        }

                        context.InstructorAvailabilities.Remove(InstructorAvailability);

                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Delete, Action.DeleteInstructorTime, UserId, MasterEntity.InstructorAvailability, "Instructor Availability Deleted");
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
