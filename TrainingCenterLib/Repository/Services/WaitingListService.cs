using System;
using System.Collections.Generic;
using System.Data.Entity;
using WebMatrix.WebData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;
using TrainingCenterLib.Repository.Interfaces;
using System.Runtime.Remoting.Contexts;

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

        public void UpdateActiveCourseSituation(TrainingCenterLibDbContext context, WaitingList waitingList) 
        {
            if (waitingList.IsPaid)
            {
                var activeCourse = context.ActiveCourseByGroups
                .Where(ac => ac.AvailableCourseID == waitingList.AvailableCourseID && DbFunctions.DiffDays(ac.StartAt, DateTime.Now) <= 1)
                .FirstOrDefault();

                if (activeCourse != null && waitingList.AvailableCourseID == activeCourse.AvailableCourseID)
                {
                    waitingList.ActiveCourseID = activeCourse.ActiveCourseID;
                    waitingList.GroupName = activeCourse.GroupName;
                    context.SaveChanges();
                }



                else if (activeCourse == null)
                {

                    var waitingListCount = context.WaitingLists
                    .Count(w => w.AvailableCourseID == waitingList.AvailableCourseID && w.ActiveCourseID == null && waitingList.IsPaid == true);
                    if (waitingListCount >= 3)
                    {
                        NewWaitingListService newWaiting = new NewWaitingListService();


                        newWaiting.CreateActiveCourse(waitingList.AvailableCourseID, "Group" + waitingList.AvailableCourseID);



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
                    //try
                    //{
                        context.Entry(waitingList).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Update, Action.UpdateWaitingList, _UserId, MasterEntity.WaitingList, "Waiting List Info Updated");
                        transaction.Commit();
                    //}
                    //catch (Exception ex)
                    //{
                    //    transaction.Rollback();
                    //    throw new Exception(ex.Message.ToString());
                    //}
                }

                UpdateActiveCourseSituation(context, waitingList);
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
                                  where c.IsDeleted == false
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

        public bool IsAlreadyEnrolled(int StudentId, int AvailableCourseId) 
        {
            using (var context = new TrainingCenterLibDbContext()) 
            {
                try
                {
                    return context.WaitingLists.Any(e => e.StudentID == StudentId && e.AvailableCourseID == AvailableCourseId);
                }
                catch
                {
                }

                return false;
            }
            
            
        }


    }
}
