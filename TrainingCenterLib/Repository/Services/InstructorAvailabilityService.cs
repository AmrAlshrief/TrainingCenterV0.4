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
        private readonly int _UserId;

        public InstructorAvailabilityService(int userId) 
        {
            _UserId = userId;
        }

        public async Task<IEnumerable<InstructorAvailability>> GetAllInstructorAvailabilitiesAsync()
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                try
                {
                    var instructorAvailabilities = context.InstructorAvailabilities.Include(i => i.Instructor).Include(i => i.TimeSlot);
                    return await instructorAvailabilities.ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }


        public async Task CreateInstructorAvailabilityAsync(InstructorAvailability instructorAvailability)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        context.InstructorAvailabilities.Add(instructorAvailability);
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Add, Action.AddInstructorTimeForCourse, _UserId, MasterEntity.InstructorAvailability, "Instructor Availability Added");
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

        public async Task UpdateInstructorAvailabilityAsync(InstructorAvailability instructorAvailability)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(instructorAvailability).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Update, Action.UpdateInstructorTimeForCourse, _UserId, MasterEntity.InstructorAvailability, "Instructor Availability Info Updated");
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

        

        public async Task DeleteInstructorAvailabilityAsync(int id)
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
                        UserInfo.CreateAudit(ActionType.Delete, Action.DeleteInstructorTime, _UserId, MasterEntity.InstructorAvailability, "Instructor Availability Deleted");
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

        Task<InstructorAvailability> IInstructorAvailabilityService.GetInstructorAvailabilityByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
