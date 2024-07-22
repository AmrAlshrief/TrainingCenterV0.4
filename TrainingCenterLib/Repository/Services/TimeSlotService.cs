using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;
using TrainingCenterLib.Repository.Interfaces;

namespace TrainingCenterLib.Repository.Services
{
    public class TimeSlotService : ITimeSlotService
    {

        public async Task<IEnumerable<TimeSlot>> GetAllAsync()
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                try
                {
                    return await context.TimeSlots.ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task AddTimeAsync(TimeSlot timeSlot, int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        context.TimeSlots.Add(timeSlot);
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Add, Action.AddTime, UserId, MasterEntity.TimeSlot, "Time Slot  Added");
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

        public async Task DeleteTimeAsync(int timeId, int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var TimeSlot = await context.TimeSlots.FindAsync(timeId);
                        if (TimeSlot == null)
                        {
                            throw new KeyNotFoundException("Time Slot not found.");
                        }

                        context.TimeSlots.Remove(TimeSlot);
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Delete, Action.DeleteTime, UserId, MasterEntity.TimeSlot, "Time Slot Deleted");
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

        public async Task UpdateTimeAsync(TimeSlot timeSlot, int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(timeSlot).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Update, Action.UpdateTime, UserId, MasterEntity.TimeSlot, "Time Slot  Info Updated");
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
