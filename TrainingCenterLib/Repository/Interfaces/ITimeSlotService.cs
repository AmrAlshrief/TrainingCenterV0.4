using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;

namespace TrainingCenterLib.Repository.Interfaces
{
    internal interface ITimeSlotService
    {
        Task<IEnumerable<TimeSlot>> GetAllAsync();
        Task AddTimeAsync(TimeSlot timeSlot);
        void AddTime(TimeSlot timeSlot);
        Task UpdateTimeAsync(TimeSlot timeSlot);
        Task DeleteTimeAsync(int timeId);
    }
}
