using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;

namespace TrainingCenterLib.Repository.Interfaces
{
    internal interface ICourseService
    {
        Task<List<Cours>> GetAllCoursesAsync();
        Task<Cours> GetCourseByIdAsync(int id);
        Task CreateCourseAsync(Cours course);
        Task UpdateCourseAsync(Cours course);
        Task DeleteCourseAsync(int id);
    }
}
