using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;
using TrainingCenterLib.Repository.Interfaces;

namespace TrainingCenterLib.Repository.Services
{
    public class InstructorService : IInstructorService
    {
        public async Task<IEnumerable<Instructor>> GetAllAsync()
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                try
                {
                    return await context.Instructors
                        .Where(i => !i.IsDeleted)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<Instructor> FindInstructorByPhoneNumberAsync(string phoneNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    return null;
                }

                string trimmedPhoneNumber = phoneNumber.Trim();

                using (var context = new TrainingCenterLibDbContext())
                {
                    return await context.Instructors.SingleOrDefaultAsync(s => s.Phone == trimmedPhoneNumber);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            

        }

        public async Task AddInstructorAsync(Instructor instructor, int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        instructor.CreatedAt = DateTime.Now;
                        instructor.HiringDate = DateTime.Now;
                        context.Instructors.Add(instructor);
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Add, Action.AddInstructor, UserId, MasterEntity.Instructor, "Instructor Added");
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

        //public void AddInstructor(Instructor instructor)
        //{
        //    using (var context = new TrainingCenterLibDbContext())
        //    {
        //        using (var transaction = context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                instructor.CreatedAt = DateTime.Now;
        //                instructor.HiringDate = DateTime.Now;
        //                context.Instructors.Add(instructor);
        //                context.SaveChanges();
        //                UserInfo.CreateAudit(ActionType.Add, Action.AddInstructor, UserInfo.GlobalUserID, MasterEntity.Instructor, "Instructor Added");
        //                transaction.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                throw new Exception(ex.Message);
        //            }
        //        }
        //    }
        //}

        public async Task UpdateInstructorAsync(Instructor instructor, int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(instructor).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Update, Action.UpdateInstructor, UserId, MasterEntity.Instructor, "Instructor Info Updated");
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

        public async Task SoftDeleteInstructorAsync(int instructorId, int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var instructor = await context.Instructors.FindAsync(instructorId);
                        if (instructor == null)
                        {
                            throw new KeyNotFoundException("Instructor not found.");
                        }

                        instructor.IsDeleted = true;
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Delete, Action.DeleteInstructor, UserId, MasterEntity.Instructor, "Instructor Deleted");
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
