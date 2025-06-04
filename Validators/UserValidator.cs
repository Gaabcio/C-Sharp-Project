using FluentValidation;
using ParkingManagementSystem.Models;
using ParkingManagementSystem.Services;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingManagementSystem.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        private readonly IUserService _userService;

        public UserValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Username is required")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters")
                .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers and underscores")
                .MustAsync(BeUniqueUsername).WithMessage("Username already exists");

            RuleFor(u => u.Email)
                .EmailAddress().WithMessage("Invalid email format")
                .MustAsync(BeUniqueEmail).WithMessage("Email already exists")
                .When(u => !string.IsNullOrEmpty(u.Email));

            RuleFor(u => u.FirstName)
                .Matches(@"^[\p{L}\s]*$").WithMessage("First name can only contain letters and spaces")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters")
                .When(u => !string.IsNullOrEmpty(u.FirstName));

            RuleFor(u => u.LastName)
                .Matches(@"^[\p{L}\s]*$").WithMessage("Last name can only contain letters and spaces")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters")
                .When(u => !string.IsNullOrEmpty(u.LastName));
        }

        private async Task<bool> BeUniqueUsername(User user, string username, CancellationToken token)
        {
            return await _userService.IsUsernameUniqueAsync(username, user.Id == 0 ? null : user.Id);
        }

        private async Task<bool> BeUniqueEmail(User user, string email, CancellationToken token)
        {
            return await _userService.IsEmailUniqueAsync(email, user.Id == 0 ? null : user.Id);
        }
    }
}