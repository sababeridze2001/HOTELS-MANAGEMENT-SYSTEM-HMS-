using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models.Entities
{
    public class Hotel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required] 
        public int ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public Manager? Manager { get; set; } 

        [MinLength(1, ErrorMessage = "At least one room is required.")] 
        public ICollection<Room> Rooms { get; set; } = new List<Room>(); 
    }
}