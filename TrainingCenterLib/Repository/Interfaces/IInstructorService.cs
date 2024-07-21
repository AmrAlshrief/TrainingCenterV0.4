using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;

namespace TrainingCenterLib.Repository.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<Instructor>> GetAllAsync();
        Task <Instructor> FindInstructorByPhoneNumberAsync(string phoneNumber);
        Task AddInstructorAsync(Instructor instructor);
        //void AddInstructor(Instructor instructor);
        Task UpdateInstructorAsync(Instructor instructor);
        Task SoftDeleteInstructorAsync(int instructorId);
        Task<int> GetNumberOfInstructorsAsync();
    }
}
