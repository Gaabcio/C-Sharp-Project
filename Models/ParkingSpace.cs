using System.ComponentModel.DataAnnotations;

namespace ParkingManagementSystem.Models
{
    public class ParkingSpace
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Row { get; set; }

        [Required]
        public int Column { get; set; }

        public bool IsOccupied { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<ParkingReservation> ParkingReservations { get; set; } = new List<ParkingReservation>();
    }
}