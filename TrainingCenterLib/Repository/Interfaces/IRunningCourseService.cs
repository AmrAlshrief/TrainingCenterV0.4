using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;

namespace TrainingCenterLib.Repository.Interfaces
{
    public interface IRunningCourseService
    {
        Task<IEnumerable<RunningCours>> GetAllRunningCoursesAsync();
        Task<RunningCours> GetRunningCourseByIdAsync(int id);
        Task CreateRunningCourseAsync(RunningCours runningCourse);
        Task UpdateRunningCourseAsync(RunningCours runningCourse);
        Task DeleteRunningCourseAsync(int id);
    }
}
