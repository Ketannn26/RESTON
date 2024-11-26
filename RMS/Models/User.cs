using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class User
    {
        [Key]
        public int U_Id { get; set; }

       
        [StringLength(40)]
        public required string FullName { get; set; }

        
        [EmailAddress]
        [StringLength(40)]
        public required string Email { get; set; }

       
        [StringLength(40)]
        public required string Password { get; set; }

        [Phone]
        [StringLength(40)]
        public required string PhoneNumber { get; set; }

        [StringLength(40)]
        public required string Role { get; set; } // Customer or Admin

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
