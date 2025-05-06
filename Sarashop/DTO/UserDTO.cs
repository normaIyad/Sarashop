namespace Sarashop.DTO
{
    public class UserDTO
    {
        public string? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
