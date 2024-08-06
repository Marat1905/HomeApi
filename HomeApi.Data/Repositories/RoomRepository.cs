using HomeApi.Data.Context;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using Microsoft.EntityFrameworkCore;

namespace HomeApi.Data.Repositories
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;

        public RoomRepository(HomeApiContext context)
        {
            _context = context;
        }

        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Найти устройство по идентификатору
        /// </summary>
        public async Task<Room?> GetRoomById(Guid id)
        {
            return await _context.Rooms.FirstOrDefaultAsync(x=>x.Id==id);
        }

        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);

            await _context.SaveChangesAsync();
        }

        public async Task<Room[]> GetRooms()
        {
            return await _context.Rooms.ToArrayAsync();
        }

        public async Task<Room?> Update(Room room)
        {
            Room? result = null;
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Modified)
              result= _context.Rooms.Update(room).Entity;

           await _context.SaveChangesAsync();

           return result;
        }

        ///// <summary>
        ///// Обновить существующее устройство
        ///// </summary>
        //public async Task UpdateDevice(Device device, Room room, UpdateDeviceQuery query)
        //{
        //    // Привязываем новое устройство к соответствующей комнате перед сохранением
        //    device.RoomId = room.Id;
        //    device.Room = room;

        //    // Если в запрос переданы параметры для обновления — проверяем их на null
        //    // И если нужно — обновляем устройство
        //    if (!string.IsNullOrEmpty(query.NewName))
        //        device.Name = query.NewName;
        //    if (!string.IsNullOrEmpty(query.NewSerial))
        //        device.SerialNumber = query.NewSerial;

        //    // Добавляем в базу
        //    var entry = _context.Entry(device);
        //    if (entry.State == EntityState.Detached)
        //        _context.Devices.Update(device);

        //    // Сохраняем изменения в базе
        //    await _context.SaveChangesAsync();
        //}
    }
}
