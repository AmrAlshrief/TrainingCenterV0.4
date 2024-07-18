using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenterLib.Repository
{
    public enum MasterEntity 
    {
        Course = 1,
        Instructor = 2,
        Student = 3, 
        Room = 4,
        TimeSlot = 5,
        InstructorAvailabilitity = 6,
        AvailableCourse = 7,
        WaitingList = 8,
        RunningCourse = 9
    }

    public enum ActionType 
    {
        Add = 1,
        Update = 2,
        Delete = 3
    }

    public enum Action 
    {
        AddInstructor = 1,
        UpdateInstructor = 2,
        DeleteInstructor = 3,
        AddCourse = 4,
        UpdateCourse = 5,
        DeleteCourse = 6,
        AddTime = 7, 
        UpdateTime = 8,
        DeleteTime = 9,
        AddRoom = 10,
        UpdateRoom = 11,
        DeleteRoom = 12,
        AddInstructorTime = 13,
        UpdateInstructorTime = 14,
        DeleteInstructorTime = 15,
        AddInstructorTimeForCourse = 16,
        UpdateInstructorTimeForCourse = 17,
        DeleteInstructorTimeForCourse = 18,
        AddWaitingList = 19,
        UpdateWaitingList = 20,
        DeleteWaitingList = 21,
        AddRunningCourse = 22,
        UpdateRunningCourse = 23,
        DeleteRunningCourse = 24,
        AddStudent = 25,
        UpdateStudent = 26,
        DeleteStudent = 27,

    }
    internal class Utilities
    {
        //public void CreateAudit(ActionType actionType, Action action, int userID, MasterEntity masterEntity) 
        //{

        //}
    }
}
