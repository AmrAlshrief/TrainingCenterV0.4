using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;

namespace TrainingCenterLib.Repository.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<Cours>> GetAllProgCoursesAsync();
        Task<IEnumerable<Cours>> GetAllLanguageCoursesAsync();
        List<Cours> GetCourses(Predicate<Cours> filter);
        List<Cours> GetAllProgrammingCourses();
        List<Cours> GetAllLanguageCourses();
        Task<Cours> GetCourseByIdAsync(int id);
        Task CreateCourseAsync(Cours course, int UserId);
        Task UpdateCourseAsync(Cours course, int UserId);
        Task SoftDeleteCourseAsync(int id, int UserId);
        Task<int> GetNumberOfCoursesAsync();

    }
}
