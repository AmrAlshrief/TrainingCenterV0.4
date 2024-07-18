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
        Task<List<WaitingList>> GetAllWaitingListsAsync();
        Task<WaitingList> GetWaitingListByIdAsync(int id);
        Task CreateWaitingListAsync(WaitingList waitingList);
        Task UpdateWaitingListAsync(WaitingList waitingList);
        Task DeleteWaitingListAsync(int id);
    }
}
