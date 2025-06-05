using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingManagementSystem.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-Z0-9\s]+$", ErrorMessage = "License plate must contain only uppercase letters, numbers and spaces")]
        public string LicensePlate { get; set; } = string.Empty;

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [ForeignKey("VehicleType")]
        public int VehicleTypeId { get; set; }

        [StringLength(50)]
        public string? Brand { get; set; }

        [StringLength(50)]
        public string? Model { get; set; }

        [StringLength(20)]
        public string? Color { get; set; }

        public int? Year { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public virtual User User { get; set; } = null!;  // Właściwość nawigacyjna do właściciela pojazdu
        public virtual VehicleType VehicleType { get; set; } = null!;  // Właściwość nawigacyjna do typu pojazdu
        public virtual ICollection<ParkingReservation> ParkingReservations { get; set; } = new List<ParkingReservation>();  // Kolekcja rezerwacji parkingowych powiązanych z pojazdem
    }
}