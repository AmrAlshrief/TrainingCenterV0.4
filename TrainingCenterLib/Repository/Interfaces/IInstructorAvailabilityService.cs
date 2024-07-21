using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;

namespace TrainingCenterLib.Repository.Interfaces
{
    internal interface IInstructorAvailabilityService
    {
        Task<IEnumerable<InstructorAvailability>> GetAllInstructorAvailabilitiesAsync();
        Task<InstructorAvailability> GetInstructorAvailabilityByIdAsync(int id);
        Task CreateInstructorAvailabilityAsync(InstructorAvailability instructorAvailability);
        Task UpdateInstructorAvailabilityAsync(InstructorAvailability instructorAvailability);
        Task DeleteInstructorAvailabilityAsync(int id);
    }
}
