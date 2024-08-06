using FluentValidation;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Contracts.Models.Rooms;

namespace HomeApi.Contracts.Validators
{

    /// <summary>
    /// Класс-валидатор запросов обновления комнаты
    /// </summary>
    public class EditRoomRequestValidator:AbstractValidator<EditRoomRequest>
    {
        public EditRoomRequestValidator()
        {
            RuleFor(x=>x.NewName).NotEmpty();
            RuleFor(x=>x.NewArea).NotEmpty();
            RuleFor(x=>x.NewVoltage).NotEmpty().ExclusiveBetween(120,260);
            RuleFor(x=>x.NewGasConnected).NotNull();
        }
    }
}
