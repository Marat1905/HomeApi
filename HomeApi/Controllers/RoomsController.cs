using AutoMapper;
using Azure.Core;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _repository;
        private IMapper _mapper;

        public RoomsController(IRoomRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //TODO: Задание - добавить метод на получение всех существующих комнат
        /// <summary>
        /// Просмотр списка всех комнат
        /// </summary>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _repository.GetRooms();

            var resp = new GetRoomsResponse
            {
                Rooms = _mapper.Map<Room[], RoomView[]>(rooms)
            };

            return StatusCode(200, resp);
        }


        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _repository.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _repository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }

            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateRoom([FromRoute] Guid id,
            [FromBody] EditRoomRequest request)
        {
            var room = await _repository.GetRoomById(id);
            if (room == null)
                return StatusCode(409, $"Ошибка: Комната c идентификатором {id}  не найдена");

            var withSameName = await _repository.GetRoomByName(request.NewName);
            if (withSameName != null)
                return StatusCode(400, $"Ошибка: Комната с именем {request.NewName} уже существует в нашем доме. Выберите другое имя!");


            return StatusCode(200);
        }
    }
}
