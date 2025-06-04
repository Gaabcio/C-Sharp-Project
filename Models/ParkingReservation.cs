using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingManagementSystem.Models
{
    public class ParkingReservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [ForeignKey("ParkingSpace")]
        public int ParkingSpaceId { get; set; }

        public DateTime StartTime { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Vehicle Vehicle { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ParkingSpace ParkingSpace { get; set; } = null!;
    }
}