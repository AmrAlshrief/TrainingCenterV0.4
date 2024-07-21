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
    public class WaitingListService : IWaitingListService
    {
        private readonly int _UserId;
        public WaitingListService(int UserId) 
        {
            _UserId = UserId;
        }
        //Dont Implement this method in Control
        public async Task<IEnumerable<WaitingList>> GetAllWaitingListsAsync()
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                try
                {
                    
                   return null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        //Dont Implement this method in Control
        public async Task<WaitingList> GetWaitingListByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task CreateWaitingListAsync(WaitingList waitingList)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        context.WaitingLists.Add(waitingList);
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Add, Action.AddWaitingList, _UserId, MasterEntity.WaitingList, "Waiting List Added");
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

        public async Task UpdateWaitingListAsync(WaitingList waitingList)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(waitingList).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Update, Action.UpdateWaitingList, _UserId, MasterEntity.WaitingList, "Waiting List Info Updated");
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

        public async Task DeleteWaitingListAsync(int id)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var WaitingList = await context.WaitingLists.FindAsync(id);
                        if (WaitingList == null)
                        {
                            throw new KeyNotFoundException("Waiting List not found.");
                        }

                        context.WaitingLists.Remove(WaitingList);
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Delete, Action.DeleteWaitingList, _UserId, MasterEntity.WaitingList, "Waiting List Deleted");
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

        
        public List<CourseDto> GetCourseName() 
        {
            using(var context = new TrainingCenterLibDbContext()) 
            {
                try
                {
                    var courses = from ac in context.AvailableCourses
                                  join c in context.Courses on ac.CourseID equals c.CourseID
                                  select new CourseDto
                                  {
                                      AvailableCourseID = ac.AvailableCourseID,
                                      CourseName = c.CourseName
                                  };

                    return courses.ToList();
                }
                catch (Exception ex) 
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        
    }
}
