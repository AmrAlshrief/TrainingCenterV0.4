using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;
using TrainingCenterLib.Repository.Interfaces;

namespace TrainingCenterLib.Repository
{
    public class StudentService : IStudentService
    {
        

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            using(var context = new TrainingCenterLibDbContext()) 
            {
                
                try
                {
                    return await context.Students
                      .Where(s => !s.IsDeleted)
                      .ToListAsync();
                }
                catch (Exception ex) 
                {
                    throw new Exception(ex.Message);
                }
            }


        }

       
        public async Task<Student>GetByIdAsync(int id) 
        {
            return null;
        }

        public async Task<Student> FindStudentByPhoneNumberAsync(string phoneNumber)
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
                    return await context.Students.SingleOrDefaultAsync(s => s.Phone == trimmedPhoneNumber);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }


        public async Task AddStudentAsync(Student student, int UserId)
        {
            try 
            {
                using (var context = new TrainingCenterLibDbContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {

                        try
                        {
                            student.CreatedAt = DateTime.Now;
                            context.Students.Add(student);
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
            }catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }


        }

        public void AddStudent(Student student)
        {
            using (var context = new TrainingCenterLibDbContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    { 


                     // student.CreatedAt = DateTime.Now;
                      context.Students.Add(student);
                      context.SaveChangesAsync();
                      transaction.Commit();
                    }
                    catch(Exception ex) 
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }

            }
            
        }


        public async Task UpdateStudentAsync(Student student, int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(student).State = EntityState.Modified;
                        UserInfo.CreateAudit(ActionType.Update, Action.UpdateStudent, UserId, MasterEntity.Student, "Update Student");
                        await context.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception (ex.Message);
                    }
                }
            }
                
        }


        public async Task SoftDeleteStudentAsync(int studentId, int UserId)
        {

            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var student = await context.Students.FindAsync(studentId);
                        if (student == null)
                        {
                            throw new KeyNotFoundException("Student not found.");
                        }

                        student.IsDeleted = true;

                        UserInfo.CreateAudit(ActionType.Delete, Action.DeleteStudent, UserId, MasterEntity.Student, "Delete Student");
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

        public async Task<int> GetNumberOfStudentsAsync()
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                try
                {
                    return await context.Students.Where(s => !s.IsDeleted).CountAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
    
}
