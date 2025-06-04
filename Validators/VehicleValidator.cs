using FluentValidation;
using ParkingManagementSystem.Models;
using ParkingManagementSystem.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingManagementSystem.Validators
{
    public class VehicleValidator : AbstractValidator<Vehicle>
    {
        private readonly IVehicleService _vehicleService;

        public VehicleValidator(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;

            RuleFor(v => v.LicensePlate)
                .NotEmpty().WithMessage("License plate is required")
                .Length(2, 20).WithMessage("License plate must be between 2 and 20 characters")
                .Matches(@"^[A-Z0-9\s]+$").WithMessage("License plate must contain only uppercase letters, numbers and spaces")
                .MustAsync(BeUniqueLicensePlate).WithMessage("License plate already exists");

            RuleFor(v => v.VehicleTypeId)
                .GreaterThan(0).WithMessage("Vehicle type must be selected");

            RuleFor(v => v.Brand)
                .MaximumLength(50).WithMessage("Brand cannot exceed 50 characters")
                .Matches(@"^[\p{L}0-9\s-]*$").WithMessage("Brand can only contain letters, numbers, spaces and hyphens")
                .When(v => !string.IsNullOrEmpty(v.Brand));

            RuleFor(v => v.Model)
                .MaximumLength(50).WithMessage("Model cannot exceed 50 characters")
                .Matches(@"^[\p{L}0-9\s-]*$").WithMessage("Model can only contain letters, numbers, spaces and hyphens")
                .When(v => !string.IsNullOrEmpty(v.Model));

            RuleFor(v => v.Year)
                .InclusiveBetween(1900, DateTime.Now.Year + 1).WithMessage($"Year must be between 1900 and {DateTime.Now.Year + 1}")
                .When(v => v.Year.HasValue);

            RuleFor(v => v.Color)
                .MaximumLength(20).WithMessage("Color cannot exceed 20 characters")
                .Matches(@"^[\p{L}\s]*$").WithMessage("Color can only contain letters and spaces")
                .When(v => !string.IsNullOrEmpty(v.Color));
        }

        private async Task<bool> BeUniqueLicensePlate(Vehicle vehicle, string licensePlate, CancellationToken token)
        {
            return await _vehicleService.IsLicensePlateUniqueAsync(licensePlate, vehicle.Id == 0 ? null : vehicle.Id);
        }
    }
}