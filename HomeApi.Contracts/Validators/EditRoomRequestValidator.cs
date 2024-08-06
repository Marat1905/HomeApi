using FluentValidation;
using HomeApi.Contracts.Models.Devices;

namespace HomeApi.Contracts.Validators
{

    /// <summary>
    /// Класс-валидатор запросов обновления комнаты
    /// </summary>
    public class EditRoomRequestValidator:AbstractValidator<EditDeviceRequest>
    {
        public EditRoomRequestValidator()
        {
            
        }
    }
}
