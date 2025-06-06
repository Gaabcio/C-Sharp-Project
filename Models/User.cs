using System.ComponentModel.DataAnnotations;

namespace ParkingManagementSystem.Models
{
    public class User
    {
        [Key] //głowny klucz tabeli
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty; 

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;


        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public virtual ICollection<ParkingReservation> ParkingReservations { get; set; } = new List<ParkingReservation>();
    }
}