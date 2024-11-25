using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class User
    {
        [Key]
        public int U_Id { get; set; }

        [Required]
        [StringLength(40)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(40)]
        public string Email { get; set; }

        [Required]
        [StringLength(40)]
        public string Password { get; set; }

        [Required]
        [Phone]
        [StringLength(40)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(40)]
        public string Role { get; set; } // Customer or Admin

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
