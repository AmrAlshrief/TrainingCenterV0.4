using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;

namespace TrainingCenterLib.Repository.Interfaces
{
    public interface IRoomService
    {
        Task<List<Room>> GetAllRoomsAsync();
        Task<Room> GetRoomByIdAsync(int id);
        Task CreateRoomAsync(Room room, int UserId);
        Task UpdateRoomAsync(Room room, int UserId);
        Task DeleteRoomAsync(int id, int UserId);
    }
}
