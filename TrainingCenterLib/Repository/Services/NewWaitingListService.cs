using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;

namespace TrainingCenterLib.Repository.Services
{
    public class NewWaitingListService
    {
        private readonly TrainingCenterLibDbContext _context;

        public NewWaitingListService()
        {
            _context = new TrainingCenterLibDbContext();
        }

        public string AddStudentToWaitingList(int studentId, int availableCourseId, bool isPaid, string GroupName, bool IsCash)
        {
            //try 
            //{
               
                var waitingList = new WaitingList
                {
                    StudentID = studentId,
                    AvailableCourseID = availableCourseId,
                    IsPaid = isPaid,
                    GroupName = "Group " + availableCourseId,
                    IsCash = IsCash
                };
                
                GroupName = waitingList.GroupName;

                _context.WaitingLists.Add(waitingList);
                _context.SaveChanges();

                var waitingListCount = _context.WaitingLists
                    .Count(w => w.AvailableCourseID == availableCourseId && w.ActiveCourseID == null);

                if (waitingListCount >= 3)
                {
                    CreateActiveCourse(availableCourseId, GroupName);
                    return "Group Added To Succesfully to ActiveCourse";
                }
                else
                {
                    if (isPaid)
                    {
                        var activeCourse = _context.ActiveCourseByGroups
                        .Where(ac => ac.AvailableCourseID == availableCourseId && DbFunctions.DiffDays(ac.StartAt, DateTime.Now) <= 1)
                        .FirstOrDefault();

                        if (activeCourse != null)
                        {
                            waitingList.ActiveCourseID = activeCourse.ActiveCourseID;
                            _context.SaveChanges();
                            return "Student added to existing ActiveCourse.";
                        }

                    }
                    else if (!isPaid) 
                    {
                        return "You have to Pay For Course First before join running course";
                    }
                }


            //}
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
            //        }
            //    }
            //    throw;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}


            return $"Please Wait util group arrive to 3 the number now is {waitingListCount}";


        }

        public void CreateActiveCourse(int availableCourseId, string GroupName)
        {

            

            var availableCourse = _context.AvailableCourses
            .Include(ac => ac.Cours)
            .Include(ac => ac.InstructorAvailability)
            .SingleOrDefault(ac => ac.AvailableCourseID == availableCourseId);

            if (availableCourse == null)
            {
                throw new InvalidOperationException("Course is not exist.");
            }

            var course = availableCourse.Cours;
            var instructorAvailability = availableCourse.InstructorAvailability;

            var room = _context.Rooms
            .Where(r => (course.IsProgramming && r.IsProgramming) || (!course.IsProgramming && !r.IsProgramming))
            .FirstOrDefault();

            if (room == null)
            {
                throw new InvalidOperationException("There's no Room Available to Course");
            }

            var waitingListEntries = _context.WaitingLists
                    .Where(w => w.AvailableCourseID == availableCourseId && w.ActiveCourseID == null && w.IsPaid == true)
                    .OrderBy(w => w.WaitingListID)
                    .Take(3)
                    .ToList();

            if (waitingListEntries.Count < 3)
            {
                throw new InvalidOperationException("The waiting list does not contain enough students Or some not paid.");
            }

            var groupDay = instructorAvailability.IsGroupDays1; 
            var timeSlot = _context.TimeSlots
                .SingleOrDefault(ts => ts.TimeSlotID == instructorAvailability.timeSlotID);
            try 
            {
                var activeCourse = new ActiveCourseByGroup
                {
                    AvailableCourseID = availableCourseId,
                    StartAt = DateTime.Now,
                    EndAt = DateTime.Now.AddDays(30),
                    GroupName = GroupName,
                    GroupDay = groupDay,
                    TimeSlotID = timeSlot.TimeSlotID,
                    RoomID = room.RoomID

                };

                _context.ActiveCourseByGroups.Add(activeCourse);
                _context.SaveChanges();


                if (!_context.Rooms.Any(r => r.RoomID == activeCourse.RoomID))
                {
                    throw new InvalidOperationException("RoomID غير صالح.");
                }


                foreach (var entry in waitingListEntries)
                {
                    entry.ActiveCourseID = activeCourse.ActiveCourseID;
                }

                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }
                throw;
            }
            
            
        }

        public IEnumerable<Student> GetStudents()
        {
            return _context.Students.Where(i => !i.IsDeleted).ToList();
        }

        public IEnumerable<AvailableCours> GetAvailableCourses()
        {
            return _context.AvailableCourses.ToList();
        }

        public IEnumerable<Room> GetAvailableRoom()
        {
            return _context.Rooms.ToList();
        }
    }
}
