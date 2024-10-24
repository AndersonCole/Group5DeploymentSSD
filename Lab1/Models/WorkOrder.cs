using System.ComponentModel.DataAnnotations;

namespace Lab1.Models
{
    public class WorkOrder
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public string Customer { get; set; }
        [Required]
        [Display(Name = "Work Description")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime? Date { get; set; }
    }
}
