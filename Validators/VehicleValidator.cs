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
                .NotEmpty().WithMessage("Numer rejestracyjny jest wymagany")
                .Length(2, 20).WithMessage("Numer rejestracyjny musi mieæ od 2 do 20 znaków")
                .Matches(@"^[A-Z0-9\s]+$").WithMessage("Numer rejestracyjny mo¿e zawieraæ tylko wielkie litery, cyfry i spacje")
                .MustAsync(BeUniqueLicensePlate).WithMessage("Podany numer rejestracyjny ju¿ istnieje");

            RuleFor(v => v.VehicleTypeId)
                .GreaterThan(0).WithMessage("Typ pojazdu musi zostaæ wybrany");

            RuleFor(v => v.Brand)
                .MaximumLength(50).WithMessage("Marka nie mo¿e przekraczaæ 50 znaków")
                .Matches(@"^[\p{L}0-9\s-]*$").WithMessage("Marka mo¿e zawieraæ tylko litery, cyfry, spacje i myœlniki")
                .When(v => !string.IsNullOrEmpty(v.Brand));

            RuleFor(v => v.Model)
                .MaximumLength(50).WithMessage("Model nie mo¿e przekraczaæ 50 znaków")
                .Matches(@"^[\p{L}0-9\s-]*$").WithMessage("Model mo¿e zawieraæ tylko litery, cyfry, spacje i myœlniki")
                .When(v => !string.IsNullOrEmpty(v.Model));

            RuleFor(v => v.Year)
                .InclusiveBetween(1900, DateTime.Now.Year + 1)
                .WithMessage($"Rok musi byæ z zakresu od 1900 do {DateTime.Now.Year + 1}")
                .When(v => v.Year.HasValue);

            RuleFor(v => v.Color)
                .MaximumLength(20).WithMessage("Kolor nie mo¿e przekraczaæ 20 znaków")
                .Matches(@"^[\p{L}\s]*$").WithMessage("Kolor mo¿e zawieraæ tylko litery i spacje")
                .When(v => !string.IsNullOrEmpty(v.Color));
        }

        private async Task<bool> BeUniqueLicensePlate(Vehicle vehicle, string licensePlate, CancellationToken token)
        {
            return await _vehicleService.IsLicensePlateUniqueAsync(licensePlate, vehicle.Id == 0 ? null : vehicle.Id);
        }
    }
}