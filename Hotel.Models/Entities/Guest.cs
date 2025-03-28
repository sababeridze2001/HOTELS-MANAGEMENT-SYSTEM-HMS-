using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Models.Entities
{
    public class Guest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Required]
        [StringLength(11)]
        [Column(TypeName = "CHAR(11)")]
        public string PersonalNumber { get; set; }

        [Required]
        [MaxLength(15)]
        public string MobileNumber { get; set; }

        
        public int? UserId { get; set; }

        
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public ICollection<GuestReservation> GuestReservations { get; set; }
    }
}