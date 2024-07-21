using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;

namespace TrainingCenterLib.Repository.Interfaces
{
    internal interface IWaitingListService
    {
        Task<IEnumerable<WaitingList>> GetAllWaitingListsAsync();
        Task<WaitingList> GetWaitingListByIdAsync(int id);
        Task CreateWaitingListAsync(WaitingList waitingList, int UserId);
        Task UpdateWaitingListAsync(WaitingList waitingList, int UserId);
        Task DeleteWaitingListAsync(int id, int UserId);
    }
}
