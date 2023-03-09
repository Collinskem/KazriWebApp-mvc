using System.ComponentModel.DataAnnotations.Schema;

namespace ZamaraWebApp.Models
{
    public class StaffViewModel
    {
        public int Id { get; set; }
        public string? StaffNumber { get; set; }
        public string? Name { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }
        public decimal? Salary { get; set; }
    }
}
