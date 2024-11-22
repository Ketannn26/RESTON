using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Models
{
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Mark as auto-generated
        public int MenuItemID { get; set; }

        public string ItemName { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        [Column(TypeName = "decimal(18,2)")] // Set column type
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public string ImageUrl { get; set; }
    }
}
