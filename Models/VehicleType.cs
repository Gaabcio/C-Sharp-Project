using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ParkingManagementSystem.Models
{
    public class VehicleType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        public int SpacesRequired { get; set; } = 1;

        [StringLength(20)]
        public string? AllowedRows { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>(); // 
    }
}