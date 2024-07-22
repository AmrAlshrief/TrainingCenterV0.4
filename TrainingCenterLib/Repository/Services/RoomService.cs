using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;
using TrainingCenterLib.Repository.Interfaces;

namespace TrainingCenterLib.Repository.Services
{
    public class RoomService : IRoomService
    {

        public async Task<List<Room>> GetAllRoomsAsync()
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                try
                {
                    return await context.Rooms.ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public Task<Room> GetRoomByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task CreateRoomAsync(Room room, int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        context.Rooms.Add(room);
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Add, Action.AddRoom, UserId, MasterEntity.Room, "Room Added");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public async Task UpdateRoomAsync(Room room, int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(room).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Update, Action.UpdateRoom, UserId, MasterEntity.Room, "Room Info Updated");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }
        

        public async Task DeleteRoomAsync(int id, int UserId)
        {
            using (var context = new TrainingCenterLibDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var Room = await context.Rooms.FindAsync(id);
                        if (Room == null)
                        {
                            throw new KeyNotFoundException("Room not found.");
                        }
                        context.Rooms.Remove(Room);

                        await context.SaveChangesAsync();
                        UserInfo.CreateAudit(ActionType.Delete, Action.DeleteRoom, UserId, MasterEntity.Room, "Room Deleted");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

       
    }
}
