using FluentValidation;

namespace SMJRegisterAPI.Features.Room.Command;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x=>x.Room.Capacity)
            .GreaterThan(0)
            .WithMessage("Ingrese una Capacidad");
        
        RuleFor(x=>x.Room.Name)
            .NotEmpty().WithMessage("Ingrese un Nombre")
            .NotNull().WithMessage("Ingrese un Nombre");
    }
}