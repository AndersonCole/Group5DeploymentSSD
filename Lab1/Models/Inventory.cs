using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lab1.Models
{
    public class Inventory
    {
        [Key]
        [Display(Name="Inventory ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name="Item Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name="Quantity In Stock")]
        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name="Date Last Ordered")]
        public DateTime? LastOrderDate { get; set; }
    }
}
