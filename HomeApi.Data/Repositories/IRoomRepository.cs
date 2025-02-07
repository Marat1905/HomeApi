﻿using HomeApi.Data.Models;
using HomeApi.Data.Queries;

namespace HomeApi.Data.Repositories
{
    /// <summary>
    /// Интерфейс определяет методы для доступа к объектам типа Room в базе 
    /// </summary>
    public interface IRoomRepository
    {
        Task<Room> GetRoomByName(string name);
        Task AddRoom(Room room);

        Task<Room[]> GetRooms();

        public Task<Room?> GetRoomById(Guid id);

        public Task<Room?> Update(Room room, UpdateRoomQuery query);
    }
}
