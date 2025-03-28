using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Models.Entities
{
    public class Manager
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
        public string PersonalNumber { get; set; }

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        [Column(TypeName = "VARCHAR(50)")]
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]
        public string MobileNumber { get; set; }

        public int? UserId { get; set; }  
        [ForeignKey("UserId")]
        public User? User { get; set; }  
    }
}