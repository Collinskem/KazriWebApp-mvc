namespace ZamaraWebApp.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string? StaffNumber { get; set; }
        public string? Name { get; set; }
        public IFormFile? Photo { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }
        public decimal? Salary { get; set; }
    }
}
