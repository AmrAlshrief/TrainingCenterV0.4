using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
        private readonly int _UserId;
        public bool IsValid { get; set; }

        public InstructorService(int userId) 
        {
            _UserId = userId;
            IsValid = true;
        }

        public InstructorService()
        {
            
        }
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

        public async Task AddInstructorAsync(Instructor instructor)
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
                        UserInfo.CreateAudit(ActionType.Add, Action.AddInstructor, _UserId, MasterEntity.Instructor, "Instructor Added");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        IsValid = false;
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

        public async Task UpdateInstructorAsync(Instructor instructor)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(instructor).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Update, Action.UpdateInstructor, _UserId, MasterEntity.Instructor, "Instructor Info Updated");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        IsValid = false;
                        transaction.Rollback();
                        throw new Exception(ex.Message);

                    }
                }
            }
        }

        public async Task SoftDeleteInstructorAsync(int instructorId)
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
                        UserInfo.CreateAudit(ActionType.Delete, Action.DeleteInstructor, _UserId, MasterEntity.Instructor, "Instructor Deleted");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        IsValid = false;
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public void SoftDeleteInstructor(int instructorId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var instructor = context.Instructors.Find(instructorId);
                        if (instructor == null)
                        {
                            IsValid = false;
                            throw new KeyNotFoundException("Instructor not found.");
                        }

                        instructor.IsDeleted = true;
                        context.SaveChanges();
                        UserInfo.CreateAudit(ActionType.Delete, Action.DeleteInstructor, _UserId, MasterEntity.Instructor, "Instructor Deleted");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        
                        IsValid=false;
                        transaction.Rollback();
                        


                    }
                    
                }
            }
        }

        public async Task<int> GetNumberOfInstructorsAsync()
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                try
                {
                    return await context.Instructors.Where(s => !s.IsDeleted).CountAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public bool IsAttachedToAnother(int id)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                try
                {
                    var availableCourse = context.AvailableCourses.Find(id);

                    bool isReferenced = context.AvailableCourses.Any(x => x.InstructorAvailabilityID == id);

                    return availableCourse != null && isReferenced;
                }
                catch (Exception ex)
                {
                }
            }

            return false;
        }
    }
}
