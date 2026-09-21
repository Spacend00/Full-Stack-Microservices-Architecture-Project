using FluentValidation;

namespace UserService.Application.Features.Users.Commands.UpdatePassword;

public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 6 characters long.")
            .Matches("[A-Z]").WithMessage("Password must have one large char.")
            .Matches("[a-z]").WithMessage("Password must have one small char.")
            .Matches("[0-9]").WithMessage("Password must have one number.");
    }
}
