namespace Server.DataAccess.Models
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        public string Name { get; set;}
        public string Surname { get; set;}
        public string Password { get; set;}
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ApplicationUserRole ApplicationUserRole { get; set; }

    }
}
