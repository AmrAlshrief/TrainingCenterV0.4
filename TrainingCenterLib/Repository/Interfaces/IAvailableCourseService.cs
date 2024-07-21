using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;

namespace TrainingCenterLib.Repository.Interfaces
{
    internal interface IAvailableCourseService
    {
        Task<IEnumerable<AvailableCours>> GetAllAvailableCoursesAsync();
        Task<AvailableCours> GetAvailableCourseByIdAsync(int id);
        Task CreateAvailableCourseAsync(AvailableCours availableCours, int UserId);
        Task UpdateAvailableCourseAsync(AvailableCours availableCours, int UserId);
        Task DeleteAvailableCourseAsync(int id, int UserId);
    }
}
